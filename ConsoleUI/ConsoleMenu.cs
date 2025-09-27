using BankingApplication.Models;
using BankingApplication.Services;
using BankingApplication.Utils;
using Microsoft.Extensions.Logging;

namespace BankingApplication.ConsoleUI
{
    public class ConsoleMenu
    {
        private static readonly ILogger<ConsoleMenu> logger = AtmLoggerFactory.CreateLogger<ConsoleMenu>();

        public static void Show(Account account, AccountService accountService)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("\n=== ATM Menu ===");
                    Console.WriteLine(
                        """
                        1. View Balance
                        2. Withdraw Money
                        3. Deposit Money
                        4. Last 5 Transactions
                        5. Change PIN
                        6. Currency Conversion
                        7. Exit                       
                        """
                        );
                    Console.Write("Choose option: ");
                    string? choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            accountService.ShowBalance();
                            break;
                        case "2":
                            Console.Write("Enter currency (GEL/USD/EUR): ");
                            string? wcur = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(wcur))
                            {
                                Console.WriteLine("\nCurrency is required.\n");
                                continue;
                            }
                            if (wcur.Length != 3)
                            {
                                Console.WriteLine($"\nInvalid Currency. Length Should be 3.\n");
                                logger.LogWarning("User with id {id} entered invalid currency: {wcur}", account.Id, wcur);
                                continue;
                            }
                            Console.Write("Enter amount: ");
                            string? wamountInput = Console.ReadLine();
                            decimal wamount;
                            if (!decimal.TryParse(wamountInput, out wamount))
                            {
                                Console.WriteLine("\nInvalid amount entered.\n");
                                logger.LogWarning("User with id {id} entered invalid amount: {wamount}", account.Id, wamount);
                                continue;
                            }
                            accountService.Withdraw(wamount, wcur);
                            break;
                        case "3":
                            Console.Write("Enter currency (GEL/USD/EUR): ");
                            string? dcur = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(dcur))
                            {
                                Console.WriteLine("\nCurrency is required.\n");
                                return;
                            }
                            Utils.Utils.CheckCurrency(account, dcur);
                            Console.Write("Enter amount: ");
                            string? damountInput = Console.ReadLine();
                            decimal damount;
                            if (!decimal.TryParse(damountInput, out damount))
                            {
                                Console.WriteLine("\nInvalid amount entered.\n");
                                logger.LogWarning("User with id {id} entered invalid amount: {wamount}", account.Id, damount);
                                continue;
                            }
                            accountService.Deposit(damount, dcur);
                            break;
                        case "4":
                            accountService.ShowLastTransactions(5);
                            break;
                        case "5":
                            Console.Write("Enter new PIN: ");
                            string? newPin = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(newPin))
                            {
                                Console.WriteLine("\nPIN is required.\n");
                                continue;
                            }
                            if (newPin.Length != 4)
                            {
                                Console.WriteLine($"\nInvalid PIN. Length Should be 4.\n");
                                logger.LogWarning("User with id {id}entered invalid PIN: {pin}", account.Id, newPin);
                                continue;
                            }
                            accountService.ChangePin(newPin);
                            break;
                        case "6":
                            Console.Write("From currency: ");
                            string? fcur = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(fcur))
                            {
                                Console.WriteLine("\nCurrency is required.\n");
                                continue;
                            }
                            Utils.Utils.CheckCurrency(account, fcur);

                            Console.Write("To currency: ");
                            string? tcur = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(tcur))
                            {
                                Console.WriteLine("\nCurrency is required.\n");
                                continue;
                            }
                            Utils.Utils.CheckCurrency(account, tcur);

                            Console.Write("Amount: ");
                            string? amountInput = Console.ReadLine();
                            decimal amount;
                            if (!decimal.TryParse(amountInput, out amount))
                            {
                                Console.WriteLine("\nInvalid amount entered.\n");
                                logger.LogWarning("User with id {id} entered invalid amount: {amount}", account.Id, amount);
                                continue;
                            }
                            accountService.ConvertCurrency(amount, fcur, tcur);
                            break;
                        case "7":
                            Console.WriteLine("\nThank you! Goodbye.");
                            return;
                        default:
                            Console.WriteLine("\nInvalid choice, try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Unexpected error during login.");
                    Console.WriteLine("\nSomething went wrong, please try again.");
                    continue;
                }
            }
        }
    }
}
