using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IntellaLend.EntityDataHandler
{
    public class IntellaLendDataAccess
    {
        private static string SystemSchema = "IL";

        public IntellaLendDataAccess()
        { }

        public void SetEmailEntry(Int64 TemplateID, string EmailContent)
        {
            EmailMaster em = new EmailMaster() { EMAILSP = EmailContent, REQUESTTIME = DateTime.Now, TEMPLATEID = TemplateID, STATUS = 0 };

            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    db.EmailMaster.Add(em);
                    db.SaveChanges();
                    tran.Commit();
                }
            }
        }

        public ReviewTypeMaster GetSystemReviewType(Int64 reviewTypeID)
        {
            ReviewTypeMaster lm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                lm = db.ReviewTypeMaster.AsNoTracking().Where(l => l.ReviewTypeID == reviewTypeID && l.Active == true && l.Type == 0).FirstOrDefault();
            }

            return lm;
        }

        public List<DocumentTypeMaster> GetSystemDocumentTypesWithFields(Int64 loanTypeID)
        {
            List<DocumentTypeMaster> dm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                List<CustLoanDocMapping> cldm = db.CustLoanDocMapping.AsNoTracking().Where(cld => cld.LoanTypeID == loanTypeID && cld.Active == true).ToList();

                dm = new List<DocumentTypeMaster>();

                foreach (CustLoanDocMapping loanDocMap in cldm)
                {
                    DocumentTypeMaster doc = db.DocumentTypeMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID && ld.Active == true).FirstOrDefault();

                    List<DocumentFieldMaster> docF = db.DocumentFieldMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID && ld.Active == true).ToList();

                    doc.DocumentFieldMasters = docF;

                    dm.Add(doc);
                }
            }

            return dm;
        }

        public List<DocumentTypeMaster> GetSystemDocumentTypes(Int64 loanTypeID)
        {
            List<DocumentTypeMaster> dm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                List<CustLoanDocMapping> cldm = db.CustLoanDocMapping.AsNoTracking().Where(cld => cld.LoanTypeID == loanTypeID && cld.Active == true).ToList();

                dm = new List<DocumentTypeMaster>();

                foreach (CustLoanDocMapping loanDocMap in cldm)
                {
                    DocumentTypeMaster doc = db.DocumentTypeMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID && ld.Active == true).FirstOrDefault();

                    dm.Add(doc);
                }
            }

            return dm;
        }

        public List<DocumentFieldMaster> GetSystemDocumentFields(Int64 docTypeID)
        {
            List<DocumentFieldMaster> dfm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                dfm = db.DocumentFieldMaster.AsNoTracking().Where(cld => cld.DocumentTypeID == docTypeID && cld.Active).ToList();
            }

            return dfm;
        }
        
        public CheckListMaster GetSystemCheckLists(Int64 loanTypeID, Int64 reviewTypeID)
        {
            CheckListMaster clm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                CustReviewLoanCheckMapping clrm = db.CustReviewLoanCheckMapping.Where(r => r.LoanTypeID == loanTypeID && r.ReviewTypeID == reviewTypeID && r.Active == true).AsNoTracking().FirstOrDefault();

                if (clrm != null)
                {
                    clm = db.CheckListMaster.Where(cd => cd.CheckListID == clrm.CheckListID && cd.Active == true).AsNoTracking().FirstOrDefault();

                    if (clm != null)
                    {
                        clm.CheckListDetailMasters = db.CheckListDetailMaster.Where(cd => cd.CheckListID == clm.CheckListID && cd.Active == true).AsNoTracking().ToList();

                        foreach (CheckListDetailMaster item in clm.CheckListDetailMasters)
                        {
                            RuleMaster rm = db.RuleMaster.AsNoTracking().Where(r => r.CheckListDetailID == item.CheckListDetailID && r.Active == true).FirstOrDefault();
                            item.RuleMasters = rm;
                        }
                    }                    
                }
            }

            return clm;
        }

        public DocumentTypeMaster GetSystemDocumentType(Int64 documentTypeID)
        {
            DocumentTypeMaster documentTypeMaster = null;
            using (var db = new DBConnect(SystemSchema))
            {
                documentTypeMaster = db.DocumentTypeMaster.AsNoTracking().Where(d => d.DocumentTypeID == documentTypeID).FirstOrDefault();
            }
            return documentTypeMaster;
        }

        public StackingOrderMaster GetSystemStackingOrderMaster(Int64 loanTypeID, Int64 reviewTypeID)
        {
            StackingOrderMaster clm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                CustReviewLoanStackMapping clrm = db.CustReviewLoanStackMapping.Where(r => r.LoanTypeID == loanTypeID && r.ReviewTypeID == reviewTypeID && r.Active == true).AsNoTracking().FirstOrDefault();

                if (clrm != null)
                {
                    clm = db.StackingOrderMaster.Where(cd => cd.StackingOrderID == clrm.StackingOrderID && cd.Active == true).AsNoTracking().FirstOrDefault();

                    if (clm != null)
                        clm.StackingOrderDetailMasters = db.StackingOrderDetailMaster.Where(cd => cd.StackingOrderID == clm.StackingOrderID && cd.Active == true).AsNoTracking().ToList();                                           
                }
            }

            return clm;
        }        

        public List<WorkFlowStatusMaster> GetWorkFlowMaster()
        {
            List<WorkFlowStatusMaster> WorkFlowStatusMaster = null;
            using (var db = new DBConnect(SystemSchema))
            {
                WorkFlowStatusMaster = db.WorkFlowStatusMaster.AsNoTracking().ToList();
            }
            return WorkFlowStatusMaster;
        }

        public List<WorkFlowStatusMaster> GetSearchWorkFlowSatus()
        {
            List<WorkFlowStatusMaster> WorkFlowStatusMaster = null;
            using (var db = new DBConnect(SystemSchema))
            {
                WorkFlowStatusMaster = db.WorkFlowStatusMaster.AsNoTracking().Where(w => w.StatusID < StatusConstant.PENDING_OCR && w.StatusID != StatusConstant.PENDING_IDC).ToList();
            }
            return WorkFlowStatusMaster;
        }
        
        public List<LoanTypeMaster> GetSystemLoanTypes()
        {
            List<LoanTypeMaster> LoanTypeMaster = null;

            using (var db = new DBConnect(SystemSchema))
            {
                LoanTypeMaster = db.LoanTypeMaster.AsNoTracking().Where(l => l.Active == true && l.Type == 0).ToList();

            }
            return LoanTypeMaster;
        }

        public LoanTypeMaster GetSystemLoanTypes(Int64 loanTypeID)
        {
            LoanTypeMaster lm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                lm = db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeID == loanTypeID && l.Active == true && l.Type == 0).FirstOrDefault();
            }

            return lm;
        }

        public List<ReviewTypeMaster> GetSystemReviewTypes()
        {
            List<ReviewTypeMaster> ReviewTypeMaster = null;

            using (var db = new DBConnect(SystemSchema))
            {
                ReviewTypeMaster = db.ReviewTypeMaster.AsNoTracking().Where(l => l.Active == true && l.Type == 0).ToList();
            }
            return ReviewTypeMaster;
        }

        public List<LoanTypeMaster> GetSystemLoanTypesByMapping(Int64 reviewTypeID)
        {
            List<LoanTypeMaster> LoanTypeMaster = new List<Model.LoanTypeMaster>();

            using (var db = new DBConnect(SystemSchema))
            {
                var custReviewLoanTypes = db.CustReviewLoanMapping.AsNoTracking().Where(c => c.ReviewTypeID == reviewTypeID && c.Active == true).ToList();

                foreach (var item in custReviewLoanTypes)
                {
                    LoanTypeMaster.Add(db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeID == item.LoanTypeID && l.Active == true && l.Type == 0).FirstOrDefault());
                }
            }
            return LoanTypeMaster;
        }

    }
}
