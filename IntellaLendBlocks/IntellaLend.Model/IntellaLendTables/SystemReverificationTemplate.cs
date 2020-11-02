using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class SystemReverificationTemplate
    {
        [Key]
        public Int64 TemplateID { get; set; }
        public string TemplateName { get; set; }
        public string TemplateFileName { get; set; }
        public string TemplateFields { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
