namespace WalletDepositListener.Models
{
    public class WalletTransaction
    {
        public string Address { get; set; }
        public string Account { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string TxId { get; set; }
        public int Time { get; set; }
        public int TimeReceived { get; set; }
        public int Confirmations { get; set; }
    }
}
