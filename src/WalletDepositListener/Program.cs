using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace WalletTransactionListener
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = GetConfiguration();

            var startup = new Startup(config);

            startup.StartEventListener();

            Console.ReadKey();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
