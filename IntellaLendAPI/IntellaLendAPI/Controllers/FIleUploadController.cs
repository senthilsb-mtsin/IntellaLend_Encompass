using IntellaLend.BoxWrapper;
using IntellaLend.CommonServices;
using IntellaLend.Constance;
using IntellaLend.Model;
using IntellaLendAPI.Models;
using IntellaLendJWTToken;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IntellaLendAPI.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FileUploadController : ApiController
    {
        private static string[] HeaderParams = { "TableSchema", "UploadFileName", "ReviewType", "LoanType", "UserId", "CustomerID", "DocId", "LoanId", "AuditMonthYear", "PriorityType", "AuditDueDate" };
        private static string filePath = ConfigurationManager.AppSettings["filePath"].ToString();

        [HttpPost]
        public async Task<TokenResponse> FileUploader()
        {

            Logger.WriteTraceLog($"Start FileUploader()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                Dictionary<string, string> paramsValues = GetHeaderValue(Request.Headers);
                if (paramsValues.Count.Equals(9))
                {
                    response.token = new JWTToken().CreateJWTToken();
                    response.data = new JWTToken().CreateJWTToken(new FileUploadService(paramsValues["TableSchema"]).FileUpload(paramsValues, filePath, provider));
                }
                else
                    throw new Exception("Header Parameter Mismatch");
            }
            catch (Exception Exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = Exc.Message;
                MTSExceptionHandler.HandleException(ref Exc);
            }
            Logger.WriteTraceLog($"End FileUploader()");
            return response;
        }


        [HttpPost]
        public async Task<TokenResponse> MissingDocFileUploader()
        {

            Logger.WriteTraceLog($"Start MissingDocFileUploader()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                Dictionary<string, string> paramsValues = GetHeaderValue(Request.Headers);
                if (paramsValues.Count.Equals(5))
                {
                    response.token = new JWTToken().CreateJWTToken();
                    response.data = new JWTToken().CreateJWTToken(new FileUploadService(paramsValues["TableSchema"]).MissingDocFileUploader(paramsValues, filePath, provider));
                }
                else
                    throw new Exception("Header Parameter Mismatch");
            }
            catch (Exception Exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = Exc.Message;
                MTSExceptionHandler.HandleException(ref Exc);
            }
            Logger.WriteTraceLog($"End MissingDocFileUploader()");
            return response;
        }

        [HttpPost]
        public async Task<TokenResponse> MissingDocGlobalFileUploader()
        {

            Logger.WriteTraceLog($"Start MissingDocGlobalFileUploader()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                Dictionary<string, string> paramsValues = GetHeaderValue(Request.Headers);
                if (paramsValues.Count.Equals(5))
                {
                    response.token = new JWTToken().CreateJWTToken();
                    response.data = new JWTToken().CreateJWTToken(new FileUploadService(paramsValues["TableSchema"]).MissingDocGlobalFileUploader(paramsValues, filePath, provider));
                }
                else
                    throw new Exception("Header Parameter Mismatch");
            }
            catch (Exception Exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = Exc.Message;
                MTSExceptionHandler.HandleException(ref Exc);
            }
            Logger.WriteTraceLog($"End MissingDocGlobalFileUploader()");
            return response;
        }



        [HttpPost]
        public TokenResponse CheckUserBoxToken(BoxTokenValidateRequest request)
        {
            Logger.WriteTraceLog($"Start CheckUserBoxToken()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new BoxAPIWrapper(request.TableSchema, request.UserID).CheckUserBoxToken());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End CheckUserBoxToken()");
            return response;
        }

        [HttpPost]
        public TokenResponse GenrateUserBoxToken(BoxTokenValidateRequest request)
        {
            Logger.WriteTraceLog($"Start GenrateUserBoxToken()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new BoxAPIWrapper(request.TableSchema, request.UserID).GenrateToken(request.AuthCode));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End GenrateUserBoxToken()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetFileListFromBox(BoxGetFileListRequest request)
        {
            Logger.WriteTraceLog($"Start GetFileListFromBox()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new BoxAPIWrapper(request.TableSchema, request.UserID).GetFolderDetails(request.FolderID, request.limit, request.offSet, request.FileFilter));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End GetFileListFromBox()");
            return response;
        }

        [HttpPost]
        public TokenResponse UploadFileFromBox(BoxUploadRequest request)
        {
            Logger.WriteTraceLog($"Start UploadFileFromBox()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                Loan loan = new Loan();
                //Hard Coding Till QCIQ access is given
                //loan.ReviewTypeID = 2; // 2 = Post-Closing Audit
                //loan.LoanTypeID = 1; // 1 = Post-Close Conventional Purchase
                loan.ReviewTypeID = request.ReviewType;
                loan.LoanTypeID = request.LoanType;
                loan.UploadedUserID = request.UserID;
                loan.CustomerID = request.CustomerID;
                loan.LastAccessedUserID = 0;
                //loan.FromBox = true;
                loan.UploadType = UploadConstant.BOX;
                loan.CreatedOn = DateTime.Now;
                DateTime AuditDueDate = request.AuditDueDate != null ? Convert.ToDateTime(Convert.ToDateTime(request.AuditDueDate).ToString(IntellaLend.Constance.DateConstance.AuditDateFormat)) : Convert.ToDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(IntellaLend.Constance.DateConstance.AuditDateFormat));

                //   loan.AuditMonthYear = request.AuditMonthYear == null ? Convert.ToDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(IntellaLend.Constance.DateConstance.AuditDateFormat)) : Convert.ToDateTime(new DateTime(request.AuditMonthYear.Value.Year, request.AuditMonthYear.Value.Month, 1).ToString(IntellaLend.Constance.DateConstance.AuditDateFormat));
                loan.AuditMonthYear = request.AuditMonthYear != null ? Convert.ToDateTime(Convert.ToDateTime(request.AuditMonthYear).ToString(IntellaLend.Constance.DateConstance.AuditDateFormat)) : Convert.ToDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(IntellaLend.Constance.DateConstance.AuditDateFormat));
                foreach (var item in request.BoxItems)
                {
                    loan.CreatedOn = DateTime.Now;
                    loan.LoanGUID = Guid.NewGuid();
                    loan.Priority = item.Priority;
                    List<BoxDownloadQueue> boxdownloadList = new List<BoxDownloadQueue>();
                    AddBoxFileUploadDetails(loan, item.Priority, request.TableSchema, request.UserID, item.BoxID, item.ItemType, request.FileFilter, boxdownloadList);
                    new FileUploadService(request.TableSchema).AddBoxFileUploadDetails(loan, boxdownloadList, AuditDueDate);
                }
                response.data = new JWTToken().CreateJWTToken(true);
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End UploadFileFromBox()");
            return response;
        }


        [HttpPost]
        public TokenResponse GetBoxItemCount(BoxUploadRequest request)
        {
            Logger.WriteTraceLog($"Start GetBoxItemCount()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            List<BoxItemsCountsWithDuplicateFileLists> _boxDatas = new List<BoxItemsCountsWithDuplicateFileLists>();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                int itemCount = 0;
                int boxItemActualCount = 0;
                boxItemActualCount = request.BoxItems.Count;
                bool isNonDuplicatesFileUploaded = false;
                //Dictionary<Int64, string> _listItems = new Dictionary<Int64, string>();
                foreach (var item in request.BoxItems)
                {
                    itemCount += GetFolderItemCount(request.TableSchema, request.UserID, item.BoxID, item.ItemType, request.FileFilter);
                }

                List<BoxDuplicatedFilesFolder> _boxDuplicateDatas = new FileUploadService(request.TableSchema).GetBoxDuplicateUploadFiles(request.CustomerID, request.ReviewType, request.BoxItems, request.UserID, request.FileFilter);

                foreach (var item in _boxDuplicateDatas[0].FolderFilesCount)
                {
                    request.BoxItems.RemoveAll(r => r.BoxID == item.FolderID);
                }
                foreach (var item in _boxDuplicateDatas[0].FilesExistsCount)
                {
                    request.BoxItems.RemoveAll(r => r.BoxID == item.Id);
                }
                if (boxItemActualCount != request.BoxItems.Count)
                {
                    if (request.BoxItems.Count != 0)
                    {
                        UploadFileFromBox(request);
                        isNonDuplicatesFileUploaded = true;
                    }

                }
                if (itemCount > 0 || _boxDuplicateDatas != null)
                {
                    _boxDatas.Add(new BoxItemsCountsWithDuplicateFileLists()
                    {
                        BoxItemCounts = itemCount,
                        BoxDuplicateFileList = _boxDuplicateDatas,
                        IsNonDupFileAdded = isNonDuplicatesFileUploaded
                    });
                }
                //_listItems.Add(BoxItemCounts: itemCount, vals: "");
                response.data = new JWTToken().CreateJWTToken(_boxDatas);
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End GetBoxItemCount()");
            return response;
        }


        [HttpPost]
        public TokenResponse GetBoxUploadedItems(BoxUploadSearchRequest request)
        {
            Logger.WriteTraceLog($"Start GetBoxUploadedItems()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new FileUploadService(request.TableSchema).GetBoxUploadedItems(request.FromDate, request.ToDate, request.UserID, request.UploadStatus, request.CustomerID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End GetBoxUploadedItems()");
            return response;
        }


        [HttpPost]
        public TokenResponse BoxFileUploadRetry(BoxUploadResetRequest request)
        {
            Logger.WriteTraceLog($"Start BoxFileUploadRetry()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new FileUploadService(request.TableSchema).BoxFileUploadRetry(request.LoanID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End BoxFileUploadRetry()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetEphesoftURL(EphesoftRequest batchInstance)
        {
            Logger.WriteTraceLog($"Start GetEphesoftURL()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(batchInstance)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new FileUploadService(batchInstance.TableSchema).GetEphesoftURL(batchInstance.EphesoftBatchInstanceID, batchInstance.EphesoftURL, batchInstance.CustomerID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End GetEphesoftURL()");
            return response;


        }

        #region Private Methods


        private int GetFolderItemCount(string TableSchema, Int64 UserID, string BoxID, string ItemType, string FileFilter)
        {
            Logger.WriteTraceLog($"Start GetFolderItemCount()");
            int itemCount = 0;
            BoxAPIWrapper wrap = new BoxAPIWrapper(TableSchema, UserID);

            if (ItemType == "file")
            {
                itemCount++;
            }
            else
            {
                BoxCollection colDetails = new BoxAPIWrapper(TableSchema, UserID).GetFolderDetails(BoxID, 1, 0, FileFilter);
                for (int offset = 0; offset < colDetails.TotalCount; offset = offset + 500)
                {
                    BoxCollection col = new BoxAPIWrapper(TableSchema, UserID).GetFolderDetails(BoxID, 500, offset, FileFilter);
                    foreach (var item in col.BoxEntities)
                    {
                        if (item.Type == "folder")
                        { itemCount += GetFolderItemCount(TableSchema, UserID, item.Id, item.Type, FileFilter); }
                        else
                            itemCount++;
                    }
                }
            }
            Logger.WriteTraceLog($"End GetFolderItemCount()");
            return itemCount;
        }


        private bool AddBoxFileUploadDetails(Loan loan, Int64 priority, string TableSchema, Int64 UserID, string BoxID, string ItemType, string FileFilter, List<BoxDownloadQueue> boxdownloadList)
        {
            Logger.WriteTraceLog($"Start AddBoxFileUploadDetails()");
            BoxAPIWrapper wrap = new BoxAPIWrapper(TableSchema, UserID);

            if (ItemType == "file")
            {
                BoxEntity file = wrap.GetFileDetails(BoxID);
                BoxDownloadQueue boxq = new BoxDownloadQueue();
                boxq.BoxEntityID = file.Id;
                boxq.BoxFileName = file.Name;
                boxq.BoxFilePath = file.ParentPath;
                boxq.FileSize = file.Size;
                boxq.Priority = priority;
                boxdownloadList.Add(boxq);
            }
            else
            {
                BoxCollection colDetails = new BoxAPIWrapper(TableSchema, UserID).GetFolderDetails(BoxID, 1, 0, FileFilter);
                for (int offset = 0; offset < colDetails.TotalCount; offset = offset + 500)
                {
                    BoxCollection col = new BoxAPIWrapper(TableSchema, UserID).GetFolderDetails(BoxID, 500, offset, FileFilter);
                    foreach (var item in col.BoxEntities)
                    {
                        AddBoxFileUploadDetails(loan, priority, TableSchema, UserID, item.Id, item.Type, FileFilter, boxdownloadList);
                    }
                }
            }
            Logger.WriteTraceLog($"End AddBoxFileUploadDetails()");
            return true;
        }


        private Dictionary<string, string> GetHeaderValue(HttpRequestHeaders Headers)
        {
            Logger.WriteTraceLog($"Start GetHeaderValue()");
            Dictionary<string, string> paramsValue = new Dictionary<string, string>();

            foreach (string header in HeaderParams)
            {
                if (Headers.Contains(header))
                    paramsValue.Add(header, Headers.GetValues(header).FirstOrDefault().ToString());
            }
            Logger.WriteTraceLog($"End GetHeaderValue()");
            return paramsValue;
        }

        #endregion
    }
}