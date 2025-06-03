using System;

namespace OFXnet.Domain
{
    public class HeaderExtract
    {
        public HeaderExtract()
        {

        }
        public HeaderExtract(string status, string language, DateTime serverDate, string bankName, string currency) : this()
        {
            Status = status;
            Language = language;
            ServerDate = serverDate;
            BankName = bankName;
            Currency = currency;
        }
        public string Status { get; set; }
        public string Language { get; set; }
        public DateTime ServerDate { get; set; }
        public string BankName { get; set; }
        public string Currency { get; set; }
    }
}
