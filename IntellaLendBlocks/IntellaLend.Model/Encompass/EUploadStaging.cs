using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class EUploadStaging
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 UploadStagingID { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 TypeOfUpload { get; set; }
        public string Document { get; set; }
        public string EParkingSpot { get; set; }
        public int Version { get; set; }
        public Int64 Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ErrorMsg { get; set; }

    }
}
