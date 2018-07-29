using System;
namespace ConsoleTrialProject.Items
{
    public class FoodItem
    {
        private string name;private double price;

        private double discount;

        private ItemType itemType;

        public FoodItem(String name, double price, ItemType itemType)
        {
            this.name = name;
            this.price = price;
            this.itemType = itemType;
            this.discount = 0.0;
        }

        public void AddToStock(int count)
        {
            throw new NotImplementedException();
        }

        public void ChangePrice(double newPrice)
        {
            this.price = newPrice;
        }

        public double GetCurrentPrice()
        {
            return price * (100 - discount) / 100;
        }

        public double SellItems(int count)
        {
            throw new NotImplementedException();
        }

        public bool SetDiscount(double discountValue)
        {
            if (discountValue > 100 || discountValue < 0)
                return false;            

            this.discount = discountValue;
            return true;
        }
    }
}
