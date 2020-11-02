using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
{
    public class User
    {
      
        //public User()
        //{
        //  //  this.CustomAddressDetails = new HashSet<CustomAddressDetail>();
        //    this.UserAddressDetails = new HashSet<UserAddressDetail>();
        //    this.UserRoleMappings = new HashSet<UserRoleMapping>();
        //}
    
        [Key]
        public Int64 UserID { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public string FirstName { get; set; }
        public DateTime LastModified { get; set; }
        public string LastName { get; set; }
        public bool Locked { get; set; }
        public string MiddleName { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Int64 CustomerID { get; set; }
        public DateTime PasswordCreatedDate { get; set; }
        public Int64 NoOfAttempts { get; set; }
        public DateTime LastPwdModifiedDate { get; set; }
        [NotMapped]
        public List<CustomAddressDetail> CustomAddressDetails { get; set; }
        [NotMapped]
        public UserAddressDetail UserAddressDetail { get; set; }
        [NotMapped]
        public List<UserRoleMapping> UserRoleMapping { get; set; }
        [NotMapped]
        public CustomerMaster customerDetail { get; set; }
        [NotMapped]
        public UserSecurityQuestion userSecurityQuestion { get; set; }


    }
}
