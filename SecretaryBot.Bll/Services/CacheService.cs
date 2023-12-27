using Microsoft.Extensions.Caching.Memory;
using SecretaryBot.Domain.Abstractions.Services;

namespace SecretaryBot.Bll.Services
{
    public class CacheService(IMemoryCache cache) : ICacheService
    {
        private readonly IMemoryCache _cache = cache;
        private const int CacheExpirationMinutes = 15;

        public void AddLastCommand(long userId, string command) =>
            _cache.Set(userId, command, DateTimeOffset.Now.AddMinutes(CacheExpirationMinutes));

        public string? GetLastCommand(long userId)
        {
            var lastCommand = _cache.Get<string>(userId);
            _cache.Set(userId, string.Empty);
            return lastCommand;
        }
    }
}
