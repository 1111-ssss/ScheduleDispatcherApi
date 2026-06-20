using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Dispatcher;

public class SubjectByNameSpec : Specification<Subject>
    {
    public SubjectByNameSpec(string name)
    {
        Query.Where(s => s.Name == name);
    }
    }
