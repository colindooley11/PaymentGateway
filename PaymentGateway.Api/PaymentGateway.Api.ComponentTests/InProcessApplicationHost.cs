namespace PaymentGateway.Api.ComponentTests
{
    using System.Net.Http;
    using Commands;
    using Microsoft.AspNetCore.Mvc.Testing;

    public class InProcessApplicationHost : IApplicationHost
    {
        private readonly CustomWebApplicationFactory<Startup> _webApplicationFactory;

        public InProcessApplicationHost(ISaveCardPaymentCommand cardPaymentCommand)
        {
            _webApplicationFactory = new CustomWebApplicationFactory<Startup>(cardPaymentCommand);
        }
        public HttpClient CreateClient()
        {
            return _webApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }
    }
}