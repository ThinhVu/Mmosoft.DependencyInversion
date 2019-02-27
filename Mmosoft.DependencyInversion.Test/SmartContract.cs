namespace Mmosoft.DependencyInversion.Test
{
    public class SmartContract : IContract
    {
        private ILogger logger;

        public SmartContract(ILogger logger)
        {
            this.logger = logger;
        }

        public void Push()
        {
            this.logger.Log("Push the smart contract into new block");
        }
    }

}
