using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class EncompassAccessToken
    {
        [Key]
        public Int64 ID { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
