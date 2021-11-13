using System;
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

        public Bank RegisterBank(string name, SortedDictionary<double, double> depositRates, double creditCommissionRate, double suspiciousLimit, double creditLimit, double debitRate)
        {
            var bank = new Bank(name, depositRates, creditCommissionRate, suspiciousLimit, creditLimit, debitRate);
            _banks.Add(bank);
            return bank;
        }

        public Bank FindBank(string name)
        {
            return _banks.Find(bank => bank.Name == name);
        }

        public void ApplyCommission(TimeSpan term)
        {
            foreach (Bank bank in _banks)
                bank.ApplyCommission(term);
        }
    }
}