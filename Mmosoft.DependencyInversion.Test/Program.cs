namespace Mmosoft.DependencyInversion.Test
{
    public class Program
    {
        public static void Main()
        {
            // register
            DI.Register<ILogger, ConsoleLogger>(RegisterOption.Cache);
            DI.Register<IContract, SmartContract>(RegisterOption.NoCache);

            // resolve
            IContract value = DI.Resolve<IContract>();
            value.Push();
        }
    }
}
