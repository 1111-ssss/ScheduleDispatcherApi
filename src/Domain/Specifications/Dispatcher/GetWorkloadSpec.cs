using System.Diagnostics.Tracing;
using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Dispatcher;

public class GetWorkloadSpec : Specification<Employer>
{
    public GetWorkloadSpec(string teacher, string group, int semester, string lesson)
    {
        Query
            .Where(e => (e.Surname + " " + e.Name.Substring(0, 1) + ". " + e.FatherName.Substring(0, 1) + ".") == teacher)
            .Where(e => e.Teachers.Any(
                t => t.SubjectTeachers.Any(
                    st => st.CnGNavigation.Name == group
                    && st.IdSubjectNavigation.Name == lesson
                )
            ))
            .Where(e => e.Teachers.Any(
                t => t.SubjectTeachers.Any(
                    st => st.Subjectteachersemesters.Any(
                        sts => (semester == 1 && sts.Semester1 == true)
                        || (semester == 2 && sts.Semester2 == true)
                    )
                )
            ));

        Query
            .Include(e => e.Teachers)
                .ThenInclude(t => t.SubjectTeachers)
                    .ThenInclude(st => st.CnGNavigation)

            .Include(e => e.Teachers)
                .ThenInclude(t => t.SubjectTeachers)
                    .ThenInclude(st => st.IdSubjectNavigation)

            .Include(e => e.Teachers)
                .ThenInclude(t => t.SubjectTeachers)
                    .ThenInclude(st => st.Subjectteachersemesters)

            .Include(e => e.Teachers)
                .ThenInclude(t => t.SubjectTeachers)
                    .ThenInclude(st => st.SubjectTeacherSchedules)
                        .ThenInclude(sts => sts.IdScheduleNavigation)
                            .ThenInclude(s => s.RemovalAsFirst)

            .Include(e => e.Teachers)
                .ThenInclude(t => t.SubjectTeachers)
                    .ThenInclude(st => st.SubjectTeacherSchedules)
                        .ThenInclude(sts => sts.IdScheduleNavigation)
                            .ThenInclude(s => s.RemovalAsSecond)

            .AsSplitQuery();
    }
}