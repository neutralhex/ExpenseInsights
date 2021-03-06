using ExpenseInsights.WebApi.DbContexts;
using ExpenseInsights.WebApi.Models;
using System.Text.Json;

namespace ExpenseInsights.WebApi.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ILogger<TransactionRepository> _logger;
        private readonly TransactionDbContext _db;

        public TransactionRepository(ILogger<TransactionRepository> logger, TransactionDbContext dbContext)
        {
            _logger = logger;
            _db = dbContext;
        }

        public void Create(Transaction transaction)
        {
            try
            {
                _db.Add(transaction);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _db.Remove(transaction);
                _logger.LogError("Unable to create Transaction in Database", transaction);
            }
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _db.Transactions.OrderBy(t => t.TransactionId);
        }
    }
}
