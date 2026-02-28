using Application.Features.Dispatcher.GetAllLessons;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.DataBase.Repository.Base;
using Infrastructure.DataBase.Repository.Custom;
using Infrastructure.Tests.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Features.Dispatcher.Queries.GetAllLessons;

public class GetAllLessonsQueryTests : IDisposable
{
    private readonly InMemoryDbContextFixture _fixture;
    private readonly string _dbName;

    public GetAllLessonsQueryTests()
    {
        _fixture = new InMemoryDbContextFixture();
        _dbName = _fixture.CreateDatabaseName();
    }

    [Fact]
    public async Task Handle_ShouldReturnAllLessons_WhenSubjectsExist()
    {
        // Arrange
        await using var context = _fixture.CreateContext(_dbName);
        _fixture.Seed(context);

        var subjectRepository = new SubjectRepository(context, new BaseRepository<Subject>(context));
        var loggerMock = new Mock<ILogger<GetAllLessonsQueryHandler>>();

        var handler = new GetAllLessonsQueryHandler(loggerMock.Object, subjectRepository);
        var query = new GetAllLessonsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Lessons.Should().HaveCount(3);

        var databasesLesson = result.Value.Lessons.FirstOrDefault(l => l.LessonName == "Базы данных");
        databasesLesson.Should().NotBeNull();
        databasesLesson!.Semester1.Should().BeTrue();
        databasesLesson.Semester2.Should().BeTrue();
        databasesLesson.Course.Should().Be(2);
        databasesLesson.Groups.Should().ContainKey("Информационные системы");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoSubjects()
    {
        // Arrange
        await using var context = _fixture.CreateContext(_dbName);

        var subjectRepository = new SubjectRepository(context, new BaseRepository<Subject>(context));
        var loggerMock = new Mock<ILogger<GetAllLessonsQueryHandler>>();

        var handler = new GetAllLessonsQueryHandler(loggerMock.Object, subjectRepository);
        var query = new GetAllLessonsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Lessons.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldGroupGroupsBySpecialty_WhenMultipleGroupsExist()
    {
        // Arrange
        await using var context = _fixture.CreateContext(_dbName);
        _fixture.Seed(context);

        var subjectRepository = new SubjectRepository(context, new BaseRepository<Subject>(context));
        var loggerMock = new Mock<ILogger<GetAllLessonsQueryHandler>>();

        var handler = new GetAllLessonsQueryHandler(loggerMock.Object, subjectRepository);
        var query = new GetAllLessonsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var webDevLesson = result.Value.Lessons.FirstOrDefault(l => l.LessonName == "Веб-программирование");
        webDevLesson.Should().NotBeNull();

        var groupsDict = webDevLesson!.Groups;
        groupsDict.Should().ContainKey("Информационные системы");
        groupsDict["Информационные системы"].Should().Contain(new[] { "П21" });
    }

    public void Dispose()
    {
        using var context = _fixture.CreateContext(_dbName);
        context.Database.EnsureDeleted();
    }
}
