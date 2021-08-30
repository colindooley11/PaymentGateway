namespace PaymentGateway.Api.Commands
{
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos;
    using Models;
    using Models.Data;

    public class SaveCardPaymentCommand : ISaveCardPaymentCommand
    {
        private readonly Container _cardPaymentContainer;

        public SaveCardPaymentCommand(Container cardPaymentContainer)
        {
            _cardPaymentContainer = cardPaymentContainer;
        }

        public async Task Execute(CardPaymentData cardPaymentData)
        {
            await _cardPaymentContainer.CreateItemAsync(cardPaymentData, new PartitionKey(cardPaymentData.PaymentReference.ToString()));
        }
    }
}