using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
{

    public class SMTPDETAILS
    {
        [Key]
        public Int64 SMTPID { get; set; }
        public string SMTPNAME { get; set; }
        public string SMTPCLIENTHOST { get; set; }
        public int SMTPCLIENTPORT { get; set; }
        public string USERNAME { get; set; }
        public byte[] PASSWORD { get; set; }
        public string DOMAIN { get; set; }
        public bool ENABLESSL { get; set; }
        public int TIMEOUT { get; set; }
        public byte SMTPDELIVERYMETHOD { get; set; }
        public bool USEDEFAULTCREDENTIALS { get; set; }

        [NotMapped]
        public string PasswordString { get; set; }
    }
}
