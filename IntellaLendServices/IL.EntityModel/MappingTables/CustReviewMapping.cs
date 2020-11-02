using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{
    public class CustReviewMapping
    {
        [Key]
        public long CustReviewMappingID { get; set; }
        public long CustomerID { get; set; }
        public long ReviewTypeID { get; set; }        
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual CustomerMaster CustomerMaster { get; set; }
        public virtual ReviewTypeMaster ReviewTypeMaster { get; set; }
    }
}
