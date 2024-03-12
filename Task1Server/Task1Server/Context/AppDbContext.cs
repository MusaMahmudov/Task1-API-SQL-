using Microsoft.EntityFrameworkCore;
using Task1Server.Entities;

namespace Task1Server.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<MatchHistory> MatchHistories { get; set; } = null!;
        public DbSet<GameTransactions> GameTransactions { get; set; } = null!;
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
