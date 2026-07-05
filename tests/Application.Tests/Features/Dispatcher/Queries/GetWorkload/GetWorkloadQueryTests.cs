using Application.Features.Dispatcher.GetWorkload;
using Application.Features.Dispatcher.SaveWorkload;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.DataBase.Repository.Base;
using Infrastructure.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace Application.Tests.Features.Dispatcher.Queries.GetWorkload;

public class GetWorkloadQueryTests : IDisposable
{
    private readonly InMemoryDbContextFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly string _dbName;

    public GetWorkloadQueryTests(ITestOutputHelper testOutputHelper)
    {
        _fixture = new InMemoryDbContextFixture();
        _dbName = _fixture.CreateDatabaseName();
        _output = testOutputHelper;
    }

    [Fact]
    public async Task Handle_ShouldReturnWorkload_WhenDataExists()
    {
        
        await using var context = _fixture.CreateContext(_dbName);
        _fixture.Seed(context);

        var spec = new Domain.Specifications.Dispatcher.GetWorkloadSpec("Иванов И. И.", "П21", 1, "Базы данных");
        var employers = await context.Employers
            .WithSpecification(spec)
            .ToListAsync();

        _output.WriteLine($"Employers found by spec: {employers.Count}");
        foreach (var e in employers)
        {
            _output.WriteLine($"  Employer: {e.CnE}, {e.Surname} {e.Name} {e.FatherName}");
            foreach (var t in e.Teachers)
            {
                _output.WriteLine($"    Teacher: {t.CnT}");
                foreach (var st in t.SubjectTeachers)
                {
                    _output.WriteLine($"      SubjectTeacher: {st.CnG}, {st.IdSubject}");
                    _output.WriteLine($"        Group Name: {st.CnGNavigation?.Name}");
                    _output.WriteLine($"        Subject Name: {st.IdSubjectNavigation?.Name}");
                }
            }
        }

        var employerRepository = new BaseRepository<Employer>(context);
        var loggerMock = new Mock<ILogger<GetWorkloadQueryHandler>>();

        var handler = new GetWorkloadQueryHandler(loggerMock.Object, employerRepository);
        var query = new GetWorkloadQuery("Базы данных", "Иванов И. И.", "П21", 1);

var result = await handler.Handle(query, CancellationToken.None);

result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.WorkloadList.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnEmpty_WhenTeacherNotFound()
    {
        
        await using var context = _fixture.CreateContext(_dbName);
        _fixture.Seed(context);

        var employerRepository = new BaseRepository<Employer>(context);
        var loggerMock = new Mock<ILogger<GetWorkloadQueryHandler>>();

        var handler = new GetWorkloadQueryHandler(loggerMock.Object, employerRepository);
        var query = new GetWorkloadQuery("Базы данных", "Неexistent Teacher", "П21", 1);

var result = await handler.Handle(query, CancellationToken.None);

result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.WorkloadList.Should().BeEmpty();
        result.Value.RemovalList.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnCorrectWorkloadCount_WhenFilteredByGroup()
    {
        
        await using var context = _fixture.CreateContext(_dbName);
        _fixture.Seed(context);

        var employerRepository = new BaseRepository<Employer>(context);
        var loggerMock = new Mock<ILogger<GetWorkloadQueryHandler>>();

        var handler = new GetWorkloadQueryHandler(loggerMock.Object, employerRepository);
        var query = new GetWorkloadQuery("Базы данных", "Иванов И. И.", "П21", 1);

var result = await handler.Handle(query, CancellationToken.None);

result.IsSuccess.Should().BeTrue();
        result.Value.WorkloadList.Should().HaveCount(1);

        var workload = result.Value.WorkloadList.First();
        workload.LessonName.Should().Be("Базы данных");
        workload.LessonIndex.Should().Be(1);
        workload.IsLessonSplit.Should().BeFalse();
        workload.LessonDate1.Should().Be(new DateTime(2025, 9, 1));
        workload.LessonDate2.Should().Be(new DateTime(2025, 9, 2));
    }

    [Fact]
    public async Task Handle_ShouldReturnRemovals_WhenSchedulesHaveRemovals()
    {
        
        await using var context = _fixture.CreateContext(_dbName);
        _fixture.Seed(context);

        var employerRepository = new BaseRepository<Employer>(context);
        var loggerMock = new Mock<ILogger<GetWorkloadQueryHandler>>();

        var handler = new GetWorkloadQueryHandler(loggerMock.Object, employerRepository);
        var query = new GetWorkloadQuery("Базы данных", "Иванов И. И.", "П21", 1);

var result = await handler.Handle(query, CancellationToken.None);

result.IsSuccess.Should().BeTrue();
        result.Value.RemovalList.Should().NotBeEmpty();

        var removal = result.Value.RemovalList.First();
        removal.RemovalIndex.Should().Be(1);
        removal.FirstLessonIndex.Should().Be(1);
        removal.SecondLessonIndex.Should().Be(2);
    }

    [Fact]
    public async Task Handle_ShouldReturnWorkloadForSecondSemester_WhenSemester2IsTrue()
    {
        
        await using var context = _fixture.CreateContext(_dbName);
        _fixture.Seed(context);

        var employerRepository = new BaseRepository<Employer>(context);
        var loggerMock = new Mock<ILogger<GetWorkloadQueryHandler>>();

        var handler = new GetWorkloadQueryHandler(loggerMock.Object, employerRepository);
        var query = new GetWorkloadQuery("Базы данных", "Иванов И. И.", "П22", 2);

var result = await handler.Handle(query, CancellationToken.None);

result.IsSuccess.Should().BeTrue();
        result.Value.WorkloadList.Should().NotBeEmpty();

        var workload = result.Value.WorkloadList.First();
        workload.LessonName.Should().Be("Базы данных");
        workload.IsLessonSplit.Should().BeTrue();
        workload.LessonDate1.Should().Be(new DateTime(2025, 9, 2));
        workload.LessonDate2.Should().Be(new DateTime(2025, 9, 3));
    }

    [Fact]
    public async Task Handle_ShouldReturnEmpty_WhenGroupNotFound()
    {
        
        await using var context = _fixture.CreateContext(_dbName);
        _fixture.Seed(context);

        var employerRepository = new BaseRepository<Employer>(context);
        var loggerMock = new Mock<ILogger<GetWorkloadQueryHandler>>();

        var handler = new GetWorkloadQueryHandler(loggerMock.Object, employerRepository);
        var query = new GetWorkloadQuery("Базы данных", "Иванов И. И.", "Щ01", 1);

var result = await handler.Handle(query, CancellationToken.None);

result.IsSuccess.Should().BeTrue();
        result.Value.WorkloadList.Should().BeEmpty();
        result.Value.RemovalList.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnEmpty_WhenLessonNotFound()
    {
        
        await using var context = _fixture.CreateContext(_dbName);
        _fixture.Seed(context);

        var employerRepository = new BaseRepository<Employer>(context);
        var loggerMock = new Mock<ILogger<GetWorkloadQueryHandler>>();

        var handler = new GetWorkloadQueryHandler(loggerMock.Object, employerRepository);
        var query = new GetWorkloadQuery("Иванов И. И.", "T001", "П21", 1);

var result = await handler.Handle(query, CancellationToken.None);

result.IsSuccess.Should().BeTrue();
    }

    public void Dispose()
    {
        using var context = _fixture.CreateContext(_dbName);
        context.Database.EnsureDeleted();
    }
}
