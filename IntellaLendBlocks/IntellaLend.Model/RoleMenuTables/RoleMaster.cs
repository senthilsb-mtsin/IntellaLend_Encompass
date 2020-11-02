using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
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
        public Int64 RoleID { get; set; }
        public string RoleName { get; set; }
        public string StartPage { get; set; }
        public Nullable<int> AuthorityLevel { get; set; } 
        public bool Active { get; set; }
        public Boolean IncludeKpi { get; set; }
        public Boolean ExternalRole { get; set; }
        public Int64 ADGroupID { get; set; }

        //public virtual ICollection<RoleMenuMapping> RoleMenuMappings { get; set; }    
        //public virtual ICollection<AccessURL> AccessURLs { get; set; }    
        //public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; }
    }

    public class RoleMasterADGroup
    {
        public Int64 RoleID { get; set; }
        public string RoleName { get; set; }
        public string StartPage { get; set; }
        public Nullable<int> AuthorityLevel { get; set; }
        public bool Active { get; set; }
        public Boolean IncludeKpi { get; set; }
        public Boolean ExternalRole { get; set; }
        public Int64 ADGroupID { get; set; }
        public string ADGroupName { get; set; }
    }
}
