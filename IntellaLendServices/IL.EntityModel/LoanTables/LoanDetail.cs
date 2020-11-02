using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{
    public class LoanDetail
    {
        [Key]
        public long LoanDetailID { get; set; }
        public long LoanID { get; set; }
        public string LoanXML { get; set; }

        public virtual Loan Loan { get; set; }
    }
}
