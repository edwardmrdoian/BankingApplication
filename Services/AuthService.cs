using BankingApplication.Models;
using BankingApplication.Utils;
using Microsoft.Extensions.Logging;

namespace BankingApplication.Services
{
    public class AuthService(List<Account> accounts, JsonStorageService storageService)
    {
        private readonly List<Account>? accounts = accounts;
        private readonly JsonStorageService storage = storageService;
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

        public Account? Register()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("\n=== USER REGISTRATION ===");

                    Console.Write("First name: ");
                    string? firstName = Console.ReadLine();
                    if (firstName?.ToLower() == "exit") return null;
                    if (string.IsNullOrWhiteSpace(firstName))
                    {
                        Console.WriteLine("\nFirst name is required.\n");
                        continue;
                    }
                    logger.LogInformation("User entered first name: {first}", firstName);

                    Console.Write("Last name: ");
                    string? lastName = Console.ReadLine();
                    if (lastName?.ToLower() == "exit") return null;
                    if (string.IsNullOrWhiteSpace(lastName))
                    {
                        Console.WriteLine("\nLast name is required.\n");
                        continue;
                    }
                    logger.LogInformation("User entered last name: {last}", lastName);

                    Console.Write("Create PIN (4 digits): ");
                    string? pin = Console.ReadLine();
                    if (pin?.ToLower() == "exit") return null;
                    if (string.IsNullOrWhiteSpace(pin))
                    {
                        Console.WriteLine("\nPIN is required.\n");
                        continue;
                    }
                    if (pin.Length != 4 || !pin.All(char.IsDigit))
                    {
                        Console.WriteLine("\nInvalid PIN. Length must be 4 digits.\n");
                        logger.LogWarning("User entered invalid PIN format: {pin}", pin);
                        continue;
                    }
                    logger.LogInformation("User created PIN");

                    string cardNumber = GenerateCardNumber();
                    string cvc = GenerateCvc();
                    string expiration = GenerateExpirationDate();

                    long newId = (accounts == null || accounts.Count == 0)
                        ? 1
                        : accounts.Max(a => a.Id) + 1;

                    var newAccount = new Account(
                        newId,
                        firstName,
                        lastName,
                        new CardDetails(cardNumber, cvc, expiration, pin),
                        new Dictionary<string, decimal>
                        {
                            {"GEL", 0},
                            {"USD", 0},
                            {"EUR", 0}
                        },
                        new List<TransactionHistory>()
                    );

                    accounts.Add(newAccount);
                    storage.SaveAccounts(accounts);

                    Console.WriteLine("\n=== REGISTRATION SUCCESSFUL ===");
                    Console.WriteLine($"User ID: {newAccount.Id}");
                    Console.WriteLine($"Card Number: {newAccount.CardDetails.CardNumber}");
                    Console.WriteLine($"PIN: {newAccount.CardDetails.Pin}");
                    Console.WriteLine($"CVC: {newAccount.CardDetails.Cvc}");
                    Console.WriteLine($"Expiration Date: {newAccount.CardDetails.ExpirationDate}");
                    Console.WriteLine("Please save this information.\n");

                    logger.LogInformation(
                        "New account created: ID={id}, Name={first} {last}",
                        newAccount.Id, newAccount.FirstName, newAccount.LastName
                    );

                    return newAccount;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Unexpected error during registration.");
                    Console.WriteLine("Something went wrong. Please try again.\n");
                    continue;
                }
            }
        }


        private static string GenerateCardNumber()
        {
            Random rnd = new Random();
            int[] digits = new int[16];

            for (int i = 0; i < 15; i++)
                digits[i] = rnd.Next(0, 10);

            // Luhn checksum for card number validity
            int sum = 0;
            for (int i = 0; i < 15; i++)
            {
                int digit = digits[14 - i];
                if (i % 2 == 0)
                {
                    digit *= 2;
                    if (digit > 9) digit -= 9;
                }
                sum += digit;
            }

            digits[15] = (10 - (sum % 10)) % 10;

            string cardNumber = Utils.Utils.CardNumberFormatHelper(string.Concat(digits)) ?? string.Empty;
            return cardNumber;
        }

        private static string GenerateCvc()
        {
            Random rnd = new Random();
            return rnd.Next(100, 999).ToString();
        }
        private static string GenerateExpirationDate()
        {
            var future = DateTime.Now.AddYears(4);
            return future.ToString("MM/yy");
        }
    }
}
