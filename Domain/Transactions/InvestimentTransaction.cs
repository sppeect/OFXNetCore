using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFXnet.Domain.Transactions
{
    public class InvestmentTransaction : Transaction
    {


        public InvestmentTransaction(string id, string type, DateTime date, double transactionValue, string description, string investmentType, double quantity, double price) : base(id, type, date, transactionValue, description)
        {
            InvestmentType = investmentType;
            Quantity = quantity;
            Price = price;
        }
        public string InvestmentType { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }

        public override void ProcessTransaction()
        {
            if (Type == "BUY")
            {
                Console.WriteLine($"Processing investment buy of {Quantity} {InvestmentType} at {Price} per unit.");
            }
            else if (Type == "SELL")
            {
                Console.WriteLine($"Processing investment sell of {Quantity} {InvestmentType} at {Price} per unit.");
            }
            else
            {
                Console.WriteLine("Unknown investment transaction type.");
            }
        }
    }

}
