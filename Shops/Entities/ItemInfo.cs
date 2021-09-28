namespace Shops.Entities
{
    public class ItemInfo
    {
        private uint _count;

        public ItemInfo(float price, uint count = 0)
        {
            Price = price;
            _count = count;
        }

        public float Price { get; set; }

        public uint Count => _count;

        public void Restock(uint quantity)
        {
            _count += quantity;
        }

        public float GetPrice(uint quantity = 1)
        {
            return Price * quantity;
        }

        public void Obtain(uint quantity)
        {
            _count -= quantity;
        }
    }
}