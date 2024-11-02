using OFXnet.Domain.Transactions;
using System;
using System.Text.RegularExpressions;

namespace OFXnet.Core.Domain.Transactions
{
    public class BankTransaction : Transaction
    {
        public BankTransaction(string id, string type, DateTime date, double transactionValue, string description, long checksum)
            : base(id, type, date, transactionValue, description)
        {
            Checksum = checksum;

        }

        public long Checksum { get; set; }

        public override void ProcessTransaction()
        {
            if (Type == "CREDIT")
            {
                Console.WriteLine($"Processing credit transaction of {TransactionValue}.");
            }
            else if (Type == "DEBIT")
            {
                Console.WriteLine($"Processing debit transaction of {TransactionValue}.");
            }
            else
            {
                Console.WriteLine("Unknown transaction type.");
            }
        }


    }
}
