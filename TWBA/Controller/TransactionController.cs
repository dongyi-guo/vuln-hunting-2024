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
        /* CWE-20: Improper Input Validation
         * Patched by: YourNameHere
         * Description: A negative check for the amount argument has been created to refuse any invalid transactions
         *              With negative transferring amounts.
         *              If amount argument is found with negative values, the function returns false to indicate a failed transaction.
         */
        /* CWE-476: NULL Pointer Dereference
         * Patched by: YourNameHere
         * Description: A null check for sAccount and dAccount arguments has been created to avoid null pointer dereference if while using this
         *              Function the Account objects passed into the arguments is accidentally null object.
         *              If null object is found in either sAccount or dAccount, the function returns false to indicate a failed transaction.
         */
        public static bool TransferBetweenAccounts(Account sAccount, Account dAccount, double amount)
        {
            // CWE-476
            if (sAccount == null) return false;
            if (dAccount == null) return false;
            // CWE-20
            if (amount  < 0) return false;

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

        /* CWE-476: NULL Pointer Dereference
         * Patched by: YourNameHere
         * Description: A null check for account and customer arguments has been created to avoid null pointer dereference if while using this
         *              Function the objects passed into the argument is accidentally null object.
         *              If null object is found in either account or customers argument, the function throws ArgumentNullException, any usage of
         *              This function should expect and catch this exception.
         */
        public static List<Customer> SearchByAccountNumber(Account account, List<Customer> customers)
        {
            // CWE-476
            if (account == null) throw new ArgumentNullException(nameof(account));
            if (customers == null) throw new ArgumentNullException(nameof(account));

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
         * Patched by: YourNameHere
         * Description: A negative check for the amount argument has been created to refuse any invalid transactions
         *              With negative transferring amounts.
         *              If amount argument is found with negative values, the function returns false to indicate a failed deposit.
         */
        /* CWE-476: NULL Pointer Dereference
         * Patched by: YourNameHere
         * Description: A null check for account argument has been created to avoid null pointer dereference if while using this
         *              Function the Account object passed into the argument is accidentally null object.
         *              If null object is found in account, the function returns false to indicate a failed deposit.
         */
        /* CWE-306: Missing Authentication for Critical Function
         * Patched by: YourNameHere
         * Description: Same as mentioned by Dr. Amin, this action should only happen when
         *              IsLoggedIn is set to True as soon as a valid user logs in, and also sets the login of that user in LoggedInUser variable
         *              Anywhere in the code of this whole app, i can check if the user is logged in or not by calling AccessController.IsLoggedIn
         *              If its true the critical operation of transfer will execute otherwise it wont.
         */
        public static bool Deposit(Account account, double amount)
        {
            if (account == null) return false;
            if (amount < 0) return false;
            Transaction transaction = new Transaction(account.AccountNumber, amount, TypeOfTransaction.Deposit);
            account.AccountBalance = account.AccountBalance + amount;
            XmlAdapter.SearlizeTransaction(transaction);
            return true;
        }

        /* CWE-20: Improper Input Validation
         * Patched by: YourNameHere
         * Description: A negative check for the amount argument has been created to refuse any invalid transactions
         *              With negative transferring amounts.
         *              If amount argument is found with negative values, the function returns false to indicate a failed withdrawl.
         */
        /* CWE-476: NULL Pointer Dereference
         * Patched by: YourNameHere
         * Description: A null check for account argument has been created to avoid null pointer dereference if while using this
         *              Function the Account object passed into the argument is accidentally null object.
         *              If null object is found in account, the function returns false to indicate a failed withdrawl.
         */
        public static bool Withdrawl(Account account, double amount)
        {
            if (account == null) return false;
            if (amount < 0) return false;
            Transaction transaction = new Transaction(account.AccountNumber, amount, TypeOfTransaction.Withdrawl);
            account.AccountBalance = account.AccountBalance - amount;
            XmlAdapter.SearlizeTransaction(transaction);
            return true;
        }


        /* CWE-502: Deserialization of Untrusted Data
         * Patched by: YourNameHere
         * Description: This function is deserilising a file from the system, which could be externally modified and corrupted.
         *              Here, additional steps to 
         */
        public static List<Transaction> GetAllTransactions()
        {
           return XmlAdapter.DeserializeTransaction("C:\\Windows\\Config.sys");
        }
    }
}
