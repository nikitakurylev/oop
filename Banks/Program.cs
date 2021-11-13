using System;
using System.Collections.Generic;
using Banks.Entities;
using Banks.Services;

namespace Banks
{
    internal static class Program
    {
        private const double CreditCommissionRate = 1000;
        private const double SuspiciousLimit = 5000;
        private const double CreditLimit = -50000;
        private const double DebitRate = 0.01;
        private static readonly SortedDictionary<double, double> DepositRates = new SortedDictionary<double, double>()
            { { 0, 0.03 }, { 50000, 0.035 }, { 100000, 0.04 } };
        private static CentralBank _centralBank;
        private static Bank _bank;
        private static Client _trustedClient;
        private static Client _suspiciousClient;
        private static Account _trustedAccount;
        private static Account _suspiciousAccount;

        private static void Main()
        {
            _centralBank = new CentralBank();
            _bank = _centralBank.RegisterBank("Bank", DepositRates, CreditCommissionRate, SuspiciousLimit, CreditLimit, DebitRate);
            _trustedClient = _bank.AddClient("Ivan", "Saint-Petersburg", "1234567890");
            _trustedAccount = _bank.CreateDebitAccount(_trustedClient);
            _trustedAccount.Deposit(1000);
            _suspiciousClient = _bank.AddClient("Nikita");
            _suspiciousAccount = _bank.CreateDebitAccount(_suspiciousClient);

            Console.WriteLine("Enter your name");
            string name = Console.ReadLine();
            var consoleUi = new ConsoleUi(new UserService(name, _centralBank));

            while (!consoleUi.IsFinished)
            {
                consoleUi.WaitForInput();
            }
        }
    }
}
