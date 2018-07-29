using System;
namespace GarageStockApp.Items
{
    public interface Item
    {
        void ChangePrice(double newPrice);

        void AddToStock(int count);

        bool SetDiscount(double discountValue);

        double GetCurrentPriceAfterDiscount();

        double SellItems(int count);

        double GetCurrentValue();
    }
}
