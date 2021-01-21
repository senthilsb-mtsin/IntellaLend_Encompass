
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using DataTable = System.Data.DataTable;

namespace IL.LenderImport
{
    public class LenderImportDataAccess
    {
        public static string SystemSchema = "IL";
        public string TenantSchema;

        public LenderImportDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }
        public static List<TenantMaster> GetAllTenants()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.TenantMaster.AsNoTracking().Where(m => m.Active == true).ToList();
            }
        }
        public CustomerMaster CheckCustomerExists(string _lenderName, string _lenderCode)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                CustomerMaster cust = db.CustomerMaster.AsNoTracking().Where(x => x.CustomerName.Equals(_lenderName)).FirstOrDefault();

                if (cust != null)
                {
                    if (string.IsNullOrEmpty(cust.CustomerCode))
                    {
                        CustomerMaster cm = db.CustomerMaster.AsNoTracking().Where(x => x.CustomerCode.Equals(_lenderCode)).FirstOrDefault();
                        if (cm != null)
                            throw new Exception($"Lender : {_lenderName} and LenderCode : {_lenderCode} are mismatch");

                        return cust;
                    }
                    else
                    {
                        if (cust.CustomerCode == _lenderCode)
                        {
                            return cust;
                        }
                        else
                        {
                            throw new Exception($"Lender : {_lenderName} and LenderCode : {_lenderCode} are mismatch");
                        }
                    }
                }
                else
                {
                    CustomerMaster cm = db.CustomerMaster.AsNoTracking().Where(x => x.CustomerCode.Equals(_lenderCode)).FirstOrDefault();
                    if (cm == null)
                        return null;
                }

            }
            throw new Exception($"Error while checking Lender : {_lenderName} and LenderCode : {_lenderCode}");

        }
        public Int64 InsertCustomerDetails(CustomerImportStagingDetail _lenderDetail)
        {
            using (var db = new DBConnect(TenantSchema))
            {

                if (_lenderDetail != null)
                {
                    CustomerMaster _import = new CustomerMaster()
                    {
                        CustomerName = _lenderDetail.CustomerName,
                        CustomerCode = _lenderDetail.CustomerCode,
                        ZipCode = _lenderDetail.Zip,
                        Country = _lenderDetail.Country,
                        State = _lenderDetail.State,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };
                    db.CustomerMaster.Add(_import);
                    db.SaveChanges();
                    return _import.CustomerID;

                }

            }
            return 0;
        }
        public ReviewTypeMaster CheckReviewTypeExists(string _serviceType)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.ReviewTypeMaster.AsNoTracking().Where(m => m.ReviewTypeName == _serviceType).FirstOrDefault();
            }
        }

        public LoanTypeMaster CheckLoanTypeExists(string loanType)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.LoanTypeMaster.AsNoTracking().Where(m => m.LoanTypeName == loanType).FirstOrDefault();
            }
        }
        public bool CheckMappingExists(Int64 _customerID, Int64 _reviewTypeID, Int64 _loanTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.CustReviewLoanMapping.AsNoTracking().Where(m => m.CustomerID == _customerID && m.ReviewTypeID == _reviewTypeID && m.LoanTypeID == _loanTypeID && m.Active == true).Any();
            }
        }
        public bool CustReviewMapping(Int64 _customerID, Int64 _reviewTypeID, Int64 _loanTypeID)
        {
            bool _cusReviewMapping = false;
            using (var db = new DBConnect(TenantSchema))
            {
                ReviewTypeMaster _rt = db.ReviewTypeMaster.AsNoTracking().Where(m => m.ReviewTypeID == _reviewTypeID).FirstOrDefault();

                if (_rt == null)
                {
                    SystemReviewTypeMaster tempRt = db.SystemReviewTypeMaster.AsNoTracking().Where(m => m.ReviewTypeID == _reviewTypeID).FirstOrDefault();
                    _rt = new ReviewTypeMaster();
                    _rt.ReviewTypeID = tempRt.ReviewTypeID;
                    _rt.ReviewTypeName = tempRt.ReviewTypeName;
                    _rt.ReviewTypePriority = tempRt.ReviewTypePriority;
                    _rt.Type = tempRt.Type;
                    _rt.UserRoleID = tempRt.UserRoleID;
                    _rt.SearchCriteria = tempRt.SearchCriteria;
                    _rt.ModifiedOn = tempRt.ModifiedOn;
                    _rt.Active = tempRt.Active;
                    db.ReviewTypeMaster.Add(_rt);
                }

                if (!db.CustReviewMapping.AsNoTracking().Any(x => x.CustomerID == _customerID && x.ReviewTypeID == _reviewTypeID))
                {
                    db.CustReviewMapping.Add(new CustReviewMapping()
                    {
                        CustomerID = _customerID,
                        ReviewTypeID = _reviewTypeID,
                        Active = true,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });

                    db.SaveChanges();
                }

                _cusReviewMapping = true;

            }
            return _cusReviewMapping;
        }
        public bool CustReviewLoanMapping(Int64 _customerID, Int64 _reviewTypeID, Int64 _loanTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.CustReviewLoanMapping.AsNoTracking().Any(x => x.CustomerID == _customerID && x.ReviewTypeID == _reviewTypeID && x.LoanTypeID == _loanTypeID);
            }
        }


        public Int64 UpdateLenderImportStagingTable(string _filePath, Int32 _status, Int64 _importCount, string _errorMSG = "")
        {
            using (var db = new DBConnect(TenantSchema))
            {
                CustomerImportStaging _import = new CustomerImportStaging()
                {
                    FilePath = _filePath,
                    Status = _status,
                    ImportCount = _importCount,
                    ErrorMsg = _errorMSG,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    AssignType = LenderAssignTypeStatusConstant.LENDER_IMPORT
                };

                db.CustomerImportStaging.Add(_import);
                db.SaveChanges();

                return _import.ID;
            }
        }
        public void UpdateLenderStagingStatus(Int64 _importStagingID, Int32 _status, string _errorMessage = "")
        {
            using (var db = new DBConnect(TenantSchema))
            {
                CustomerImportStaging _importStage = db.CustomerImportStaging.AsNoTracking().Where(fs => fs.ID == _importStagingID).FirstOrDefault();

                if (_importStage != null)
                {
                    _importStage.Status = _status;
                    _importStage.ModifiedOn = DateTime.Now;
                    _importStage.ErrorMsg = _errorMessage;
                    db.Entry(_importStage).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public string GetLastErrorMessage(Int64 _importStagingID, Int32 _status)
        {
            string lastErrMsg = string.Empty;
            using (var db = new DBConnect(TenantSchema))
            {
                CustomerImportStagingDetail cisd = db.CustomerImportStagingDetail.AsNoTracking().Where(fs => (fs.CustomerImportStagingID == _importStagingID) && (fs.Status == _status)).OrderByDescending(fs => fs.ID).FirstOrDefault();
                if (cisd != null)
                {
                    lastErrMsg = cisd.ErrorMsg;
                }
            }
            return lastErrMsg;
        }

        public bool CheckLenderImportStagingDetailStatus(Int64 _importStagingID, Int32 _status)
        {
            bool isPartiallyProcessedOrError = false;
            using (var db = new DBConnect(TenantSchema))
            {
                isPartiallyProcessedOrError = db.CustomerImportStagingDetail.AsNoTracking().Where(fs => fs.CustomerImportStagingID == _importStagingID).Any(x => x.Status == _status);
            }

            return isPartiallyProcessedOrError;

        }
        public void UpdateLenderImportStagingDetailStatus(Int64 _importStagingID, Int32 _status, string _errorMessage = "")
        {
            using (var db = new DBConnect(TenantSchema))
            {
                CustomerImportStagingDetail _importStage = db.CustomerImportStagingDetail.AsNoTracking().Where(fs => fs.ID == _importStagingID).FirstOrDefault();

                if (_importStage != null)
                {
                    _importStage.Status = _status;
                    _importStage.ModifiedOn = DateTime.Now;
                    _importStage.ErrorMsg = _errorMessage;
                    db.Entry(_importStage).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

        }
        public bool CheckMappingExists(Int64 _reviewTypeID, Int64 _loanTypeID)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.CustReviewLoanMapping.AsNoTracking().Where(m => m.CustomerID == 1 && m.ReviewTypeID == _reviewTypeID && m.LoanTypeID == _loanTypeID && m.Active == true).Any();
            }
        }
        public void InsertRetainUpdateStagingDetails(Int64 _loanTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                RetainUpdateStaging _retainUpdate = new RetainUpdateStaging()
                {
                    UserID = 1,
                    LoanTypeID = _loanTypeID,
                    SyncLevel = SynchronizeConstant.AllSync,
                    Synchronized = SynchronizeConstant.Staged,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };

                db.RetainUpdateStaging.Add(_retainUpdate);
                db.SaveChanges();
            }
        }

        public bool SaveCustReviewLoanMapping(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID, string BoxUploadPath, string LoanUploadPath)
        {
            bool result = false;

            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    db.CustReviewLoanMapping.RemoveRange(db.CustReviewLoanMapping.Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == LoanTypeID));
                    db.SaveChanges();

                    db.CustReviewMapping.RemoveRange(db.CustReviewMapping.Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID));
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
                    List<DocumentTypeMaster> _lsDocumentTypeMaster = _ILDataAccess.GetSystemDocumentTypesWithAllFields(LoanTypeID);

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

                    db.CustReviewMapping.Add(new CustReviewMapping()
                    {
                        CustomerID = CustomerID,
                        ReviewTypeID = ReviewTypeID,
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

        public void updatecustreviewloanmapping(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                CustReviewLoanMapping _custReviewLoanMap = db.CustReviewLoanMapping.AsNoTracking().Where(c => c.ReviewTypeID == ReviewTypeID && c.LoanTypeID == LoanTypeID && c.CustomerID == CustomerID).FirstOrDefault();

                if (_custReviewLoanMap != null && !_custReviewLoanMap.Active)
                {
                    _custReviewLoanMap.Active = true;
                    _custReviewLoanMap.ModifiedOn = DateTime.Now;
                    db.Entry(_custReviewLoanMap).State = EntityState.Modified;
                    db.SaveChanges();
                }

                CustReviewMapping _custReviewMap = db.CustReviewMapping.AsNoTracking().Where(c => c.ReviewTypeID == ReviewTypeID && c.CustomerID == CustomerID).FirstOrDefault();

                if (_custReviewMap != null && !_custReviewMap.Active)
                {
                    _custReviewMap.Active = true;
                    _custReviewMap.ModifiedOn = DateTime.Now;
                    db.Entry(_custReviewMap).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public void SetCustLoanDocumentStacking(DBConnect db, Int64 CustomerID, Int64 LoanTypeID, IntellaLendDataAccess _ILDataAccess, List<StackingOrderDetailMaster> _stackDetails, List<DocumentTypeMaster> _lsTenantDocs)
        {
            List<DocumentTypeMaster> _sysDocs = _ILDataAccess.GetAllSysDocTypeMasters();

            List<DocumentTypeMaster> _lsDocs = _sysDocs.Where(s => (_stackDetails.Any(d => d.DocumentTypeID == s.DocumentTypeID))).ToList();

            List<DocumentTypeMaster> _mappedDocs = _lsTenantDocs; //GetMappedDocuments(db, CustomerID, LoanTypeID);

            List<DocumentTypeMaster> _unMappedDocs = _lsDocs.Where(s => !(_mappedDocs.Any(m => m.Name == s.Name))).ToList();

            if (_unMappedDocs != null)
                MapCustLoanDocuments(db, CustomerID, LoanTypeID, _unMappedDocs);
        }


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

                    if (_documentTypeMaster != null)
                    {
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



        public void InsertCustomerStagingDetails(Int64 _importstagingID, DataTable _lenderDetails, Int32 _status, string ErrorMsg)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                foreach (DataRow lenderDetails in _lenderDetails.Rows)
                {

                    CustomerImportStagingDetail _customerStaging = new CustomerImportStagingDetail()
                    {
                        CustomerImportStagingID = _importstagingID,
                        CustomerName = lenderDetails["LenderName"].ToString(),
                        CustomerCode = lenderDetails["LenderCode"].ToString(),
                        Zip = lenderDetails["Zip"].ToString(),
                        Country = lenderDetails["Country"].ToString(),
                        State = lenderDetails["State"].ToString(),
                        Status = _status,
                        ErrorMsg = ErrorMsg,
                        ServiceType = lenderDetails["ServiceTypeName"].ToString(),
                        LoanType = lenderDetails["LoanTypeName"].ToString(),
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };

                    db.CustomerImportStagingDetail.Add(_customerStaging);
                    db.SaveChanges();

                }
            }
        }

        public List<CustomerImportStaging> GetServiceCustomerImportStagings()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.CustomerImportStaging.AsNoTracking().Where(c => c.Status == LenderImportStatusConstant.LENDER_IMPORT_STAGED).ToList();
            }
        }

        public List<CustomerImportStagingDetail> GetServiceCustomerImportStagingDetails(Int64 customerImportStagingID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.CustomerImportStagingDetail.AsNoTracking().Where(c => c.CustomerImportStagingID == customerImportStagingID  &&  c.Status == LenderImportStatusConstant.LENDER_IMPORT_STAGED).ToList();
            }
        }
    }
}

