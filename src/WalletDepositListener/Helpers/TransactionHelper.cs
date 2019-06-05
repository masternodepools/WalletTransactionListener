using System;
using WalletDepositListener.Models;

namespace WalletDepositListener.Helpers
{
    public static class TransactionHelper
    {
        public static Transaction ConvertToTransactionEntity(string coinName, WalletTransaction transaction)
            => new Transaction
            {
                Amount = transaction.Amount,
                Coin = coinName,
                DateTime = new DateTime(transaction.Time),
                ToAddress = transaction.Address,
                ToUserId = transaction.Account,
                TransactionId = transaction.TxId,
                TransactionStatus = "Pending"
            };
    }
}
