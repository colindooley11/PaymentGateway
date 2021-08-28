namespace PaymentGateway.Api.Models
{
    public class CardPayment
    {
        public string CardNumber { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public int CVV { get; set; }

    }
}