namespace PaymentGateway.Api.Models.Data
{
    using System;
    using Newtonsoft.Json;
    using Web;

    /// <summary>
    /// Used to represent card payment data in the database
    /// </summary>
    public class CardPaymentData
    {
        /// <summary>
        /// Id required by  Cosmos 
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Used to place payment
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Expiry Month
        /// </summary>
        public int ExpiryMonth { get; set; }

        /// <summary>
        /// Expiry Year
        /// </summary>
        public int ExpiryYear { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The 3 letter currency code
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The Payment Reference used to capture payment
        /// </summary>
        public Guid PaymentReference { get; set; }

        /// <summary>
        /// The status of the payment 
        /// </summary>
        public PaymentStatusEnum Status { get; set; }

    }
}
