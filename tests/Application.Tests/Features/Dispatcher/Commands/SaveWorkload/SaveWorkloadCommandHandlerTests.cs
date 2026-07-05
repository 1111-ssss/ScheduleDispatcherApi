using Application.Abstractions.Repository.Base;
using Application.Features.Dispatcher.Common;
using Application.Features.Dispatcher.SaveWorkload;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Model.Result;
using Domain.Specifications.Dispatcher;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Features.Dispatcher.Commands.SaveWorkload;

public class SaveWorkloadCommandHandlerTests
{
    private readonly Mock<ILogger<SaveWorkloadCommandHandler>> _loggerMock;
    private readonly Mock<IBaseRepository<Employer>> _employerRepositoryMock;
    private readonly Mock<IBaseRepository<Schedule>> _scheduleRepositoryMock;
    private readonly Mock<IBaseRepository<SubjectTeacherSchedule>> _stsRepositoryMock;
    private readonly Mock<IBaseRepository<Removal>> _removalRepositoryMock;
    private readonly Mock<IBaseRepository<Subject>> _subjectRepositoryMock;
    private readonly Mock<IBaseRepository<Group>> _groupRepositoryMock;
    private readonly Mock<IBaseRepository<SubjectTeacher>> _linkRepositoryMock;
    private readonly Mock<IBaseRepository<SubjectTeacherSemester>> _semesterRepositoryMock;
    private readonly SaveWorkloadCommandHandler _handler;

    public SaveWorkloadCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<SaveWorkloadCommandHandler>>();
        _employerRepositoryMock = new Mock<IBaseRepository<Employer>>();
        _scheduleRepositoryMock = new Mock<IBaseRepository<Schedule>>();
        _stsRepositoryMock = new Mock<IBaseRepository<SubjectTeacherSchedule>>();
        _removalRepositoryMock = new Mock<IBaseRepository<Removal>>();
        _subjectRepositoryMock = new Mock<IBaseRepository<Subject>>();
        _groupRepositoryMock = new Mock<IBaseRepository<Group>>();
        _linkRepositoryMock = new Mock<IBaseRepository<SubjectTeacher>>();
        _semesterRepositoryMock = new Mock<IBaseRepository<SubjectTeacherSemester>>();

        _removalRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Removal>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Removal r, CancellationToken ct) => r);

        _handler = new SaveWorkloadCommandHandler(
            _loggerMock.Object,
            _employerRepositoryMock.Object,
            _scheduleRepositoryMock.Object,
            _stsRepositoryMock.Object,
            _removalRepositoryMock.Object,
            _subjectRepositoryMock.Object,
            _groupRepositoryMock.Object,
            _linkRepositoryMock.Object,
            _semesterRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenValidDataProvided()
    {
        
        var employer = CreateEmployer();
        _employerRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetWorkloadSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employer);

        _scheduleRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Schedule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Schedule s, CancellationToken ct) =>
            {
                s.IdSchedule = 1;
                return s;
            });

        _stsRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<SubjectTeacherSchedule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SubjectTeacherSchedule sts, CancellationToken ct) => sts);

        var workloadSummary = new WorkloadSummaryDTO(
            new List<WorkloadDTO>
            {
                new("Базы данных", 1, false, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), 1)
            },
            new List<RemovalDTO>()
        );

        var command = new SaveWorkloadCommand("Базы данных", "Иванов И. И.", "П21", 1, workloadSummary);

var result = await _handler.Handle(command, CancellationToken.None);

result.IsSuccess.Should().BeTrue();
        _scheduleRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Schedule>(), It.IsAny<CancellationToken>()), Times.Once);
        _stsRepositoryMock.Verify(r => r.AddAsync(It.IsAny<SubjectTeacherSchedule>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenTeacherNotFound()
    {
        
        _employerRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetWorkloadSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employer?)null);

        var workloadSummary = new WorkloadSummaryDTO(
            new List<WorkloadDTO>
            {
                new("Базы данных", 1, false, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), 1)
            },
            new List<RemovalDTO>()
        );

        var command = new SaveWorkloadCommand("Базы данных", "Неexistent Teacher", "П21", 1, workloadSummary);

var result = await _handler.Handle(command, CancellationToken.None);

result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Преподаватель не найден");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenLessonNotFound()
    {
        
        var employer = CreateEmployer();
        _employerRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetWorkloadSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employer);

_subjectRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<SubjectByNameSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Subject?)null);

        _employerRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<EmployerByFioSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employer);

        var workloadSummary = new WorkloadSummaryDTO(
            new List<WorkloadDTO>
            {
                new("Несуществующая дисциплина", 1, false, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), 1)
            },
            new List<RemovalDTO>()
        );

        var command = new SaveWorkloadCommand("Несуществующая дисциплина", "Иванов И. И.", "П21", 1, workloadSummary);

var result = await _handler.Handle(command, CancellationToken.None);

result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Дисциплина не найдена.");
    }

    [Fact]
    public async Task Handle_ShouldDeleteExistingSchedules_WhenSavingNewWorkload()
    {
        
        var employer = CreateEmployer();
        _employerRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetWorkloadSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employer);

        _scheduleRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Schedule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Schedule s, CancellationToken ct) =>
            {
                s.IdSchedule = 1;
                return s;
            });

        _stsRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<SubjectTeacherSchedule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SubjectTeacherSchedule sts, CancellationToken ct) => sts);

        var workloadSummary = new WorkloadSummaryDTO(
            new List<WorkloadDTO>
            {
                new("Базы данных", 1, false, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), 1)
            },
            new List<RemovalDTO>()
        );

        var command = new SaveWorkloadCommand("Базы данных", "Иванов И. И.", "П21", 1, workloadSummary);

var result = await _handler.Handle(command, CancellationToken.None);

result.IsSuccess.Should().BeTrue();
        _stsRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<SubjectTeacherSchedule>(), It.IsAny<CancellationToken>()), Times.Once);
        _scheduleRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Schedule>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCreateMultipleSchedules_WhenMultipleWorkloadItemsProvided()
    {
        
        var employer = CreateEmployer();
        _employerRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetWorkloadSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employer);

        var scheduleId = 1;
        _scheduleRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Schedule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Schedule s, CancellationToken ct) =>
            {
                s.IdSchedule = scheduleId++;
                return s;
            });

        _stsRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<SubjectTeacherSchedule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SubjectTeacherSchedule sts, CancellationToken ct) => sts);

        var workloadSummary = new WorkloadSummaryDTO(
            new List<WorkloadDTO>
            {
                new("Базы данных", 1, false, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), 1),
                new("Базы данных", 2, false, DateTime.Today.AddDays(3), DateTime.Today.AddDays(4), 2),
                new("Базы данных", 3, false, DateTime.Today.AddDays(5), DateTime.Today.AddDays(6), 3)
            },
            new List<RemovalDTO>()
        );

        var command = new SaveWorkloadCommand("Базы данных", "Иванов И. И.", "П21", 1, workloadSummary);

var result = await _handler.Handle(command, CancellationToken.None);

result.IsSuccess.Should().BeTrue();
        _scheduleRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Schedule>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
        _stsRepositoryMock.Verify(r => r.AddAsync(It.IsAny<SubjectTeacherSchedule>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
    }

    [Fact]
    public async Task Handle_ShouldHandleSplitLesson_WhenIsLessonSplitIsTrue()
    {
        
        var employer = CreateEmployer();
        _employerRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetWorkloadSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employer);

        Schedule? createdSchedule = null;
        _scheduleRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Schedule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Schedule s, CancellationToken ct) =>
            {
                s.IdSchedule = 1;
                createdSchedule = s;
                return s;
            });

        _stsRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<SubjectTeacherSchedule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SubjectTeacherSchedule sts, CancellationToken ct) => sts);

        var workloadSummary = new WorkloadSummaryDTO(
            new List<WorkloadDTO>
            {
                new("Базы данных", 1, true, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), 1)
            },
            new List<RemovalDTO>()
        );

        var command = new SaveWorkloadCommand("Базы данных", "Иванов И. И.", "П21", 1, workloadSummary);

var result = await _handler.Handle(command, CancellationToken.None);

result.IsSuccess.Should().BeTrue();
        createdSchedule.Should().NotBeNull();
        createdSchedule!.IsPractical.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldPersistRemoval_ByLessonIndex()
    {

var employer = CreateEmployer();
        _employerRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetWorkloadSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employer);

        var nextId = 10;
        var created = new List<Schedule>();
        _scheduleRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Schedule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Schedule s, CancellationToken ct) =>
            {
                s.IdSchedule = nextId++;
                created.Add(s);
                return s;
            });

        _stsRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<SubjectTeacherSchedule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SubjectTeacherSchedule sts, CancellationToken ct) => sts);

        Removal? savedRemoval = null;
        _removalRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Removal>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Removal r, CancellationToken ct) =>
            {
                savedRemoval = r;
                return r;
            });

        var workloadSummary = new WorkloadSummaryDTO(
            new List<WorkloadDTO>
            {
                new("Базы данных", 3, false, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), 0),
                new("Базы данных", 4, false, DateTime.Today.AddDays(3), DateTime.Today.AddDays(4), 0)
            },
            new List<RemovalDTO> { new(1, 3, 4) }
        );

        var command = new SaveWorkloadCommand("Базы данных", "Иванов И. И.", "П21", 1, workloadSummary);

var result = await _handler.Handle(command, CancellationToken.None);

result.IsSuccess.Should().BeTrue();
        savedRemoval.Should().NotBeNull();
        savedRemoval!.IdSchedule1.Should().Be(created[0].IdSchedule);
        savedRemoval.IdSchedule2.Should().Be(created[1].IdSchedule);
        _removalRepositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Removal>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldAcceptNullDates_ForUndatedLessons()
    {
        
        var employer = CreateEmployer();
        _employerRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetWorkloadSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employer);

        Schedule? createdSchedule = null;
        _scheduleRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Schedule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Schedule s, CancellationToken ct) =>
            {
                s.IdSchedule = 1;
                createdSchedule = s;
                return s;
            });

        _stsRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<SubjectTeacherSchedule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SubjectTeacherSchedule sts, CancellationToken ct) => sts);

        var workloadSummary = new WorkloadSummaryDTO(
            new List<WorkloadDTO>
            {
                new("Базы данных", 1, false, null, null, 0)
            },
            new List<RemovalDTO>()
        );

        var command = new SaveWorkloadCommand("Базы данных", "Иванов И. И.", "П21", 1, workloadSummary);

var result = await _handler.Handle(command, CancellationToken.None);

result.IsSuccess.Should().BeTrue();
        createdSchedule.Should().NotBeNull();
        createdSchedule!.Date1.Should().BeNull();
        createdSchedule.Date2.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldPersistLessonType_OnSchedule()
    {
        
        var employer = CreateEmployer();
        _employerRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetWorkloadSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employer);

        Schedule? createdSchedule = null;
        _scheduleRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Schedule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Schedule s, CancellationToken ct) =>
            {
                s.IdSchedule = 1;
                createdSchedule = s;
                return s;
            });

        _stsRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<SubjectTeacherSchedule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SubjectTeacherSchedule sts, CancellationToken ct) => sts);

        var workloadSummary = new WorkloadSummaryDTO(
            new List<WorkloadDTO>
            {
                new("Базы данных", 1, false, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), 0, "Лабораторная")
            },
            new List<RemovalDTO>()
        );

        var command = new SaveWorkloadCommand("Базы данных", "Иванов И. И.", "П21", 1, workloadSummary);

var result = await _handler.Handle(command, CancellationToken.None);

result.IsSuccess.Should().BeTrue();
        createdSchedule.Should().NotBeNull();
        createdSchedule!.LessonType.Should().Be("Лабораторная");
    }

    [Fact]
    public async Task Handle_ShouldReturnDatabaseError_WhenExceptionOccurs()
    {
        
        var employer = CreateEmployer();
        _employerRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetWorkloadSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employer);

        _scheduleRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Schedule>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        var workloadSummary = new WorkloadSummaryDTO(
            new List<WorkloadDTO>
            {
                new("Базы данных", 1, false, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), 1)
            },
            new List<RemovalDTO>()
        );

        var command = new SaveWorkloadCommand("Базы данных", "Иванов И. И.", "П21", 1, workloadSummary);

var result = await _handler.Handle(command, CancellationToken.None);

result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Ошибка БД");
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Ошибка при сохранении нагрузки")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCreateLink_WhenReplacementTeacherHasNoBinding()
    {

var employer = CreateEmployerWithoutLink();
        _employerRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetWorkloadSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employer);

        _employerRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<EmployerByFioSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employer);

        _subjectRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<SubjectByNameSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Subject { IdSubject = 5, Name = "Базы данных", CnSpec = "09.02.07" });

        _groupRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<GroupByCnGSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Group { CnG = "П21", Name = "П21", Cours = 2, CnSpec = "09.02.07" });

        _linkRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<SubjectTeacherLinkSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SubjectTeacher?)null);

        _linkRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<SubjectTeacher>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SubjectTeacher st, CancellationToken ct) =>
            {
                st.IdsubjectTeacher = 7;
                return st;
            });

        _semesterRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<SubjectTeacherSemester>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SubjectTeacherSemester s, CancellationToken ct) => s);

        _scheduleRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Schedule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Schedule s, CancellationToken ct) =>
            {
                s.IdSchedule = 1;
                return s;
            });

        _stsRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<SubjectTeacherSchedule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SubjectTeacherSchedule sts, CancellationToken ct) => sts);

        var workloadSummary = new WorkloadSummaryDTO(
            new List<WorkloadDTO>
            {
                new("Базы данных", 1, false, DateTime.Today, null, 0)
            },
            new List<RemovalDTO>()
        );

        var command = new SaveWorkloadCommand("Базы данных", "Иванов И. И.", "П21", 1, workloadSummary);

var result = await _handler.Handle(command, CancellationToken.None);

result.IsSuccess.Should().BeTrue();
        _linkRepositoryMock.Verify(
            r => r.AddAsync(It.Is<SubjectTeacher>(st => st.IdSubject == 5 && st.CnG == "П21" && st.CnT == "T001"),
                It.IsAny<CancellationToken>()),
            Times.Once);
        _semesterRepositoryMock.Verify(
            r => r.AddAsync(It.Is<SubjectTeacherSemester>(s => s.IdsubjectTeacher == 7 && s.Semester1 == true),
                It.IsAny<CancellationToken>()),
            Times.Once);
        _scheduleRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Schedule>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    private static Employer CreateEmployerWithoutLink()
    {
        var employer = new Employer
        {
            CnE = "E001",
            Surname = "Иванов",
            Name = "Иван",
            FatherName = "Иванович"
        };

        var teacher = new Teacher
        {
            CnT = "T001",
            CnE = "E001",
            CnENavigation = employer,
            SubjectTeachers = new List<SubjectTeacher>()
        };

        employer.Teachers = new List<Teacher> { teacher };

        return employer;
    }

    private static Employer CreateEmployer()
    {
        var employer = new Employer
        {
            CnE = "E001",
            Surname = "Иванов",
            Name = "Иван",
            FatherName = "Иванович"
        };

        var teacher = new Teacher
        {
            CnT = "T001",
            CnE = "E001",
            CnENavigation = employer
        };

        var group = new Group
        {
            CnG = "П21",
            Name = "П21",
            Cours = 2,
            CnSpec = "09.02.07"
        };

        var subject = new Subject
        {
            Name = "Базы данных",
            Fullname = "Базы данных и СУБД",
            CnSpec = "09.02.07",
            Totalhourcount = 100,
            Practichourcount = 40,
            PcClassNeed = true,
            Optional = false
        };

        var subjectTeacher = new SubjectTeacher
        {
            IdsubjectTeacher = 1,
            IdSubject = subject.IdSubject,
            CnG = group.CnG,
            CnT = teacher.CnT,
            IdSubjectNavigation = subject,
            CnGNavigation = group,
            CnTNavigation = teacher,
            SubjectTeacherSchedules = new List<SubjectTeacherSchedule>
            {
                new()
                {
                    IdsubjectTeacher = 1,
                    IdSchedule = 1,
                    GroupSplit = false,
                    IdScheduleNavigation = new Schedule
                    {
                        IdSchedule = 1,
                        Lessonnumber = 1,
                        Date1 = DateTime.Today,
                        IsPractical = false,
                        IsOver = false
                    }
                }
            }
        };

        teacher.SubjectTeachers = new List<SubjectTeacher> { subjectTeacher };
        employer.Teachers = new List<Teacher> { teacher };

        return employer;
    }
}
