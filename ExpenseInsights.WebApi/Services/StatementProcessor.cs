using ExpenseInsights.WebApi.Models;
using ExpenseInsights.WebApi.Repositories;
using System.Globalization;

namespace ExpenseInsights.WebApi.Services
{
    public class StatementProcessor : IStatementProcessor
    {
        private readonly ITransactionRepository _repository;
        private readonly ILogger<StatementProcessor> _logger;

        public StatementProcessor(ITransactionRepository repository, ILogger<StatementProcessor> logger)
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
                    _repository.Create(new Transaction
                    {
                        Time = DateTime.ParseExact(parts[1], "yyyyMMdd", CultureInfo.InvariantCulture),
                        Amount = Math.Abs(double.Parse(parts[3], CultureInfo.InvariantCulture)),
                        Category = double.Parse(parts[3], CultureInfo.InvariantCulture) < 0 ? TransactionCategory.Out : TransactionCategory.In,
                        Vendor = parts[5],
                        Detail = parts[4]
                    });
                }

                DeleteStatement(fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error processing file.", ex);
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
    }
}
