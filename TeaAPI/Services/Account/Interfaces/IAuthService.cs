using System.Security.Claims;
using TeaAPI.Models.Auths;

namespace TeaAPI.Services.Account.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(string account, string password);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
        string GenerateRefreshToken();
        ClaimsPrincipal ValidateExpiredToken(string token);
    }
}
