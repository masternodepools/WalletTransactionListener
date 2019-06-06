using System;
using Newtonsoft.Json;

namespace WalletTransactionListener.Models
{
    public class WalletTransaction
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("account", NullValueHandling=NullValueHandling.Ignore)]
        public string Account { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("txid")]
        public string TxId { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("timereceived")]
        public int TimeReceived { get; set; }

        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }

        [JsonProperty("coin")]
        public string CoinName { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        internal void ReplaceAccountPropertyName()
        {
            UserId = Account;
            Account = null;
            if (string.IsNullOrEmpty(UserId))
            {
                UserId = "Unknown";
            }
        }

    }
}
