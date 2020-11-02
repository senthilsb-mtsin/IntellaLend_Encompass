using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{

    public class DocumentTypeMaster
    {
        
        //public DocumentTypeMaster()
        //{
        //    this.DocumentFieldMasters = new HashSet<DocumentFieldMaster>();
        //    this.LoanImages = new HashSet<LoanImage>();
        //}
        [Key]
        public long DocumentTypeID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool Active { get; set; }

        //public virtual LoanTypeMaster LoanTypeMaster { get; set; }
        //public virtual List<DocumentFieldMaster> DocumentFieldMasters { get; set; }
        //public virtual ICollection<LoanImage> LoanImages { get; set; }
    }
}
