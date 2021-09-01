namespace PaymentGateway.Api.ComponentTests.BankSimulator
{
    public class BankSimulatorScenarioBuilder
    {
        private bool failure; 
       
        public BankSimulatorScenarioBuilder WithFailure()
        {
            failure = true;
            return this; 
        }

        public BankSimulatorDelegatingHandlerSpy Build()
        {
            return new BankSimulatorDelegatingHandlerSpy(failure); 
        }
    }
}