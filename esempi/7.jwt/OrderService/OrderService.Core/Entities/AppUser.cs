// Core/Entities/AppUser.cs
namespace OrderService.Core.Entities;

// In produzione: tabella Users con password hashata (Identity o custom)
public class AppUser
{
    public string Id           { get; set; } = Guid.NewGuid().ToString();
    public string Email        { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role         { get; set; } = "User";
}
