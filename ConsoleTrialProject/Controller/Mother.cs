using System;
using System.Collections.Generic;
using System.IO;
using GarageStockApp.Items;
using CsvHelper;

namespace GarageStockApp
{
    /// <summary>
    /// Mother Class.
    /// </summary>
    public class Mother
    {
        /// <summary>
        /// The single mother instance (Singleton).
        /// </summary>
        private static Mother mother;

        /// <summary>
        /// The database.
        /// </summary>
        private Dictionary<long, CarItem> database;

        /// <summary>
        /// The barcodes.
        /// </summary>
        private List<long> barcodes;

        /// <summary>
        /// The shopping cart.
        /// </summary>
        private Dictionary<long, int> shoppingCart;

        /// <summary>
        /// Initializes a new instance of the Mother class.
        /// </summary>
        private Mother()
        {
            this.database = new Dictionary<long, CarItem>();
            this.barcodes = new List<long>();
        }

        /// <summary>
        /// Gets the mother singleton instance.
        /// </summary>
        /// <returns>The instance.</returns>
        public static Mother GetInstance()
        {
            if (mother == null)
            {
                mother = new Mother();
            }
            
            return mother;
        }

        /// <summary>
        /// Disposes the mother.
        /// </summary>
        public static void DisposeMother()
        {
            mother = null;
        }

        /// <summary>
        /// Adds a new item to database.
        /// </summary>
        /// <param name="carItem">Car item.</param>
        public void AddNewItemToDatabase(CarItem carItem)
        {
            long barcode = carItem.Barcode;

            if (!barcodes.Contains(barcode))
            {
                this.barcodes.Add(barcode);
                this.database.Add(barcode, carItem);
            }
        }

        /// <summary>
        /// Adds items to old stock.
        /// </summary>
        /// <param name="barcode">Barcode.</param>
        /// <param name="count">Count.</param>
        public void AddToStock(long barcode, int count)
        {
            if (!barcodes.Contains(barcode) || !database.ContainsKey(barcode))
            {
                throw new Exception("Barcode does not exist.");
            }

            if (count < 0)
            {
                throw new Exception("Count less than zero.");
            }

            this.database.TryGetValue(barcode, out CarItem oldItem);
            oldItem.AddToStock(count);
        }

        /// <summary>
        /// Calculates the current stock value.
        /// </summary>
        /// <returns>The current stock value.</returns>
        public double CalculateCurrentStockValue()
        {
            double total = 0;

            foreach (long barcode in barcodes)
            {
                database.TryGetValue(barcode, out CarItem carItem);
                total += carItem.GetCurrentValue();
            }

            return total;
        }

        /// <summary>
        /// Starts the shopping, creates the cart.
        /// </summary>
        public void StartShopping()
        {
            this.shoppingCart = new Dictionary<long, int>();
        }

        /// <summary>
        /// Adds an item to cart.
        /// </summary>
        /// <param name="item">Item.</param>
        public void AddToCart(CarItem item, int count)
        {
            if(this.shoppingCart != null)
            {
                this.shoppingCart.Add(item.Barcode, count);
            }
        }

        public Dictionary<string, double> CheckoutCartAndGenerateReceipt()
        {
            if(this.shoppingCart != null)
            {
                return this.GenerateReceipt(this.SellItemsAndGetReceipt(this.shoppingCart));
            }

            return null;
        }

        /// <summary>
        /// Sells the items and get receipt.
        /// </summary>
        /// <returns>The items and get receipt.</returns>
        /// <param name="itemsSold">Items sold.</param>
        public Dictionary<long, double> SellItemsAndGetReceipt(Dictionary<long, int> itemsSold)
        {
            Dictionary<long, double> receipt = new Dictionary<long, double>();

            foreach (long barcode in itemsSold.Keys)
            {
                if (!this.barcodes.Contains(barcode) || !database.ContainsKey(barcode))
                {
                    throw new Exception("Barcode " + barcode + " does not exist");
                }

                this.database.TryGetValue(barcode, out CarItem carItem);

                itemsSold.TryGetValue(barcode, out int count);

                double cost = carItem.SellItems(count);

                receipt.Add(barcode, cost);
            }

            return receipt;
        }

        /// <summary>
        /// Calculates the receipt total.
        /// </summary>
        /// <returns>The receipt total.</returns>
        /// <param name="receipt">Receipt.</param>
        public static double CalculateReceiptTotal(Dictionary<string, double> receipt)
        {
            double total = 0;

            foreach (double itemCost in receipt.Values)
            {
                total += itemCost;
            }

            return total;
        }

        /// <summary>
        /// Generates the receipt with name and cost for each item.
        /// </summary>
        /// <returns>The receipt.</returns>
        /// <param name="receipt">Receipt.</param>
        public Dictionary<string, double> GenerateReceipt(Dictionary<long, double> receipt)
        {
            Dictionary<string, double> nameReceipt = new Dictionary<string, double>();

            foreach (long barcode in receipt.Keys)
            {
                if (!this.barcodes.Contains(barcode) || !database.ContainsKey(barcode))
                {
                    throw new Exception("Barcode " + barcode + " does not exist");
                }

                this.database.TryGetValue(barcode, out CarItem carItem);
                receipt.TryGetValue(barcode, out double cost);

                nameReceipt.Add(carItem.Name, cost);
            }

            return nameReceipt;
        }

        /// <summary>
        /// Saves the database to a csv file.
        /// </summary>
        /// <param name="path">Path.</param>
        public void SaveDatabaseToCSV(string path)
        {
            using (StreamWriter stream = new StreamWriter(path))
            {
                CsvWriter csvWriter = new CsvWriter(stream);
                csvWriter.Configuration.RegisterClassMap<CarItemCSVMap>();
                csvWriter.WriteRecords<CarItem>(database.Values);
            }
        }

        /// <summary>
        /// Loads the databes from a csv file.
        /// </summary>
        /// <param name="path">Path.</param>
        public void LoadDatabaseFromCSV(string path)
        {
            using (StreamReader stream = new StreamReader(path))
            {
                CsvReader csvReader = new CsvReader(stream);
                csvReader.Read();

                while(csvReader.Read())
                {
                    long barcode = Convert.ToInt64(csvReader.GetField(0));
                    string name = csvReader.GetField(1);
                    int quanitity = Convert.ToInt32(csvReader.GetField(2));
                    double price = Convert.ToDouble(csvReader.GetField(3));
                    double discount = Convert.ToDouble(csvReader.GetField(4));

                    CarItem carItem = new CarItem(name, barcode, price, quanitity);
                    carItem.SetDiscount(discount);

                    database.Add(barcode, carItem);
                    barcodes.Add(barcode);
                }
            }
        }

        #region Dummy Data Generation

        /// <summary>
        /// The random.
        /// </summary>
        private Random random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Creates the dummy data to a csv file.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="records">Records.</param>
        public void CreateDummyDataToCSV(string path, int records)
        {
            using (StreamWriter stream = new StreamWriter(path))
            {
                CsvWriter csvWriter = new CsvWriter(stream);
                csvWriter.Configuration.RegisterClassMap<CarItemCSVMap>();

                var items = new List<CarItem>();
                for (int i = 0; i < records; i++)
                {
                    items.Add(GenerateRandomCarItem());
                }

                csvWriter.WriteRecords<CarItem>(items);
            }
        }

        /// <summary>
        /// Generates the random car item.
        /// </summary>
        /// <returns>The random car item.</returns>
        public CarItem GenerateRandomCarItem(){
            string name = GenerateRandomName();
            int code = GenerateRandomeCode();
            double price = GenerateRandomPrice();
            int quantity = GenerateRandomQuantity();
            double discount = GenerateRandomDiscount();
            CarItem randomCarItem = new CarItem(name, code, price, quantity);
            randomCarItem.SetDiscount(discount);
            return randomCarItem;
        }

        /// <summary>
        /// Generates the random quantity.
        /// </summary>
        /// <returns>The random quantity.</returns>
        private int GenerateRandomQuantity()
        {
            int quantity = random.Next(100);
            return quantity;
        }

        /// <summary>
        /// Generates the random price.
        /// </summary>
        /// <returns>The random price.</returns>
        private double GenerateRandomPrice()
        {
            int price = random.Next(10, 150);
            return price;
        }

        /// <summary>
        /// Generates the randome code.
        /// </summary>
        /// <returns>The randome code.</returns>
        private int GenerateRandomeCode()
        {
            int code = random.Next(1000000, 9999999);
            return code;
        }

        /// <summary>
        /// Generates the random name.
        /// </summary>
        /// <returns>The random name.</returns>
        private string GenerateRandomName()
        {
            string name = "CI";
            name += random.Next(10000, 99999);
            return name;
        }

        /// <summary>
        /// Generates the random discount.
        /// </summary>
        /// <returns>The random discount.</returns>
        private double GenerateRandomDiscount()
        {
            double discount = random.Next(0, 30);
            return discount;
        }
        #endregion
    }
}
