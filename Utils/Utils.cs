using BankingApplication.Models;
using Microsoft.Extensions.Logging;
using System.Text;

namespace BankingApplication.Utils
{
    public static class Utils
    {
        private static readonly ILogger logger = AtmLoggerFactory.CreateLogger<object>();
        public static string? CardNumberFormatHelper(string cardNumber)
        {
            StringBuilder sb = new();
            for (int i = 0; i < 16; i++)
            {
                sb.Append(cardNumber[i]);
                if ((i + 1) % 4 == 0 && i != 15)
                    sb.Append('-');
            }
            return sb.ToString();
        }

        public static string GetFilePathFromProject(string folder, string file)
        {
            string baseDir = AppContext.BaseDirectory;

            string projectRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));

            return Path.Combine(projectRoot, folder, file);
        }

        public static void CheckCurrency(Account account, string currency)
        {
            if (currency.Length != 3)
            {
                Console.WriteLine($"\nInvalid Currency. Length Should be 3.\n");
                logger.LogWarning("User with id {id} entered invalid currency: {wcur}", account.Id, currency);
                return;
            }
        }
    }
}
