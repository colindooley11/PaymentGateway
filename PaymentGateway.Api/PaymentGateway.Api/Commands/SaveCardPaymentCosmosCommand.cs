namespace PaymentGateway.Api.Commands
{
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos;
    using Models.Data;

    public class SaveCardPaymentCosmosCommand : ISaveCardPaymentCommand
    {
        private readonly Container _cardPaymentContainer;

        public SaveCardPaymentCosmosCommand(Container cardPaymentContainer)
        {
            _cardPaymentContainer = cardPaymentContainer;
        }

        public async Task Execute(CardPaymentData cardPaymentData)
        {
            await _cardPaymentContainer.CreateItemAsync(cardPaymentData, new PartitionKey(cardPaymentData.PaymentReference.ToString()));
        }
    }
}