namespace PaymentGateway.Api.Models.Web
{
    using System;

    public class CardPaymentRequest
    {
        public string CardNumber { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public int CVV { get; set; }

        public Guid  PaymentReference { get; set; }

    }
}