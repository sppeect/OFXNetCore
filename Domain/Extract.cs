using OFXnet.Domain.Transactions;
using System;
using System.Collections.Generic;

namespace OFXnet.Domain
{
    public class Extract
    {
        public Extract(HeaderExtract header, BankAccount bankAccount, string status, DateTime initialDate, DateTime finalDate)
        {
            Init(header, bankAccount, status);

            InitialDate = initialDate;
            FinalDate = finalDate;
            InitialBalance = 0;  
            FinalBalance = 0;
        }

        public Extract(HeaderExtract header, BankAccount bankAccount, string status)
        {
            Init(header, bankAccount, status);
            InitialBalance = 0;  
            FinalBalance = 0;
        }

        public HeaderExtract Header { get; set; }
        public BankAccount BankAccount { get; set; }
        public string Status { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public IList<Transaction> Transactions { get; set; }
        public IList<string> ImportingErrors { get; private set; }
        public double InitialBalance { get; set; }
        public double FinalBalance { get; set; }

        private void Init(HeaderExtract header, BankAccount bankAccount, string status)
        {
            Header = header;
            BankAccount = bankAccount;
            Status = status;
            Transactions = new List<Transaction>();
            ImportingErrors = new List<string>();
        }

        public void AddTransaction(Transaction transaction)
        {
            if (Transactions == null)
                Transactions = new List<Transaction>();

            Transactions.Add(transaction);
        }

        public void CalculateBalances()
        {
            if (Transactions.Count > 0)
            {
                InitialBalance = Transactions[0].TransactionValue; 
            }

            FinalBalance = InitialBalance;

            foreach (var transaction in Transactions)
            {
                FinalBalance += transaction.TransactionValue;
            }
        }
    }

}
