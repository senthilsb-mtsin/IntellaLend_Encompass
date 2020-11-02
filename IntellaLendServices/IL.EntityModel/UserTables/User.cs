
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
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
        public int UserID { get; set; }
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
        public long CustomerID { get; set; }
    
        public virtual List<CustomAddressDetail> CustomAddressDetails { get; set; }    
        public virtual UserAddressDetail UserAddressDetail { get; set; }        
        public virtual List<UserRoleMapping> UserRoleMapping { get; set; }
        public virtual CustomerMaster customerDetail { get; set; }
        public virtual UserSecurityQuestion userSecurityQuestion { get; set; }


    }
}
