namespace Banks.Entities
{
    public class DebitAccount : Account
    {
        public DebitAccount(Client client, double suspiciousLimit)
            : base(client, suspiciousLimit)
        {
        }

        protected override bool CanTakeAmount(double amount)
        {
            return amount <= Balance;
        }
    }
}