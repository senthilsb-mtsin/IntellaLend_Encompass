using EncompassAPIHelper;
using EncompassRequestBody.WrapperReponseModel;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;

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
                Roles = db.Roles.AsNoTracking().Where(a => a.Active).ToList();
            }
            return Roles;
        }
        public List<RoleMasterADGroup> GetAllRoleMasterList()
        {
            List<RoleMasterADGroup> ADRoles = null;
            using (var db = new DBConnect(TenantSchema))
            {
                ADRoles = (from r in db.Roles
                           join adg in db.ADGroupMasters on r.ADGroupID equals adg.ADGroupID into lsjoin
                           from adgm in lsjoin.DefaultIfEmpty()
                           select new RoleMasterADGroup
                           {
                               RoleID = r.RoleID,
                               RoleName = r.RoleName,
                               StartPage = r.StartPage,
                               AuthorityLevel = r.AuthorityLevel,
                               IncludeKpi = r.IncludeKpi,
                               Active = r.Active,
                               ExternalRole = r.ExternalRole,
                               ADGroupID = adgm.ADGroupID == null ? 0 : adgm.ADGroupID,
                               ADGroupName = adgm.ADGroupName ?? string.Empty,
                           }).ToList();
            }
            return ADRoles;
        }
        public List<ADGroupMasters> GetAllADGroupMasterList()
        {

            List<ADGroupMasters> ADGroupMaster = null;
            using (var db = new DBConnect(TenantSchema))
            {
                ADGroupMaster = db.ADGroupMasters.AsNoTracking().ToList();
            }
            return ADGroupMaster;
        }
        public List<EncompassParkingSpot> GetEncompassParkingSpot()
        {
            List<EncompassParkingSpot> EncompassParkingSpot = null;
            using (var db = new DBConnect(SystemSchema))
            {
                EncompassParkingSpot = db.EncompassParkingSpot.AsNoTracking().ToList();
            }
            return EncompassParkingSpot;
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


        public List<ReviewPriorityMaster> GetReviewPriorityMaster()
        {
            List<ReviewPriorityMaster> lst = null;
            using (var db = new DBConnect(TenantSchema))
            {
                lst = db.ReviewPriorityMaster.Where(c => c.Active == true).ToList();
            }
            return lst;
        }
        public bool SyncDocType(string Schema)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                try
                {
                    Logger.WriteTraceLog($"{Schema} system Doctype sync - Processing");
                    db.Database.ExecuteSqlCommand($"EXEC {Schema}.SYNCDOCTYPES");
                    Logger.WriteTraceLog($"{Schema} system Doctype sync  Completed");

                    return true;
                }
                catch (Exception ex)
                {
                    MTSExceptionHandler.HandleException(ref ex);
                    throw ex;
                }
                return false;
            }
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

        public List<SystemReviewTypeMaster> GetReviewTypeMaster(bool activeFilter)
        {
            //List<ReviewTypeMaster> reviewTypeMasters = null;
            List<SystemReviewTypeMaster> reviewTypeMasters = null;
            using (var db = new DBConnect(TenantSchema))
            {
                //reviewTypeMasters = activeFilter ? db.ReviewTypeMaster.Where(r => r.Active == true && r.Type == 0).ToList() : db.ReviewTypeMaster.Where(r => r.Type == 0).ToList();
                reviewTypeMasters = activeFilter ? db.SystemReviewTypeMaster.Where(r => r.Active == true && r.Type == 0).ToList() : db.SystemReviewTypeMaster.Where(r => r.Type == 0).ToList();
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
                    SystemReviewTypeMaster dbObject = db.SystemReviewTypeMaster.AsNoTracking().Where(l => l.ReviewTypeID == ReviewType.ReviewTypeID && l.Type == 0).FirstOrDefault();
                    dbObject.ReviewTypeName = ReviewType.ReviewTypeName;
                    dbObject.ReviewTypePriority = ReviewType.ReviewTypePriority == null ? 0 : ReviewType.ReviewTypePriority;
                    dbObject.Active = ReviewType.Active;
                    dbObject.BatchClassInputPath = ReviewType.BatchClassInputPath;
                    dbObject.SearchCriteria = ReviewType.SearchCriteria;
                    dbObject.UserRoleID = ReviewType.UserRoleID;
                    db.Entry(dbObject).State = EntityState.Modified;
                    //db.SaveChanges();


                    ReviewTypeMaster rtm = db.ReviewTypeMaster.AsNoTracking().Where(l => l.ReviewTypeID == ReviewType.ReviewTypeID && l.Type == 0).FirstOrDefault();

                    if (rtm != null)
                    {
                        rtm.ReviewTypeName = ReviewType.ReviewTypeName;
                        rtm.ReviewTypePriority = ReviewType.ReviewTypePriority;
                        rtm.SearchCriteria = ReviewType.SearchCriteria;

                        rtm.UserRoleID = ReviewType.UserRoleID;
                        db.Entry(rtm).State = EntityState.Modified;
                        //db.SaveChanges();
                        // transaction.Commit();
                    }
                    db.SaveChanges();
                    transaction.Commit();

                    return true;
                }


            }


        }

        public List<object> GetActiveDocumentTypeMasterWithCustandLoan(Int64 CustomerID, Int64 LoanTypeID)
        {
            List<Object> data = new List<object>();
            using (var db = new DBConnect(TenantSchema))
            {
                List<CustLoanDocMapping> custLoanDocMap = db.CustLoanDocMapping.AsNoTracking().Where(l => l.CustomerID == CustomerID && l.LoanTypeID == LoanTypeID).ToList();
                foreach (CustLoanDocMapping doclcm in custLoanDocMap)
                {

                    DocumentTypeMaster dt = db.DocumentTypeMaster.AsNoTracking().Where(dc => dc.DocumentTypeID == doclcm.DocumentTypeID).FirstOrDefault();
                    CustomerMaster cs = db.CustomerMaster.AsNoTracking().Where(c => c.CustomerID == doclcm.CustomerID).FirstOrDefault();
                    LoanTypeMaster lt = db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeID == doclcm.LoanTypeID).FirstOrDefault();
                    if (dt != null && cs != null && lt != null)
                    {
                        data.Add(new
                        {
                            DocumentTypeID = dt.DocumentTypeID,
                            Name = dt.Name,
                            DisplayName = dt.DisplayName,
                            DocumentLevel = dt.DocumentLevel,
                            Active = dt.Active,
                            CustomerName = cs.CustomerName,
                            CustomerID = cs.CustomerID,
                            LoanTypeID = lt.LoanTypeID,
                            LoanTypeName = lt.LoanTypeName
                        });
                    }
                }
                return data;
            }
        }
        //public List<object> GetActiveDocumentTypeMasterWithCustandLoan()
        //{
        //    List<Object> data = new List<object>();
        //    using (var db = new DBConnect(TenantSchema))
        //    {
        //        List<CustLoanDocMapping> custLoanDocMap = db.CustLoanDocMapping.AsNoTracking().ToList();
        //        foreach (CustLoanDocMapping doclcm in custLoanDocMap)
        //        {
        //            DocumentTypeMaster dt = db.DocumentTypeMaster.AsNoTracking().Where(dc => dc.DocumentTypeID == doclcm.DocumentTypeID).FirstOrDefault();
        //            CustomerMaster cs = db.CustomerMaster.AsNoTracking().Where(c => c.CustomerID == doclcm.CustomerID).FirstOrDefault();
        //            LoanTypeMaster lt = db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeID == doclcm.LoanTypeID).FirstOrDefault();
        //            if (dt != null && cs != null && lt != null)
        //            {
        //                data.Add(new
        //                {
        //                    DocumentTypeID = dt.DocumentTypeID,
        //                    Name = dt.Name,
        //                    DisplayName = dt.DisplayName,
        //                    DocumentLevel = dt.DocumentLevel,
        //                    Active = dt.Active,
        //                    CustomerName = cs.CustomerName,
        //                    CustomerID = cs.CustomerID,
        //                    LoanTypeID = lt.LoanTypeID,
        //                    LoanTypeName = lt.LoanTypeName
        //                });
        //            }
        //        }
        //        return data;
        //    }
        //}

        public bool AddManagerDocumentType(DocumentTypeMaster docTypeMaster, Int64 CustomerID, Int64 LoanTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    DocumentTypeMaster dbObject = new DocumentTypeMaster()
                    {
                        Name = docTypeMaster.Name,
                        DisplayName = docTypeMaster.DisplayName,
                        Active = docTypeMaster.Active,
                        DocumentLevel = docTypeMaster.DocumentLevel,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        DocumentLevelDesc = null
                    };

                    db.SaveChanges();

                    CustLoanDocMapping custLDData = new CustLoanDocMapping()
                    {
                        CustomerID = CustomerID,
                        LoanTypeID = LoanTypeID,
                        DocumentTypeID = dbObject.DocumentTypeID,
                        CreatedOn = DateTime.Now,
                        Active = true,
                        ModifiedOn = DateTime.Now
                    };

                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
            }
        }

        public bool UpdateManagerDocumentType(DocumentTypeMaster docTypeMaster, Int64 CustomerID, Int64 LoanTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    DocumentTypeMaster dbObject = db.DocumentTypeMaster.AsNoTracking().Where(l => l.DocumentTypeID == docTypeMaster.DocumentTypeID).FirstOrDefault();
                    if (dbObject != null)
                    {
                        dbObject.Name = docTypeMaster.Name;
                        dbObject.DisplayName = docTypeMaster.DisplayName;
                        dbObject.Active = docTypeMaster.Active;
                        dbObject.DocumentLevel = docTypeMaster.DocumentLevel;
                        db.Entry(dbObject).State = EntityState.Modified;
                    }

                    CustLoanDocMapping custLDData = db.CustLoanDocMapping.AsNoTracking().Where(scl => scl.DocumentTypeID == docTypeMaster.DocumentTypeID).FirstOrDefault();

                    if (custLDData != null)
                    {
                        custLDData.CustomerID = CustomerID;
                        custLDData.LoanTypeID = LoanTypeID;
                        custLDData.ModifiedOn = DateTime.Now;
                        db.Entry(custLDData).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
            }
        }
        public List<User> GetUserMasters()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.Users.AsNoTracking().Where(us => us.Active == true).ToList();
            }
        }


        /*
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
*/
        #endregion

        #region LoanTypeMaster

        public List<LoanTypeMasterList> GetLoanTypeMaster(bool activeFilter)
        {
            List<RetainUpdateStaging> _retainupdate = null;
            List<LoanTypeMasterList> _data = null;
            _retainupdate = GetRetainUpdateStaging();
            List<LoanTypeMaster> loanTypeMasters = null;
            using (var db = new DBConnect(TenantSchema))
            {
                loanTypeMasters = activeFilter ? db.LoanTypeMaster.Where(l => l.Active == true && l.Type == 0).ToList() : db.LoanTypeMaster.Where(l => l.Type == 0).ToList();
            }

            _data = (from lt in loanTypeMasters
                     join rs in _retainupdate on lt.LoanTypeID equals rs.LoanTypeID into rsjoin
                     from Grs in rsjoin.DefaultIfEmpty()
                     select new LoanTypeMasterList
                     {
                         LoanTypeID = lt.LoanTypeID,
                         LoanTypeName = lt.LoanTypeName,
                         Active = lt.Active,
                         LoanTypePriority = lt.LoanTypePriority,
                         CreatedOn = lt.CreatedOn,
                         ModifiedOn = lt.ModifiedOn,
                         Type = lt.Type,
                         SyncStatusID = (Grs == null) ? SynchronizeConstant.DefaultVal : Grs.Synchronized,
                     }).ToList();

            return _data;
        }

        public List<RetainUpdateStaging> GetRetainUpdateStaging()
        {
            using (var sysdb = new DBConnect("T1"))
            {
                return sysdb.RetainUpdateStaging.AsNoTracking().ToList();
            }
        }
        public List<LoanTypeMaster> GetLoanTypeMaster(Int64 LoanTypeID)
        {
            List<LoanTypeMaster> loanTypeMasters = null;
            using (var db = new DBConnect(TenantSchema))
            {
                loanTypeMasters = LoanTypeID != null ? db.LoanTypeMaster.Where(l => l.Active == true && l.LoanTypeID == LoanTypeID).ToList() : db.LoanTypeMaster.Where(l => l.LoanTypeID == LoanTypeID).ToList();
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

            using (var db = new DBConnect(SystemSchema))
            {
                //using (var transaction = db.Database.BeginTransaction())
                //{

                LoanTypeMaster dbObject = db.LoanTypeMaster.Where(l => l.LoanTypeID == loanType.LoanTypeID).FirstOrDefault();

                List<TenantMaster> _tenantMs = db.TenantMaster.AsNoTracking().ToList();

                foreach (TenantMaster item in _tenantMs)
                {
                    updateLoanTypeMaters(dbObject.LoanTypeName, loanType.LoanTypeName, item.TenantSchema);
                }


                dbObject.LoanTypeName = loanType.LoanTypeName;
                dbObject.Active = loanType.Active;
                dbObject.ModifiedOn = DateTime.Now;
                db.Entry(dbObject).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
        }

        public bool updateLoanTypeMaters(string loanTypeName, string UpdateName, string Schema)
        {
            bool result = false;
            using (var db = new DBConnect(Schema))
            {
                LoanTypeMaster dbData = db.LoanTypeMaster.Where(l => l.LoanTypeName == loanTypeName).FirstOrDefault();
                if (dbData == null)
                {
                    result = true;
                }
                else if (dbData.LoanTypeName == loanTypeName && dbData != null)
                {
                    dbData.LoanTypeName = UpdateName;
                    dbData.ModifiedOn = DateTime.Now;
                    db.Entry(dbData).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return result;
        }



        /*
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
                        loanType.LoanTypePriority = loanType.LoanTypePriority == null ? 0 : loanType.LoanTypePriority;
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
*/
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

        public bool UpdateDocumentField(DocumentFieldMaster Field, Int64 AssignedFieldID)
        {
            bool result = false;

            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    if (AssignedFieldID != 0)
                    {
                        DocumentFieldMaster overRideFieldDocMaster = db.DocumentFieldMaster.AsNoTracking().Where(f => f.FieldID == AssignedFieldID).FirstOrDefault();
                        if (overRideFieldDocMaster != null)
                        {
                            overRideFieldDocMaster.DocOrderByField = null;
                            db.Entry(overRideFieldDocMaster).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    DocumentFieldMaster fDocMaster = db.DocumentFieldMaster.AsNoTracking().Where(f => f.FieldID == Field.FieldID).FirstOrDefault();

                    if (fDocMaster != null)
                    {
                        db.Entry(fDocMaster).State = EntityState.Deleted;
                        db.SaveChanges();

                        db.DocumentFieldMaster.Add(new DocumentFieldMaster()
                        {
                            DocumentTypeID = Field.DocumentTypeID,
                            Name = Field.Name,
                            DisplayName = Field.DisplayName,
                            Active = Field.Active,
                            DocOrderByField = Field.OrderBy ? "Desc" : null,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        });

                        db.SaveChanges();

                        tran.Commit();
                        result = true;
                    }
                }
            }
            return result;
        }

        public List<object> GetDocumentTypeMaster(bool activeFilter)
        {
            List<object> docTypeMasters = null;
            using (var db = new DBConnect(TenantSchema))
            {
                List<DocumentTypeMaster> docTypes = activeFilter ? db.DocumentTypeMaster.Where(r => r.Active == true).ToList() : db.DocumentTypeMaster.ToList();

                List<LoanTypeMasterList> loanTypes = this.GetLoanTypeMaster(false);

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
                                      DocumentLevel = docs.DocumentLevel,
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
        public List<DocumentTypeMaster> CheckDocumentDupForEdit(string DocumentTypeName)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                List<DocumentTypeMaster> _doc = db.DocumentTypeMaster.AsNoTracking().Where(d => d.Name == DocumentTypeName).ToList();
                return _doc;
            }
        }

        public bool CheckDocumentDup(string DocumentTypeName)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                DocumentTypeMaster _doc = db.DocumentTypeMaster.AsNoTracking().Where(d => d.Name == DocumentTypeName).FirstOrDefault();

                return (_doc != null);
            }
        }
        public List<DocumentTypeMaster> CheckManagerDocumentDupForEdit(string DocumentTypeName)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<DocumentTypeMaster> _doc = db.DocumentTypeMaster.AsNoTracking().Where(d => d.Name == DocumentTypeName).ToList();
                return _doc;
            }
        }
        public Int64 AddDocumentType(string DocumentTypeName, string DocumentDisplayName, Int32 DocumentLevel, Int64 ParkingSpotID)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    DocumentTypeMaster _doc = new DocumentTypeMaster()
                    {
                        Name = DocumentTypeName,
                        DisplayName = DocumentDisplayName,
                        DocumentLevel = DocumentLevel,
                        Active = true,
                        DocumentLevelDesc = null,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };

                    db.DocumentTypeMaster.Add(_doc);
                    db.SaveChanges();
                    EncompassParkingSpot dObject = db.EncompassParkingSpot.Where(l => l.ID == ParkingSpotID).FirstOrDefault();
                    if (dObject != null)
                    {
                        dObject.DocumentTypeID = _doc.DocumentTypeID;
                        db.Entry(dObject).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    tran.Commit();

                    return _doc.DocumentTypeID;
                }
            }
            return 0;
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

        public bool UpdateDocumentType(DocumentTypeMaster documentType, Int64 ParkingSpotID)
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
                    Int64? DocID = dbObject.DocumentTypeID;
                    EncompassParkingSpot oldDocname = db.EncompassParkingSpot.Where(l => l.DocumentTypeID == DocID).FirstOrDefault();
                    if (oldDocname != null)
                    {
                        oldDocname.DocumentTypeID = 0;
                        db.Entry(oldDocname).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    EncompassParkingSpot dObject = db.EncompassParkingSpot.Where(l => l.ID == ParkingSpotID).FirstOrDefault();
                    if (dObject != null)
                    {
                        dObject.DocumentTypeID = dbObject.DocumentTypeID;
                        db.Entry(dObject).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
            }
        }

        #endregion

        #region AD Group 

        public object AddADGroups(List<string> _ADGroups)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<ADGroupMasters> _groups = db.ADGroupMasters.AsNoTracking().ToList();

                List<string> newGroups = _ADGroups.Where(x => !_groups.Any(g => g.ADGroupName == x)).ToList();

                foreach (var item in newGroups)
                {
                    db.ADGroupMasters.Add(new ADGroupMasters()
                    {
                        ADGroupName = item,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });

                    db.SaveChanges();
                }

                return new { AvailableGroup = _groups.Select(x => x.ADGroupName).ToList(), NewGroups = newGroups };
            }
        }

        #endregion

        #region WebHook

        public AppConfig GetAppConfig(string _configKey)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.AppConfig.AsNoTracking().Where(x => x.ConfigKey == _configKey).FirstOrDefault();
            }
        }

        public object CreateWebHookSubscription(int _eventType)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                EWebhookEventCreation _req = new EWebhookEventCreation()
                {
                    events = new List<string>() { "change" },
                    resource = "Loan",
                    signingkey = ConfigurationManager.AppSettings["EWebhookSigningKey"],
                };

                if (_eventType == EWebHookEventsLogConstant.DOCUMENT_LOG)
                {
                    _req.filters = new EWebHookFilter() { attributes = new List<string>() { EWebHookEventAttribute.DocumentAttribute } };
                    _req.endpoint = ConfigurationManager.AppSettings["EncompassConnectorURL"] + GetAppConfig(ConfigConstant.WEBHOOK_DOCUMENT_CALLBACK).ConfigValue;
                }
                else if (_eventType == EWebHookEventsLogConstant.MILESTONELOG)
                {
                    _req.filters = new EWebHookFilter() { attributes = new List<string>() { EWebHookEventAttribute.MileStoneAttribute } };
                    _req.endpoint = ConfigurationManager.AppSettings["EncompassConnectorURL"] + GetAppConfig(ConfigConstant.WEBHOOK_MILESTONE_CALLBACK).ConfigValue;
                }

                EncompassWrapperAPI _api = new EncompassWrapperAPI(ConfigurationManager.AppSettings["EncompassConnectorURL"], TenantSchema.ToUpper());

                bool result = _api.CreateWebhookSubscription(_req);

                if (result)
                {
                    List<WebHookSubscriptions> _subscriptions = _api.GetWebhookSubscriptions();
                    WebHookSubscriptions _sub = _subscriptions.Where(x => x.Endpoint == _req.endpoint).FirstOrDefault();
                    if (_sub != null)
                    {
                        db.EWebhookSubscription.Add(new EWebhookSubscription() { SubscriptionID = _sub.SubscriptionID, EventType = _eventType, CreatedOn = DateTime.Now });
                        db.SaveChanges();
                    }
                    else
                        result = false;

                }

                return new { Success = result };
            }
        }

        public object DeleteWebhookSubscription(int _eventType)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                EWebhookSubscription _event = db.EWebhookSubscription.AsNoTracking().Where(x => x.EventType == _eventType).FirstOrDefault();
                if (_event != null)
                {
                    EncompassWrapperAPI _api = new EncompassWrapperAPI(ConfigurationManager.AppSettings["EncompassConnectorURL"], TenantSchema.ToUpper());
                    bool result = _api.DeleteWebhookSubscription(_event.SubscriptionID);
                    if (result)
                    {
                        db.Entry(_event).State = EntityState.Deleted;
                        db.SaveChanges();
                        return new { Success = true };
                    }
                }
            }
            return new { Success = false };
        }

        #endregion

        #region Encompass Parking Spot 

        public object AddParkingSpot(string ParkingSpotName, Boolean Active)
        {
            object data = null;
            using (var db = new DBConnect(SystemSchema))
            {
                db.EncompassParkingSpot.Add(new EncompassParkingSpot
                {
                    ParkingSpotName = ParkingSpotName,
                    Active = Active,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                });
                db.SaveChanges();
                data = GetEncompassParkingSpot();
            }
            return data;
        }
        public object UpdateParkingSpot(string ParkingSpotName, Boolean Active, Int64 ParkingSpotID)
        {
            object data = null;
            using (var db = new DBConnect(SystemSchema))
            {
                EncompassParkingSpot ParkSpotObj = db.EncompassParkingSpot.AsNoTracking().Where(x => x.ID == ParkingSpotID).FirstOrDefault();
                if (ParkSpotObj != null)
                {

                    ParkSpotObj.ParkingSpotName = ParkingSpotName;
                    ParkSpotObj.Active = Active;
                    ParkSpotObj.ModifiedOn = DateTime.Now;
                    db.Entry(ParkSpotObj).State = EntityState.Modified;
                    db.SaveChanges();
                }
                data = GetEncompassParkingSpot();
            }
            return data;
        }
        #endregion

        #endregion

    }
    public class LoanTypeMasterList
    {
        public Int64 LoanTypeID { get; set; }
        public string LoanTypeName { get; set; }
        public Int32? Type { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Int64? LoanTypePriority { get; set; }
        public Int64? SyncStatusID { get; set; }
        public string SyncStatus { get; set; }
    }
}
