using QTWalletClient;
using QTWalletClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WalletDepositListener.Models;

namespace WalletDepositListener.Services
{
    public class WalletEventListener
    {
        public Action<IList<WalletTransaction>> OnNewTransactionReceived { get; set; }
        private IList<string> _lastTransactionIds;
        private WalletClient _walletClient;

        public WalletEventListener(WalletSettings settings)
        {
            _walletClient = new WalletClient(settings);
            _lastTransactionIds = new List<string>();
        }

        public async void RunListener()
        {
            try
            {
                var transactions = await GetWalletTransactions();
                var transactionIds = transactions.Select(t => t.TxId).ToList();

                if (ContainsNewTransactions(transactionIds))
                {
                    OnNewTransactionReceived(transactions.ToList());
                }

                _lastTransactionIds = transactionIds;

                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(1000);
            }

            RunListener();
        }

        private bool ContainsNewTransactions(IList<string> transactionIds)
        {
            var difference = transactionIds.Except(_lastTransactionIds, StringComparer.OrdinalIgnoreCase).ToList();
            return difference.Any();
        }

        private async Task<IList<WalletTransaction>> GetWalletTransactions()
        {
            var response = await _walletClient.SendCommandAsync<IList<WalletTransaction>>(WalletCommands.ListTransactions);
            return response.Result;
        }

    }
}
