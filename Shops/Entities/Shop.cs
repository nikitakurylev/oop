using System.Collections.Generic;

namespace Shops.Entities
{
    public class Shop
    {
        private Dictionary<Item, ItemInfo> _itemStock;
        private BankAccount _bankAccount;
        public Shop(string shopName, string address, BankAccount bankAccount)
        {
            ShopName = shopName;
            Address = address;
            _bankAccount = bankAccount;
            _itemStock = new Dictionary<Item, ItemInfo>();
        }

        private string ShopName { get; }
        private string Address { get; }

        public void Restock(Item item, uint quantity, float price)
        {
            if (!AddItemIfAbsent(item, price, quantity))
                _itemStock[item].Restock(quantity);
        }

        public void SetPrice(Item item, float price)
        {
            if (!AddItemIfAbsent(item, price, 0))
                _itemStock[item].Price = price;
        }

        public bool CanOrder(Dictionary<Item, uint> order)
        {
            foreach ((Item key, uint quantity) in order)
            {
                if (!_itemStock.ContainsKey(key))
                    return false;
                if (_itemStock[key].Count < quantity)
                    return false;
            }

            return true;
        }

        public float GetOrderPrice(Dictionary<Item, uint> order)
        {
            float price = 0;
            foreach ((Item item, uint quantity) in order)
                price += _itemStock[item].GetPrice(quantity);
            return price;
        }

        public bool SellOrder(Dictionary<Item, uint> order, Person person)
        {
            if (!CanOrder(order))
                return false;
            float price = GetOrderPrice(order);
            if (person.Transaction(_bankAccount, price))
            {
                foreach ((Item item, uint quantity) in order)
                {
                    _itemStock[item].Obtain(quantity);
                    person.GiveItem(item, quantity);
                }

                return true;
            }

            return false;
        }

        public bool Sell(Item item, uint quantity, Person person)
        {
            return SellOrder(new Dictionary<Item, uint> { { item, quantity } }, person);
        }

        public ItemInfo GetItemInfo(Item item)
        {
            return _itemStock[item];
        }

        private bool AddItemIfAbsent(Item item, float price, uint quantity)
        {
            if (_itemStock.ContainsKey(item))
                return false;
            _itemStock.Add(item, new ItemInfo(price, quantity));
            return true;
        }
    }
}