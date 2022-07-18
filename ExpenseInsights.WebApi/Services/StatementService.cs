using ExpenseInsights.WebApi.Models;
using ExpenseInsights.WebApi.Repositories;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace ExpenseInsights.WebApi.Services
{
    public class StatementService : IStatementService
    {
        private readonly ITransactionRepository _repository;
        private readonly ILogger<StatementService> _logger;

        public StatementService(ITransactionRepository repository, ILogger<StatementService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // should this method be in this class?
        public bool Upload(IFormFile statement)
        {
            if (statement == null)
                return false;

            if (!statement.FileName.EndsWith(".csv"))
                return false;

            try
            {
                var tempFile = new FileStream(statement.FileName, FileMode.Create, FileAccess.Write);
                statement.CopyTo(tempFile);
                tempFile.Close();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error uploading statment.", ex);
                return false;
            }

            return true;
        }

        public bool Process(string fileName)
        {
            try
            {
                var fileContents = File.ReadAllLines(fileName);

                foreach (var line in fileContents)
                {
                    if (!line.StartsWith("HIST"))
                        continue;

                    var parts = line.Split(",");

                    // TODO: Make this more robust
                    var date = DateTime.ParseExact(parts[1], "yyyyMMdd", CultureInfo.InvariantCulture);
                    var amount = Math.Abs(double.Parse(parts[3], CultureInfo.InvariantCulture));
                    var category = double.Parse(parts[3], CultureInfo.InvariantCulture) < 0 ? TransactionCategory.Out : TransactionCategory.In;
                    var vendor = parts[5];
                    var detail = parts[4];

                    var transaction = new Transaction
                    {
                        Date = date,
                        Amount = amount,
                        Category = category,
                        Vendor = vendor,
                        Detail = detail
                    };

                    // find a better way to not mutate state after object creation
                    transaction.IdempotencyKey = GenerateHash(transaction);
                    
                    _repository.Create(transaction);
                }

                DeleteStatement(fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing file", ex);
                return false;
            }

            return true;
        }

        private static bool DeleteStatement(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            return File.Exists(fileName); //probably don't need this
        }

        private static string GenerateHash(Transaction transaction)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(transaction));
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashBytes);
        }

        public List<Transaction> GetAllTransactions()
        {
            return _repository.GetAll().ToList();
        }
    }
}
