using System.Collections.Generic;

namespace Shops.Entities
{
    public class Person
    {
        private Dictionary<Item, uint> _items;

        public Person(string name, BankAccount bankAccount)
        {
            Name = name;
            Purse = bankAccount;
            _items = new Dictionary<Item, uint>();
        }

        public Person(string name, float money)
        {
            Name = name;
            _items = new Dictionary<Item, uint>();
            Purse = new BankAccount(money);
        }

        public float Money => Purse.Value;
        private string Name { get; }
        private BankAccount Purse { get; }

        public void GiveItem(Item item, uint quantity)
        {
            if (_items.ContainsKey(item))
                _items[item] += quantity;
            else
                _items.Add(item, quantity);
        }

        public bool Transaction(BankAccount bankAccount, float amount)
        {
            return Purse.Transaction(bankAccount, amount);
        }
    }
}