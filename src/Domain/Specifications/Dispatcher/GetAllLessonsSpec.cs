using Ardalis.Specification;
using Infrastructure.src.Domain.Entities;

namespace Domain.Specifications.Dispatcher;

public class GetAllLessonsSpec : Specification<Subject>
{
    public GetAllLessonsSpec()
    {
        Query
            .Include(x => x.CnSpecNavigation)
            .ThenInclude(x => x.Groups)
            .ThenInclude(x => x.SubjectTeachers)
            .ThenInclude(x => x.Subjectteachersemesters)
            .AsNoTracking();
    }
}
