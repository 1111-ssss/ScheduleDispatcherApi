using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Dispatcher;

public class GetEmployerByNameSpec : Specification<Employer>
{
    public GetEmployerByNameSpec(string teacher)
    {
        Query.Where(e => (e.Surname + " " + e.Name.Substring(0, 1) + ". " + 
                        e.FatherName.Substring(0, 1) + ".") == teacher);

        Query.Include(e => e.Teachers)
            .ThenInclude(t => t.SubjectTeachers)
                .ThenInclude(st => st.CnGNavigation)
            .Include(e => e.Teachers)
                .ThenInclude(t => t.SubjectTeachers)
                    .ThenInclude(st => st.IdSubjectNavigation)
            .Include(e => e.Teachers)
                .ThenInclude(t => t.SubjectTeachers)
                    .ThenInclude(st => st.Subjectteachersemesters)
                        .ThenInclude(sts => sts.IdsubjectTeacherNavigation)
                            .ThenInclude(sts => sts.SubjectTeacherSchedules)
                                .ThenInclude(sts => sts.IdScheduleNavigation)
                                    .ThenInclude(s => s.RemovalAsFirst)
            .Include(e => e.Teachers)
                .ThenInclude(t => t.SubjectTeachers)
                    .ThenInclude(st => st.Subjectteachersemesters)
                        .ThenInclude(sts => sts.IdsubjectTeacherNavigation)
                            .ThenInclude(sts => sts.SubjectTeacherSchedules)
                                .ThenInclude(sts => sts.IdScheduleNavigation)
                                    .ThenInclude(s => s.RemovalAsSecond);

    }
}