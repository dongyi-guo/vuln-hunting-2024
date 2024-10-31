using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheWeakestBankOfAntarctica.Model;
using TheWeakestBankOfAntarctica.Utility;

namespace TheWeakestBankOfAntarctica.Data
{
    public static class DataAdapter
    {
        private static TWBA twba = null;

        public static void Init(TWBA model)
        {
            twba = model;
        }

        /* CWE-798 : Use of Hard coded Credentials
         * Patched by : Divya Saini
         * Description : I have removed the plain-text `server`, `database`, `username`, and `password` variables to prevent exposure to potential attackers
         *               accessing this code. Instead, the user will now be prompted to provide the information.
         */
        /* CWE-522 : Use of Hard coded Credentials
         * Patched by : Divya Saini
         * Description : I have removed the plain-text `server`, `database`, `username`, and `password` variables to prevent exposure to potential attackers
         *               accessing this code. Instead, the user will now be prompted to provide the information.
         */
        static void ConnectToRemoteDB()
        {
            Console.WriteLine("Enter the server address: ");
            string server = Console.ReadLine();
            Console.WriteLine("Enter the database name: ");
            string database = Console.ReadLine();
            Console.WriteLine("Enter the username: ");
            string username = Console.ReadLine();
            Console.WriteLine("Enter the password: ");
            string password = Console.ReadLine();

            string connectionString = $"Server={server};Database={database};Uid={username};Pwd={password};";
            try {
                Console.WriteLine("Connected to MySQL server!");
                // We arent using the Remote DB here; however, this is a valid code in the App
            }
            catch (Exception ex) { }
        }


        public static Account GetAccountByAccountNumber(string accountNumber)
        {
            Account account = (from a in twba.GetAllAccounts()
                       where a.AccountNumber == accountNumber
                       select a).FirstOrDefault();

            return account; 
        }

        public static List<Account> GetAccountOwners(string customerId)
        {
            List<Account> customerAccounts = (from account in twba.GetAllAccounts()
                                    where account.AccountOwners.Contains(customerId)
                                    select account).ToList();

            return customerAccounts;
        }
        public static string CloseAccount(string accountNumber)
        {
            Account account = GetAccountByAccountNumber(accountNumber);
            twba.GetAllAccounts().Remove(account);
            account = null;
            return account.AccountNumber;
        }
    }
}
