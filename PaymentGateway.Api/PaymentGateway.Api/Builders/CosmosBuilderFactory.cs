namespace PaymentGateway.Api.Builders
{
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Cosmos.Fluent;

    public class CosmosBuilderFactory
    {
        public Container Build(string accountEndpoint, string authKeyOrResourceToken, string cardpaymentscontainer, string paymentgatewayApi)
        {
            var cosmosClient = new CosmosClientBuilder(accountEndpoint, authKeyOrResourceToken)
                .Build();
            var database = cosmosClient.CreateDatabaseIfNotExistsAsync(paymentgatewayApi).Result.Database;
            var container = database.CreateContainerIfNotExistsAsync(cardpaymentscontainer, "/PaymentReference").Result.Container;
            return container;
        }
    }
}
