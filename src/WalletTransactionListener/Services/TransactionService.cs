using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WalletTransactionListener.Models;

namespace WalletTransactionListener.Services
{
    public class TransactionService
    {
        private Table _transactionsTable;

        public TransactionService(AwsSettings settings)
        {
            var region = RegionEndpoint.GetBySystemName(settings.Region);
            var client = new AmazonDynamoDBClient(
                settings.AccessId,
                settings.AccessSecret,
                region);

            _transactionsTable = Table.LoadTable(client, "transactions");
        }

        public async Task AddTransactionAsync(WalletTransaction transaction)
        {
            var item = Document.FromJson(JsonConvert.SerializeObject(transaction));
            await _transactionsTable.PutItemAsync(item);
        }

        public async Task<WalletTransaction> GetTransactionById(string transactionId, string userId)
        {
            var transaction = await _transactionsTable.GetItemAsync(transactionId, userId);
            if (transaction == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<WalletTransaction>(transaction.ToJson());
        }
    }
}
