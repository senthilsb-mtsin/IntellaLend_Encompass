using IntellaLend.Constance;
using IntellaLend.Model;
using IntellaLend.WorkFlow;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace IntellaLend.LOSWorkflow
{
    public class LOSWorkflow : IWorkFlow
    {
        public virtual void SetWorkFlowState(ref Dictionary<string, string> wfValues)
        {
            try
            {
                Logger.WriteTraceLog("LOSWorkflow");
                Int64 loanID = Convert.ToInt64(wfValues["LOANID"]);
                string TenantSchema = wfValues["TENANT_SCHEMA"];

                LOSWorkFlowData _loanDataAccess = new LOSWorkFlowData(TenantSchema);
                var obj = _loanDataAccess.GetQCIQLookupDetails(loanID);
                Boolean _qciqEnabled = _loanDataAccess.QCIQEnabled();
                Dictionary<string, object> loanDetails = _loanDataAccess.GetLoanNumber(loanID);
                if (obj != null && _qciqEnabled)
                {
                    DateTime? QCIQStartDate = Convert.ToDateTime(loanDetails["startDate"]);

                    QCIQLoanDeails _lDetails = JsonConvert.DeserializeObject<QCIQLoanDeails>(JsonConvert.SerializeObject(obj));

                    string sqlScript = _lDetails.MasterSQLScript;
                    sqlScript = sqlScript.Replace("<<CUSTOMER_NAME>>", _lDetails.CustomerName.Replace("'", "''"));
                    //Get Master Data QCIQ
                    System.Data.DataSet MasterQCIQData = _loanDataAccess.GetQCIQData(_lDetails.MasterConnectionString, sqlScript);

                    Int64 CustomerID = 0;
                    if (MasterQCIQData != null && MasterQCIQData.Tables.Count > 0)
                    {
                        System.Data.DataTable dt = GetTableFromSet(MasterQCIQData, "CUSTOMER");
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            CustomerID = Convert.ToInt64(dt.Rows[0]["customerid"]);

                            if (CustomerID > 0)
                            {
                                sqlScript = _lDetails.SQLScript;
                                sqlScript = sqlScript.Replace("<<CUSTOMER_ID>>", CustomerID.ToString());
                                sqlScript = sqlScript.Replace("<<LOAN_NUMBER>>", loanDetails["loanNumber"].ToString().Replace("'", "''"));

                                //Get Data for particular loan from QCIQ
                                System.Data.DataSet QCIQData = _loanDataAccess.GetQCIQData(_lDetails.ConnectionString, sqlScript);

                                if (QCIQData != null && QCIQData.Tables.Count > 0)
                                {
                                    dt = GetTableFromSet(QCIQData, "ASSIGNMENTHISTORYDATA");
                                    if (dt != null && dt.Rows.Count > 0)
                                    {

                                        QCIQStartDate = Convert.ToDateTime(dt.Rows[0]["WhenAssigned"]);

                                    }
                                }
                            }
                        }
                    }
                    _loanDataAccess.UpdateQCIQStartDate(QCIQStartDate, loanID);
                }
                Logger.WriteTraceLog("After UpdateQCIQStartDate ");

                _loanDataAccess.UpdateLoanCompleteUserDetails(loanID, RoleConstant.SYSTEM_ADMINISTRATOR, 1, "Auto Staged");

                Logger.WriteTraceLog("After UpdateLoanCompleteUserDetails ");

                _loanDataAccess.InsertLOSLoanExport(loanID, loanDetails["loanNumber"].ToString());

                Logger.WriteTraceLog("After InsertLOSLoanExport ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private System.Data.DataTable GetTableFromSet(System.Data.DataSet QCIQData, string TableName)
        {

            foreach (System.Data.DataTable dt in QCIQData.Tables)
            {
                if (dt.Rows.Count > 0 &&
                    Convert.ToString(dt.Rows[0]["TABLE_NAME"]).ToUpper().Trim() == TableName)
                    return dt;
            }

            return null;
        }

    }
}
