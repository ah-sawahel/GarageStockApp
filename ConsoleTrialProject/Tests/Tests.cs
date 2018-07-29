using NUnit.Framework;
using GarageStockApp.Items;
using System.IO;
using System.Collections.Generic;
using System;

namespace GarageStockApp
{
    public class Tests
    {
        private Mother mother;

        /// <summary>
        /// Initialize this mother instance.
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
            mother = Mother.GetInstance();
        }

        /// <summary>
        /// Tests the create dummy database.
        /// Create a dummy car item and save it to a csv file.
        /// Ensure the file is created and can be read.
        /// </summary>
        [Test]
        public void TestCreateDummyDatabase()
        {
            mother.CreateDummyDataToCSV(".//DummyDatabase.csv", 1);
            string data = "";

            using(StreamReader stream = new StreamReader(".//DummyDatabase.csv"))
            {
                data = stream.ReadLine();
            }

            Assert.IsNotEmpty(data);
        }

        /// <summary>
        /// Tests the database insertion.
        /// Create 10 random items and add them to a csv file.
        /// Dispose and re-initialize the mother instance.
        /// Load data from csv.
        /// compare stock value before and after loading the csv files to ensure same data loaded again.
        /// </summary>
        [Test]
        public void TestDatabaseInsertion()
        {
            for (int i = 0; i < 10; i++)
            {
                mother.AddNewItemToDatabase(mother.GenerateRandomCarItem());
            }

            double value = mother.CalculateCurrentStockValue();

            mother.SaveDatabaseToCSV(".//database.csv");

            Mother.DisposeMother();
            mother = Mother.GetInstance();
                  
            mother.LoadDatabaseFromCSV(".//database.csv");

            double reloadedValue = mother.CalculateCurrentStockValue();

            Assert.AreEqual(value, reloadedValue);
        }

        /// <summary>
        /// Tests the used car items.
        /// Tests inheritance and hidden feature of setting the discount.
        /// </summary>
        [Test]
        public void TestUsedCarItems()
        {
            // Discount should always be 70% for used items.
            double price = 100;
            UsedCarItem usedCarItem = new UsedCarItem("CI1234", 12345, price, 100);

            Assert.AreEqual(price * 0.3, usedCarItem.GetCurrentPriceAfterDiscount());

            usedCarItem.SetDiscount(50);
            Assert.AreEqual(price * 0.3, usedCarItem.GetCurrentPriceAfterDiscount());
        }

        /// <summary>
        /// Tests the receipt.
        /// Creates 4 items and add them to database.
        /// Calculates stock value before selling.
        /// Add some items to shopping cart.
        /// Creates the receipt.
        /// Compare values of receipt and cost of "Used" items having 30% of their value.
        /// Compare stock value before and after selling with original price values.
        /// </summary>
        [Test]
        public void TestReceipt()
        {
            UsedCarItem usedCarItem1 = new UsedCarItem("CI1111", 11111, 100, 100);
            UsedCarItem usedCarItem2 = new UsedCarItem("CI2222", 22222, 100, 100);
            UsedCarItem usedCarItem3 = new UsedCarItem("CI3333", 33333, 100, 100);
            UsedCarItem usedCarItem4 = new UsedCarItem("CI4444", 44444, 100, 100);

            mother.AddNewItemToDatabase(usedCarItem1);
            mother.AddNewItemToDatabase(usedCarItem2);
            mother.AddNewItemToDatabase(usedCarItem3);
            mother.AddNewItemToDatabase(usedCarItem4);

            double stockBeforeSellValue = mother.CalculateCurrentStockValue();

            mother.StartShopping();
            mother.AddToCart(usedCarItem1, 10);
            mother.AddToCart(usedCarItem2, 10);
            mother.AddToCart(usedCarItem3, 10);
            mother.AddToCart(usedCarItem4, 10);

            Dictionary<string, double> receipt = mother.CheckoutCartAndGenerateReceipt();

            double receiptTotal = Mother.CalculateReceiptTotal(receipt);
            Assert.AreEqual(receiptTotal, 4 * 10 * 30);

            double stockAfterSellValue = mother.CalculateCurrentStockValue();
            Assert.AreNotEqual(stockAfterSellValue, stockBeforeSellValue);

            Assert.AreEqual(stockAfterSellValue + (4 * 10 * 100), stockBeforeSellValue);
        }
    }
}
