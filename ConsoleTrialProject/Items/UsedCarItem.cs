using System;
namespace GarageStockApp.Items
{
    public class UsedCarItem : CarItem
    {
        /// <summary>
        /// Initializes a new instance of the UsedCarItem class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="code">Code.</param>
        /// <param name="price">Price.</param>
        /// <param name="quantity">Quantity.</param>
        public UsedCarItem(string name, long code, double price, int quantity = 0) : base(name, code, price, quantity)
        {
            // set discount to 70 for all used items.
            base.SetDiscount(70);
        }

        /// <summary>
        /// Sets the discount, hidden to disable update disount for used car items.
        /// </summary>
        /// <returns><c>true</c>, if discount was set, <c>false</c> otherwise.</returns>
        /// <param name="discountValue">Discount value.</param>
        public new bool SetDiscount(double discountValue)
        {
            return true;
        }
    }
}
