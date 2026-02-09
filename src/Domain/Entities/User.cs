namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string Role { get; set; } = default!;

    public static User Create(
        string username,
        string password,
        string role
    )
    {
        return new User
        {
            Username = username,
            PasswordHash = password,
            Role = role
        };
    }
}