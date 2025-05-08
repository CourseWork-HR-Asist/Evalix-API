using Domain.Roles;

namespace Domain.Users;

public class User
{
    public UserId Id { get; }
    public string FirstName { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public RoleId RoleId { get; private set; }
    public Role? Role { get; }

    private User(UserId id, string firstName, string email, string passwordHash, RoleId roleId)
    {
        Id = id;
        FirstName = firstName;
        UpdatedAt = DateTime.UtcNow;
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;
    }


    public static User New(UserId id, string firstName, string email,
        string password, RoleId roleId)
        => new(id, firstName, email, password, roleId);
    
    
    public void UpdateDetails(string firstName)
    {
        FirstName = firstName;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateRole(RoleId roleId)
    {
        RoleId = roleId;
    }

}