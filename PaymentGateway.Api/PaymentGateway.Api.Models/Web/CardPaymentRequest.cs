namespace PaymentGateway.Api.Models.Web
{
    using System;

    /// <summary>
    /// Request used to place a card payment
    /// </summary>
    /// <remarks>Please use 4444333322221111  </remarks>
    public class CardPaymentRequest
    {
        /// <summary>
        /// Credit Card Number
        /// </summary>
        /// <example>4444333322221111</example>
        public string CardNumber { get; set; }

        /// <summary>
        /// Month Card Expires
        /// </summary>
        /// <example>1</example>
        public int ExpiryMonth { get; set; }

        /// <summary>
        /// 2 digit year card expires
        /// </summary>
        /// <example>25</example>
        public int ExpiryYear { get; set; }

        /// <summary>
        /// The amount to pay 
        /// </summary>
        /// <example>50</example>
        public decimal Amount { get; set; }

        /// <summary>
        /// 3 Letter Currency Code
        /// </summary>
        /// <example>GBP</example>
        public string Currency { get; set; }

        /// <summary>
        /// 3 or 4 digit CVV/Security code
        /// </summary>
        /// <example>555</example>
        public int CVV { get; set; }

        /// <summary>
        /// A unique reference  to associate the payment with and can be used to search upon
        /// </summary>
        /// <example>43c2fc90-2444-4cf0-8e39-3eb806911d85</example>
        public Guid  PaymentReference { get; set; }

    }
}