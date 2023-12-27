using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SecretaryBot.Dal.Models;
using SecretaryBot.Domain.Abstractions.Repositories;

namespace SecretaryBot.Dal.Repositories
{
    public class DbLogger : DbContext, IDbLogger
    {
        private DbSet<DalLog> Logs { get; set; }

        public DbLogger(DbContextOptions<DbLogger> options) : base(options)
        {
        }

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
            Logs.Add(new DalLog
            {
                Level = level.ToString(),
                Message = message,
                LogDateTime = DateTime.Today
            });
            await SaveChangesAsync();
        }
    }
}
