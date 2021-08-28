namespace PaymentGateway.Api.ComponentTests
{
    using Commands;
    using Gateways;
    using InMemory.Stub;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;

    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly ISaveCardPaymentCommand _cardPaymentCommand;

        public CustomWebApplicationFactory(ISaveCardPaymentCommand cardPaymentCommand)
        {
            _cardPaymentCommand = cardPaymentCommand;
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IAcquiringBankGateway, BankGatewaySimulatorStub>();
                services.AddSingleton(this._cardPaymentCommand);
            });
        }
    }
}