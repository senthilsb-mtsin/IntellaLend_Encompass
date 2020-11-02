using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntellaLend.Model
{    
    public class TenantMaster
    {
        [Key]
        public int ID { get; set; }
        public string TenantName { get; set; }
    }
}
