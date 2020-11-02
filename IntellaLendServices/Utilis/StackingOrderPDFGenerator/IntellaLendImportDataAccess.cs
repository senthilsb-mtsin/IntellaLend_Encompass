using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace StackingOrderPDFGenerator
{
    public class IntellaLendImportDataAccess
    {

        #region Private Variables

        private static string TenantSchema;
        private static string SystemSchema = "IL";
        #endregion

        #region Constructor

        public IntellaLendImportDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion


        public Batch GetBatchDetails(Int64 loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanDetail loanDetail = db.LoanDetail.AsNoTracking().Where(l => l.LoanID == loanID).FirstOrDefault();

                return JsonConvert.DeserializeObject<Batch>(loanDetail.LoanObject);
            }
        }

        public Loan GetLoanInfo(Int64 loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                var loanList = db.Loan.AsNoTracking().Where(m => m.LoanID == loanID).ToList();
                if (loanList.Count > 0)
                    return loanList[0];
            }
            return null;
        }

        public Int64 InitializeEncompassUpload(Int64 LoanID, bool isMissingDocument, string MissingDocObject)
        {
            using (var db = new DBConnect(TenantSchema))
            {

                Loan _loan = db.Loan.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                if (_loan != null)
                {
                    ELoanAttachmentUpload _eUpload = new ELoanAttachmentUpload()
                    {
                        LoanID = LoanID,
                        ELoanGUID = _loan.EnCompassLoanGUID.GetValueOrDefault(),
                        TypeOfUpload = EncompassLoanAttachmentDownloadConstant.Loan,
                        Documents = string.Empty,
                        Error = string.Empty,
                        Status = EncompassStatusConstant.IMPORT_WAITING,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };

                    if (isMissingDocument)
                    {
                        _eUpload.TypeOfUpload = EncompassLoanAttachmentDownloadConstant.TrailingDocuments;
                        _eUpload.Documents = MissingDocObject;
                    }

                    db.ELoanAttachmentUpload.Add(_eUpload);
                    db.SaveChanges();

                    return _eUpload.ID;
                }

                return 0;
            }
        }

        public Boolean CheckCustomerConfigKey()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                var customerconfigkeyList = db.CustomerConfig.AsNoTracking().Where(m => (m.CustomerID == 0) && (m.ConfigKey == "Delete_Loan_Source") && (m.ConfigValue.ToLower().Equals("true")) && (m.Active)).ToList();
                if (customerconfigkeyList.Count > 0)
                    return true;
            }
            return false;
        }

        public bool GetIncLoantypeDocs()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                CustomerConfig cust = db.CustomerConfig.AsNoTracking().Where(m => m.ConfigKey == CustomerConfiguration.INCLUDE_LOANTYPE_DOCUMENTS && m.CustomerID == 0).FirstOrDefault();
                if (cust != null)
                {
                    return Convert.ToBoolean(cust.ConfigValue);
                }
                else { return false; }

            }
        }

        public void InsertLoanPdf(Batch batchObj, string pdfpath)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    if (batchObj != null)
                    {
                        LoanDetail lDetails = db.LoanDetail.AsNoTracking().Where(ld => ld.LoanID == batchObj.LoanID).FirstOrDefault();

                        if (lDetails != null)
                        {
                            lDetails.LoanObject = JsonConvert.SerializeObject(batchObj);
                            lDetails.ModifiedOn = DateTime.Now;
                            db.Entry(lDetails).State = EntityState.Modified;
                            db.SaveChanges();
                            string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.CREATED_LOAN_PDF);
                            LoanAudit.InsertLoanDetailsAudit(db, lDetails, 0, auditDescs[0], auditDescs[1]);
                        }
                    }

                    trans.Commit();
                }
            }
        }


        public List<StackingOrderDetailMaster> GetStackingOrderInfo(Int64 stackingOrderId)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.StackingOrderDetailMaster.AsNoTracking().Where(m => m.StackingOrderID == stackingOrderId).OrderBy(m => m.SequenceID).ToList();
            }
        }
        public Int64 GetStackingOrderId(Int64 customerId, Int64 reviewTypeId, Int64 loanTypeId)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                var stackingorderList = db.CustReviewLoanStackMapping.AsNoTracking().Where(m => (m.CustomerID == customerId) && (m.ReviewTypeID == reviewTypeId) && (m.LoanTypeID == loanTypeId) && (m.Active == true)).ToList();
                if (stackingorderList.Count > 0)
                    return stackingorderList[0].StackingOrderID;
            }
            return 0;
        }

        public void UpdateEUploadStatus(Int64 _eUploadID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                ELoanAttachmentUpload _eUpload = db.ELoanAttachmentUpload.AsNoTracking().Where(l => l.ID == _eUploadID).FirstOrDefault();

                if (_eUpload != null)
                {
                    _eUpload.Status = EncompassStatusConstant.UPLOAD_PENDING;
                    _eUpload.ModifiedOn = DateTime.Now;
                    db.Entry(_eUpload).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public string GetLoanPdfPath(Int64 loanId)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanPDF loanPdf = db.LoanPDF.AsNoTracking().Where(l => l.LoanID == loanId).FirstOrDefault();

                if (loanPdf != null)
                {
                    return loanPdf.LoanPDFPath;
                }
            }
            return string.Empty;
        }

        public Loan GetLoanInfo(Int64 loanID, DBConnect db)
        {
            var loanList = db.Loan.AsNoTracking().Where(m => m.LoanID == loanID).ToList();
            if (loanList.Count > 0)
                return loanList[0];

            return null;
        }

    }
}
