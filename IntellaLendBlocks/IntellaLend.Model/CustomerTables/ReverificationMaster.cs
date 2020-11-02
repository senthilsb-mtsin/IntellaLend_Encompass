using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class ReverificationMaster
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 ReverificationID { get; set; }
        public string ReverificationName { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid? LogoGuid { get; set; }
        public string FileName { get; set; }

    }
}
