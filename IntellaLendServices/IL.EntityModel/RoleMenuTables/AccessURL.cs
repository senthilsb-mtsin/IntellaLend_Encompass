using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{
    public class AccessURL
    {
        [Key]
        public int ID { get; set; }
        public int RoleID { get; set; }
        public string URL { get; set; }
    
        //public virtual RoleMaster RoleMaster { get; set; }
    }
}
