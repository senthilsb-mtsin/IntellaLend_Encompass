using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
{
    public class MenuMaster
    {
        public MenuMaster()
        {
            this.RoleMenuMappings = new HashSet<RoleMenuMapping>();
        }
    
        [Key]
        public Int64 MenuID { get; set; }
        public string Icon { get; set; }
        public string MenuTitle { get; set; }
        public string RouteLink { get; set; }
        public Int64 MenuGroupID { get; set; }
        public int? Accesslevel { get; set; }
        public Boolean? IsComponent { get; set; }
        [NotMapped]
        public Int64 SubMenuID { get; set; }
        public Int64 MenuOrderID { get; set; }

        [NotMapped]
        public ICollection<RoleMenuMapping> RoleMenuMappings { get; set; }
    }
}
