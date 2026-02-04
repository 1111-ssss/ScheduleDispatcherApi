using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Auth;

public class UserByUsernameSpec : Specification<User>
{
    public UserByUsernameSpec(string username)
    {
        Query.Where(e =>
            e.Username == username
        );
    }
}
