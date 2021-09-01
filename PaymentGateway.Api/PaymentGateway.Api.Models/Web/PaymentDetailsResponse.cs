namespace PaymentGateway.Api.Models.Web
{
    using System;

    /// <summary>
    /// The response containing payment related information
    /// </summary>
    public class PaymentDetailsResponse
    {
        /// <summary>
        /// The masked First 6 and Last 4 of a credit card number/PAN
        /// </summary>
        /// <example>444433******1111</example>
        public string FirstSixLastFour { get; set; }

        /// <summary>
        /// Month Card Expires
        /// </summary>
        /// <example>1</example>
        public int ExpiryMonth { get; set; }

        /// <summary>
        /// 2 digit year card expires
        /// </summary>
        /// <example>22</example>
        public int ExpiryYear { get; set; }

        /// <summary>
        /// The Status of the the Payment placed
        /// </summary>
        /// <example>Success</example>
        public PaymentStatusEnum PaymentStatus { get; set; }

        /// <summary>
        /// The amount the was paid
        /// </summary>
        /// <example>50</example>
        public decimal Amount { get; set; }

        /// <summary>
        /// 3 Letter Currency Code used to place payment
        /// </summary>
        /// <example>GBP</example>
        public string Currency { get; set; }

        /// <summary>
        /// A unique reference used to associate the payment with
        /// </summary>
        /// <example>5fad3780-a1b3-4065-a4f7-89d3ae69154fd</example>
        public Guid PaymentReference { get; set; }

    }
}