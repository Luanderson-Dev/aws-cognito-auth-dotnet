namespace AuthService.Domain.Entities;

public class User
{
    public String Id { get; private set; } 
    public String Username { get; private set; }
    public String Email { get; private set; }

    public User(String id, String username, String email)
    {
        Id = id;
        Username = username;
        Email = email;
    }
}