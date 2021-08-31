namespace PaymentGateway.Api.Models.Web
{
    public class ErrorResponse
    {
        public string Message { get; set; }

        public string PropertyName { get; set; }

        public string ErrorMessage { get; set; }
    }
}