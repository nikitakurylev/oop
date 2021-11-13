namespace Banks.Entities
{
    public class CreditAccount : Account
    {
        private readonly double _creditLimit;

        public CreditAccount(Client client, double suspiciousLimit, double creditLimit)
            : base(client, suspiciousLimit)
        {
            _creditLimit = creditLimit;
        }

        public void ApplyInterest(double amount)
        {
            Balance -= amount;
        }

        protected override bool CanTakeAmount(double amount)
        {
            return Balance - amount >= _creditLimit;
        }
    }
}