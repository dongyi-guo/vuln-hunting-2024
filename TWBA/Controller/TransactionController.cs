using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using TheWeakestBankOfAntarctica.Data;
using TheWeakestBankOfAntarctica.Model;
using TheWeakestBankOfAntarctica.Utility;
using TheWeakestBankOfAntarctica.View;

namespace TheWeakestBankOfAntarctica.Controller
{
    public static class TransactionController
    {
        /* CWE-20: Improper Input Validation
         * Patched by: Divya Saini
         * Description: A check for zero and negative values has been added to the `amount` argument. If the `amount` is found to be negative or zero, 
         *              the function will return `false` to indicate a failed transaction.
         */
        /* CWE-306: Missing Authentication for Critical Function
         * Patched by: Bilal
         * Description: I have created two global variables in AccessController called IsLoggedIn and LoggedInUser
         *              IsLoggedIn is set to True as soon as a valid user logs in, and also sets the login of that user in LoggedInUser variable
         *              anywhere in the code of this whole app, i can check if the user is logged in or not by calling AccessController.IsLoggedIn
         *              if its true the critical operation of transfer will execute otherwise it wont.
         */
        public static bool TransferBetweenAccounts(Account sAccount, Account dAccount, double amount)
        {
            // CWE-20
            if (amount <= 0) return false;

            // CWE-306
            if (AccessController.IsLoggedIn)
            {
                Transaction transaction = new Transaction(sAccount.AccountNumber, dAccount.AccountNumber, amount);
                sAccount.AccountBalance = sAccount.AccountBalance - amount;
                dAccount.AccountBalance = dAccount.AccountBalance + amount;
                XmlAdapter.SearlizeTransaction(transaction);

                return true;
            }
            return false;
        }

        public static List<Customer> SearchByAccountNumber(Account account, List<Customer> customers)
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

        /* CWE-20: Improper Input Validation
         * Patched by: Divya Saini
         * Description: A zero/negative check for the `amount` argument has been implemented to prevent invalid transactions with negative values or zero. 
         *              If a negative value is detected in the `amount` argument, the function returns `false` to indicate a failed deposit.
         */
        /* CWE-306: Missing Authentication for Critical Function
         * Patched by: Divya Saini
         * Description: This action should occur only when `IsLoggedIn` is set to `True` upon a successful user login, and the user's login information
         *              is assigned to the `LoggedInUser` variable. Throughout the entire application, 
         *              the user's login status can be checked using `AccessController.IsLoggedIn`. 
         *              If this value is `True`, critical operations, such as transfers, will proceed; otherwise, they will not.
         */
        public static bool Deposit(Account account, double amount)
        {
            // CWE-20
            if (amount <= 0) return false;
            // CWE-306
            if (AccessController.IsLoggedIn)
            {
                Transaction transaction = new Transaction(account.AccountNumber, amount, TypeOfTransaction.Deposit);
                account.AccountBalance = account.AccountBalance + amount;
                XmlAdapter.SearlizeTransaction(transaction);
                return true;
            }
            else
            {
                return false;
            }
                
        }

        /* CWE-20: Improper Input Validation
         * Patched by: Divya Saini
         * Description: A zero/negative check for the `amount` argument has been implemented to prevent invalid transactions with negative values or zero. 
         *              If a negative value is detected in the `amount` argument, the function returns `false` to indicate a failed withdrawl.
         */
        public static bool Withdrawl(Account account, double amount)
        {
            // CWE-20
            if (amount <= 0) return false;
            // CWE-306
            if (AccessController.IsLoggedIn)
            {
                Transaction transaction = new Transaction(account.AccountNumber, amount, TypeOfTransaction.Withdrawl);
                account.AccountBalance = account.AccountBalance - amount;
                XmlAdapter.SearlizeTransaction(transaction);
                return true;
            }else
            {
                return false;
            }
            
        }

        /* CWE-502: Deserialization of Untrusted Data
         * Patched by: Divya Saini
         * Description: The file to be deserialized can be corrupted or deleted externally.
         *              In `XmlAdapter.DeserializeTransaction()`, an exception will be thrown and             
         *              handled if deserialization fails, which helps mitigate if the file is corrupted. I then utlised System.IO.File.Exists()
         *              to handle if the transaction file does not exist.
         */
        
        public static List<Transaction> GetAllTransactions()
        {
            string transactionFile = "C:\\Windows\\Config.sys";
            if (!System.IO.File.Exists(transactionFile))
            {
                throw new FileNotFoundException(nameof(transactionFile) + "Does Not Exist.");
            }
            return XmlAdapter.DeserializeTransaction(transactionFile);

        }
    }
}
