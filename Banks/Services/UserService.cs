using System.Collections.Generic;
using Banks.Entities;

namespace Banks.Services
{
    public class UserService
    {
        private CentralBank _centralBank;
        private string _clientName;

        public UserService(string clientName, CentralBank centralBank)
        {
            _clientName = clientName;
            _centralBank = centralBank;
        }

        private Bank Bank { get; set; }
        private Client Client { get; set; }
        private bool IsLoggedIn => Bank != null && Client != null;

        public string LogInBank(string bankName)
        {
            Bank = _centralBank.FindBank(bankName);
            Client = Bank?.FindClient(_clientName);
            if (!IsLoggedIn)
                return "No such bank or client is not registered";
            return "Logged in " + Bank.Name;
        }

        public string ShowClients()
        {
            if (!IsLoggedIn)
                return "Not logged in";
            string result = string.Empty;
            foreach (Client client in Bank.Clients)
                result += client.Fullname + "\n";

            return result;
        }

        public string ShowAccounts(string clientName)
        {
            if (!IsLoggedIn)
                return "Not logged in";
            Client client = Bank.FindClient(clientName);
            if (client == null)
                return "No such client";
            List<Account> accounts = Bank.GetAccountsOfClient(client);
            string result = string.Empty;
            for (int i = 0; i < accounts.Count; i++)
                result += i + ") " + accounts[i] + ": " + accounts[i].Balance + "\n";
            return result;
        }

        public string ShowAccounts()
        {
            return ShowAccounts(_clientName);
        }

        public string Transfer(int accountNumber, string destinationName, int destinationAccountNumber, double amount)
        {
            if (!IsLoggedIn)
                return "Not logged in";
            Client destinationClient = Bank.FindClient(destinationName);
            if (destinationClient == null)
                return "No such client";
            List<Account> accounts = Bank.GetAccountsOfClient(destinationClient);
            if (destinationAccountNumber >= accounts.Count)
                return "No account with this number";
            return Bank.GetAccountsOfClient(Client)[accountNumber].Transfer(accounts[destinationAccountNumber], amount) ? "Transfer successful" : "Transfer failed";
        }
    }
}