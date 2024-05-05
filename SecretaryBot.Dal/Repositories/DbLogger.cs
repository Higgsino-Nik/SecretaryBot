using Microsoft.Extensions.Logging;
using SecretaryBot.Dal.Models;
using SecretaryBot.Domain.Abstractions;

namespace SecretaryBot.Dal.Repositories
{
    public class DbLogger(PostgresContext context) : ICustomLogger
    {
        private readonly PostgresContext _context = context;

        public async Task Error(string message)
        {
            await WriteLogAsync(LogLevel.Error, message);
        }

        public async Task Info(string message)
        {
            await WriteLogAsync(LogLevel.Information, message);
        }

        public async Task WriteLogAsync(LogLevel level, string message)
        {
            _context.Logs.Add(new DalLog
            {
                Level = level.ToString(),
                Message = message,
                LogDateTime = DateTime.Today
            });
            await _context.SaveChangesAsync();
        }
    }
}
