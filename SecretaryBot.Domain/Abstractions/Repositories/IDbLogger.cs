using Microsoft.Extensions.Logging;

namespace SecretaryBot.Domain.Abstractions.Repositories
{
    public interface IDbLogger
    {
        public Task WriteLogAsync(LogLevel level, string message);
        public Task Info(string message);
        public Task Error(string message);
    }
}
