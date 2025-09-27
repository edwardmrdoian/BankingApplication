using BankingApplication.Models;
using BankingApplication.Services.Transactions;
using BankingApplication.Utils;
using Microsoft.Extensions.Logging;
using System.Text;

namespace BankingApplication.Services
{
    public class AccountService(JsonStorageService storage, List<Account> accounts, Account account)
    {
        private readonly ILogger<AccountService> logger = AtmLoggerFactory.CreateLogger<AccountService>();
        public void ShowBalance()
        {
            Console.WriteLine("\n=== Balance Information ===");
            StringBuilder sb = new();
            foreach (var balance in account.Balances)
            {
                sb.AppendLine($"{balance.Key}: {balance.Value}");
            }
            Console.WriteLine(sb.ToString());
            logger.LogInformation("user {accountName} with id {id} checking balance {balance}",
                account.FirstName, account.Id, sb.Replace("\r\n", " "));
        }
        public void Deposit(decimal amount, string currency)
        {
            new DepositTransaction().Create(account, amount, currency);
            storage.SaveAccounts(accounts);
        }

        public void Withdraw(decimal amount, string currency)
        {
            new WithdrawTransaction().Create(account, amount, currency);
            storage.SaveAccounts(accounts);
        }
        public void ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
        {
            if (!account.Balances.ContainsKey(fromCurrency.ToString()))
            {
                Console.WriteLine($"\nAccount doesn't have currency {fromCurrency}.");
                logger.LogWarning("Account with id {id} doesn't have this currency: {curr}.", account.Id, fromCurrency);
            }

            if (!account.Balances.ContainsKey(toCurrency.ToString()))
            {
                Console.WriteLine($"\nAccount doesn't have currency {toCurrency}.");
                logger.LogWarning("Account with id {id} doesn't have this currency: {curr}.", account.Id, fromCurrency);
                return;
            }

            if (account.Balances[fromCurrency.ToString()] < amount)
            {
                Console.WriteLine("\nInsufficient funds for conversion.");
                logger.LogWarning("Account with id {id} has insufficient funds for conversion currency: {curr}.",
                    account.Id, fromCurrency);
                return;
            }

            decimal rate = ConversionRates.GetRate
                (
                CurrencyUtils.FromString(fromCurrency),
                CurrencyUtils.FromString(toCurrency)
                );
            decimal converted = amount * rate;

            account.Balances[fromCurrency.ToString()] -= amount;
            account.Balances[toCurrency.ToString()] += converted;
            account.TransactionHistory.Add(new TransactionHistory
            (
                DateTime.Now,
                "Conversion",
                fromCurrency == "GEL" ? -amount : toCurrency == "GEL" ? converted : 0,
                fromCurrency == "USD" ? -amount : toCurrency == "USD" ? converted : 0,
                fromCurrency == "EUR" ? -amount : toCurrency == "EUR" ? converted : 0
            ));

            Console.WriteLine($"Converted {amount} {fromCurrency}  =>  {converted} {toCurrency}");
            logger.LogInformation("Account with id {id} Added Conversion operation: {transactionHistory}",
                   account.Id, account.TransactionHistory.Last());
            storage.SaveAccounts(accounts);
        }

        public void ShowLastTransactions(int size)
        {
            try
            {
                Console.WriteLine("\n=== Last 5 Transactions ===");
                var lastFive = account.TransactionHistory
                    .OrderByDescending(t => t.TransactionDate)
                    .Take(size);
                StringBuilder historyStr = new();

                historyStr.AppendLine("\"transactionHistory\": [");
                foreach (var transaction in lastFive)
                {
                    var transactionsStr = string.Join(",\n", transaction);
                    historyStr.AppendLine($"{transactionsStr}");
                }
                historyStr.Append(" ]");
                Console.WriteLine(historyStr.ToString());
                logger.LogInformation("user with id {id} getting last 5 transactions {transactions}",
                   account.Id, historyStr.ToString());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error during getting last transactions.");
                Console.WriteLine("Something went wrong, please try again.");
            }
        }

        public void ChangePin(string newPin)
        {
            try
            {
                account.CardDetails.Pin = newPin;

                account.TransactionHistory.Add(new TransactionHistory
                (DateTime.Now,
                   "PIN Change",
                    0,
                    0,
                    0
                ));
                Console.WriteLine("\nPIN updated successfully.");
                logger.LogInformation("Account with id {id} Changed Pin with newPin:{newPin} : {transactionHistory}",
                      account.Id, newPin, account.TransactionHistory.Last());
                storage.SaveAccounts(accounts);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error during changing pin.");
                Console.WriteLine("Something went wrong, please try again.");
            }
        }

    }
}
