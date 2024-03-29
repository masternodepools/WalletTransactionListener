﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletTransactionListener.Models;

namespace WalletTransactionListener.Services
{
    public class WalletEventHandler
    {
        private MainWalletService _mainWalletService;
        private TransactionService _transactionService;

        public WalletEventHandler(AwsSettings settings)
        {
            _mainWalletService = new MainWalletService(settings);
            _transactionService = new TransactionService(settings);
        }

        public async Task<int> HandleTransactionsReceived(IList<WalletTransaction> transactions)
        {
            var newTransactions = 0;

            foreach (var transaction in transactions)
            {
                transaction.ReplaceAccountPropertyName();
                var time = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(transaction.Time).ToLocalTime();
                var existing = await _transactionService.GetTransactionById(transaction.TxId, transaction.UserId);
                if (existing != null)
                {
                    continue;
                }

                await _transactionService.AddTransactionAsync(transaction);
                newTransactions++;
            }

            return newTransactions;
        }
    }
}
