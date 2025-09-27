using BankingApplication.Models;
using BankingApplication.Utils;
using Microsoft.Extensions.Logging;

namespace BankingApplication.Services
{
    public class AuthService(List<Account> accounts)
    {
        private readonly List<Account>? accounts = accounts;
        private readonly ILogger<AuthService> logger = AtmLoggerFactory.CreateLogger<AuthService>();

        public Account? Login()
        {
            while (true)
            {
                try
                {
                    Console.Write("\nEnter Card Number: ");
                    string? cardNumber = Console.ReadLine();
                    if (cardNumber?.ToLower() == "exit")
                    {
                        logger.LogInformation("User chose to exit at card step.");
                        return null;
                    }
                    if (string.IsNullOrWhiteSpace(cardNumber))
                    {
                        Console.WriteLine("\nCard number is required.\n");
                        continue;
                    }
                    if (cardNumber.Length != 16 && cardNumber.Length != 19)
                    {
                        Console.WriteLine("\nInvalid Card number.\nEnter (xxxx-xxxx-xxxx-xxxx) or (xxxxxxxxxxxx)\n");
                        logger.LogWarning("User entered invalid Card Number: {cardnumber}", cardNumber);
                        continue;
                    }
                    if (cardNumber.Length == 16)
                    {
                        cardNumber = Utils.Utils.CardNumberFormatHelper(cardNumber);
                    }
                    logger.LogInformation("User entered Card Number {cardnumber}", cardNumber);

                    Console.Write("Enter CVC: ");
                    string? cvc = Console.ReadLine();
                    if (cvc?.ToLower() == "exit")
                    {
                        logger.LogInformation("User chose to exit at cvc step.");
                        return null;
                    }
                    if (string.IsNullOrWhiteSpace(cvc))
                    {
                        Console.WriteLine("\nCVC is required.\n");
                        continue;
                    }
                    if (cvc.Length != 3)
                    {
                        Console.WriteLine($"\nInvalid CVC. Length Should be 3.\n");
                        logger.LogWarning("User entered invalid CVC: {cvc}", cvc);
                        continue;
                    }
                    logger.LogInformation("User entered Cvc {cvc}", cvc);

                    Console.Write("Enter Expiration Date (MM/dd): ");
                    string? dateExpiry = Console.ReadLine();
                    if (dateExpiry?.ToLower() == "exit")
                    {
                        logger.LogInformation("User chose to exit at expiry date step.");
                        return null;
                    }
                    if (dateExpiry is null || !DateTime.TryParse(dateExpiry, out DateTime expiry))
                    {
                        Console.WriteLine("\nInvalid date or date format.\n");
                        logger.LogWarning("User entered invalid date or date format: {date}", dateExpiry);
                        continue;
                    }
                    logger.LogInformation("User entered expiry date {date}", dateExpiry);

                    var account = accounts?.Find(a =>
                        a.CardDetails.CardNumber == cardNumber &&
                        a.CardDetails.Cvc == cvc &&
                        a.CardDetails.ExpirationDate.Equals(expiry.ToString("MM/dd")));

                    if (account == null)
                    {
                        Console.WriteLine("Invalid card data! Try again.");
                        logger.LogWarning("Failed card validation attempt.");
                        continue;
                    }

                    Console.Write("Enter PIN: ");
                    string? pin = Console.ReadLine();
                    if (pin?.ToLower() == "exit")
                    {
                        logger.LogInformation("User chose to exit at PIN step.");
                        return null;
                    }
                    if (string.IsNullOrWhiteSpace(pin))
                    {
                        Console.WriteLine("\nPIN is required.\n");
                        continue;
                    }
                    if (pin.Length != 4)
                    {
                        Console.WriteLine($"\nInvalid PIN. Length Should be 4.\n");
                        logger.LogWarning("User entered invalid PIN: {pin}", pin);
                        continue;
                    }

                    if (account.CardDetails.Pin != pin)
                    {
                        Console.WriteLine("\nInvalid PIN! Try again.\n");
                        logger.LogWarning("Failed PIN attempt for card {Cardnumber}", cardNumber);
                        continue;
                    }

                    Console.WriteLine($"\nWelcome {account.FirstName}!\n");
                    logger.LogInformation("User {firstName} with id {id} successfully logged in.", account.FirstName, account.Id);
                    return account;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Unexpected error during login.");
                    Console.WriteLine("Something went wrong, please try again.");
                    continue;
                }
            }
        }
    }
}
