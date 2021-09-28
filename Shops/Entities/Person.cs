using System.Collections.Generic;

namespace Shops.Entities
{
    public class Person
    {
        private Dictionary<Item, uint> _items;

        public Person(string name, Cheque cheque)
        {
            Name = name;
            Purse = cheque;
            _items = new Dictionary<Item, uint>();
        }

        public Person(string name, float money)
        {
            Name = name;
            _items = new Dictionary<Item, uint>();
            Purse = new Cheque(money);
        }

        public float Money => Purse.Value;
        private string Name { get; }
        private Cheque Purse { get; }

        public void GiveItem(Item item, uint quantity)
        {
            if (_items.ContainsKey(item))
                _items[item] += quantity;
            else
                _items.Add(item, quantity);
        }

        public bool Transaction(Cheque cheque, float amount)
        {
            return Purse.Transaction(cheque, amount);
        }
    }
}