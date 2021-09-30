using System.Collections.Generic;
using NUnit.Framework;
using Shops.Entities;
using Shops.Services;

namespace Shops.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(100, 10, 11U, 10U)]
        [TestCase(100, 10, 9U, 10U)]
        [TestCase(99, 10, 11U, 10U)]
        public void BuyItem(float moneyBefore, float productPrice, uint productQuantity, uint productToBuyQuantity)
        {
            var person = new Person("Nikita Kurylev", moneyBefore);
            var shopManager = new ShopManager();
            Shop shop = shopManager.Create("DNS", "Dolgoozernaya Ulitsa, 14к2, St Petersburg");
            Item item = shopManager.RegisterItem("Sony PlayStation 5");

            shop.Restock(item, productQuantity, productPrice);
            if (shop.Sell(item, productToBuyQuantity, person)) {
                Assert.AreEqual(moneyBefore - productPrice * productToBuyQuantity, person.Money);
                Assert.AreEqual(productQuantity - productToBuyQuantity, shop.GetItemInfo(item).Count);
            }
            else
            {
                Assert.AreEqual(moneyBefore, person.Money);
                Assert.AreEqual(productQuantity, shop.GetItemInfo(item).Count);
            }
        }

        [TestCase(100, 200)]
        public void SetPrice(float price1, float price2)
        {
            var shop = new Shop("", "", new BankAccount());
            var item = new Item("");
            shop.SetPrice(item, price1);
            Assert.AreEqual(price1, shop.GetItemInfo(item).Price);
            shop.SetPrice(item, price2);
            Assert.AreEqual(price2, shop.GetItemInfo(item).Price);
        }

        [Test]
        public void GetMin()
        {
            var shopManager = new ShopManager();
            Shop expensiveShop = shopManager.Create("Expensive shop", "");
            Shop cheapShop = shopManager.Create("Cheap shop", "");
            Shop cheapestShop = shopManager.Create("Cheapest shop", "");
            Item item1 = shopManager.RegisterItem("1");
            Item item2 = shopManager.RegisterItem("2");
            expensiveShop.Restock(item1, 5, 10);
            expensiveShop.Restock(item2, 2, 1);
            cheapShop.Restock(item1, 5, 9);
            cheapShop.Restock(item2, 2, 1);
            cheapestShop.Restock(item1, 4, 1);
            cheapestShop.Restock(item2, 2, 1);
            var smallOrder = new Dictionary<Item, uint>() {{item1, 4}, {item2, 2}};
            var bigOrder = new Dictionary<Item, uint>() {{item1, 5}, {item2, 2}};
            var biggestOrder = new Dictionary<Item, uint>() {{item1, 5}, {item2, 3}};
            Assert.AreEqual(shopManager.FindCheapest(smallOrder), cheapestShop);
            Assert.AreEqual(shopManager.FindCheapest(bigOrder), cheapShop);
            Assert.AreEqual(shopManager.FindCheapest(biggestOrder), null);
        }

        [TestCase(100, 10, 11U, 5U)]
        [TestCase(100, 10, 9U, 5U)]
        [TestCase(99, 10, 11U, 5U)]
        public void BuyOrder(float moneyBefore, float productPrice, uint productQuantity, uint productToBuyQuantity)
        {
            var person = new Person("Nikita Kurylev", moneyBefore);
            var shopManager = new ShopManager();
            Shop shop = shopManager.Create("DNS", "Dolgoozernaya Ulitsa, 14к2, St Petersburg");
            Item item1 = shopManager.RegisterItem("Sony PlayStation 5");
            Item item2 = shopManager.RegisterItem("Xbox Series X");

            shop.Restock(item1, productQuantity, productPrice);
            shop.Restock(item2, productQuantity, productPrice);
            var order = new Dictionary<Item, uint>() {{item1, productToBuyQuantity}, {item2, productToBuyQuantity}};
            if (shop.SellOrder(order, person)) {
                Assert.AreEqual(moneyBefore - productPrice * productToBuyQuantity * 2, person.Money);
                Assert.AreEqual(productQuantity - productToBuyQuantity, shop.GetItemInfo(item1).Count);
                Assert.AreEqual(productQuantity - productToBuyQuantity, shop.GetItemInfo(item2).Count);
            }
            else
            {
                Assert.AreEqual(moneyBefore, person.Money);
                Assert.AreEqual(productQuantity, shop.GetItemInfo(item1).Count);
                Assert.AreEqual(productQuantity, shop.GetItemInfo(item2).Count);
            }
        }
    }
}