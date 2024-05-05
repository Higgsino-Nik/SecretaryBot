using Microsoft.EntityFrameworkCore;
using SecretaryBot.Dal.Models;

namespace SecretaryBot.Dal
{
    public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options)
    {
        public DbSet<DalCategory> Categories { get; set; }
        public DbSet<DalLog> Logs { get; set; }
        public DbSet<DalPurchase> Purchases { get; set; }
        public DbSet<DalUser> Users { get; set; }
    }
}
