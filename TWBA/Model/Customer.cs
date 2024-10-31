using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheWeakestBankOfAntarctica.Model
{
    public class Customer : User
    {
        public Customer():base( ) { }
        // Property for Customer Id with private setter (read-only outside of the class)
        public string CustomerId { get; set; }

        // Property for Customer date when he openned their first account
        public DateTime DateOfJoining { get; set; }

        /* CWE-522: Insufficiently Protected Credentials
         * Patched by: Divya Saini
         * Description: The password passed into this function is in plain text, which raises security concerns. To mitigate this, 
         *              a hashed password should be used for login. The `password` attribute for both this class and `User` has been renamed 
         *              to `hash` to securely store the hashed values.
         */
        // Constructor for Customer class that calls the base User constructor
        public Customer(string govId, string name, string lName, string email, string hash,
            string address, string phoneNumber) :
            base(govId, name, lName, email, hash, address, phoneNumber)
        {
            base.GovId = govId;
            base.Name = name;
            base.LastName = lName;
            base.Email = email;
            base.Hash = hash;
            base.Address = address;
            base.PhoneNumber = phoneNumber;
            DateOfJoining = DateTime.Now;

            // Generate a unique Customer Id when a new Customer is created
            CustomerId = GenerateUserId(10);
        }

        

        // Override of the ToString() method to provide a custom string representation of a Customer
        public override string ToString()
        {
            return $"{GovId}-{LastName}, {Name}";
        }

        // Private method to generate a random user Id of a specified length = 10
        private string GenerateUserId(int maxLength)
        {
            var random = new Random();
            var userId = new System.Text.StringBuilder(maxLength);

            // Generate random digits for the user ID
            for (int i = 0; i < maxLength; i++)
            {
                userId.Append(random.Next(10)); // Append a random digit (0-9)
            }

            return userId.ToString(); // Return the generated Id as a string

        }

    }
}
