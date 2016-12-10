using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace LogMaster4Net.MasterServer
{
    public class Program
    {
        private static ILogger _logger;

        public static void Main(string[] args)
        {
            var factory = new LoggerFactory();
            factory.AddConsole();
            _logger = factory.CreateLogger("MasterServer");
            _logger.LogInformation("Hello World!");
        }
    }
}
