// Core/Services/AuthService.cs
using System.Security.Cryptography;
using System.Text;
using OrderService.Core.DTOs;
using OrderService.Core.Entities;
using OrderService.Core.Interfaces;

namespace OrderService.Core.Services;

public class AuthService
{
    private readonly TokenService       _tokenService;
    private readonly IRefreshTokenStore _refreshStore;

    private static readonly List<AppUser> Users =
    [
        new() { Id = "1", Email = "admin@example.com",
                PasswordHash = HashPassword("admin123"), Role = "Admin" },
        new() { Id = "2", Email = "user@example.com",
                PasswordHash = HashPassword("user123"),  Role = "User"  }
    ];

    public AuthService(TokenService tokenService, IRefreshTokenStore refreshStore)
    {
        _tokenService = tokenService;
        _refreshStore = refreshStore;
    }

    public TokenResponse? Login(string email, string password)
    {
        var user = Users.FirstOrDefault(u =>
            u.Email == email && u.PasswordHash == HashPassword(password));

        if (user is null) return null;

        return IssueTokens(user);
    }

    public TokenResponse? Refresh(string refreshToken)
    {
        var stored = _refreshStore.Get(refreshToken);
        if (stored is null || !stored.IsValid) return null;

        var user = Users.FirstOrDefault(u => u.Id == stored.UserId);
        if (user is null) return null;

        // Refresh token rotation — il vecchio non è più valido
        _refreshStore.Revoke(refreshToken);

        return IssueTokens(user);
    }

    public bool Logout(string refreshToken)
    {
        var stored = _refreshStore.Get(refreshToken);
        if (stored is null) return false;
        _refreshStore.Revoke(refreshToken);
        return true;
    }

    private TokenResponse IssueTokens(AppUser user)
    {
        var accessToken  = _tokenService.GenerateToken(user.Id, user.Email, user.Role);
        var refreshToken = new RefreshToken
        {
            Token     = _tokenService.GenerateRefreshToken(),
            UserId    = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            Revoked   = false
        };

        _refreshStore.Store(refreshToken);
        return new TokenResponse(accessToken, refreshToken.Token, ExpiresIn: 3600);
    }

    // SHA256 per semplicità — in produzione usa BCrypt o Argon2
    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}
