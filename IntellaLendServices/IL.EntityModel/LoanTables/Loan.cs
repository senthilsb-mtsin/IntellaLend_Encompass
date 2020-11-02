using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{
    public class Loan
    {       
        //public Loan()
        //{
        //    this.LoanDetails = new HashSet<LoanDetail>();
        //    this.LoanImages = new HashSet<LoanImage>();
        //}
    
        [Key]
        public long LoanID { get; set; }
        public int UploadedUserID { get; set; }
        public long ReviewTypeID { get; set; }
        public long LoanTypeID { get; set; }
        public int LoggedUserID { get; set; }
        public int Status { get; set; }
        public int SubStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string FileName { get; set; }
        public long CustomerID { get; set; }
        //public virtual LoanDetail LoanDetails { get; set; }  
        //public virtual LoanImage LoanImages { get; set; }
        //public virtual CustomerMaster CustomerDetails { get; set; }
        //public virtual User User { get; set; }
        //public virtual ReviewTypeMaster ReviewTypeMaster { get; set; }
        //public virtual LoanTypeMaster LoanTypeMaster { get; set; }
        //public virtual User LoggedUser { get; set; }
    }
}
