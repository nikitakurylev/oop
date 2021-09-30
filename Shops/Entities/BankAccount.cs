namespace Shops.Entities
{
    public class BankAccount
    {
        private float _value;
        public BankAccount(float value = 0f)
        {
            _value = value;
        }

        public float Value => _value;

        public void Deposit(float amount)
        {
            _value += amount;
        }

        public bool Transaction(BankAccount bankAccount, float amount)
        {
            if (Value < amount)
                return false;
            _value -= amount;
            bankAccount.Deposit(amount);
            return true;
        }
    }
}