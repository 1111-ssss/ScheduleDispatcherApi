using Application.Abstractions.Repository.Base;
using Application.Abstractions.Repository.Custom;
using Application.Features.Dispatcher.GetAllLessons;
using Ardalis.Specification;
using Infrastructure.DataBase.Context;
using Infrastructure.DataBase.Repository.Base;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase.Repository.Custom;

public class SubjectRepository : BaseRepository<Subject>, ISubjectRepository
{
    public readonly IBaseRepository<Subject> _subjectRepository;

    public SubjectRepository(
        AppDbContext context,
        IBaseRepository<Subject> subjectRepository
    ) : base(context)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task<List<LessonInfoDTO>> GetAllLessons(
        Specification<Subject> spec,
        CancellationToken ct
    )
    {
        var query = ApplySpecification(spec);

        var lessons = await query
            .Select(s => new {
                LessonName = s.Name,

                Semester1 = s.CnSpecNavigation!
                    .Groups!
                    .SelectMany(g => g.SubjectTeachers!)
                    .SelectMany(st => st.Subjectteachersemesters!)
                    .Any(sts => sts.Semester1 == true),

                Semester2 = s.CnSpecNavigation!
                    .Groups!
                    .SelectMany(g => g.SubjectTeachers!)
                    .SelectMany(st => st.Subjectteachersemesters!)
                    .Any(sts => sts.Semester2 == true),

                Course = s.CnSpecNavigation!
                    .Groups!
                    .Select(g => g.Cours)
                    .Distinct()
                    .OrderBy(c => c)
                    .FirstOrDefault(),

                GroupsFlat = s.CnSpecNavigation!
                    .Groups!
                    .Where(g => !string.IsNullOrEmpty(g.Name))
                    .Select(g => new
                    {
                        GroupName = g.Name!,
                        SpecName  = g.CnSpecNavigation != null 
                            ? g.CnSpecNavigation.Name ?? "Без специальности"
                            : "Без специальности"
                    })
            }).ToListAsync(ct);

        return lessons.Select(r => new LessonInfoDTO(
            LessonName: r.LessonName,
            Semester1: r.Semester1,
            Semester2: r.Semester2,
            Course: r.Course,
            Groups: r.GroupsFlat
                .GroupBy(x => x.SpecName)
                .ToDictionary(
                    g => g.Key,
                    g => g
                        .Select(x => x.GroupName)
                        .Distinct()
                        .OrderBy(name => name)
                        .ToList()
                )
        )).ToList();;
    }
}