using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Dispatcher;

public class GroupByCnGSpec : Specification<Group>
    {
    public GroupByCnGSpec(string cnG)
    {
        Query.Where(g => g.CnG == cnG);
    }
    }
