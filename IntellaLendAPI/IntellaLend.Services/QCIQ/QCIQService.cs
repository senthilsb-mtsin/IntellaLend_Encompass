using IntellaLend.EntityDataHandler;
using IntellaLend.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace IntellaLend.CommonServices
{
    public class QCIQService
    {
        #region Private Variable

        private static string TenantSchema;

        #endregion

        #region Constructor

        public QCIQService() { }

        public QCIQService(string tenantschema)
        {
            TenantSchema = tenantschema;

        }

        #endregion

        #region Public Methods

        public object GetQueryResultData(string QueryString)
        {
            return JsonConvert.SerializeObject(QCIQDataAccess.GetQueryResult(QueryString));
        }

        public bool QCIQAddLoanType(List<SystemLoanTypeMaster> reqloantypes)
        {
            return new IntellaLendDataAccess(TenantSchema).QCIQAddLoanType(reqloantypes);
        }

        public bool QCIQAddReviewType(List<SystemReviewTypeMaster> reviewtypereq)
        {
            return new IntellaLendDataAccess(TenantSchema).QCIQAddReviewType(reviewtypereq);
        }

        #endregion

    }
}
