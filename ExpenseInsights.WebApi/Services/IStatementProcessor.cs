namespace ExpenseInsights.WebApi.Services
{
    public interface IStatementProcessor
    {
        bool Upload(IFormFile statement);

        bool Process(string fileName);
    }
}