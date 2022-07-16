using System.ComponentModel.DataAnnotations;

namespace ExpenseInsights.WebApi.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public double Amount { get; set; }
        public string Vendor { get; set; } = null!;
        public DateTime Date { get; set; }
        public TransactionCategory Category { get; set; }
        public string Detail { get; set; } = null!;
        public string IdempotencyKey { get; set; } = null!;
    }

    public enum TransactionCategory
    {
        In,
        Out
    }
}
