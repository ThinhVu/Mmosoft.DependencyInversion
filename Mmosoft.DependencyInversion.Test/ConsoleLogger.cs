using System;
namespace Mmosoft.DependencyInversion.Test
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string msg) 
        { 
            Console.WriteLine(msg);
        }
    }
}
