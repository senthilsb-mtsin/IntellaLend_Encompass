using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntellaLend.Model.Encompass
{
    public class EWebhookEvents
    {
        [Key]
        public Int64 ID { get; set; }
        public string Response { get; set; }
        public Int32 EventType { get; set; }
        public Int32 Status { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}