using System;
using System.Collections.Generic;
using System.Linq;

namespace Banks.Entities
{
    public class Bank
    {
        private readonly double _creditCommissionRate;
        private readonly double _creditLimit;
        private readonly double _suspiciousLimit;
        private readonly double _debitRate;
        private readonly List<Account> _accounts;
        private readonly List<AccountTransaction> _transactions;
        private readonly SortedDictionary<double, double> _depositRates;

        public Bank(string name, SortedDictionary<double, double> depositRates, double creditCommissionRate, double suspiciousLimit, double creditLimit, double debitRate)
        {
            Name = name;
            _depositRates = depositRates;
            _creditCommissionRate = creditCommissionRate;
            _suspiciousLimit = suspiciousLimit;
            _creditLimit = creditLimit;
            _debitRate = debitRate;
            ClientBuilder = new ClientBuilder();
            Clients = new HashSet<Client>();
            _accounts = new List<Account>();
            _transactions = new List<AccountTransaction>();
        }

        public HashSet<Client> Clients { get; }

        public string Name { get; }

        private ClientBuilder ClientBuilder { get; set; }

        public Client AddClient(string fullname)
        {
            ClientBuilder.SetFullName(fullname);
            Client client = ClientBuilder.GetClient();
            Clients.Add(client);
            return client;
        }

        public Client AddClient(string fullname, string address, string passport)
        {
            ClientBuilder.SetFullName(fullname);
            ClientBuilder.SetAddress(address);
            ClientBuilder.SetPassport(passport);
            Client client = ClientBuilder.GetClient();
            Clients.Add(client);
            return client;
        }

        public DebitAccount CreateDebitAccount(Client client)
        {
            Clients.Add(client);
            var debitAccount = new DebitAccount(client, _suspiciousLimit);
            debitAccount.Transferred += OnTransfer;
            _accounts.Add(debitAccount);
            return debitAccount;
        }

        public DepositAccount CreateDepositAccount(Client client, TimeSpan term)
        {
            Clients.Add(client);
            var depositAccount = new DepositAccount(client, _suspiciousLimit, term);
            depositAccount.Transferred += OnTransfer;
            _accounts.Add(depositAccount);
            return depositAccount;
        }

        public CreditAccount CreateCreditAccount(Client client)
        {
            Clients.Add(client);
            var creditAccount = new CreditAccount(client, _suspiciousLimit, _creditLimit);
            creditAccount.Transferred += OnTransfer;
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

        public Client FindClient(string name)
        {
            return Clients.ToList().Find(client => client.Fullname == name);
        }

        public bool HasClient(Client client)
        {
            return Clients.Contains(client);
        }

        public List<Account> GetAccountsOfClient(Client client)
        {
            return _accounts.FindAll(account => account.Client == client);
        }

        private void OnTransfer(AccountTransaction accountTransaction)
        {
            _transactions.Add(accountTransaction);
        }
    }
}