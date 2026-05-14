using Microsoft.Extensions.Caching.Memory;
using SecretaryBot.Domain.Abstractions.Services;

namespace SecretaryBot.Bll.Services
{
    public class CacheService(IMemoryCache cache) : ICacheService
    {
        private readonly IMemoryCache _cache = cache;
        private const int CacheExpirationMinutes = 15;

        public void SetLastCommand(long userId, string command) =>
            _cache.Set(userId, command, DateTimeOffset.Now.AddMinutes(CacheExpirationMinutes));

        public string? GetLastCommand(long userId) => _cache.Get<string>(userId);

        public void ClearLastCommand(long userId) => _cache.Remove(userId);
    }
}
