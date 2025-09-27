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
                accounts = jsonStorageService.GetAccounts();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to read accounts from JSON file");
                Console.WriteLine("Failed to load accounts. See logs for details.");
                return;
            }

            if (accounts == null || accounts.Count == 0)
            {
                Console.WriteLine("No accounts found in the system.");
                logger.LogWarning("No accounts loaded from JSON file: {filepath} ", filepath);
                return;
            }

            Console.Write("{0,65}", "ATM Started");
            Console.WriteLine("{0,30}", "(type 'exit' to quit)");

            var authService = new AuthService(accounts);
            Account? account;
            try
            {
                account = authService.Login();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during login process");
                Console.WriteLine("Login failed due to an unexpected error.");
                return;
            }

            if (account == null)
            {
                logger.LogWarning("User failed login attempt.");
                Console.WriteLine("Invalid credentials. Exiting program.");
                return;
            }
            var accountService = new AccountService(jsonStorageService, accounts, account);
            try
            {
                ConsoleMenu.Show(account, accountService);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in console menu");
                Console.WriteLine("An error occurred while interacting with the menu.");
            }
        }
    }
}