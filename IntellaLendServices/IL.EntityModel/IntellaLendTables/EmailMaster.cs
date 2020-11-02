using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntellaLend.Model
{
    public class EmailMaster
    {
        [Key]
        public int ID { get; set; }
        public Int64 TEMPLATEID { get; set; }
        public string EMAILSP { get; set; }
        public DateTime REQUESTTIME { get; set; }
        public int STATUS { get; set; }
    }
}
