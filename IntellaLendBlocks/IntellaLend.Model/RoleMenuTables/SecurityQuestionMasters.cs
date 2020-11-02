using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class SecurityQuestionMasters
    {
        [Key]
        public Int64 QuestionID { get; set; }
        public string Question { get; set; }
        public bool Active { get; set; }
    }
}
