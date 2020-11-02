using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class CategoryLists
    {
        [Key]
        public Int64 CategoryID { get; set; }
        public string Category { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
