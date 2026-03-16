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
    private readonly SaveWorkloadCommandHandler _handler;

    public SaveWorkloadCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<SaveWorkloadCommandHandler>>();
        _employerRepositoryMock = new Mock<IBaseRepository<Employer>>();
        _scheduleRepositoryMock = new Mock<IBaseRepository<Schedule>>();
        _stsRepositoryMock = new Mock<IBaseRepository<SubjectTeacherSchedule>>();

        _handler = new SaveWorkloadCommandHandler(
            _loggerMock.Object,
            _employerRepositoryMock.Object,
            _scheduleRepositoryMock.Object,
            _stsRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenValidDataProvided()
    {
        // Arrange
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

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _scheduleRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Schedule>(), It.IsAny<CancellationToken>()), Times.Once);
        _stsRepositoryMock.Verify(r => r.AddAsync(It.IsAny<SubjectTeacherSchedule>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenTeacherNotFound()
    {
        // Arrange
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

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Преподаватель не найден");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenLessonNotFound()
    {
        // Arrange
        var employer = CreateEmployer();
        _employerRepositoryMock
            .Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetWorkloadSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employer);

        var workloadSummary = new WorkloadSummaryDTO(
            new List<WorkloadDTO>
            {
                new("Несуществующая дисциплина", 1, false, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), 1)
            },
            new List<RemovalDTO>()
        );

        var command = new SaveWorkloadCommand("Несуществующая дисциплина", "Иванов И. И.", "П21", 1, workloadSummary);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Дисциплина не найдена.");
    }

    [Fact]
    public async Task Handle_ShouldDeleteExistingSchedules_WhenSavingNewWorkload()
    {
        // Arrange
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

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _stsRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<SubjectTeacherSchedule>(), It.IsAny<CancellationToken>()), Times.Once);
        _scheduleRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Schedule>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCreateMultipleSchedules_WhenMultipleWorkloadItemsProvided()
    {
        // Arrange
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

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _scheduleRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Schedule>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
        _stsRepositoryMock.Verify(r => r.AddAsync(It.IsAny<SubjectTeacherSchedule>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
    }

    [Fact]
    public async Task Handle_ShouldHandleSplitLesson_WhenIsLessonSplitIsTrue()
    {
        // Arrange
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

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        createdSchedule.Should().NotBeNull();
        createdSchedule!.IsPractical.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnDatabaseError_WhenExceptionOccurs()
    {
        // Arrange
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

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
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
