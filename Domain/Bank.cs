namespace OFXnet.Domain
{
    public class Bank
    {
        public Bank()
        {
        }

        public Bank(int code, string name) : this()
        {
            Code = code;
            Name = name;
        }
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
