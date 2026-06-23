using Application.Abstractions.Repository.Base;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Model.Result;
using Domain.Specifications.Dispatcher;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Dispatcher.SaveWorkload;

public class SaveWorkloadCommandHandler : IRequestHandler<SaveWorkloadCommand, Result>
    {
    private readonly ILogger<SaveWorkloadCommandHandler> _logger;
    private readonly IBaseRepository<Employer> _employerRepository;
    private readonly IBaseRepository<Schedule> _scheduleRepository;
    private readonly IBaseRepository<SubjectTeacherSchedule> _stsRepository;
    private readonly IBaseRepository<Removal> _removalRepository;
    private readonly IBaseRepository<Subject> _subjectRepository;
    private readonly IBaseRepository<Group> _groupRepository;
    private readonly IBaseRepository<SubjectTeacher> _linkRepository;
    private readonly IBaseRepository<SubjectTeacherSemester> _semesterRepository;

    public SaveWorkloadCommandHandler(
        ILogger<SaveWorkloadCommandHandler> logger,
        IBaseRepository<Employer> employerRepository,
        IBaseRepository<Schedule> scheduleRepository,
        IBaseRepository<SubjectTeacherSchedule> stsRepository,
        IBaseRepository<Removal> removalRepository,
        IBaseRepository<Subject> subjectRepository,
        IBaseRepository<Group> groupRepository,
        IBaseRepository<SubjectTeacher> linkRepository,
        IBaseRepository<SubjectTeacherSemester> semesterRepository
    )
    {
        _logger = logger;
        _employerRepository = employerRepository;
        _scheduleRepository = scheduleRepository;
        _stsRepository = stsRepository;
        _removalRepository = removalRepository;
        _subjectRepository = subjectRepository;
        _groupRepository = groupRepository;
        _linkRepository = linkRepository;
        _semesterRepository = semesterRepository;
    }

    public async Task<Result> Handle(SaveWorkloadCommand command, CancellationToken ct)
    {
        var employer = await _employerRepository.FirstOrDefaultAsync(new GetWorkloadSpec(command.Teacher, command.Group, command.Semester, command.Lesson), ct);

        var subjectTeacher = employer?.Teachers
            .SelectMany(t => t.SubjectTeachers)
            .FirstOrDefault(st => st.CnGNavigation.Name == command.Group
            && st.IdSubjectNavigation.Name == command.Lesson);

        if (subjectTeacher == null)
        {
            employer = await _employerRepository.FirstOrDefaultAsync(new EmployerByFioSpec(command.Teacher), ct);
        }

        if (employer == null)
            return Result.Failed(ErrorCode.TeacherNotFound, "Преподаватель не найден");

        if (subjectTeacher == null)
        {
            subjectTeacher = await EnsureLinkAsync(employer, command, ct);
            if (subjectTeacher == null)
            {
                return Result.Failed(ErrorCode.InvalidLessonName, "Дисциплина не найдена.");
            }
        }

        var existingSchedules = subjectTeacher.SubjectTeacherSchedules.ToList();

        try
        {
            var oldScheduleIds = existingSchedules.Select(s => s.IdSchedule).ToList();

            var removalsToDelete = existingSchedules
                .SelectMany(s => new[] { s.IdScheduleNavigation.RemovalAsFirst, s.IdScheduleNavigation.RemovalAsSecond })
                .Where(r => r != null)
                .Distinct()
                .ToList();

            foreach (var removal in removalsToDelete)
            {
                await _removalRepository.DeleteAsync(removal, ct);
            }

            foreach (var oldSts in existingSchedules)
            {
                await _stsRepository.DeleteAsync(oldSts, ct);
                await _scheduleRepository.DeleteAsync(oldSts.IdScheduleNavigation, ct);
            }

            var createdSchedules = new Dictionary<int, Schedule>();

            for (int i = 0; i < command.WorkloadSummary.WorkloadList.Count; i++)
            {
                var item = command.WorkloadSummary.WorkloadList[i];

                var newSchedule = new Schedule
                {
                    Lessonnumber = item.LessonIndex,
                    Date1 = item.LessonDate1,
                    Date2 = item.LessonDate2,
                    IsPractical = item.IsLessonSplit,
                    IsOver = false,
                    LessonType = item.LessonType ?? string.Empty
                };

                await _scheduleRepository.AddAsync(newSchedule, ct);

                createdSchedules[item.LessonIndex] = newSchedule;

                var newSts = new SubjectTeacherSchedule
                {
                    IdsubjectTeacher = subjectTeacher.IdsubjectTeacher,
                    IdSchedule = newSchedule.IdSchedule,
                    GroupSplit = item.IsLessonSplit
                };
                await _stsRepository.AddAsync(newSts, ct);
            }

            foreach (var removalDto in command.WorkloadSummary.RemovalList)
            {
                if (createdSchedules.TryGetValue(removalDto.FirstLessonIndex, out var schedule1)
                    && createdSchedules.TryGetValue(removalDto.SecondLessonIndex, out var schedule2))
                {
                    await _removalRepository.AddAsync(new Removal
                    {
                        IdSchedule1 = schedule1.IdSchedule,
                        IdSchedule2 = schedule2.IdSchedule
                    }, ct);
                }
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при сохранении нагрузки и переносов");
            return Result.Failed(ErrorCode.DatabaseError, "Ошибка БД");
        }
    }

        private async Task<SubjectTeacher?> EnsureLinkAsync(
        Employer employer, SaveWorkloadCommand command, CancellationToken ct)
    {
        var teacherRow = employer.Teachers.FirstOrDefault();
        if (teacherRow == null)
        {
            return null;
        }

        var subject = await _subjectRepository.FirstOrDefaultAsync(
            new SubjectByNameSpec(command.Lesson), ct);
        if (subject == null)
        {
            return null;
        }

        var group = await _groupRepository.FirstOrDefaultAsync(
            new GroupByCnGSpec(command.Group), ct);
        if (group == null)
        {
            return null;
        }

        var existing = await _linkRepository.FirstOrDefaultAsync(
            new SubjectTeacherLinkSpec(teacherRow.CnT, group.CnG, subject.IdSubject), ct);
        if (existing != null)
        {
            await EnsureSemesterAsync(existing, command.Semester, ct);
            return existing;
        }

        var link = new SubjectTeacher
        {
            IdSubject = subject.IdSubject,
            CnG = group.CnG,
            CnT = teacherRow.CnT
        };
        await _linkRepository.AddAsync(link, ct);
        await _semesterRepository.AddAsync(new SubjectTeacherSemester
        {
            IdsubjectTeacher = link.IdsubjectTeacher,
            Semester1 = command.Semester == 1,
            Semester2 = command.Semester == 2
        }, ct);

        _logger.LogInformation(
            "Создана связка замены: {Teacher} — {Lesson} — {Group} — семестр {Semester}",
            command.Teacher, command.Lesson, command.Group, command.Semester);

        return link;
    }

        private async Task EnsureSemesterAsync(SubjectTeacher link, int semester, CancellationToken ct)
    {
        var has = link.Subjectteachersemesters.Any(s =>
            (semester == 1 && s.Semester1 == true)
            || (semester == 2 && s.Semester2 == true));
        if (!has)
        {
            await _semesterRepository.AddAsync(new SubjectTeacherSemester
            {
                IdsubjectTeacher = link.IdsubjectTeacher,
                Semester1 = semester == 1,
                Semester2 = semester == 2
            }, ct);
        }
    }
    }
