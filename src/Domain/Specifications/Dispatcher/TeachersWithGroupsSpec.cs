using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Dispatcher;

public class TeachersWithGroupsSpec : Specification<Teacher>
    {
    public TeachersWithGroupsSpec()
    {
        Query
            .Include(t => t.CnENavigation)
            .Include(t => t.SubjectTeachers)
                .ThenInclude(st => st.CnGNavigation)
            .Include(t => t.SubjectTeachers)
                .ThenInclude(st => st.IdSubjectNavigation)
            .Include(t => t.SubjectTeachers)
                .ThenInclude(st => st.Subjectteachersemesters);

        Query.AsSplitQuery();
    }
    }
