using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFXnet.Domain.Transactions
{

    public class CreditCardTransaction : Transaction
    {
        public CreditCardTransaction(string id, string type, DateTime date, double transactionValue, string description, string cardNumber) : base(id, type, date, transactionValue, description)
        {
            CardNumber = cardNumber;
        }
        public string CardNumber { get; set; }

        public override void ProcessTransaction()
        {
            if (Type == "PURCHASE")
            {
                Console.WriteLine($"Processing credit card purchase of {TransactionValue}.");
            }
            else if (Type == "PAYMENT")
            {
                Console.WriteLine($"Processing credit card payment of {TransactionValue}.");
            }
            else
            {
                Console.WriteLine("Unknown transaction type.");
            }
        }
    }
}
