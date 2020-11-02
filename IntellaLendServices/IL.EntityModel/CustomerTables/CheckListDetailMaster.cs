using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{

    public class CheckListDetailMaster
    {
      
        [Key]
        public long CheckListDetailID { get; set; }
        public long CheckListID { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; } 

        public virtual CheckListMaster CheckListMaster { get; set; }
        //public virtual List<RuleMaster> RuleMasters { get; set; }
    }
}
