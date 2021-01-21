using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;

namespace IntellaLend.EntityDataHandler
{
    public class MappingDataAccess
    {
        protected static string TableSchema;

        #region Constructor

        public MappingDataAccess()
        { }

        public MappingDataAccess(string tableSchema)
        {
            TableSchema = tableSchema;
        }

        #endregion

        public List<ReviewTypeMaster> CustReviewTypeMapping(Int64 CustId)
        {
            List<ReviewTypeMaster> reviewType = null;

            using (var db = new DBConnect(TableSchema))
            {
                List<CustReviewMapping> custLoanList = db.CustReviewMapping.Where(u => u.CustomerID == CustId && u.Active).ToList();

                if (custLoanList != null)
                {
                    reviewType = new List<ReviewTypeMaster>();

                    foreach (var loanList in custLoanList)
                    {
                        reviewType.Add(db.ReviewTypeMaster.AsNoTracking().Where(lt => lt.ReviewTypeID == loanList.ReviewTypeID && lt.Type == 0 && lt.Active).FirstOrDefault());
                    }
                }
            }

            return reviewType;
        }
        public List<object> GetCustLoanDocTypeMapping(Int64 loanTypeID, Int64 DocumentTypeID)
        {
            List<object> _custLoanDocs = null;

            using (var db = new DBConnect(TableSchema))
            {
                _custLoanDocs = (from dm in db.CustLoanDocMapping
                                 where dm.LoanTypeID == loanTypeID && dm.DocumentTypeID == DocumentTypeID
                                 select new
                                 {
                                     DocumentTypeID = dm.DocumentTypeID,
                                     Condition = dm.Condition,
                                     CustomerID = dm.CustomerID,
                                     LoanTypeID = dm.LoanTypeID,
                                 }).ToList<object>();
            }
            return _custLoanDocs;

        }
        public List<DocumentTypeMaster> CustLoanDocTypeMapping(Int64 customerID, Int64 loanTypeID)
        {

            List<DocumentTypeMaster> dm = null;

            using (var db = new DBConnect(TableSchema))
            {
                List<CustLoanDocMapping> cldm = db.CustLoanDocMapping.AsNoTracking().Where(cld => cld.CustomerID == customerID && cld.LoanTypeID == loanTypeID && cld.Active == true).ToList();

                dm = new List<DocumentTypeMaster>();

                foreach (CustLoanDocMapping loanDocMap in cldm)
                {
                    //
                    // DocumentTypeMaster doc = db.DocumentTypeMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID && ld.Active == true).FirstOrDefault();

                    string docQuery = db.DocumentTypeMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID && ld.Active == true).ToString();
                    DocumentTypeMaster doc = db.DocumentTypeMaster.SqlQuery(docQuery.Replace("@p__linq__0", loanDocMap.DocumentTypeID.ToString())).FirstOrDefault();




                    doc.DocumentFieldMasters = new List<DocumentFieldMaster>();

                    doc.RuleDocumentTables = new List<RuleDocumentTables>();
                    //List<RuleDocumentTables> docR = db.RuleDocumentTables.AsNoTracking().Where(rd => rd.DocumentID == loanDocMap.DocumentTypeID).ToList();

                    docQuery = db.RuleDocumentTables.AsNoTracking().Where(rd => rd.DocumentID == loanDocMap.DocumentTypeID).ToString();
                    List<RuleDocumentTables> docR = db.RuleDocumentTables.SqlQuery(docQuery.Replace("@p__linq__0", loanDocMap.DocumentTypeID.ToString())).ToList();
                    if (docR != null)
                        doc.RuleDocumentTables = docR;


                    docQuery = db.DocumentFieldMaster.AsNoTracking().Where(rd => rd.DocumentTypeID == loanDocMap.DocumentTypeID).ToString();
                    List<DocumentFieldMaster> docF = db.DocumentFieldMaster.SqlQuery(docQuery.Replace("@p__linq__0", loanDocMap.DocumentTypeID.ToString())).ToList();
                    doc.DocumentFieldMasters = docF;
                    dm.Add(doc);
                }

                List<LOSDocument> Losdocs = db.LOSDocument.AsNoTracking().ToList();
                foreach (LOSDocument los in Losdocs)
                {
                    DocumentTypeMaster data = new DocumentTypeMaster()
                    {
                        DocumentTypeID = los.LOSDocumentID,
                        Name = los.DocumentName,
                        DisplayName = los.DocumentDisplayName,
                        Active = true,
                        DocumentLevel = 0,
                        CreatedOn = los.Createdon,
                        ModifiedOn = los.ModifiedOn,
                        DocumentFieldMasters = new List<DocumentFieldMaster>(),
                        DocumetTypeTables = new List<DocumetTypeTables>(),
                        DocumentLevelDesc = "",
                        RuleDocumentTables = new List<RuleDocumentTables>(),
                        Condition = "",
                        CustDocumentLevel = 0
                    };
                    dm.Add(data);
                }
            }

            return dm.OrderBy(x => x.Name).ToList();

            //List<DocumentTypeMaster> dm = null;

            //using (var db = new DBConnect(TableSchema))
            //{
            //    List<CustLoanDocMapping> cldm = db.CustLoanDocMapping.AsNoTracking().Where(cld => cld.CustomerID == customerID && cld.LoanTypeID == loanTypeID && cld.Active).ToList();

            //    dm = new List<DocumentTypeMaster>();

            //    foreach (CustLoanDocMapping loanDocMap in cldm)
            //    {
            //        DocumentTypeMaster doc = db.DocumentTypeMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID).FirstOrDefault();

            //        if (doc != null)
            //        {
            //            doc.DocumentLevelDesc = DocumentLevelConstant.GetDocumentLevelDescription(doc.DocumentLevel);
            //            dm.Add(doc);
            //        }
            //    }
            //}

            //return dm;
        }

        public List<object> GetDocumentFieldMasters(Int64[] documentTypeID)
        {
            List<object> docFiledMasters = null;

            using (var db = new DBConnect(TableSchema))
            {
                docFiledMasters = (from dm in db.DocumentTypeMaster
                                   where (documentTypeID.Contains((int)dm.DocumentTypeID))
                                   select new
                                   {
                                       DocID = dm.DocumentTypeID,
                                       DocName = dm.Name,
                                       Fields = (from f in db.DocumentFieldMaster
                                                 where f.DocumentTypeID == dm.DocumentTypeID
                                                 select f).ToList()
                                   }).ToList<object>();
            }

            return docFiledMasters;
        }

        public List<object> GetDocumentTypesBasedonLoanType()
        {
            List<object> docTypeMasters = null;

            //using (var db = new DBConnect(TableSchema))
            //{
            //    docTypeMasters = (from dm in db.CustLoanDocMapping
            //                      where (customerID == dm.CustomerID && loanTypeID == dm.LoanTypeID)
            //                      select new
            //                      {
            //                          DocID = dm.DocumentTypeID,

            //                          DocumentName = (from f in db.DocumentTypeMaster
            //                                          where f.DocumentTypeID == dm.DocumentTypeID
            //                                          select f).ToList()
            //                      }).ToList<object>();
            //}

            using (var db = new DBConnect(TableSchema))
            {
                docTypeMasters = (from f in db.DocumentTypeMaster
                                  select new
                                  {
                                      DocID = f.DocumentTypeID,
                                      DocumentName = f.DisplayName
                                  }).ToList<object>();
            }

            return docTypeMasters;
        }

        public List<LoanTypeMaster> GetLoanTypeForCustomer(Int64 CustomerID)
        {
            List<LoanTypeMaster> loantypemaster = null;

            using (var db = new DBConnect(TableSchema))
            {

                List<CustLoanDocMapping> custdoc = db.CustLoanDocMapping.Where(c => c.CustomerID == CustomerID).Distinct().ToList();

                if (custdoc != null)
                {
                    loantypemaster = new List<LoanTypeMaster>();
                    foreach (var getloanid in custdoc)
                    {
                        loantypemaster.Add(db.LoanTypeMaster.Where(lm => lm.LoanTypeID == getloanid.LoanTypeID).FirstOrDefault());

                    }
                    loantypemaster = loantypemaster.Distinct().ToList();
                }
            }
            return loantypemaster;
        }

        public List<StackingOrderMaster> CustReviewLoanStackMapping(int customerID, int loanTypeID, int reviewTypeID)
        {
            List<StackingOrderMaster> stackMaster = null;
            using (var db = new DBConnect(TableSchema))
            {
                List<CustReviewLoanStackMapping> CLRStackList = db.CustReviewLoanStackMapping.
                     Where(u => u.CustomerID == customerID && u.LoanTypeID == loanTypeID && u.ReviewTypeID == reviewTypeID).ToList();
                if (CLRStackList != null)
                {
                    stackMaster = new List<StackingOrderMaster>();
                    foreach (var getStackingOrder in CLRStackList)
                    {
                        stackMaster.Add(db.StackingOrderMaster.Where(SO => SO.StackingOrderID == getStackingOrder.StackingOrderID).FirstOrDefault());
                    }
                    stackMaster = stackMaster.Distinct().ToList();
                }
            }
            return stackMaster;
        }

        public List<CheckListMaster> CustReviewLoanCheckListMapping(Int64 customerID, Int64 reviewTypeID, Int64 loanTypeID)
        {
            List<CheckListMaster> chklstmaster = null;

            using (var db = new DBConnect(TableSchema))
            {
                List<CustReviewLoanCheckMapping> custrevloanchkmap = db.CustReviewLoanCheckMapping.AsNoTracking()
                    .Where(u => u.CustomerID == customerID && u.LoanTypeID == loanTypeID && u.ReviewTypeID == reviewTypeID).ToList();

                if (custrevloanchkmap != null)
                {
                    chklstmaster = new List<CheckListMaster>();

                    foreach (var mappedCheckList in custrevloanchkmap)
                    {
                        chklstmaster.Add(db.CheckListMaster.AsNoTracking().Where(cm => cm.CheckListID == mappedCheckList.CheckListID).FirstOrDefault());
                    }

                    chklstmaster = chklstmaster.Distinct().ToList();
                }
            }
            return chklstmaster;
        }

        public List<CustReviewLoanUploadPath> CustReviewLoanPath(Int64 customerID, Int64 ReviewTypeId)
        {
            List<CustReviewLoanUploadPath> CustReviewLoanUploadPath;
            using (var db = new DBConnect(TableSchema))
            {
                CustReviewLoanUploadPath = db.CustReviewLoanUploadPath.Where(c => c.CustomerID == customerID && c.ReviewTypeID == ReviewTypeId).ToList();
            }
            return CustReviewLoanUploadPath;
        }
        public List<LoanTypeMaster> CustReviewLoanMapping(Int64 customerID, Int64 ReviewTypeId)
        {

            List<LoanTypeMaster> custreviewloanmaster;

            using (var db = new DBConnect(TableSchema))
            {
                List<CustReviewLoanMapping> custloanreviewmap = db.CustReviewLoanMapping.Where(u => u.CustomerID == customerID && u.ReviewTypeID == ReviewTypeId && u.Active).ToList();

                custreviewloanmaster = new List<LoanTypeMaster>();

                if (custloanreviewmap != null)
                {
                    foreach (var revmaster in custloanreviewmap)
                    {
                        custreviewloanmaster.Add(db.LoanTypeMaster.AsNoTracking().Where(rt => rt.LoanTypeID == revmaster.LoanTypeID && rt.Type == 0 && rt.Active == true).FirstOrDefault());
                    }
                    custreviewloanmaster = custreviewloanmaster.Distinct().ToList();
                }

            }
            return custreviewloanmaster;
        }

        public List<LoanTypeMaster> CustReviewLoanMapping(Int64 customerID, Int64 ReviewTypeId, List<LoanTypeMaster> sysloantypes)
        {
            List<LoanTypeMaster> custreviewloanmaster;

            using (var db = new DBConnect(TableSchema))
            {
                List<CustReviewLoanMapping> custloanreviewmap = db.CustReviewLoanMapping.Where(u => u.CustomerID == customerID && u.ReviewTypeID == ReviewTypeId && u.Active).ToList();

                custreviewloanmaster = new List<LoanTypeMaster>();

                if (custloanreviewmap != null)
                {
                    foreach (var revmaster in custloanreviewmap)
                    {
                        custreviewloanmaster.Add(db.LoanTypeMaster.AsNoTracking().Where(rt => rt.LoanTypeID == revmaster.LoanTypeID && rt.Type == 0 && rt.Active == true).FirstOrDefault());
                    }
                    custreviewloanmaster = custreviewloanmaster.Distinct().ToList();
                }

            }
            return custreviewloanmaster;
        }

        public List<DocumentTypeMaster> GetDocumentTypesWithFields(Int64 CustomerID, Int64 LoanTypeID)
        {
            List<DocumentTypeMaster> dm = null;

            using (var db = new DBConnect(TableSchema))
            {
                List<CustLoanDocMapping> cldm = db.CustLoanDocMapping.AsNoTracking().Where(cld => cld.CustomerID == CustomerID && cld.LoanTypeID == LoanTypeID && cld.Active == true).ToList();

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


        #region Mapping Page DataAccess

        #region Customer On-Boarding

        #region Customer On-Boarding Review Mapping

        public bool RemoveCustReviewMapping(Int64 customerID, Int64 reviewTypeID)
        {
            bool result = false;

            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    CustReviewMapping _custReviewMapping = db.CustReviewMapping.AsNoTracking().Where(r => r.CustomerID == customerID && r.ReviewTypeID == reviewTypeID).FirstOrDefault();
                    if (_custReviewMapping != null)
                    {
                        List<CustReviewLoanMapping> _lsCustReviewLoanMapping = db.CustReviewLoanMapping.AsNoTracking().Where(r => r.CustomerID == customerID && r.ReviewTypeID == _custReviewMapping.ReviewTypeID).ToList();
                        List<CustReviewLoanCheckMapping> _lsCustReviewLoanCheckMapping = db.CustReviewLoanCheckMapping.AsNoTracking().Where(r => r.CustomerID == customerID && r.ReviewTypeID == _custReviewMapping.ReviewTypeID).ToList();
                        List<CustReviewLoanStackMapping> _lsCustReviewLoanStackMapping = db.CustReviewLoanStackMapping.AsNoTracking().Where(r => r.CustomerID == customerID && r.ReviewTypeID == _custReviewMapping.ReviewTypeID).ToList();

                        foreach (CustReviewLoanStackMapping item in _lsCustReviewLoanStackMapping)
                        {
                            item.Active = false;
                            item.ModifiedOn = DateTime.Now;
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        foreach (CustReviewLoanCheckMapping item in _lsCustReviewLoanCheckMapping)
                        {
                            item.Active = false;
                            item.ModifiedOn = DateTime.Now;
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        foreach (CustReviewLoanMapping item in _lsCustReviewLoanMapping)
                        {
                            // List<DocumentTypeMaster> _lsDocMaster = 
                            item.Active = false;
                            item.ModifiedOn = DateTime.Now;
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                        }


                        _custReviewMapping.Active = false;
                        _custReviewMapping.ModifiedOn = DateTime.Now;
                        db.Entry(_custReviewMapping).State = EntityState.Modified;
                        db.SaveChanges();
                        result = true;
                        tran.Commit();
                    }
                }
            }

            return result;
        }

        public bool SaveCustReviewMapping(Int64 CustomerID, Int64 ReviewTypeID)
        {
            bool result = false;
            List<ReportMaster> rm = new IntellaLendDataAccess().GetReportMasters();
            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    if (db.ReviewTypeMaster.AsNoTracking().Where(l => l.ReviewTypeID == ReviewTypeID).FirstOrDefault() == null)
                    {
                        ReviewTypeMaster lm = new IntellaLendDataAccess().GetSystemReviewType(ReviewTypeID);
                        lm.ReviewTypeID = ReviewTypeID;
                        lm.CreatedOn = DateTime.Now;
                        lm.ModifiedOn = DateTime.Now;
                        lm.Active = true;
                        db.ReviewTypeMaster.Add(lm);
                        db.SaveChanges();
                    }

                    db.CustReviewMapping.RemoveRange(db.CustReviewMapping.Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID));
                    db.SaveChanges();

                    db.CustReviewMapping.Add(new CustReviewMapping()
                    {
                        CustomerID = CustomerID,
                        ReviewTypeID = ReviewTypeID,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });
                    db.SaveChanges();


                    foreach (var item in rm)
                    {
                        if (item.ReviewTypeID == ReviewTypeID)
                        {

                            List<ReportMaster> _tenanrRm = GetReportMasters(ReviewTypeID);
                            if (_tenanrRm.Count == 0)
                            {
                                ReportMaster tenantReport = new ReportMaster()
                                {
                                    ReportName = item.ReportName,
                                    ReviewTypeID = ReviewTypeID,
                                    CreatedOn = DateTime.Now,
                                    ModifiedOn = DateTime.Now
                                };
                                db.ReportMaster.Add(tenantReport);
                                db.SaveChanges();

                                List<ReportConfig> sysytemReportConfig = new IntellaLendDataAccess().GetReportMasterDocumentNames(item.ReportMasterID);

                                List<ReportConfig> _rc = new List<ReportConfig>();
                                foreach (ReportConfig systemReportConfigDetails in sysytemReportConfig)
                                {
                                    ReportConfig tenantReportConfig = new ReportConfig()
                                    {
                                        ReportMasterID = tenantReport.ReportMasterID,
                                        DocumentName = systemReportConfigDetails.DocumentName,
                                        CreatedOn = DateTime.Now,
                                        ModifiedOn = DateTime.Now
                                    };
                                    db.ReportConfig.Add(tenantReportConfig);
                                    db.SaveChanges();
                                }

                            }
                        }



                    }
                    result = true;
                    tran.Commit();
                }
                return result;
            }
        }

        public bool RetainCustReviewMapping(Int64 CustomerID, Int64 ReviewTypeID)
        {
            bool result = false;
            List<ReportMaster> rm = new IntellaLendDataAccess().GetReportMasters();
            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    CustReviewMapping _CustReviewMapping = db.CustReviewMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID).FirstOrDefault();

                    if (_CustReviewMapping != null)
                    {
                        _CustReviewMapping.Active = true;
                        _CustReviewMapping.ModifiedOn = DateTime.Now;
                        db.Entry(_CustReviewMapping).State = EntityState.Modified;
                        db.SaveChanges();

                        List<CustReviewLoanMapping> _lsCustReviewLoanMapping = db.CustReviewLoanMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID).ToList();

                        foreach (var _loanType in _lsCustReviewLoanMapping)
                        {
                            CustReviewLoanCheckMapping _loanCheck = db.CustReviewLoanCheckMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == _loanType.LoanTypeID).FirstOrDefault();
                            CustReviewLoanStackMapping _loanStack = db.CustReviewLoanStackMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == _loanType.LoanTypeID).FirstOrDefault();

                            if (_loanCheck != null && _loanStack != null)
                            {
                                _loanCheck.Active = true;
                                _loanCheck.ModifiedOn = DateTime.Now;
                                db.Entry(_loanCheck).State = EntityState.Modified;
                                db.SaveChanges();

                                _loanStack.Active = true;
                                _loanStack.ModifiedOn = DateTime.Now;
                                db.Entry(_loanStack).State = EntityState.Modified;
                                db.SaveChanges();

                                _loanType.Active = true;
                                _loanType.ModifiedOn = DateTime.Now;
                                db.Entry(_loanType).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        foreach (var item in rm)
                        {
                            if (item.ReviewTypeID == ReviewTypeID)
                            {
                                //Should Be changed if more than one documentMaster
                                List<ReportMaster> _tenanrRm = GetReportMasters(ReviewTypeID);
                                Int64 ReportMasterID = _tenanrRm[0].ReportMasterID;
                                if (_tenanrRm.Count != 0)
                                {
                                    db.ReportConfig.RemoveRange(db.ReportConfig.Where(rc => rc.ReportMasterID == ReportMasterID));
                                    db.SaveChanges();

                                    db.ReportMaster.RemoveRange(db.ReportMaster.Where(r => r.ReportMasterID == ReportMasterID));
                                    db.SaveChanges();
                                    ReportMaster tenantReport = new ReportMaster()
                                    {
                                        ReportName = item.ReportName,
                                        ReviewTypeID = ReviewTypeID,
                                        CreatedOn = DateTime.Now,
                                        ModifiedOn = DateTime.Now
                                    };
                                    db.ReportMaster.Add(tenantReport);
                                    db.SaveChanges();

                                    List<ReportConfig> sysytemReportConfig = new IntellaLendDataAccess().GetReportMasterDocumentNames(item.ReportMasterID);

                                    List<ReportConfig> _rc = new List<ReportConfig>();
                                    foreach (ReportConfig systemReportConfigDetails in sysytemReportConfig)
                                    {
                                        ReportConfig tenantReportConfig = new ReportConfig()
                                        {
                                            ReportMasterID = tenantReport.ReportMasterID,
                                            DocumentName = systemReportConfigDetails.DocumentName,
                                            CreatedOn = DateTime.Now,
                                            ModifiedOn = DateTime.Now
                                        };
                                        db.ReportConfig.Add(tenantReportConfig);
                                        db.SaveChanges();
                                    }

                                }
                            }
                        }
                        result = true;
                        tran.Commit();
                    }
                }
            }
            return result;
        }

        public bool CheckCustReviewMapping(Int64 CustomerID, Int64 ReviewTypeID)
        {
            bool result = false;

            using (var db = new DBConnect(TableSchema))
            {
                CustReviewMapping _custReviewMapping = db.CustReviewMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID).FirstOrDefault();

                if (_custReviewMapping != null)
                    result = true;

            }
            return result;
        }

        public object GetCustReviewTypes(Int64 CustomerID)
        {
            List<ReviewTypeMaster> systemReviewTypes = new IntellaLendDataAccess().GetSystemReviewTypes();
            List<ReviewTypeMaster> mappedReviewTypes = CustReviewTypeMapping(CustomerID);


            var reivewTypes = (from sys in systemReviewTypes
                               join tenant in mappedReviewTypes on sys.ReviewTypeID equals (tenant != null ? tenant.ReviewTypeID : 0) into rmJoin
                               from rmGroup in rmJoin.DefaultIfEmpty()
                               select new
                               {
                                   ReviewTypeID = sys.ReviewTypeID,
                                   ReviewTypeName = sys.ReviewTypeName,
                                   Mapped = rmGroup?.ReviewTypeID != null,
                                   DBMapped = rmGroup?.ReviewTypeID != null,
                                   SearchCriteria = sys.SearchCriteria
                               }).ToList();

            return reivewTypes;
        }

        #endregion

        #region Customer On-Boarding Loan Type Mapping

        public object CustReviewLoanTypes(Int64 CustomerID, Int64 ReviewTypeID, bool isSaveEdit)
        {
            List<LoanTypeMaster> mappedLoanTypes = CustReviewLoanMapping(CustomerID, ReviewTypeID);
            List<LoanTypeMaster> systemLoanTypes = new IntellaLendDataAccess().GetSystemLoanTypesByMapping(ReviewTypeID);
            List<LoanTypeMaster> inActiveLoanTypes = systemLoanTypes.Where(lt => lt.Active == false).ToList();
            List<LoanTypeMaster> compareLoantypes = new List<LoanTypeMaster>();

            foreach (var item in inActiveLoanTypes)
            {
                bool status = CheckCustReviewLoanMapping(CustomerID, ReviewTypeID, item.LoanTypeID);
                if (!status)
                {
                    systemLoanTypes.Remove(item);
                }
            }

            List<CustReviewLoanUploadPath> loanPath = CustReviewLoanPath(CustomerID, ReviewTypeID);

            var loanTypes = (from sys in systemLoanTypes
                             join tenant in mappedLoanTypes on sys.LoanTypeID equals tenant.LoanTypeID into rmJoin
                             from rmGroup in rmJoin.DefaultIfEmpty()
                             select new
                             {
                                 BoxUploadPath = (from path in loanPath where path.LoanTypeID == sys.LoanTypeID select path).FirstOrDefault() == null ? string.Empty : (from path in loanPath where path.LoanTypeID == sys.LoanTypeID select path).FirstOrDefault().BoxUploadPath,
                                 LoanUploadPath = (from path in loanPath where path.LoanTypeID == sys.LoanTypeID select path).FirstOrDefault() == null ? string.Empty : (from path in loanPath where path.LoanTypeID == sys.LoanTypeID select path).FirstOrDefault().UploadPath,
                                 LoanTypeID = sys.LoanTypeID,
                                 LoanTypeName = sys.LoanTypeName,
                                 Mapped = rmGroup?.LoanTypeID != null,
                                 DBMapped = rmGroup?.LoanTypeID != null
                             }).ToList();

            return loanTypes;
        }

        public bool CheckCustReviewLoanMapping(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID)
        {
            bool result = false;

            using (var db = new DBConnect(TableSchema))
            {
                CustReviewLoanMapping _custReviewLoanMapping = db.CustReviewLoanMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == LoanTypeID).FirstOrDefault();

                if (_custReviewLoanMapping != null)
                    result = true;

            }
            return result;
        }

        public bool SaveCustReviewLoanMapping(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID, string BoxUploadPath, string LoanUploadPath)
        {
            bool result = false;

            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    db.CustReviewLoanMapping.RemoveRange(db.CustReviewLoanMapping.Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == LoanTypeID));
                    db.SaveChanges();

                    CustReviewLoanCheckMapping _checkList = db.CustReviewLoanCheckMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == LoanTypeID).FirstOrDefault();

                    if (_checkList != null)
                    {
                        CheckListMaster _checkListMaster = db.CheckListMaster.AsNoTracking().Where(r => r.CheckListID == _checkList.CheckListID).FirstOrDefault();

                        if (_checkListMaster != null)
                        {
                            List<CheckListDetailMaster> _checkListDetails = db.CheckListDetailMaster.AsNoTracking().Where(r => r.CheckListID == _checkListMaster.CheckListID).ToList();

                            foreach (CheckListDetailMaster item in _checkListDetails)
                            {
                                db.RuleMaster.RemoveRange(db.RuleMaster.Where(r => r.CheckListDetailID == item.CheckListDetailID));
                                db.Entry(item).State = EntityState.Deleted;
                            }
                            db.Entry(_checkListMaster).State = EntityState.Deleted;
                        }

                        db.Entry(_checkList).State = EntityState.Deleted;
                        db.SaveChanges();
                    }

                    CustReviewLoanStackMapping _stackOrder = db.CustReviewLoanStackMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == LoanTypeID).FirstOrDefault();

                    if (_stackOrder != null)
                    {
                        StackingOrderMaster _stackOrderMaster = db.StackingOrderMaster.AsNoTracking().Where(r => r.StackingOrderID == _stackOrder.StackingOrderID).FirstOrDefault();

                        if (_stackOrderMaster != null)
                        {
                            db.StackingOrderDetailMaster.RemoveRange(db.StackingOrderDetailMaster.Where(r => r.StackingOrderID == _stackOrderMaster.StackingOrderID));
                            db.Entry(_stackOrderMaster).State = EntityState.Deleted;
                        }

                        db.Entry(_stackOrder).State = EntityState.Deleted;
                        db.SaveChanges();
                    }

                    List<CustReviewLoanReverifyMapping> _custReverify = db.CustReviewLoanReverifyMapping.Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == 0 && r.LoanTypeID == LoanTypeID).ToList();

                    foreach (var item in _custReverify)
                    {
                        db.CustReverificationDocMapping.RemoveRange(db.CustReverificationDocMapping.Where(r => r.CustomerID == CustomerID && r.ReverificationID == item.ReverificationID));

                        ReverificationMaster _rMaster = db.ReverificationMaster.AsNoTracking().Where(l => l.ReverificationID == item.ReverificationID).FirstOrDefault();

                        if (_rMaster != null)
                            db.Entry(_rMaster).State = EntityState.Deleted;
                    }
                    db.SaveChanges();

                    db.CustReviewLoanReverifyMapping.RemoveRange(_custReverify);
                    db.SaveChanges();

                    IntellaLendDataAccess _ILDataAccess = new IntellaLendDataAccess();

                    CheckListMaster _loanCheck = _ILDataAccess.GetSystemLoanCheckList(LoanTypeID);
                    StackingOrderMaster _loanStack = _ILDataAccess.GetSystemLoanStackingOrder(LoanTypeID);
                    List<CustReviewLoanReverifyMapping> _reverify = _ILDataAccess.GetSystemReVerifyMappings(LoanTypeID);
                    List<DocumentTypeMaster> _lsDocumentTypeMaster = _ILDataAccess.GetSystemDocumentTypesWithFieldsAll(LoanTypeID);

                    if (db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeID == LoanTypeID).FirstOrDefault() == null)
                    {
                        LoanTypeMaster lm = _ILDataAccess.GetSystemLoanTypes(LoanTypeID);
                        lm.LoanTypeID = LoanTypeID;
                        lm.CreatedOn = DateTime.Now;
                        lm.ModifiedOn = DateTime.Now;
                        lm.Active = true;
                        db.LoanTypeMaster.Add(lm);
                        db.SaveChanges();
                    }

                    List<CustLoanDocMapping> lsTenantLoanDocMapping = db.CustLoanDocMapping.AsNoTracking().Where(c => c.CustomerID == CustomerID && c.LoanTypeID == LoanTypeID).ToList();

                    List<DocumentTypeMaster> lsTenantDocs = (from lt in lsTenantLoanDocMapping
                                                             join dm in db.DocumentTypeMaster.AsNoTracking() on lt.DocumentTypeID equals dm.DocumentTypeID
                                                             select dm).ToList();

                    List<DocumentTypeMaster> lsTenantDocsCondition = lsTenantDocs;

                    if (lsTenantLoanDocMapping.Count == 0)
                    {
                        MapCustLoanDocuments(db, CustomerID, LoanTypeID, _lsDocumentTypeMaster);
                        lsTenantLoanDocMapping = db.CustLoanDocMapping.AsNoTracking().Where(c => c.CustomerID == CustomerID && c.LoanTypeID == LoanTypeID).ToList();
                        lsTenantDocs = (from lt in lsTenantLoanDocMapping
                                        join dm in db.DocumentTypeMaster.AsNoTracking() on lt.DocumentTypeID equals dm.DocumentTypeID
                                        select dm).ToList();
                        _lsDocumentTypeMaster = _ILDataAccess.GetSystemDocumentTypes(LoanTypeID);
                    }
                    else
                    {
                        foreach (DocumentTypeMaster dtm in lsTenantDocsCondition)
                        {
                            dtm.Condition = _lsDocumentTypeMaster.Where(x => x.Name == dtm.Name).Select(l => l.Condition).FirstOrDefault();
                        }
                        foreach (CustLoanDocMapping cldm in lsTenantLoanDocMapping)
                        {
                            cldm.Condition = lsTenantDocsCondition.Where(x => x.DocumentTypeID == cldm.DocumentTypeID).Select(l => l.Condition).FirstOrDefault();
                            cldm.ModifiedOn = DateTime.Now;
                            db.Entry(cldm).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    if (_reverify != null)
                    {
                        foreach (var item in _reverify)
                        {
                            Int64 ReverificationID = item.ReverificationID;
                            SystemReverificationMasters sysRv = _ILDataAccess.GetSystemReverification(item.ReverificationID);
                            if (sysRv != null && sysRv.Active)
                            {
                                ReverificationMaster rv = new ReverificationMaster()
                                {
                                    //ReverificationID = sysRv.ReverificationID,
                                    ReverificationID = 0,
                                    ReverificationName = sysRv.ReverificationName,
                                    CreatedOn = DateTime.Now,
                                    ModifiedOn = DateTime.Now,
                                    LogoGuid = sysRv.LogoGuid,
                                    FileName = string.IsNullOrEmpty(sysRv.FileName) ? string.Empty : sysRv.FileName,
                                    Active = true
                                };
                                db.ReverificationMaster.Add(rv);
                                db.SaveChanges();

                                ReverificationID = rv.ReverificationID;

                                List<CustReverificationDocMapping> _lsDocs = _ILDataAccess.GetSystemCustReverifyDocMapping(item.ReverificationID);

                                foreach (CustReverificationDocMapping sysReverifyDoc in _lsDocs)
                                {
                                    Int64 DocumentTypeID = sysReverifyDoc.DocumentTypeID;

                                    //DocumentTypeMaster sysRDocType = new IntellaLendDataAccess().GetSystemDocumentType(DocumentTypeID);

                                    DocumentTypeMaster sysRDocType = _lsDocumentTypeMaster.Where(d => d.DocumentTypeID == DocumentTypeID).FirstOrDefault();

                                    if (sysRDocType != null)
                                    {
                                        foreach (var itemTenant in lsTenantLoanDocMapping)
                                        {
                                            DocumentTypeMaster docType = lsTenantDocs.Where(d => d.DocumentTypeID == itemTenant.DocumentTypeID).FirstOrDefault();

                                            if (docType != null)
                                            {
                                                if (sysRDocType.Name.Equals(docType.Name))
                                                {
                                                    DocumentTypeID = docType.DocumentTypeID;
                                                    break;
                                                }
                                            }
                                        }

                                        //Insert CustReverificationDocMapping Add
                                        CustReverificationDocMapping _custReverificationDocMapping = new CustReverificationDocMapping();
                                        _custReverificationDocMapping.ID = 0;
                                        _custReverificationDocMapping.CustomerID = CustomerID;
                                        _custReverificationDocMapping.ReverificationID = ReverificationID;
                                        _custReverificationDocMapping.DocumentTypeID = DocumentTypeID;
                                        _custReverificationDocMapping.Active = true;
                                        _custReverificationDocMapping.CreatedOn = DateTime.Now;
                                        _custReverificationDocMapping.ModifiedOn = DateTime.Now;
                                        db.CustReverificationDocMapping.Add(_custReverificationDocMapping);
                                        db.SaveChanges();
                                    }
                                }


                                CustReviewLoanReverifyMapping _reVerifyMapping = new CustReviewLoanReverifyMapping();
                                _reVerifyMapping.CustomerID = CustomerID;
                                _reVerifyMapping.ReviewTypeID = 0;
                                _reVerifyMapping.LoanTypeID = LoanTypeID;
                                _reVerifyMapping.ReverificationID = ReverificationID;
                                _reVerifyMapping.TemplateID = item.TemplateID;
                                _reVerifyMapping.TemplateFields = item.TemplateFields;
                                _reVerifyMapping.Active = true;
                                _reVerifyMapping.CreatedOn = DateTime.Now;
                                _reVerifyMapping.ModifiedOn = DateTime.Now;
                                db.CustReviewLoanReverifyMapping.Add(_reVerifyMapping);
                                db.SaveChanges();
                            }
                        }
                    }

                    if (_loanCheck != null)
                    {
                        CheckListMaster _checkListMaster = new CheckListMaster();
                        _checkListMaster.CheckListID = 0;
                        _checkListMaster.CheckListName = _loanCheck.CheckListName;
                        _checkListMaster.Active = _loanCheck.Active;
                        _checkListMaster.Sync = _loanCheck.Sync;
                        _checkListMaster.CreatedOn = DateTime.Now;
                        _checkListMaster.ModifiedOn = DateTime.Now;
                        db.CheckListMaster.Add(_checkListMaster);
                        db.SaveChanges();

                        if (_loanCheck.CheckListDetailMasters != null)
                        {
                            foreach (CheckListDetailMaster _checkDetail in _loanCheck.CheckListDetailMasters)
                            {
                                CheckListDetailMaster _checkListDetailMaster = new CheckListDetailMaster();
                                _checkListDetailMaster.CheckListDetailID = 0;
                                _checkListDetailMaster.CheckListID = _checkListMaster.CheckListID;
                                _checkListDetailMaster.Description = _checkDetail.Description;
                                _checkListDetailMaster.Name = _checkDetail.Name;
                                _checkListDetailMaster.UserID = _checkDetail.UserID;
                                _checkListDetailMaster.Rule_Type = _checkDetail.Rule_Type;
                                _checkListDetailMaster.Category = _checkDetail.Category;
                                _checkListDetailMaster.LOSFieldToEvalRule = _checkDetail.LOSFieldToEvalRule;
                                _checkListDetailMaster.LOSValueToEvalRule = string.IsNullOrEmpty(_checkDetail.LOSValueToEvalRule) ? string.Empty : _checkDetail.LOSValueToEvalRule;
                                _checkListDetailMaster.Modified = false;
                                _checkListDetailMaster.SystemID = _checkDetail.CheckListDetailID;
                                _checkListDetailMaster.Active = _loanCheck.Active == true ? _checkDetail.Active : false; //_checkDetail.Active; //true;
                                _checkListDetailMaster.CreatedOn = DateTime.Now;
                                _checkListDetailMaster.ModifiedOn = DateTime.Now;
                                _checkListDetailMaster.LosIsMatched = _checkDetail.LosIsMatched;
                                db.CheckListDetailMaster.Add(_checkListDetailMaster);
                                db.SaveChanges();

                                if (_checkDetail.RuleMasters != null)
                                {

                                    lsTenantLoanDocMapping = db.CustLoanDocMapping.AsNoTracking().Where(c => c.CustomerID == CustomerID && c.LoanTypeID == LoanTypeID).ToList();
                                    lsTenantDocs = (from lt in lsTenantLoanDocMapping
                                                    join dm in db.DocumentTypeMaster.AsNoTracking() on lt.DocumentTypeID equals dm.DocumentTypeID
                                                    select dm).ToList();

                                    string _selectedDocuments = SetCustLoanDocuments(db, _ILDataAccess, CustomerID, LoanTypeID, _checkDetail.RuleMasters.DocumentType, _lsDocumentTypeMaster, lsTenantDocs);

                                    lsTenantLoanDocMapping = db.CustLoanDocMapping.AsNoTracking().Where(c => c.CustomerID == CustomerID && c.LoanTypeID == LoanTypeID).ToList();
                                    lsTenantDocs = (from lt in lsTenantLoanDocMapping
                                                    join dm in db.DocumentTypeMaster.AsNoTracking() on lt.DocumentTypeID equals dm.DocumentTypeID
                                                    select dm).ToList();

                                    string _activeDocuments = SetCustLoanDocuments(db, _ILDataAccess, CustomerID, LoanTypeID, _checkDetail.RuleMasters.ActiveDocumentType, _lsDocumentTypeMaster, lsTenantDocs);

                                    RuleMaster _ruleMaster = new RuleMaster();
                                    _ruleMaster.RuleID = 0;
                                    _ruleMaster.CheckListDetailID = _checkListDetailMaster.CheckListDetailID;
                                    _ruleMaster.RuleDescription = _checkDetail.RuleMasters.RuleDescription;
                                    _ruleMaster.RuleJson = _checkDetail.RuleMasters.RuleJson;
                                    _ruleMaster.DocumentType = _selectedDocuments;
                                    _ruleMaster.DocVersion = _checkDetail.RuleMasters.DocVersion;
                                    _ruleMaster.ActiveDocumentType = _activeDocuments;
                                    _ruleMaster.Active = true;
                                    _ruleMaster.CreatedOn = DateTime.Now;
                                    _ruleMaster.ModifiedOn = DateTime.Now;
                                    db.RuleMaster.Add(_ruleMaster);
                                    db.SaveChanges();
                                }
                            }
                        }

                        db.CustReviewLoanCheckMapping.Add(new CustReviewLoanCheckMapping()
                        {
                            CustomerID = CustomerID,
                            ReviewTypeID = ReviewTypeID,
                            LoanTypeID = LoanTypeID,
                            CheckListID = _checkListMaster.CheckListID,
                            Active = true,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        });
                        db.SaveChanges();
                    }

                    if (_loanStack != null)
                    {
                        StackingOrderMaster _stackMaster = new StackingOrderMaster();
                        _stackMaster.StackingOrderID = 0;
                        _stackMaster.Description = _loanStack.Description;
                        _stackMaster.Active = _loanStack.Active;
                        _stackMaster.CreatedOn = DateTime.Now;
                        _stackMaster.ModifiedOn = DateTime.Now;
                        db.StackingOrderMaster.Add(_stackMaster);
                        db.SaveChanges();

                        if (_loanStack.StackingOrderDetailMasters != null)
                        {
                            SetCustLoanDocumentStacking(db, CustomerID, LoanTypeID, _ILDataAccess, _loanStack.StackingOrderDetailMasters, lsTenantDocs);

                            lsTenantLoanDocMapping = db.CustLoanDocMapping.AsNoTracking().Where(c => c.CustomerID == CustomerID && c.LoanTypeID == LoanTypeID).ToList();
                            lsTenantDocs = (from lt in lsTenantLoanDocMapping
                                            join dm in db.DocumentTypeMaster.AsNoTracking() on lt.DocumentTypeID equals dm.DocumentTypeID
                                            select dm).ToList();

                            Int64 StackGroupId = 0;
                            StackingOrderGroupmasters tenantStackMasters = null;
                            string dupName = "";
                            foreach (StackingOrderDetailMaster sysStackingOrderDetail in _loanStack.StackingOrderDetailMasters)
                            {
                                Int64 DocumentTypeID = sysStackingOrderDetail.DocumentTypeID;

                                //DocumentTypeMaster sysDocType = new IntellaLendDataAccess().GetSystemDocumentType(DocumentTypeID);
                                DocumentTypeMaster sysDocType = _lsDocumentTypeMaster.Where(d => d.DocumentTypeID == DocumentTypeID).FirstOrDefault();

                                if (sysDocType != null)
                                {
                                    //foreach (var item in lsLoanDocMapping)
                                    //{
                                    //    DocumentTypeMaster docType = db.DocumentTypeMaster.AsNoTracking().Where(d => d.DocumentTypeID == item.DocumentTypeID).FirstOrDefault();

                                    //    if (docType != null)
                                    //    {
                                    //        if (sysDocType.Name.Equals(docType.Name))
                                    //        {
                                    //            DocumentTypeID = docType.DocumentTypeID;
                                    //            break;
                                    //        }
                                    //    }
                                    //}
                                    for (int i = 0; i < lsTenantLoanDocMapping.Count; i++)
                                    {
                                        var item = lsTenantLoanDocMapping[i];
                                        DocumentTypeMaster docType = lsTenantDocs.Where(d => d.DocumentTypeID == item.DocumentTypeID).FirstOrDefault();

                                        if (docType != null)
                                        {
                                            if (sysDocType.Name.Equals(docType.Name))
                                            {
                                                DocumentTypeID = docType.DocumentTypeID;
                                                break;
                                            }
                                        }
                                    }
                                    //Insert  StackingOrder Detail Table

                                    if (sysStackingOrderDetail.StackingOrderGroupID > 0)
                                    {
                                        //StackGroupId = sysStackingOrderDetail.StackingOrderGroupID;

                                        var stackingOrderGroup = new IntellaLendDataAccess().AddStackingOrderMasterGroup(sysStackingOrderDetail.StackingOrderGroupID);
                                        if (stackingOrderGroup.StackingOrderGroupName != dupName)
                                        {
                                            dupName = stackingOrderGroup.StackingOrderGroupName;
                                            //StackingOrderGroupmasters tenantStackMasters = new StackingOrderGroupmasters();

                                            tenantStackMasters = db.StackingOrderGroupmasters.Add(new StackingOrderGroupmasters()
                                            {
                                                StackingOrderID = _stackMaster.StackingOrderID,
                                                StackingOrderGroupName = stackingOrderGroup.StackingOrderGroupName,
                                                Active = true,
                                                GroupSortField = stackingOrderGroup.GroupSortField,
                                                CreatedOn = DateTime.Now,
                                                ModifiedOn = DateTime.Now
                                            });
                                            //db.StackingOrderGroupmasters.Add(stackingOrderGroup);
                                            db.SaveChanges();
                                        }
                                    }

                                    StackingOrderDetailMaster stackingOrderDetail = new StackingOrderDetailMaster();
                                    stackingOrderDetail.StackingOrderDetailID = 0;
                                    stackingOrderDetail.StackingOrderID = _stackMaster.StackingOrderID;
                                    stackingOrderDetail.DocumentTypeID = DocumentTypeID;
                                    stackingOrderDetail.SequenceID = sysStackingOrderDetail.SequenceID;
                                    stackingOrderDetail.Active = true;
                                    stackingOrderDetail.CreatedOn = DateTime.Now;
                                    stackingOrderDetail.ModifiedOn = DateTime.Now;
                                    stackingOrderDetail.StackingOrderGroupID = (sysStackingOrderDetail.StackingOrderGroupID > 0) ? tenantStackMasters.StackingOrderGroupID : 0;
                                    db.StackingOrderDetailMaster.Add(stackingOrderDetail);
                                    db.SaveChanges();
                                }
                            }
                        }


                        //Insert  Cust->Review->Loan->StackingOrder
                        db.CustReviewLoanStackMapping.Add(new CustReviewLoanStackMapping()
                        {
                            CustomerID = CustomerID,
                            ReviewTypeID = ReviewTypeID,
                            LoanTypeID = LoanTypeID,
                            StackingOrderID = _stackMaster.StackingOrderID,
                            Active = true,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        });
                        db.SaveChanges();
                    }


                    db.CustReviewLoanMapping.Add(new CustReviewLoanMapping()
                    {
                        CustomerID = CustomerID,
                        ReviewTypeID = ReviewTypeID,
                        LoanTypeID = LoanTypeID,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });
                    db.SaveChanges();

                    CustLoantypeMapping custLoantypeMapping = db.CustLoantypeMapping.AsNoTracking().Where(x => x.CustomerID == CustomerID && x.LoanTypeID == LoanTypeID).FirstOrDefault();
                    if (custLoantypeMapping == null)
                    {
                        db.CustLoantypeMapping.Add(new CustLoantypeMapping()
                        {
                            CustomerID = CustomerID,
                            LoanTypeID = LoanTypeID,
                            DocumentTypeSync = true,
                            LoanTypeSync = true,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        });
                        db.SaveChanges();
                    }



                    result = true;
                    tran.Commit();
                }
            }
            return result;
        }

        public bool SaveCustLoanUploadPath(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID, string BoxUploadPath, string LoanUploadPath, bool isRetainUpdate)
        {
            bool result = false;
            using (var db = new DBConnect(TableSchema))
            {
                CustReviewLoanUploadPath CustReviewLoanUploadPath = db.CustReviewLoanUploadPath.AsNoTracking().Where(d => d.LoanTypeID == LoanTypeID && d.ReviewTypeID == ReviewTypeID && d.CustomerID == CustomerID).FirstOrDefault();

                // var uploadpathDuplicate = db.CustReviewLoanUploadPath.AsNoTracking().Where(p => p.UploadPath == LoanUploadPath).ToList();
                if (isRetainUpdate == false)
                {
                    return true;
                }
                //else if (uploadpathDuplicate.Count == 0 || LoanUploadPath.Trim() == string.Empty )
                //{
                if (CustReviewLoanUploadPath == null)
                {
                    db.CustReviewLoanUploadPath.Add(new CustReviewLoanUploadPath()
                    {
                        CustomerID = CustomerID,
                        ReviewTypeID = ReviewTypeID,
                        LoanTypeID = LoanTypeID,
                        BoxUploadPath = BoxUploadPath,
                        UploadPath = LoanUploadPath,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    CustReviewLoanUploadPath.Active = true;
                    CustReviewLoanUploadPath.ModifiedOn = DateTime.Now;
                    CustReviewLoanUploadPath.BoxUploadPath = BoxUploadPath;
                    CustReviewLoanUploadPath.UploadPath = LoanUploadPath;
                    db.Entry(CustReviewLoanUploadPath).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                }
                //}
                //else
                //{
                //    result = false;
                //}               
            }
            return result;
        }

        public bool CheckCustLoanUploadPath(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID, string LoanUploadPath)
        {
            bool result = false;
            using (var db = new DBConnect(TableSchema))
            {
                CustReviewLoanUploadPath CustReviewLoanUploadPath = db.CustReviewLoanUploadPath.AsNoTracking().Where(d => d.LoanTypeID == LoanTypeID && d.ReviewTypeID == ReviewTypeID && d.CustomerID == CustomerID).FirstOrDefault();
                var uploadpathDuplicate = db.CustReviewLoanUploadPath.AsNoTracking().Where(p => p.UploadPath == LoanUploadPath).ToList();
                if (CustReviewLoanUploadPath != null && uploadpathDuplicate.Count == 1)
                {
                    foreach (var path in uploadpathDuplicate)
                    {
                        if (path.UploadPath == CustReviewLoanUploadPath.UploadPath)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
                else
                {
                    result = uploadpathDuplicate.Count == 0 ? true : false;
                }
            }
            return result;
        }

        public List<DocumentTypeMaster> GetMappedDocuments(DBConnect db, Int64 CustomerID, Int64 LoanTypeID)
        {
            List<DocumentTypeMaster> dm = null;

            List<CustLoanDocMapping> cldm = db.CustLoanDocMapping.AsNoTracking().Where(cld => cld.CustomerID == CustomerID && cld.LoanTypeID == LoanTypeID).ToList();

            dm = new List<DocumentTypeMaster>();

            foreach (CustLoanDocMapping loanDocMap in cldm)
            {
                DocumentTypeMaster doc = db.DocumentTypeMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID).FirstOrDefault();

                List<DocumentFieldMaster> docF = db.DocumentFieldMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID).ToList();

                doc.DocumentFieldMasters = docF;

                dm.Add(doc);
            }

            return dm;
        }

        public string MapCustLoanDocuments(DBConnect db, Int64 CustomerID, Int64 LoanTypeID, List<DocumentTypeMaster> _lsDocumentTypeMaster)
        {
            List<Int64> _insertedDocs = new List<Int64>();

            foreach (DocumentTypeMaster doc in _lsDocumentTypeMaster)
            {
                doc.DocumentTypeID = 0;
                doc.Active = true;
                doc.CreatedOn = DateTime.Now;
                doc.ModifiedOn = DateTime.Now;
                db.DocumentTypeMaster.Add(doc);
                db.SaveChanges();

                _insertedDocs.Add(doc.DocumentTypeID);

                //Insert  Cust->Loan->Document Master
                CustLoanDocMapping docLoanMap = new CustLoanDocMapping();
                docLoanMap.ID = 0;
                docLoanMap.CustomerID = CustomerID;
                docLoanMap.LoanTypeID = LoanTypeID;
                docLoanMap.DocumentTypeID = doc.DocumentTypeID;
                docLoanMap.CreatedOn = DateTime.Now;
                docLoanMap.ModifiedOn = DateTime.Now;
                docLoanMap.Active = true;
                docLoanMap.Condition = doc.Condition;
                docLoanMap.DocumentLevel = doc.CustDocumentLevel;
                db.CustLoanDocMapping.Add(docLoanMap);
                db.SaveChanges();

                //Insert Document Fields
                if (doc.DocumentFieldMasters != null)
                {
                    foreach (var field in doc.DocumentFieldMasters)
                    {
                        field.FieldID = 0;
                        //field.Active = true;
                        field.DocumentTypeID = doc.DocumentTypeID;
                        field.CreatedOn = DateTime.Now;
                        field.ModifiedOn = DateTime.Now;
                        db.DocumentFieldMaster.Add(field);
                    }
                }

                if (doc.DocumetTypeTables != null)
                {
                    foreach (var table in doc.DocumetTypeTables)
                    {
                        table.TableID = 0;
                        table.DocumentTypeID = doc.DocumentTypeID;
                        table.CreatedDate = DateTime.Now;
                        table.ModifiedDate = DateTime.Now;
                        db.DocumetTypeTables.Add(table);
                    }
                }
                if (doc.RuleDocumentTables != null)
                {
                    foreach (var ruledoc in doc.RuleDocumentTables)
                    {
                        ruledoc.ID = 0;
                        ruledoc.DocumentID = doc.DocumentTypeID;
                        ruledoc.CreatedOn = DateTime.Now;
                        ruledoc.ModifiedOn = DateTime.Now;
                        db.RuleDocumentTables.Add(ruledoc);
                    }
                }

                db.SaveChanges();
            }
            return String.Join(",", _insertedDocs);
        }
        public List<object> GetSyncLoanDetails(Int64 LoanTypeID)
        {

            List<object> result = new List<object>();
            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    List<RetainUpdateStagingDetails> retainupdateDetails = db.RetainUpdateStagingDetails.AsNoTracking().Where(r => r.LoanTypeID == LoanTypeID).OrderBy(o => o.ID).ToList();
                    foreach (var rD in retainupdateDetails)
                    {
                        if (rD != null)
                        {
                            CustomerMaster CusDetails = db.CustomerMaster.AsNoTracking().Where(c => c.CustomerID == rD.CustomerID).FirstOrDefault();
                            LoanTypeMaster _loanDetails = db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeID == rD.LoanTypeID).FirstOrDefault();
                            ReviewTypeMaster ReviewDetails = db.ReviewTypeMaster.AsNoTracking().Where(r => r.ReviewTypeID == rD.ReviewTypeID).FirstOrDefault();

                            if (CusDetails != null && _loanDetails != null && ReviewDetails != null)
                            {
                                result.Add(new
                                {
                                    ID = rD.ID,
                                    CustomerName = CusDetails.CustomerName,
                                    LoanTypeName = _loanDetails.LoanTypeName,
                                    ReviewTypeName = ReviewDetails.ReviewTypeName,
                                    Status = rD.Synchronized,
                                    ModifiedOn = rD.Synchronized == SynchronizeConstant.Completed ? rD.ModifiedOn == DateTime.MinValue ? "" : rD.ModifiedOn.GetValueOrDefault().ToString("MM/dd/yyyy hh:mm:ss tt ", CultureInfo.InvariantCulture) : "",
                                    ErrorMsg = rD.ErrorMsg
                                });
                            }
                        }
                    }
                }
                return result;
            }
        }
        public bool SyncCustomerLoanType(Int64 LoanTypeID, Int64 UserID, Int64 SyncLevel)
        {
            using (var db = new DBConnect(TableSchema))
            {
                RetainUpdateStaging _retainupdatestaging = db.RetainUpdateStaging.AsNoTracking().Where(x => x.LoanTypeID == LoanTypeID).FirstOrDefault();
                if (_retainupdatestaging == null)
                {
                    db.RetainUpdateStaging.Add(new RetainUpdateStaging()
                    {
                        LoanTypeID = LoanTypeID,
                        UserID = UserID,
                        SyncLevel = SyncLevel,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        Synchronized = SynchronizeConstant.Staged
                    });
                }
                else
                {
                    if (SyncLevel == SynchronizeConstant.RetrySync)
                    {
                        _retainupdatestaging.Synchronized = SynchronizeConstant.RetrySync;
                    }
                    else
                    {
                        _retainupdatestaging.SyncLevel = SyncLevel;
                        _retainupdatestaging.Synchronized = SynchronizeConstant.Staged;
                    }

                    _retainupdatestaging.LoanTypeID = LoanTypeID;
                    _retainupdatestaging.UserID = UserID;
                    _retainupdatestaging.SyncLevel = SyncLevel;
                    _retainupdatestaging.ModifiedOn = DateTime.Now;
                    _retainupdatestaging.Synchronized = SynchronizeConstant.Staged;
                    db.Entry(_retainupdatestaging).State = EntityState.Modified;
                }
                db.SaveChanges();
                return true;
            }
        }

        //public bool RetainCustReviewLoanMapping(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID)
        //{
        //    bool result = false;

        //    using (var db = new DBConnect(TableSchema))
        //    {
        //        using (var tran = db.Database.BeginTransaction())
        //        {
        //            CustReviewLoanMapping _CustReviewLoanMapping = db.CustReviewLoanMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == LoanTypeID).FirstOrDefault();

        //            if (_CustReviewLoanMapping != null)
        //            {
        //                _CustReviewLoanMapping.Active = true;
        //                _CustReviewLoanMapping.ModifiedOn = DateTime.Now;
        //                db.Entry(_CustReviewLoanMapping).State = EntityState.Modified;
        //                db.SaveChanges();

        //                CustReviewLoanCheckMapping _loanCheck = db.CustReviewLoanCheckMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == _CustReviewLoanMapping.LoanTypeID).FirstOrDefault();
        //                CustReviewLoanStackMapping _loanStack = db.CustReviewLoanStackMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == _CustReviewLoanMapping.LoanTypeID).FirstOrDefault();
        //                List<CustReviewLoanReverifyMapping> _reverify = db.CustReviewLoanReverifyMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == 0 && r.LoanTypeID == _CustReviewLoanMapping.LoanTypeID).ToList();

        //                IntellaLendDataAccess _ILDataAccess = new IntellaLendDataAccess();
        //                List<CustReviewLoanReverifyMapping> _sysReverify = _ILDataAccess.GetSystemReVerifyMappings(LoanTypeID);

        //                List<ReverificationMaster> _tenantReverifyMaster = new List<ReverificationMaster>();
        //                List<SystemReverificationMasters> _sysReverifyMaster = new List<SystemReverificationMasters>();

        //                if (_reverify != null)
        //                {
        //                    foreach (CustReviewLoanReverifyMapping item in _reverify)
        //                    {
        //                        ReverificationMaster _rm = db.ReverificationMaster.Where(r => r.ReverificationID == item.ReverificationID).FirstOrDefault();
        //                        if (_rm != null)
        //                            _tenantReverifyMaster.Add(_rm);

        //                        item.Active = true;
        //                        item.ModifiedOn = DateTime.Now;
        //                        db.Entry(item).State = EntityState.Modified;
        //                        db.SaveChanges();
        //                    }
        //                }

        //                if (_sysReverify != null)
        //                {
        //                    foreach (CustReviewLoanReverifyMapping item in _sysReverify)
        //                    {
        //                        SystemReverificationMasters _rm = _ILDataAccess.GetSystemReverification(item.ReverificationID);
        //                        if (_rm != null)
        //                            _sysReverifyMaster.Add(_rm);
        //                    }
        //                }

        //                var _diffReverify = _sysReverifyMaster.Where(s => !(_tenantReverifyMaster.Any(t => t.ReverificationName == s.ReverificationName))).ToList();

        //                if (_diffReverify.Count > 0)
        //                {
        //                    foreach (SystemReverificationMasters sysRv in _diffReverify)
        //                    {
        //                        Int64 ReverificationID = 0;
        //                        ReverificationMaster rv = new ReverificationMaster()
        //                        {
        //                            //ReverificationID = sysRv.ReverificationID,
        //                            ReverificationID = 0,
        //                            ReverificationName = sysRv.ReverificationName,
        //                            CreatedOn = DateTime.Now,
        //                            ModifiedOn = DateTime.Now,
        //                            Active = true
        //                        };
        //                        db.ReverificationMaster.Add(rv);
        //                        db.SaveChanges();

        //                        ReverificationID = rv.ReverificationID;

        //                        List<CustLoanDocMapping> lsTenantLoanDocMapping = db.CustLoanDocMapping.AsNoTracking().Where(c => c.CustomerID == CustomerID && c.LoanTypeID == LoanTypeID).ToList();

        //                        List<CustReverificationDocMapping> _lsDocs = _ILDataAccess.GetSystemCustReverifyDocMapping(sysRv.ReverificationID);
        //                        foreach (CustReverificationDocMapping sysReverifyDoc in _lsDocs)
        //                        {
        //                            Int64 DocumentTypeID = sysReverifyDoc.DocumentTypeID;

        //                            DocumentTypeMaster sysRDocType = new IntellaLendDataAccess().GetSystemDocumentType(DocumentTypeID);

        //                            foreach (var itemTenant in lsTenantLoanDocMapping)
        //                            {
        //                                DocumentTypeMaster docType = db.DocumentTypeMaster.AsNoTracking().Where(d => d.DocumentTypeID == itemTenant.DocumentTypeID).FirstOrDefault();

        //                                if (docType != null)
        //                                {
        //                                    if (sysRDocType.Name.Equals(docType.Name))
        //                                    {
        //                                        DocumentTypeID = docType.DocumentTypeID;
        //                                        break;
        //                                    }
        //                                }
        //                            }

        //                            //Insert CustReverificationDocMapping Add
        //                            CustReverificationDocMapping _custReverificationDocMapping = new CustReverificationDocMapping();
        //                            _custReverificationDocMapping.ID = 0;
        //                            _custReverificationDocMapping.CustomerID = CustomerID;
        //                            _custReverificationDocMapping.ReverificationID = ReverificationID;
        //                            _custReverificationDocMapping.DocumentTypeID = DocumentTypeID;
        //                            _custReverificationDocMapping.Active = true;
        //                            _custReverificationDocMapping.CreatedOn = DateTime.Now;
        //                            _custReverificationDocMapping.ModifiedOn = DateTime.Now;
        //                            db.CustReverificationDocMapping.Add(_custReverificationDocMapping);
        //                            db.SaveChanges();
        //                        }


        //                        var _sysRv = _sysReverify.Where(s => _diffReverify.Any(d => d.ReverificationID == s.ReverificationID)).FirstOrDefault();
        //                        if (_sysRv != null)
        //                        {
        //                            CustReviewLoanReverifyMapping _reVerifyMapping = new CustReviewLoanReverifyMapping();
        //                            _reVerifyMapping.CustomerID = CustomerID;
        //                            _reVerifyMapping.ReviewTypeID = 0;
        //                            _reVerifyMapping.LoanTypeID = LoanTypeID;
        //                            _reVerifyMapping.ReverificationID = ReverificationID;
        //                            _reVerifyMapping.TemplateID = _sysRv.TemplateID;
        //                            _reVerifyMapping.TemplateFields = _sysRv.TemplateFields;
        //                            _reVerifyMapping.Active = true;
        //                            _reVerifyMapping.CreatedOn = DateTime.Now;
        //                            _reVerifyMapping.ModifiedOn = DateTime.Now;
        //                            db.CustReviewLoanReverifyMapping.Add(_reVerifyMapping);
        //                            db.SaveChanges();
        //                        }
        //                    }

        //                }

        //                if (_loanCheck != null)
        //                {
        //                    _loanCheck.Active = true;
        //                    _loanCheck.ModifiedOn = DateTime.Now;
        //                    db.Entry(_loanCheck).State = EntityState.Modified;
        //                    db.SaveChanges();
        //                }

        //                if (_loanStack != null)
        //                {
        //                    _loanStack.Active = true;
        //                    _loanStack.ModifiedOn = DateTime.Now;
        //                    db.Entry(_loanStack).State = EntityState.Modified;
        //                    db.SaveChanges();
        //                }

        //                result = true;
        //                tran.Commit();
        //            }
        //        }
        //    }

        //    return result;
        //}

        public bool RemoveCustReviewLoanMapping(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID)
        {
            bool result = false;

            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    CustReviewLoanMapping _CustReviewLoanMapping = db.CustReviewLoanMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == LoanTypeID).FirstOrDefault();

                    if (_CustReviewLoanMapping != null)
                    {
                        _CustReviewLoanMapping.Active = false;
                        _CustReviewLoanMapping.ModifiedOn = DateTime.Now;
                        db.Entry(_CustReviewLoanMapping).State = EntityState.Modified;
                        db.SaveChanges();

                        CustReviewLoanCheckMapping _loanCheck = db.CustReviewLoanCheckMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == _CustReviewLoanMapping.LoanTypeID).FirstOrDefault();
                        CustReviewLoanStackMapping _loanStack = db.CustReviewLoanStackMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == _CustReviewLoanMapping.LoanTypeID).FirstOrDefault();
                        List<CustReviewLoanReverifyMapping> _reverify = db.CustReviewLoanReverifyMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == 0 && r.LoanTypeID == _CustReviewLoanMapping.LoanTypeID).ToList();

                        if (_reverify != null)
                        {
                            foreach (CustReviewLoanReverifyMapping item in _reverify)
                            {
                                item.Active = false;
                                item.ModifiedOn = DateTime.Now;
                                db.Entry(item).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }

                        if (_loanCheck != null)
                        {
                            _loanCheck.Active = false;
                            _loanCheck.ModifiedOn = DateTime.Now;
                            db.Entry(_loanCheck).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        if (_loanStack != null)
                        {
                            _loanStack.Active = false;
                            _loanStack.ModifiedOn = DateTime.Now;
                            db.Entry(_loanStack).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        result = true;
                        tran.Commit();
                    }
                }
            }

            return result;
        }
        public bool RemoveCustConfigUploadPath(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID)
        {
            bool result = false;
            using (var db = new DBConnect(TableSchema))
            {
                CustReviewLoanUploadPath CustReviewLoanUploadPath = db.CustReviewLoanUploadPath.AsNoTracking().Where(d => d.LoanTypeID == LoanTypeID && d.ReviewTypeID == ReviewTypeID && d.CustomerID == CustomerID).FirstOrDefault();
                if (CustReviewLoanUploadPath != null)
                {

                    CustReviewLoanUploadPath.Active = false;
                    CustReviewLoanUploadPath.ModifiedOn = DateTime.Now;
                    db.Entry(CustReviewLoanUploadPath).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                }
            }
            return result;
        }
        #endregion

        #region Customer On-Boarding CheckList

        public object GetCustReviewLoanCheckList(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID)
        {
            string CheckListName = string.Empty;
            Int64 CheckListID = 0;
            string StackingOrderName = string.Empty;
            Int64 StackingOrderID = 0;
            bool ConfigAvailable = false;
            bool Sync = false;
            using (var db = new DBConnect(TableSchema))
            {
                CustomerConfig _custConfig = db.CustomerConfig.AsNoTracking().Where(c => c.CustomerID == CustomerID).FirstOrDefault();

                ConfigAvailable = _custConfig != null;

                CustReviewLoanCheckMapping _CustReviewLoanCheckMapping = db.CustReviewLoanCheckMapping.AsNoTracking().Where(c => c.CustomerID == CustomerID && c.ReviewTypeID == ReviewTypeID && c.LoanTypeID == LoanTypeID && c.Active).FirstOrDefault();

                if (_CustReviewLoanCheckMapping != null)
                {
                    CheckListMaster _checkList = db.CheckListMaster.AsNoTracking().Where(c => c.CheckListID == _CustReviewLoanCheckMapping.CheckListID).FirstOrDefault();

                    if (_checkList != null)
                    {
                        CheckListID = _checkList.CheckListID;
                        CheckListName = _checkList.CheckListName;
                        Sync = _checkList.Sync.GetValueOrDefault();
                    }
                }

                CustReviewLoanStackMapping _CustReviewLoanStackMapping = db.CustReviewLoanStackMapping.AsNoTracking().Where(c => c.CustomerID == CustomerID && c.ReviewTypeID == ReviewTypeID && c.LoanTypeID == LoanTypeID && c.Active).FirstOrDefault();

                if (_CustReviewLoanStackMapping != null)
                {
                    StackingOrderMaster _stackingOrder = db.StackingOrderMaster.AsNoTracking().Where(c => c.StackingOrderID == _CustReviewLoanStackMapping.StackingOrderID).FirstOrDefault();

                    if (_stackingOrder != null)
                    {
                        StackingOrderID = _stackingOrder.StackingOrderID;
                        StackingOrderName = _stackingOrder.Description;
                    }
                }
            }

            return new { CheckListID = CheckListID, CheckListName = CheckListName, Sync = Sync, StackingOrderID = StackingOrderID, StackingOrderName = StackingOrderName, ConfigAvailable = ConfigAvailable };

        }

        #endregion


        public string SetCustLoanDocuments(DBConnect db, IntellaLendDataAccess _ILDataAccess, Int64 CustomerID, Int64 LoanTypeID, string DocIDs, List<DocumentTypeMaster> _lsSysDocMaster, List<DocumentTypeMaster> _lsTenantDocMaster)
        {
            string _returnDocIDs = string.Empty;

            List<Int64> _docIDs = string.IsNullOrEmpty(DocIDs) ? new List<Int64>() : DocIDs.Split(',').Select(i => Int64.Parse(i)).ToList<Int64>();

            if (_docIDs != null)
            {
                List<DocumentTypeMaster> _lsDocumentTypeMaster = new List<DocumentTypeMaster>();

                foreach (Int64 DocumentTypeID in _docIDs)
                {
                    DocumentTypeMaster _documentTypeMaster = _lsSysDocMaster.Where(d => d.DocumentTypeID == DocumentTypeID).FirstOrDefault();

                    //List<DocumentFieldMaster> _lsDocumentTypeFieldMaster = _ILDataAccess.GetSystemDocumentFields(DocumentTypeID);

                    //List<DocumetTypeTables> _lsDocumentTypeTables = _ILDataAccess.GetSystemDocumentTables(DocumentTypeID);

                    if (_documentTypeMaster != null)
                    {
                        //_documentTypeMaster.DocumentFieldMasters = new List<DocumentFieldMaster>();
                        //_documentTypeMaster.DocumetTypeTables = new List<DocumetTypeTables>();

                        //if (_lsDocumentTypeFieldMaster != null)
                        //{
                        //    _documentTypeMaster.DocumentFieldMasters = _lsDocumentTypeFieldMaster;
                        //}

                        //if (_lsDocumentTypeTables != null)
                        //{
                        //    _documentTypeMaster.DocumetTypeTables = _lsDocumentTypeTables;
                        //}

                        _lsDocumentTypeMaster.Add(_documentTypeMaster);
                    }
                }

                List<DocumentTypeMaster> _mappedDocs = _lsTenantDocMaster; // GetMappedDocuments(db, CustomerID, LoanTypeID);

                List<DocumentTypeMaster> _unMappedDocs = _lsDocumentTypeMaster.Where(l => !(_mappedDocs.Any(m => m.Name == l.Name))).ToList();

                _returnDocIDs = MapCustLoanDocuments(db, CustomerID, LoanTypeID, _unMappedDocs);

                List<Int64> _mappedDocIDs = _mappedDocs.Where(l => (_lsDocumentTypeMaster.Any(m => m.Name == l.Name))).Select(d => d.DocumentTypeID).ToList<Int64>();

                _returnDocIDs = string.IsNullOrEmpty(_returnDocIDs) ? string.Join(",", _mappedDocIDs) : String.Format("{0}, {1}", _returnDocIDs, string.Join(",", _mappedDocIDs));

            }

            return _returnDocIDs;
        }

        public void SetCustLoanDocumentStacking(DBConnect db, Int64 CustomerID, Int64 LoanTypeID, IntellaLendDataAccess _ILDataAccess, List<StackingOrderDetailMaster> _stackDetails, List<DocumentTypeMaster> _lsTenantDocs)
        {
            List<DocumentTypeMaster> _sysDocs = _ILDataAccess.GetAllSysDocTypeMasters();

            //List<DocumentTypeMaster> _lsDocs = _ILDataAccess.GetSysStackDocLibMatchingDocTypes(_stackDetails); //_sysDocs.Where(s => (_stackDetails.Any(d => d.DocumentTypeID == s.DocumentTypeID))).ToList();

            List<DocumentTypeMaster> _lsDocs = _sysDocs.Where(s => (_stackDetails.Any(d => d.DocumentTypeID == s.DocumentTypeID))).ToList();

            List<DocumentTypeMaster> _mappedDocs = _lsTenantDocs; //GetMappedDocuments(db, CustomerID, LoanTypeID);

            List<DocumentTypeMaster> _unMappedDocs = _lsDocs.Where(s => !(_mappedDocs.Any(m => m.Name == s.Name))).ToList();

            if (_unMappedDocs != null)
                MapCustLoanDocuments(db, CustomerID, LoanTypeID, _unMappedDocs);
        }


        #endregion

        public List<ReviewTypeMaster> GetSystemReviewTypes(Int64 CustomerID)
        {
            List<ReviewTypeMaster> systemReviewTypes = new IntellaLendDataAccess().GetSystemReviewTypes();
            List<LoanTypeMaster> SystmerLoanTypeMaster = new IntellaLendDataAccess().GetSystemLoanTypes();

            if (systemReviewTypes != null)
            {
                using (var db = new DBConnect(TableSchema))
                {
                    var custReviewTypes = db.CustReviewMapping.Where(cl => cl.CustomerID == CustomerID).AsNoTracking().ToList();

                    foreach (var item in custReviewTypes)
                    {
                        var custReviewMapping = db.CustReviewLoanMapping.Where(x => x.CustomerID == CustomerID && x.ReviewTypeID == item.ReviewTypeID).AsNoTracking().ToList();
                        var result = SystmerLoanTypeMaster.Where(p => !custReviewMapping.Any(p2 => p2.LoanTypeID == p.LoanTypeID)).ToList();
                        if (result.Count == 0)
                        {
                            systemReviewTypes.Remove(systemReviewTypes.Where(x => x.ReviewTypeID == item.ReviewTypeID).FirstOrDefault());
                        }
                    }
                }
            }
            else
            {
                systemReviewTypes = new List<ReviewTypeMaster>();
            }


            return systemReviewTypes;
        }

        public object GetCustReviewLoanTypes(Int64 customerID, Int64 reviewTypeID)
        {
            IntellaLendDataAccess ilAccess = new IntellaLendDataAccess();
            List<LoanTypeMaster> SystmerLoanTypeMaster = ilAccess.GetSystemLoanTypesByMapping(reviewTypeID);
            List<LoanTypeMaster> LoanTypeMaster = null;
            List<LoanTypeMaster> ClonedLoanTypes = null;

            if (SystmerLoanTypeMaster != null)
            {
                using (var db = new DBConnect(TableSchema))
                {
                    var reviewType = db.ReviewTypeMaster.AsNoTracking().Where(l => l.ReviewTypeID == reviewTypeID && l.Type == 0).FirstOrDefault();

                    if (reviewType != null)
                    {
                        var custReviewLoanTypes = db.CustReviewLoanMapping.Where(cl => cl.CustomerID == customerID && cl.ReviewTypeID == reviewType.ReviewTypeID).AsNoTracking().ToList();

                        foreach (var item in custReviewLoanTypes)
                        {
                            item.LoanTypeMaster = db.LoanTypeMaster.AsNoTracking().Where(rm => rm.LoanTypeID == item.LoanTypeID && rm.Type == 0).FirstOrDefault();
                        }

                        LoanTypeMaster = SystmerLoanTypeMaster.Where(x => !(custReviewLoanTypes.Any(y => x.LoanTypeName == y.LoanTypeMaster.LoanTypeName))).ToList();

                        ClonedLoanTypes = SystmerLoanTypeMaster.Where(x => (custReviewLoanTypes.Any(y => x.LoanTypeName == y.LoanTypeMaster.LoanTypeName))).ToList();
                    }
                    else
                    {
                        LoanTypeMaster = SystmerLoanTypeMaster;
                        ClonedLoanTypes = new List<LoanTypeMaster>();
                    }
                }
            }

            return new { UnMappedLoanTypes = LoanTypeMaster, MappedLoanTypes = ClonedLoanTypes };
        }

        public bool CloneFromSystem(Int64 customerID, Int64 reviewTypeID, Int64[] loanTypeIDs)
        {
            IntellaLendDataAccess ilAccess = new IntellaLendDataAccess();

            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    //Insert review                            
                    if (db.ReviewTypeMaster.AsNoTracking().Where(l => l.ReviewTypeID == reviewTypeID).FirstOrDefault() == null)
                    {
                        ReviewTypeMaster lm = ilAccess.GetSystemReviewType(reviewTypeID);
                        lm.ReviewTypeID = reviewTypeID;
                        lm.CreatedOn = DateTime.Now;
                        lm.ModifiedOn = DateTime.Now;
                        lm.Active = true;
                        db.ReviewTypeMaster.Add(lm);
                        db.SaveChanges();
                    }

                    //Customer Review Mapping
                    if (db.CustReviewMapping.AsNoTracking().Where(l => l.ReviewTypeID == reviewTypeID && l.CustomerID == customerID).FirstOrDefault() == null)
                    {
                        CustReviewMapping lm = new CustReviewMapping();
                        lm.CustReviewMappingID = 0;
                        lm.CustomerID = customerID;
                        lm.ReviewTypeID = reviewTypeID;
                        lm.CreatedOn = DateTime.Now;
                        lm.ModifiedOn = DateTime.Now;
                        lm.Active = true;
                        db.CustReviewMapping.Add(lm);
                        db.SaveChanges();
                    }

                    foreach (Int64 loanTypeID in loanTypeIDs)
                    {
                        //Insert Loan Type     
                        if (db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeID == loanTypeID).FirstOrDefault() == null)
                        {
                            LoanTypeMaster lm = ilAccess.GetSystemLoanTypes(loanTypeID);
                            lm.LoanTypeID = loanTypeID;
                            lm.CreatedOn = DateTime.Now;
                            lm.ModifiedOn = DateTime.Now;
                            lm.Active = true;
                            db.LoanTypeMaster.Add(lm);
                            db.SaveChanges();
                        }

                        //Insert Document Master
                        List<DocumentTypeMaster> docMaster = new IntellaLendDataAccess().GetSystemDocumentTypesWithFields(loanTypeID);
                        foreach (DocumentTypeMaster doc in docMaster)
                        {

                            doc.DocumentTypeID = 0;
                            doc.Active = true;
                            doc.CreatedOn = DateTime.Now;
                            doc.ModifiedOn = DateTime.Now;
                            db.DocumentTypeMaster.Add(doc);
                            db.SaveChanges();

                            //Insert  Cust->Loan->Document Master
                            CustLoanDocMapping docLoanMap = new CustLoanDocMapping();
                            docLoanMap.ID = 0;
                            docLoanMap.CustomerID = customerID;
                            docLoanMap.LoanTypeID = loanTypeID;
                            docLoanMap.DocumentTypeID = doc.DocumentTypeID;
                            docLoanMap.CreatedOn = DateTime.Now;
                            docLoanMap.ModifiedOn = DateTime.Now;
                            docLoanMap.Active = true;
                            db.CustLoanDocMapping.Add(docLoanMap);
                            db.SaveChanges();

                            //Insert Document Fields
                            foreach (var field in doc.DocumentFieldMasters)
                            {
                                field.FieldID = 0;
                                field.Active = true;
                                field.DocumentTypeID = doc.DocumentTypeID;
                                field.CreatedOn = DateTime.Now;
                                field.ModifiedOn = DateTime.Now;
                                db.DocumentFieldMaster.Add(field);
                                db.SaveChanges();
                            }
                        }
                        //}

                        //Customer->Review->loan Mapping
                        if (db.CustReviewLoanMapping.AsNoTracking().Where(l => l.ReviewTypeID == reviewTypeID && l.CustomerID == customerID && l.LoanTypeID == loanTypeID).FirstOrDefault() == null)
                        {
                            CustReviewLoanMapping crl = new CustReviewLoanMapping();
                            crl.ID = 0;
                            crl.CustomerID = customerID;
                            crl.ReviewTypeID = reviewTypeID;
                            crl.LoanTypeID = loanTypeID;
                            crl.Active = true;
                            crl.CreatedOn = DateTime.Now;
                            crl.ModifiedOn = DateTime.Now;
                            db.CustReviewLoanMapping.Add(crl);
                            db.SaveChanges();
                        }

                        //Check Lists
                        CheckListMaster sysCheckListMaster = new IntellaLendDataAccess().GetSystemCheckLists(loanTypeID, reviewTypeID);
                        if (sysCheckListMaster != null)
                        {
                            //Insert  CheckList Master Table
                            CheckListMaster checkListMaster = new CheckListMaster();
                            checkListMaster.CheckListID = 0;
                            checkListMaster.CheckListName = sysCheckListMaster.CheckListName;
                            checkListMaster.Active = true;
                            checkListMaster.CreatedOn = DateTime.Now;
                            checkListMaster.ModifiedOn = DateTime.Now;
                            db.CheckListMaster.Add(checkListMaster);
                            db.SaveChanges();

                            if (sysCheckListMaster.CheckListDetailMasters != null)
                            {
                                foreach (CheckListDetailMaster sysCheckListDetailMasters in sysCheckListMaster.CheckListDetailMasters)
                                {
                                    //Insert  CheckList Master Table
                                    CheckListDetailMaster checkListDetailMaster = new CheckListDetailMaster();
                                    checkListDetailMaster.CheckListDetailID = 0;
                                    checkListDetailMaster.CheckListID = checkListMaster.CheckListID;
                                    checkListDetailMaster.Description = sysCheckListDetailMasters.Description;
                                    checkListDetailMaster.Active = true;
                                    checkListDetailMaster.Name = sysCheckListDetailMasters.Name;
                                    checkListDetailMaster.CreatedOn = DateTime.Now;
                                    checkListDetailMaster.ModifiedOn = DateTime.Now;
                                    checkListDetailMaster.LosIsMatched = sysCheckListDetailMasters.LosIsMatched;
                                    db.CheckListDetailMaster.Add(checkListDetailMaster);
                                    db.SaveChanges();

                                    if (sysCheckListDetailMasters.RuleMasters != null)
                                    {
                                        RuleMaster ruleMaster = new RuleMaster();
                                        ruleMaster.RuleID = 0;
                                        ruleMaster.CheckListDetailID = checkListDetailMaster.CheckListDetailID;
                                        ruleMaster.RuleDescription = sysCheckListDetailMasters.RuleMasters.RuleDescription;
                                        ruleMaster.RuleJson = sysCheckListDetailMasters.RuleMasters.RuleJson;
                                        ruleMaster.DocumentType = sysCheckListDetailMasters.RuleMasters.DocumentType;
                                        ruleMaster.ActiveDocumentType = sysCheckListDetailMasters.RuleMasters.ActiveDocumentType;
                                        ruleMaster.Active = true;
                                        ruleMaster.CreatedOn = DateTime.Now;
                                        ruleMaster.ModifiedOn = DateTime.Now;
                                        db.RuleMaster.Add(ruleMaster);
                                        db.SaveChanges();
                                    }
                                }
                            }

                            //Insert  Cust->Review->Loan->CheckList
                            CustReviewLoanCheckMapping custReviewLoanCheckMap = new CustReviewLoanCheckMapping();
                            custReviewLoanCheckMap.ID = 0;
                            custReviewLoanCheckMap.CustomerID = customerID;
                            custReviewLoanCheckMap.ReviewTypeID = reviewTypeID;
                            custReviewLoanCheckMap.LoanTypeID = loanTypeID;
                            custReviewLoanCheckMap.CheckListID = checkListMaster.CheckListID;
                            custReviewLoanCheckMap.CreatedOn = DateTime.Now;
                            custReviewLoanCheckMap.ModifiedOn = DateTime.Now;
                            custReviewLoanCheckMap.Active = true;
                            db.CustReviewLoanCheckMapping.Add(custReviewLoanCheckMap);
                            db.SaveChanges();
                        }


                        //Stacking Order
                        StackingOrderMaster sysStackingOrder = new IntellaLendDataAccess().GetSystemStackingOrderMaster(loanTypeID, reviewTypeID);
                        if (sysStackingOrder != null)
                        {
                            //Insert  CheckList Master Table
                            StackingOrderMaster stackingOrder = new StackingOrderMaster();
                            stackingOrder.StackingOrderID = 0;
                            stackingOrder.Description = sysStackingOrder.Description;
                            stackingOrder.Active = true;
                            stackingOrder.CreatedOn = DateTime.Now;
                            stackingOrder.ModifiedOn = DateTime.Now;
                            db.StackingOrderMaster.Add(stackingOrder);
                            db.SaveChanges();


                            if (sysStackingOrder.StackingOrderDetailMasters != null)
                            {
                                foreach (StackingOrderDetailMaster sysStackingOrderDetail in sysStackingOrder.StackingOrderDetailMasters)
                                {
                                    Int64 DocumentTypeID = sysStackingOrderDetail.DocumentTypeID;

                                    List<CustLoanDocMapping> lsLoanDocMapping = db.CustLoanDocMapping.AsNoTracking().Where(c => c.LoanTypeID == loanTypeID).ToList();

                                    foreach (var item in lsLoanDocMapping)
                                    {
                                        DocumentTypeMaster sysDocType = new IntellaLendDataAccess().GetSystemDocumentType(sysStackingOrderDetail.DocumentTypeID);

                                        DocumentTypeMaster docType = db.DocumentTypeMaster.AsNoTracking().Where(d => d.DocumentTypeID == item.DocumentTypeID).FirstOrDefault();

                                        if (docType != null)
                                        {
                                            if (sysDocType.Name.Equals(docType.Name))
                                                DocumentTypeID = docType.DocumentTypeID;
                                        }
                                    }

                                    //Insert  StackingOrder Detail Table
                                    StackingOrderDetailMaster stackingOrderDetail = new StackingOrderDetailMaster();
                                    stackingOrderDetail.StackingOrderDetailID = 0;
                                    stackingOrderDetail.StackingOrderID = stackingOrder.StackingOrderID;
                                    stackingOrderDetail.DocumentTypeID = DocumentTypeID;
                                    stackingOrderDetail.SequenceID = sysStackingOrderDetail.SequenceID;
                                    stackingOrderDetail.Active = true;
                                    stackingOrderDetail.CreatedOn = DateTime.Now;
                                    stackingOrderDetail.ModifiedOn = DateTime.Now;
                                    db.StackingOrderDetailMaster.Add(stackingOrderDetail);
                                    db.SaveChanges();
                                }
                            }

                            //Insert  Cust->Review->Loan->StackingOrder
                            CustReviewLoanStackMapping custReviewLoanStackMap = new CustReviewLoanStackMapping();
                            custReviewLoanStackMap.ID = 0;
                            custReviewLoanStackMap.CustomerID = customerID;
                            custReviewLoanStackMap.ReviewTypeID = reviewTypeID;
                            custReviewLoanStackMap.LoanTypeID = loanTypeID;
                            custReviewLoanStackMap.StackingOrderID = stackingOrder.StackingOrderID;
                            custReviewLoanStackMap.CreatedOn = DateTime.Now;
                            custReviewLoanStackMap.ModifiedOn = DateTime.Now;
                            custReviewLoanStackMap.Active = true;
                            db.CustReviewLoanStackMapping.Add(custReviewLoanStackMap);
                            db.SaveChanges();
                        }
                    }

                    tran.Commit();
                }

                return true;
            }

        }

        #endregion
        public bool RetainCustReviewLoanMapping(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID)
        {
            bool result = false;

            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    CustReviewLoanMapping _CustReviewLoanMapping = db.CustReviewLoanMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == LoanTypeID).FirstOrDefault();

                    if (_CustReviewLoanMapping != null)
                    {
                        _CustReviewLoanMapping.Active = true;
                        _CustReviewLoanMapping.ModifiedOn = DateTime.Now;
                        db.Entry(_CustReviewLoanMapping).State = EntityState.Modified;
                        db.SaveChanges();

                        CustReviewLoanCheckMapping _loanCheck = db.CustReviewLoanCheckMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == _CustReviewLoanMapping.LoanTypeID).FirstOrDefault();
                        CustReviewLoanStackMapping _loanStack = db.CustReviewLoanStackMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == _CustReviewLoanMapping.LoanTypeID).FirstOrDefault();
                        List<CustReviewLoanReverifyMapping> _reverify = db.CustReviewLoanReverifyMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == 0 && r.LoanTypeID == _CustReviewLoanMapping.LoanTypeID).ToList();

                        IntellaLendDataAccess _ILDataAccess = new IntellaLendDataAccess();
                        List<CustReviewLoanReverifyMapping> _sysReverify = _ILDataAccess.GetSystemReVerifyMappings(LoanTypeID);
                        SystemReverificationMasters _srm = new SystemReverificationMasters();
                        // _srm = _ILDataAccess.GetSystemReverification(_sysReverify;
                        List<DocumentTypeMaster> _sysDocumentTypeMaster = _ILDataAccess.GetSystemDocumentTypesWithFields(LoanTypeID);
                        List<DocumentTypeMaster> _tenantDocumentTypeMaster = _ILDataAccess.GetDocumentTypesWithFields(TableSchema, CustomerID, LoanTypeID);
                        string _insertedDocs = string.Empty;

                        List<ReverificationMaster> _tenantReverifyMaster = new List<ReverificationMaster>();
                        List<SystemReverificationMasters> _sysReverifyMaster = new List<SystemReverificationMasters>();

                        List<CustLoanDocMapping> lsTenantLoanDocMapping = db.CustLoanDocMapping.AsNoTracking().Where(c => c.CustomerID == CustomerID && c.LoanTypeID == LoanTypeID).ToList();

                        if (_reverify != null)
                        {
                            foreach (CustReviewLoanReverifyMapping item in _reverify)
                            {
                                ReverificationMaster _rm = db.ReverificationMaster.Where(r => r.ReverificationID == item.ReverificationID).FirstOrDefault();
                                if (_rm != null)
                                    _tenantReverifyMaster.Add(_rm);

                                item.Active = true;
                                item.ModifiedOn = DateTime.Now;
                                db.Entry(item).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }

                        if (_sysReverify != null)
                        {
                            foreach (CustReviewLoanReverifyMapping item in _sysReverify)
                            {
                                SystemReverificationMasters _rm = _ILDataAccess.GetSystemReverification(item.ReverificationID);
                                if (_rm != null)
                                    _sysReverifyMaster.Add(_rm);
                            }
                        }

                        var _diffReverify = _sysReverifyMaster.Where(s => !(_tenantReverifyMaster.Any(t => t.ReverificationName == s.ReverificationName))).ToList();


                        var _tenantReverificationUpdate = _tenantReverifyMaster.Where(s => (_sysReverifyMaster.Any(t => t.ReverificationName == s.ReverificationName))).ToList();
                        if (_tenantReverificationUpdate.Count > 0)
                        {
                            foreach (ReverificationMaster _item in _tenantReverificationUpdate)
                            {
                                SystemReverificationMasters _rev = _sysReverifyMaster.Where(s => s.ReverificationName == _item.ReverificationName).FirstOrDefault();

                                //ReverificationID = sysRv.ReverificationID,
                                _item.LogoGuid = _rev.LogoGuid;
                                _item.FileName = string.IsNullOrEmpty(_rev.FileName) ? string.Empty : _rev.FileName;
                                _item.ModifiedOn = DateTime.Now;
                                db.Entry(_item).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }

                        if (_diffReverify.Count > 0)
                        {
                            foreach (SystemReverificationMasters sysRv in _diffReverify)
                            {
                                Int64 ReverificationID = 0;
                                ReverificationMaster rv = new ReverificationMaster()
                                {
                                    //ReverificationID = sysRv.ReverificationID,
                                    ReverificationID = 0,
                                    ReverificationName = sysRv.ReverificationName,
                                    LogoGuid = sysRv.LogoGuid,
                                    FileName = string.IsNullOrEmpty(sysRv.FileName) ? string.Empty : sysRv.FileName,
                                    CreatedOn = DateTime.Now,
                                    ModifiedOn = DateTime.Now,
                                    Active = true
                                };
                                db.ReverificationMaster.Add(rv);
                                db.SaveChanges();

                                ReverificationID = rv.ReverificationID;



                                List<CustReverificationDocMapping> _lsDocs = _ILDataAccess.GetSystemCustReverifyDocMapping(sysRv.ReverificationID);
                                foreach (CustReverificationDocMapping sysReverifyDoc in _lsDocs)
                                {
                                    Int64 DocumentTypeID = sysReverifyDoc.DocumentTypeID;

                                    DocumentTypeMaster sysRDocType = new IntellaLendDataAccess().GetSystemDocumentType(DocumentTypeID);

                                    foreach (var itemTenant in lsTenantLoanDocMapping)
                                    {
                                        DocumentTypeMaster docType = db.DocumentTypeMaster.AsNoTracking().Where(d => d.DocumentTypeID == itemTenant.DocumentTypeID).FirstOrDefault();

                                        if (docType != null)
                                        {
                                            if (sysRDocType.Name.Equals(docType.Name))
                                            {
                                                DocumentTypeID = docType.DocumentTypeID;
                                                break;
                                            }
                                        }
                                    }

                                    //Insert CustReverificationDocMapping Add
                                    CustReverificationDocMapping _custReverificationDocMapping = new CustReverificationDocMapping();
                                    _custReverificationDocMapping.ID = 0;
                                    _custReverificationDocMapping.CustomerID = CustomerID;
                                    _custReverificationDocMapping.ReverificationID = ReverificationID;
                                    _custReverificationDocMapping.DocumentTypeID = DocumentTypeID;
                                    _custReverificationDocMapping.Active = true;
                                    _custReverificationDocMapping.CreatedOn = DateTime.Now;
                                    _custReverificationDocMapping.ModifiedOn = DateTime.Now;
                                    db.CustReverificationDocMapping.Add(_custReverificationDocMapping);
                                    db.SaveChanges();
                                }


                                var _sysRv = _sysReverify.Where(s => _diffReverify.Any(d => d.ReverificationID == s.ReverificationID)).FirstOrDefault();
                                if (_sysRv != null)
                                {
                                    CustReviewLoanReverifyMapping _reVerifyMapping = new CustReviewLoanReverifyMapping();
                                    _reVerifyMapping.CustomerID = CustomerID;
                                    _reVerifyMapping.ReviewTypeID = 0;
                                    _reVerifyMapping.LoanTypeID = LoanTypeID;
                                    _reVerifyMapping.ReverificationID = ReverificationID;
                                    _reVerifyMapping.TemplateID = _sysRv.TemplateID;
                                    _reVerifyMapping.TemplateFields = _sysRv.TemplateFields;
                                    _reVerifyMapping.Active = true;
                                    _reVerifyMapping.CreatedOn = DateTime.Now;
                                    _reVerifyMapping.ModifiedOn = DateTime.Now;
                                    db.CustReviewLoanReverifyMapping.Add(_reVerifyMapping);
                                    db.SaveChanges();
                                }
                            }

                        }

                        if (_loanCheck != null)
                        {
                            _loanCheck.Active = true;
                            _loanCheck.ModifiedOn = DateTime.Now;
                            db.Entry(_loanCheck).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        if (_sysDocumentTypeMaster != null && _tenantDocumentTypeMaster != null)
                        {
                            List<DocumentTypeMaster> _diffDocs = _sysDocumentTypeMaster.Where(sysDoc => !(_tenantDocumentTypeMaster.Any(tDoc => tDoc.Name == sysDoc.Name))).ToList();

                            if (_diffDocs.Count > 0)
                            {
                                _insertedDocs = MapCustLoanDocuments(db, CustomerID, LoanTypeID, _diffDocs);
                            }
                            else
                            {
                                foreach (DocumentTypeMaster dtm in _tenantDocumentTypeMaster)
                                {
                                    dtm.Condition = _sysDocumentTypeMaster.Where(x => x.Name == dtm.Name).Select(l => l.Condition).FirstOrDefault();
                                }
                                foreach (CustLoanDocMapping cldm in lsTenantLoanDocMapping)
                                {
                                    cldm.Condition = _tenantDocumentTypeMaster.Where(x => x.DocumentTypeID == cldm.DocumentTypeID).Select(l => l.Condition).FirstOrDefault();
                                    cldm.ModifiedOn = DateTime.Now;
                                    db.Entry(cldm).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }

                        if (_loanStack != null)
                        {

                            StackingOrderMaster _sysLoanStack = _ILDataAccess.GetSystemLoanStackingOrder(LoanTypeID);
                            StackingOrderMaster _tenantLoanStack = _ILDataAccess.GetStackingOrder(TableSchema, _loanStack.StackingOrderID);

                            if (_sysLoanStack != null && _tenantLoanStack != null)
                            {
                                db.StackingOrderDetailMaster.RemoveRange(db.StackingOrderDetailMaster.Where(s => s.StackingOrderID == _loanStack.StackingOrderID));
                                db.SaveChanges();

                                List<CustLoanDocMapping> lsLoanDocMapping = db.CustLoanDocMapping.AsNoTracking().Where(c => c.CustomerID == CustomerID && c.LoanTypeID == LoanTypeID).ToList();
                                string dupName = "";
                                StackingOrderGroupmasters tenantStackMasters = null;
                                foreach (StackingOrderDetailMaster sysStackingOrderDetail in _sysLoanStack.StackingOrderDetailMasters)
                                {
                                    Int64 DocumentTypeID = sysStackingOrderDetail.DocumentTypeID;

                                    DocumentTypeMaster sysDocType = _ILDataAccess.GetSystemDocumentType(DocumentTypeID);

                                    if (sysDocType != null)
                                    {
                                        foreach (var item in lsLoanDocMapping)
                                        {
                                            DocumentTypeMaster docType = db.DocumentTypeMaster.AsNoTracking().Where(d => d.DocumentTypeID == item.DocumentTypeID).FirstOrDefault();

                                            if (docType != null)
                                            {
                                                if (sysDocType.Name.Equals(docType.Name))
                                                {
                                                    DocumentTypeID = docType.DocumentTypeID;
                                                    break;
                                                }
                                            }
                                        }
                                        if (sysStackingOrderDetail.StackingOrderGroupID > 0)
                                        {
                                            //StackGroupId = sysStackingOrderDetail.StackingOrderGroupID;

                                            var stackingOrderGroup = new IntellaLendDataAccess().AddStackingOrderMasterGroup(sysStackingOrderDetail.StackingOrderGroupID);
                                            if (stackingOrderGroup.StackingOrderGroupName != dupName)
                                            {
                                                dupName = stackingOrderGroup.StackingOrderGroupName;
                                                //StackingOrderGroupmasters tenantStackMasters = new StackingOrderGroupmasters();

                                                tenantStackMasters = db.StackingOrderGroupmasters.Add(new StackingOrderGroupmasters()
                                                {
                                                    StackingOrderID = _loanStack.StackingOrderID,
                                                    StackingOrderGroupName = stackingOrderGroup.StackingOrderGroupName,
                                                    Active = true,
                                                    GroupSortField = stackingOrderGroup.GroupSortField,
                                                    CreatedOn = DateTime.Now,
                                                    ModifiedOn = DateTime.Now
                                                });
                                                //db.StackingOrderGroupmasters.Add(stackingOrderGroup);
                                                db.SaveChanges();
                                            }
                                        }

                                        //Insert  StackingOrder Detail Table
                                        StackingOrderDetailMaster stackingOrderDetail = new StackingOrderDetailMaster();
                                        stackingOrderDetail.StackingOrderDetailID = 0;
                                        stackingOrderDetail.StackingOrderID = _loanStack.StackingOrderID;
                                        stackingOrderDetail.DocumentTypeID = DocumentTypeID;
                                        stackingOrderDetail.SequenceID = sysStackingOrderDetail.SequenceID;
                                        stackingOrderDetail.StackingOrderGroupID = (sysStackingOrderDetail.StackingOrderGroupID > 0) ? tenantStackMasters.StackingOrderGroupID : 0;
                                        stackingOrderDetail.Active = true;
                                        stackingOrderDetail.CreatedOn = DateTime.Now;
                                        stackingOrderDetail.ModifiedOn = DateTime.Now;
                                        db.StackingOrderDetailMaster.Add(stackingOrderDetail);
                                        db.SaveChanges();
                                    }
                                }
                            }

                            _loanStack.Active = true;
                            _loanStack.ModifiedOn = DateTime.Now;
                            db.Entry(_loanStack).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        CustLoantypeMapping custLoantypeMapping = db.CustLoantypeMapping.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.LoanTypeID == LoanTypeID).FirstOrDefault();
                        if (custLoantypeMapping == null)
                        {
                            db.CustLoantypeMapping.Add(new CustLoantypeMapping()
                            {
                                CustomerID = CustomerID,
                                LoanTypeID = LoanTypeID,
                                DocumentTypeSync = true,
                                LoanTypeSync = true,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now
                            });
                            db.SaveChanges();
                        }

                        result = true;
                        tran.Commit();
                    }
                }
            }

            return result;
        }

        public List<ReportMaster> GetReportMasters(Int64 ReviewTypeID)
        {
            List<ReportMaster> reportMaster = new List<ReportMaster>();
            using (var db = new DBConnect(TableSchema))
            {
                //reportConfig = (from rm in db.ReportMaster.AsNoTracking()
                //                join rc in db.ReportConfig.AsNoTracking() on rm.ReportMasterID equals rc.ReportMasterID
                //                where rm.ReportMasterID == ReportMasterID
                //                select rc).ToList();

                reportMaster = db.ReportMaster.AsNoTracking().Where(x => x.ReviewTypeID == ReviewTypeID).ToList();


            }
            return reportMaster;
        }

        #region Sync Configuration Mapping

        public List<CustLoantypeMappingDetails> GetCustLoantypeMapping(Int64 CustomerID)
        {
            List<CustLoantypeMapping> custLoanTypeMappings = new List<CustLoantypeMapping>();
            List<CustLoantypeMappingDetails> _custLoantypeMappings = new List<CustLoantypeMappingDetails>();

            using (var db = new DBConnect(TableSchema))
            {
                List<LoanTypeMaster> loanTypeMasters = GetLoanTypeForCustomer(CustomerID);
                foreach (LoanTypeMaster loanTypeMaster in loanTypeMasters)
                {
                    custLoanTypeMappings = db.CustLoantypeMapping.AsNoTracking().Where(x => x.CustomerID == CustomerID && x.LoanTypeID == loanTypeMaster.LoanTypeID).ToList();

                    CustLoantypeMappingDetails custLoantype = new CustLoantypeMappingDetails();
                    custLoantype.CustomerID = CustomerID;
                    custLoantype.LoanTypeID = loanTypeMaster.LoanTypeID;
                    custLoantype.LoanTypeSync = false;
                    custLoantype.DocumentTypeSync = false;
                    custLoantype.LoanTypeName = loanTypeMaster.LoanTypeName;

                    if (custLoanTypeMappings.Count > 0)
                    {
                        CustLoantypeMapping _custmapping = custLoanTypeMappings.Where(x => x.LoanTypeID == loanTypeMaster.LoanTypeID).FirstOrDefault();
                        if (_custmapping != null)
                        {
                            custLoantype.ID = _custmapping.ID;
                            custLoantype.LoanTypeSync = _custmapping.LoanTypeSync;
                            custLoantype.DocumentTypeSync = _custmapping.DocumentTypeSync;
                        }
                    }
                    _custLoantypeMappings.Add(custLoantype);
                }

            }
            return _custLoantypeMappings;
        }

        public bool SaveCustLoantypeMapping(List<CustLoantypeMapping> loantypeMappings)
        {
            using (var db = new DBConnect(TableSchema))
            {
                foreach (CustLoantypeMapping loantypeMapping in loantypeMappings)
                {
                    CustLoantypeMapping _loantypemapping = db.CustLoantypeMapping.AsNoTracking().Where(x => x.ID == loantypeMapping.ID).FirstOrDefault();
                    if (_loantypemapping != null)
                    {
                        _loantypemapping.DocumentTypeSync = loantypeMapping.DocumentTypeSync;
                        _loantypemapping.LoanTypeSync = loantypeMapping.LoanTypeSync;
                        _loantypemapping.ModifiedOn = DateTime.Now;
                        db.Entry(_loantypemapping).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return true;
            }
        }

        #endregion

        #region Review Loan Lender Mapping

        public object GetReviewLoanLenderMapped(Int64 ReviewTypeID, Int64 LoanTypeID)
        {
            using (var db = new DBConnect(TableSchema))
            {
                List<CustomerMaster> lsLenderMaster = new List<CustomerMaster>();

                lsLenderMaster = db.CustomerMaster.ToList();

                List<CustReviewLoanMapping> mappedLenders = db.CustReviewLoanMapping.AsNoTracking().Where(c => c.ReviewTypeID == ReviewTypeID && c.LoanTypeID == LoanTypeID && c.Active == true).ToList();

                var lsMappedLenders = (from map in mappedLenders
                                       join lt in lsLenderMaster on map.CustomerID equals lt.CustomerID into rmJoin
                                       from rmGroup in rmJoin.DefaultIfEmpty()
                                       select new
                                       {
                                           CustomerID = rmGroup.CustomerID,
                                           CustomerName = rmGroup.CustomerName
                                       }).ToList();

                var lsUnMappedLenders = lsLenderMaster
                        .Where(x => (!(lsMappedLenders.Any(y => x.CustomerID == y.CustomerID))) && x.Active == true)
                        .Select(a => new
                        {
                            CustomerID = a.CustomerID,
                            CustomerName = a.CustomerName,

                        }).ToList();

                return new { AssignedLenders = lsMappedLenders, AllLenders = lsUnMappedLenders };
            }
        }

        public object SaveReviewLoanLenderMapping(Int64 ReviewTypeID, Int64 LoanTypeID, Int64[] allLenderIDs, Int64[] assignedLenderIDs, bool IsAdd)
        {
            long _importStagingID = 0;
            long _lenderCount = 0;
            string ReviewTypeName = string.Empty;
            string LoanTypeName = string.Empty;
            using (var db = new DBConnect("IL"))
            {
                ReviewTypeMaster reviewTypeMaster = db.ReviewTypeMaster.AsNoTracking().Where(r => r.ReviewTypeID == ReviewTypeID).FirstOrDefault();
                if (reviewTypeMaster != null)
                    ReviewTypeName = reviewTypeMaster.ReviewTypeName;
                LoanTypeMaster loanTypeMaster = db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeID == LoanTypeID).FirstOrDefault();
                if (loanTypeMaster != null)
                    LoanTypeName = loanTypeMaster.LoanTypeName;

            }

            using (var db = new DBConnect(TableSchema))
            {
                foreach (var _lenderID in allLenderIDs)
                {
                    CustReviewLoanMapping _custReviewLoanMap = db.CustReviewLoanMapping.AsNoTracking().Where(c => c.ReviewTypeID == ReviewTypeID && c.LoanTypeID == LoanTypeID && c.CustomerID == _lenderID).FirstOrDefault();

                    if (_custReviewLoanMap != null)
                    {
                        _custReviewLoanMap.Active = false;
                        _custReviewLoanMap.ModifiedOn = DateTime.Now;
                        db.Entry(_custReviewLoanMap).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    CustReviewMapping _custReviewMap = db.CustReviewMapping.AsNoTracking().Where(c => c.ReviewTypeID == ReviewTypeID && c.CustomerID == _lenderID).FirstOrDefault();

                    if (_custReviewMap != null)
                    {
                        _custReviewMap.Active = false;
                        _custReviewMap.ModifiedOn = DateTime.Now;
                        db.Entry(_custReviewMap).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }

                foreach (var _lenderID in assignedLenderIDs)
                {
                    CustomerMaster _lenderMaster = db.CustomerMaster.AsNoTracking().Where(c => c.CustomerID == _lenderID).FirstOrDefault();

                    CustReviewLoanMapping _custReviewLoanMap = db.CustReviewLoanMapping.AsNoTracking().Where(c => c.ReviewTypeID == ReviewTypeID && c.LoanTypeID == LoanTypeID && c.CustomerID == _lenderID).FirstOrDefault();

                    if(_custReviewLoanMap == null)
                    {
                        _lenderCount++;
                    }

                    if (_custReviewLoanMap != null && !_custReviewLoanMap.Active)
                    {
                        _custReviewLoanMap.Active = true;
                        _custReviewLoanMap.ModifiedOn = DateTime.Now;
                        db.Entry(_custReviewLoanMap).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    CustReviewMapping _custReviewMap = db.CustReviewMapping.AsNoTracking().Where(c => c.ReviewTypeID == ReviewTypeID && c.CustomerID == _lenderID).FirstOrDefault();

                    if (_custReviewMap != null && !_custReviewMap.Active)
                    {
                        _custReviewMap.Active = true;
                        _custReviewMap.ModifiedOn = DateTime.Now;
                        db.Entry(_custReviewMap).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    CustReviewLoanCheckMapping _loanCheck = db.CustReviewLoanCheckMapping.AsNoTracking().Where(r => r.CustomerID == _lenderID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == LoanTypeID).FirstOrDefault();
                    if (_loanCheck != null)
                    {
                        _loanCheck.Active = true;
                        _loanCheck.ModifiedOn = DateTime.Now;
                        db.Entry(_loanCheck).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    CustReviewLoanStackMapping _loanStack = db.CustReviewLoanStackMapping.AsNoTracking().Where(r => r.CustomerID == _lenderID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == LoanTypeID).FirstOrDefault();
                    if (_loanStack != null)
                    {
                        _loanStack.Active = true;
                        _loanStack.ModifiedOn = DateTime.Now;
                        db.Entry(_loanStack).State = EntityState.Modified;
                        db.SaveChanges();
                    }


                }
                if (_lenderCount > 0 && IsAdd)
                {
                    CustomerImportStaging _import = new CustomerImportStaging()
                    {
                        FilePath = string.Empty,
                        Status = LenderImportStatusConstant.LENDER_IMPORT_STAGED,
                        ImportCount = _lenderCount,
                        ErrorMsg = string.Empty,
                        AssignType = LenderAssignTypeStatusConstant.SERVICE_LENDER_IMPORT,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };

                    db.CustomerImportStaging.Add(_import);
                    db.SaveChanges();

                    _importStagingID = _import.ID;

                    foreach (var _lenderID in assignedLenderIDs)
                    {
                        CustomerMaster _lenderMaster = db.CustomerMaster.AsNoTracking().Where(c => c.CustomerID == _lenderID).FirstOrDefault();

                        CustReviewLoanMapping _custReviewLoanMap = db.CustReviewLoanMapping.AsNoTracking().Where(c => c.ReviewTypeID == ReviewTypeID && c.LoanTypeID == LoanTypeID && c.CustomerID == _lenderID).FirstOrDefault();

                        if (_custReviewLoanMap == null)
                        {
                            db.CustomerImportStagingDetail.Add(new CustomerImportStagingDetail()
                            {
                                CustomerImportStagingID = _importStagingID,
                                CustomerName = _lenderMaster.CustomerName,
                                CustomerCode = _lenderMaster.CustomerCode,
                                State = _lenderMaster.State,
                                Country = _lenderMaster.Country,
                                Zip = _lenderMaster.ZipCode,
                                ServiceType = ReviewTypeName,
                                LoanType = LoanTypeName,
                                Status = LenderImportStatusConstant.LENDER_IMPORT_STAGED,
                                ErrorMsg = string.Empty,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now
                            });
                            db.SaveChanges();
                        }
                    }
                }
                return true;
            }
        }

        #endregion
    }

}
