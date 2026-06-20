using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Dispatcher;

public class GetGroupsSpec : Specification<Group>
    {
    public GetGroupsSpec()
    {
        Query
            .Include(x => x.CnSpecNavigation)
            .AsNoTracking();
    }
    }
