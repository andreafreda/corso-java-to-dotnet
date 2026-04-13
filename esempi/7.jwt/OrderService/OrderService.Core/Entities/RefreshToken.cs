// Core/Entities/RefreshToken.cs
namespace OrderService.Core.Entities;

public class RefreshToken
{
    public string   Token     { get; set; } = string.Empty;
    public string   UserId    { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool     Revoked   { get; set; }

    public bool IsValid => !Revoked && ExpiresAt > DateTime.UtcNow;
}
