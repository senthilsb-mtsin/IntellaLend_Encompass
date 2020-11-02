using IntellaLend.Constance;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using MTSEntBlocks.UtilsBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace IL.LOSLoanImport
{
    public class LOSLoanImport : IMTSServiceBase
    {
        private bool logTracing = false;
        private static string MASLoanImportPath = string.Empty;
        private static string MASLoanImportErrorPath = string.Empty;
        private static string MASLoanImportDonePath = string.Empty;
        private static string FannieMaeVersion = string.Empty;
        private static string IntellaLendLoanUploadPath = string.Empty;
        private static string LockExt = ".lck";
        private static string ErrorExt = ".error";
        private static string DonExt = ".don";

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            MASLoanImportPath = Params.Find(f => f.Key == "MASLoanImportPath").Value;
            MASLoanImportErrorPath = Params.Find(f => f.Key == "MASLoanImportErrorPath").Value;
            MASLoanImportDonePath = Params.Find(f => f.Key == "MASLoanImportDonePath").Value;
            FannieMaeVersion = Params.Find(f => f.Key == "FannieMaeVersion").Value;
            IntellaLendLoanUploadPath = Params.Find(f => f.Key == "IntellaLendLoanUploadPath").Value;
        }

        public bool DoTask()
        {
            try
            {
                List<TenantMaster> TenantLists = LOSLoanImportDataAccess.GetAllTenants();
                LOSLoanImportDataAccess _dataAccess = null;
                foreach (TenantMaster tenantLists in TenantLists)
                {
                    DirectoryInfo dic = new DirectoryInfo(MASLoanImportPath);
                    bool result = true;
                    do
                    {
                        _dataAccess = new LOSLoanImportDataAccess(tenantLists.TenantSchema);
                        DownloadLoanFromMAS(tenantLists.TenantSchema, dic, _dataAccess);
                        FileInfo[] files = dic.GetFiles("*.json", SearchOption.AllDirectories).Where(f => f.Length > 0).ToArray();
                        result = files.Length > 0;
                    }
                    while (result);
                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }

            return true;
        }

        public void DownloadLoanFromMAS(string schema, DirectoryInfo importDirectory, LOSLoanImportDataAccess _dataAccess)
        {
            try
            {
                FileInfo[] files = importDirectory.GetFiles("*.json", SearchOption.AllDirectories).Where(f => f.Length > 0).ToArray();

                foreach (var item in files)
                {
                    string lckpath = Path.ChangeExtension(item.FullName, LockExt);
                    string inputJSONFileName = Path.GetFileName(item.FullName);
                    string errorFileName = Path.ChangeExtension(inputJSONFileName, ErrorExt);
                    string donFileName = Path.ChangeExtension(inputJSONFileName, DonExt);

                    File.Move(item.FullName, lckpath);

                    string errorMsg = string.Empty;
                    string fileData = File.ReadAllText(lckpath);
                    Int64 _customerID = 0;
                    Int64 _reviewTypeID = 0;
                    Int64 _loanTypeID = 0;
                    Int64 _loanID = 0;
                    InputJson _inputJson = new InputJson();
                    try
                    {
                        _inputJson = JsonConvert.DeserializeObject<InputJson>(fileData);
                    }
                    catch (Exception ex)
                    {
                        _dataAccess.UpdateImportStagingTable(item.FullName, _customerID, _reviewTypeID, _loanTypeID, _loanID, false, LOSImportStatusConstant.LOS_IMPORT_FAILED, $"Error while parsing input file '{item.FullName}'. Inner exception : {ex.Message}");
                        File.Move(lckpath, Path.Combine(MASLoanImportErrorPath, errorFileName));
                        MTSExceptionHandler.HandleException(ref ex);
                        continue;
                    }

                    CustomerMaster _customer = _dataAccess.CheckCustomerExists(_inputJson.LenderCode.ToString());

                    if (_customer == null)
                        errorMsg += $"Lender : {_inputJson.LenderName} ({_inputJson.LenderCode.ToString()}) not exist in IntellaLend. ";
                    else
                        _customerID = _customer.CustomerID;

                    ReviewTypeMaster _reivewType = _dataAccess.CheckReviewTypeExists(_inputJson.ReviewType.Trim());

                    if (_reivewType == null)
                        errorMsg += $"ReviewType : {_inputJson.ReviewType} not exist in IntellaLend. ";
                    else
                        _reviewTypeID = _reivewType.ReviewTypeID;

                    LoanTypeMaster _loanType = _dataAccess.CheckLoanTypeExists(_inputJson.LoanType.Trim());

                    if (_loanType == null)
                        errorMsg += $"LoanType : {_inputJson.LoanType} not exist in IntellaLend. ";
                    else
                        _loanTypeID = _loanType.LoanTypeID;


                    if (_inputJson.Priority > 5)
                        errorMsg += $"Priority : {_inputJson.Priority} not exist in IntellaLend. ";

                    if (_customerID > 0 && _reviewTypeID > 0 && _loanTypeID > 0)
                    {
                        bool _mappingExists = _dataAccess.CheckMappingExists(_customerID, _reviewTypeID, _loanTypeID);

                        if (!_mappingExists)
                            errorMsg += $"Lender, ReviewType and LoanType Mapping not available in IntellaLend.";

                        if (!string.IsNullOrEmpty(_inputJson.FNMFile) && !File.Exists(_inputJson.FNMFile))
                            errorMsg += $"Fannie Mae file not exists on the mentioned path '{_inputJson.FNMFile}'";

                        if (_inputJson.LoanFiles != null && _inputJson.LoanFiles.Length > 0)
                        {
                            foreach (string _loanPDF in _inputJson.LoanFiles)
                            {
                                if (!File.Exists(_loanPDF))
                                    errorMsg += $"Loan media not exists on the mentioned path '{_loanPDF}'";
                            }
                        }
                        else errorMsg += $"Loan not mentioned on the Import file '{item.FullName}";

                        if (string.IsNullOrEmpty(errorMsg))
                        {
                            Int64 StagingID = _dataAccess.UpdateImportStagingTable(lckpath, _customerID, _reviewTypeID, _loanTypeID, _loanID, true, LOSImportStatusConstant.LOS_IMPORT_STAGED);
                            Logger.WriteTraceLog("Loan Processing Started");
                            ProcessInputJSONFile(_dataAccess, StagingID, _inputJson, _customerID, _reviewTypeID, _loanTypeID, lckpath, errorFileName, donFileName);
                            Logger.WriteTraceLog("Loan Processing Ended");
                        }
                        else
                        {
                            _dataAccess.UpdateImportStagingTable(item.FullName, _customerID, _reviewTypeID, _loanTypeID, _loanID, false, LOSImportStatusConstant.LOS_IMPORT_FAILED, errorMsg);
                            File.Move(lckpath, Path.Combine(MASLoanImportErrorPath, errorFileName));
                        }
                    }
                    else
                    {
                        _dataAccess.UpdateImportStagingTable(item.FullName, _customerID, _reviewTypeID, _loanTypeID, _loanID, false, LOSImportStatusConstant.LOS_IMPORT_FAILED, errorMsg);
                        File.Move(lckpath, Path.Combine(MASLoanImportErrorPath, errorFileName));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ProcessInputJSONFile(LOSLoanImportDataAccess _dataAccess, Int64 StagingID, InputJson _inputJSON, Int64 _customerID, Int64 _reviewTypeID, Int64 _loanTypeID, string lckpath, string errorFileName, string donFileName)
        {
            Int64 LoanID = 0;
            string newFileName = string.Empty;
            bool isMissingDoc = false;
            try
            {
                _dataAccess.UpdateImportStagingTable(StagingID, 0, LOSImportStatusConstant.LOS_IMPORT_PROCESSING);
                string _fannieMaeJson = string.Empty;
                if (!string.IsNullOrEmpty(_inputJSON.FNMFile))
                {
                    Logger.WriteTraceLog("FannieMae File Reading Started");
                    FannieMaeReader _reader = new FannieMaeReader(_inputJSON.FNMFile, _dataAccess.GetFannieMaeTemplate(FannieMaeVersion));
                    _fannieMaeJson = _reader.Read();
                    Logger.WriteTraceLog("FannieMae File Reading End");
                }
                try
                {
                    LoanID = _dataAccess.CheckLoanExists(_inputJSON.LoanID);

                    if (LoanID > 0)
                    {
                        Logger.WriteTraceLog($"Loan Exists :{LoanID}");
                        int auditLoanMissingDocCount = _dataAccess.GetAuditLoanMissingDocCount(LoanID);
                        newFileName = _dataAccess.TenantSchema.ToUpper() + "_" + LoanID.ToString() + "_" + (auditLoanMissingDocCount + 1) + ".lck";
                        isMissingDoc = true;
                    }
                    else
                    {
                        LoanID = CreateLoan(_dataAccess, _inputJSON, _customerID, _reviewTypeID, _loanTypeID);
                        Logger.WriteTraceLog($"Loan Created :{LoanID}");
                        newFileName = _dataAccess.TenantSchema.ToUpper() + "_" + LoanID.ToString() + ".lck";
                    }
                }
                catch (Exception ex)
                {
                    LoanID = 0;
                    MTSExceptionHandler.HandleException(ref ex);
                    Logger.WriteTraceLog($"Loan Creation Failed");
                }
                Int64 LOSDocID = _dataAccess.GetFannieMaeDocumentID(FannieMaeVersion);
                Logger.WriteTraceLog($"LOS Document ID :{LOSDocID}");

                if (LoanID > 0)
                {
                    string exactPath = Path.Combine(IntellaLendLoanUploadPath, _dataAccess.TenantSchema.ToUpper(), "Input", DateTime.Now.ToString("yyyyMMdd"));

                    if (!Directory.Exists(exactPath))
                        Directory.CreateDirectory(exactPath);

                    Logger.WriteTraceLog($"Loan PDF FileName : {newFileName}");
                    string lockFileName = Path.Combine(exactPath, newFileName);
                    string OrgFileName = Path.ChangeExtension(lockFileName, ".pdf");

                    string[] LoanFiles = _inputJSON.LoanFiles;
                    Logger.WriteTraceLog($"Import Loan PDF Count : {LoanFiles.Length}");

                    Logger.WriteTraceLog($"Loan PDF Started Processing");
                    PDFMerger merger = new PDFMerger(lockFileName);
                    merger.OpenDocument();
                    foreach (var item in LoanFiles)
                    {
                        merger.AppendPDF(File.ReadAllBytes(item));
                    }
                    merger.SaveDocument();

                    Logger.WriteTraceLog($"Loan PDF Created");

                    if (!string.IsNullOrEmpty(_fannieMaeJson))
                    {
                        _dataAccess.InsertFannieMaeDetails(LoanID, LOSDocID, _fannieMaeJson);
                        Logger.WriteTraceLog($"Loan FannieMae JSON Inserted");
                    }

                    File.Move(lockFileName, OrgFileName);

                    _dataAccess.UpdateImportStagingTable(StagingID, LoanID, LOSImportStatusConstant.LOS_IMPORT_PROCESSED);

                    if (!isMissingDoc)
                        _dataAccess.UpdateLoanTable(LoanID, StatusConstant.READY_FOR_IDC);
                    else
                    {
                        Dictionary<string, object> missingDocAuditInfo = new Dictionary<string, object>();
                        missingDocAuditInfo["LOANID"] = LoanID;
                        missingDocAuditInfo["DOCID"] = 0;
                        missingDocAuditInfo["USERID"] = 1;
                        missingDocAuditInfo["FILENAME"] = newFileName;
                        missingDocAuditInfo["EDOWNLOADSTAGINGID"] = -1;
                        _dataAccess.MissingDocFileUpload(missingDocAuditInfo);
                    }

                    File.Move(lckpath, Path.Combine(MASLoanImportDonePath, donFileName));
                }
                else
                {
                    _dataAccess.UpdateImportStagingTable(StagingID, 0, LOSImportStatusConstant.LOS_IMPORT_FAILED, "Cannot able to create Loan record.");
                    File.Move(lckpath, Path.Combine(MASLoanImportErrorPath, errorFileName));
                }
            }
            catch (Exception ex)
            {
                _dataAccess.UpdateImportStagingTable(StagingID, LoanID, LOSImportStatusConstant.LOS_IMPORT_FAILED, "Error While Processing Loan");
                File.Move(lckpath, Path.Combine(MASLoanImportErrorPath, errorFileName));
                throw ex;
            }
        }

        private Int64 CreateLoan(LOSLoanImportDataAccess _dataAccess, InputJson _inputJSON, Int64 _customerID, Int64 _reviewTypeID, Int64 _loanTypeID)
        {
            Loan loan = new Loan();
            loan.ReviewTypeID = _reviewTypeID;
            loan.LoanTypeID = _loanTypeID;
            loan.UploadedUserID = 1;
            loan.CustomerID = _customerID;
            loan.LoanNumber = _inputJSON.LoanID.Trim() == "0" ? string.Empty : _inputJSON.LoanID.Trim();
            loan.LastAccessedUserID = 0;
            loan.UploadType = UploadConstant.LOS;
            loan.CreatedOn = DateTime.Now;
            loan.AuditMonthYear = Convert.ToDateTime(Convert.ToDateTime(_inputJSON.AuditPeriod).ToString(DateConstance.AuditDateFormat));
            loan.CreatedOn = DateTime.Now;
            loan.LoanGUID = Guid.NewGuid();
            loan.Priority = _inputJSON.Priority;

            return _dataAccess.CreateLoan(loan);
        }
    }
}
