namespace Shops.Entities
{
    public class Cheque
    {
        private float _value;
        public Cheque(float value = 0f)
        {
            _value = value;
        }

        public float Value => _value;

        public void Deposit(float amount)
        {
            _value += amount;
        }

        public bool Transaction(Cheque cheque, float amount)
        {
            if (Value < amount)
                return false;
            _value -= amount;
            cheque.Deposit(amount);
            return true;
        }
    }
}