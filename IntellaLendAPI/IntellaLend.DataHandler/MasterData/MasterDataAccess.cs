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
    public class MasterDataAccess
    {
        private static string TenantSchema;
        private static string SystemSchema = "IL";

        #region Constructor

        public MasterDataAccess()
        { }

        public MasterDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion

        #region Public Methods

        #region Get Role, Customer, Security Question List

        public List<RoleMaster> GetRoleMaster()
        {
            List<RoleMaster> Roles = null;
            using (var db = new DBConnect(TenantSchema))
            {
                Roles = db.Roles.ToList();
            }
            return Roles;
        }

        public List<CustomerMaster> GetCustomerList()
        {
            List<CustomerMaster> cust = null;
            using (var db = new DBConnect(TenantSchema))
            {
                cust = db.CustomerMaster.Where(c => c.Active == true).ToList();
            }
            return cust;
        }

        public List<SecurityQuestionMasters> GetSecurityQuestionList()
        {
            List<SecurityQuestionMasters> SecurityQuestionLs = null;
            using (var db = new DBConnect(TenantSchema))
            {
                SecurityQuestionLs = db.SecurityQuestionMasters.Where(q => q.Active == true).ToList();
            }
            return SecurityQuestionLs;
        }

        #endregion

        #region ReviewType

        public List<ReviewTypeMaster> GetReviewTypeMaster(bool activeFilter)
        {
            List<ReviewTypeMaster> reviewTypeMasters = null;
            using (var db = new DBConnect(TenantSchema))
            {
                reviewTypeMasters = activeFilter ? db.ReviewTypeMaster.Where(r => r.Active == true && r.Type == 0).ToList() : db.ReviewTypeMaster.ToList();
            }
            return reviewTypeMasters;
        }


        public ReviewTypeMaster GetReviewType(Int64 ReviewTypeID)
        {
            ReviewTypeMaster ReviewType = new ReviewTypeMaster();
            using (var db = new DBConnect(TenantSchema))
            {
                ReviewType = db.ReviewTypeMaster.Where(l => l.ReviewTypeID == ReviewTypeID).First();
            }
            return ReviewType;
        }

        public bool UpdateReviewType(ReviewTypeMaster ReviewType)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    ReviewTypeMaster dbObject = db.ReviewTypeMaster.Where(l => l.ReviewTypeID == ReviewType.ReviewTypeID).First();
                    dbObject.ReviewTypeName = ReviewType.ReviewTypeName;
                    dbObject.Active = ReviewType.Active;
                    db.Entry(dbObject).State = EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
            }
        }
        public Int64 AddReviewTypeToSystemSchema(DBConnect db, ReviewTypeMaster reviewtype)
        {
            SystemReviewTypeMaster sReviewMaster = new SystemReviewTypeMaster()
            {
                ReviewTypeName = reviewtype.ReviewTypeName,
                Type = reviewtype.Type,
                Active = reviewtype.Active,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            if (!db.SystemReviewTypeMaster.Any(x => x.ReviewTypeName.Equals(reviewtype.ReviewTypeName)))
            {
                db.SystemReviewTypeMaster.Add(sReviewMaster);
                db.SaveChanges();
            }
            return sReviewMaster.ReviewTypeID;

        }

        public bool AddReviewType(ReviewTypeMaster ReviewType)
        {
            bool isReviewTypeAdd = false;
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    if (!db.ReviewTypeMaster.Any(x => x.ReviewTypeName.Equals(ReviewType.ReviewTypeName)))
                    {
                        ReviewType.ReviewTypeID = this.AddReviewTypeToSystemSchema(db, ReviewType);
                        ReviewType.CreatedOn = DateTime.Now;
                        ReviewType.ModifiedOn = DateTime.Now;
                        db.ReviewTypeMaster.Add(ReviewType);
                        db.SaveChanges();
                        tran.Commit();
                        isReviewTypeAdd = true;
                    }
                }
            }
            return isReviewTypeAdd;
        }

        #endregion

        #region LoanTypeMaster

        public List<LoanTypeMaster> GetLoanTypeMaster(bool activeFilter)
        {
            List<LoanTypeMaster> loanTypeMasters = null;
            using (var db = new DBConnect(TenantSchema))
            {
                loanTypeMasters = activeFilter ? db.LoanTypeMaster.Where(l => l.Active == true && l.Type == 0).ToList() : db.LoanTypeMaster.ToList();
            }
            return loanTypeMasters;
        }

        public LoanTypeMaster GetLoanType(Int64 LoanTypeID)
        {
            LoanTypeMaster LoanType = new LoanTypeMaster();
            using (var db = new DBConnect(TenantSchema))
            {
                LoanType = db.LoanTypeMaster.Where(l => l.LoanTypeID == LoanTypeID).First();
            }
            return LoanType;
        }

        public bool UpdateLoanType(LoanTypeMaster loanType)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    LoanTypeMaster dbObject = db.LoanTypeMaster.Where(l => l.LoanTypeID == loanType.LoanTypeID).First();
                    dbObject.LoanTypeName = loanType.LoanTypeName;
                    dbObject.Active = loanType.Active;
                    db.Entry(dbObject).State = EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
            }
        }
        public Int64 AddLoanTypeToSystemSchema(DBConnect db, LoanTypeMaster loantype)
        {
            SystemLoanTypeMaster systemLoanMaster = new SystemLoanTypeMaster()
            {
                LoanTypeName = loantype.LoanTypeName,
                Type = loantype.Type,
                Active = loantype.Active,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            if (!db.SystemLoanTypeMaster.Any(x => x.LoanTypeName.Equals(loantype.LoanTypeName)))
            {
                db.SystemLoanTypeMaster.Add(systemLoanMaster);
                db.SaveChanges();
            }
            return systemLoanMaster.LoanTypeID;

        }
        public bool AddLoanType(LoanTypeMaster loanType)
        {
            bool isLoanTypeAdd=false;

            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    if (!db.LoanTypeMaster.Any(x => x.LoanTypeName.Equals(loanType.LoanTypeName)))
                    {
                        loanType.LoanTypeID = this.AddLoanTypeToSystemSchema(db, loanType);
                        loanType.CreatedOn = DateTime.Now;
                        loanType.ModifiedOn = DateTime.Now;
                        db.LoanTypeMaster.Add(loanType);
                        db.SaveChanges();
                        tran.Commit();
                        isLoanTypeAdd = true;
                    }
                }
            }
            return isLoanTypeAdd;
        }

        #endregion

        #region CheckList

        public List<CheckListMaster> GetCheckListMaster(bool activeFilter)
        {
            List<CheckListMaster> CheckListMasters = null;
            using (var db = new DBConnect(TenantSchema))
            {

                CheckListMasters = activeFilter ? db.CheckListMaster.Where(l => l.Active == true).ToList() : db.CheckListMaster.ToList();
            }
            return CheckListMasters;
        }

        public CheckListMaster GetCheckList(Int64 CheckListID)
        {
            CheckListMaster CheckList = new CheckListMaster();
            using (var db = new DBConnect(TenantSchema))
            {
                CheckList = db.CheckListMaster.Where(l => l.CheckListID == CheckListID).First();
            }
            return CheckList;
        }

        public bool UpdateCheckList(CheckListMaster CheckList)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    CheckListMaster dbObject = db.CheckListMaster.Where(l => l.CheckListID == CheckList.CheckListID).First();
                    dbObject.CheckListName = CheckList.CheckListName;
                    dbObject.Active = CheckList.Active;
                    db.Entry(dbObject).State = EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
            }
        }

        public bool AddCheckList(CheckListMaster CheckList)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    db.CheckListMaster.Add(CheckList);
                    db.SaveChanges();
                    tran.Commit();
                }
            }
            return true;
        }

        #endregion

        #region StackingOrderMaster

        public List<StackingOrderMaster> GetStackingOrderMaster(bool activeFilter)
        {
            List<StackingOrderMaster> StackingOrderMasters = null;
            using (var db = new DBConnect(TenantSchema))
            {

                StackingOrderMasters = activeFilter ? db.StackingOrderMaster.Where(l => l.Active == true).ToList() : db.StackingOrderMaster.ToList();
            }
            return StackingOrderMasters;
        }

        public StackingOrderMaster GetStackingOrder(Int64 StackingOrderID)
        {
            StackingOrderMaster StackingOrder = new StackingOrderMaster();
            using (var db = new DBConnect(TenantSchema))
            {
                StackingOrder = db.StackingOrderMaster.Where(l => l.StackingOrderID == StackingOrderID).First();
            }
            return StackingOrder;
        }

        public bool UpdateStackingOrder(StackingOrderMaster StackingOrder)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    StackingOrderMaster dbObject = db.StackingOrderMaster.Where(l => l.StackingOrderID == StackingOrder.StackingOrderID).First();
                    dbObject.Description = StackingOrder.Description;
                    dbObject.Active = StackingOrder.Active;
                    db.Entry(dbObject).State = EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
            }
        }

        public bool AddStackingOrder(StackingOrderMaster StackingOrder)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    db.StackingOrderMaster.Add(StackingOrder);
                    db.SaveChanges();
                    tran.Commit();
                }
            }
            return true;
        }

        #endregion

        #region DocumentType Master

        public List<object> GetDocumentTypeMaster(bool activeFilter)
        {
            List<object> docTypeMasters = null;
            using (var db = new DBConnect(TenantSchema))
            {
                List<DocumentTypeMaster> docTypes = activeFilter ? db.DocumentTypeMaster.Where(r => r.Active == true).ToList() : db.DocumentTypeMaster.ToList();

                List<LoanTypeMaster> loanTypes = this.GetLoanTypeMaster(false);

                var docWithLoanID = (from doc in db.CustLoanDocMapping
                                     join cDoc in db.DocumentTypeMaster on doc.DocumentTypeID equals cDoc.DocumentTypeID
                                     select new
                                     {
                                         DocumentTypeID = cDoc.DocumentTypeID,
                                         Name = cDoc.Name,
                                         DisplayName = cDoc.DisplayName,
                                         Active = cDoc.Active,
                                         LoanTypeID = doc.LoanTypeID,
                                         DocumentLevel = cDoc.DocumentLevel
                                     }).ToList();

                docTypeMasters = (from docs in docWithLoanID
                                  join ln in loanTypes on docs.LoanTypeID equals ln.LoanTypeID
                                  select new
                                  {
                                      DocumentTypeID = docs.DocumentTypeID,
                                      Name = docs.Name,
                                      DisplayName = docs.DisplayName,
                                      Active = docs.Active,
                                      LoanTypeID = docs.LoanTypeID,
                                      LoanTypeName = ln.LoanTypeName,
                                      DocumentLevel=docs.DocumentLevel,
                                      DocumentLevelDesc = DocumentLevelConstant.GetDocumentLevelDescription(docs.DocumentLevel)
                                  }).ToList<object>();
            }
            return docTypeMasters;
        }

        public List<DocumentTypeMaster> GetActiveDocumentTypeMaster()
        {
            List<DocumentTypeMaster> docTypeMasters = null;
            using (var db = new DBConnect(TenantSchema))
            {
                docTypeMasters = db.DocumentTypeMaster.Where(r => r.Active == true).ToList();                
            }
            return docTypeMasters;
        }

        public bool AddDocumentType(DocumentTypeMaster documentType)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    documentType.DocumentLevel = DocumentLevelConstant.NON_CRITICAL;
                    db.DocumentTypeMaster.Add(documentType);
                    db.SaveChanges();
                    tran.Commit();
                }
            }
            return true;
        }

        public bool UpdateDocumentType(DocumentTypeMaster documentType)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    DocumentTypeMaster dbObject = db.DocumentTypeMaster.Where(l => l.DocumentTypeID.Equals(documentType.DocumentTypeID)).First();
                    dbObject.Name = documentType.Name;
                    dbObject.DisplayName = documentType.DisplayName;
                    dbObject.Active = documentType.Active;
                    dbObject.DocumentLevel = documentType.DocumentLevel;
                    db.Entry(dbObject).State = EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
            }
        }

        #endregion

        #endregion
    }
}
