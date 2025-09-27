namespace BankingApplication.Models
{
    public class CardDetails(string cardNumber, string cvc, string expirationDate, string pin)
    {
        public string CardNumber { get; private set; } = cardNumber;
        public string Cvc { get; private set; } = cvc;
        public string ExpirationDate { get; private set; } = expirationDate;
        public string Pin { get; set; } = pin;

        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(new
            {
                CardNumber,
                Cvc,
                ExpirationDate,
                Pin
            });
        }
    }
}
