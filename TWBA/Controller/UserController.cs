using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheWeakestBankOfAntarctica.Data;
using TheWeakestBankOfAntarctica.Model;
using TheWeakestBankOfAntarctica.Utility;

namespace TheWeakestBankOfAntarctica.Controller
{
    static class UserController
    {
        /* CWE-522: Insufficiently Protected Credentials
         * Patched by: Divya Saini
         * Description: The password passed into this function is in plain text, which poses security concerns. To address this, 
         *              I now use `UtilityFunctions.CreateHash(login, password)` to generate a hash from the user's email and password for secure authentication.
         *              Additionally, I have streamlined this change across the `Customer`, `Admin`, and `User` classes by updating the password attributes to 
         *              `hash` for storing the hash. Changes were also made to `FakeData.cs` to ensure customers are created using 
         *              the updated `Customer` constructor.
         */
        public static string CreateCustomer(string govId, string name, string lName,
           string email, string password, string address, string phoneNumber, double initialBalance)
        {
            string hash = UtilityFunctions.CreateHash(email, password);
            Customer customer = new Customer(govId, name, lName, email, hash,
                address, phoneNumber);

            // SQLiteDB db = new SQLiteDB();
            // XmlAdapter.SerializeCustomerDataToXml(customer);
            // Account account = new Account(initialBalance, customer.CustomerId);
            //db.AddNewCustomer(customer.CustomerId, govId, name, lName, email,
            //   password, address, phoneNumber);
            return customer.CustomerId;
        }

        public static List<Customer> SearchByAccount(Account account, List<Customer> customers)
        {
            List<Customer> accountOwners = new List<Customer>();

            // Get the list of owner IDs from the account
            List<string> ownerIds = account.AccountOwners;

            // Iterate through the provided customers list and match them by CustomerId
            foreach (var customer in customers)
            {
                // If the customer's ID is found in the account's owner list, add them to the result list
                if (ownerIds.Contains(customer.CustomerId))
                {
                    accountOwners.Add(customer);
                }
            }

            // Return the list of owners (Customer objects)
            return accountOwners;
        }

        /* CWE-522: Insufficiently Protected Credentials
         * Patched by: Divya Saini
         * Description: The password passed into this function is in plain text, which poses security concerns. To address this, 
         *              I now use `UtilityFunctions.CreateHash(login, password)` to generate a hash from the user's email and password for secure authentication.
         *              Additionally, I have streamlined this change across the `Customer`, `Admin`, and `User` classes by updating the password attributes to 
         *              `hash` for storing the hash.
         */
        public static string CreateAdminUser(string govId, string name, string lName, string email,
            string password, string branchName, string branchId, string address,
            string phoneNumber)
        {
            //  SQLiteDB db = new SQLiteDB();
            string hash = UtilityFunctions.CreateHash(email, password);
            Admin admin = new Admin(govId, name, lName, email, hash, Position.manager,
                Role.Admin, branchName, branchId, address, phoneNumber);
            return admin.AdminId;
        }

    }
}
