using BankingApplication.Utils;
using Microsoft.Extensions.Logging;

namespace BankingApplication.Models
{
    public static class ConversionRates
    {
        private static readonly ILogger logger = AtmLoggerFactory.CreateLogger<object>();
        private static readonly Dictionary<(Currency From, Currency To), decimal> rates =
         new()
         {
                { (Currency.GEL, Currency.USD), 0.36m },
                { (Currency.USD, Currency.GEL), 2.75m },
                { (Currency.GEL, Currency.EUR), 0.33m },
                { (Currency.EUR, Currency.GEL), 3.0m },
                { (Currency.USD, Currency.EUR), 0.92m },
                { (Currency.EUR, Currency.USD), 1.09m }
         };

        public static decimal GetRate(Currency from, Currency to)
        {
            try
            {
                if (from == to)
                {
                    logger.LogInformation("Conversion {From}=>{To}, returning 1.0 (same currency).", from, to);
                    return 1.0m;
                }

                if (rates.TryGetValue((from, to), out var rate))
                {
                    logger.LogInformation("Conversion rate found: {From}=>{To} = {Rate}", from, to, rate);
                    return rate;
                }

                logger.LogWarning("Conversion rate missing: {From}=>{To}", from, to);
                throw new InvalidOperationException($"Conversion rate not found: {from} => {to}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving conversion rate for {From}=>{To}", from, to);
                throw;
            }
        }
    }
}
