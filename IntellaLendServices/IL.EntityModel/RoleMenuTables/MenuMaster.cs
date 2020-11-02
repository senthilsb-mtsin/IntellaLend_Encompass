using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{
    public class MenuMaster
    {
        public MenuMaster()
        {
            this.RoleMenuMappings = new HashSet<RoleMenuMapping>();
        }
    
        [Key]
        public int MenuID { get; set; }
        public string Icon { get; set; }
        public string MenuTitle { get; set; }
        public string RouteLink { get; set; }
            
        public virtual ICollection<RoleMenuMapping> RoleMenuMappings { get; set; }
    }
}
