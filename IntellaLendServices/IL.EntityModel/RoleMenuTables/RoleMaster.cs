using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{
    public class RoleMaster
    {
        //public RoleMaster()
        //{
        //    this.RoleMenuMappings = new HashSet<RoleMenuMapping>();
        //    this.AccessURLs = new HashSet<AccessURL>();
        //    this.UserRoleMappings = new HashSet<UserRoleMapping>();
        //}
    
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string StartPage { get; set; }
        public Nullable<int> AuthorityLevel { get; set; } 
           
        //public virtual ICollection<RoleMenuMapping> RoleMenuMappings { get; set; }    
        //public virtual ICollection<AccessURL> AccessURLs { get; set; }    
        //public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; }
    }
}
