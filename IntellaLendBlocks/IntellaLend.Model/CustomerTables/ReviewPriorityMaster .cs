using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
{

    public class ReviewPriorityMaster
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 ReviewPriorityID { get; set; }
        public Int64 ReviewPriority { get; set; }
        public string ReviewPriorityName { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }


    }
}
