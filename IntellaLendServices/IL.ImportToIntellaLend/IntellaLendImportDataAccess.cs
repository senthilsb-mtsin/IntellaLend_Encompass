using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.MinIOWrapper;
using IntellaLend.Model;
using MTSEntBlocks.LoggerBlock;
using MTSEntBlocks.UtilsBlock;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace IL.ImportToIntellaLend
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

        #region Public Methods
        //Update Document id and field id
        public void UpdateDocumentTypeAndFieldType(Batch batchObj)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = GetLoanInfo(batchObj.LoanID, db);

                var docList = GetCustLoanDocTypes(db, _loan.CustomerID, _loan.LoanTypeID);

                foreach (var document in batchObj.Documents)
                {
                    var docID = docList.Where(m => m.Name == document.Type).ToList();
                    if (docID.Count > 0)
                        document.DocumentTypeID = docID[0].DocumentTypeID;

                    var fieldList = db.DocumentFieldMaster.AsNoTracking().Where(m => m.DocumentTypeID == document.DocumentTypeID).ToList();
                    foreach (DocumentLevelFields field in document.DocumentLevelFields)
                    {
                        var fieldID = fieldList.Where(m => m.Name == field.Name).ToList();
                        if (fieldID.Count > 0)
                            field.FieldID = fieldID[0].FieldID;
                    }
                }
            }
        }

        public bool CheckLoanType(Int64 loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(m => m.LoanID == loanID).FirstOrDefault();
                if (_loan != null)
                    return (_loan.LoanTypeID != 0);
            }

            return false;
        }
        public string GetReviewTypeName(Int64 loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(m => m.LoanID == loanID).FirstOrDefault();
                if (_loan != null)
                {
                    ReviewTypeMaster _reviewType = db.ReviewTypeMaster.AsNoTracking().Where(r => r.ReviewTypeID == _loan.ReviewTypeID).FirstOrDefault();
                    if (_reviewType != null)
                        return _reviewType.ReviewTypeName;
                }
            }

            return string.Empty;
        }

        public bool SetLoanType(Int64 loanID, string loanTypeName)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    Loan _loan = db.Loan.AsNoTracking().Where(m => m.LoanID == loanID).FirstOrDefault();
                    LoanTypeMaster _loanType = db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeName == loanTypeName).FirstOrDefault();
                    if (_loan != null && _loanType != null)
                    {
                        _loan.LoanTypeID = _loanType.LoanTypeID;
                        _loan.ModifiedOn = DateTime.Now;

                        db.Entry(_loan).State = EntityState.Modified;
                        db.SaveChanges();
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.LOANTYPE_SET_BY_SYSTEM);
                        LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0].Replace(AuditConfigConstant.LOANTYPENAME, loanTypeName), auditDescs[1].Replace(AuditConfigConstant.LOANTYPENAME, loanTypeName));
                        tran.Commit();
                        return true;
                    }
                }
            }

            return false;
        }


        public bool CheckDeletedLoans(Int64 loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == loanID && l.Status == StatusConstant.LOAN_DELETED).FirstOrDefault();
                return (_loan == null);
            }
        }
        public string GetLoanObjects(Int64 loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                var loanDetail = db.LoanDetail.AsNoTracking().Where(m => m.LoanID == loanID).FirstOrDefault();
                if (loanDetail != null)
                    return loanDetail.LoanObject;
            }

            return null;
        }

        public Loan GetLoanInfo(Int64 loanID, DBConnect db)
        {
            var loanList = db.Loan.AsNoTracking().Where(m => m.LoanID == loanID).ToList();
            if (loanList.Count > 0)
                return loanList[0];

            return null;
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

        public void InsertDocumentImages(Int64 loanID, Int64 documentTypeID, int pageNo, byte[] imageBytes, int version, DBConnect db)
        {
            ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(db.TenantSchema);

            LoanImage _loanImage = new LoanImage()
            {
                LoanID = loanID,
                DocumentTypeID = documentTypeID,
                PageNo = pageNo,
                // Image = new byte[0],
                Version = version.ToString(),
                CreatedOn = DateTime.Now
            };

            _imageWrapper.InsertLoanImage(loanID, imageBytes, _loanImage);

        }

        public void UpdateLoanStatus(Int64 LoanID, Int64 status)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.Where(x => x.LoanID == LoanID).FirstOrDefault();
                if (_loan != null)
                {
                    _loan.Status = status;
                    _loan.ModifiedOn = DateTime.Now;
                    db.Entry(_loan).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
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

        public void UpdateLoanStatus(Int64 LoanID, Int64 status, Int32 errorCode, bool isMissingDocument, Int64 _docID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                if (!isMissingDocument)
                {
                    Loan _loan = db.Loan.Where(x => x.LoanID == LoanID).FirstOrDefault();
                    var _loansearch = db.LoanSearch.Where(x => x.LoanID == LoanID).FirstOrDefault();
                    if (_loan != null)
                    {
                        _loan.SubStatus = errorCode;
                        _loan.Status = status;
                        _loan.ModifiedOn = DateTime.Now;
                        db.Entry(_loan).State = EntityState.Modified;
                        db.SaveChanges();
                        if (_loansearch != null)
                        {
                            _loansearch.Status = status;
                            _loansearch.ModifiedOn = DateTime.Now;
                            db.Entry(_loansearch).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        if (_loan.SubStatus == ErrorCodeConstant.FAILED_TO_IMPORT)
                            LoanAudit.InsertLoanAudit(db, _loan, "failed to import", "");
                    }

                }
                else
                {
                    AuditLoanMissingDoc _auditDoc = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == LoanID && l.DocID == _docID).FirstOrDefault();

                    if (_auditDoc != null)
                    {
                        _auditDoc.Status = StatusConstant.IDCERROR;
                        _auditDoc.ModifiedOn = DateTime.Now;
                        db.Entry(_auditDoc).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
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

        public void RemoveLoanEntries(Int64 LoanID, bool isMissingDocument, Int64 _docID, Int64 _eUploadID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                if (isMissingDocument)
                {
                    AuditLoanMissingDoc _auditDoc = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == LoanID && l.DocID == _docID).FirstOrDefault();

                    if (_auditDoc != null)
                    {
                        _auditDoc.Status = StatusConstant.IDCERROR;
                        _auditDoc.ModifiedOn = DateTime.Now;
                        db.Entry(_auditDoc).State = EntityState.Modified;
                        db.ELoanAttachmentUpload.RemoveRange((db.ELoanAttachmentUpload.Where(x => x.ID == _eUploadID)));
                        db.SaveChanges();
                    }
                }
                else
                {
                    db.AuditLoanDetail.RemoveRange(db.AuditLoanDetail.Where(x => x.LoanID == LoanID));
                    db.AuditLoanMissingDoc.RemoveRange(db.AuditLoanMissingDoc.Where(x => x.LoanID == LoanID));
                    db.AuditLoanSearch.RemoveRange(db.AuditLoanSearch.Where(x => x.LoanID == LoanID));
                    db.LoanDetail.RemoveRange(db.LoanDetail.Where(x => x.LoanID == LoanID));
                    db.LoanPDF.RemoveRange(db.LoanPDF.Where(x => x.LoanID == LoanID));
                    db.LoanReverification.RemoveRange(db.LoanReverification.Where(x => x.LoanID == LoanID));
                    db.LoanSearch.RemoveRange(db.LoanSearch.Where(x => x.LoanID == LoanID));
                    db.LoanImage.RemoveRange(db.LoanImage.Where(x => x.LoanID == LoanID));
                    db.ELoanAttachmentUpload.RemoveRange((db.ELoanAttachmentUpload.Where(x => x.ID == _eUploadID)));
                    db.SaveChanges();
                }


            }
        }

        public void UpdateLoanDetails(Batch batchObj, Batch existingLoan, string EphesoftOutputPath, string ImageMaxHeight, string ImageMaxWidth, string DocID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {

                    MissingDocInsertImagesToDB(batchObj, db, EphesoftOutputPath, ImageMaxHeight, ImageMaxWidth);

                    //Prakash - Missing document dont need to update loan search table(Senthil)
                    //Loan _loan = UpdateLoanSearchFields(batchObj.LoanID, batchObj, db);
                    Loan _loan = GetLoanInfo(batchObj.LoanID, db);

                    LoanDetail lDetails = db.LoanDetail.AsNoTracking().Where(ld => ld.LoanID == batchObj.LoanID).FirstOrDefault();

                    if (lDetails != null)
                    {
                        lDetails.LoanObject = JsonConvert.SerializeObject(existingLoan);
                        lDetails.ModifiedOn = DateTime.Now;
                        db.Entry(lDetails).State = EntityState.Modified;
                        db.SaveChanges();
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.LOAN_DETAIL_UPDATED_BY_SYSTEM);
                        LoanAudit.InsertLoanDetailsAudit(db, lDetails, 0, auditDescs[0], auditDescs[1]);
                    }


                    //Update OCR Confidence
                    //Loan loan = db.Loan.AsNoTracking().Where(l => l.LoanID == batchObj.LoanID).FirstOrDefault();

                    //Prakash : No need to update IDCFields for Missing Document, since it is changed to new approach. We update only AuditLoanMissingDoc status
                    //if (_loan != null)
                    //{
                    //    IDCFields _idcField = db.IDCFields.AsNoTracking().Where(l => l.LoanID == _loan.LoanID).FirstOrDefault();
                    //    if (_idcField != null)
                    //    {
                    //        _idcField.IDCOCRAccuracy = batchObj.Confidence;
                    //        _idcField.IDCExtractionAccuracy = batchObj.BatchExtractionAccuracy;
                    //        _idcField.ModifiedOn = DateTime.Now;
                    //        db.Entry(_idcField).State = EntityState.Modified;
                    //        db.SaveChanges();
                    //    }
                    //    else {
                    //        _idcField = new IDCFields();
                    //        _idcField.IDCOCRAccuracy = batchObj.Confidence;
                    //        _idcField.IDCExtractionAccuracy = batchObj.BatchExtractionAccuracy;
                    //        _idcField.ModifiedOn = DateTime.Now;
                    //        _idcField.Createdon = DateTime.Now;
                    //        db.IDCFields.Add(_idcField);
                    //        db.SaveChanges();
                    //    }
                    //    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.LOAN_MODIFIED);

                    //    LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0], auditDescs[1]);
                    //}
                    Int64 _docID = 0;

                    Int64.TryParse(DocID, out _docID);

                    string fileName = $"{TenantSchema.ToUpper()}_{batchObj.LoanID.ToString()}_{DocID.ToString()}";

                    AuditLoanMissingDoc _auditDoc = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == batchObj.LoanID && l.DocID == _docID).FirstOrDefault();

                    if (_auditDoc == null)
                        _auditDoc = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == batchObj.LoanID && l.FileName.Contains(fileName)).FirstOrDefault();

                    if (_auditDoc != null)
                    {
                        _auditDoc.Status = StatusConstant.IDC_COMPLETE;
                        _auditDoc.ModifiedOn = DateTime.Now;
                        db.Entry(_auditDoc).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    trans.Commit();
                }
            }

        }

        private LoanDetail GetLoanDetails(DBConnect db, Int64 LoanID)
        {
            return db.LoanDetail.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
        }

        public Int32 GetLoanUploadType(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault().UploadType;
            }
        }


        public void InsertLoanDetails(Batch batchObj, string EphesoftOutputPath, string ImageMaxHeight, string ImageMaxWidth)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    LogMessage($"Start Loan Images Insert : {batchObj.LoanID.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
                    Int64 _pageCount = InsertImagesToDB(batchObj, db, EphesoftOutputPath, ImageMaxHeight, ImageMaxWidth);
                    LogMessage($"End Loan Images Insert : {batchObj.LoanID.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");

                    Loan _loan = UpdateLoanSearchFields(batchObj, db);

                    Logger.WriteTraceLog($"Get loanDetails Start time : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
                    LoanDetail loanDetails = GetLoanDetails(db, _loan.LoanID);
                    Logger.WriteTraceLog($"Get loanDetails End time : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");

                    if (loanDetails == null)
                    {
                        loanDetails = new LoanDetail()
                        {
                            LoanID = batchObj.LoanID,
                            LoanObject = JsonConvert.SerializeObject(batchObj),
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        };
                        Logger.WriteTraceLog($"Insert loanDetails Start time : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
                        db.LoanDetail.Add(loanDetails);
                        db.SaveChanges();
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.LOAN_DETAIL_INSERTED);
                        LoanAudit.InsertLoanDetailsAudit(db, loanDetails, 0, auditDescs[0], auditDescs[1]);

                    }

                    //Update OCR Confidence
                    //Loan loan = db.Loan.AsNoTracking().Where(l => l.LoanID == batchObj.LoanID).FirstOrDefault();
                    if (_loan != null)
                    {

                        IDCFields _idcField = db.IDCFields.AsNoTracking().Where(l => l.LoanID == _loan.LoanID).FirstOrDefault();
                        if (_idcField != null)
                        {
                            _idcField.IDCOCRAccuracy = batchObj.Confidence;
                            _idcField.ModifiedOn = DateTime.Now;
                            db.Entry(_idcField).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            _idcField = new IDCFields();
                            _idcField.IDCOCRAccuracy = batchObj.Confidence;
                            _idcField.OCRAccuracyCalculated = false;
                            _idcField.ModifiedOn = DateTime.Now;
                            _idcField.Createdon = DateTime.Now;
                            db.IDCFields.Add(_idcField);
                            db.SaveChanges();
                        }
                        _loan.PageCount = _pageCount;
                        _loan.ModifiedOn = DateTime.Now;
                        db.Entry(_loan).State = EntityState.Modified;
                        db.SaveChanges();
                        string[] auditDescss = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.LOAN_MODIFIED);
                        LoanAudit.InsertLoanAudit(db, _loan, auditDescss[0], auditDescss[1]);
                    }
                    trans.Commit();
                }
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

        public List<StackingOrderDetailMaster> GetStackingOrderInfo(Int64 stackingOrderId)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.StackingOrderDetailMaster.AsNoTracking().Where(m => m.StackingOrderID == stackingOrderId).OrderBy(m => m.SequenceID).ToList();
            }
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
        public List<LoanImage> GetLoanImages(Int64 loanId, Int64 documentTypeId)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                var imageList = db.LoanImage.AsNoTracking().Where(m => (m.LoanID == loanId) && (m.DocumentTypeID == documentTypeId)).OrderByDescending(x => x.Version).ThenBy(i => i.PageNo).ToList();
                // var maxItem = imageList.OrderByDescending(x => Convert.ToInt32(x.Version)).FirstOrDefault();
                return imageList;
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

        #endregion

        #region Private Methods

        private Loan UpdateLoanSearchFields(Batch batchObj, DBConnect db)
        {
            Logger.WriteTraceLog("Start UpdateLoanSearchFields");
            var loanDetails = GetLoanInfo(batchObj.LoanID, db);
            Logger.WriteTraceLog("After loanDetails");
            if (loanDetails != null)
            {
                string LoanNumber = string.Empty;
                string BorrowerName = string.Empty;
                string SSN = string.Empty;
                decimal LoanAmount = 0m;
                string propertyAddress = string.Empty;
                foreach (var document in batchObj.Documents)
                {
                    Logger.WriteTraceLog($"Document : {document.Type}");
                    if (document.DocumentLevelFields != null)
                    {
                        foreach (var field in document.DocumentLevelFields)
                        {
                            switch (field.Name.ToUpper())
                            {
                                //case "LOANNUMBER":
                                case "LOAN NUMBER":
                                    {
                                        if (!LoanNumber.Equals(string.Empty))
                                            break;

                                        LoanNumber = field.Value;
                                        break;
                                    }
                                case "LOAN NO":
                                    {
                                        if (!LoanNumber.Equals(string.Empty))
                                            break;

                                        LoanNumber = field.Value;
                                        break;
                                    }
                                case "PROPERTY ADDRESS":
                                    {
                                        if (!propertyAddress.Equals(string.Empty))
                                            break;

                                        propertyAddress = field.Value;
                                        break;
                                    }
                                case "BORROWER NAME":
                                    {
                                        if (!BorrowerName.Equals(string.Empty))
                                            break;

                                        BorrowerName = field.Value;
                                        break;
                                    }
                                case "LOAN AMOUNT":
                                    {
                                        if (LoanAmount != 0m)
                                            break;

                                        //LoanAmount = Convert.ToDecimal(field.Value);
                                        decimal.TryParse(field.Value.Replace("%", "").Replace("$", "").Replace(",", "").Trim(), out LoanAmount);
                                        break;
                                    }
                                case "SSN":
                                    {
                                        if (!SSN.Equals(string.Empty))
                                            break;

                                        SSN = field.Value;
                                        break;
                                    }
                            }
                        }
                    }
                }
                Logger.WriteTraceLog($"Before GetLoanSearch");
                LoanSearch loanSearch = GetLoanSearch(batchObj.LoanID, db);
                //{
                //    LoanID = batchObj.LoanID,
                //    LoanTypeID = loanDetails.LoanTypeID,
                //    LoanNumber = LoanNumber,
                //    BorrowerName = BorrowerName,
                //    ReceivedDate = loanDetails.CreatedOn,
                //    LoanAmount = LoanAmount,
                //    Status = loanDetails.Status,
                //    SSN = SSN,
                //    //AuditMonthYear
                //    CustomerID = loanDetails.CustomerID,
                //    CreatedOn = DateTime.Now
                //};
                if (loanSearch != null)
                {
                    Logger.WriteTraceLog($"Loan Search Already Exists");
                    loanSearch.LoanID = batchObj.LoanID;
                    loanSearch.LoanTypeID = loanDetails.LoanTypeID;

                    if (string.IsNullOrEmpty(loanDetails.LoanNumber))
                        loanSearch.LoanNumber = LoanNumber;

                    loanSearch.BorrowerName = loanDetails.UploadType != UploadConstant.ENCOMPASS ? BorrowerName : loanSearch.BorrowerName;
                    loanSearch.ReceivedDate = loanDetails.CreatedOn;
                    loanSearch.LoanAmount = LoanAmount;
                    loanSearch.Status = loanDetails.Status;
                    loanSearch.PropertyAddress = propertyAddress;
                    loanSearch.SSN = SSN;
                    loanSearch.CustomerID = loanDetails.CustomerID;
                    loanSearch.CreatedOn = DateTime.Now;
                    db.Entry(loanSearch).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    Logger.WriteTraceLog($"Loan Search Not Already Exists");
                    loanSearch = new LoanSearch()
                    {
                        LoanID = batchObj.LoanID,
                        LoanTypeID = loanDetails.LoanTypeID,
                        LoanNumber = string.IsNullOrEmpty(loanDetails.LoanNumber) ? LoanNumber : loanDetails.LoanNumber,
                        BorrowerName = BorrowerName,
                        ReceivedDate = loanDetails.CreatedOn,
                        LoanAmount = LoanAmount,
                        Status = loanDetails.Status,
                        SSN = SSN,
                        CustomerID = loanDetails.CustomerID,
                        PropertyAddress = propertyAddress,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };
                    db.LoanSearch.Add(loanSearch);
                }

                db.SaveChanges();
                string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.SEARCH_FIELDS_INSERTED);
                LoanAudit.InsertLoanSearchAudit(db, loanSearch != null ? loanSearch : loanSearch, auditDescs[0], auditDescs[1]);
                Logger.WriteTraceLog($"After Audit Loan Search");
                if (!string.IsNullOrEmpty(LoanNumber) && string.IsNullOrEmpty(loanDetails.LoanNumber))
                    loanDetails.LoanNumber = LoanNumber;
            }
            Logger.WriteTraceLog($"End UpdateLoanSearchFields");
            return loanDetails;
        }

        private LoanSearch GetLoanSearch(Int64 LoanID, DBConnect db)
        {
            return db.LoanSearch.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
        }

        private Loan UpdateLoanSearchFields(Int64 LoanID, Batch batchObj, DBConnect db)
        {
            var loanDetails = GetLoanInfo(batchObj.LoanID, db);
            if (loanDetails != null)
            {
                string LoanNumber = string.Empty;
                string BorrowerName = string.Empty;
                string SSN = string.Empty;
                decimal LoanAmount = 0m;
                foreach (var document in batchObj.Documents)
                {
                    foreach (var field in document.DocumentLevelFields)
                    {
                        switch (field.Name.ToUpper())
                        {
                            case "LOAN NUMBER":
                                {
                                    if (!LoanNumber.Equals(string.Empty))
                                        break;

                                    LoanNumber = field.Value;
                                    break;
                                }
                            case "LOAN NO":
                                {
                                    if (!LoanNumber.Equals(string.Empty))
                                        break;

                                    LoanNumber = field.Value;
                                    break;
                                }
                            case "BORROWER NAME":
                                {
                                    if (!BorrowerName.Equals(string.Empty))
                                        break;

                                    BorrowerName = field.Value;
                                    break;
                                }
                            case "LOAN AMOUNT":
                                {
                                    if (LoanAmount != 0m)
                                        break;

                                    //LoanAmount = string.IsNullOrEmpty(field.Value) ? 0m : Convert.ToDecimal(field.Value);
                                    decimal.TryParse(field.Value, out LoanAmount);
                                    break;
                                }
                            case "SSN":
                                {
                                    if (!SSN.Equals(string.Empty))
                                        break;

                                    SSN = field.Value;
                                    break;
                                }
                        }
                    }
                }

                LoanSearch loanSearch = db.LoanSearch.AsNoTracking().Where(s => s.LoanID == LoanID).FirstOrDefault();

                if (loanSearch != null)
                {
                    if (!(string.IsNullOrEmpty(LoanNumber)))
                    {
                        loanSearch.LoanNumber = LoanNumber;
                        loanDetails.LoanNumber = LoanNumber;
                        //UpdateLoanNumber(db, LoanID, LoanNumber, "Loan Number Modified");
                    }
                    if (!(string.IsNullOrEmpty(BorrowerName)))
                        loanSearch.BorrowerName = BorrowerName;
                    if (LoanAmount != 0m)
                        loanSearch.LoanAmount = LoanAmount;
                    if (!(string.IsNullOrEmpty(SSN)))
                        loanSearch.SSN = SSN;

                    loanSearch.Status = loanDetails.Status;
                    loanSearch.ModifiedOn = DateTime.Now;
                    db.Entry(loanSearch).State = EntityState.Modified;
                    db.SaveChanges();
                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.SEARCH_FIELDS_MODIFIED);
                    LoanAudit.InsertLoanSearchAudit(db, loanSearch, auditDescs[0], auditDescs[1]);
                }
            }
            return loanDetails;
        }

        private void UpdateLoanNumber(DBConnect db, Int64 LoanID, string LoanNumber, string auditDesc)
        {
            Loan loan = db.Loan.AsNoTracking().Where(s => s.LoanID == LoanID).FirstOrDefault();

            if (loan != null)
            {
                loan.LoanNumber = LoanNumber;
                loan.ModifiedOn = DateTime.Now;
                db.Entry(loan).State = EntityState.Modified;
                db.SaveChanges();

                LoanAudit.InsertLoanAudit(db, loan, "", "");
            }
        }

        private Int64 GetLoanPageCount(DBConnect db, Int64 LoanID)
        {
            List<LoanImage> _loans = db.LoanImage.AsNoTracking().Where(l => l.LoanID == LoanID).ToList();
            if (_loans != null && _loans.Count > 0)
                return _loans.Count;
            else
                return 0;
        }

        private Int64 InsertImagesToDB(Batch batchObj, DBConnect db, string EphesoftOutputPath, string ImageMaxHeight, string ImageMaxWidth)
        {

            ImageUtilities imgUtil = new ImageUtilities();
            Int64 _pageCount = GetLoanPageCount(db, batchObj.LoanID);
            if (_pageCount == 0)
            {
                LogMessage($"Total Documents : { batchObj.Documents.Count} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
                foreach (var doc in batchObj.Documents)
                {
                    LogMessage($"Start Document Processing : {doc.Type}, LoanID : {batchObj.LoanID.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
                    string DocType = doc.Type;
                    string fileType = string.IsNullOrEmpty(doc.MultiPageTiffFile) ? "pdf" : "tiff";
                    if (fileType == "tiff")
                    {
                        string tiffFileName = Path.GetFileName(doc.MultiPageTiffFile);
                        doc.MultiPageTiffFile = Path.Combine(EphesoftOutputPath, tiffFileName); //EphesoftOutputPath + doc.MultiPageTiffFile.Substring(doc.MultiPageTiffFile.ToLower().IndexOf(@"\output\"));
                        LogMessage($"doc.MultiPageTiffFile : {doc.MultiPageTiffFile}");
                        byte[] imageBytes = File.ReadAllBytes(doc.MultiPageTiffFile);
                        Int64 pageCount = imgUtil.GetByteDataPageCount(imageBytes, "image/tiff");
                        Int32 _maxWidth = 1654;
                        Int32 _maxHeight = 2339;
                        Int32.TryParse(ImageMaxWidth, out _maxWidth);
                        Int32.TryParse(ImageMaxHeight, out _maxHeight);
                        LogMessage($"Document : {doc.Type} , PageCount : {pageCount},  {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
                        for (int i = 1; i <= pageCount; i++)
                        {
                            LogMessage($"Current Page No : {i} , {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
                            _pageCount = _pageCount + 1;
                            var img = imgUtil.ConvertTiffToJpegGrayScale(imageBytes, i, _maxWidth, _maxHeight);
                            //var img = imgUtil.ConvertTiffToJpeg(imageBytes, i, _maxWidth, _maxHeight);
                            LogMessage($"Start Loan Upload Processing Page No : {i} , LoanID : {batchObj.LoanID.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
                            InsertDocumentImages(batchObj.LoanID, doc.DocumentTypeID, (i - 1), img.Image, doc.VersionNumber, db);
                            //LogMessage($"End Loan Upload Processing Page No : {i} , LoanID : {batchObj.LoanID.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");

                            UpdateFieldCoordinates(doc, img.OrginalImageWidth, img.OrginalImageWidth, img.CompressedImageWidth, img.CompressedImageHeight, (i - 1).ToString());
                            LogMessage($"End Coordinate Update  : {batchObj.LoanID.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");

                        }
                    }
                    else
                    {
                        string pdfFileName = Path.GetFileName(doc.MultiPagePdfFile);
                        doc.MultiPagePdfFile = Path.Combine(EphesoftOutputPath, pdfFileName); //EphesoftOutputPath + doc.MultiPagePdfFile.Substring(doc.MultiPagePdfFile.IndexOf(@"\Output\"));
                        LogMessage($"doc.MultiPagePdfFile : {doc.MultiPagePdfFile}");
                        byte[] imageBytes = File.ReadAllBytes(doc.MultiPagePdfFile);
                        Int64 pageCount = imgUtil.GetByteDataPageCount(imageBytes, "application/pdf");
                        LogMessage($"Document : {doc.Type} , PageCount : {pageCount},  {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
                        for (int i = 1; i <= pageCount; i++)
                        {
                            LogMessage($"Current Page No : {i} , {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
                            var img = imgUtil.GetJpegFromPDF(imageBytes, i);
                            InsertDocumentImages(batchObj.LoanID, doc.DocumentTypeID, (i - 1), img, doc.VersionNumber, db);
                        }
                    }
                    LogMessage($"End Document Processing : {doc.Type}, LoanID :  {batchObj.LoanID.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
                }
            }
            return _pageCount;
        }




        public List<DocumentFieldMaster> GetFieldsForAccuracyCalc(Int64 docID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.DocumentFieldMaster.AsNoTracking().Where(x => x.DocumentTypeID == docID && x.AllowAccuracyCalc == true).ToList();
            }
        }


        private void UpdateFieldCoordinates(Documents doc, int orgWidth, int orgHeight, int newWidth, int newHeight, string pageNo)
        {
            var zoomVal = (newWidth * 1f / orgWidth) * 100;
            var curFields = doc.DocumentLevelFields.Where(p => p.PageNo == pageNo);
            if (curFields != null)
            {
                foreach (var field in curFields)
                {
                    field.CoordinatesList.x0 = Convert.ToInt32((field.CoordinatesList.x0 * zoomVal) / 100);
                    field.CoordinatesList.x1 = Convert.ToInt32((field.CoordinatesList.x1 * zoomVal) / 100);
                    field.CoordinatesList.y0 = Convert.ToInt32((field.CoordinatesList.y0 * zoomVal) / 100);
                    field.CoordinatesList.y1 = Convert.ToInt32((field.CoordinatesList.y1 * zoomVal) / 100);
                }
            }
        }

        private void MissingDocInsertImagesToDB(Batch batchObj, DBConnect db, string EphesoftOutputPath, string ImageMaxHeight, string ImageMaxWidth)
        {

            ImageUtilities imgUtil = new ImageUtilities();

            foreach (var doc in batchObj.Documents)
            {
                string DocType = doc.Type;
                string fileType = string.IsNullOrEmpty(doc.MultiPageTiffFile) ? "pdf" : "tiff";
                Dictionary<string, object> missingDocAuditInfo = new Dictionary<string, object>();
                missingDocAuditInfo["LOANID"] = batchObj.LoanID;
                missingDocAuditInfo["DOCID"] = doc.DocumentTypeID;
                missingDocAuditInfo["USERID"] = 0;
                missingDocAuditInfo["FILENAME"] = Path.GetFileName(doc.MultiPageTiffFile);
                if (fileType == "tiff")
                {
                    string tiffFileName = Path.GetFileName(doc.MultiPageTiffFile);
                    doc.MultiPageTiffFile = Path.Combine(EphesoftOutputPath, tiffFileName); // + doc.MultiPageTiffFile.Substring(doc.MultiPageTiffFile.IndexOf(@"\Output\"));
                    byte[] imageBytes = File.ReadAllBytes(doc.MultiPageTiffFile);
                    Int64 pageCount = imgUtil.GetByteDataPageCount(imageBytes, "image/tiff");

                    Int32 _maxWidth = 1654;
                    Int32 _maxHeight = 2339;
                    Int32.TryParse(ImageMaxWidth, out _maxWidth);
                    Int32.TryParse(ImageMaxHeight, out _maxHeight);
                    for (int i = 1; i <= pageCount; i++)
                    {

                        var img = imgUtil.ConvertTiffToJpegGrayScale(imageBytes, i, _maxWidth, _maxHeight);
                        //var img = imgUtil.ConvertTiffToJpeg(imageBytes, i, _maxWidth, _maxHeight);
                        InsertDocumentImages(batchObj.LoanID, doc.DocumentTypeID, (i - 1), img.Image, doc.VersionNumber, db);

                        UpdateFieldCoordinates(doc, img.OrginalImageWidth, img.OrginalImageWidth, img.CompressedImageWidth, img.CompressedImageHeight, (i - 1).ToString());
                    }
                    //string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.MISSING_DOCUMENT_FROM_SYSTEM);
                    //LoanAudit.InsertLoanMissingDocAudit(db, missingDocAuditInfo, 0, auditDescs[0], auditDescs[1]);
                }
                else
                {
                    string pdfFileName = Path.GetFileName(doc.MultiPagePdfFile);
                    doc.MultiPagePdfFile = Path.Combine(EphesoftOutputPath, pdfFileName); //EphesoftOutputPath + doc.MultiPagePdfFile.Substring(doc.MultiPagePdfFile.IndexOf(@"\Output\"));
                    byte[] imageBytes = File.ReadAllBytes(doc.MultiPagePdfFile);
                    Int64 pageCount = imgUtil.GetByteDataPageCount(imageBytes, "application/pdf");
                    for (int i = 1; i <= pageCount; i++)
                    {
                        var img = imgUtil.GetJpegFromPDF(imageBytes, i);
                        InsertDocumentImages(batchObj.LoanID, doc.DocumentTypeID, (i - 1), img, doc.VersionNumber, db);
                    }
                    //string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.MISSING_DOCUMENT_FROM_SYSTEM);
                    //LoanAudit.InsertLoanMissingDocAudit(db, missingDocAuditInfo, 0, auditDescs[0], auditDescs[1]);
                }
            }

        }

        private List<DocumentTypeMaster> GetCustLoanDocTypes(DBConnect db, Int64 CustomerID, Int64 LoanTypeID)
        {
            List<DocumentTypeMaster> docTypes = new List<DocumentTypeMaster>();

            string _getCustLoanDocMapping = db.CustLoanDocMapping.AsNoTracking()
                     .Where(d => d.CustomerID == CustomerID && d.LoanTypeID == LoanTypeID).ToString();

            _getCustLoanDocMapping = _getCustLoanDocMapping.Replace("@p__linq__0", CustomerID.ToString()).Replace("@p__linq__1", LoanTypeID.ToString());

            List<CustLoanDocMapping> CustReviewLoanDocMapping = db.CustLoanDocMapping.SqlQuery(_getCustLoanDocMapping).ToList();

            if (CustReviewLoanDocMapping != null)
            {
                foreach (var item in CustReviewLoanDocMapping)
                {
                    DocumentTypeMaster doc = db.DocumentTypeMaster.AsNoTracking().Where(dm => dm.DocumentTypeID == item.DocumentTypeID).FirstOrDefault();
                    if (doc != null)
                        docTypes.Add(doc);
                }
            }

            return docTypes;
        }

        #endregion

        #region  ImportStaging
        public ImportStagings GetImportStage()
        {
            ImportStagings _import = new ImportStagings();
            using (var db = new DBConnect(TenantSchema))
            {
                _import = db.ImportStaging.AsNoTracking().Where(x => x.Status == ImportStagingConstant.Staged).OrderBy(x => x.Priority).OrderByDescending(x => x.LoanCreatedDate).Take(1).FirstOrDefault();
                return _import;
            }
        }
        public ImportStagings GetImportStageStatus()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.ImportStaging.AsNoTracking().Where(x => x.Status == ImportStagingConstant.Staged && x.LoanPicked == false).OrderBy(p => p.Priority).ThenBy(c => c.LoanCreatedDate).FirstOrDefault();
            }
        }

        public Int64 GetImportStageStatusCount()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.ImportStaging.AsNoTracking().Where(x => x.Status == ImportStagingConstant.Staged && x.LoanPicked == false).ToList().Count;
            }
        }
        public void UpdateImportStageStatus(ImportStagings _importstaging)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                _importstaging.ModifiedOn = DateTime.Now;
                db.Entry(_importstaging).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void UpdateLoanTable(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
                if (_loan != null)
                {
                    _loan.SubStatus = StatusConstant.LOAN_TYPE_NOT_FOUND;
                    _loan.ModifiedOn = DateTime.Now;
                    db.Entry(_loan).State = EntityState.Modified;
                    db.SaveChanges();

                    LoanAudit.InsertLoanAudit(db, _loan, "Loan Type Not Found", "Loan Type Not Found");
                }
            }
        }
        private void LogMessage(string msg)
        {
            Logger.WriteTraceLog(msg);
        }
        #endregion
    }
}
