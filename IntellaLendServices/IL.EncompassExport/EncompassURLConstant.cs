using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EncompassExport
{
    public class EncompassURLConstant
    {
        public const string GET_PIPELINE_LOANS = "Encompass/GetPipelineLoans";
        public const string GET_LOAN = "Encompass/GetLoan";
        public const string GET_LOAN_FIELD_VALUES = "Encompass/GetFieldLookupValue?loanGUID={0}&fieldID={1}";
        public const string GET_LOAN_ATTACHMENT = "Encompass/GetLoanAttachments";
        public const string UPDATE_LOAN_CUSTOM_FIELD = "Encompass/UpdateLoanCustomField";
        public const string GET_LOAN_FIELDS = "Encompass/GetLoanFields";
    }
}
