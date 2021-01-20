using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
{
    public class UserRoleMapping
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 RoleID { get; set; }
        public Int64 UserID { get; set; }
        public string RoleName { get; set; }

        //public virtual User User { get; set; }
        [NotMapped]
        public RoleMaster RoleMaster { get; set; }
    }

    public class UserRoleMappingTemp
    {
        public Int64 RoleID { get; set; }
        public Int64 UserID { get; set; }
        public string RoleName { get; set; }

    }

}
