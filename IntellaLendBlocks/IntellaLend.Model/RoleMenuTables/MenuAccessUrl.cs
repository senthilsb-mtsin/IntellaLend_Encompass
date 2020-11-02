using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class MenuAccessUrl
    {
        [Key]
        public int MenuAccessId { get; set; }
        public int MenuID {get;set;}
        public string Url { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
