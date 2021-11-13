namespace Banks.Entities
{
    public interface IClientBuilder
    {
        public void SetFullName(string fullname);
        public void SetAddress(string address);
        public void SetPassport(string passport);
    }
}