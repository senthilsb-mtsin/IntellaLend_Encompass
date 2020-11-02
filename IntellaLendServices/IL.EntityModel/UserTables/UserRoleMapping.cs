using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{
    public class UserRoleMapping
    {
        [Key]
        public int ID { get; set; }
        public int RoleID { get; set; }
        public int UserID { get; set; }
        public string RoleName { get; set; }

        //public virtual User User { get; set; }
        //public virtual RoleMaster RoleMaster { get; set; }
    }
}
