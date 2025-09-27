using BankingApplication.Models;
using BankingApplication.Utils;
using Microsoft.Extensions.Logging;

namespace BankingApplication.Services.Transactions
{
    public class DepositTransaction : ITransaction
    {
        private readonly ILogger<DepositTransaction> logger = AtmLoggerFactory.CreateLogger<DepositTransaction>();
        public void Create(Account account, decimal amount, string currency)
        {
            try
            {
                currency = currency.ToUpper();
                if (!account.Balances.ContainsKey(currency))
                {
                    logger.LogWarning("user with id {id} doesn't have this currency {curr}", account.Id, currency);
                    return;
                }

                account.Balances[currency] += amount;

                account.TransactionHistory.Add(new TransactionHistory
                (
                    DateTime.Now,
                    "Deposit",
                    currency == "GEL" ? amount : 0,
                    currency == "USD" ? amount : 0,
                    currency == "EUR" ? amount : 0
                ));
                logger.LogInformation("Account with id {id} Added Deposit operation: {transactionHistory}",
                    account.Id, account.TransactionHistory.Last());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error during deposit.");
                Console.WriteLine("Something went wrong, please try again.");
            }
        }
    }
}
