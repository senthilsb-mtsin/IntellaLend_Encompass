using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class EmailMaster
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 TEMPLATEID { get; set; }
        public string EMAILSP { get; set; }
        public DateTime REQUESTTIME { get; set; }
        public Nullable<byte> STATUS { get; set; }
    }
}
