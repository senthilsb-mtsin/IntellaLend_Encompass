using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{

    public class CustomerMaster
    {
        //public CustomerMaster()
        //{
        //    this.CustLoanReviewCheckStackMappings = new HashSet<CustLoanReviewCheckStackMapping>();
        //    this.Users = new HashSet<User>();
        //}

        [Key]
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string CustomerCode { get; set; }
        // public string BoxFolderName { get; set; }

        public bool Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        //public virtual ICollection<CustLoanReviewCheckStackMapping> CustLoanReviewCheckStackMappings { get; set; }
        //public virtual ICollection<User> Users { get; set; }
    }
}
