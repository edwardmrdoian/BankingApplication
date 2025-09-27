namespace BankingApplication.Models
{
    public class TransactionHistory(DateTime transactionDate, string type, decimal amountGEL, decimal amountUSD, decimal amountEUR)
    {
        public DateTime TransactionDate { get; set; } = transactionDate;
        public string Type { get; set; } = type;
        public decimal AmountGEL { get; set; } = amountGEL;
        public decimal AmountUSD { get; set; } = amountUSD;
        public decimal AmountEUR { get; set; } = amountEUR;
        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(new
            {
                TransactionDate,
                Type,
                AmountGEL,
                AmountUSD,
                AmountEUR
            });
        }
    }
}
