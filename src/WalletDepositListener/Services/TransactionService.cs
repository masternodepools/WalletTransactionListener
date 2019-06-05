using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace WalletDepositListener.Services
{
    public class TransactionService
    {
        public async Task AddTransactionAsync(Transaction transaction)
        {
            using (var context = new TransactionContext())
            {
                context.Transactions.Add(transaction);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Transaction> GetLastTransaction()
        {
            using (var context = new TransactionContext())
            {
                return await context.Transactions
                    .OrderByDescending(d => d.Id)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<Transaction> GetTransactionById(string transactionId)
        {
            using (var context = new TransactionContext())
            {
                return await context.Transactions
                    .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
            }
        }
    }
}
