using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Dispatcher;

public class EmployerByFioSpec : Specification<Employer>
    {
    public EmployerByFioSpec(string teacher)
    {

        var normalizedTeacher = teacher.Replace(" ", string.Empty);
        Query.Where(e => (e.Surname + " " + e.Name.Substring(0, 1) + ". " + e.FatherName.Substring(0, 1) + ".").Replace(" ", string.Empty) == normalizedTeacher);

        Query.Include(e => e.Teachers)
            .ThenInclude(t => t.SubjectTeachers);
    }
    }
