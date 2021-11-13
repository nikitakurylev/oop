using System;
using System.Collections.Generic;
using Banks.Entities;
using NUnit.Framework;

namespace Banks.Tests
{
    public class BanksTest
    {
        private CentralBank _centralBank;
        private Bank _bank;
        private Client _trustedClient, _suspiciousClient;
        private Account _trustedAccount, _suspiciousAccount;
        private const double CreditCommissionRate = 1000, SuspiciousLimit = 5000, CreditLimit = -50000, DebitRate = 0.01;
        private readonly SortedDictionary<double, double> _depositRates = new SortedDictionary<double, double>()
            {{0, 0.03}, {50000, 0.035}, {100000, 0.04}}; 
        [SetUp]
        public void SetUp()
        {
            _centralBank = new CentralBank();
            _bank = _centralBank.RegisterBank("Bank", _depositRates, CreditCommissionRate, SuspiciousLimit, CreditLimit, DebitRate);
            _trustedClient = _bank.AddClient("Ivan Ivanov", "Saint-Petersburg", "1234567890");
            _trustedAccount = _bank.CreateDebitAccount(_trustedClient);
            _suspiciousClient = _bank.AddClient("Sususs Amongus");
            _suspiciousAccount = _bank.CreateDebitAccount(_suspiciousClient);
        }

        [TestCase(1000, 1000)]
        [TestCase(1000, 2000)]
        [TestCase(10000, 10000)]
        public void TransferTest(double initialAmount, double transferAmount)
        {
            _trustedAccount.Deposit(initialAmount);
            Assert.AreEqual(initialAmount >= transferAmount,_trustedAccount.Transfer(_suspiciousAccount, transferAmount));
            Assert.AreEqual(initialAmount >= transferAmount && SuspiciousLimit >= transferAmount,_suspiciousAccount.Transfer(_trustedAccount, transferAmount));
        }

        [TestCase(1000, 1)]
        [TestCase(1000, 30)]
        [TestCase(1000, 365)]
        public void InterestTest(double initialAmount, double days)
        {
            _trustedAccount.Deposit(initialAmount);
            _centralBank.ApplyCommission(TimeSpan.FromDays(days));
            Assert.AreEqual(initialAmount * (1 + DebitRate / 365 * days) , _trustedAccount.Balance);
        }
    }
}