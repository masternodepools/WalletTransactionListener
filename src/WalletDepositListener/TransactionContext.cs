using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;

namespace WalletDepositListener
{
    public class TransactionContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Program.Configuration.GetConnectionString("Transactions");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    public class Transaction
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public string ToAddress { get; set; }
        public string ToUserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
        public string Coin { get; set; }
        public string TransactionStatus { get; set; }
    }
}
