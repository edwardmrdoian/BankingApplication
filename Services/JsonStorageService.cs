using BankingApplication.Models;
using BankingApplication.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BankingApplication.Services
{
    public class JsonStorageService(string filePath)
    {
        private readonly ILogger<JsonStorageService> logger = AtmLoggerFactory.CreateLogger<JsonStorageService>();

        public List<Account>? GetAccounts()
        {
            try
            {
                List<Account>? accounts = null;
                if (!File.Exists(filePath))
                {
                    logger.LogWarning("File not found: {filePath}, returning empty account list.", filePath);
                    return null;
                }

                string json = File.ReadAllText(filePath);
                if (json != null)
                {
                    accounts = JsonConvert.DeserializeObject<List<Account>>(json);
                    logger.LogInformation("Reading accounts from file: {filePath}", filePath);
                }

                if (accounts == null)
                {
                    logger.LogInformation("No accounts found in file: {filePath}", filePath);
                    return null;
                }

                return accounts;
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to read accounts from JSON file. Exception: {Exception}", ex);
                return null;
            }
        }
        public void SaveAccounts(List<Account> accounts)
        {
            try
            {
                string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
                File.WriteAllText(filePath, json);
                logger.LogInformation("Accounts successfully saved.");
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to save accounts to JSON file. Exception: {Exception}", ex);
            }
        }
    }
}