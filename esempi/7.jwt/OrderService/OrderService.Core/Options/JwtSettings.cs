// Core/Options/JwtSettings.cs
namespace OrderService.Core.Options;

public record JwtSettings(
    string Key,
    string Issuer,
    string Audience,
    int    ExpiryMinutes);
