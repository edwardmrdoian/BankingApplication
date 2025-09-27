namespace BankingApplication.Models
{
    public enum Currency
    {
        GEL,
        USD,
        EUR
    }

    public static class CurrencyUtils
    {
        public static Currency FromString(string stringCurrency, Currency defaultCurrency = Currency.GEL)
        {
            if (Enum.TryParse(stringCurrency, true, out Currency currency))
            {
                return currency;
            }
            return defaultCurrency;
        }
    }
}
