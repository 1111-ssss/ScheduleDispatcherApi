namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;

    private User() { }
    public static User Create(string username, string password)
    {
        return new User
        {
            Username = username,
            PasswordHash = password
        };
    }
}