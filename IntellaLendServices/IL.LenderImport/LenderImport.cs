using IntellaLend.Constance;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;


namespace IL.LenderImport
{
    public class LenderImport : IMTSServiceBase
    {
        private bool logTracing = false;
        private static string LenderImportPath = string.Empty;
        private static string LenderImportErrorPath = string.Empty;
        private static string LenderImportDonePath = string.Empty;
        private static string LockTxt = ".lck";
        private static string ErrorTxt = ".error";
        private static string DonTxt = ".don";
        bool result = true;

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            LenderImportPath = Params.Find(f => f.Key == "LenderImportPath").Value;
            LenderImportErrorPath = Params.Find(f => f.Key == "LenderImportErrorPath").Value;
            LenderImportDonePath = Params.Find(f => f.Key == "LenderImportDonePath").Value;

        }
        public bool DoTask()
        {
            try
            {
                List<TenantMaster> TenantLists = LenderImportDataAccess.GetAllTenants();
                LenderImportDataAccess _dataAccess = null;
                foreach (TenantMaster tenantLists in TenantLists)
                {
                    DirectoryInfo _directory = new DirectoryInfo(LenderImportPath);
                    do
                    {
                        _dataAccess = new LenderImportDataAccess(tenantLists.TenantSchema);

                        DownloadLenderImport(tenantLists.TenantSchema, _directory, _dataAccess);

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
        public void DownloadLenderImport(string schema, DirectoryInfo importDirectory, LenderImportDataAccess _dataAccess)
        {
            try
            {
                FileInfo[] files = importDirectory.GetFiles("*.csv", System.IO.SearchOption.AllDirectories).Where(f => f.Length > 0).ToArray();
                System.Data.DataTable _lenderImportDetails;

                foreach (var item in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(item.FullName);
                    string lckpath = Path.ChangeExtension(item.FullName, LockTxt);
                    string errorFileName = Path.ChangeExtension(item.FullName, ErrorTxt);
                    string doneFileName = Path.ChangeExtension(item.FullName, DonTxt);
                    File.Move(item.FullName, lckpath);

                    string errorMsg = string.Empty;
                    Int64 _importStagingID = 0;
                    Int64 _importCount = 0;

                    try
                    {
                        _lenderImportDetails = ParseCsv(lckpath);
                        _importStagingID = _dataAccess.UpdateLenderImportStagingTable(item.FullName, LenderImportStatusConstant.LENDER_IMPORT_STAGED, _lenderImportDetails.Rows.Count, errorMsg);
                        _dataAccess.InsertCustomerStagingDetails(_importStagingID, _lenderImportDetails, LenderImportStatusConstant.LENDER_IMPORT_STAGED, errorMsg);
                    }
                    catch (Exception ex)
                    {
                        _dataAccess.UpdateLenderImportStagingTable(item.FullName, LenderImportStatusConstant.LENDER_IMPORT_FAILED, _importCount, $"Error while parsing Lender import file '{item.FullName}'. Inner exception : {ex.Message}");
                        File.Move(lckpath, Path.Combine(LenderImportErrorPath, $"{ fileName}_{DateTime.Now.ToString("MMddyyyyHHmmssFFF")}{ErrorTxt}"));
                        MTSExceptionHandler.HandleException(ref ex);
                        continue;
                    }
                }

                List<CustomerImportStaging> customerImportStagings = _dataAccess.GetServiceCustomerImportStagings();
                if (customerImportStagings != null)
                {
                    result = customerImportStagings.Count > 0;
                    if (customerImportStagings.Count > 0)
                    {
                        foreach (var custImportStaging in customerImportStagings)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(custImportStaging.FilePath);
                            string lckpath = Path.ChangeExtension(custImportStaging.FilePath, LockTxt);
                            string _errorMsg = string.Empty;

                            List<CustomerImportStagingDetail> _customerStgDetails = _dataAccess.GetServiceCustomerImportStagingDetails(custImportStaging.ID);

                            if (custImportStaging.AssignType == LenderAssignTypeStatusConstant.LENDER_IMPORT)
                            {
                                try
                                {
                                    if (_customerStgDetails.Count > 0)
                                    {
                                        _dataAccess.UpdateLenderStagingStatus(custImportStaging.ID, LenderImportStatusConstant.LENDER_IMPORT_PROCESSING, _errorMsg);

                                        this.ImportLenderDetails(_customerStgDetails, custImportStaging.ID, _errorMsg, _dataAccess);

                                    }
                                }
                                catch (Exception ex)
                                {
                                    _dataAccess.UpdateLenderStagingStatus(custImportStaging.ID, LenderImportStatusConstant.LENDER_IMPORT_FAILED, $"Error while parsing input file '{custImportStaging.FilePath}'. Inner exception : {ex.Message}");
                                    File.Move(lckpath, Path.Combine(LenderImportErrorPath, $"{ fileName}_{DateTime.Now.ToString("MMddyyyyHHmmssFFF")}{ErrorTxt}"));
                                    MTSExceptionHandler.HandleException(ref ex);
                                    continue;
                                }
                            }
                            else
                            {

                                if (_customerStgDetails.Count > 0)
                                {
                                    try
                                    {
                                        this.ImportLenderDetails(_customerStgDetails, custImportStaging.ID, _errorMsg, _dataAccess);
                                    }
                                    catch (Exception ex)
                                    {
                                        _dataAccess.UpdateLenderStagingStatus(custImportStaging.ID, LenderImportStatusConstant.LENDER_IMPORT_FAILED, $"Inner exception : {ex.Message}");
                                        MTSExceptionHandler.HandleException(ref ex);
                                        continue;
                                    }
                                }
                            }
                            if (_dataAccess.CheckLenderImportStagingDetailStatus(custImportStaging.ID, LenderImportStatusConstant.LENDER_IMPORT_FAILED))
                            {
                                string lastErrorMsg = _dataAccess.GetLastErrorMessage(custImportStaging.ID, LenderImportStatusConstant.LENDER_IMPORT_FAILED);
                                _dataAccess.UpdateLenderStagingStatus(custImportStaging.ID, LenderImportStatusConstant.LENDER_IMPORT_FAILED, lastErrorMsg);
                            }
                            else if (_dataAccess.CheckLenderImportStagingDetailStatus(custImportStaging.ID, LenderImportStatusConstant.LENDER_IMPORT_PARTILLY_PROCESSED))
                            {
                                string lastErrorMsg = _dataAccess.GetLastErrorMessage(custImportStaging.ID, LenderImportStatusConstant.LENDER_IMPORT_PARTILLY_PROCESSED);
                                _dataAccess.UpdateLenderStagingStatus(custImportStaging.ID, LenderImportStatusConstant.LENDER_IMPORT_PARTILLY_PROCESSED, lastErrorMsg);
                            }
                            else
                            {
                                _dataAccess.UpdateLenderStagingStatus(custImportStaging.ID, LenderImportStatusConstant.LENDER_IMPORT_PROCESSED, _errorMsg);
                                if (custImportStaging.AssignType == LenderAssignTypeStatusConstant.LENDER_IMPORT)
                                {
                                    File.Move(lckpath, Path.Combine(LenderImportDonePath, $"{ fileName}_{DateTime.Now.ToString("MMddyyyyHHmmssFFF")}{DonTxt}"));
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ImportLenderDetails(List<CustomerImportStagingDetail> _customerStgDetails, Int64 _importStagingID, string errorMsg, LenderImportDataAccess _dataAccess)
        {
            foreach (var _importDetail in _customerStgDetails)
            {
                long _customerID = 0;
                long _loanTypeID = 0;
                long _reviewTypeID = 0;
                errorMsg = string.Empty;
                CustomerMaster _cusExist = null;
                try
                {
                    if (!string.IsNullOrEmpty(_importDetail.CustomerName) && !string.IsNullOrEmpty(_importDetail.CustomerCode))
                    {
                        _cusExist = _dataAccess.CheckCustomerExists(_importDetail.CustomerName, _importDetail.CustomerCode);
                        if (_cusExist == null)
                        {
                            _customerID = _dataAccess.InsertCustomerDetails(_importDetail);
                        }
                        else
                        {
                            _customerID = _cusExist.CustomerID;
                        }
                    }
                    else
                    {
                        throw new Exception($"LenderName : {_importDetail.CustomerName} or LenderCode : {_importDetail.CustomerCode} is empty");
                    }

                }
                catch (Exception ex)
                {
                    _dataAccess.UpdateLenderImportStagingDetailStatus(_importDetail.ID, LenderImportStatusConstant.LENDER_IMPORT_FAILED, ex.Message);
                    continue;
                }


                ReviewTypeMaster _reivewType = _dataAccess.CheckReviewTypeExists(_importDetail.ServiceType);

                if (_reivewType == null)
                {
                    errorMsg += $"ServiceType : {_importDetail.ServiceType.ToString().Trim()} not available in IntellaLend. ";
                    _dataAccess.UpdateLenderImportStagingDetailStatus(_importDetail.ID, _customerID == 0 ? LenderImportStatusConstant.LENDER_IMPORT_FAILED : LenderImportStatusConstant.LENDER_IMPORT_PARTILLY_PROCESSED, errorMsg);
                    continue;
                }
                else
                {
                    _reviewTypeID = _reivewType.ReviewTypeID;
                    _dataAccess.CustReviewMapping(_customerID, _reviewTypeID, _loanTypeID);
                    _dataAccess.UpdateLenderImportStagingDetailStatus(_importDetail.ID, LenderImportStatusConstant.LENDER_IMPORT_PARTILLY_PROCESSED, errorMsg);
                }

                LoanTypeMaster _loanType = _dataAccess.CheckLoanTypeExists(_importDetail.LoanType.ToString().Trim());


                if (_loanType == null)
                {
                    errorMsg += $"LoanType : {_importDetail.LoanType.ToString().Trim()} not exist in IntellaLend. ";
                    _dataAccess.UpdateLenderImportStagingDetailStatus(_importDetail.ID, _customerID == 0 ? LenderImportStatusConstant.LENDER_IMPORT_FAILED : LenderImportStatusConstant.LENDER_IMPORT_PARTILLY_PROCESSED, errorMsg);
                    continue;
                }
                else
                {
                    _loanTypeID = _loanType.LoanTypeID;
                }

                bool _checkReviewLoanTypeMapping = _dataAccess.CheckMappingExists(_reviewTypeID, _loanTypeID);
                if (_checkReviewLoanTypeMapping == true)
                {
                    if (!_dataAccess.CustReviewLoanMapping(_customerID, _reviewTypeID, _loanTypeID))
                    {
                        _dataAccess.SaveCustReviewLoanMapping(_customerID, _reviewTypeID, _loanTypeID, "", "");
                        _dataAccess.UpdateLenderImportStagingDetailStatus(_importDetail.ID, LenderImportStatusConstant.LENDER_IMPORT_PROCESSED, errorMsg);
                    }
                    else
                        _dataAccess.UpdateLenderImportStagingDetailStatus(_importDetail.ID, LenderImportStatusConstant.LENDER_IMPORT_FAILED, $"Lender : {_importDetail.CustomerName}, ServiceType : {_reivewType.ReviewTypeName}, LoanType : {_loanType.LoanTypeName} mapping already exist");
                }
                else
                {
                    errorMsg += $"ServiceType:{_importDetail.ServiceType.ToString().Trim()}, LoanType:({ _importDetail.LoanType.ToString().Trim()}) mapping not available in IntellaLend. ";
                    _dataAccess.UpdateLenderImportStagingDetailStatus(_importDetail.ID, LenderImportStatusConstant.LENDER_IMPORT_PARTILLY_PROCESSED, errorMsg);
                    _dataAccess.UpdateLenderStagingStatus(_importStagingID, LenderImportStatusConstant.LENDER_IMPORT_PARTILLY_PROCESSED, errorMsg);
                }
            }

        }

        public System.Data.DataTable ParseCsv(string filePath)
        {
            System.Data.DataTable res = ConvertCSVtoDataTable(filePath);
            return res;
        }
        public static System.Data.DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            Regex regx = new Regex(',' + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            StreamReader sr = new StreamReader(strFilePath);
            string qoatedHdrStr = sr.ReadLine();
            string[] headers = regx.Split(qoatedHdrStr);
            System.Data.DataTable dt = new System.Data.DataTable();
            foreach (string header in headers)
            {
                dt.Columns.Add(header.TrimStart('"').TrimEnd('"'));
            }
            while (!sr.EndOfStream)
            {

                string qoatedStr = sr.ReadLine();
                string[] rows = regx.Split(qoatedStr);
                if (rows.Length == headers.Length)
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i].TrimStart('"').TrimEnd('"');
                    }
                    dt.Rows.Add(dr);
                }
            }

            sr.Close();

            return dt;
        }




    }
}


