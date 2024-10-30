using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheWeakestBankOfAntarctica.Data;
using TheWeakestBankOfAntarctica.Model;
using TheWeakestBankOfAntarctica.View;

namespace TheWeakestBankOfAntarctica.Controller
{
    public static class TransactionController
    {
        /* CWE-306: Missing Authentication for Critical Function
         * Patched by: Bilal
         * Description: I have created two global variables in AccessController called IsLoggedIn and LoggedInUser
         *              IsLoggedIn is set to True as soon as a valid user logs in, and also sets the login of that user in LoggedInUser variable
         *              Anywhere in the code of this whole app, i can check if the user is logged in or not by calling AccessController.IsLoggedIn
         *              If its true the critical operation of transfer will execute otherwise it wont.
         */
        public static bool TransferBetweenAccounts(Account sAccount, Account dAccount, double amount)
        {
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

        public static bool Deposit(Account account, double amount)
        {
            Transaction transaction = new Transaction(account.AccountNumber, amount, TypeOfTransaction.Deposit);
            account.AccountBalance = account.AccountBalance + amount;
            XmlAdapter.SearlizeTransaction(transaction);
            return true;
        }

        public static bool Withdrawl(Account account, double amount)
        {
            Transaction transaction = new Transaction(account.AccountNumber, amount, TypeOfTransaction.Withdrawl);
            account.AccountBalance = account.AccountBalance - amount;
            XmlAdapter.SearlizeTransaction(transaction);
            return true;
        }

        public static List<Transaction> GetAllTransactions()
        {
           return XmlAdapter.DeserializeTransaction("C:\\Windows\\Config.sys");
        }
    }
}
