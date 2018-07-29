using System;
using System.Collections.Generic;
using GarageStockApp.Items;

namespace GarageStockApp
{
    class MainClass
    {
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            Mother mother = Mother.GetInstance();

            mother.CreateDummyDataToCSV(".//DummyDatabase.csv", 100);
            mother.LoadDatabaseFromCSV(".//DummyDatabase.csv");

            Console.WriteLine("This application demonstrates a 100 car items generated randomly, saved to a csv file and calculated their total values. \n");
            Console.WriteLine("Current Stock Value: {0} \n", mother.CalculateCurrentStockValue());
            Console.WriteLine("Please check the Tests class to check some scenarios.");

            Console.ReadLine();
        }
    }
}
