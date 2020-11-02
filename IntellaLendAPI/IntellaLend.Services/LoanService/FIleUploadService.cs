using IntellaLend.Constance;
using IntellaLend.EntityDataHandler;
using IntellaLend.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntellaLend.CommonServices
{
    public class FileUploadService
    {
        protected static string TableSchema;

        #region Constructor

        public FileUploadService()
        {
        }

        public FileUploadService(string tableSchema)
        {
            TableSchema = tableSchema;
        }

        #endregion

        #region Public Methods        

        public async Task<bool> FileUpload(Dictionary<string, string> paramsValues, string filePath, MultipartMemoryStreamProvider provider)
        {
            Loan loan = null;
            try
            {
                string exactPath = Path.Combine(filePath, TableSchema, "Input", DateTime.Now.ToString("yyyyMMdd"));

                if (!Directory.Exists(exactPath))
                    Directory.CreateDirectory(exactPath);

                byte[] fileStream = null;

                foreach (var file in provider.Contents)
                {
                    fileStream = await file.ReadAsByteArrayAsync();
                }
                if (fileStream.Length == 0)
                    throw new InvalidOperationException("File Content Length Zero");

                if (fileStream != null)
                {
                    try
                    {
                        DateTime AuditDueDate = paramsValues.ContainsKey("AuditDueDate") && !(string.IsNullOrEmpty(paramsValues["AuditDueDate"])) ? Convert.ToDateTime(Convert.ToDateTime(paramsValues["AuditDueDate"]).ToString(IntellaLend.Constance.DateConstance.AuditDateFormat)) : Convert.ToDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(IntellaLend.Constance.DateConstance.AuditDateFormat));

                        loan = new FileUploadDataAccess(TableSchema).AddFileUploadDetails(SetLoanData(paramsValues), AuditDueDate);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidDataException(ex.Message, ex.InnerException);
                    }

                    string newFileName = TableSchema + "_" + loan.LoanID + Path.GetExtension(paramsValues["UploadFileName"]);
                    File.WriteAllBytes(Path.Combine(exactPath, newFileName), fileStream);

                    if (loan != null)
                        return true;
                    else
                        throw new Exception("DB Exception");
                }
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            catch (InvalidDataException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                new FileUploadDataAccess(TableSchema).DeleteFileDetails(loan);
                throw ex;
            }
            return true;
        }

        public async Task<object> MissingDocFileUploader(Dictionary<string, string> paramsValues, string filePath, MultipartMemoryStreamProvider provider)
        {
            try
            {
                string exactPath = Path.Combine(filePath, TableSchema, "Input", DateTime.Now.ToString("yyyyMMdd"));

                if (!Directory.Exists(exactPath))
                    Directory.CreateDirectory(exactPath);

                byte[] fileStream = null;

                foreach (var file in provider.Contents)
                {
                    fileStream = await file.ReadAsByteArrayAsync();
                }
                if (fileStream.Length == 0)
                    throw new InvalidOperationException("File Content Length Zero");

                if (fileStream != null)
                {
                    Dictionary<string, object> missingDocAuditInfo = new Dictionary<string, object>();
                    missingDocAuditInfo["LOANID"] = Convert.ToInt64(paramsValues["LoanId"]);
                    missingDocAuditInfo["DOCID"] = Convert.ToInt64(paramsValues["DocId"]);
                    missingDocAuditInfo["USERID"] = Convert.ToInt64(paramsValues["UserId"]);
                    missingDocAuditInfo["FILENAME"] = Convert.ToString(Path.GetFileName(paramsValues["UploadFileName"]));
                    string newFileName = TableSchema + "_" + paramsValues["LoanId"] + "_" + paramsValues["DocId"] + Path.GetExtension(paramsValues["UploadFileName"]);
                    File.WriteAllBytes(Path.Combine(exactPath, newFileName), fileStream);

                    new FileUploadDataAccess(TableSchema).MissingDocFileUpload(missingDocAuditInfo);

                    return new { DocID = missingDocAuditInfo["DOCID"], Result = true };
                }
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new { DocID = 0, Result = false };
        }

        public async Task<object> MissingDocGlobalFileUploader(Dictionary<string, string> paramsValues, string filePath, MultipartMemoryStreamProvider provider)
        {
            try
            {
                string exactPath = Path.Combine(filePath, TableSchema, "Input", DateTime.Now.ToString("yyyyMMdd"));


                //AuditLoanMissingDoc auditLoanMissingDoc
                int auditLoanMissingDocCount = new FileUploadDataAccess(TableSchema).GetAuditLoanMissingDocCount(Convert.ToInt64(paramsValues["LoanId"]));

                string newFileName = TableSchema + "_" + paramsValues["LoanId"].ToString() + "_" + (auditLoanMissingDocCount + 1) + ".pdf";

                if (!Directory.Exists(exactPath))
                    Directory.CreateDirectory(exactPath);

                byte[] fileStream = null;

                foreach (var file in provider.Contents)
                {
                    fileStream = await file.ReadAsByteArrayAsync();
                }
                if (fileStream.Length == 0)
                    throw new InvalidOperationException("File Content Length Zero");

                if (fileStream != null)
                {
                    Dictionary<string, object> missingDocAuditInfo = new Dictionary<string, object>();
                    missingDocAuditInfo["LOANID"] = Convert.ToInt64(paramsValues["LoanId"]);
                    missingDocAuditInfo["DOCID"] = 0;
                    missingDocAuditInfo["USERID"] = Convert.ToInt64(paramsValues["UserId"]);
                    missingDocAuditInfo["FILENAME"] = newFileName;
                    missingDocAuditInfo["EDOWNLOADSTAGINGID"] = -1;

                    File.WriteAllBytes(Path.Combine(exactPath, newFileName), fileStream);

                    new FileUploadDataAccess(TableSchema).MissingDocFileUpload(missingDocAuditInfo);

                    return new { Result = true };
                }
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new { DocID = 0, Result = false };
        }


        public bool AddBoxFileUploadDetails(Loan loan, List<BoxDownloadQueue> boxq, DateTime AuditDueDate)
        {
            return new FileUploadDataAccess(TableSchema).AddBoxFileUploadDetails(loan, boxq, AuditDueDate);
        }

        public object GetBoxUploadedItems(DateTime FromDate, DateTime ToDate, Int64 CurrentUserID, int status, Int64 CustomerID)
        {
            return new FileUploadDataAccess(TableSchema).GetBoxUploadedItems(FromDate, ToDate, CurrentUserID, status, CustomerID);
        }
        public bool BoxFileUploadRetry(Int64 loanid)
        {
            return new FileUploadDataAccess(TableSchema).BoxFileUploadRetry(loanid);
        }

        public Dictionary<string, string> GetEphesoftURL(string BatchID, string EphesoftURL, Int64 CustomerID)
        {
            return new FileUploadDataAccess(TableSchema).GetEphesoftURL(BatchID, EphesoftURL, CustomerID);

        }

        #endregion

        #region Private Methods        

        private Loan SetLoanData(Dictionary<string, string> paramsValues)
        {
            string AuditDateFormat = IntellaLend.Constance.DateConstance.AuditDateFormat;
            Loan loan = new Loan();
            loan.ReviewTypeID = Convert.ToInt64(paramsValues["ReviewType"]);
            loan.LoanTypeID = Convert.ToInt64(paramsValues["LoanType"]);
            loan.SubStatus = 0;
            loan.UploadedUserID = Convert.ToInt32(paramsValues["UserId"]);
            loan.LoggedUserID = 0;
            loan.FileName = paramsValues["UploadFileName"];
            //loan.FromBox = false;
            loan.UploadType = UploadConstant.ADHOC;
            loan.CreatedOn = DateTime.Now;
            loan.ModifiedOn = DateTime.Now;
            loan.CustomerID = Convert.ToInt16(paramsValues["CustomerID"]);
            loan.LastAccessedUserID = 0;
            loan.LoanGUID = Guid.NewGuid();
            loan.AuditMonthYear = paramsValues.ContainsKey("AuditMonthYear") && !(string.IsNullOrEmpty(paramsValues["AuditMonthYear"])) ? Convert.ToDateTime(Convert.ToDateTime(paramsValues["AuditMonthYear"]).ToString(IntellaLend.Constance.DateConstance.AuditDateFormat)) : Convert.ToDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(IntellaLend.Constance.DateConstance.AuditDateFormat));
            loan.Priority = Convert.ToInt64(paramsValues["PriorityType"]);
            return loan;
        }

        public List<BoxDuplicatedFilesFolder> GetBoxDuplicateUploadFiles(long customerID, long reviewType, List<BoxItem> boxItems, Int64 UserID, string FileFilter)
        {
            return new FileUploadDataAccess(TableSchema).GetBoxDuplicateUploadFiles(customerID, reviewType, boxItems, UserID, FileFilter);
        }


        #endregion
    }
}
