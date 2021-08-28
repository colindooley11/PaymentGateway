namespace PaymentGateway.Api.Commands
{
    using System.Threading.Tasks;
    using Models;

    public class SaveCardPaymentCommandNoOp : ISaveCardPaymentCommand
    {
        private readonly string _connectionstring;

        public SaveCardPaymentCommandNoOp(string connectionstring)
        {
            _connectionstring = connectionstring;
        }

        public async Task Execute(CardPayment cardPayment)
        {
            await Task.CompletedTask;
        }
    }
}