namespace OFXnet.Domain
{
    public class Bank
    {
        public Bank(int code, string name)
        {
            Code = code;
            Name = name;
        }
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
