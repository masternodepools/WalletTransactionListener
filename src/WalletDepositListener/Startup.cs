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
        private WalletEventListener _eventListener;
        private WalletEventHandler _eventHandler;
        
        public Startup(IConfiguration configuration)
        {
            var walletSettings = configuration
                .GetSection("WalletSettings")
                .Get<WalletSettings>();

            var awsSettings = configuration
                .GetSection("AwsSettings")
                .Get<AwsSettings>();

            _eventListener = new WalletEventListener(walletSettings);

            _eventHandler = new WalletEventHandler(awsSettings);
        }

        public void StartEventListener()
        {
            _eventListener.OnNewTransactionReceived = OnNewTransactionReceived;
            Task.Run(_eventListener.RunListener);
        }

        private async void OnNewTransactionReceived(IList<WalletTransaction> transactions)
            => await _eventHandler.HandleTransactionsReceived(transactions);
    }
}
