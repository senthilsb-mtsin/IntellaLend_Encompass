using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{
    public class UserAddressDetail
    {
        [Key]
        public int ID { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int UserID { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    
        //public virtual User User { get; set; }
    }
}
