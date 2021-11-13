namespace Banks.Entities
{
    public abstract class Account
    {
        private readonly double _suspiciousLimit;

        protected Account(Client client, double suspiciousLimit)
        {
            Client = client;
            _suspiciousLimit = suspiciousLimit;
            Balance = 0;
        }

        public event IAccount.AccountHandler<AccountTransaction> Transferred;

        public Client Client { get; }

        public double Balance { get; protected set; }

        public void Deposit(double amount)
        {
            Balance += amount;
        }

        public bool Withdraw(double amount)
        {
            if (!CanTakeAmount(amount))
                return false;
            if (Client.IsSuspicious() && amount > _suspiciousLimit)
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