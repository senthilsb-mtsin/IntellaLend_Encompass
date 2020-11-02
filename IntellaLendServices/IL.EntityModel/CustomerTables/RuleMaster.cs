using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{
    
    
    public class RuleMaster
    {
        [Key]
        public long RuleID { get; set; }
        public long CheckListDetailID { get; set; }
        public string RuleDescription { get; set; }
        public bool Active { get; set; }

        public virtual CheckListDetailMaster CheckListDetailMaster { get; set; }
    }
}
