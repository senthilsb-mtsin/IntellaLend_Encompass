using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{
    public class LoanImage
    {
        [Key]
        public long LoanImageID { get; set; }
        public long LoanID { get; set; }
        public long DocumentTypeID { get; set; }
        public string PageNo { get; set; }
        public string Image { get; set; }
        public string Version { get; set; }
    
        public virtual Loan Loan { get; set; }
        public virtual DocumentTypeMaster DocumentTypeMaster { get; set; }
    }
}
