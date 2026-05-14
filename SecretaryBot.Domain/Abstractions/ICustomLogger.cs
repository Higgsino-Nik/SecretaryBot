using Microsoft.Extensions.Logging;

namespace SecretaryBot.Domain.Abstractions
{
    public interface ICustomLogger
    {
        public Task WriteLogAsync(LogLevel level, string message);
        public Task InfoAsync(string message);
        public Task ErrorAsync(string message);
    }
}
