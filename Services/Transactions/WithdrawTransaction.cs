using BankingApplication.Models;
using BankingApplication.Utils;
using Microsoft.Extensions.Logging;

namespace BankingApplication.Services.Transactions
{
    public class WithdrawTransaction : ITransaction
    {
        private readonly ILogger<WithdrawTransaction> logger = AtmLoggerFactory.CreateLogger<WithdrawTransaction>();
        public void Create(Account account, decimal amount, string currency)
        {
            try
            {
                currency = currency.ToUpper();
                if (!account.Balances.TryGetValue(currency, out decimal value))
                {
                    logger.LogWarning("Account with id {id} doesn't have this currency {curr}", account.Id, currency);
                    return;
                }

                if (value < amount)
                {
                    Console.WriteLine("Insufficient funds.");
                    logger.LogWarning("Account with id {id} has Insufficient funds with {curr} currency", account.Id, currency);
                    return;
                }

                account.Balances[currency] -= amount;

                account.TransactionHistory.Add(new TransactionHistory
                (
                    DateTime.Now,
                    "Withdraw",
                    currency == "GEL" ? amount : 0,
                    currency == "USD" ? amount : 0,
                    currency == "EUR" ? amount : 0
                ));
                logger.LogInformation("Account with id {id} Added Withdraw operation: {transactionHistory}",
                    account.Id, account.TransactionHistory.Last());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error during withdraw.");
                Console.WriteLine("Something went wrong, please try again.");
            }
        }
    }
}
