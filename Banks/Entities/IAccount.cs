namespace Banks.Entities
{
    public interface IAccount
    {
        public delegate void AccountHandler<in T>(T argument);
        public event AccountHandler<AccountTransaction> Transferred;
        public void Deposit(double amount);
        public bool Withdraw(double amount);
        public bool Transfer(Account account, double amount);
    }
}