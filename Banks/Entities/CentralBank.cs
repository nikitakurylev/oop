using System.Collections.Generic;

namespace Banks.Entities
{
    public class CentralBank
    {
        private readonly List<Bank> _banks;

        public CentralBank()
        {
            _banks = new List<Bank>();
        }

        public Bank RegisterBank(SortedDictionary<double, double> depositRates, float creditCommissionRate, float suspiciousLimit, float creditLimit, float debitRate)
        {
            var bank = new Bank(depositRates, creditCommissionRate, suspiciousLimit, creditLimit, debitRate);
            _banks.Add(bank);
            return bank;
        }
    }
}