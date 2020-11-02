using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class AccessURL
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 RoleID { get; set; }
        public string URL { get; set; }
    
        //public virtual RoleMaster RoleMaster { get; set; }
    }
}
