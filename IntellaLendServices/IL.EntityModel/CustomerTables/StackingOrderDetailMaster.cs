using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{    
    public class StackingOrderDetailMaster
    {
        [Key]
        public long StackingOrderDetailID { get; set; }
        public long StackingOrderID { get; set; }
        public string StackingOrderDescription { get; set; }
        public bool Active { get; set; }

        public virtual StackingOrderMaster StackingOrderMaster { get; set; }
    }
}
