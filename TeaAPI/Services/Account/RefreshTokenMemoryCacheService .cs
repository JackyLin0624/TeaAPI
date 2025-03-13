using Microsoft.Extensions.Caching.Memory;
using TeaAPI.Services.Account.Interfaces;

namespace TeaAPI.Services.Account
{
    public class RefreshTokenMemoryCacheService : IRefreshTokenCacheService
    {
        private readonly IMemoryCache _cache;

        public RefreshTokenMemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task SetRefreshTokenAsync(int userId, string refreshToken, TimeSpan expiration)
        {
            var cacheKey = GetCacheKey(refreshToken);
            _cache.Set(cacheKey, userId, expiration); 
            return Task.CompletedTask;
        }

        public Task<int?> GetUserIdByRefreshTokenAsync(string refreshToken)
        {
            var cacheKey = GetCacheKey(refreshToken);
            if (_cache.TryGetValue(cacheKey, out int userId))
            {
                return Task.FromResult<int?>(userId);
            }
            return Task.FromResult<int?>(null);
        }

        public Task RemoveRefreshTokenAsync(string refreshToken)
        {
            var cacheKey = GetCacheKey(refreshToken);
            _cache.Remove(cacheKey);
            return Task.CompletedTask;
        }

        private string GetCacheKey(string refreshToken)
        {
            return $"refresh_token_{refreshToken}";
        }
    }

}
