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
            .Select(s => new LessonInfoDTO(
                LessonName: s.Name,

                Semester1: s.CnSpecNavigation!
                    .Groups!
                    .SelectMany(g => g.SubjectTeachers!)
                    .SelectMany(st => st.Subjectteachersemesters!)
                    .Any(sts => sts.Semester1 == true),

                Semester2: s.CnSpecNavigation!
                    .Groups!
                    .SelectMany(g => g.SubjectTeachers!)
                    .SelectMany(st => st.Subjectteachersemesters!)
                    .Any(sts => sts.Semester2 == true),

                Course: s.CnSpecNavigation!
                    .Groups!
                    .Select(g => g.Cours)
                    .Distinct()
                    .OrderBy(c => c)
                    .FirstOrDefault(),

                Groups: s.CnSpecNavigation!
                    .Groups!
                    .Where(g => !string.IsNullOrEmpty(g.Name))
                    .GroupBy(g => g.CnSpecNavigation != null 
                        ? g.CnSpecNavigation.Name ?? "Без специальности" 
                        : "Без специальности")
                    .ToDictionary(
                        grp => grp.Key,
                        grp => grp
                            .Select(g => g.Name!)
                            .Distinct()
                            .OrderBy(name => name)
                            .ToList()
                    )
            ))
            .ToListAsync(ct);

        return lessons;
        
    }
}