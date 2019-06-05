using Microsoft.Extensions.Configuration;
using QTWalletClient.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WalletDepositListener.Helpers;
using WalletDepositListener.Models;
using WalletDepositListener.Services;

namespace WalletDepositListener
{
    class Program
    {
        public static IConfigurationRoot Configuration;

        private static DateTime LastTransactionDate;

        private static string CoinName;

        private static TransactionService TransactionService = new TransactionService();

        static void Main(string[] args)
        {
            InitializeConfiguration();

            CoinName = Configuration.GetSection("CoinName").Value;

            var settings = new WalletSettings();
            Configuration.GetSection("WalletSettings").Bind(settings);

            InitializeLastTransactionDate().Wait();

            StartEventListener(settings);

            Console.ReadKey();
        }

        private static async Task InitializeLastTransactionDate()
        {
            var lastTransaction = await TransactionService.GetLastTransaction();
            LastTransactionDate = lastTransaction.DateTime;
        }

        private static void StartEventListener(WalletSettings settings)
        {
            var eventListener = new WalletEventListener(settings);

            eventListener.OnNewTransactionReceived = async (IList<WalletTransaction> transactions)
                => await HandleTransactionsReceived(transactions);
        }

        private static async Task HandleTransactionsReceived(IList<WalletTransaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                var time = new DateTime(transaction.Time);
                if (time < LastTransactionDate)
                {
                    continue;
                }

                var existing = await TransactionService.GetTransactionById(transaction.TxId);
                if (existing != null)
                {
                    continue;
                }

                var transactionEntity = TransactionHelper.ConvertToTransactionEntity(CoinName, transaction);
                await TransactionService.AddTransactionAsync(transactionEntity);
            }
        }

        private static void InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }
    }
}
