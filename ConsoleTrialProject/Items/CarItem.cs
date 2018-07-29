using System;
using CsvHelper.Configuration;

namespace GarageStockApp.Items
{
    public class CarItem : Item
    {
        /// <summary>
        /// The name.
        /// </summary>
        private string name;

        /// <summary>
        /// The barcode.
        /// </summary>
        private long barcode;

        /// <summary>
        /// The price.
        /// </summary>
        private double price;

        /// <summary>
        /// The discount.
        /// </summary>
        private double discount;

        /// <summary>
        /// The quantity.
        /// </summary>
        private int quantity;

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>The code.</value>
        public long Barcode
        {
            get
            {
                return barcode;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public int Quantity
        {
            get
            {
                return quantity;
            }
        }

        /// <summary>
        /// Gets the price.
        /// </summary>
        /// <value>The price.</value>
        public double Price
        {
            get
            {
                return price;
            }
        }

        /// <summary>
        /// Gets the discount.
        /// </summary>
        /// <value>The discount.</value>
        public double Discount
        {
            get
            {
                return discount;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ConsoleTrialProject.Items.CarItem"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="code">Code.</param>
        /// <param name="price">Price.</param>
        /// <param name="quantity">Quantity.</param>
        public CarItem(string name, long code, double price, int quantity)
        {
            this.name = name;
            this.barcode = code;
            this.price = price;
            this.quantity = quantity;
        }

        /// <summary>
        /// Changes the price.
        /// </summary>
        /// <param name="newPrice">New price.</param>
        public void ChangePrice(double newPrice)
        {
            this.price = newPrice;
        }

        /// <summary>
        /// Gets the current price after discount.
        /// </summary>
        /// <returns>The current price.</returns>
        public double GetCurrentPriceAfterDiscount()
        {
            return this.price * (100 - this.discount) / 100;
        }

        /// <summary>
        /// Sets the discount.
        /// </summary>
        /// <returns><c>true</c>, if discount was set, <c>false</c> otherwise.</returns>
        /// <param name="discountValue">Discount value.</param>
        public bool SetDiscount(double discountValue)
        {
            if (discountValue > 100 || discountValue < 0)
                return false;

            this.discount = discountValue;
            return true;
        }

        /// <summary>
        /// Sells the items and remove from stock.
        /// </summary>
        /// <returns>The items.</returns>
        /// <param name="count">Count.</param>
        public double SellItems(int count)
        {
            if (count > this.quantity)
                throw new Exception("No enough items in storage");

            this.quantity -= count;

            return count * this.GetCurrentPriceAfterDiscount();
        }

        /// <summary>
        /// Gets the current value for the stock.
        /// </summary>
        /// <returns>The current value.</returns>
        public double GetCurrentValue()
        {
            return this.quantity * this.price;
        }

        /// <summary>
        /// Adds items to stock.
        /// </summary>
        /// <param name="count">Count.</param>
        public void AddToStock(int count)
        {
            if (count < 0)
                throw new Exception("Can not add negative values");

            quantity += count;
        }
    }

    public sealed class CarItemCSVMap : ClassMap<CarItem>
    {
        public CarItemCSVMap()
        {
            AutoMap();
        }
    }
}
