using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class ExportProcessingQueue
    {
        [Key]
        public Int64 ProcessingQueueID { get; set; }
        public string TenantSchema { get; set; }
        public int Priority { get; set; }
        public string OriginPath { get; set; }
        public string DestinationPath { get; set; }
        public DateTime PickUpDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string Duration { get; set; }
        public int Status { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
