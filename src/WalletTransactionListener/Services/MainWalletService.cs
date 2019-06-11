using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WalletTransactionListener.Models;

namespace WalletTransactionListener.Services
{
    public class MainWalletService
    {
        private Table _mainWalletsTable;

        public MainWalletService(AwsSettings settings)
        {
            var region = RegionEndpoint.GetBySystemName(settings.Region);
            var client = new AmazonDynamoDBClient(
                settings.AccessId,
                settings.AccessSecret,
                region);

            _mainWalletsTable = Table.LoadTable(client, "main-wallets");
        }

        public async Task UpdateBalanceAsync(string coin, decimal balance)
        {
            var item = Document.FromJson(JsonConvert.SerializeObject(new MainWallet
            {
                Coin = coin,
                Balance = balance
            }));

            await _mainWalletsTable.PutItemAsync(item);
        }
    }
}
