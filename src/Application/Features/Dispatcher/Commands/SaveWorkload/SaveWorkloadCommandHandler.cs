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

    public SaveWorkloadCommandHandler(
        ILogger<SaveWorkloadCommandHandler> logger,
        IBaseRepository<Employer> employerRepository,
        IBaseRepository<Schedule> scheduleRepository,
        IBaseRepository<SubjectTeacherSchedule> stsRepository
    )
    {
        _logger = logger;
        _employerRepository = employerRepository;
        _scheduleRepository = scheduleRepository;
        _stsRepository = stsRepository;
    }

    public async Task<Result> Handle(SaveWorkloadCommand command, CancellationToken ct)
    {
        var employer = await _employerRepository.FirstOrDefaultAsync(new GetWorkloadSpec(command.Teacher, command.Group, command.Semester, command.Lesson), ct);

        if (employer == null)
            return Result.Failed(ErrorCode.TeacherNotFound, "Преподаватель не найден");

        var subjectTeacher = employer.Teachers
            .SelectMany(t => t.SubjectTeachers)
            .FirstOrDefault(st => st.CnGNavigation.Name == command.Group
            && st.IdSubjectNavigation.Name == command.Lesson);

        if (subjectTeacher == null)
        {
            return Result.Failed(ErrorCode.InvalidLessonName, "Дисциплина не найдена.");
        }

        var existingSchedules = subjectTeacher.SubjectTeacherSchedules.ToList();

        try
        {
            var oldScheduleIds = existingSchedules.Select(s => s.IdSchedule).ToList();

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
                };

                await _scheduleRepository.AddAsync(newSchedule, ct);

                createdSchedules.Add(i, newSchedule);

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
                    var newRemoval = new Removal
                    {
                        IdSchedule1 = schedule1.IdSchedule,
                        IdSchedule2 = schedule2.IdSchedule
                    };
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
}