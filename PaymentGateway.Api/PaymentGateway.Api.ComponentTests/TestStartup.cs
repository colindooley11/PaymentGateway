namespace PaymentGateway.Api.ComponentTests
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void BuildCosmosFactory(IServiceCollection services, string accountEndpoint, string authKeyOrResourceToken,
            string cardpaymentscontainer)
        {
            // don't do anything in memory 
        }
    }
}
