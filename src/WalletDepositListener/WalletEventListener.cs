using QTWalletClient;
using QTWalletClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WalletDepositListener.Models;

namespace WalletDepositListener
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
            RunListener();
        }

        private async Task RunListener()
        {
            while (true)
            {
                try
                {
                    var transactions = await GetWalletTransactions();

                    var transactionIds = transactions.Select(t => t.TxId).ToList();
                    var difference = transactionIds.Except(_lastTransactionIds, StringComparer.OrdinalIgnoreCase).ToList();

                    if (difference.Any())
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
            }
        }

        private async Task<IList<WalletTransaction>> GetWalletTransactions()
        {
            var response = await _walletClient.SendCommandAsync<IList<WalletTransaction>>(WalletCommands.ListTransactions);
            return response.Result;
        }

    }
}
