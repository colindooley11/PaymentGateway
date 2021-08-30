namespace PaymentGateway.Api.ComponentTests
{
    using System.Net.Http;
    using Commands;

    public class InProcessApplicationHost : IApplicationHost
    {
        private readonly CustomWebApplicationFactory<TestStartup> _webApplicationFactory;

        public InProcessApplicationHost(ISaveCardPaymentCommand cardPaymentCommand)
        {
            _webApplicationFactory = new CustomWebApplicationFactory<TestStartup>(cardPaymentCommand);
        }
        public HttpClient CreateClient()
        {
            return _webApplicationFactory.CreateClient();
        }
    }
}