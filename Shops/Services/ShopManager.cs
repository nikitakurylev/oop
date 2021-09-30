using System.Collections.Generic;
using Shops.Entities;

namespace Shops.Services
{
    public class ShopManager
    {
        private List<Shop> _shops;
        private BankAccount _bankAccount;
        public ShopManager()
        {
            _bankAccount = new BankAccount();
            _shops = new List<Shop>();
            ItemRegister = new HashSet<Item>();
        }

        private HashSet<Item> ItemRegister { get; }

        public Shop Create(string name, string address)
        {
            var shop = new Shop(name, address, _bankAccount);
            _shops.Add(shop);
            return shop;
        }

        public Item RegisterItem(string name)
        {
            var item = new Item(name);
            return item;
        }

        public Shop FindCheapest(Dictionary<Item, uint> order)
        {
            List<Shop> canOrder = _shops.FindAll(t => t.CanOrder(order));
            if (canOrder.Count == 0)
                return null;
            Shop result = canOrder[0];
            float minPrice = result.GetOrderPrice(order);
            foreach (Shop shop in canOrder)
            {
                if (shop.GetOrderPrice(order) < minPrice)
                    result = shop;
            }

            return result;
        }
    }
}