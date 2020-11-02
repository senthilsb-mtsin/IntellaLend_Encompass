using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntellaLend.Model
{
    public class ADGroupMasters
    {
        [Key]
        public Int64 ADGroupID { get; set; }
        public string ADGroupName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
