using System;

namespace Banks.Entities
{
    public class DepositAccount : Account
    {
        public DepositAccount(Client client, double suspiciousLimit, TimeSpan term)
            : base(client, suspiciousLimit)
        {
            Term = term;
        }

        private TimeSpan Term { get; set; }

        public void ApplyRate(double rate, TimeSpan term)
        {
            Balance += Balance * rate;
            Term -= term;
        }

        protected override bool CanTakeAmount(double amount)
        {
            return Term.TotalDays <= 0;
        }
    }
}