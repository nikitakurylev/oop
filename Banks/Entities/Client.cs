namespace Banks.Entities
{
    public class Client
    {
        public string Fullname { get; set; }
        public string Address { get; set; }
        public string Passport { get; set; }

        public bool IsSuspicious()
        {
            return string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(Passport);
        }
    }
}