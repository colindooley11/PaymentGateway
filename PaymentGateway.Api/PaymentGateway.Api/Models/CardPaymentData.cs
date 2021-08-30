using System;

namespace PaymentGateway.Api.Models
{
    using Newtonsoft.Json;

    public class CardPaymentData
    {

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public string CardNumber { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public int CVV { get; set; }

        public Guid PaymentReference { get; set; }

    }
}
