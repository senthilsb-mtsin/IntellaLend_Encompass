using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class EncompassParkingSpot
    {
        [Key]
        public Int64 ID { get; set; }
        public string ParkingSpotName { get; set; }
        public Int64? DocumentTypeID { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
