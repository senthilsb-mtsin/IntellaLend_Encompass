using IntellaLend.Constance;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using MTSEntBlocks.UtilsBlock;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace IL.ImportFromUNC
{
    public class ImportFromUNC : IMTSServiceBase
    {
        private bool logTracing = false;
        private static string IntellaLendLoanUploadPath = string.Empty;
        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            IntellaLendLoanUploadPath = Params.Find(f => f.Key == "IntellaLendLoanUploadPath").Value;
            Boolean.TryParse(ConfigurationManager.AppSettings["ImportFromUNC"].ToLower(), out logTracing);
        }

        public bool DoTask()
        {
            try
            {
                List<TenantMaster> TenantLists = ImportFromUNCDataAccess.GetAllTenants();
                foreach (TenantMaster tenantLists in TenantLists)
                {
                    StartImport(tenantLists.TenantSchema);

                }
                return true;
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
            return false;

        }
        private void StartImport(string tenantSchema)
        {
            LogMessage($"Tenant Schema : {tenantSchema.ToString()}");
            ImportFromUNCDataAccess _import = null;
            try
            {
                _import = new ImportFromUNCDataAccess(tenantSchema);
                List<CustReviewLoanUploadPath> _lsCustReviewLoanUploadPath = _import.GetAllLoanPathDetails();
                LogMessage($"Available Upload Path Count : {_lsCustReviewLoanUploadPath.Count.ToString()}");
                foreach (CustReviewLoanUploadPath _custReviewLoanUploadPath in _lsCustReviewLoanUploadPath)
                {
                    LogMessage($"Checking Folder Path : {_custReviewLoanUploadPath.UploadPath.ToString()}");

                    string drive = Path.GetPathRoot(_custReviewLoanUploadPath.UploadPath);
                    if (Directory.Exists(drive))
                    {
                        if (!Directory.Exists(_custReviewLoanUploadPath.UploadPath))
                            Directory.CreateDirectory(_custReviewLoanUploadPath.UploadPath);
                        string[] dirNames = Directory.GetDirectories(_custReviewLoanUploadPath.UploadPath);
                        PDFMerger merger = null;
                        if (dirNames.Length > 0)
                        {
                            foreach (string _dir in dirNames)
                            {
                                string[] _dirFiles = Directory.GetFiles(_dir, "*.pdf", SearchOption.AllDirectories);
                                string tempFileName = Path.GetFullPath(_dir) + ".pdf";
                                //string tempFileName = Path.Combine(_custReviewLoanUploadPath.UploadPath, Path.GetDirectoryName(_dir) + ".pdf");

                                merger = new PDFMerger(tempFileName);
                                merger.OpenDocument();
                                foreach (string _pdfFile in _dirFiles)
                                {
                                    merger.AppendPDF(File.ReadAllBytes(_pdfFile));

                                }
                                merger.SaveDocument();
                                Directory.Delete(_dir, true);

                            }
                        }
                        
                    }
                    else
                    {
                        Exception ex = new Exception("Drive Not Found : "+ _custReviewLoanUploadPath.UploadPath);
                        MTSExceptionHandler.HandleException(ref ex);
                    }
                    string[] files = Directory.GetFiles(_custReviewLoanUploadPath.UploadPath, "*.pdf", SearchOption.AllDirectories);
                    LogMessage($"Files Count : {files.Length.ToString()}");

                    foreach (string _file in files)
                    {
                        Loan _loan = null;
                        try
                        {
                            LogMessage($"Current File : {_file.ToString()}");
                            string fileName = Path.GetFileName(_file);
                            string exactPath = Path.Combine(IntellaLendLoanUploadPath, tenantSchema.ToUpper(), "Input", DateTime.Now.ToString("yyyyMMdd"));

                            if (!Directory.Exists(exactPath))
                                Directory.CreateDirectory(exactPath);

                            _loan = new Loan();
                            _loan = SetLoanData(_custReviewLoanUploadPath, fileName, tenantSchema);
                            LogMessage($"Current LoanId : {_loan.LoanID.ToString()}");

                            string newFileName = tenantSchema.ToUpper() + "_" + _loan.LoanID + Path.GetExtension(fileName);
                            string lckFileName = tenantSchema.ToUpper() + "_" + _loan.LoanID + ".lck";
                            LogMessage($"New File Name : {newFileName.ToString()}");

                            File.Move(_file, Path.Combine(exactPath, lckFileName));

                            File.Move(Path.Combine(exactPath, lckFileName), Path.Combine(exactPath, newFileName));

                            //string[] tempSearch = Directory.GetFiles(dicName, "*.pdf", SearchOption.AllDirectories);

                            //if (tempSearch.Length == 0)
                            //    Directory.Delete(dicName);

                            LogMessage($"File Moved Successfully, LoanId : {_loan.LoanID.ToString()}");
                        }
                        catch (Exception Ex)
                        {
                            if (_loan != null && _loan.LoanID > 0)
                            {
                                _import.DeleteLoan(_loan.LoanID);
                            }
                            throw Ex;
                        }

                    }
                }
            }
            catch (Exception Ex)
            {
                MTSExceptionHandler.HandleException(ref Ex);
            }
        }
        private Loan SetLoanData(CustReviewLoanUploadPath loanPath, string fileName, string _schema)
        {
            try
            {
                var db = new DBConnect(_schema);
                string AuditDateFormat = DateConstance.AuditDateFormat;


                ImportFromUNCDataAccess _import = null;
                _import = new ImportFromUNCDataAccess(_schema);
                Loan loan = new Loan();
                loan.ReviewTypeID = loanPath.ReviewTypeID;
                loan.LoanTypeID = loanPath.LoanTypeID;
                loan.SubStatus = 0;
                loan.UploadedUserID = 1;
                loan.LoggedUserID = 0;
                loan.FileName = fileName;
               // loan.FromBox = false;
                loan.UploadType = 0;
                loan.CreatedOn = DateTime.Now;
                loan.ModifiedOn = DateTime.Now;
                loan.CustomerID = loanPath.CustomerID;
                loan.LastAccessedUserID = 0;
                loan.Priority = _import.GetPriority(loanPath.ReviewTypeID);  
                //loan.LoanGUID = Guid.NewGuid();
                loan.AuditMonthYear = Convert.ToDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(IntellaLend.Constance.DateConstance.AuditDateFormat));
            
                loan = _import.AddFileUploadDetails(loan);
                return loan;
            }
            catch (Exception Ex)
            {

                throw Ex;
            }

        }
        private void LogMessage(string _msg)
        {
            Logger.WriteTraceLog(_msg);
        }

    }
}
