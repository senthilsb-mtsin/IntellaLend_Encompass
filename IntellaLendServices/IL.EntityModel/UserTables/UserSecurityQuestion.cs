using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{
    public class UserSecurityQuestion
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int QuestionID { get; set; }
        public string Answer { get; set; }

        public virtual SecurityQuestionMasters securityQuestion { get; set; }
    }
}
