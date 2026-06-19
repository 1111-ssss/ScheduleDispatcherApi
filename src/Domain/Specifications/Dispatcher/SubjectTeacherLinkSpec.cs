using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Dispatcher;

public class SubjectTeacherLinkSpec : Specification<SubjectTeacher>
    {
    public SubjectTeacherLinkSpec(string cnT, string cnG, int idSubject)
    {
        Query.Where(st => st.CnT == cnT && st.CnG == cnG && st.IdSubject == idSubject);

        Query.Include(st => st.Subjectteachersemesters);

        Query.Include(st => st.SubjectTeacherSchedules)
            .ThenInclude(sts => sts.IdScheduleNavigation)
                .ThenInclude(s => s.RemovalAsFirst);

        Query.Include(st => st.SubjectTeacherSchedules)
            .ThenInclude(sts => sts.IdScheduleNavigation)
                .ThenInclude(s => s.RemovalAsSecond);
    }
    }
