using ExpenseInsights.WebApi.Models;

namespace ExpenseInsights.WebApi.Services
{
    public interface IStatementService
    {
        bool Upload(IFormFile statement);

        bool Process(string fileName);

        List<Transaction> GetAllTransactions();
    }
}