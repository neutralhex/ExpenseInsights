using ExpenseInsights.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseInsights.WebApi.DbContexts
{
    public class TransactionDbContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public string DbPath { get; set; }

        public TransactionDbContext(DbContextOptions<TransactionDbContext> options)
            : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "transactions.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
