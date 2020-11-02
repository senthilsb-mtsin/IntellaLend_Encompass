using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class MenuGroupMaster
    {       
        [Key]
        public Int64 MenuGroupID { get; set; }
        public string MenuGroupIcon { get; set; }
        public string MenuGroupTitle { get; set; }
    }
}
