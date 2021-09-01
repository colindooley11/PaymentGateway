namespace PaymentGateway.Api.Models.Web
{
    /// <summary>
    /// The model used to represent errors
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// An Error message detailing a validation issue
        /// </summary>
        /// <example>Please provide a credit card number</example>
        public string Message { get; set; }

        /// <summary>
        /// The name of the property with which there is a fault
        /// </summary>
        /// <example>CardNumber</example>
        public string PropertyName { get; set; }

    }
}