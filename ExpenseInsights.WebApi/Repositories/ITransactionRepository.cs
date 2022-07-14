using ExpenseInsights.WebApi.Models;

namespace ExpenseInsights.WebApi.Repositories
{
    public interface ITransactionRepository
    {
        void Create(Transaction transaction);
        IEnumerable<Transaction> GetAll();
    }
}