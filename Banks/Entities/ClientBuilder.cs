namespace Banks.Entities
{
    public class ClientBuilder : IClientBuilder
    {
        private Client _client = new Client();

        public ClientBuilder()
        {
            Reset();
        }

        public void SetFullName(string fullname)
        {
            _client.Fullname = fullname;
        }

        public void SetAddress(string address)
        {
            _client.Address = address;
        }

        public void SetPassport(string passport)
        {
            _client.Passport = passport;
        }

        public Client GetClient()
        {
            Client result = _client;
            Reset();
            return result;
        }

        private void Reset()
        {
            _client = new Client();
        }
    }
}