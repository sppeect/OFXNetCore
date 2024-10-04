using System;
using System.Text.RegularExpressions;

namespace OFXnet.Domain.Transactions
{
    public abstract class Transaction
    {
        public Transaction(string id, string type, DateTime date, double transactionValue, string description)
        {
            Id = id;
            Type = type;
            Date = date;
            TransactionValue = transactionValue;
            Description = description;
        }

        public string Id { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public double TransactionValue { get; set; }
        public string Description { get; set; }
        public string SenderDocument { get; set; }
        public abstract void ProcessTransaction();
        public string ExtractDocumentFromDescription(string description)
        {
            var legalRegex = new Regex(@"\b\d{14}\b");
            var naturalRegex = new Regex(@"\b\d{11}\b");

            var matchLegal = legalRegex.Match(description);
            var matchNatural = naturalRegex.Match(description);

            if (matchLegal.Success)
            {
                return matchLegal.Value;
            }
            else if (matchNatural.Success)
            {
                return matchNatural.Value;
            }

            return string.Empty;
        }
    }
}
