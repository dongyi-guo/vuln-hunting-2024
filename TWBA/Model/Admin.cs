using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheWeakestBankOfAntarctica.Model
{
    class Admin : User
    {
        public DateTime StartDate { get; set; }
        public Position Position { get; set; }
        public Role Role { get; set; }
        public string BranchName { get; set; }
        public string BranchId { get; set; }
        public string AdminId { get; private set; }

        /* CWE-522: Insufficiently Protected Credentials
         * Patched by: Divya Saini
         * Description: The password parameter passed into this function is currently in plain text, which poses a security risk. To enhance security, 
         *              a hashed password should always be used for login. The `password` attribute for both this class and `User` has been updated
         *              to `hash` to store the hashed values.
         */
        public Admin(string govId, string name, string lName, string email, string hash,
            Position position, Role role, string branchName, string branchId,
            string address, string phoneNumber) :
            base(govId, name, lName, email, hash, address, phoneNumber)
        {
            base.GovId = govId;
            base.Name = name;
            base.LastName = lName;
            base.Email = email;
            base.Hash = hash;
            base.Address = Address;
            base.PhoneNumber = phoneNumber;

            StartDate = DateTime.Now;
            Position = position;
            Role = role;
            BranchName = branchName;
            BranchId = branchId;
            AdminId = GenerateAdminId(8); // maximum length of an Id must be 8 characters
        }

        public string GenerateAdminId(int max)
        {
            Random random = new Random();
            string id = "";

            for (int i = 0; i < max; i++)
            {
                id += random.Next(10).ToString();
            }

            return BranchId + "-" + id + "A";
        }
    }

    public enum Position
    {
        manager, specialist, attendent, representative, none
    }

    public enum Role
    {
        Admin, Teller, none
    }

}
