namespace Banks.Entities
{
    public class AccountTransaction
    {
        public AccountTransaction(Account from, Account to, double amount)
        {
            From = from;
            To = to;
            Amount = amount;
        }

        private Account From { get; }
        private Account To { get; }
        private double Amount { get; }

        public void Revert()
        {
            To.Withdraw(Amount);
            From.Deposit(Amount);
        }
    }
}