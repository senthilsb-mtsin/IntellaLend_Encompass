using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class QCIQConnectionString
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public string ConnectIonString { get; set; }
        public string SQLScript { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
