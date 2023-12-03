using Microsoft.EntityFrameworkCore;
using SecretaryBot.Interfaces;

namespace SecretaryBot
{
    public class Repository : DbContext, IRepository
    {
        private readonly string _connectionString;

        public Repository(string connectionString)
        {
            Database.EnsureCreated();
            _connectionString = connectionString;
        }

        public async Task WriteLog(string message)
        {

        }

        public async Task GetUser(string message)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }
    }
}
