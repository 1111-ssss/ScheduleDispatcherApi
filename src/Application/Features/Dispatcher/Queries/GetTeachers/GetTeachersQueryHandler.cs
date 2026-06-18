using Application.Abstractions.Repository.Base;
using Domain.Entities;
using Domain.Model.Result;
using Domain.Specifications.Dispatcher;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Dispatcher.GetTeachers;

public class GetTeachersQueryHandler : IRequestHandler<GetTeachersQuery, Result<List<string>>>
    {
    private readonly ILogger<GetTeachersQueryHandler> _logger;
    private readonly IBaseRepository<Teacher> _teacherRepository;

    public GetTeachersQueryHandler(
        ILogger<GetTeachersQueryHandler> logger,
        IBaseRepository<Teacher> teacherRepository
    )
    {
        _logger = logger;
        _teacherRepository = teacherRepository;
    }

    public async Task<Result<List<string>>> Handle(GetTeachersQuery query, CancellationToken ct)
    {
        var teachers = await _teacherRepository.ListAsync(new TeachersWithGroupsSpec(), ct);

        var filtered = teachers.Where(t => t.SubjectTeachers.Any(st =>
            (string.IsNullOrWhiteSpace(query.Group)
                || string.Equals(st.CnGNavigation?.Name, query.Group, StringComparison.OrdinalIgnoreCase)
                || string.Equals(st.CnG, query.Group, StringComparison.OrdinalIgnoreCase))
            && (string.IsNullOrWhiteSpace(query.Lesson)
                || string.Equals(st.IdSubjectNavigation?.Name, query.Lesson, StringComparison.OrdinalIgnoreCase))
            && (query.Semester is null || query.Semester <= 0
                || st.Subjectteachersemesters.Any(semester =>
                    query.Semester == 1 && semester.Semester1 == true
                    || query.Semester == 2 && semester.Semester2 == true))));

        var names = filtered
            .Select(t => ToFio(t.CnENavigation))
            .Where(fio => !string.IsNullOrWhiteSpace(fio))
            .Select(fio => fio!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(fio => fio)
            .ToList();

        return Result<List<string>>.Success(names);
    }

    private static string? ToFio(Employer? employer)
    {
        if (employer is null
            || string.IsNullOrWhiteSpace(employer.Surname)
            || string.IsNullOrWhiteSpace(employer.Name)
            || string.IsNullOrWhiteSpace(employer.FatherName))
        {
            return null;
        }

        return $"{employer.Surname} {employer.Name[..1]}. {employer.FatherName[..1]}.";
    }
    }
