namespace Banks.Entities
{
    public abstract class Account : IAccount
    {
        private readonly Client _client;
        private readonly double _suspiciousLimit;

        protected Account(Client client, double suspiciousLimit)
        {
            _client = client;
            _suspiciousLimit = suspiciousLimit;
            Balance = 0;
        }

        public event IAccount.AccountHandler<AccountTransaction> Transferred;

        public double Balance { get; protected set; }

        public void Deposit(double amount)
        {
            Balance += amount;
        }

        public bool Withdraw(double amount)
        {
            if (!CanTakeAmount(amount))
                return false;
            if (_client.IsSuspicious() && amount > _suspiciousLimit)
                return false;
            Balance -= amount;
            return true;
        }

        public bool Transfer(Account account, double amount)
        {
            if (!Withdraw(amount))
                return false;
            account.Deposit(amount);
            Transferred?.Invoke(new AccountTransaction(this, account, amount));
            return true;
        }

        protected abstract bool CanTakeAmount(double amount);
    }
}