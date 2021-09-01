namespace PaymentGateway.Api.Models.Web
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Response when placing a card payment
    /// </summary>
    public class AcquiringBankResponse
    {
        /// <summary>
        /// The status of the acquiring bank response
        /// </summary>
        public PaymentStatusEnum Status { get; set; }

        /// <summary>
        /// The reason for the failure
        /// </summary>
        public string FailureReason { get; set; }

        /// <summary>
        /// The unique reference for the bank
        /// </summary>
        public Guid BankIdentifier { get; set; }
    }
}