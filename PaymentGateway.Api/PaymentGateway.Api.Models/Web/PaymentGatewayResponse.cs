namespace PaymentGateway.Api.Models.Web
{
    /// <summary>
    /// Response when placing a card payment
    /// </summary>
    public class PaymentGatewayResponse
    {
        /// <summary>
        /// The Status of the the Payment placed
        /// </summary>
        /// <example>Success</example>
        public PaymentStatusEnum Status { get; set; }
    }
}