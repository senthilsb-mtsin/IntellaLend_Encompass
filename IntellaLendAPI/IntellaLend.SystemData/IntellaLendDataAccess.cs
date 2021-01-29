using IntellaLend.Constance;
using IntellaLend.MinIOWrapper;
using IntellaLend.Model;
using IntellaLend.Model.Encompass;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IntellaLend.EntityDataHandler
{
    public class IntellaLendDataAccess
    {
        #region Private Variable

        private static string TenantSchema;

        private static string SystemSchema = "IL";

        #endregion

        #region Constructor

        public IntellaLendDataAccess() { }

        public IntellaLendDataAccess(string tenantSchema)
        {

            TenantSchema = tenantSchema;

        }

        #endregion

        #region Public Methods

        #region System Review Type Master

        public ReviewTypeMaster GetSystemReviewType(Int64 reviewTypeID)
        {
            ReviewTypeMaster lm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                lm = db.ReviewTypeMaster.AsNoTracking().Where(l => l.ReviewTypeID == reviewTypeID && l.Active == true && l.Type == 0).FirstOrDefault();
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

        public bool AddReviewType(SystemReviewTypeMaster ReviewType)
        {
            bool isReviewTypeAdd = false;
            using (var db = new DBConnect(TenantSchema))
            {
                if (!db.SystemReviewTypeMaster.AsNoTracking().Any(x => x.ReviewTypeName.Equals(ReviewType.ReviewTypeName) && x.Type == 0))
                {
                    ReviewType.ReviewTypeID = AddReviewTypeToSystem(db, ReviewType);
                    if (ReviewType.ReviewTypeID > 0)
                    {
                        isReviewTypeAdd = true;
                    }
                }
            }
            return isReviewTypeAdd;
        }

        public object AddObjectReviewType(SystemReviewTypeMaster ReviewType)
        {
            bool isReviewTypeAdd = false;
            using (var db = new DBConnect(TenantSchema))
            {
                if (!db.SystemReviewTypeMaster.AsNoTracking().Any(x => x.ReviewTypeName.Equals(ReviewType.ReviewTypeName) && x.Type == 0))
                {
                    ReviewType.ReviewTypeID = AddReviewTypeToSystem(db, ReviewType);
                    if (ReviewType.ReviewTypeID > 0)
                    {
                        isReviewTypeAdd = true;
                    }
                }
            }
            return new { Success = isReviewTypeAdd, ReviewTypeID = ReviewType.ReviewTypeID };
        }

        public bool QCIQAddReviewType(List<SystemReviewTypeMaster> reviewtype)
        {
            bool isaddreviewtype = false;
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (SystemReviewTypeMaster reviewtypeobject in reviewtype)
                        {
                            if (!this.AddReviewType(reviewtypeobject))
                            {
                                isaddreviewtype = false;
                            }
                            else
                            {
                                isaddreviewtype = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
            }
            return isaddreviewtype;
        }

        public bool SaveAllSMTPDetails(SMTPDETAILS sMTPMaster)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    SMTPDETAILS smtpData = db.SMTPDETAILS.AsNoTracking().Where(sm => sm.SMTPID == sMTPMaster.SMTPID).FirstOrDefault();
                    smtpData.SMTPNAME = sMTPMaster.SMTPNAME;
                    smtpData.SMTPCLIENTHOST = sMTPMaster.SMTPCLIENTHOST;
                    smtpData.USERNAME = sMTPMaster.USERNAME;
                    smtpData.SMTPCLIENTPORT = sMTPMaster.SMTPCLIENTPORT;
                    smtpData.SMTPDELIVERYMETHOD = sMTPMaster.SMTPDELIVERYMETHOD;
                    smtpData.TIMEOUT = sMTPMaster.TIMEOUT;
                    smtpData.DOMAIN = sMTPMaster.DOMAIN;
                    smtpData.ENABLESSL = sMTPMaster.ENABLESSL;
                    smtpData.USEDEFAULTCREDENTIALS = sMTPMaster.USEDEFAULTCREDENTIALS;
                    //FOR UHS : smtpData.PASSWORD = DBHashing.Encrypt(db, sMTPMaster.PasswordString);
                    db.Entry(smtpData).State = EntityState.Modified;
                    db.SaveChanges();
                    trans.Commit();
                    return true;
                }

            }
            return false;
        }

        public List<SMTPDETAILS> GetAllSMPTDetails()
        {
            List<SMTPDETAILS> _smtp = null;

            using (var db = new DBConnect(SystemSchema))
            {
                _smtp = db.SMTPDETAILS.AsNoTracking().ToList();

                //_smtp.ForEach(elt =>
                //{
                //    elt.PasswordString = DBHashing.Decrypt(db, elt.PASSWORD);
                //});
            }
            return _smtp;

        }

        public List<ReviewTypeMaster> GetSystemAllReviewTypes()
        {
            List<ReviewTypeMaster> lsReviewTypes = null;

            using (var db = new DBConnect(SystemSchema))
            {
                lsReviewTypes = db.ReviewTypeMaster.AsNoTracking().Where(l => l.Type == 0).ToList();
            }

            return lsReviewTypes;
        }


        #endregion

        #region System Loan Type Master
        public object GetAssignedSystemAllLoanTypes(Int64 DocumentTypeID)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                List<LoanTypeMaster> lsLoanTypeMaster = db.LoanTypeMaster.AsNoTracking().Where(l => l.Type == 0).ToList();

                List<CustLoanDocMapping> mappedLoanTypes = db.CustLoanDocMapping.AsNoTracking().Where(c => c.CustomerID == 1 && c.DocumentTypeID == DocumentTypeID).ToList();

                var lsMappedLoanTypes = (from map in mappedLoanTypes
                                         join lt in lsLoanTypeMaster on map.LoanTypeID equals lt.LoanTypeID
                                         where map.DocumentTypeID == DocumentTypeID
                                         select new
                                         {
                                             LoanTypeID = map.LoanTypeID,
                                             LoanTypeName = lt.LoanTypeName
                                         }).Distinct().ToList();

                var lsUnMappedLoanTypes = lsLoanTypeMaster
                                        .Where(x => !(lsMappedLoanTypes.Any(y => x.LoanTypeID == y.LoanTypeID)))
                                        .Select(a => new
                                        {
                                            LoanTypeID = a.LoanTypeID,
                                            LoanTypeName = a.LoanTypeName
                                        }).Distinct().ToList();

                return new { AllLoanTypes = lsUnMappedLoanTypes, AssignedLoanTypes = lsMappedLoanTypes };
            }
        }



        public object GetSystemAllLoanTypes(Int64 ReviewTypeID)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                List<LoanTypeMaster> lsLoanTypeMaster = db.LoanTypeMaster.AsNoTracking().Where(l => l.Type == 0).ToList();

                List<CustReviewLoanMapping> mappedLoanTypes = db.CustReviewLoanMapping.AsNoTracking().Where(c => c.ReviewTypeID == ReviewTypeID).ToList();

                var lsMappedLoanTypes = (from map in mappedLoanTypes
                                         join lt in lsLoanTypeMaster on map.LoanTypeID equals lt.LoanTypeID
                                         where map.ReviewTypeID == ReviewTypeID
                                         select new
                                         {
                                             LoanTypeID = map.LoanTypeID,
                                             LoanTypeName = lt.LoanTypeName
                                         }).ToList();

                var lsUnMappedLoanTypes = lsLoanTypeMaster
                                        .Where(x => !(lsMappedLoanTypes.Any(y => x.LoanTypeID == y.LoanTypeID)))
                                        .Select(a => new
                                        {
                                            LoanTypeID = a.LoanTypeID,
                                            LoanTypeName = a.LoanTypeName
                                        }).ToList();

                return new { AllLoanTypes = lsUnMappedLoanTypes, AssignedLoanTypes = lsMappedLoanTypes };
            }
        }

        public object GetSystemAllLoanTypes(Int64 ReviewTypeID, bool IsAdd)
        {
            List<LoanTypeMaster> lsLoanTypeMaster = new List<LoanTypeMaster>();

            if (IsAdd)
            {
                lsLoanTypeMaster = GetSystemAllLoanTypes();
            }
            else
            {
                using (var db = new DBConnect(SystemSchema))
                {
                    List<CustReviewLoanMapping> mappedLoanTypes = db.CustReviewLoanMapping.AsNoTracking().Where(c => c.ReviewTypeID == ReviewTypeID).ToList();

                    foreach (CustReviewLoanMapping item in mappedLoanTypes)
                    {
                        lsLoanTypeMaster.Add(db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeID == item.LoanTypeID).FirstOrDefault());
                    }
                }
            }

            return lsLoanTypeMaster;
        }

        public List<LoanTypeMaster> GetSystemAllLoanTypes()
        {
            List<LoanTypeMaster> lsLoanTypes = null;

            using (var db = new DBConnect(SystemSchema))
            {
                lsLoanTypes = new List<LoanTypeMaster>();
                lsLoanTypes = db.LoanTypeMaster.AsNoTracking().Where(l => l.Type == 0).ToList();
            }

            return lsLoanTypes;
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

        public SystemReverificationMasters GetSystemReverification(Int64 ReverifyID)
        {
            SystemReverificationMasters lm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                lm = db.SystemReverificationMasters.AsNoTracking().Where(l => l.ReverificationID == ReverifyID).FirstOrDefault();
            }

            return lm;
        }

        public List<CustReverificationDocMapping> GetSystemCustReverifyDocMapping(Int64 ReverifyID)
        {
            List<CustReverificationDocMapping> lm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                lm = db.CustReverificationDocMapping.AsNoTracking().Where(l => l.CustomerID == 1 && l.ReverificationID == ReverifyID).ToList();
            }

            return lm;
        }

        public List<LoanTypeMaster> GetSystemLoanTypesByMapping(Int64 reviewTypeID)
        {
            List<LoanTypeMaster> LoanTypeMaster = new List<Model.LoanTypeMaster>();

            using (var db = new DBConnect(SystemSchema))
            {
                var custReviewLoanTypes = db.CustReviewLoanMapping.AsNoTracking().Where(c => c.ReviewTypeID == reviewTypeID && c.Active == true).ToList();

                foreach (var item in custReviewLoanTypes)
                {
                    //LoanTypeMaster _loanType = db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeID == item.LoanTypeID && l.Active == true && l.Type == 0).FirstOrDefault();
                    LoanTypeMaster _loanType = db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeID == item.LoanTypeID && l.Type == 0).FirstOrDefault();
                    if (_loanType != null)
                        LoanTypeMaster.Add(_loanType);
                }
            }
            return LoanTypeMaster;
        }

        public List<ReportConfig> GetReportMasterDocumentNames(Int64 ReportMasterID)
        {
            List<ReportConfig> reportConfig = new List<ReportConfig>(); ;
            using (var db = new DBConnect(SystemSchema))
            {
                //reportConfig = (from rm in db.ReportMaster.AsNoTracking()
                //                join rc in db.ReportConfig.AsNoTracking() on rm.ReportMasterID equals rc.ReportMasterID
                //                where rm.ReportMasterID == ReportMasterID
                //                select rc).ToList();

                reportConfig = db.ReportConfig.AsNoTracking().Where(x => x.ReportMasterID == ReportMasterID).ToList();


            }
            return reportConfig;
        }

        public List<ReportMaster> GetReportMasters()
        {
            List<ReportMaster> reportMaster = new List<ReportMaster>(); ;
            using (var db = new DBConnect(SystemSchema))
            {
                //reportConfig = (from rm in db.ReportMaster.AsNoTracking()
                //                join rc in db.ReportConfig.AsNoTracking() on rm.ReportMasterID equals rc.ReportMasterID
                //                where rm.ReportMasterID == ReportMasterID
                //                select rc).ToList();

                reportMaster = db.ReportMaster.AsNoTracking().ToList();


            }
            return reportMaster;
        }

        public List<LOSDocumentFields> GetLosDocumentFields(long lOSDocumentId, string fieldSearchWord)
        {
            List<LOSDocumentFields> losDocFields = new List<LOSDocumentFields>();
            using (var db = new DBConnect(SystemSchema))
            {
                losDocFields = db.LOSDocumentFields.AsNoTracking().Where(a => a.LOSDocumentID == lOSDocumentId && a.FieldName.ToLower().StartsWith(fieldSearchWord.ToLower())).ToList();
            }
            return losDocFields;
        }

        public object AddSingleLoanType(SystemLoanTypeMaster loanType)
        {
            bool isLoanTypeAdd = false;

            using (var db = new DBConnect(TenantSchema))
            {
                if (!db.SystemLoanTypeMaster.AsNoTracking().Any(x => x.LoanTypeName.Equals(loanType.LoanTypeName)))
                {
                    loanType.LoanTypeID = AddLoanTypeToSystem(db, loanType);
                    if (loanType.LoanTypeID > 0)
                        isLoanTypeAdd = true;
                }
            }
            return new { LoanTypeID = loanType.LoanTypeID, Success = isLoanTypeAdd };
        }

        public bool AddLoanType(SystemLoanTypeMaster loanType)
        {
            bool isLoanTypeAdd = false;

            using (var db = new DBConnect(TenantSchema))
            {
                if (!db.SystemLoanTypeMaster.AsNoTracking().Any(x => x.LoanTypeName.Equals(loanType.LoanTypeName)))
                {
                    loanType.LoanTypeID = AddLoanTypeToSystem(db, loanType);
                    if (loanType.LoanTypeID > 0)
                    {
                        isLoanTypeAdd = true;
                    }
                }
            }
            return isLoanTypeAdd;
        }

        public bool QCIQAddLoanType(List<SystemLoanTypeMaster> loantype)
        {
            bool isaddloantype = false;
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (SystemLoanTypeMaster loanobject in loantype)
                        {
                            if (!AddLoanType(loanobject))
                            {
                                isaddloantype = false;
                            }
                            else
                            {
                                isaddloantype = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
            }

            return isaddloantype;
        }

        /*
        public bool UpdateLoanType(LoanTypeMaster loanType)
        {
            using (var db = new DBConnect(SystemSchema))
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
                */

        #endregion

        #region Email Master

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

        #endregion

        #region Document Type Master


        public List<DocumentTypeMasterList> GetSystemDocumentTypes()
        {
            List<DocumentTypeMasterList> data = new List<DocumentTypeMasterList>();
            using (var db = new DBConnect(SystemSchema))
            {
                data = (from doc in db.DocumentTypeMaster.AsNoTracking()
                        join Pspot in db.EncompassParkingSpot.AsNoTracking() on doc.DocumentTypeID equals Pspot.DocumentTypeID into Pgrp
                        from grp in Pgrp.DefaultIfEmpty()
                        select new DocumentTypeMasterList
                        {
                            Name = doc.Name,
                            DisplayName = doc.DisplayName,
                            ParkingSpotID = (grp != null) ? grp.ID : 0,
                            ParkingSpotName = grp.ParkingSpotName,
                            DocumentTypeID = doc.DocumentTypeID,
                            Active = doc.Active,
                            DocumentLevel = doc.DocumentLevel
                        }).ToList();
                // return db.DocumentTypeMaster.AsNoTracking().ToList();
            }
            return data;
        }

        public List<object> GetStackSystemDocumentTypes()
        {
            List<object> result = new List<object>();
            using (var db = new DBConnect(SystemSchema))
            {
                result = (from sod in db.DocumentTypeMaster.AsNoTracking()
                          where sod.Active == true
                          select new
                          {
                              DocumentTypeID = sod.DocumentTypeID,
                              DisplayName = sod.DisplayName,
                              DocFieldList = (from f in db.DocumentFieldMaster
                                              where f.DocumentTypeID == sod.DocumentTypeID
                                              select f).ToList(),
                              OrderByFieldID = (from f in db.DocumentFieldMaster
                                                where f.DocumentTypeID == sod.DocumentTypeID && f.DocOrderByField != null
                                                select f.FieldID).FirstOrDefault()
                          }).ToList<object>();
            }

            return result;
        }

        public object GetLoanStackDocs(Int64 LoanTypeID)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                //result = (from cusDoc in db.CustLoanDocMapping.AsNoTracking()
                //          join docS in db.DocumentTypeMaster.AsNoTracking() on cusDoc.DocumentTypeID equals docS.DocumentTypeID 
                //          where cusDoc.Active == true
                //          select new
                //          {
                //              DocumentTypeID = cusDoc.DocumentTypeID,
                //              DisplayName = docS.DisplayName,
                //              DocFieldList = (from f in db.DocumentFieldMaster
                //                              where f.DocumentTypeID == cusDoc.DocumentTypeID
                //                              select f).ToList(),
                //              OrderByFieldID = (from f in db.DocumentFieldMaster
                //                                where f.DocumentTypeID == cusDoc.DocumentTypeID && f.DocOrderByField == null
                //                                select f.FieldID).FirstOrDefault()
                //          }).ToList<object>();

                List<DocumentTypeMaster> lsDocumentTypeMaster = db.DocumentTypeMaster.AsNoTracking().Where(d => d.Active == true).ToList();

                List<CustLoanDocMapping> mappedDocTypes = db.CustLoanDocMapping.AsNoTracking().Where(c => c.LoanTypeID == LoanTypeID).ToList();

                var lsMappedDocTypes = (from map in mappedDocTypes
                                        join lt in lsDocumentTypeMaster on map.DocumentTypeID equals lt.DocumentTypeID
                                        where map.LoanTypeID == LoanTypeID
                                        select new
                                        {
                                            DocumentTypeID = map.DocumentTypeID,
                                            Name = lt.Name,
                                            DisplayName = lt.DisplayName,
                                            DocFieldList = (from f in db.DocumentFieldMaster
                                                            where f.DocumentTypeID == map.DocumentTypeID
                                                            select f).ToList(),
                                            OrderByFieldID = (from f in db.DocumentFieldMaster
                                                              where f.DocumentTypeID == map.DocumentTypeID && f.DocOrderByField == null
                                                              select f.FieldID).FirstOrDefault()
                                        }).ToList();

                var lsUnMappedDocTypes = lsDocumentTypeMaster
                                        .Where(x => !(lsMappedDocTypes.Any(y => x.DocumentTypeID == y.DocumentTypeID)))
                                        .Select(a => new
                                        {
                                            DocumentTypeID = a.DocumentTypeID,
                                            Name = a.Name,
                                            DisplayName = a.DisplayName,

                                        }).ToList();

                return new { AllDocTypes = lsUnMappedDocTypes, AssignedDocTypes = lsMappedDocTypes };
            }

        }
        public List<DocumentFieldMaster> GetSystemDocumentFields()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                // return db.DocumentFieldMaster.AsNoTracking().Where(x => x.Active == true).Distinct().ToList();
                return db.DocumentFieldMaster.AsNoTracking().GroupBy(x => x.DisplayName).Select(x => x.FirstOrDefault()).ToList();
            }
        }

        public string GetLosValue(string searchValue)
        {
            string FieldDesc = string.Empty;
            List<EncompassDocFields> LosFields = new List<EncompassDocFields>();
            using (var db = new DBConnect(SystemSchema))
            {

                if (searchValue != null && searchValue != "")
                {
                    FieldDesc = db.EncompassFields.AsNoTracking().Where(a => a.FieldDesc.ToLower() == searchValue).Select(a => "#" + a.FieldID + "# - " + a.FieldDesc).FirstOrDefault();

                }


            }
            return FieldDesc;
        }
        public object GetSystemDocuments(Int64 LoanTypeID)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                List<DocumentTypeMaster> lsDocumentTypeMaster = db.DocumentTypeMaster.AsNoTracking().Where(d => d.Active == true).ToList();

                List<CustLoanDocMapping> mappedDocTypes = db.CustLoanDocMapping.AsNoTracking().Where(c => c.LoanTypeID == LoanTypeID).ToList();

                var lsMappedDocTypes = (from map in mappedDocTypes
                                        join lt in lsDocumentTypeMaster on map.DocumentTypeID equals lt.DocumentTypeID
                                        where map.LoanTypeID == LoanTypeID
                                        select new
                                        {
                                            DocumentTypeID = map.DocumentTypeID,
                                            Name = lt.Name,
                                            DocumentLevel = map.DocumentLevel,
                                            DocumentFieldMasters = db.DocumentFieldMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == lt.DocumentTypeID && ld.Active == true).ToList(),
                                            DocumetTypeTables = db.DocumetTypeTables.AsNoTracking().Where(ld => ld.DocumentTypeID == lt.DocumentTypeID).ToList(),
                                            RuleDocumentTables = db.RuleDocumentTables.AsNoTracking().Where(rd => rd.DocumentID == lt.DocumentTypeID).ToList(),
                                            Condition = map.Condition,
                                        }).ToList();

                var lsUnMappedDocTypes = lsDocumentTypeMaster
                                        .Where(x => !(lsMappedDocTypes.Any(y => x.DocumentTypeID == y.DocumentTypeID)))
                                        .Select(a => new
                                        {
                                            DocumentTypeID = a.DocumentTypeID,
                                            Name = a.Name,
                                            DocumentLevel = DocumentLevelConstant.CRITICAL,
                                            DocumentFieldMasters = db.DocumentFieldMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == a.DocumentTypeID && ld.Active == true).ToList(),
                                            DocumetTypeTables = db.DocumetTypeTables.AsNoTracking().Where(ld => ld.DocumentTypeID == a.DocumentTypeID).ToList(),
                                            RuleDocumentTables = db.RuleDocumentTables.AsNoTracking().Where(rd => rd.DocumentID == a.DocumentTypeID).ToList(),

                                            Condition = "",
                                        }).ToList();

                return new { AllDocTypes = lsUnMappedDocTypes, AssignedDocTypes = lsMappedDocTypes };
            }
        }
        //List<CustLoanDocMapping> cldm = db.CustLoanDocMapping.AsNoTracking().Where(cld => cld.LoanTypeID == loanTypeID && cld.Active == true).ToList();

        //dm = new List<DocumentTypeMaster>();

        //        if (cldm != null)
        //        {

        //            dm = (from cl in cldm
        //                  join dms in db.DocumentTypeMaster.AsNoTracking() on cl.DocumentTypeID equals dms.DocumentTypeID
        //                  where dms.Active == true
        //                  select new DocumentTypeMaster()
        //{
        //    DocumentTypeID = dms.DocumentTypeID,
        //                      Name = dms.Name,
        //                      DisplayName = dms.DisplayName,
        //                      Active = dms.Active,
        //                      DocumentLevel = dms.DocumentLevel,
        //                      CreatedOn = dms.CreatedOn,
        //                      ModifiedOn = dms.ModifiedOn,
        //                      DocumentFieldMasters = db.DocumentFieldMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == dms.DocumentTypeID && ld.Active == true).ToList(),
        //                      DocumetTypeTables = db.DocumetTypeTables.AsNoTracking().Where(ld => ld.DocumentTypeID == dms.DocumentTypeID).ToList(),
        //                      DocumentLevelDesc = dms.DocumentLevelDesc,
        //                      RuleDocumentTables = db.RuleDocumentTables.AsNoTracking().Where(rd => rd.DocumentID == dms.DocumentTypeID).ToList()
        //                  }).OrderBy(x => x.Name).ToList();

        public bool SaveConditionGeneralRule(Int64 DocumentID, string RuleValues, Int64 LoanTypeID)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    CustLoanDocMapping mappedDocTypes = db.CustLoanDocMapping.AsNoTracking().Where(c => c.LoanTypeID == LoanTypeID && c.DocumentTypeID == DocumentID).FirstOrDefault();

                    if (mappedDocTypes != null)
                    {
                        mappedDocTypes.Condition = RuleValues;
                        mappedDocTypes.ModifiedOn = DateTime.Now;
                        db.Entry(mappedDocTypes).State = EntityState.Modified;
                        db.SaveChanges();
                        return true;

                    }

                    else
                    {
                        CustLoanDocMapping _doc = new CustLoanDocMapping()
                        {

                            CustomerID = 1,
                            LoanTypeID = LoanTypeID,
                            DocumentTypeID = DocumentID,
                            DocumentLevel = DocumentLevelConstant.CRITICAL,
                            Active = true,
                            Condition = RuleValues,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        };

                        db.CustLoanDocMapping.Add(_doc);
                        db.SaveChanges();
                        return true;
                    }
                    trans.Commit();

                }
                return false;

            }

        }
        public List<DocumentTypeMaster> GetSystemDocumentTypesWithFields(Int64 loanTypeID)
        {
            List<DocumentTypeMaster> dm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                List<CustLoanDocMapping> cldm = db.CustLoanDocMapping.AsNoTracking().Where(cld => cld.LoanTypeID == loanTypeID && cld.Active == true).ToList();

                dm = new List<DocumentTypeMaster>();

                if (cldm != null)
                {

                    dm = (from cl in cldm
                          join dms in db.DocumentTypeMaster.AsNoTracking() on cl.DocumentTypeID equals dms.DocumentTypeID
                          where dms.Active == true
                          select new DocumentTypeMaster()
                          {
                              DocumentTypeID = dms.DocumentTypeID,
                              Name = dms.Name,
                              DisplayName = dms.DisplayName,
                              Active = dms.Active,
                              DocumentLevel = dms.DocumentLevel,
                              CreatedOn = dms.CreatedOn,
                              ModifiedOn = dms.ModifiedOn,
                              DocumentFieldMasters = db.DocumentFieldMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == dms.DocumentTypeID && ld.Active == true).ToList(),
                              DocumetTypeTables = db.DocumetTypeTables.AsNoTracking().Where(ld => ld.DocumentTypeID == dms.DocumentTypeID).ToList(),
                              DocumentLevelDesc = dms.DocumentLevelDesc,
                              RuleDocumentTables = db.RuleDocumentTables.AsNoTracking().Where(rd => rd.DocumentID == dms.DocumentTypeID).ToList(),
                              Condition = cl.Condition,
                              CustDocumentLevel = cl.DocumentLevel
                          }).ToList();
                }
            }

            return dm.OrderBy(a => a.Name).ToList();
        }

        public List<DocumentTypeMaster> GetSystemDocumentTypesWithFieldsAll(Int64 loanTypeID)
        {
            List<DocumentTypeMaster> dm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                List<CustLoanDocMapping> cldm = db.CustLoanDocMapping.AsNoTracking().Where(cld => cld.LoanTypeID == loanTypeID && cld.Active == true).ToList();

                dm = new List<DocumentTypeMaster>();

                if (cldm != null)
                {

                    dm = (from cl in cldm
                          join dms in db.DocumentTypeMaster.AsNoTracking() on cl.DocumentTypeID equals dms.DocumentTypeID
                          where dms.Active == true
                          select new DocumentTypeMaster()
                          {
                              DocumentTypeID = dms.DocumentTypeID,
                              Name = dms.Name,
                              DisplayName = dms.DisplayName,
                              Active = dms.Active,
                              DocumentLevel = dms.DocumentLevel,
                              CreatedOn = dms.CreatedOn,
                              ModifiedOn = dms.ModifiedOn,
                              DocumentFieldMasters = db.DocumentFieldMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == dms.DocumentTypeID).ToList(),
                              DocumetTypeTables = db.DocumetTypeTables.AsNoTracking().Where(ld => ld.DocumentTypeID == dms.DocumentTypeID).ToList(),
                              DocumentLevelDesc = dms.DocumentLevelDesc,
                              RuleDocumentTables = db.RuleDocumentTables.AsNoTracking().Where(rd => rd.DocumentID == dms.DocumentTypeID).ToList(),
                              Condition = cl.Condition,
                              CustDocumentLevel = cl.DocumentLevel
                          }).ToList();
                }
            }

            return dm.OrderBy(a => a.Name).ToList();
        }


        public List<DocumentTypeMaster> GetSystemDocumentTypesAndLosDocumentTypesWithFields(Int64 loanTypeID)
        {
            List<DocumentTypeMaster> dm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                List<CustLoanDocMapping> cldm = db.CustLoanDocMapping.AsNoTracking().Where(cld => cld.LoanTypeID == loanTypeID && cld.Active == true).ToList();

                dm = new List<DocumentTypeMaster>();

                if (cldm != null)
                {

                    dm = (from cl in cldm
                          join dms in db.DocumentTypeMaster.AsNoTracking() on cl.DocumentTypeID equals dms.DocumentTypeID
                          where dms.Active == true
                          select new DocumentTypeMaster()
                          {
                              DocumentTypeID = dms.DocumentTypeID,
                              Name = dms.Name,
                              DisplayName = dms.DisplayName,
                              Active = dms.Active,
                              DocumentLevel = dms.DocumentLevel,
                              CreatedOn = dms.CreatedOn,
                              ModifiedOn = dms.ModifiedOn,
                              DocumentFieldMasters = db.DocumentFieldMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == dms.DocumentTypeID && ld.Active == true).ToList(),
                              DocumetTypeTables = db.DocumetTypeTables.AsNoTracking().Where(ld => ld.DocumentTypeID == dms.DocumentTypeID).ToList(),
                              DocumentLevelDesc = dms.DocumentLevelDesc,
                              RuleDocumentTables = db.RuleDocumentTables.AsNoTracking().Where(rd => rd.DocumentID == dms.DocumentTypeID).ToList(),
                              Condition = cl.Condition,
                              CustDocumentLevel = cl.DocumentLevel
                          }).ToList();

                    List<LOSDocument> losd = db.LOSDocument.AsNoTracking().ToList();
                    foreach (LOSDocument los in losd)
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
                    //foreach (CustLoanDocMapping loanDocMap in cldm)
                    //{
                    //    DocumentTypeMaster doc = db.DocumentTypeMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID && ld.Active == true).FirstOrDefault();
                    //    if (doc != null)
                    //    {
                    //        doc.DocumentFieldMasters = new List<DocumentFieldMaster>();

                    //        doc.DocumetTypeTables = new List<DocumetTypeTables>();

                    //        doc.RuleDocumentTables = new List<RuleDocumentTables>();

                    //        List<RuleDocumentTables> docR = db.RuleDocumentTables.AsNoTracking().Where(rd => rd.DocumentID == loanDocMap.DocumentTypeID).ToList();
                    //        List<DocumentFieldMaster> docF = db.DocumentFieldMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID && ld.Active == true).ToList();
                    //        List<DocumetTypeTables> docT = db.DocumetTypeTables.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID).ToList();
                    //        if (docF != null)
                    //            doc.DocumentFieldMasters = docF;

                    //        if (docT != null)
                    //            doc.DocumetTypeTables = docT;

                    //        if (docR != null)
                    //            doc.RuleDocumentTables = docR;

                    //        dm.Add(doc);
                    //    }
                    //}
                }
            }

            return dm.OrderBy(a => a.Name).ToList();
        }

        public List<string> GetLosSystemDocumentTypesWithDocFields(string losName)
        {
            // List<EncompassFields> ef = null;
            List<string> docTypes = null;
            List<string> DocumentTypes = new List<string>();
            using (var db = new DBConnect(SystemSchema))
            {
                docTypes = db.EncompassFields.AsNoTracking().Where(ed => ed.DocumentType != "").Select(ed => ed.DocumentType).Distinct().ToList();
                foreach (var item in docTypes)
                {
                    string newDocName = item.Replace("\n", " ");
                    DocumentTypes.Add(newDocName);
                }

            }
            return DocumentTypes;
        }
        public List<LosFieldDetails> GetLosSystemDocFields(string docName)
        {
            List<string> docFields = null;
            Dictionary<string, string> DocFields = new Dictionary<string, string>();
            List<LosFieldDetails> lf = new List<LosFieldDetails>();
            using (var db = new DBConnect(SystemSchema))
            {
                DocFields = db.EncompassFields.AsNoTracking().Where(ed => ed.DocumentType == docName).Select(ed => new { ed.FieldID, ed.FieldDesc }).ToDictionary(a => a.FieldID, a => a.FieldDesc);
                foreach (var item in DocFields)
                {
                    string _fieldDesc = item.Value.Replace("\n", " ");
                    lf.Add(new LosFieldDetails
                    {
                        FieldID = item.Key,
                        FieldDesc = _fieldDesc
                    });

                }
            }
            return lf;
        }
        public List<DocumentTypeMaster> GetDocumentTypesWithFields(string TenantSchema, Int64 CustomerID, Int64 loanTypeID)
        {
            List<DocumentTypeMaster> dm = null;

            using (var db = new DBConnect(TenantSchema))
            {
                List<CustLoanDocMapping> cldm = db.CustLoanDocMapping.AsNoTracking().Where(cld => cld.CustomerID == CustomerID && cld.LoanTypeID == loanTypeID).ToList();

                dm = new List<DocumentTypeMaster>();

                if (cldm != null)
                {
                    foreach (CustLoanDocMapping loanDocMap in cldm)
                    {
                        DocumentTypeMaster doc = db.DocumentTypeMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID).FirstOrDefault();
                        if (doc != null)
                        {
                            doc.DocumentFieldMasters = new List<DocumentFieldMaster>();

                            doc.DocumetTypeTables = new List<DocumetTypeTables>();

                            List<DocumentFieldMaster> docF = db.DocumentFieldMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID).ToList();
                            List<DocumetTypeTables> docT = db.DocumetTypeTables.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID).ToList();
                            if (docF != null)
                                doc.DocumentFieldMasters = docF;

                            if (docT != null)
                                doc.DocumetTypeTables = docT;

                            dm.Add(doc);
                        }
                    }
                }
            }

            return dm;
        }

        public List<CustReviewLoanReverifyMapping> GetSystemReVerifyMappings(Int64 loanTypeID)
        {
            List<CustReviewLoanReverifyMapping> dm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                dm = new List<CustReviewLoanReverifyMapping>();
                dm = db.CustReviewLoanReverifyMapping.AsNoTracking().Where(r => r.CustomerID == 1 && r.ReviewTypeID == 0 && r.LoanTypeID == loanTypeID).ToList();
            }

            return dm;
        }

        public ReverificationMaster GetSystemReVerification(Int64 reverificationID)
        {
            ReverificationMaster _reverify = null;

            using (var db = new DBConnect(SystemSchema))
            {
                _reverify = new ReverificationMaster();
                _reverify = db.ReverificationMaster.AsNoTracking().Where(r => r.ReverificationID == reverificationID).FirstOrDefault();
            }

            return _reverify;
        }

        public object SaveSysCheckListDetails(CheckListDetailMaster checkListDetailMaster, RuleMaster rulemasters, Int64 LoanTypeID)
        {
            object data;
            using (var db = new DBConnect(SystemSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    CheckListDetailMaster _lastCheck = db.CheckListDetailMaster.AsNoTracking().Where(c => c.CheckListID == checkListDetailMaster.CheckListID).ToList().OrderByDescending(a => a.SequenceID).FirstOrDefault();

                    checkListDetailMaster.SequenceID = _lastCheck != null ? _lastCheck.SequenceID + 1 : 0;

                    db.CheckListDetailMaster.Add(checkListDetailMaster);
                    db.SaveChanges();
                    rulemasters.CheckListDetailID = checkListDetailMaster.CheckListDetailID;
                    db.RuleMaster.Add(rulemasters);
                    db.SaveChanges();
                    trans.Commit();
                    // --------- insert / update checklist details into Tenant table
                    //CheckListMaster _getchecklistmaster = db.CheckListMaster.Where(x => x.CheckListID == checkListDetailMaster.CheckListID && x.Sync == true).FirstOrDefault();
                    //if (_getchecklistmaster != null)
                    //{
                    //    List<CheckListDetailMaster> GetTenantCheckListDetails = db.CheckListDetailMaster.Where(x => x.CheckListID == checkListDetailMaster.CheckListID).ToList();
                    //    AddCustomerCheckListDetails(LoanTypeID, checkListDetailMaster, rulemasters);
                    //}
                    var dData = (from cl in db.CheckListDetailMaster
                                 join rm in db.RuleMaster on cl.CheckListDetailID equals rm.CheckListDetailID into rmJoin
                                 from rmGroup in rmJoin.DefaultIfEmpty()
                                 join l in db.EncompassFields on cl.LOSFieldToEvalRule equals l.ID into los
                                 from losGroup in los.DefaultIfEmpty()
                                 where cl.CheckListID == checkListDetailMaster.CheckListID
                                 select new
                                 {
                                     CheckListDetailID = cl.CheckListDetailID,
                                     ChecklistActive = cl.Active,
                                     RuleID = rmGroup.RuleID == null ? 0 : rmGroup.RuleID,
                                     ChecklistGroupId = cl.CheckListID,
                                     CheckListName = cl.Name,
                                     Category = cl.Category,
                                     CheckListDescription = cl.Description,
                                     CreatedOn = cl.CreatedOn,
                                     RuleDescription = rmGroup.RuleDescription == null ? String.Empty : rmGroup.RuleDescription,
                                     RuleJson = rmGroup.RuleJson == null ? String.Empty : rmGroup.RuleJson,
                                     DocumentType = rmGroup.DocumentType == null ? String.Empty : rmGroup.DocumentType,
                                     UserID = cl.UserID,
                                     SequenceID = cl.SequenceID,
                                     DocVersion = rmGroup.DocVersion,
                                     LOSFieldDescription = string.IsNullOrEmpty(losGroup.FieldDesc) ? string.Empty : losGroup.FieldDesc,
                                     LOSValue = string.IsNullOrEmpty(cl.LOSValueToEvalRule) ? string.Empty : cl.LOSValueToEvalRule,
                                     RuleType = cl.Rule_Type,
                                     LosIsMatched = cl.LosIsMatched
                                 }).ToList();

                    data = (from d in dData
                            join r in db.Users on d.UserID equals r.UserID into lu
                            from ul in lu.DefaultIfEmpty()
                            select new
                            {
                                CheckListDetailID = d.CheckListDetailID,
                                ChecklistActive = d.ChecklistActive,
                                RuleID = d.RuleID,
                                ChecklistGroupId = d.ChecklistGroupId,
                                CheckListName = d.CheckListName,
                                CheckListDescription = d.CheckListDescription,
                                Category = d.Category,
                                CreatedOn = d.CreatedOn,
                                RuleDescription = d.RuleDescription,
                                RuleJson = d.RuleJson,
                                DocumentType = d.DocumentType,
                                UserID = d.UserID,
                                FirstName = ul?.FirstName ?? "System",
                                LastName = ul?.LastName ?? String.Empty,
                                SequenceID = d.SequenceID,
                                DocVersion = d.DocVersion,
                                LOSFieldDescription = d.LOSFieldDescription,
                                LOSValue = d.LOSValue,
                                RuleType = d.RuleType,
                                LosIsMatched = d.LosIsMatched,
                            }).ToList();
                }
            }
            return data;
        }

        public object DeleteSysCheckListItem(long[] checkListDetailsID, Int64 LoanTypeID)
        {
            string ChecklistItemName = "";
            using (var db = new DBConnect(SystemSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    foreach (var checklist in checkListDetailsID)
                    {
                        RuleMaster rulemaster = db.RuleMaster.AsNoTracking().Where(r => r.CheckListDetailID == checklist).FirstOrDefault();

                        if (rulemaster != null)
                        {
                            db.Entry(rulemaster).State = EntityState.Deleted;
                            db.RuleMaster.Remove(rulemaster);
                            db.SaveChanges();
                        }

                        CheckListDetailMaster checklistmaster = db.CheckListDetailMaster.Where(ch => ch.CheckListDetailID == checklist).FirstOrDefault();
                        if (checklistmaster != null)
                        {
                            ChecklistItemName = checklistmaster.Name;

                            db.Entry(checklistmaster).State = EntityState.Deleted;
                            db.CheckListDetailMaster.Remove(checklistmaster);
                            db.SaveChanges();
                        }
                        // Delete checklistdetails  from tenant level (customer)
                        //  DeleteCustomerCheckListDetails(checklist, LoanTypeID, ChecklistItemName);
                        // end
                    }
                    trans.Commit();
                    return true;
                }

            }
            return false;
        }

        public object CloneSysCheckListItem(long[] checkListDetailsID, string modifiedCheckListName, Int64 LoanTypeID)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                foreach (var clonechecklistid in checkListDetailsID)
                {
                    CheckListDetailMaster checklistdetials = db.CheckListDetailMaster.AsNoTracking().Where(c => c.CheckListDetailID == clonechecklistid).FirstOrDefault();
                    if (checklistdetials != null)
                    {
                        RuleMaster rm = db.RuleMaster.AsNoTracking().Where(r => r.CheckListDetailID == checklistdetials.CheckListDetailID).FirstOrDefault();
                        checklistdetials.Name = modifiedCheckListName;
                        checklistdetials.CheckListDetailID = 0;
                        db.CheckListDetailMaster.Add(checklistdetials);
                        db.SaveChanges();
                        rm.CheckListDetailID = checklistdetials.CheckListDetailID;
                        rm.RuleID = 0;
                        db.RuleMaster.Add(rm);
                        db.SaveChanges();
                        // --------- insert / update checklist details into Tenant table
                        //CheckListMaster _getchecklistmaster = db.CheckListMaster.Where(x => x.CheckListID == checklistdetials.CheckListID && x.Sync == true).FirstOrDefault();
                        //if (_getchecklistmaster != null)
                        //{
                        //    List<CheckListDetailMaster> GetTenantCheckListDetails = db.CheckListDetailMaster.Where(x => x.CheckListID == checklistdetials.CheckListID).ToList();
                        //    AddCustomerCheckListDetails(LoanTypeID, checklistdetials, rm);
                        //}
                    }
                }
                return true;
            }
            return false;
        }

        public CheckListMaster GetCheckList(Int64 loanTypeID)
        {
            CheckListMaster clm = null;
            using (var db = new DBConnect(SystemSchema))
            {
                var custLoancheckmap = db.CustReviewLoanCheckMapping.AsNoTracking().Where(c => c.ReviewTypeID == 0 && c.LoanTypeID == loanTypeID && c.Active == true).FirstOrDefault();
                if (custLoancheckmap != null)
                {
                    clm = db.CheckListMaster.AsNoTracking().Where(l => l.CheckListID == custLoancheckmap.CheckListID).FirstOrDefault();
                }
            }
            return clm;
        }

        public StackingOrderMaster GetStackingOrder(Int64 loanTypeID)
        {
            StackingOrderMaster som = null;
            using (var db = new DBConnect(SystemSchema))
            {
                var custLoanstackmap = db.CustReviewLoanStackMapping.AsNoTracking().Where(c => c.ReviewTypeID == 0 && c.LoanTypeID == loanTypeID && c.Active == true).FirstOrDefault();
                if (custLoanstackmap != null)
                {
                    som = db.StackingOrderMaster.AsNoTracking().Where(l => l.StackingOrderID == custLoanstackmap.StackingOrderID).FirstOrDefault();
                }
            }
            return som;
        }

        public object SaveSysEditCheckListDetails(CheckListDetailMaster checkListDetailMaster, RuleMaster rulemasters, Int64 LoanTypeID)
        {

            object data;
            using (var db = new DBConnect(SystemSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    CheckListDetailMaster _lastCheck = db.CheckListDetailMaster.AsNoTracking().Where(c => c.CheckListID == checkListDetailMaster.CheckListID).ToList().OrderByDescending(a => a.SequenceID).FirstOrDefault();

                    checkListDetailMaster.SequenceID = _lastCheck != null ? _lastCheck.SequenceID + 1 : 0;

                    db.CheckListDetailMaster.Add(checkListDetailMaster);
                    db.SaveChanges();
                    rulemasters.CheckListDetailID = checkListDetailMaster.CheckListDetailID;
                    db.RuleMaster.Add(rulemasters);
                    db.SaveChanges();
                    trans.Commit();
                    // --------- insert / update checklist details into Tenant table
                    //CheckListMaster _getchecklistmaster = db.CheckListMaster.Where(x => x.CheckListID == checkListDetailMaster.CheckListID && x.Sync == true).FirstOrDefault();
                    //if (_getchecklistmaster != null)
                    //{
                    //    List<CheckListDetailMaster> GetTenantCheckListDetails = db.CheckListDetailMaster.Where(x => x.CheckListID == checkListDetailMaster.CheckListID).ToList();
                    //    AddCustomerCheckListDetails(LoanTypeID, checkListDetailMaster, rulemasters);
                    //}
                    var dData = (from cl in db.CheckListDetailMaster
                                 join rm in db.RuleMaster on cl.CheckListDetailID equals rm.CheckListDetailID into rmJoin
                                 from rmGroup in rmJoin.DefaultIfEmpty()
                                 join l in db.EncompassFields on cl.LOSFieldToEvalRule equals l.ID into los
                                 from losGroup in los.DefaultIfEmpty()
                                 where cl.CheckListID == checkListDetailMaster.CheckListID
                                 select new
                                 {
                                     CheckListDetailID = cl.CheckListDetailID,
                                     ChecklistActive = cl.Active,
                                     RuleID = rmGroup.RuleID == null ? 0 : rmGroup.RuleID,
                                     ChecklistGroupId = cl.CheckListID,
                                     CheckListName = cl.Name,
                                     CheckListDescription = cl.Description,
                                     CreatedOn = cl.CreatedOn,
                                     Category = cl.Category,
                                     RuleDescription = rmGroup.RuleDescription == null ? String.Empty : rmGroup.RuleDescription,
                                     RuleJson = rmGroup.RuleJson == null ? String.Empty : rmGroup.RuleJson,
                                     DocumentType = rmGroup.DocumentType == null ? String.Empty : rmGroup.DocumentType,
                                     UserID = cl.UserID,
                                     SequenceID = cl.SequenceID,
                                     DocVersion = rmGroup.DocVersion,
                                     LOSFieldDescription = string.IsNullOrEmpty(losGroup.FieldDesc) ? string.Empty : "#" + losGroup.FieldID + "# - " + losGroup.FieldDesc,
                                     LOSValue = string.IsNullOrEmpty(cl.LOSValueToEvalRule) ? string.Empty : cl.LOSValueToEvalRule,
                                     RuleType = cl.Rule_Type,
                                     LosIsMatched = cl.LosIsMatched
                                 }).ToList();

                    data = (from d in dData
                            join r in db.Users on d.UserID equals r.UserID into lu
                            from ul in lu.DefaultIfEmpty()
                            select new
                            {
                                CheckListDetailID = d.CheckListDetailID,
                                ChecklistActive = d.ChecklistActive,
                                RuleID = d.RuleID,
                                ChecklistGroupId = d.ChecklistGroupId,
                                CheckListName = d.CheckListName,
                                CheckListDescription = d.CheckListDescription,
                                CreatedOn = d.CreatedOn,
                                Category = d.Category,
                                RuleDescription = d.RuleDescription,
                                RuleJson = d.RuleJson,
                                DocumentType = d.DocumentType,
                                UserID = d.UserID,
                                FirstName = ul?.FirstName ?? "System",
                                LastName = ul?.LastName ?? String.Empty,
                                SequenceID = d.SequenceID,
                                DocVersion = d.DocVersion,
                                LOSFieldDescription = d.LOSFieldDescription,
                                LOSValue = d.LOSValue,
                                RuleType = d.RuleType,
                                LosIsMatched = d.LosIsMatched,
                            }).ToList();
                }
            }
            return data;
        }

        public object GetAllCheckListItems(Int64 checkListID)
        {
            List<ChecklistDetailOutput> dData = new List<ChecklistDetailOutput>();

            using (var db = new DBConnect(SystemSchema))
            {
                dData = (from cl in db.CheckListDetailMaster.AsNoTracking()
                         join rm in db.RuleMaster.AsNoTracking() on cl.CheckListDetailID equals rm.CheckListDetailID
                         where cl.CheckListID == checkListID
                         select new ChecklistDetailOutput()
                         {
                             CheckListDetailID = cl.CheckListDetailID,
                             ChecklistActive = cl.Active,
                             RuleID = rm.RuleID,
                             ChecklistGroupId = cl.CheckListID,
                             CheckListName = cl.Name,
                             CheckListDescription = cl.Description,
                             Category = cl.Category,
                             CreatedOn = cl.CreatedOn,
                             RuleDescription = rm.RuleDescription == null ? string.Empty : rm.RuleDescription,
                             RuleJson = rm.RuleJson == null ? string.Empty : rm.RuleJson,
                             DocumentType = rm.DocumentType == null ? string.Empty : rm.DocumentType,
                             UserID = cl.UserID == null ? 0 : cl.UserID,
                             SequenceID = cl.SequenceID,
                             DocVersion = rm.DocVersion,
                             LOSFieldDescription = string.Empty,
                             LOSFieldToEvalRule = cl.LOSFieldToEvalRule,
                             LOSValueToEvalRule = string.IsNullOrEmpty(cl.LOSValueToEvalRule) ? string.Empty : cl.LOSValueToEvalRule,
                             RuleType = cl.Rule_Type,
                             LosIsMatched = cl.LosIsMatched
                         }).OrderBy(m => m.SequenceID).ToList();

                foreach (var losGroup in dData)
                {
                    if (!string.IsNullOrEmpty(losGroup.LOSValueToEvalRule))
                    {
                        EncompassFields eField = db.EncompassFields.AsNoTracking().Where(e => e.ID == losGroup.LOSFieldToEvalRule).FirstOrDefault();
                        if (eField != null)
                            losGroup.LOSFieldDescription = string.IsNullOrEmpty(eField.FieldDesc) ? string.Empty : "#" + eField.FieldID + "# - " + eField.FieldDesc;
                    }

                    if (losGroup.UserID > 0)
                    {
                        User user = db.Users.AsNoTracking().Where(u => u.UserID == losGroup.UserID).FirstOrDefault();
                        if (user != null)
                        {
                            losGroup.FirstName = user.FirstName;
                            losGroup.LastName = user.LastName;
                        }
                    }
                    else
                    {
                        losGroup.FirstName = "System";
                        losGroup.LastName = string.Empty;
                    }
                }

            }
            using (var db = new DBConnect("T1"))
            {
                foreach (var losGroup in dData)
                {
                    if (losGroup.UserID > 0)
                    {
                        User user = db.Users.AsNoTracking().Where(u => u.UserID == losGroup.UserID).FirstOrDefault();
                        if (user != null)
                        {
                            losGroup.FirstName = user.FirstName;
                            losGroup.LastName = user.LastName;
                        }
                    }
                    else
                    {
                        losGroup.FirstName = "System";
                        losGroup.LastName = string.Empty;
                    }
                }
            }
            return dData;
        }

        public object UpdateSysCheckListDetails(CheckListDetailMaster checkListDetailMaster, RuleMaster rulemasters, Int64 LoanTypeID)
        {
            object data;
            using (var db = new DBConnect(SystemSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    CheckListDetailMaster updatechecklistdetailsmaster = db.CheckListDetailMaster.AsNoTracking().Where(up => up.CheckListDetailID == checkListDetailMaster.CheckListDetailID).FirstOrDefault();
                    if (updatechecklistdetailsmaster != null)
                    {
                        updatechecklistdetailsmaster.Name = checkListDetailMaster.Name;
                        updatechecklistdetailsmaster.Description = checkListDetailMaster.Description;
                        updatechecklistdetailsmaster.Rule_Type = checkListDetailMaster.Rule_Type;
                        updatechecklistdetailsmaster.UserID = checkListDetailMaster.UserID;
                        updatechecklistdetailsmaster.Active = checkListDetailMaster.Active;
                        updatechecklistdetailsmaster.ModifiedOn = DateTime.Now;
                        updatechecklistdetailsmaster.Category = checkListDetailMaster.Category;
                        updatechecklistdetailsmaster.LOSFieldToEvalRule = checkListDetailMaster.LOSFieldToEvalRule;
                        updatechecklistdetailsmaster.LOSValueToEvalRule = checkListDetailMaster.LOSValueToEvalRule;
                        updatechecklistdetailsmaster.LosIsMatched = checkListDetailMaster.LosIsMatched;
                        db.Entry(updatechecklistdetailsmaster).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    RuleMaster updaterulemasters = db.RuleMaster.AsNoTracking().Where(upd => upd.RuleID == rulemasters.RuleID).FirstOrDefault();
                    if (updaterulemasters != null)
                    {
                        updaterulemasters.RuleDescription = rulemasters.RuleDescription;
                        updaterulemasters.RuleJson = rulemasters.RuleJson;
                        updaterulemasters.Active = rulemasters.Active;
                        updaterulemasters.DocumentType = rulemasters.DocumentType;
                        updaterulemasters.ActiveDocumentType = rulemasters.ActiveDocumentType;
                        updaterulemasters.DocVersion = rulemasters.DocVersion;
                        db.Entry(updaterulemasters).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    trans.Commit();
                    // --------- insert / update checklist details into Tenant table
                    CheckListMaster _getchecklistmaster = db.CheckListMaster.Where(x => x.CheckListID == checkListDetailMaster.CheckListID && x.Sync == true).FirstOrDefault();
                    if (_getchecklistmaster != null)
                    {
                        UpdateCustomerCheckListDetails(LoanTypeID, checkListDetailMaster, rulemasters);
                    }
                    var dData = (from cl in db.CheckListDetailMaster
                                 join rm in db.RuleMaster on cl.CheckListDetailID equals rm.CheckListDetailID into rmJoin
                                 from rmGroup in rmJoin.DefaultIfEmpty()
                                 join l in db.EncompassFields on cl.LOSFieldToEvalRule equals l.ID into los
                                 from losGroup in los.DefaultIfEmpty()
                                 where cl.CheckListID == checkListDetailMaster.CheckListID
                                 select new
                                 {
                                     CheckListDetailID = cl.CheckListDetailID,
                                     ChecklistActive = cl.Active,
                                     RuleID = rmGroup.RuleID == null ? 0 : rmGroup.RuleID,
                                     ChecklistGroupId = cl.CheckListID,
                                     CheckListName = cl.Name,
                                     CheckListDescription = cl.Description,
                                     CreatedOn = cl.CreatedOn,
                                     Category = cl.Category,
                                     RuleDescription = rmGroup.RuleDescription == null ? String.Empty : rmGroup.RuleDescription,
                                     RuleJson = rmGroup.RuleJson == null ? String.Empty : rmGroup.RuleJson,
                                     DocumentType = rmGroup.DocumentType == null ? String.Empty : rmGroup.DocumentType,
                                     UserID = cl.UserID,
                                     SequenceID = cl.SequenceID,
                                     DocVersion = rmGroup.DocVersion,
                                     LOSFieldDescription = string.IsNullOrEmpty(losGroup.FieldDesc) ? string.Empty : losGroup.FieldDesc,
                                     LOSValue = string.IsNullOrEmpty(cl.LOSValueToEvalRule) ? string.Empty : cl.LOSValueToEvalRule,
                                     RuleType = cl.Rule_Type,
                                     LosIsMatched = cl.LosIsMatched
                                 }).ToList();

                    data = (from d in dData
                            join r in db.Users on d.UserID equals r.UserID into lu
                            from ul in lu.DefaultIfEmpty()
                            select new
                            {
                                CheckListDetailID = d.CheckListDetailID,
                                ChecklistActive = d.ChecklistActive,
                                RuleID = d.RuleID,
                                ChecklistGroupId = d.ChecklistGroupId,
                                CheckListName = d.CheckListName,
                                CheckListDescription = d.CheckListDescription,
                                CreatedOn = d.CreatedOn,
                                Category = d.Category,
                                RuleDescription = d.RuleDescription,
                                RuleJson = d.RuleJson,
                                DocumentType = d.DocumentType,
                                UserID = d.UserID,
                                FirstName = ul?.FirstName ?? "System",
                                LastName = ul?.LastName ?? String.Empty,
                                SequenceID = d.SequenceID,
                                LoanType = string.Empty,
                                DocVersion = d.DocVersion,
                                LOSFieldDescription = d.LOSFieldDescription,
                                LOSValue = d.LOSValue,
                                RuleType = d.RuleType,
                                LosIsMatched = d.LosIsMatched,
                            }).ToList();
                }
            }
            return data;
        }

        public Object GetEditSysDocTypeMasters(long CheckListDetailID, long loanTypeID)
        {
            //List<DocumentTypeMaster> dm = null;
            List<DocumentTypeMaster> docMaster = GetSystemDocumentTypesWithFields(loanTypeID);
            //using (var db = new DBConnect(SystemSchema))
            //{
            //    List<RuleMaster> ruleMasterDatas = db.RuleMaster.AsNoTracking().Where(rm => rm.CheckListDetailID == CheckListDetailID).ToList();
            //    if (ruleMasterDatas != null || ruleMasterDatas.Count > 0)
            //    {
            //        if (ruleMasterDatas[0].DocumentType != "")
            //        {
            //            //List<Int64> docIDs = ruleMasterDatas[0].DocumentType.Split(',').Select(docId => Convert.ToInt64(docId)).ToList();
            //            dm = new List<DocumentTypeMaster>();
            //            foreach (Int64 docTypeID in docIDs)
            //            {
            //                DocumentTypeMaster doc = db.DocumentTypeMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == docTypeID).FirstOrDefault();

            //                dm.Add(doc);
            //            }
            //        }

            //    }

            //}
            return new { AllDocTypes = docMaster };
        }

        public List<object> GetSysDocumentFieldMasters(long documentTypeID)
        {
            List<object> docFiledMasters = null;
            using (var db = new DBConnect(SystemSchema))
            {
                //docFiledMasters = (from dm in db.DocumentTypeMaster
                //                   where (documentTypeID.Contains((int)dm.DocumentTypeID))
                //                   select new
                //                   {
                //                       DocID = dm.DocumentTypeID,
                //                       DocName = dm.Name,
                //                       Fields = (from f in db.DocumentFieldMaster
                //                                 where f.DocumentTypeID == dm.DocumentTypeID
                //                                 select f).ToList()
                //                   }).ToList<object>();
                docFiledMasters = db.DocumentFieldMaster.AsNoTracking().Where(df => df.DocumentTypeID == documentTypeID).ToList<object>();
            }
            return docFiledMasters;
        }

        public List<DocumentTypeMaster> GetAllSysDocTypeMasters()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.DocumentTypeMaster.AsNoTracking().ToList();
            }
        }

        public List<DocumentTypeMaster> GetSysStackDocLibMatchingDocTypes(List<StackingOrderDetailMaster> _stacksDocs)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                List<DocumentTypeMaster> _dsLs = new List<DocumentTypeMaster>();

                _dsLs = db.DocumentTypeMaster.AsNoTracking().Join(_stacksDocs,
                    post => post.DocumentTypeID,
                    meta => meta.DocumentTypeID,
                    (post, meta) => post).ToList();

                //_dsLs = (from dm in db.DocumentTypeMaster.AsNoTracking()
                //        join s in _stacksDocs on dm.DocumentTypeID equals s.DocumentTypeID
                //        select dm).ToList();

                return _dsLs;

            }
        }

        public object AddSysCheckList(CheckListMaster checkListMaster)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    db.CheckListMaster.Add(checkListMaster);
                    db.SaveChanges();
                    trans.Commit();
                    return db.CheckListMaster.AsNoTracking().Where(chls => chls.CheckListID == checkListMaster.CheckListID).ToList();
                    //return true;
                }
            }
            //return false;
        }

        public object GetAllSysCheckListDetails()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                List<CheckListMaster> lsCheckGrp = db.CheckListMaster.AsNoTracking().ToList();
                return lsCheckGrp;
            }
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
                    if (doc != null)
                        dm.Add(doc);
                }
            }

            return dm;
        }

        public DocumentTypeMaster GetSystemDocumentType(Int64 documentTypeID)
        {
            DocumentTypeMaster documentTypeMaster = null;
            using (var db = new DBConnect(SystemSchema))
            {
                documentTypeMaster = db.DocumentTypeMaster.AsNoTracking().Where(d => d.DocumentTypeID == documentTypeID && d.Active).FirstOrDefault();
            }
            return documentTypeMaster;
        }

        #endregion

        #region Document Field Master

        public List<DocumentFieldMaster> GetSystemDocumentFields(Int64 docTypeID)
        {
            List<DocumentFieldMaster> dfm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                dfm = db.DocumentFieldMaster.AsNoTracking().Where(cld => cld.DocumentTypeID == docTypeID && cld.Active).ToList();
            }

            return dfm;
        }

        public List<DocumetTypeTables> GetSystemDocumentTables(Int64 docTypeID)
        {
            List<DocumetTypeTables> dfm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                dfm = db.DocumetTypeTables.AsNoTracking().Where(cld => cld.DocumentTypeID == docTypeID).ToList();
            }

            return dfm;
        }

        #endregion

        #region Checklist Master

        public List<CheckListMaster> GetSystemCheckListMaster()
        {
            List<CheckListMaster> lsCheckListMaster = null;

            using (var db = new DBConnect(SystemSchema))
            {
                lsCheckListMaster = new List<CheckListMaster>();
                lsCheckListMaster = db.CheckListMaster.AsNoTracking().ToList();
            }

            return lsCheckListMaster;
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

        public CheckListMaster GetCheckListGroup(DBConnect db, Int64 CheckListID)
        {
            CheckListMaster clm = db.CheckListMaster.Where(cd => cd.CheckListID == CheckListID && cd.Active == true).AsNoTracking().FirstOrDefault();

            if (clm != null)
            {
                clm.CheckListDetailMasters = db.CheckListDetailMaster.Where(cd => cd.CheckListID == clm.CheckListID && cd.Active == true).AsNoTracking().ToList();

                foreach (CheckListDetailMaster item in clm.CheckListDetailMasters)
                {
                    RuleMaster rm = db.RuleMaster.AsNoTracking().Where(r => r.CheckListDetailID == item.CheckListDetailID && r.Active == true).FirstOrDefault();
                    item.RuleMasters = rm;
                }
            }

            return clm;
        }

        public StackingOrderMaster GetSysStackingOrder(DBConnect db, Int64 StackingOrderID)
        {
            StackingOrderMaster clm = db.StackingOrderMaster.Where(cd => cd.StackingOrderID == StackingOrderID).AsNoTracking().FirstOrDefault();

            if (clm != null)
            {
                clm.StackingOrderDetailMasters = db.StackingOrderDetailMaster.Where(cd => cd.StackingOrderID == clm.StackingOrderID && cd.Active == true).AsNoTracking().ToList();
            }

            return clm;
        }

        #endregion

        #region Stacking Order Master


        public object SaveSystemStackingOrderDetails(int stackOrderID, string StackOrderName, bool IsActive, List<GetStackOrder> stackingOrderDetails)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                if (stackOrderID != 0)
                {
                    using (var trans = db.Database.BeginTransaction())
                    {
                        var item = db.StackingOrderMaster.Where(sodm => sodm.StackingOrderID == stackOrderID).FirstOrDefault();
                        item.Description = StackOrderName;
                        item.ModifiedOn = DateTime.Now;
                        item.Active = IsActive;
                        db.StackingOrderDetailMaster.RemoveRange(db.StackingOrderDetailMaster.Where(sodm => sodm.StackingOrderID == stackOrderID));
                        db.StackingOrderGroupmasters.RemoveRange(db.StackingOrderGroupmasters.Where(sogm => sogm.StackingOrderID == stackOrderID));
                        db.SaveChanges();
                        int sequence = 1;
                        string dupName = "";
                        StackingOrderGroupmasters stackOrderGrp = null;
                        foreach (var stackingoderdetail in stackingOrderDetails)
                        {
                            //StackingOrderGroupmasters stackOrderGrp = null;
                            if (stackingoderdetail.isGroup && (stackingoderdetail.Name != dupName))
                            {
                                stackOrderGrp = null;
                                dupName = stackingoderdetail.Name;

                                stackOrderGrp = db.StackingOrderGroupmasters.Add(new StackingOrderGroupmasters()
                                {
                                    StackingOrderID = stackOrderID,
                                    StackingOrderGroupName = stackingoderdetail.Name,
                                    Active = true,
                                    GroupSortField = stackingoderdetail.StackingOrderFieldName,
                                    CreatedOn = DateTime.Now,
                                    ModifiedOn = DateTime.Now
                                });
                                db.SaveChanges();
                            }
                            db.StackingOrderDetailMaster.Add(new StackingOrderDetailMaster()
                            {
                                DocumentTypeID = stackingoderdetail.ID,
                                StackingOrderID = stackOrderID,
                                SequenceID = sequence,
                                Active = true,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                StackingOrderGroupID = stackingoderdetail.isGroup ? stackOrderGrp.StackingOrderGroupID : 0
                            });
                            db.SaveChanges();
                            sequence++;


                        }
                        trans.Commit();
                        return new { Success = true, StackOrderID = stackOrderID };
                    }
                }
                else
                {

                    using (var trans = db.Database.BeginTransaction())
                    {
                        var item = db.StackingOrderMaster.Add(
                            new StackingOrderMaster()
                            {
                                Description = StackOrderName,
                                Active = IsActive,
                                CreatedOn = DateTime.Now
                            }
                        );
                        db.SaveChanges();

                        int sequence = 1;
                        string dupName = "";
                        StackingOrderGroupmasters stackOrderGrp = null;
                        foreach (var stackingoderdetail in stackingOrderDetails)
                        {
                            //StackingOrderDetailMaster stackorder = db.StackingOrderDetailMaster.AsNoTracking().Where(sodm => sodm.StackingOrderDetailID == stackingOrderDetails.StackingOrderDetailID)

                            if (stackingoderdetail.isGroup && (stackingoderdetail.Name != dupName))
                            {
                                stackOrderGrp = null;
                                dupName = stackingoderdetail.Name;
                                stackOrderGrp = db.StackingOrderGroupmasters.Add(new StackingOrderGroupmasters()
                                {
                                    StackingOrderID = item.StackingOrderID,
                                    StackingOrderGroupName = stackingoderdetail.Name,
                                    Active = true,
                                    CreatedOn = DateTime.Now,
                                    ModifiedOn = DateTime.Now,
                                    GroupSortField = stackingoderdetail.StackingOrderFieldName,
                                });
                                db.SaveChanges();
                            }
                            db.StackingOrderDetailMaster.Add(new StackingOrderDetailMaster()
                            {
                                DocumentTypeID = stackingoderdetail.ID,
                                StackingOrderID = item.StackingOrderID,
                                SequenceID = sequence,
                                Active = true,
                                StackingOrderGroupID = stackingoderdetail.isGroup ? stackOrderGrp.StackingOrderGroupID : 0
                            });
                            db.SaveChanges();
                            sequence++;
                        }
                        trans.Commit();
                        return new { Success = true, StackOrderID = item.StackingOrderID };
                    }

                }
            }
        }


        public object GetAllSystemStackingOrderDetails()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.StackingOrderMaster.AsNoTracking().ToList();
            }
        }

        public StackingOrderGroupmasters AddStackingOrderMasterGroup(Int64? StackingOrderGroupId)
        {
            StackingOrderGroupmasters lsStackingOrderGroupMaster = null;
            using (var db = new DBConnect(SystemSchema))
            {

                lsStackingOrderGroupMaster = db.StackingOrderGroupmasters.AsNoTracking().Where(a => a.StackingOrderGroupID == StackingOrderGroupId).FirstOrDefault();
            }
            return lsStackingOrderGroupMaster;
        }


        public List<StackingOrderMaster> GetSystemStackingOrderMaster()
        {
            List<StackingOrderMaster> lsStackingOrderMaster = null;

            using (var db = new DBConnect(SystemSchema))
            {
                lsStackingOrderMaster = new List<StackingOrderMaster>();
                lsStackingOrderMaster = db.StackingOrderMaster.AsNoTracking().ToList();
            }

            return lsStackingOrderMaster;
        }

        public List<WorkFlowStatusMaster> GetRetentionWorkFlowStatus()
        {
            List<WorkFlowStatusMaster> RetentionLoanStatus;
            using (var db = new DBConnect(SystemSchema))
            {
                RetentionLoanStatus = db.WorkFlowStatusMaster.AsNoTracking().Where(wfstm => wfstm.StatusID >= StatusConstant.LOAN_PURGED && wfstm.StatusID < StatusConstant.PENDING_OCR && wfstm.StatusID != StatusConstant.LOAN_EXPIRED).ToList();
            }
            return RetentionLoanStatus;
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

        #endregion

        #region Workflow Status Master

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
                WorkFlowStatusMaster = db.WorkFlowStatusMaster.AsNoTracking().Where(w => w.StatusID < StatusConstant.PENDING_OCR || w.StatusID == StatusConstant.LOAN_DELETED
                || w.StatusID == StatusConstant.CLASSIFICATION_WAITING || w.StatusID == StatusConstant.FIELD_EXTRACTION_WAITING || w.StatusID == StatusConstant.LOS_LOAN_STAGED)
                                .Where(w => w.StatusID != StatusConstant.IDC_COMPLETE
                                         && w.StatusID != StatusConstant.PENDING_IDC
                                         && w.StatusID != StatusConstant.PENDING_BOX_DOWNLOAD
                                         && w.StatusID != StatusConstant.FAILED_BOX_DOWNLOAD
                                         //&& w.StatusID != StatusConstant.READY_FOR_IDC
                                         //&& w.StatusID != StatusConstant.IDC_ERROR
                                         && w.StatusID > StatusConstant.DELETE_FILE_READY).ToList();
            }
            return WorkFlowStatusMaster;
        }

        #endregion

        #region System Mapping

        public bool DeleteDocumentField(Int64 FieldID)
        {
            bool result = false;

            using (var db = new DBConnect(SystemSchema))
            {
                DocumentFieldMaster _field = db.DocumentFieldMaster.AsNoTracking().Where(f => f.FieldID == FieldID).FirstOrDefault();
                if (_field != null)
                {
                    db.Entry(_field).State = EntityState.Deleted;
                    db.SaveChanges();
                }
                result = true;
            }

            return result;
        }

        public Int64 AddDocumentField(Int64 DocumentTypeID, string FieldName, string FieldDisplayName)
        {

            using (var db = new DBConnect(SystemSchema))
            {
                DocumentFieldMaster _docField = new DocumentFieldMaster()
                {
                    Name = FieldName,
                    DisplayName = FieldDisplayName,
                    DocOrderByField = null,
                    AllowAccuracyCalc = false,
                    DocumentTypeID = DocumentTypeID,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Active = true,
                    OrderBy = false
                };

                db.DocumentFieldMaster.Add(_docField);
                db.SaveChanges();

                return _docField.FieldID;
            }
        }

        public object GetReviewLoanMapped(Int64 ReviewTypeID)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                List<CustReviewLoanCheckMapping> LoanChecks = db.CustReviewLoanCheckMapping.AsNoTracking().Where(c => c.CustomerID == 1 && c.ReviewTypeID == 0).ToList();
                List<CustReviewLoanStackMapping> LoanStacks = db.CustReviewLoanStackMapping.AsNoTracking().Where(c => c.CustomerID == 1 && c.ReviewTypeID == 0).ToList();

                var lsLoanTypes = (from cl in LoanChecks
                                   join ls in LoanStacks on cl.LoanTypeID equals ls.LoanTypeID
                                   select new
                                   {
                                       LoanTypeID = cl.LoanTypeID
                                   }).ToList();

                List<LoanTypeMaster> lsLoanTypeMaster = new List<LoanTypeMaster>();

                foreach (var item in lsLoanTypes)
                {
                    LoanTypeMaster lType = db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeID == item.LoanTypeID && l.Type == 0).FirstOrDefault();

                    if (lType != null)
                        lsLoanTypeMaster.Add(lType);
                }

                List<CustReviewLoanMapping> mappedLoanTypes = db.CustReviewLoanMapping.AsNoTracking().Where(c => c.ReviewTypeID == ReviewTypeID && c.Active == true).ToList();

                var lsMappedLoanTypes = (from lt in lsLoanTypeMaster
                                         join map in mappedLoanTypes on lt.LoanTypeID equals map.LoanTypeID into rmJoin
                                         from rmGroup in rmJoin.DefaultIfEmpty()
                                         select new
                                         {
                                             LoanTypeID = lt.LoanTypeID,
                                             LoanTypeName = lt.LoanTypeName,
                                             Mapped = rmGroup?.LoanTypeID != null,
                                             DBMapped = rmGroup?.LoanTypeID != null
                                         }).ToList();

                var lsUnMappedLoanTypes = lsLoanTypeMaster
                                        .Where(x => (!(lsMappedLoanTypes.Any(y => x.LoanTypeID == y.LoanTypeID))) && x.Active == true)
                                        .Select(a => new
                                        {
                                            LoanTypeID = a.LoanTypeID,
                                            LoanTypeName = a.LoanTypeName,

                                        }).ToList();

                return new { AllLoanTypes = lsUnMappedLoanTypes, AssignedLoanTypes = lsMappedLoanTypes };
            }
        }

        public object SetReviewLoanMapping(Int64 ReviewTypeID, Int64[] LoanTypeIDs)
        {
            bool result = false;

            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    foreach (var _loanTypeID in LoanTypeIDs)
                    {
                        db.CustReviewLoanMapping.RemoveRange(db.CustReviewLoanMapping.Where(r => r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == _loanTypeID));
                        db.SaveChanges();

                        db.CustReviewLoanCheckMapping.RemoveRange(db.CustReviewLoanCheckMapping.Where(r => r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == _loanTypeID));
                        db.SaveChanges();

                        db.CustReviewLoanStackMapping.RemoveRange(db.CustReviewLoanStackMapping.Where(r => r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == _loanTypeID));
                        db.SaveChanges();

                        CustReviewLoanCheckMapping _loanCheck = db.CustReviewLoanCheckMapping.AsNoTracking().Where(r => r.CustomerID == 1 && r.ReviewTypeID == 0 && r.LoanTypeID == _loanTypeID).FirstOrDefault();
                        CustReviewLoanStackMapping _loanStack = db.CustReviewLoanStackMapping.AsNoTracking().Where(r => r.CustomerID == 1 && r.ReviewTypeID == 0 && r.LoanTypeID == _loanTypeID).FirstOrDefault();

                        if (_loanCheck != null && _loanStack != null)
                        {
                            db.CustReviewLoanMapping.Add(new CustReviewLoanMapping()
                            {
                                CustomerID = 1, //1 - Default Customer
                                ReviewTypeID = ReviewTypeID,
                                LoanTypeID = _loanTypeID,
                                Active = true,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now
                            });
                            db.SaveChanges();

                            db.CustReviewLoanCheckMapping.Add(new CustReviewLoanCheckMapping()
                            {
                                CustomerID = 1, //1 - Default Customer
                                ReviewTypeID = ReviewTypeID,
                                LoanTypeID = _loanTypeID,
                                CheckListID = _loanCheck.CheckListID,
                                Active = true,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now
                            });
                            db.SaveChanges();

                            db.CustReviewLoanStackMapping.Add(new CustReviewLoanStackMapping()
                            {
                                CustomerID = 1, //1 - Default Customer
                                ReviewTypeID = ReviewTypeID,
                                LoanTypeID = _loanTypeID,
                                StackingOrderID = _loanStack.StackingOrderID,
                                Active = true,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now
                            });
                            db.SaveChanges();
                        }
                    }
                    result = true;
                    tran.Commit();
                }
            }

            return result;
        }

        public object RemoveReviewLoanMapping(Int64 ReviewTypeID, Int64[] LoanTypeIDs)
        {
            bool result = false;

            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    foreach (var _loanTypeID in LoanTypeIDs)
                    {
                        CustReviewLoanMapping _CustReviewLoanMapping = db.CustReviewLoanMapping.AsNoTracking().Where(r => r.CustomerID == 1 && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == _loanTypeID).FirstOrDefault();
                        CustReviewLoanCheckMapping _loanCheck = db.CustReviewLoanCheckMapping.AsNoTracking().Where(r => r.CustomerID == 1 && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == _loanTypeID).FirstOrDefault();
                        CustReviewLoanStackMapping _loanStack = db.CustReviewLoanStackMapping.AsNoTracking().Where(r => r.CustomerID == 1 && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == _loanTypeID).FirstOrDefault();

                        if (_CustReviewLoanMapping != null)
                        {
                            _CustReviewLoanMapping.Active = false;
                            _CustReviewLoanMapping.ModifiedOn = DateTime.Now;
                            db.Entry(_CustReviewLoanMapping).State = EntityState.Modified;
                            db.SaveChanges();
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
                    }
                    result = true;
                    tran.Commit();
                }
            }

            return result;
        }

        public object GetSystemStackCheck(Int64 ReviewTypeID, Int64 LoanTypeID, bool IsAdd)
        {
            List<CheckListMaster> lsCheckListMaster = GetSystemCheckListMaster();
            List<StackingOrderMaster> lsStackingOrderMaster = GetSystemStackingOrderMaster();
            if (!IsAdd)
            {
                Int64 checkListID = 0;
                Int64 stackingOrderID = 0;

                using (var db = new DBConnect(SystemSchema))
                {
                    CustReviewLoanCheckMapping custReviewLoanCheckMap = db.CustReviewLoanCheckMapping.AsNoTracking().Where(c => c.ReviewTypeID == ReviewTypeID && c.LoanTypeID == LoanTypeID).FirstOrDefault();
                    CustReviewLoanStackMapping custReviewLoanStackMap = db.CustReviewLoanStackMapping.AsNoTracking().Where(c => c.ReviewTypeID == ReviewTypeID && c.LoanTypeID == LoanTypeID).FirstOrDefault();

                    if (custReviewLoanCheckMap != null)
                        checkListID = custReviewLoanCheckMap.CheckListID;

                    if (custReviewLoanStackMap != null)
                        stackingOrderID = custReviewLoanStackMap.StackingOrderID;
                }

                return new { CheckList = lsCheckListMaster, StackingOrder = lsStackingOrderMaster, MappedCheckListID = checkListID, MappedStackingOrderID = stackingOrderID };
            }

            return new { CheckList = lsCheckListMaster, StackingOrder = lsStackingOrderMaster };
        }

        public bool SetLoanDocTypeMapping(Int64 LoanTypeID, List<DocMappingDetails> DocMappingDetails)
        {
            bool result = false;

            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    List<CustLoanDocMapping> lsCrL = db.CustLoanDocMapping.AsNoTracking().Where(crl => crl.LoanTypeID == LoanTypeID).ToList();

                    foreach (CustLoanDocMapping crl in lsCrL)
                    {
                        db.Entry(crl).State = EntityState.Deleted;
                        db.SaveChanges();
                    }

                    foreach (var _docMappingDetails in DocMappingDetails)
                    {
                        db.CustLoanDocMapping.Add(new CustLoanDocMapping()
                        {
                            CustomerID = 1, //Default for IL Schema                            
                            LoanTypeID = LoanTypeID,
                            DocumentTypeID = _docMappingDetails.DocumentTypeID,
                            DocumentLevel = _docMappingDetails.DocumentLevel,
                            Condition = _docMappingDetails.Condition,
                            Active = true,
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

        public object SaveMapping(Int64 ReviewTypeID, Int64 LoanTypeID, Int64 CheckListID, Int64 StackingOrderID, bool IsAdd)
        {
            bool result = false;

            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    if (IsAdd)
                    {
                        CustReviewLoanMapping custReviewLoanMap = db.CustReviewLoanMapping.AsNoTracking().Where(c => c.ReviewTypeID == ReviewTypeID && c.LoanTypeID == LoanTypeID).FirstOrDefault();

                        if (custReviewLoanMap != null)
                            return new { Success = result, Message = "Mapping Already Exists" };
                        else
                        {
                            db.CustReviewLoanMapping.Add(new CustReviewLoanMapping()
                            {
                                CustomerID = 1, //Default for System Schema
                                ReviewTypeID = ReviewTypeID,
                                LoanTypeID = LoanTypeID,
                                Active = true,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now
                            });
                        }
                    }
                    else
                    {
                        CustReviewLoanCheckMapping custReviewLoanCheckMap = db.CustReviewLoanCheckMapping.AsNoTracking().Where(c => c.ReviewTypeID == ReviewTypeID && c.LoanTypeID == LoanTypeID).FirstOrDefault();
                        CustReviewLoanStackMapping custReviewLoanStackMap = db.CustReviewLoanStackMapping.AsNoTracking().Where(c => c.ReviewTypeID == ReviewTypeID && c.LoanTypeID == LoanTypeID).FirstOrDefault();

                        db.Entry(custReviewLoanCheckMap).State = EntityState.Deleted;
                        db.SaveChanges();

                        db.Entry(custReviewLoanStackMap).State = EntityState.Deleted;
                        db.SaveChanges();
                    }

                    List<DocumentTypeMaster> UnMappedDocTypes = CheckForDocMapping(db, LoanTypeID, CheckListID);

                    if (UnMappedDocTypes != null && UnMappedDocTypes.Count > 0)
                    {
                        foreach (DocumentTypeMaster item in UnMappedDocTypes)
                        {
                            db.CustLoanDocMapping.Add(new CustLoanDocMapping()
                            {
                                CustomerID = 1, //Default for System Schema                                
                                LoanTypeID = LoanTypeID,
                                DocumentTypeID = item.DocumentTypeID,
                                Active = true,
                                DocumentLevel = item.DocumentLevel,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now
                            });
                        }
                    }

                    db.CustReviewLoanCheckMapping.Add(new CustReviewLoanCheckMapping()
                    {
                        CustomerID = 1, //Default for System Schema
                        ReviewTypeID = ReviewTypeID,
                        LoanTypeID = LoanTypeID,
                        CheckListID = CheckListID,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });

                    db.CustReviewLoanStackMapping.Add(new CustReviewLoanStackMapping()
                    {
                        CustomerID = 1, //Default for System Schema
                        ReviewTypeID = ReviewTypeID,
                        LoanTypeID = LoanTypeID,
                        StackingOrderID = StackingOrderID,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });

                    db.SaveChanges();
                    tran.Commit();
                    result = true;
                }
            }

            return new { Success = result, Message = string.Empty };
        }

        public object SetLoanCheckMapping(Int64 LoanTypeID, Int64 CheckListID, List<ChecklistItemSequence> ChecklistItemSeq)
        {
            bool result = false;
            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    db.CustReviewLoanCheckMapping.RemoveRange(db.CustReviewLoanCheckMapping.Where(c => c.CustomerID == 1 && c.ReviewTypeID == 0 && c.LoanTypeID == LoanTypeID));
                    db.SaveChanges();

                    db.CustReviewLoanCheckMapping.Add(new CustReviewLoanCheckMapping()
                    {
                        CustomerID = 1, //1 - Default Customer
                        ReviewTypeID = 0, //0 - When Review and Loan Not Mapped
                        LoanTypeID = LoanTypeID,
                        CheckListID = CheckListID,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        Active = true
                    });
                    db.SaveChanges();

                    List<CheckListDetailMaster> _checkListItems = db.CheckListDetailMaster.AsNoTracking().Where(c => c.CheckListID == CheckListID).ToList();

                    foreach (CheckListDetailMaster item in _checkListItems)
                    {
                        ChecklistItemSequence _seqItem = ChecklistItemSeq.Where(s => s.CheckListDetailID == item.CheckListDetailID).FirstOrDefault();

                        if (_seqItem != null)
                        {
                            item.SequenceID = _seqItem.SequenceID;
                            item.ModifiedOn = DateTime.Now;

                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    tran.Commit();
                    result = true;
                }
            }
            return result;
        }

        public object SetLoanStackMapping(Int64 LoanTypeID, Int64 StackingOrderID)
        {
            bool result = false;
            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    db.CustReviewLoanStackMapping.RemoveRange(db.CustReviewLoanStackMapping.Where(c => c.CustomerID == 1 && c.ReviewTypeID == 0 && c.LoanTypeID == LoanTypeID));
                    db.SaveChanges();

                    //SetCustLoanDocumentStacking(db, LoanTypeID, StackingOrderID);

                    List<DocumentTypeMaster> UnMappedDocTypes = CheckForLoanDocMapping(db, LoanTypeID, StackingOrderID);

                    if (UnMappedDocTypes != null && UnMappedDocTypes.Count > 0)
                    {
                        foreach (DocumentTypeMaster item in UnMappedDocTypes)
                        {
                            db.CustLoanDocMapping.Add(new CustLoanDocMapping()
                            {
                                CustomerID = 1, //Default for System Schema                                
                                LoanTypeID = LoanTypeID,
                                DocumentTypeID = item.DocumentTypeID,
                                Active = true,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                DocumentLevel = item.DocumentLevel
                            });
                        }
                    }

                    db.CustReviewLoanStackMapping.Add(new CustReviewLoanStackMapping()
                    {
                        CustomerID = 1, //1 - Default Customer
                        ReviewTypeID = 0, //0 - When Review and Loan Not Mapped
                        LoanTypeID = LoanTypeID,
                        StackingOrderID = StackingOrderID,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        Active = true
                    });
                    db.SaveChanges();
                    tran.Commit();
                    result = true;
                }
            }
            return result;
        }

        public List<CustReviewLoanMapping> GetSystemReviewLoanTypes(Int64 ReviewTypeID)
        {
            List<CustReviewLoanMapping> _lsCustReviewLoanMapping = new List<CustReviewLoanMapping>(); ;

            using (var db = new DBConnect(SystemSchema))
            {
                _lsCustReviewLoanMapping = db.CustReviewLoanMapping.AsNoTracking().Where(r => r.CustomerID == 1 && r.ReviewTypeID == ReviewTypeID).ToList();
            }

            return _lsCustReviewLoanMapping;
        }

        public CheckListMaster GetSystemLoanCheckList(Int64 LoanTypeID)
        {
            CheckListMaster _checkListMaster = null;

            using (var db = new DBConnect(SystemSchema))
            {
                CustReviewLoanCheckMapping _custReviewLoanCheckMapping = db.CustReviewLoanCheckMapping.AsNoTracking().Where(r => r.CustomerID == 1 && r.ReviewTypeID == 0 && r.LoanTypeID == LoanTypeID).FirstOrDefault();

                if (_custReviewLoanCheckMapping != null)
                {
                    _checkListMaster = db.CheckListMaster.AsNoTracking().Where(c => c.CheckListID == _custReviewLoanCheckMapping.CheckListID).FirstOrDefault();

                    if (_checkListMaster != null)
                    {

                        _checkListMaster.CheckListDetailMasters = GetSystemChecklistDetails(db, _checkListMaster.CheckListID);

                        if (_checkListMaster.CheckListDetailMasters != null)
                        {
                            foreach (CheckListDetailMaster item in _checkListMaster.CheckListDetailMasters)
                            {
                                item.RuleMasters = GetSystemCheckRuleMaster(db, item.CheckListDetailID);
                            }
                        }
                    }
                }
            }

            return _checkListMaster;
        }

        public List<CheckListDetailMaster> GetSystemChecklistDetails(DBConnect db, Int64 CheckListID)
        {
            return db.CheckListDetailMaster.AsNoTracking().Where(r => r.CheckListID == CheckListID).ToList();
        }

        public List<StackingOrderDetailMaster> GetSystemStackingOrderDetails(DBConnect db, Int64 StackingOrderID)
        {
            return db.StackingOrderDetailMaster.AsNoTracking().Where(r => r.StackingOrderID == StackingOrderID).ToList();
        }

        public RuleMaster GetSystemCheckRuleMaster(DBConnect db, Int64 ChecklistDetailID)
        {
            return db.RuleMaster.AsNoTracking().Where(r => r.CheckListDetailID == ChecklistDetailID).FirstOrDefault();
        }

        public StackingOrderMaster GetSystemLoanStackingOrder(Int64 LoanTypeID)
        {
            StackingOrderMaster _stackingOrderMaster = null;

            using (var db = new DBConnect(SystemSchema))
            {
                CustReviewLoanStackMapping _StackMapping = db.CustReviewLoanStackMapping.AsNoTracking().Where(r => r.CustomerID == 1 && r.ReviewTypeID == 0 && r.LoanTypeID == LoanTypeID).FirstOrDefault();

                if (_StackMapping != null)
                {
                    _stackingOrderMaster = db.StackingOrderMaster.AsNoTracking().Where(c => c.StackingOrderID == _StackMapping.StackingOrderID).FirstOrDefault();

                    if (_stackingOrderMaster != null)
                    {
                        _stackingOrderMaster.StackingOrderDetailMasters = GetSystemStackingOrderDetails(db, _stackingOrderMaster.StackingOrderID);
                        _stackingOrderMaster.StackingOrderGroupmasters = db.StackingOrderGroupmasters.AsNoTracking().Where(s => s.StackingOrderID == _stackingOrderMaster.StackingOrderID).FirstOrDefault();
                    }
                }
            }

            return _stackingOrderMaster;
        }
        public StackingOrderMaster GetStackingOrder(string TenantSchema, Int64 StackingOrderID)
        {
            StackingOrderMaster _stackingOrderMaster = null;

            using (var db = new DBConnect(TenantSchema))
            {
                _stackingOrderMaster = db.StackingOrderMaster.AsNoTracking().Where(c => c.StackingOrderID == StackingOrderID).FirstOrDefault();

                if (_stackingOrderMaster != null)
                {
                    _stackingOrderMaster.StackingOrderDetailMasters = GetSystemStackingOrderDetails(db, _stackingOrderMaster.StackingOrderID);
                }
            }

            return _stackingOrderMaster;
        }


        #endregion

        #region Re-Verification
        public bool SetDocLoanTypeMapping(Int64 DocumentTypeID, Int64[] LoanTypeIDs)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    foreach (Int64 _loanTypeID in LoanTypeIDs)
                    {
                        List<CustLoanDocMapping> mapping = db.CustLoanDocMapping.AsNoTracking().Where(l => l.LoanTypeID == _loanTypeID && l.DocumentTypeID == DocumentTypeID).ToList();
                        if (mapping.Count > 0)
                        {
                            foreach (var mapp in mapping)
                            {
                                db.Entry(mapp).State = EntityState.Deleted;
                                db.SaveChanges();

                                db.CustLoanDocMapping.Add(new CustLoanDocMapping() { CustomerID = 1, DocumentTypeID = DocumentTypeID, LoanTypeID = _loanTypeID, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Active = true });
                            }

                            //db.CustLoanDocMapping.Add(new CustLoanDocMapping() { CustomerID = 1, DocumentTypeID = DocumentTypeID, LoanTypeID = _loanTypeID, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Active = true });
                        }

                        CustReviewLoanStackMapping _stack = db.CustReviewLoanStackMapping.AsNoTracking().Where(c => c.CustomerID == 1 && c.ReviewTypeID == 0 && c.LoanTypeID == _loanTypeID).FirstOrDefault();

                        if (_stack != null)
                        {
                            Int32 _totalStackCount = db.StackingOrderDetailMaster.AsNoTracking().Where(s => s.StackingOrderID == _stack.StackingOrderID).Count();
                            List<StackingOrderDetailMaster> _stackDetails = db.StackingOrderDetailMaster.AsNoTracking().Where(s => s.StackingOrderID == _stack.StackingOrderID && s.DocumentTypeID == DocumentTypeID).ToList();
                            if (_stackDetails.Count > 0)
                            {
                                foreach (var _stackDetail in _stackDetails)
                                {
                                    db.Entry(_stackDetail).State = EntityState.Deleted;
                                    db.SaveChanges();
                                }

                                //}
                                db.StackingOrderDetailMaster.Add(new StackingOrderDetailMaster()
                                {
                                    StackingOrderID = _stack.StackingOrderID,
                                    DocumentTypeID = DocumentTypeID,
                                    Active = true,
                                    SequenceID = _totalStackCount + 1,
                                    CreatedOn = DateTime.Now,
                                    ModifiedOn = DateTime.Now
                                });
                            }
                        }
                    }

                    db.SaveChanges();
                    tran.Commit();
                    return true;
                }
            }
            return false;
        }

        public List<object> GetReverificationMaster()
        {

            List<object> ReverificationMaster = new List<object>();
            using (var db = new DBConnect(SystemSchema))
            {
                List<CustReviewLoanReverifyMapping> _mapping = db.CustReviewLoanReverifyMapping.AsNoTracking().Where(m => m.CustomerID == 1 && m.ReviewTypeID == 0).ToList();

                foreach (CustReviewLoanReverifyMapping item in _mapping)
                {
                    LoanTypeMaster _loanType = db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeID == item.LoanTypeID).FirstOrDefault();

                    SystemReverificationMasters _reverify = db.SystemReverificationMasters.AsNoTracking().Where(r => r.ReverificationID == item.ReverificationID).FirstOrDefault();

                    ReverificationMaster.Add(new
                    {
                        LoanTypeID = _loanType.LoanTypeID,
                        ReverificationID = _reverify.ReverificationID,
                        LoanTypeName = _loanType.LoanTypeName,
                        ReverificationName = _reverify.ReverificationName,
                        Active = _reverify.Active,
                        MappingID = item.ID,
                        TemplateID = item.TemplateID,
                        TemplateFields = item.TemplateFields,
                        // LogoGuid = _reverify.LogoGuid,
                        //LogoFileName =string.IsNullOrEmpty(_reverify.FileName) ? _reverify.FileName :""
                    });
                }
            }
            return ReverificationMaster;
        }

        public object UpdateReverification(Int64 LoanTypeID, string ReverificationName, Int64 TemplateID, Int64 MappingID, Int64 ReverificationID, bool Active)
        {
            object result = new { ReverificationID = ReverificationID, Success = false, MappingID = MappingID };

            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    SystemReverificationMasters _reverify = db.SystemReverificationMasters.AsNoTracking().Where(r => r.ReverificationID == ReverificationID).FirstOrDefault();

                    if (_reverify != null)
                    {
                        _reverify.ReverificationName = ReverificationName;
                        _reverify.Active = Active;
                        _reverify.ModifiedOn = DateTime.Now;
                        db.Entry(_reverify).State = EntityState.Modified;
                        db.SaveChanges();

                        tran.Commit();
                        result = new { ReverificationID = _reverify.ReverificationID, Success = true, MappingID = MappingID };
                    }
                    else
                    {
                        result = new { ReverificationID = ReverificationID, Success = false, MappingID = MappingID };
                    }
                }
            }

            return result;
        }

        public byte[] GetReverificationLogo(string Guid)
        {
            return new ImageMinIOWrapper(TenantSchema).GetObject("Reverification", Guid);
        }

        public object ReverificationFileUploader(Int64 ReverificationID, byte[] fileStream, Guid? LogoGUID, string FileName)
        {
            SystemReverificationMasters _reverify = new SystemReverificationMasters();
            using (var db = new DBConnect(SystemSchema))
            {

                _reverify = db.SystemReverificationMasters.AsNoTracking().Where(r => r.ReverificationID == ReverificationID).FirstOrDefault();
                //string imageGuid = Convert.ToString(_reverify.LogoGuid);
                ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);
                if (_reverify.LogoGuid != null)
                {
                    _imageWrapper.CheckAndDeleteReverificationFile("reverification", _reverify.LogoGuid);
                }
                _imageWrapper.InsertReverificationImage(ReverificationID, fileStream, LogoGUID);
                _reverify.LogoGuid = LogoGUID;
                _reverify.FileName = FileName;
                _reverify.ModifiedOn = DateTime.Now;
                db.Entry(_reverify).State = EntityState.Modified;
                db.SaveChanges();
                return new { logoGuid = LogoGUID };
            }
        }

        public object CustReverificationFileUploader(Int64 ReverificationID, byte[] fileStream, Guid? LogoGUID, string FileName)
        {
            ReverificationMaster _reverify = new ReverificationMaster();
            using (var db = new DBConnect(TenantSchema))
            {

                _reverify = db.ReverificationMaster.AsNoTracking().Where(r => r.ReverificationID == ReverificationID).FirstOrDefault();
                //string imageGuid = Convert.ToString(_reverify.LogoGuid);
                ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);
                if (_reverify.LogoGuid != null)
                {
                    _imageWrapper.CheckAndDeleteReverificationFile("reverification", _reverify.LogoGuid);
                }
                _imageWrapper.InsertReverificationImage(ReverificationID, fileStream, LogoGUID);
                _reverify.LogoGuid = LogoGUID;
                _reverify.FileName = FileName;
                _reverify.ModifiedOn = DateTime.Now;
                db.Entry(_reverify).State = EntityState.Modified;
                db.SaveChanges();
                return new { logoGuid = LogoGUID };
            }
        }
        //public object SaveReverification(Int64 LoanTypeID, string ReverificationName, Int64 TemplateID,Guid LogoGUID, byte[] imagebyte, Int64 ReverificationID, Int64  MappingID , bool Active,string FileName)
        //{
        //    object result;

        //    using (var db = new DBConnect(SystemSchema))
        //    {
        //        using (var tran = db.Database.BeginTransaction())
        //        {
        //            var bucketName = "Reverification";
        //            var ObjectName = LogoGUID.ToString();
        //            var contentType = "application/image";
        //            MemoryStream imageStream = new MemoryStream(imagebyte);
        //            ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);
        //            if (ReverificationID != 0)
        //            {
        //                SystemReverificationMasters _reverify = db.SystemReverificationMasters.AsNoTracking().Where(r => r.ReverificationID == ReverificationID).FirstOrDefault();

        //                if (_reverify != null)
        //                {
        //                    _reverify.ReverificationName = ReverificationName;
        //                    _reverify.Active = Active;
        //                    _reverify.ModifiedOn = DateTime.Now;
        //                    _reverify.LogoGuid = LogoGUID;
        //                    _reverify.FileName = FileName;
        //                    db.Entry(_reverify).State = EntityState.Modified;
        //                    db.SaveChanges();

        //                    result = new { ReverificationID = _reverify.ReverificationID, Success = true, MappingID = MappingID ,FileName = _reverify.FileName };
        //                }
        //                else
        //                {
        //                    result = new { ReverificationID = ReverificationID, Success = false, MappingID = MappingID, FileName = FileName };
        //                }

        //            }
        //            else
        //            {
        //                result = new { ReverificationID = 0, Success = false, MappingID = 0, FileName ="" };
        //                SystemReverificationMasters _reverify = new SystemReverificationMasters()
        //                {
        //                    ReverificationName = ReverificationName,
        //                    Active = true,
        //                    CreatedOn = DateTime.Now,
        //                    ModifiedOn = DateTime.Now,
        //                    LogoGuid = LogoGUID,
        //                    FileName = FileName
        //                };
        //                db.SystemReverificationMasters.Add(_reverify);
        //                db.SaveChanges();

        //                CustReviewLoanReverifyMapping _mapping = new CustReviewLoanReverifyMapping()
        //                {
        //                    CustomerID = 1,
        //                    ReviewTypeID = 0,
        //                    LoanTypeID = LoanTypeID,
        //                    ReverificationID = _reverify.ReverificationID,
        //                    TemplateID = TemplateID,
        //                    TemplateFields = string.Empty,
        //                    Active = true,
        //                    CreatedOn = DateTime.Now,
        //                    ModifiedOn = DateTime.Now
        //                };
        //                db.CustReviewLoanReverifyMapping.Add(_mapping);
        //                db.SaveChanges();
        //                result = new { ReverificationID = _reverify.ReverificationID, Success = true, MappingID = _mapping.ID, FileName =_reverify.FileName };

        //            }
        //            tran.Commit();

        //            _imageWrapper.UploadFile(bucketName, ObjectName, imageStream, contentType);
        //        }
        //    }

        //    return result;
        //}

        public object SaveReverification(Int64 LoanTypeID, string ReverificationName, Int64 TemplateID)
        {
            object result = new { ReverificationID = 0, Success = false, MappingID = 0 };

            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    SystemReverificationMasters _reverify = new SystemReverificationMasters()
                    {
                        ReverificationName = ReverificationName,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };
                    db.SystemReverificationMasters.Add(_reverify);
                    db.SaveChanges();

                    CustReviewLoanReverifyMapping _mapping = new CustReviewLoanReverifyMapping()
                    {
                        CustomerID = 1,
                        ReviewTypeID = 0,
                        LoanTypeID = LoanTypeID,
                        ReverificationID = _reverify.ReverificationID,
                        TemplateID = TemplateID,
                        TemplateFields = string.Empty,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };
                    db.CustReviewLoanReverifyMapping.Add(_mapping);
                    db.SaveChanges();

                    tran.Commit();
                    result = new { ReverificationID = _reverify.ReverificationID, Success = true, MappingID = _mapping.ID };
                }
            }

            return result;
        }

        public bool UpdateMappingTemplateFields(Int64 MappingID, string TemplateFieldJson)
        {
            bool result = false;

            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    CustReviewLoanReverifyMapping _map = db.CustReviewLoanReverifyMapping.AsNoTracking().Where(t => t.ID == MappingID).FirstOrDefault();

                    if (_map != null)
                    {
                        _map.TemplateFields = TemplateFieldJson;
                        _map.ModifiedOn = DateTime.Now;
                        db.Entry(_map).State = EntityState.Modified;
                        db.SaveChanges();
                        tran.Commit();
                        result = true;
                    }
                }
            }

            return result;
        }



        public bool UpdateCustMappingTemplateFields(Int64 MappingID, string TemplateFieldJson)
        {
            bool result = false;

            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    CustReviewLoanReverifyMapping _map = db.CustReviewLoanReverifyMapping.AsNoTracking().Where(t => t.ID == MappingID).FirstOrDefault();

                    if (_map != null)
                    {
                        _map.TemplateFields = TemplateFieldJson;
                        _map.ModifiedOn = DateTime.Now;
                        db.Entry(_map).State = EntityState.Modified;
                        db.SaveChanges();
                        tran.Commit();
                        result = true;
                    }
                }
            }

            return result;
        }

        public object GetReverificationMappedDoc(Int64 LoanTypeID, Int64 ReverificationID)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                List<DocumentTypeMaster> lsDocumentTypeMaster = db.DocumentTypeMaster.AsNoTracking().ToList();

                List<DocumentFieldMaster> lsDocumentFields = db.DocumentFieldMaster.AsNoTracking().ToList();

                List<CustReverificationDocMapping> _reverifyMappedDocTypes = db.CustReverificationDocMapping.AsNoTracking().Where(c => c.CustomerID == 1 && c.ReverificationID == ReverificationID).ToList();

                List<CustLoanDocMapping> _loanMappedDocTypes = db.CustLoanDocMapping.AsNoTracking().Where(c => c.LoanTypeID == LoanTypeID).ToList();

                var lsUnMappedDocTypes = (from map in _loanMappedDocTypes
                                          join lt in lsDocumentTypeMaster on map.DocumentTypeID equals lt.DocumentTypeID
                                          select new
                                          {
                                              DocumentTypeID = map.DocumentTypeID,
                                              Name = lt.Name
                                          }).ToList();

                lsUnMappedDocTypes = lsUnMappedDocTypes.Where(u => !_reverifyMappedDocTypes.Any(r => r.DocumentTypeID == u.DocumentTypeID)).ToList();

                var lsMappedDocTypes = (from map in _reverifyMappedDocTypes
                                        join lt in lsDocumentTypeMaster on map.DocumentTypeID equals lt.DocumentTypeID
                                        select new
                                        {
                                            DocumentTypeID = map.DocumentTypeID,
                                            Name = lt.Name
                                        }).ToList();

                if (lsMappedDocTypes.Count > 0)
                {

                    var lsMappedDocFields = (from map in lsMappedDocTypes
                                             join df in lsDocumentFields on map.DocumentTypeID equals df.DocumentTypeID
                                             select new
                                             {
                                                 DocID = map.Name,
                                                 DocFieldID = df.FieldID,
                                                 DocFieldName = df.Name
                                             }
                                             ).ToList();
                    return new { AllDocTypes = lsUnMappedDocTypes, AssignedDocTypes = lsMappedDocTypes, AssignedDocFields = lsMappedDocFields };
                }
                else
                {

                    var lsMappedDocFields = (from map in lsUnMappedDocTypes
                                             join df in lsDocumentFields on map.DocumentTypeID equals df.DocumentTypeID
                                             select new
                                             {
                                                 DocID = map.Name,
                                                 DocFieldID = df.FieldID,
                                                 DocFieldName = df.Name
                                             }
                                             ).ToList();
                    return new { AllDocTypes = lsUnMappedDocTypes, AssignedDocTypes = lsMappedDocTypes, AssignedDocFields = lsMappedDocFields };
                }

            }
        }

        public bool SetReverifyDocMapping(Int64 ReverificationID, Int64[] DocTypeIDs)
        {
            bool result = false;

            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    List<CustReverificationDocMapping> lsCrL = db.CustReverificationDocMapping.AsNoTracking().Where(crl => crl.ReverificationID == ReverificationID).ToList();

                    foreach (CustReverificationDocMapping crl in lsCrL)
                    {
                        db.Entry(crl).State = EntityState.Deleted;
                        db.SaveChanges();
                    }

                    foreach (Int64 DocTypeID in DocTypeIDs)
                    {
                        db.CustReverificationDocMapping.Add(new CustReverificationDocMapping()
                        {
                            CustomerID = 1, //Default for IL Schema                            
                            ReverificationID = ReverificationID,
                            DocumentTypeID = DocTypeID,
                            Active = true,
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

        public object GetMappedTemplate(Int64 TemplateID, Int64 MappingID, Int64 ReverificationID)
        {
            object _template = new { TemplateID = TemplateID, TemplateName = string.Empty, TemplateFileName = string.Empty, TemplateFields = string.Empty, TemplateFieldValue = string.Empty };

            using (var db = new DBConnect(SystemSchema))
            {
                // string bs64Image = "";
                // object _img = null;
                SystemReverificationTemplate _sysTemplate = db.SystemReverificationTemplate.AsNoTracking().Where(t => t.TemplateID == TemplateID).FirstOrDefault();
                if (_sysTemplate != null)
                {

                    CustReviewLoanReverifyMapping _map = db.CustReviewLoanReverifyMapping.AsNoTracking().Where(t => t.ID == MappingID).FirstOrDefault();
                    SystemReverificationMasters _systemreverification = db.SystemReverificationMasters.AsNoTracking().Where(x => x.ReverificationID == ReverificationID).FirstOrDefault();
                    //  if(_systemreverification !=null)
                    //{
                    // _img = _systemreverification.LogoGuid;
                    //var bucketName = "Reverification";
                    //ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(SystemSchema);
                    //byte[] _image = _imageWrapper.GetObject(bucketName, _systemreverification.LogoGuid.ToString());
                    // bs64Image = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(_image));

                    //if (!string.IsNullOrEmpty(bs64Image))
                    //    _img = new { Image = bs64Image };
                    //    }
                    if (_map != null)
                    {
                        _template = new { TemplateID = _sysTemplate.TemplateID, TemplateName = _sysTemplate.TemplateName, TemplateFileName = _sysTemplate.TemplateFileName, TemplateFields = _sysTemplate.TemplateFields, TemplateFieldValue = _map.TemplateFields == null ? string.Empty : _map.TemplateFields, LogoGuid = _systemreverification.LogoGuid };//,  Image = _img
                    }
                }
            }
            return _template;
        }

        public List<SystemReverificationTemplate> GetReverificationTemplate()
        {
            List<SystemReverificationTemplate> _template = new List<SystemReverificationTemplate>();
            using (var db = new DBConnect(SystemSchema))
            {
                _template = db.SystemReverificationTemplate.AsNoTracking().Where(r => r.Active).ToList();
            }

            return _template;
        }
        public List<SystemReverificationMasters> CheckReverificationExistForEdit(string ReverificationName)
        {
            List<SystemReverificationMasters> _doc = new List<SystemReverificationMasters>();
            using (var db = new DBConnect(SystemSchema))
            {
                _doc = db.SystemReverificationMasters.AsNoTracking().Where(d => d.ReverificationName == ReverificationName).ToList();

            }
            return _doc;

        }
        public List<SystemReverificationMasters> CheckManagerReverificationExistForEdit(string ReverificationName)
        {
            List<SystemReverificationMasters> _doc = new List<SystemReverificationMasters>();
            using (var db = new DBConnect(TenantSchema))
            {
                _doc = db.SystemReverificationMasters.AsNoTracking().Where(d => d.ReverificationName == ReverificationName).ToList();

            }
            return _doc;

        }
        public List<string> GetDocFieldsByName(string DocumentTypeName)
        {
            List<string> fields = new List<string>();

            using (var db = new DBConnect(SystemSchema))
            {
                DocumentTypeMaster _sysDoc = db.DocumentTypeMaster.AsNoTracking().Where(t => t.Name == DocumentTypeName).FirstOrDefault();
                if (_sysDoc != null)
                {
                    List<DocumentFieldMaster> _lsFields = db.DocumentFieldMaster.AsNoTracking().Where(t => t.DocumentTypeID == _sysDoc.DocumentTypeID).ToList();

                    if (_lsFields != null)
                    {
                        foreach (var item in _lsFields)
                        {
                            fields.Add(item.Name);
                        }
                    }
                }
            }
            return fields;
        }

        #region Customer Re-Verification

        public List<object> GetCustReverificationMaster()
        {

            List<object> ReverificationMaster = new List<object>();
            using (var db = new DBConnect(TenantSchema))
            {
                List<CustReviewLoanReverifyMapping> _mapping = db.CustReviewLoanReverifyMapping.AsNoTracking().ToList();

                foreach (CustReviewLoanReverifyMapping item in _mapping)
                {
                    LoanTypeMaster _loanType = db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeID == item.LoanTypeID).FirstOrDefault();
                    CustomerMaster _cust = db.CustomerMaster.AsNoTracking().Where(l => l.CustomerID == item.CustomerID).FirstOrDefault();
                    ReverificationMaster _reverify = db.ReverificationMaster.AsNoTracking().Where(r => r.ReverificationID == item.ReverificationID).FirstOrDefault();
                    if (_cust != null && _loanType != null && _reverify != null)
                    {
                        ReverificationMaster.Add(new
                        {
                            LoanTypeID = _loanType.LoanTypeID,
                            CustomerID = _cust.CustomerID,
                            CustomerName = _cust.CustomerName,
                            ReverificationID = _reverify.ReverificationID,
                            LoanTypeName = _loanType.LoanTypeName,
                            ReverificationName = _reverify.ReverificationName,
                            Active = _reverify.Active,
                            MappingID = item.ID,
                            TemplateID = item.TemplateID,
                            TemplateFields = item.TemplateFields,
                            // FileName =string.IsNullOrEmpty(_reverify.FileName) ? _reverify.FileName : "",
                            // LogoGuid =  _reverify.LogoGuid
                        });
                    }
                }
            }
            return ReverificationMaster;
        }

        public object UpdateCustReverification(Int64 CustomerID, Int64 LoanTypeID, string ReverificationName, Int64 TemplateID, Int64 MappingID, Int64 ReverificationID, bool Active)
        {
            object result = new { ReverificationID = ReverificationID, Success = false, MappingID = MappingID };

            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    ReverificationMaster _reverify = db.ReverificationMaster.AsNoTracking().Where(r => r.ReverificationID == ReverificationID).FirstOrDefault();

                    if (_reverify != null)
                    {
                        _reverify.ReverificationName = ReverificationName;
                        _reverify.Active = Active;
                        _reverify.ModifiedOn = DateTime.Now;
                        db.Entry(_reverify).State = EntityState.Modified;
                        db.SaveChanges();

                        tran.Commit();
                        result = new { ReverificationID = _reverify.ReverificationID, Success = true, MappingID = MappingID };
                    }
                    else
                    {
                        result = new { ReverificationID = ReverificationID, Success = false, MappingID = MappingID };
                    }
                }
            }

            return result;
        }

        public object GetCustReverificationMappedDoc(Int64 CustomerID, Int64 LoanTypeID, Int64 ReverificationID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                //List<DocumentTypeMaster> lsDocumentTypeMaster = db.DocumentTypeMaster.AsNoTracking().ToList();

                List<CustReverificationDocMapping> _reverifyMappedDocTypes = db.CustReverificationDocMapping.AsNoTracking().Where(c => c.CustomerID == CustomerID && c.ReverificationID == ReverificationID).ToList();

                List<CustLoanDocMapping> _loanMappedDocTypes = db.CustLoanDocMapping.AsNoTracking().Where(c => c.CustomerID == CustomerID && c.LoanTypeID == LoanTypeID).ToList();
                List<DocumentFieldMaster> lsDocumentFields = db.DocumentFieldMaster.AsNoTracking().ToList();
                var lsUnMappedDocTypes = (from map in _loanMappedDocTypes
                                          select new
                                          {
                                              DocumentTypeID = map.DocumentTypeID,
                                              Name = db.DocumentTypeMaster.AsNoTracking().Where(d => d.DocumentTypeID == map.DocumentTypeID).FirstOrDefault().Name
                                          }).ToList();

                lsUnMappedDocTypes = lsUnMappedDocTypes.Where(u => !_reverifyMappedDocTypes.Any(r => r.DocumentTypeID == u.DocumentTypeID)).ToList();

                var lsMappedDocTypes = (from map in _reverifyMappedDocTypes
                                        select new
                                        {
                                            DocumentTypeID = map.DocumentTypeID,
                                            Name = db.DocumentTypeMaster.AsNoTracking().Where(d => d.DocumentTypeID == map.DocumentTypeID).FirstOrDefault().Name
                                        }).ToList();
                if (lsMappedDocTypes.Count > 0)
                {

                    var lsMappedDocFields = (from map in lsMappedDocTypes
                                             join df in lsDocumentFields on map.DocumentTypeID equals df.DocumentTypeID
                                             select new
                                             {
                                                 DocID = map.Name,
                                                 DocFieldID = df.FieldID,
                                                 DocFieldName = df.Name
                                             }
                                             ).ToList();
                    return new { AllDocTypes = lsUnMappedDocTypes, AssignedDocTypes = lsMappedDocTypes, AssignedDocFields = lsMappedDocFields };
                }
                else
                {

                    var lsMappedDocFields = (from map in lsUnMappedDocTypes
                                             join df in lsDocumentFields on map.DocumentTypeID equals df.DocumentTypeID
                                             select new
                                             {
                                                 DocID = map.Name,
                                                 DocFieldID = df.FieldID,
                                                 DocFieldName = df.Name
                                             }
                                             ).ToList();
                    return new { AllDocTypes = lsUnMappedDocTypes, AssignedDocTypes = lsMappedDocTypes, AssignedDocFields = lsMappedDocFields };
                }
            }
        }

        public bool SetCustReverifyDocMapping(Int64 CustomerID, Int64 ReverificationID, Int64[] DocTypeIDs)
        {
            bool result = false;

            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    db.CustReverificationDocMapping.RemoveRange(db.CustReverificationDocMapping.Where(crl => crl.CustomerID == CustomerID && crl.ReverificationID == ReverificationID));
                    db.SaveChanges();

                    foreach (Int64 DocTypeID in DocTypeIDs)
                    {
                        db.CustReverificationDocMapping.Add(new CustReverificationDocMapping()
                        {
                            CustomerID = CustomerID,
                            ReverificationID = ReverificationID,
                            DocumentTypeID = DocTypeID,
                            Active = true,
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

        public object GetCustMappedTemplate(Int64 TemplateID, Int64 MappingID, Int64 ReverificationID)
        {
            object _template = new { TemplateID = TemplateID, TemplateName = string.Empty, TemplateFileName = string.Empty, TemplateFields = string.Empty, TemplateFieldValue = string.Empty };

            using (var db = new DBConnect(TenantSchema))
            {
                // string bs64Image = "";
                object _img = null;
                ReverificationTemplate _sysTemplate = db.ReverificationTemplate.AsNoTracking().Where(t => t.TemplateID == TemplateID).FirstOrDefault();
                if (_sysTemplate != null)
                {
                    ReverificationMaster _reverification = db.ReverificationMaster.AsNoTracking().Where(x => x.ReverificationID == ReverificationID).FirstOrDefault();
                    //ReverificationMaster _systemreverification = db.ReverificationMaster.AsNoTracking().Where(x => x.ReverificationID == ReverificationID).FirstOrDefault();
                    //if (_systemreverification != null)
                    //{
                    //    _img = CommonUtils.EnDecrypt(Convert.ToString(_systemreverification.LogoGuid), false);
                    //var bucketName = "Reverification";
                    //ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(SystemSchema);
                    //byte[] _image = _imageWrapper.GetObject(bucketName, _systemreverification.LogoGuid.ToString());
                    //bs64Image = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(_image));

                    //if (!string.IsNullOrEmpty(bs64Image))
                    //    _img = new { Image = bs64Image };
                    //   }
                    CustReviewLoanReverifyMapping _map = db.CustReviewLoanReverifyMapping.AsNoTracking().Where(t => t.ID == MappingID).FirstOrDefault();

                    if (_map != null)
                    {
                        _template = new { TemplateID = _sysTemplate.TemplateID, TemplateName = _sysTemplate.TemplateName, TemplateFileName = _sysTemplate.TemplateFileName, TemplateFields = _sysTemplate.TemplateFields, TemplateFieldValue = _map.TemplateFields == null ? string.Empty : _map.TemplateFields, LogoGuid = _reverification.LogoGuid }; //, Image = _img
                    }
                }
            }
            return _template;
        }


        #endregion

        #endregion

        #region EncompassToken

        //private List<List<Dictionary<string, string>>> _queryCombinations = new List<List<Dictionary<string, string>>>();


        public object SetMileStoneEvent(string _loanGUID, string _instanceID)
        {
            bool result = false;

            using (var db = new DBConnect(SystemSchema))
            {
                List<TenantMaster> _tenants = db.TenantMaster.AsNoTracking().ToList();

                Guid loanGUIDValue = new Guid(_loanGUID);

                DBConnect tenantDB = null;

                foreach (var tenant in _tenants)
                {
                    tenantDB = new DBConnect(tenant.TenantSchema);

                    bool isTenantFound = FindTenantSchema(tenant, _instanceID);

                    if (isTenantFound)
                    {
                        tenantDB.AuditEWebhookEvents.Add(new AuditEWebhookEvents()
                        {
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now,
                            EventType = EWebHookEventsLogConstant.MILESTONELOG,
                            Response = JsonConvert.SerializeObject(new { loanGUID = loanGUIDValue }),
                            IsTrailing = false,
                            Processed = false
                        });
                        tenantDB.SaveChanges();
                        result = true;
                        break;
                    }
                }


                //if (loanExists)
                //{
                //    RequestAgain:
                //    string[] FieldIDs = _enImportFields.Select(x => x.EncompassFieldID).ToArray();
                //    RestWebClient client = new RestWebClient(ConfigurationManager.AppSettings["EncompassConnectorURL"]);
                //    HttpRequestObject req = new HttpRequestObject() { Content = new { loanGUID = _loanGUID, fieldIDs = FieldIDs }, REQUESTTYPE = "POST", URL = string.Format(EncompassURLILConstant.GET_PREDEFINED_FIELDVALUES) };
                //    client.AddDefaultHeader("Token", tokenObject.accessToken);
                //    client.AddDefaultHeader("TokenType", tokenObject.tokenType);
                //    var result = client.Execute(req);
                //    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                //    {
                //        string res = result.Content;
                //        List<EFieldResponse> _eResponse = JsonConvert.DeserializeObject<List<EFieldResponse>>(res);
                //        IntellaAndEncompassFetchFields _serviceType = _enImportFields.Where(x => x.FieldType.Contains(LOSFieldType.SERVICETYPE)).FirstOrDefault();
                //        EFieldResponse _eServiceType = _eResponse.Where(x => x.FieldId == _serviceType.EncompassFieldID).FirstOrDefault();
                //        if (loanExists && !(_serviceType.EncompassFieldValue.Split(',').Contains(_eServiceType.Value)))
                //        {
                //            tenantDB.EWebhookEvents.Add(new EWebhookEvents()
                //            {
                //                CreatedOn = DateTime.Now,
                //                EventType = EWebHookEventsLogConstant.MILESTONELOG,
                //                Status = EWebHookStatusConstant.EWEB_HOOK_STAGED,
                //                Response = JsonConvert.SerializeObject(new { loanGUID = loanGUIDValue }),
                //                IsTrailing = false
                //            });
                //            tenantDB.SaveChanges();
                //        }
                //    }
                //    if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                //    {
                //        dynamic newToken = GetToken(tenantDB.EncompassConfig.AsNoTracking().ToList());
                //        if (newToken != null)
                //        {
                //            UpdateNewToken(tenantDB, newToken.TokenType, newToken.Token);
                //            tokenObject = new EToken() { accessToken = newToken.Token, tokenType = newToken.TokenType };
                //        }
                //        goto RequestAgain;
                //    }
                //}
                //tenantDB.Dispose();
            }
            return new { Success = result };
        }


        public object SetDocumentEvent(string _loanGUID, string _instanceID)
        {
            bool result = false;

            using (var db = new DBConnect(SystemSchema))
            {
                List<TenantMaster> _tenants = db.TenantMaster.AsNoTracking().ToList();

                Guid loanGUIDValue = new Guid(_loanGUID);

                DBConnect tenantDB = null;

                foreach (var tenant in _tenants)
                {
                    tenantDB = new DBConnect(tenant.TenantSchema);

                    bool isTenantFound = FindTenantSchema(tenant, _instanceID);

                    if (isTenantFound)
                    {
                        bool loanExists = tenantDB.Loan.AsNoTracking().Any(x => x.EnCompassLoanGUID == loanGUIDValue);

                        tenantDB.AuditEWebhookEvents.Add(new AuditEWebhookEvents()
                        {
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now,
                            EventType = EWebHookEventsLogConstant.DOCUMENT_LOG,
                            Response = JsonConvert.SerializeObject(new { loanGUID = loanGUIDValue }),
                            IsTrailing = loanExists,
                            Processed = false
                        });
                        tenantDB.SaveChanges();
                        result = true;
                        break;
                    }
                }
            }

            return new { Success = result };
        }

        //public EToken GetEncompassToken(string _instanceID)
        //{
        //    using (var db = new DBConnect(SystemSchema))
        //    {
        //        List<TenantMaster> _tenants = db.TenantMaster.AsNoTracking().ToList();

        //        foreach (var tenant in _tenants)
        //        {
        //            EToken tokenObject = GetEncompassTokenFromTenant(tenant, _instanceID);

        //            if (tokenObject != null)
        //                return tokenObject;
        //        }

        //        return null;
        //    }
        //}

        public bool FindTenantSchema(TenantMaster tenant, string _instanceID)
        {
            using (var db = new DBConnect(tenant.TenantSchema))
            {
                EncompassConfig _encompassConfig = db.EncompassConfig.AsNoTracking().Where(x => x.ConfigKey == EncompassConfigConstant.INSTANCE_ID).FirstOrDefault();

                return (_encompassConfig != null && _instanceID.Trim().ToUpper() == _encompassConfig.ConfigValue.Trim().ToUpper());
            }
        }


        //public EToken GetEncompassTokenFromTenant(TenantMaster tenant, string _instanceID)
        //{
        //    using (var db = new DBConnect(tenant.TenantSchema))
        //    {
        //        EncompassConfig _encompassConfig = db.EncompassConfig.AsNoTracking().Where(x => x.ConfigKey == EncompassConfigConstant.INSTANCE_ID).FirstOrDefault();

        //        if (_encompassConfig != null)
        //        {
        //            if (_instanceID == _encompassConfig.ConfigValue)
        //            {
        //                EncompassAccessToken token = db.EncompassAccessToken.AsNoTracking().FirstOrDefault();

        //                if (token != null)
        //                {
        //                    return new EToken() { accessToken = token.AccessToken, tokenType = token.TokenType };
        //                }
        //                else
        //                {
        //                    dynamic newToken = GetToken(db.EncompassConfig.AsNoTracking().ToList());
        //                    if (newToken != null)
        //                    {
        //                        UpdateNewToken(db, newToken.TokenType, newToken.Token);
        //                        return new EToken() { accessToken = newToken.Token, tokenType = newToken.TokenType };
        //                    }
        //                }
        //            }
        //        }

        //        return null;
        //    }
        //}

        //public void UpdateNewToken(DBConnect db, string tokenType, string token)
        //{
        //    EncompassAccessToken _accessToken = db.EncompassAccessToken.AsNoTracking().Where(m => m.Active == true).FirstOrDefault();
        //    if (_accessToken != null)
        //    {
        //        _accessToken.AccessToken = token;
        //        _accessToken.TokenType = tokenType;
        //        _accessToken.ModifiedOn = DateTime.Now;

        //        db.Entry(_accessToken).State = System.Data.Entity.EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //    else
        //    {
        //        _accessToken = new EncompassAccessToken()
        //        {
        //            AccessToken = token,
        //            Active = true,
        //            TokenType = tokenType,
        //            CreatedOn = DateTime.Now,
        //            ModifiedOn = DateTime.Now
        //        };
        //        db.EncompassAccessToken.Add(_accessToken);
        //        db.SaveChanges();
        //    }
        //}

        //private object GetToken(List<EncompassConfig> _config)
        //{
        //    string grantType = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.GRANT_TYPE).FirstOrDefault().ConfigValue;
        //    string scope = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.SCOPE).FirstOrDefault().ConfigValue;
        //    EncompassConfig instanctID = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.INSTANCE_ID).FirstOrDefault();
        //    EncompassConfig userName = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.USERNAME).FirstOrDefault();
        //    EncompassConfig password = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.PASSWORD).FirstOrDefault();
        //    EncompassConfig _clientID = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.CLIENT_ID).FirstOrDefault();
        //    EncompassConfig _clientSecret = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.CLIENT_SECRET).FirstOrDefault();

        //    RestWebClient client = new RestWebClient(ConfigurationManager.AppSettings["EncompassConnectorURL"]);

        //    HttpRequestObject req = null;
        //    if (grantType == "password")
        //    {
        //        object _res = new
        //        {
        //            ClientID = _clientID.ConfigValue,
        //            ClientSecret = _clientSecret.ConfigValue,
        //            GrantType = grantType,
        //            Scope = scope,
        //            InstanceID = instanctID != null ? instanctID.ConfigValue : string.Empty,
        //            UserName = userName != null ? userName.ConfigValue : string.Empty,
        //            Password = password != null ? password.ConfigValue : string.Empty
        //        };

        //        req = new HttpRequestObject() { Content = _res, REQUESTTYPE = "POST", URL = string.Format(EncompassURLILConstant.GET_TOKEN_WITH_USER) };
        //    }
        //    else
        //    {
        //        object _res = new
        //        {
        //            ClientID = _clientID.ConfigValue,
        //            ClientSecret = _clientSecret.ConfigValue,
        //            GrantType = grantType,
        //            Scope = scope,
        //            InstanceID = instanctID != null ? instanctID.ConfigValue : string.Empty
        //        };
        //        req = new HttpRequestObject() { Content = _res, REQUESTTYPE = "POST", URL = string.Format(EncompassURLILConstant.GET_TOKEN) };
        //    }

        //    IRestResponse result = client.Execute(req);

        //    if (result.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        string res = result.Content;
        //        dynamic resObject = JsonConvert.DeserializeObject<dynamic>(res);
        //        return new { Token = resObject.access_token, TokenType = resObject.token_type };
        //    }

        //    return null;

        //}


        #endregion

        #endregion

        #region Private Methods


        private void SetCustLoanDocumentStacking(DBConnect db, Int64 LoanTypeID, Int64 StackingOrderID)
        {
            Int64 CustomerID = 1;

            List<StackingOrderDetailMaster> _stackDetails = db.StackingOrderDetailMaster.AsNoTracking().Where(s => s.StackingOrderID == StackingOrderID).ToList();

            List<DocumentTypeMaster> _sysDocs = new IntellaLendDataAccess().GetAllSysDocTypeMasters();

            List<DocumentTypeMaster> _lsDocs = _sysDocs.Where(s => (_stackDetails.Any(d => d.DocumentTypeID == s.DocumentTypeID))).ToList();

            List<DocumentTypeMaster> _mappedDocs = GetMappedDocuments(db, CustomerID, LoanTypeID);

            List<DocumentTypeMaster> _unMappedDocs = _lsDocs.Where(s => !(_mappedDocs.Any(m => m.Name == s.Name))).ToList();

            if (_unMappedDocs != null && _unMappedDocs.Count > 0)
                MapCustLoanDocuments(db, CustomerID, LoanTypeID, _unMappedDocs);
        }

        private string MapCustLoanDocuments(DBConnect db, Int64 CustomerID, Int64 LoanTypeID, List<DocumentTypeMaster> _lsDocumentTypeMaster)
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
                db.CustLoanDocMapping.Add(docLoanMap);
                db.SaveChanges();

                //Insert Document Fields
                if (doc.DocumentFieldMasters != null)
                {
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
            }

            return String.Join(",", _insertedDocs);
        }



        private List<DocumentTypeMaster> GetMappedDocuments(DBConnect db, Int64 CustomerID, Int64 LoanTypeID)
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


        private Int64 AddReviewTypeToSystem(DBConnect db, SystemReviewTypeMaster reviewtype)
        {
            SystemReviewTypeMaster sysReviewType = db.SystemReviewTypeMaster.AsNoTracking().Where(x => x.ReviewTypeName.Equals(reviewtype.ReviewTypeName)).FirstOrDefault();

            if (sysReviewType == null)
            {
                SystemReviewTypeMaster sReviewMaster = new SystemReviewTypeMaster()
                {
                    ReviewTypeName = reviewtype.ReviewTypeName,
                    Type = reviewtype.Type,
                    Active = reviewtype.Active,
                    SearchCriteria = reviewtype.SearchCriteria,
                    BatchClassInputPath = reviewtype.BatchClassInputPath,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ReviewTypePriority = reviewtype.ReviewTypePriority,
                    UserRoleID = reviewtype.UserRoleID
                };
                db.SystemReviewTypeMaster.Add(sReviewMaster);
                db.SaveChanges();

                SetCustReviewMapping(sReviewMaster.ReviewTypeID);

                return sReviewMaster.ReviewTypeID;
            }
            return sysReviewType.ReviewTypeID;

        }

        private void SetCustReviewMapping(Int64 ReviewTypeID)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                db.CustReviewMapping.Add(new CustReviewMapping()
                {
                    CustomerID = 1, //1 - Default Customer
                    ReviewTypeID = ReviewTypeID,
                    Active = true,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                });
                db.SaveChanges();
            }
        }

        private Int64 AddLoanTypeToSystem(DBConnect db, SystemLoanTypeMaster loantype)
        {

            SystemLoanTypeMaster loanTypeMs = db.SystemLoanTypeMaster.AsNoTracking().Where(x => x.LoanTypeName.Equals(loantype.LoanTypeName)).FirstOrDefault();

            if (loanTypeMs == null)
            {
                SystemLoanTypeMaster systemLoanType = new SystemLoanTypeMaster()
                {
                    LoanTypeName = loantype.LoanTypeName,
                    Type = loantype.Type,
                    Active = loantype.Active,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };

                db.SystemLoanTypeMaster.Add(systemLoanType);
                db.SaveChanges();

                return systemLoanType.LoanTypeID;
            }

            return loanTypeMs.LoanTypeID;
        }

        private List<DocumentTypeMaster> CheckForDocMapping(DBConnect db, Int64 LoanTypeID, Int64 CheckListID)
        {
            List<CheckListDetailMaster> lsCDetailMaster = db.CheckListDetailMaster.AsNoTracking().Where(c => c.CheckListID == CheckListID).ToList();

            List<DocumentTypeMaster> lsDocMaster = GetSystemDocumentTypes(LoanTypeID);

            List<string> lsDocIDs = new List<string>();

            foreach (CheckListDetailMaster cDetail in lsCDetailMaster)
            {
                RuleMaster rm = db.RuleMaster.AsNoTracking().Where(r => r.CheckListDetailID == cDetail.CheckListDetailID).FirstOrDefault();

                if (rm != null)
                {
                    if (!string.IsNullOrEmpty(rm.DocumentType))
                        lsDocIDs.Add(rm.DocumentType);
                }
            }

            string strDocIDs = string.Join(",", lsDocIDs);

            if (!string.IsNullOrEmpty(strDocIDs))
                lsDocIDs = strDocIDs.Split(',').Distinct().ToList();

            List<DocumentTypeMaster> lsCheckDocTypes = new List<DocumentTypeMaster>();

            lsCheckDocTypes.AddRange(db.DocumentTypeMaster.AsNoTracking().Where(x => lsDocIDs.Contains(x.DocumentTypeID.ToString())).ToList());

            return lsCheckDocTypes.Where(x => !(lsDocMaster.Any(y => x.DocumentTypeID == y.DocumentTypeID))).ToList();
        }

        private List<DocumentTypeMaster> CheckForLoanDocMapping(DBConnect db, Int64 LoanTypeID, Int64 StackingOrderID)
        {
            List<StackingOrderDetailMaster> lsStackingOrderDetailMaster = db.StackingOrderDetailMaster.AsNoTracking().Where(c => c.StackingOrderID == StackingOrderID).ToList();

            List<DocumentTypeMaster> lsDocMaster = GetSystemDocumentTypes(LoanTypeID);

            List<DocumentTypeMaster> lsStackDocTypes = new List<DocumentTypeMaster>();

            foreach (StackingOrderDetailMaster sDetail in lsStackingOrderDetailMaster)
                lsStackDocTypes.Add(db.DocumentTypeMaster.AsNoTracking().Where(x => x.DocumentTypeID == sDetail.DocumentTypeID).FirstOrDefault());

            return lsStackDocTypes.Where(x => !(lsDocMaster.Any(y => x.DocumentTypeID == y.DocumentTypeID))).ToList();
        }

        public bool SetOrderByField(Int64 DocumentTypeID, Int64 FieldID)
        {
            bool result = false;
            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    //DocumentFieldMaster docField = db.DocumentFieldMaster.AsNoTracking().Where(d => d.FieldID == FieldID).FirstOrDefault();

                    //if (docField != null)
                    //{
                    List<DocumentFieldMaster> _docFields = db.DocumentFieldMaster.AsNoTracking().Where(d => d.DocumentTypeID == DocumentTypeID).ToList();

                    _docFields.ForEach(ele =>
                    {
                        if (ele.FieldID == FieldID)
                            ele.DocOrderByField = "Desc";
                        else
                            ele.DocOrderByField = null;

                        db.Entry(ele).State = EntityState.Modified;
                        db.SaveChanges();
                    });

                    //docField.DocOrderByField = "Desc";
                    //db.Entry(docField).State = EntityState.Modified;
                    //db.SaveChanges();
                    tran.Commit();
                    result = true;
                    //}
                }
            }

            return result;
        }

        public object GetSystemStackingOrderDetailMaster(long stackingOrderID)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                //db.StackingOrderDetailMaster.AsNoTracking().Where(stdm => stdm.StackingOrderID == stackingOrderID).ToList();
                return (from sod in db.StackingOrderDetailMaster
                        join dm in db.DocumentTypeMaster on sod.DocumentTypeID equals dm.DocumentTypeID
                        //join sogroup in db.StackingOrderGroupmasters on sod.StackingOrderGroupID equals sogroup.StackingOrderGroupID
                        where sod.StackingOrderID == stackingOrderID && dm.Active == true
                        select new
                        {
                            DocumentTypeID = sod.DocumentTypeID,
                            DocumentTypeName = dm.DisplayName,
                            SequenceID = sod.SequenceID,
                            StackingOrderDetailID = sod.StackingOrderDetailID,
                            StackingOrderID = sod.StackingOrderID,
                            DocFieldList = (from f in db.DocumentFieldMaster
                                            where f.DocumentTypeID == sod.DocumentTypeID
                                            select f).ToList(),
                            OrderByFieldID = (from f in db.DocumentFieldMaster
                                              where f.DocumentTypeID == sod.DocumentTypeID && f.DocOrderByField != null
                                              select f.FieldID).FirstOrDefault(),
                            DocFieldValueId = (from f in db.DocumentFieldMaster
                                               where f.DocumentTypeID == sod.DocumentTypeID && f.IsDocName == true
                                               select f.FieldID).ToList(),

                            StackingOrderGroupDetails = (from sog in db.StackingOrderGroupmasters
                                                         where sog.StackingOrderGroupID == sod.StackingOrderGroupID
                                                         select sog).ToList()
                        }).OrderBy(x => x.SequenceID).ToList();
            }
        }

        public bool SaveSysCheckListName(string checkListName, long checkListID, bool Active, bool Sync)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    CheckListMaster cMaster = db.CheckListMaster.AsNoTracking().Where(sm => sm.CheckListID == checkListID).FirstOrDefault();
                    cMaster.CheckListName = checkListName;
                    cMaster.ModifiedOn = DateTime.Now;
                    cMaster.Active = Active;
                    cMaster.Sync = Sync;
                    db.Entry(cMaster).State = EntityState.Modified;
                    db.SaveChanges();


                    using (var tenantDb = new DBConnect("T1"))
                    {
                        List<CheckListMaster> tenantcheckList = tenantDb.CheckListMaster.Where(x => x.CheckListName.ToLower().Trim() == checkListName.ToLower().Trim()).ToList();
                        if (tenantcheckList.Count() > 0)
                        {
                            foreach (CheckListMaster tenantcheck in tenantcheckList)
                            {
                                tenantcheck.Sync = Sync;
                                tenantcheck.ModifiedOn = DateTime.Now;
                                tenantDb.Entry(tenantcheck).State = EntityState.Modified;
                                tenantDb.SaveChanges();
                            }
                        }
                    }
                    trans.Commit();
                    return true;
                }
            }

            return false;
        }

        public object AssignChecklist(long LoanTypeID, string CheckListName, long CheckListID)
        {
            Int64 NewCheckListID = 0;
            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {

                    db.CustReviewLoanCheckMapping.RemoveRange(db.CustReviewLoanCheckMapping.Where(c => c.CustomerID == 1 && c.ReviewTypeID == 0 && c.LoanTypeID == LoanTypeID));
                    db.SaveChanges();

                    List<DocumentTypeMaster> UnMappedDocTypes = CheckForDocMapping(db, LoanTypeID, CheckListID);

                    if (UnMappedDocTypes != null && UnMappedDocTypes.Count > 0)
                    {
                        foreach (DocumentTypeMaster item in UnMappedDocTypes)
                        {
                            db.CustLoanDocMapping.Add(new CustLoanDocMapping()
                            {
                                CustomerID = 1, //Default for System Schema                                
                                LoanTypeID = LoanTypeID,
                                DocumentTypeID = item.DocumentTypeID,
                                Active = true,
                                DocumentLevel = item.DocumentLevel,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now
                            });
                        }
                    }

                    CheckListMaster sysCheckListMaster = GetCheckListGroup(db, CheckListID);
                    if (sysCheckListMaster != null)
                    {
                        //Insert  CheckList Master Table
                        CheckListMaster checkListMaster = new CheckListMaster();
                        checkListMaster.CheckListID = 0;
                        checkListMaster.CheckListName = CheckListName;
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
                                checkListDetailMaster.Category = sysCheckListDetailMasters.Category;
                                checkListDetailMaster.CheckListDetailID = 0;
                                checkListDetailMaster.CheckListID = checkListMaster.CheckListID;
                                checkListDetailMaster.Description = sysCheckListDetailMasters.Description;
                                checkListDetailMaster.Active = true;
                                checkListDetailMaster.Rule_Type = sysCheckListDetailMasters.Rule_Type;
                                checkListDetailMaster.UserID = sysCheckListDetailMasters.UserID;
                                checkListDetailMaster.SequenceID = sysCheckListDetailMasters.SequenceID;
                                checkListDetailMaster.Name = sysCheckListDetailMasters.Name;
                                checkListDetailMaster.CreatedOn = DateTime.Now;
                                checkListDetailMaster.ModifiedOn = DateTime.Now;
                                checkListDetailMaster.LOSFieldToEvalRule = sysCheckListDetailMasters.LOSFieldToEvalRule;
                                checkListDetailMaster.LOSValueToEvalRule = string.IsNullOrEmpty(sysCheckListDetailMasters.LOSValueToEvalRule) ? string.Empty : sysCheckListDetailMasters.LOSValueToEvalRule;
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
                                    ruleMaster.DocVersion = sysCheckListDetailMasters.RuleMasters.DocVersion;
                                    db.RuleMaster.Add(ruleMaster);
                                    db.SaveChanges();
                                }
                            }
                        }

                        //Insert  Cust->Review->Loan->CheckList
                        CustReviewLoanCheckMapping custReviewLoanCheckMap = new CustReviewLoanCheckMapping();
                        custReviewLoanCheckMap.ID = 0;
                        custReviewLoanCheckMap.CustomerID = 1;
                        custReviewLoanCheckMap.ReviewTypeID = 0;
                        custReviewLoanCheckMap.LoanTypeID = LoanTypeID;
                        custReviewLoanCheckMap.CheckListID = checkListMaster.CheckListID;
                        custReviewLoanCheckMap.CreatedOn = DateTime.Now;
                        custReviewLoanCheckMap.ModifiedOn = DateTime.Now;
                        custReviewLoanCheckMap.Active = true;
                        db.CustReviewLoanCheckMapping.Add(custReviewLoanCheckMap);
                        db.SaveChanges();

                        NewCheckListID = checkListMaster.CheckListID;
                    }

                    tran.Commit();

                    return new { CheckListID = NewCheckListID };
                }
            }

            return null;
        }

        public object AssignStackingOrder(long LoanTypeID, string StackingOrderName, long StackingOrderID)
        {
            Int64 NewStackingOrderID = 0;
            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {

                    db.CustReviewLoanStackMapping.RemoveRange(db.CustReviewLoanStackMapping.AsNoTracking().Where(c => c.CustomerID == 1 && c.ReviewTypeID == 0 && c.LoanTypeID == LoanTypeID));
                    db.SaveChanges();
                    List<DocumentTypeMaster> UnMappedDocTypes = new List<DocumentTypeMaster>();
                    UnMappedDocTypes = CheckForLoanDocMapping(db, LoanTypeID, StackingOrderID);

                    if (UnMappedDocTypes != null && UnMappedDocTypes.Count > 0)
                    {
                        foreach (DocumentTypeMaster item in UnMappedDocTypes)
                        {
                            db.CustLoanDocMapping.Add(new CustLoanDocMapping()
                            {
                                CustomerID = 1, //Default for System Schema                                
                                LoanTypeID = LoanTypeID,
                                DocumentTypeID = item.DocumentTypeID,
                                Active = true,
                                DocumentLevel = item.DocumentLevel,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now
                            });
                        }
                    }

                    StackingOrderMaster _sysStack = GetSysStackingOrder(db, StackingOrderID);
                    if (_sysStack != null)
                    {

                        StackingOrderMaster _stack = new StackingOrderMaster()
                        {
                            StackingOrderID = 0,
                            Description = StackingOrderName,
                            Active = true,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        };

                        db.StackingOrderMaster.Add(_stack);
                        db.SaveChanges();

                        StackingOrderGroupmasters systemStackMasters = null;
                        string dupName = "";
                        if (_sysStack.StackingOrderDetailMasters != null)
                        {
                            foreach (StackingOrderDetailMaster item in _sysStack.StackingOrderDetailMasters)
                            {


                                if (item.StackingOrderGroupID > 0)
                                {
                                    var stackingOrderGroup = AddStackingOrderMasterGroup(item.StackingOrderGroupID);
                                    if (stackingOrderGroup.StackingOrderGroupName != dupName)
                                    {
                                        dupName = stackingOrderGroup.StackingOrderGroupName;
                                        //StackingOrderGroupmasters tenantStackMasters = new StackingOrderGroupmasters();

                                        systemStackMasters = db.StackingOrderGroupmasters.Add(new StackingOrderGroupmasters()
                                        {
                                            StackingOrderID = _stack.StackingOrderID,
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

                                StackingOrderDetailMaster _stackDetail = new StackingOrderDetailMaster()
                                {
                                    StackingOrderDetailID = 0,
                                    StackingOrderID = _stack.StackingOrderID,
                                    Active = true,
                                    CreatedOn = DateTime.Now,
                                    ModifiedOn = DateTime.Now,
                                    DocumentTypeID = item.DocumentTypeID,
                                    SequenceID = item.SequenceID,
                                    StackingOrderGroupID = (item.StackingOrderGroupID > 0) ? systemStackMasters.StackingOrderGroupID : 0
                                };

                                db.StackingOrderDetailMaster.Add(_stackDetail);
                                db.SaveChanges();
                            }
                        }

                        db.CustReviewLoanStackMapping.Add(new CustReviewLoanStackMapping()
                        {
                            CustomerID = 1, //Default for System Schema
                            ReviewTypeID = 0,
                            LoanTypeID = LoanTypeID,
                            StackingOrderID = _stack.StackingOrderID,
                            Active = true,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        });

                        db.SaveChanges();

                        NewStackingOrderID = _stack.StackingOrderID;
                        tran.Commit();
                    }
                    return new { StackingOrderID = NewStackingOrderID };
                }
            }

            return null;
        }

        public bool SetDocFieldValue(Int64 DocumentTypeID, Int64 FieldID)
        {
            bool result = false;
            using (var db = new DBConnect(SystemSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    List<DocumentFieldMaster> _docFields = db.DocumentFieldMaster.AsNoTracking().Where(d => d.DocumentTypeID == DocumentTypeID).ToList();

                    _docFields.ForEach(ele =>
                    {
                        if (ele.FieldID == FieldID)
                        {
                            ele.IsDocName = true;
                        }
                        else
                        {
                            ele.IsDocName = false;
                        }
                        db.Entry(ele).State = EntityState.Modified;
                        db.SaveChanges();
                    });
                    tran.Commit();
                    result = true;
                }
            }
            return result;
        }


        #endregion
        //public List<DocumentTypeMaster> GetDocumentTypesWithFields(string TenantSchema, Int64 CustomerID, Int64 loanTypeID)
        //{
        //    List<DocumentTypeMaster> dm = null;

        //    using (var db = new DBConnect(TenantSchema))
        //    {
        //        List<CustLoanDocMapping> cldm = db.CustLoanDocMapping.AsNoTracking().Where(cld => cld.CustomerID == CustomerID && cld.LoanTypeID == loanTypeID).ToList();

        //        dm = new List<DocumentTypeMaster>();

        //        if (cldm != null)
        //        {
        //            foreach (CustLoanDocMapping loanDocMap in cldm)
        //            {
        //                DocumentTypeMaster doc = db.DocumentTypeMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID).FirstOrDefault();
        //                if (doc != null)
        //                {
        //                    doc.DocumentFieldMasters = new List<DocumentFieldMaster>();

        //                    doc.DocumetTypeTables = new List<DocumetTypeTables>();

        //                    List<DocumentFieldMaster> docF = db.DocumentFieldMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID).ToList();
        //                    List<DocumetTypeTables> docT = db.DocumetTypeTables.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID).ToList();
        //                    if (docF != null)
        //                        doc.DocumentFieldMasters = docF;

        //                    if (docT != null)
        //                        doc.DocumetTypeTables = docT;

        //                    dm.Add(doc);
        //                }
        //            }
        //        }
        //    }

        //    return dm;
        //}
        //public StackingOrderMaster GetStackingOrder(string TenantSchema, Int64 StackingOrderID)
        //{
        //    StackingOrderMaster _stackingOrderMaster = null;

        //    using (var db = new DBConnect(TenantSchema))
        //    {
        //        _stackingOrderMaster = db.StackingOrderMaster.AsNoTracking().Where(c => c.StackingOrderID == StackingOrderID).FirstOrDefault();

        //        if (_stackingOrderMaster != null)
        //        {
        //            _stackingOrderMaster.StackingOrderDetailMasters = GetSystemStackingOrderDetails(db, _stackingOrderMaster.StackingOrderID);
        //        }
        //    }

        //    return _stackingOrderMaster;
        //}

        #region RoleMaster
        //public class AccessUrlList
        //{
        //    public string URL { get; set; }
        //}
        public object AddRoleDetails(RoleMaster roletype, List<MenuMaster> menus)
        {
            bool isRoleAdded = false;
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    if (!db.Roles.AsNoTracking().Any(x => x.RoleName.Equals(roletype.RoleName)))
                    {
                        roletype.RoleID = AddRoleDetails(db, roletype);
                        if (roletype.RoleID > 0)
                        {
                            isRoleAdded = true;
                            if (menus != null && menus.Count > 0)
                            {
                                AddRoleMenuMapping(db, menus, roletype.RoleID);
                                AddRoleAccessUrls(db, roletype.RoleID, menus);
                            }
                        }
                    }

                    tran.Commit();
                }
            }
            return new { roleid = roletype.RoleID, Success = isRoleAdded };
        }
        public bool SyncRetainUpdateStagings(Int64 LoanTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {

                RetainUpdateStaging _syncUpdate = db.RetainUpdateStaging.Where(l => l.LoanTypeID == LoanTypeID).FirstOrDefault();
                if (_syncUpdate != null)
                {
                    _syncUpdate.SyncLevel = 2;
                    _syncUpdate.ModifiedOn = DateTime.Now;
                    db.Entry(_syncUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private Int64 AddRoleDetails(DBConnect db, RoleMaster roletype)
        {
            RoleMaster _roletype = db.Roles.AsNoTracking().Where(x => x.RoleName.Equals(roletype.RoleName)).FirstOrDefault();

            if (_roletype == null)
            {
                RoleMaster systemroletype = new RoleMaster()
                {
                    RoleName = roletype.RoleName,
                    StartPage = roletype.StartPage,
                    AuthorityLevel = 20,
                    Active = roletype.Active,
                    IncludeKpi = roletype.IncludeKpi,
                    ADGroupID = roletype.ADGroupID
                };

                db.Roles.Add(systemroletype);
                db.SaveChanges();

                return systemroletype.RoleID;
            }

            return roletype.RoleID;
        }
        public Int64 AddRoleAccessUrls(DBConnect db, Int64 RoleId, List<MenuMaster> menus)
        {
            AccessURL systemMenuAccessUrl = new AccessURL();
            foreach (var item in menus)
            {
                if (item.MenuID > 0)
                {
                    var _lstaccessUrl = db.MenuAccessUrl.AsNoTracking().Where(x => x.MenuID == item.MenuID).Select(x => x.Url).Distinct().ToList();
                    if (_lstaccessUrl != null)
                    {
                        foreach (var data in _lstaccessUrl)
                        {
                            systemMenuAccessUrl = new AccessURL()
                            {
                                URL = data,
                                RoleID = RoleId
                            };

                            db.AccessURLs.Add(systemMenuAccessUrl);
                            db.SaveChanges();
                        }
                    }
                }
            }
            return systemMenuAccessUrl.ID;
        }
        public bool AddRoleMenuMapping(DBConnect db, List<MenuMaster> menus, Int64 _RoleID)
        {
            RoleMenuMapping sysRoleMenuMap = new RoleMenuMapping();
            foreach (var item in menus)
            {
                if (item.MenuID > 0)
                {
                    Int64 _menuOrder = db.Menus.AsNoTracking().Where(x => x.MenuID == item.MenuID).FirstOrDefault().MenuOrderID;
                    sysRoleMenuMap = new RoleMenuMapping()
                    {
                        MenuID = item.MenuID,
                        RoleID = _RoleID,
                        MenuOrder = _menuOrder
                    };
                    db.RoleMenuMapping.Add(sysRoleMenuMap);
                    db.SaveChanges();
                }
            }
            return true;
        }
        public object UpdateRoleDetails(RoleMaster roletype, List<MenuMaster> menus)
        {
            bool isRoleupdate = false;
            using (var db = new DBConnect(TenantSchema))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    RoleMaster dbObject = db.Roles.AsNoTracking().Where(l => l.RoleID == roletype.RoleID).FirstOrDefault();
                    List<UserRoleMapping> dbUserRoles = db.UserRoleMapping.AsNoTracking().Where(l => l.RoleID == roletype.RoleID).ToList();

                    if (dbObject != null)
                    {
                        dbObject.RoleName = roletype.RoleName;
                        dbObject.AuthorityLevel = dbObject.AuthorityLevel;
                        dbObject.StartPage = roletype.StartPage;
                        dbObject.Active = roletype.Active;
                        dbObject.IncludeKpi = roletype.IncludeKpi;
                        dbObject.ADGroupID = roletype.ADGroupID;
                        db.Entry(dbObject).State = EntityState.Modified;
                        db.SaveChanges();
                        isRoleupdate = true;
                        if (menus != null && menus.Count > 0)
                        {
                            bool menuaccessid = UpdateMenuAccess(db, roletype.RoleID, menus);
                        }
                        if (dbUserRoles.Count > 0)
                        {
                            foreach (UserRoleMapping item in dbUserRoles)
                            {
                                item.RoleName = roletype.RoleName;
                                db.Entry(item).State = EntityState.Modified;
                            }
                            db.SaveChanges();
                        }

                        transaction.Commit();
                    }

                }
            }
            return new { roleid = roletype.RoleID, Success = isRoleupdate };
        }

        public bool UpdateMenuAccess(DBConnect db, Int64 RoleId, List<MenuMaster> Menus)
        {

            foreach (var item in Menus)
            {
                if (item.MenuID > 0)
                {

                    //
                    DeleteRoleMenuMapping(db, RoleId, item.MenuID);
                    DeleteRoleAccessUrls(db, RoleId);

                }
            }
            RoleMenuMapping _rolemenumapping = new RoleMenuMapping();
            AddRoleMenuMapping(db, Menus, RoleId);
            AddRoleAccessUrls(db, RoleId, Menus);
            return true;
        }
        public bool DeleteRoleMenuMapping(DBConnect db, Int64 RoleId, Int64 MenuID)
        {


            List<RoleMenuMapping> GetRoleMenuMapping = db.RoleMenuMapping.AsNoTracking().Where(x => x.RoleID == RoleId).ToList();
            foreach (var obj in GetRoleMenuMapping)
            {
                db.Entry(obj).State = EntityState.Deleted;
                db.SaveChanges();
                //    transaction.Commit();
            }
            return true;
        }
        public bool DeleteRoleAccessUrls(DBConnect db, Int64 RoleId)
        {

            List<AccessURL> GetRoleAccessUrl = db.AccessURLs.AsNoTracking().Where(x => x.RoleID == RoleId).ToList();
            foreach (var obj in GetRoleAccessUrl)
            {
                db.Entry(obj).State = EntityState.Deleted;
                db.SaveChanges();
                //transaction.Commit();
            }

            return true;
        }

        public object ChecUserRoleDetails(Int64 RoleId)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.UserRoleMapping.AsNoTracking().Where(x => x.RoleID == RoleId).FirstOrDefault();
            }
        }
        #endregion

        public bool SetDocGroupFieldValue(GetStackOrder StackGroupDetails, List<StackingOrderDocumentFieldMaster> StackingOrderDetails)
        {
            bool result = false;

            using (var db = new DBConnect(TenantSchema))
            {
                StackingOrderGroupmasters _stackgroupdetail = db.StackingOrderGroupmasters.AsNoTracking().Where(x => x.StackingOrderGroupName.Trim() == StackGroupDetails.Name.Trim()).FirstOrDefault();
                if (_stackgroupdetail != null)
                {
                    _stackgroupdetail.GroupSortField = StackGroupDetails.StackingOrderFieldName;
                    _stackgroupdetail.ModifiedOn = DateTime.Now;
                    db.Entry(_stackgroupdetail).State = EntityState.Modified;
                    db.SaveChanges();
                    foreach (StackingOrderDocumentFieldMaster _stackingorder in StackingOrderDetails)
                    {
                        List<DocumentFieldMaster> _docFields = db.DocumentFieldMaster.AsNoTracking().Where(d => d.DocumentTypeID == _stackingorder.DocumentTypeID).ToList();
                        _docFields.ForEach(ele =>
                        {
                            if (ele.Name.Trim() == StackGroupDetails.StackingOrderFieldName.Trim())
                                ele.DocOrderByField = "Desc";
                            else
                                ele.DocOrderByField = null;
                            db.Entry(ele).State = EntityState.Modified;
                            db.SaveChanges();
                        });
                    }
                }
                result = true;
            }



            return result;
        }

        public void AddCustomerCheckListDetails(Int64 LoanTypeId, CheckListDetailMaster checklistdetailmaster, RuleMaster rulemasters)
        {
            using (var db = new DBConnect("T1"))
            {
                List<CustReviewLoanCheckMapping> GetCusReviewLoanCheckDetails = (from crl in db.CustReviewLoanCheckMapping.AsNoTracking()
                                                                                 join lt in db.LoanTypeMaster.AsNoTracking() on crl.LoanTypeID equals lt.LoanTypeID
                                                                                 where lt.Active == true
                                                                                 && crl.LoanTypeID == LoanTypeId
                                                                                 select crl).OrderBy(x => x.CustomerID).OrderBy(y => y.ReviewTypeID).ToList();
                foreach (CustReviewLoanCheckMapping _cusreviewloancheck in GetCusReviewLoanCheckDetails)
                {
                    List<CheckListDetailMaster> _GetcheckListDetailMasters = db.CheckListDetailMaster.Where(x => x.CheckListID == _cusreviewloancheck.CheckListID).ToList();
                    var AddcheckExist = _GetcheckListDetailMasters.Where(x => x.Name.Contains(checklistdetailmaster.Name)).FirstOrDefault();
                    if (AddcheckExist == null)
                    {
                        //checklistdetailmaster.CheckListDetailID = 0;
                        //checklistdetailmaster.CheckListID = _cusreviewloancheck.CheckListID;

                        CheckListDetailMaster _checklist = db.CheckListDetailMaster.Add(new CheckListDetailMaster
                        {
                            CheckListID = _cusreviewloancheck.CheckListID,
                            Name = checklistdetailmaster.Name,
                            Description = checklistdetailmaster.Description,
                            Active = checklistdetailmaster.Active,
                            Category = checklistdetailmaster.Category,
                            CreatedOn = checklistdetailmaster.CreatedOn,
                            ModifiedOn = checklistdetailmaster.ModifiedOn,
                            UserID = checklistdetailmaster.UserID,
                            Rule_Type = checklistdetailmaster.Rule_Type,
                            SequenceID = checklistdetailmaster.SequenceID
                        });
                        db.SaveChanges();
                        rulemasters.CheckListDetailID = _checklist.CheckListDetailID;
                        db.RuleMaster.Add(rulemasters);
                        db.SaveChanges();


                    }
                }
            }
        }

        public void UpdateCustomerCheckListDetails(Int64 LoanTypeId, CheckListDetailMaster checkListDetailMaster, RuleMaster rulemasters)
        {
            using (var db = new DBConnect("T1"))
            {
                //using (var trans = db.Database.BeginTransaction())
                //{

                List<CustReviewLoanCheckMapping> GetCusReviewLoanCheckDetails = (from crl in db.CustReviewLoanCheckMapping.AsNoTracking()
                                                                                 join lt in db.LoanTypeMaster.AsNoTracking() on crl.LoanTypeID equals lt.LoanTypeID
                                                                                 where lt.Active == true
                                                                                 && crl.LoanTypeID == LoanTypeId
                                                                                 select crl).OrderBy(x => x.CustomerID).OrderBy(y => y.ReviewTypeID).ToList();
                foreach (CustReviewLoanCheckMapping _cusreviewloancheck in GetCusReviewLoanCheckDetails)
                {
                    List<CheckListDetailMaster> _GetcheckListDetailMasters = db.CheckListDetailMaster.Where(x => x.CheckListID == _cusreviewloancheck.CheckListID).ToList();
                    foreach (CheckListDetailMaster _checklist in _GetcheckListDetailMasters)
                    {
                        CheckListDetailMaster updatechecklistdetailsmaster = db.CheckListDetailMaster.AsNoTracking().Where(up => up.CheckListDetailID == _checklist.CheckListDetailID).FirstOrDefault();
                        if (updatechecklistdetailsmaster != null)
                        {
                            updatechecklistdetailsmaster.Name = checkListDetailMaster.Name;
                            updatechecklistdetailsmaster.Description = checkListDetailMaster.Description;
                            updatechecklistdetailsmaster.Active = checkListDetailMaster.Active;
                            updatechecklistdetailsmaster.ModifiedOn = DateTime.Now;
                            updatechecklistdetailsmaster.Category = checkListDetailMaster.Category;
                            db.Entry(updatechecklistdetailsmaster).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        RuleMaster updaterulemasters = db.RuleMaster.AsNoTracking().Where(upd => upd.CheckListDetailID == _checklist.CheckListDetailID).FirstOrDefault();
                        if (updaterulemasters != null)
                        {
                            updaterulemasters.RuleDescription = rulemasters.RuleDescription;
                            updaterulemasters.RuleJson = rulemasters.RuleJson;
                            updaterulemasters.Active = rulemasters.Active;
                            updaterulemasters.DocumentType = rulemasters.DocumentType;
                            updaterulemasters.ActiveDocumentType = rulemasters.ActiveDocumentType;
                            updaterulemasters.DocVersion = rulemasters.DocVersion;
                            db.Entry(updaterulemasters).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    //  }
                    // trans.Commit();
                }
            }
        }
        public void DeleteCustomerCheckListDetails(Int64 checkListDetailID, Int64 LoanTypeID, String ChecklistItemName)
        {
            using (var db = new DBConnect("T1"))
            {
                //using (var trans = db.Database.BeginTransaction())
                //{
                List<CustReviewLoanCheckMapping> GetCusReviewLoanCheckDetails = (from crl in db.CustReviewLoanCheckMapping.AsNoTracking()
                                                                                 join lt in db.LoanTypeMaster.AsNoTracking() on crl.LoanTypeID equals lt.LoanTypeID
                                                                                 where lt.Active == true
                                                                                 && crl.LoanTypeID == LoanTypeID
                                                                                 select crl).OrderBy(x => x.CustomerID).OrderBy(y => y.ReviewTypeID).ToList();
                foreach (CustReviewLoanCheckMapping _cusreviewloancheck in GetCusReviewLoanCheckDetails)
                {
                    List<CheckListDetailMaster> _GetcheckListDetailMasters = db.CheckListDetailMaster.Where(x => x.CheckListID == _cusreviewloancheck.CheckListID).ToList();
                    foreach (CheckListDetailMaster _CheckListDetails in _GetcheckListDetailMasters)
                    {

                        CheckListDetailMaster checklistmaster = db.CheckListDetailMaster.Where(ch => ch.Name.Trim().ToLower() == ChecklistItemName.Trim().ToLower() && ch.CheckListID == _cusreviewloancheck.CheckListID).FirstOrDefault();
                        if (checklistmaster != null)
                        {

                            RuleMaster rulemaster = db.RuleMaster.Where(r => r.CheckListDetailID == checklistmaster.CheckListDetailID).FirstOrDefault();

                            if (rulemaster != null)
                            {
                                db.Entry(rulemaster).State = EntityState.Deleted;
                                db.RuleMaster.Remove(rulemaster);
                                db.SaveChanges();
                            }
                            if (checklistmaster != null)
                            {
                                db.Entry(checklistmaster).State = EntityState.Deleted;
                                db.CheckListDetailMaster.Remove(checklistmaster);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
        }

        public List<RuleMaster> GetCheckListDetailRuleMaster(Int64 CheckListDetailID)
        {
            List<RuleMaster> _rulemasterList = new List<RuleMaster>();
            using (var db = new DBConnect(SystemSchema))
            {
                _rulemasterList = db.RuleMaster.Where(x => x.CheckListDetailID == CheckListDetailID).ToList();
            }
            return _rulemasterList;
        }

        public List<CategoryLists> GetChecklistCategories()
        {
            List<CategoryLists> _categorylist = new List<CategoryLists>();
            using (var db = new DBConnect(SystemSchema))
            {
                _categorylist = db.CategoryLists.AsNoTracking().Where(cl => cl.Active == true).ToList();
            }
            return _categorylist;
        }

        public bool SaveTenantCheckListDetails(Int64 CheckListID, Int64 LoanTypeID)
        {
            using (var db = new DBConnect("T1"))
            {
                CheckListMaster checklist = new CheckListMaster();
                List<CheckListDetailMaster> LoanTypechecklist = new List<CheckListDetailMaster>();
                using (var ILDb = new DBConnect(SystemSchema))
                {
                    checklist = (from crlcm in ILDb.CustReviewLoanCheckMapping.AsNoTracking()
                                 join clm in ILDb.CheckListMaster on crlcm.CheckListID equals clm.CheckListID
                                 where crlcm.LoanTypeID == LoanTypeID
                                 && clm.CheckListID == CheckListID
                                 && clm.Sync == true
                                 select clm).FirstOrDefault();
                    LoanTypechecklist = ILDb.CheckListDetailMaster.Where(x => x.CheckListID == CheckListID).ToList();
                    //checklist = ILDb.CheckListMaster.Where(x => x.CheckListID == CheckListID && x.Sync == true).FirstOrDefault();
                }
                if (checklist != null)
                {
                    List<CustReviewLoanCheckMapping> GetCusReviewLoanCheckDetails = (from crl in db.CustReviewLoanCheckMapping.AsNoTracking()
                                                                                     join lt in db.LoanTypeMaster.AsNoTracking() on crl.LoanTypeID equals lt.LoanTypeID
                                                                                     where lt.Active == true
                                                                                     && crl.LoanTypeID == LoanTypeID
                                                                                     select crl).OrderBy(x => x.CustomerID).OrderBy(y => y.ReviewTypeID).ToList();
                    foreach (CustReviewLoanCheckMapping cusreview in GetCusReviewLoanCheckDetails)
                    {
                        List<CheckListDetailMaster> GetCusCheckListCOunt = db.CheckListDetailMaster.Where(x => x.CheckListID == cusreview.CheckListID).ToList();
                        //if (GetCusCheckListCOunt.Count() <= LoanTypechecklist.Count())
                        //{

                        foreach (CheckListDetailMaster TenantChl in LoanTypechecklist)
                        {
                            var checkExistName = GetCusCheckListCOunt.Where(x => x.Name.ToLower().Trim().Contains(TenantChl.Name.ToLower().Trim())).FirstOrDefault();
                            if (checkExistName == null)
                            {
                                CheckListDetailMaster _checklst = db.CheckListDetailMaster.Add(new CheckListDetailMaster
                                {
                                    CheckListID = cusreview.CheckListID,
                                    Name = TenantChl.Name,
                                    Description = TenantChl.Description,
                                    Active = TenantChl.Active,
                                    Category = TenantChl.Category,
                                    CreatedOn = TenantChl.CreatedOn,
                                    ModifiedOn = TenantChl.ModifiedOn,
                                    UserID = TenantChl.UserID,
                                    Rule_Type = TenantChl.Rule_Type,
                                    SequenceID = TenantChl.SequenceID
                                });
                                db.SaveChanges();
                                List<RuleMaster> tenantrulemaster = GetCheckListDetailRuleMaster(TenantChl.CheckListDetailID);
                                foreach (RuleMaster rulemas in tenantrulemaster)
                                {
                                    rulemas.CheckListDetailID = _checklst.CheckListDetailID;
                                    db.RuleMaster.Add(rulemas);
                                    db.SaveChanges();
                                }


                            }
                        }

                        //  }
                    }
                    //foreach (CustReviewLoanCheckMapping _cusreviewloancheck in GetCusReviewLoanCheckDetails)
                    //{
                    //    List<CheckListDetailMaster> _GetcheckListDetailMasters = db.CheckListDetailMaster.Where(x => x.CheckListID == _cusreviewloancheck.CheckListID).ToList();
                    //    foreach (CheckListDetailMaster _CheckListDetails in _GetcheckListDetailMasters)
                    //    {
                    //        CheckListDetailMaster _checklistmaster = db.CheckListDetailMaster.Where(ch => ch.CheckListDetailID == _CheckListDetails.CheckListDetailID).FirstOrDefault();
                    //        if (_checklistmaster != null)
                    //        {
                    //            RuleMaster rulemaster = db.RuleMaster.Where(r => r.CheckListDetailID == _checklistmaster.CheckListDetailID).FirstOrDefault();
                    //            if (rulemaster != null)
                    //            {
                    //                db.Entry(rulemaster).State = EntityState.Deleted;
                    //                db.RuleMaster.Remove(rulemaster);
                    //                db.SaveChanges();
                    //            }
                    //            if (_checklistmaster != null)
                    //            {
                    //                db.Entry(_checklistmaster).State = EntityState.Deleted;
                    //                db.CheckListDetailMaster.Remove(_checklistmaster);
                    //                db.SaveChanges();
                    //            }
                    //        }
                    //    }
                    //}
                    //foreach (CheckListDetailMaster TenantChl in LoanTypechecklist)
                    //{

                    //    CheckListDetailMaster _checklst = db.CheckListDetailMaster.Add(TenantChl);
                    //    db.SaveChanges();
                    //    List<RuleMaster> tenantrulemaster = GetCheckListDetailRuleMaster(TenantChl.CheckListDetailID);
                    //    foreach (RuleMaster rulemas in tenantrulemaster)
                    //    {
                    //        rulemas.CheckListDetailID = _checklst.CheckListDetailID;
                    //        db.RuleMaster.Add(rulemas);
                    //        db.SaveChanges();
                    //    }
                    //}
                }
            }


            return true;
        }
        public List<CheckListDetailMaster> GetCheckListDetailMaster(Int64 CheckListID)
        {
            List<CheckListDetailMaster> _checklistdetailsmaster = new List<CheckListDetailMaster>();
            using (var db = new DBConnect("T1"))
            {
                List<CheckListDetailMaster> _GetcheckListDetailMasters = db.CheckListDetailMaster.Where(x => x.CheckListID == CheckListID).ToList();
            }
            return _checklistdetailsmaster;
        }

        public string GetStipulationCategoryName(Int64 StipulationCategoryID)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.InvestorStipulations.AsNoTracking().Where(m => m.StipulationID == StipulationCategoryID).Select(l => l.StipulationCategory).FirstOrDefault();
            }
        }

        public List<EncompassDocFields> GetLOSDocFields()
        {
            List<EncompassDocFields> ls = new List<EncompassDocFields>();


            using (var db = new DBConnect(SystemSchema))
            {
                ls = (from enc in db.EncompassFields.AsNoTracking()
                      select new EncompassDocFields()
                      {
                          ID = enc.ID,
                          FieldId = enc.FieldID,
                          FieldIDDescription = "#" + enc.FieldID + "# - " + enc.FieldDesc
                      }).ToList();
                return ls;
            }
        }
        public bool SetLOSDocFieldValues(string losType)
        {
            List<LoanTapeDocFields> ls = new List<LoanTapeDocFields>();
            using (var db = new DBConnect(SystemSchema))
            {
                switch (losType)
                {
                    case "Encompass":
                        ls = (from enc in db.EncompassFields.AsNoTracking()
                              select new LoanTapeDocFields()
                              {
                                  FieldId = enc.FieldID,
                                  FieldIDDescription = "#" + enc.FieldID + "# - " + enc.FieldDesc
                              }).ToList();
                        break;
                    case "3.2 Loan Tape Data Fields":
                        ls = (from enc in db.LoanTapeDefinitions.AsNoTracking()
                              select new LoanTapeDocFields()
                              {
                                  FieldId = enc.FieldID,
                                  FieldIDDescription = "#" + enc.FieldID + "# - " + enc.DataStream
                              }).ToList();
                        break;
                    default:
                        break;
                }
                if (ls != null && ls.Count > 0)
                {
                    System.Web.HttpContext.Current.Application["LoanTapeDocFields"] = ls;
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        public PasswordPolicy GetPasswordPolicy()
        {
            PasswordPolicy _passwordPolicy = null;

            using (var db = new DBConnect(SystemSchema))
            {
                _passwordPolicy = db.PasswordPolicy.AsNoTracking().FirstOrDefault();

            }
            return _passwordPolicy;

        }

        public PasswordPolicy SavePasswordPolicy(PasswordPolicy _passwordPolicy)
        {

            PasswordPolicy _pPolicy = null;
            using (var db = new DBConnect(SystemSchema))
            {
                _pPolicy = db.PasswordPolicy.AsNoTracking().FirstOrDefault();
                if (_pPolicy != null)
                {
                    _pPolicy.NoOfOldPassword = _passwordPolicy.NoOfOldPassword;
                    _pPolicy.StoreOldPassword = _passwordPolicy.StoreOldPassword;
                    _pPolicy.PasswordExpiryDays = _passwordPolicy.PasswordExpiryDays;
                    _pPolicy.ModifiedOn = DateTime.Now;
                    db.Entry(_pPolicy).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    db.PasswordPolicy.Add(new PasswordPolicy
                    {
                        PasswordExpiryDays = _passwordPolicy.PasswordExpiryDays,
                        StoreOldPassword = _passwordPolicy.StoreOldPassword,
                        NoOfOldPassword = _passwordPolicy.NoOfOldPassword,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now

                    });
                    db.SaveChanges();
                }
            }
            return _pPolicy;

        }

        public string GetLOSDocSearchFields(Int64 LOSFieldID)
        {

            using (var db = new DBConnect(SystemSchema))
            {
                return db.EncompassFields.AsNoTracking().Where(a => a.ID == LOSFieldID).Select(enc => "#" + enc.FieldID + "# - " + enc.FieldDesc).FirstOrDefault();
            }
        }

        public Dictionary<Int64, string> GetLOSDocSearchFields(Dictionary<Int64, Int64> _ChecklistDetails)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                //string _fields =  db.EncompassFields.AsNoTracking().Where(m => m.ID == ID).Select(l => l.FieldDesc).FirstOrDefault();
                Dictionary<Int64, string> _Checklist = new Dictionary<long, string>();
                foreach (var item in _ChecklistDetails)
                {
                    string value = db.EncompassFields.AsNoTracking().Where(a => a.ID == item.Value).Select(a => a.FieldDesc).FirstOrDefault();
                    value = string.IsNullOrEmpty(value) ? string.Empty : value;
                    _Checklist.Add(item.Key, value);
                }

                return _Checklist;
            }
        }


        #region KpiUserList 
        public object GetUserRoleList(Int64 RoleID)
        {
            List<UserRoleMapping> _userroles = new List<UserRoleMapping>();
            KpiUserGroupConfig _kpibuserGroupconfig = new KpiUserGroupConfig();
            List<KPIGoalConfig> _kpigoalconfig = new List<KPIGoalConfig>();
            object data = null;
            using (var db = new DBConnect(TenantSchema))
            {
                _userroles = db.UserRoleMapping.Where(x => x.RoleID == RoleID).ToList();
                _kpibuserGroupconfig = db.KpiUserGroupConfig.AsNoTracking().Where(x => x.RoleID == RoleID).FirstOrDefault();
                if (_kpibuserGroupconfig != null)
                {
                    data = (from kpigaol in db.KPIGoalConfig.AsNoTracking()
                                // join usrgrp in db.KpiUserGroupConfig.AsNoTracking() on kpigaol.UserGroupID equals usrgrp.GroupID
                            where kpigaol.UserGroupID == _kpibuserGroupconfig.GroupID
                            select new
                            {
                                UserID = kpigaol.UserID,
                                UserGroupID = kpigaol.UserGroupID,
                                PeriodFrom = kpigaol.PeriodFrom,
                                PeriodTo = kpigaol.PeriodTo,
                                Goal = kpigaol.Goal,
                                ID = kpigaol.ID,
                                FirstName = db.Users.Where(x => x.UserID == kpigaol.UserID).FirstOrDefault().FirstName,
                                LastName = db.Users.Where(x => x.UserID == kpigaol.UserID).FirstOrDefault().LastName,
                                CreatedOn = kpigaol.CreatedOn,
                                ModifiedOn = kpigaol.ModifiedOn
                            }).ToList();
                    //_kpigoalconfig = db.KPIGoalConfig.AsNoTracking().Where(x => x.UserGroupID == _kpibuserGroupconfig.GroupID).ToList();
                }

            }
            return new { UserList = _userroles, KpiUserGroup = _kpibuserGroupconfig, KpiGoalConfig = data };
        }
        public object SaveKpiConfigurationDetails(KpiUserGroupConfig _kpiUserGroupConfig, List<KPIGoalConfig> _kpiGoalConfig, bool IsExistNewUserGrp)
        {
            object data = null;
            using (var db = new DBConnect(TenantSchema))
            {
                // DateTime FromDate = new DateTime(_kpiUserGroupConfig.PeriodFrom.Year, _kpiUserGroupConfig.PeriodFrom.Month, _kpiUserGroupConfig.PeriodFrom.Day, 0, 0, 0);
                // DateTime ToDate = new DateTime(_kpiUserGroupConfig.PeriodTo.Year, _kpiUserGroupConfig.PeriodTo.Month, _kpiUserGroupConfig.PeriodTo.Day, 0, 0, 0).AddDays(1);
                KpiUserGroupConfig checkExistUserGrpData = db.KpiUserGroupConfig.Where(x => x.GroupID == _kpiUserGroupConfig.GroupID).FirstOrDefault();

                // Insert Previous Config Details (Previous KpiUserGroupConfig details)
                if (IsExistNewUserGrp == true)
                {
                    db.AuditKpiGoalConfig.Add(new AuditKpiGoalConfig
                    {
                        UserGroupID = checkExistUserGrpData.GroupID,
                        PeriodFrom = checkExistUserGrpData.PeriodFrom,
                        PeriodTo = checkExistUserGrpData.PeriodTo,
                        Goal = checkExistUserGrpData.Goal,
                        RoleID = checkExistUserGrpData.RoleID,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });
                    db.SaveChanges();

                    checkExistUserGrpData.Goal = _kpiUserGroupConfig.Goal;
                    checkExistUserGrpData.PeriodFrom = _kpiUserGroupConfig.PeriodFrom;
                    checkExistUserGrpData.PeriodTo = _kpiUserGroupConfig.PeriodTo;
                    checkExistUserGrpData.ModifiedOn = DateTime.Now;
                    db.Entry(checkExistUserGrpData).State = EntityState.Modified;
                    db.SaveChanges();
                    // Add Previous KpiGoal Config (Previous Record)
                    List<KPIGoalConfig> listkpiconfig = db.KPIGoalConfig.Where(x => x.UserGroupID == checkExistUserGrpData.GroupID).ToList();
                    foreach (KPIGoalConfig _kpi in listkpiconfig)
                    {
                        db.AuditUserKpiGoalConfig.Add(new AuditUserKpiGoalConfig
                        {
                            UserGroupID = _kpi.UserGroupID,
                            CreatedOn = DateTime.Now,
                            Goal = _kpi.Goal,
                            PeriodFrom = _kpi.PeriodFrom,
                            PeriodTo = _kpi.PeriodTo,
                            ModifiedOn = DateTime.Now,
                            UserID = _kpi.UserID
                        });
                        db.SaveChanges();
                    }
                    // New Record
                    foreach (KPIGoalConfig item in _kpiGoalConfig)
                    {
                        KPIGoalConfig existconfig = db.KPIGoalConfig.Where(x => x.UserID == item.UserID && x.UserGroupID == checkExistUserGrpData.GroupID).FirstOrDefault();
                        if (existconfig != null)
                        {
                            existconfig.PeriodFrom = item.PeriodFrom;
                            existconfig.PeriodTo = item.PeriodTo;
                            existconfig.Goal = item.Goal;
                            db.Entry(existconfig).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                }
                // Update KpiUserGroupConfig Goal Count
                else
                {

                    if (checkExistUserGrpData != null)
                    {

                        KpiUserGroupConfig existUserconfig = db.KpiUserGroupConfig.Where(x => x.GroupID == checkExistUserGrpData.GroupID).FirstOrDefault();
                        if (existUserconfig != null)
                        {
                            existUserconfig.Goal = _kpiUserGroupConfig.Goal;
                            db.Entry(existUserconfig).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        foreach (KPIGoalConfig item in _kpiGoalConfig)
                        {
                            KPIGoalConfig existconfig = db.KPIGoalConfig.Where(x => x.UserID == item.UserID && x.UserGroupID == checkExistUserGrpData.GroupID).FirstOrDefault();
                            if (existconfig != null)
                            {
                                existconfig.PeriodFrom = item.PeriodFrom;
                                existconfig.PeriodTo = item.PeriodTo;
                                existconfig.Goal = item.Goal;
                                db.Entry(existconfig).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                item.ID = 0;
                                item.UserGroupID = checkExistUserGrpData.GroupID;
                                item.CreatedOn = DateTime.Now;
                                item.ModifiedOn = DateTime.Now;
                                db.KPIGoalConfig.Add(item);
                                db.SaveChanges();
                            }
                        }
                    }
                    // Add New KpiUserGroupConfig And KpiGoalConfigDetaills
                    else
                    {
                        _kpiUserGroupConfig.CreatedOn = DateTime.Now;
                        _kpiUserGroupConfig.ModifiedOn = DateTime.Now;
                        KpiUserGroupConfig _KpiUserGroupDetails = db.KpiUserGroupConfig.Add(_kpiUserGroupConfig);
                        db.SaveChanges();

                        if (_KpiUserGroupDetails != null)
                        {
                            foreach (KPIGoalConfig kpidata in _kpiGoalConfig)
                            {
                                kpidata.UserGroupID = _KpiUserGroupDetails.GroupID;
                                kpidata.CreatedOn = DateTime.Now;
                                kpidata.ModifiedOn = DateTime.Now;
                                db.KPIGoalConfig.Add(kpidata);
                                db.SaveChanges();
                            }
                        }
                    }
                }

                data = (from x in db.KPIGoalConfig.AsNoTracking()
                        where x.UserGroupID == _kpiUserGroupConfig.GroupID
                        select new
                        {
                            ID = x.ID,
                            FirstName = db.Users.Where(u => u.UserID == x.UserID).FirstOrDefault().FirstName,
                            LastName = db.Users.Where(u => u.UserID == x.UserID).FirstOrDefault().LastName,
                            UserID = x.UserID,
                            Goal = x.Goal,
                            PeriodFrom = x.PeriodFrom,
                            PeriodTo = x.PeriodTo,
                            GroupID = x.UserGroupID,
                            CreatedOn = x.CreatedOn,
                            ModifiedOn = x.ModifiedOn
                        }).ToList();


            }
            return data;
        }

        public object SaveKpiConfigurationDetails(Int64 groupID, int configType, Int64 goal)
        {
            KPIConfigStaging KpiConfigDetails = null;
            object obj = null;
            List<KPIConfigStaging> _kpiStaging = new List<KPIConfigStaging>();
            using (var db = new DBConnect(TenantSchema))
            {

                KPIConfigStaging _KPIConfigStaging = null;
                _KPIConfigStaging = db.KPIConfigStaging.AsNoTracking().Where(k => k.GroupID == groupID && k.Status == false).FirstOrDefault();

                if (_KPIConfigStaging != null)
                {
                    //DateTime.Now.Date == _KPIConfigStaging.CreatedOn.Date;
                    if (_KPIConfigStaging.ConfigType == configType || (DateTime.Now.Day == _KPIConfigStaging.CreatedOn.Day))
                    {
                        _KPIConfigStaging.Goal = goal;
                        _KPIConfigStaging.ConfigType = configType;
                        _KPIConfigStaging.ModifiedOn = DateTime.Now;
                        db.Entry(_KPIConfigStaging).State = EntityState.Modified;
                        db.SaveChanges();
                        KpiConfigDetails = _KPIConfigStaging;
                    }
                    else
                    {
                        _KPIConfigStaging.Goal = goal;
                        _KPIConfigStaging.ModifiedOn = DateTime.Now;
                        _KPIConfigStaging.Status = true;
                        db.Entry(_KPIConfigStaging).State = EntityState.Modified;

                        KpiConfigDetails = db.KPIConfigStaging.Add(new KPIConfigStaging
                        {
                            GroupID = groupID,
                            ConfigType = configType,
                            Goal = goal,
                            Status = false,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        });
                        db.SaveChanges();
                    }
                    //db.Entry(_KPIConfigStaging).State = EntityState.Deleted;
                    //db.KPIConfigStaging.Remove(_KPIConfigStaging);
                    //db.SaveChanges();
                }
                else
                {
                    KpiConfigDetails = db.KPIConfigStaging.Add(new KPIConfigStaging
                    {
                        GroupID = groupID,
                        ConfigType = configType,
                        Goal = goal,
                        Status = false,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });
                    db.SaveChanges();
                }

                if (KpiConfigDetails != null)
                {
                    //_kpiStaging.Add(KpiConfigDetails);
                    List<User> _user = (from u in db.Users.AsNoTracking()
                                        join uR in db.UserRoleMapping.AsNoTracking() on u.UserID equals uR.UserID
                                        where uR.RoleID == groupID
                                        select u).ToList();

                    obj = (from x in _user
                           select new
                           {
                               UserID = x.UserID,
                               UserName = x.LastName + " " + x.FirstName,
                               Goal = KpiConfigDetails.Goal / _user.Count,
                               TotalGoals = KpiConfigDetails.Goal,
                               ConfigType = KpiConfigDetails.ConfigType,
                               CreatedOn = KpiConfigDetails.CreatedOn,
                               IsValid = true
                           }).ToList();
                }
            }
            return obj;
        }

        public bool UpdateKPIConfigStagingData(Int64 ID, Int64 groupID, int configType, Int64 goal)
        {

            using (var db = new DBConnect(TenantSchema))
            {
                KPIConfigStaging _config = db.KPIConfigStaging.AsNoTracking().Where(k => k.ID == ID).FirstOrDefault();
                if (_config != null)
                {
                    _config.ConfigType = configType;
                    _config.Goal = goal;
                    _config.ModifiedOn = DateTime.Now;
                    db.Entry(_config).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }

            }
            return false;
        }

        public object GetKPIGoalConfigStagingDetails(Int64 groupID, int configType)
        {
            KPIConfigStaging KpiConfigDetails = null;
            object obj = null;
            using (var db = new DBConnect(TenantSchema))
            {
                KpiConfigDetails = db.KPIConfigStaging.AsNoTracking().Where(k => k.GroupID == groupID && k.Status == false).OrderByDescending(a => a.ID).FirstOrDefault();
                List<User> _user = (from u in db.Users.AsNoTracking()
                                    join uR in db.UserRoleMapping.AsNoTracking() on u.UserID equals uR.UserID
                                    where uR.RoleID == groupID
                                    select u).ToList();
                if (KpiConfigDetails != null && _user.Count > 0 && (KpiConfigDetails.Goal % _user.Count == 0))
                {
                    obj = (from x in _user
                           select new
                           {
                               UserID = x.UserID,
                               UserName = x.LastName + " " + x.FirstName,
                               Goal = KpiConfigDetails.Goal / _user.Count,
                               TotalGoals = KpiConfigDetails.Goal,
                               ConfigType = KpiConfigDetails.ConfigType,
                               CreatedOn = KpiConfigDetails.CreatedOn,
                               IsValid = true
                           }).ToList();
                }
                else if (KpiConfigDetails != null && _user.Count > 0)
                {
                    obj = (from x in _user
                           select new
                           {
                               UserID = x.UserID,
                               UserName = x.LastName + " " + x.FirstName,
                               Goal = KpiConfigDetails.Goal / _user.Count,
                               ConfigType = KpiConfigDetails.ConfigType,
                               TotalGoals = KpiConfigDetails.Goal,
                               CreatedOn = KpiConfigDetails.CreatedOn,
                               IsValid = false
                           }).ToList();
                }
                else
                {
                    obj = (from x in _user
                           select new
                           {
                               UserID = x.UserID,
                               UserName = x.LastName + " " + x.FirstName,
                               Goal = 0,
                               TotalGoals = 0,
                               ConfigType = 0,
                               CreatedOn = x.CreatedOn,
                               IsValid = true
                           }).ToList();
                }
            }
            return obj;
        }

        #endregion

        #region ImportStagingDetail

        public LOSImportStaging GetLOSImportStagingDetail(Int64 importStagingID)
        {
            LOSImportStaging importStaging = null;
            using (var db = new DBConnect(TenantSchema))
            {
                importStaging = db.LOSImportStaging.AsNoTracking().Where(x => x.ID == importStagingID).FirstOrDefault();
            }
            return importStaging;
        }

        #endregion
    }

    public class ChecklistDetailOutput
    {
        public Int64 CheckListDetailID { get; set; }
        public bool ChecklistActive { get; set; }
        public Int64 RuleID { get; set; }
        public Int64 ChecklistGroupId { get; set; }
        public string CheckListName { get; set; }
        public string CheckListDescription { get; set; }
        public string Category { get; set; }
        public DateTime CreatedOn { get; set; }
        public string RuleDescription { get; set; }
        public string RuleJson { get; set; }
        public string DocumentType { get; set; }
        public Int64? UserID { get; set; }
        public Int64 SequenceID { get; set; }
        public string DocVersion { get; set; }
        public string LOSFieldDescription { get; set; }
        public Int64 LOSFieldToEvalRule { get; set; }
        public string LOSValueToEvalRule { get; set; }
        public int RuleType { get; set; }
        public int? LosIsMatched { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LoanType { get; set; }
    }

    public class EFieldResponse
    {
        [JsonProperty(PropertyName = "fieldId")]
        public string FieldId { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

    }

    public class EToken
    {
        public string accessToken { get; set; }
        public string tokenType { get; set; }
    }
    public class DocumentTypeMasterList
    {
        public Int64 DocumentTypeID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool Active { get; set; }
        public Int32 DocumentLevel { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Int64? ParkingSpotID { get; set; }
        public String ParkingSpotName { get; set; }
    }
}
