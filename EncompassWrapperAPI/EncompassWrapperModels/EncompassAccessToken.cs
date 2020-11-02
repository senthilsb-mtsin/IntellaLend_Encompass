using System;
using System.ComponentModel.DataAnnotations;

namespace EncompassWrapperModels
{
    public class EncompassAccessToken
    {
        [Key]
        public long ID { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
