using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{
    public class RoleMenuMapping
    {
        [Key]
        public int MappingID { get; set; }
        public int MenuID { get; set; }
        public int MenuOrder { get; set; }
        public int RoleID { get; set; }
    
        //public virtual MenuMaster MenuMaster { get; set; }
        //public virtual RoleMaster RoleMaster { get; set; }
    }
}
