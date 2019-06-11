using Microsoft.Extensions.Configuration;
using QTWalletClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletTransactionListener.Models;
using WalletTransactionListener.Services;

namespace WalletTransactionListener
{
    public class Startup
    {
        private readonly string _coin;
        private readonly WalletEventListener _eventListener;
        private readonly WalletEventHandler _eventHandler;

        public Startup(IConfiguration configuration)
        {
            var walletSettings = configuration
                .GetSection("WalletSettings")
                .Get<WalletSettings>();

            var awsSettings = configuration
                .GetSection("AwsSettings")
                .Get<AwsSettings>();

            _coin = walletSettings.CoinName;

            _eventListener = new WalletEventListener(walletSettings);

            _eventHandler = new WalletEventHandler(awsSettings);
        }

        public void StartEventListener()
        {
            _eventListener.OnNewTransactionReceived = OnNewTransactionReceived;
            Task.Run(_eventListener.RunListener);
        }

        private async void OnNewTransactionReceived(IList<WalletTransaction> transactions)
        {
            var transactions = await _eventHandler.HandleTransactionsReceived(transactions);
            if (transactions > 0)
            {
                var balance = (await _walletClient.SendCommandAsync<decimal>(
                    WalletCommands.GetBalance)).Result;

                await _mainWalletService.UpdateBalanceAsync(_coin, balance);
            }

            Console.WriteLine($"{DateTime.Now}: Found {transactions} new transactions.");
        }
    }
}
