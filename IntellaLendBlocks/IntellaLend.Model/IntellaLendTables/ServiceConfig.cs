using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class ServiceConfig
    {
        [Key]
        public Int64 SERVICEID { get; set; }
        public string SERVICENAME { get; set; }
        public string SERVICEDISPLAYNAME { get; set; }
        public string SERVICEDESCRIPTION { get; set; }
        public string SERVICEINVOKETYPE { get; set; }
        public string DLLNAME { get; set; }
        public string SERVICEPARAMS { get; set; }
        public string TIME { get; set; }
        public Nullable<short> RETRYCOUNT { get; set; }
        public Nullable<short> MAXERRORS { get; set; }
        public Nullable<byte> STATUS { get; set; }
        public Nullable<short> ENVIRONMENT { get; set; }
    }
}
