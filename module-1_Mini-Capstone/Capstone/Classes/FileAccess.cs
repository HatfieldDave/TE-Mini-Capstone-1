﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Capstone.Classes
{
    /// <summary>
    /// This class should contain any and all details of access to files
    /// </summary>
    public class FileAccess
    {
        // All external data files for this application should live in this directory.
        // You will likely need to create this directory and copy / paste any needed files.
        private const string DataDirectory = @"C:\Catering";

        // These files should be read from / written to in the DataDirectory
        private const string CateringFileName = @"cateringsystem.csv";
        private const string ReportFileName = @"totalsales.txt";
        private const string LogFileName = @"log.txt";

        private List<string> log = new List<string>();

        /// <summary>
        /// Reads in the catering inventory file and fills the given CateringSystems dictionary.
        /// </summary>
        /// <param name="catering"></param>
        public void ReadFromFile(CateringSystem catering)
        {
            try
            {
                using (StreamReader reader = new StreamReader(Path.Combine(DataDirectory, CateringFileName)))  // Create StreamReader to read our file
                {
                    while (!reader.EndOfStream)  // Goes line by line through the file
                    {
                        string line = reader.ReadLine();

                        string[] properties = line.Split("|"); // Splits line into an array of strings
                        decimal costDecimal = decimal.Parse(properties[3]);  // Converts the price to decimal
                        string itemType = properties[0];

                        // Makes an appropriate CateringItem subclass instance based on the item type.
                        if (itemType.Equals("A"))
                        {
                            AppetizerItem newAppetizerItem = new AppetizerItem(properties[0], properties[1], properties[2], costDecimal, 10);
                            catering.ItemSaver(newAppetizerItem);
                        }
                        if (itemType.Equals("B"))
                        {
                            BeverageItem newBeverageItem = new BeverageItem(properties[0], properties[1], properties[2], costDecimal, 10);
                            catering.ItemSaver(newBeverageItem);
                        }
                        if (itemType.Equals("D"))
                        {
                            DessertItem newDessertItem = new DessertItem(properties[0], properties[1], properties[2], costDecimal, 10);
                            catering.ItemSaver(newDessertItem);
                        }
                        if (itemType.Equals("E"))
                        {
                            EntreeItem newEntreeItem = new EntreeItem(properties[0], properties[1], properties[2], costDecimal, 10);
                            catering.ItemSaver(newEntreeItem);
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("You will be unable to complete orders until the file is found.");
            }
        }

        /// <summary>
        /// Creates a log entry string and adds it to the log list 
        /// </summary>
        /// <param name="logMessage">Message to be logged</param>
        /// <param name="changeInBalance">Change in balance to be logged</param>
        /// <param name="currentBalance">Current balance to be logged</param>
        public void Log(string logMessage, decimal changeInBalance, decimal currentBalance)
        {
            string currentDateTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

            string logString = currentDateTime + " " + logMessage + " " + changeInBalance + " " + currentBalance;

            log.Add(logString);
        }

        public void WriteLog()
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(DataDirectory, LogFileName), true))
            {
                foreach (string logItem in log)
                {
                    writer.WriteLine(logItem);
                }
            }
        }
    }
}

