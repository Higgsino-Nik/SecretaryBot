using Microsoft.Extensions.Logging;

namespace SecretaryBot.Domain.Abstractions
{
    public interface ICustomLogger
    {
        public Task WriteLogAsync(LogLevel level, string message);
        public Task Info(string message);
        public Task Error(string message);
    }
}
