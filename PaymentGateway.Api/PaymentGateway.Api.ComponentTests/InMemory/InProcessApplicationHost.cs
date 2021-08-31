namespace PaymentGateway.Api.ComponentTests.InMemory
{
    using System.Net.Http;
    using BankSimulator;
    using Clients;
    using Commands;
    using Gateways;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Http;

    public class InProcessApplicationHost : IApplicationHost
    {
        private readonly CustomWebApplicationFactory<TestStartup> _webApplicationFactory;

        public InProcessApplicationHost(ISaveCardPaymentCommand cardPaymentCommand, AcquiringBankGatewaySpyDelegatingHandler handler)
        {
            _webApplicationFactory = new CustomWebApplicationFactory<TestStartup>(collection =>
            {
                collection.AddSingleton(cardPaymentCommand);
                collection.AddHttpClient<AcquiringBankClient>()
                    .ConfigureHttpMessageHandlerBuilder(builder =>
                    {
                        builder.AdditionalHandlers.Clear();
                        builder.AdditionalHandlers.Add(handler);
                        builder.AdditionalHandlers.Add(new AcquiringBankGatewayStubDelegatingHandler());
                    });


               
            });
        }
        public HttpClient CreateClient()
        {
            return _webApplicationFactory.CreateClient();
        }
    }
}