using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{

    public  class DocumentFieldMaster
    {
        [Key]
        public long FieldID { get; set; }
        public long DocumentTypeID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool Active { get; set; }
        public virtual DocumentTypeMaster DocumentTypeMaster { get; set; }
    }
}
