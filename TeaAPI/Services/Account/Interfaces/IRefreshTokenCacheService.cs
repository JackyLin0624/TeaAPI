namespace TeaAPI.Services.Account.Interfaces
{
    public interface IRefreshTokenCacheService
    {
        Task SetRefreshTokenAsync(int userId, string refreshToken, TimeSpan expiration);
        Task<int?> GetUserIdByRefreshTokenAsync(string refreshToken);
        Task RemoveRefreshTokenAsync(string refreshToken);
    }
}
