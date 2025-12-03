using BankingApplication.ConsoleUI;
using BankingApplication.Models;
using BankingApplication.Services;
using BankingApplication.Utils;
using Microsoft.Extensions.Logging;

namespace BankingApplication
{
    public class Program
    {
        private static readonly ILogger<Program> logger = AtmLoggerFactory.CreateLogger<Program>();

        public static void Main()
        {
            try
            {
                StartApp();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error in ATM application");
                Console.WriteLine("An unexpected error occurred. Check logs for details.");
            }
        }

        public static void StartApp()
        {
            string filepath = Utils.Utils.GetFilePathFromProject("Data", "accounts.json");

            if (!File.Exists(filepath))
            {
                Console.WriteLine($"Data file not found: {filepath}");
                logger.LogError("Data file not found: {filepath}", filepath);
                return;
            }

            var jsonStorageService = new JsonStorageService(filepath);

            List<Account>? accounts;
            try
            {
                accounts = jsonStorageService.GetAccounts() ?? new List<Account>();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to read accounts from JSON file");
                Console.WriteLine("Failed to load accounts. See logs for details.");
                return;
            }

            Console.Write("{0,65}", "ATM Started");
            Console.WriteLine("{0,30}", "(type 'exit' inside any field to quit)");

            var authService = new AuthService(accounts, jsonStorageService);

            while (true)
            {
                Console.WriteLine("\n==== MAIN MENU ====");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Choose option: ");

                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    var newAccount = authService.Register();
                    if (newAccount != null)
                    {
                        Console.WriteLine("\nYou can now login with your new card.\n");
                    }
                }
                else if (choice == "2")
                {
                    Account? account = authService.Login();
                    if (account == null)
                    {
                        Console.WriteLine("\nLogin failed. Returning to main menu.\n");
                        continue;
                    }

                    var accountService = new AccountService(jsonStorageService, accounts, account);

                    try
                    {
                        ConsoleMenu.Show(account, accountService);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error in menu");
                        Console.WriteLine("An error occurred while interacting with the account menu.");
                    }
                }
                else if (choice == "3")
                {
                    Console.WriteLine("\nGoodbye!");
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid option. Try again.");
                }
            }
        }
    }
}