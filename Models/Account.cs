using System.Text;

namespace BankingApplication.Models
{
    public class Account(long Id, string firstName, string lastName, CardDetails cardDetails, Dictionary<string, decimal> balances, List<TransactionHistory> transactionHistory)
    {
        public long Id { get; private set; } = Id;
        public string FirstName { get; private set; } = firstName;
        public string LastName { get; private set; } = lastName;
        public CardDetails CardDetails { get; private set; } = cardDetails;
        public Dictionary<string, decimal> Balances { get; set; } = balances;
        public List<TransactionHistory> TransactionHistory { get; set; } = transactionHistory;
        public override string ToString()
        {
            var jsonFormatted = new StringBuilder();
            jsonFormatted.AppendLine("{");
            jsonFormatted.AppendLine($"  \"id\": \"{Id}\",");
            jsonFormatted.AppendLine($"  \"firstName\": \"{FirstName}\",");
            jsonFormatted.AppendLine($"  \"lastName\": \"{LastName}\",");

            jsonFormatted.AppendLine($"  \"cardDetails\": {CardDetails},");

            var balancesStr = string.Join(",\n    ", Balances.Select(b => $"\"{b.Key}\": {b.Value}"));
            jsonFormatted.AppendLine("  \"balances\": {");
            jsonFormatted.AppendLine($"    {balancesStr}");
            jsonFormatted.AppendLine("  },");

            var transactionsStr = string.Join(",\n    ", TransactionHistory);
            jsonFormatted.AppendLine("  \"transactionHistory\": [");
            jsonFormatted.AppendLine($"    {transactionsStr}");
            jsonFormatted.AppendLine("  ]");
            jsonFormatted.Append('}');

            return jsonFormatted.ToString();
        }
    }
}