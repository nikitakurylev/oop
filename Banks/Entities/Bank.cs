using System;
using System.Collections.Generic;
using System.Linq;

namespace Banks.Entities
{
    public class Bank
    {
        private readonly float _creditCommissionRate;
        private readonly float _creditLimit;
        private readonly float _suspiciousLimit;
        private readonly float _debitRate;
        private readonly HashSet<Client> _clients;
        private readonly List<IAccount> _accounts;
        private readonly List<AccountTransaction> _transactions;
        private readonly SortedDictionary<double, double> _depositRates;

        public Bank(SortedDictionary<double, double> depositRates, float creditCommissionRate, float suspiciousLimit, float creditLimit, float debitRate)
        {
            _depositRates = depositRates;
            _creditCommissionRate = creditCommissionRate;
            _suspiciousLimit = suspiciousLimit;
            _creditLimit = creditLimit;
            _debitRate = debitRate;
            ClientBuilder = new ClientBuilder();
            _clients = new HashSet<Client>();
            _accounts = new List<IAccount>();
            _transactions = new List<AccountTransaction>();
        }

        private ClientBuilder ClientBuilder { get; set; }

        public Client AddClient(string fullname)
        {
            ClientBuilder.SetFullName(fullname);
            Client client = ClientBuilder.GetClient();
            _clients.Add(client);
            return client;
        }

        public Client AddClient(string fullname, string address, string passport)
        {
            ClientBuilder.SetFullName(fullname);
            ClientBuilder.SetAddress(address);
            ClientBuilder.SetPassport(passport);
            Client client = ClientBuilder.GetClient();
            _clients.Add(client);
            return client;
        }

        public DebitAccount CreateDebitAccount(Client client)
        {
            _clients.Add(client);
            var debitAccount = new DebitAccount(client, _suspiciousLimit);
            _accounts.Add(debitAccount);
            return debitAccount;
        }

        public DepositAccount CreateDepositAccount(Client client, TimeSpan term)
        {
            _clients.Add(client);
            var depositAccount = new DepositAccount(client, _suspiciousLimit, term);
            _accounts.Add(depositAccount);
            return depositAccount;
        }

        public CreditAccount CreateCreditAccount(Client client)
        {
            _clients.Add(client);
            var creditAccount = new CreditAccount(client, _suspiciousLimit, _creditLimit);
            _accounts.Add(creditAccount);
            return creditAccount;
        }

        public void ApplyCommission(TimeSpan timeSpan)
        {
            double timePassed = timeSpan.TotalDays / 365.0;
            foreach (CreditAccount creditAccount in _accounts.OfType<CreditAccount>().Where(c => c.Balance < 0))
                creditAccount.ApplyInterest(_creditCommissionRate * timePassed);
            foreach (DepositAccount depositAccount in _accounts.OfType<DepositAccount>())
                depositAccount.ApplyRate(_depositRates.First(pair => pair.Key <= depositAccount.Balance).Value, timeSpan);
            foreach (DebitAccount debitAccount in _accounts.OfType<DebitAccount>())
                debitAccount.Deposit(debitAccount.Balance * timePassed * _debitRate);
        }

        public void RevertTransaction(AccountTransaction transaction)
        {
            if (!_transactions.Contains(transaction))
                return;
            transaction.Revert();
            _transactions.Remove(transaction);
        }

        public void RevertLastTransaction()
        {
            RevertTransaction(_transactions.Last());
        }

        private void OnTransfer(AccountTransaction accountTransaction)
        {
            _transactions.Add(accountTransaction);
        }
    }
}