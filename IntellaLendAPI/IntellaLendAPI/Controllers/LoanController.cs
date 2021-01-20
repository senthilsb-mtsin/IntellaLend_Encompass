using IntellaLend.CommonServices;
using IntellaLend.Model;
using IntellaLendAPI.Models;
using IntellaLendJWTToken;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using static IntellaLendAPI.Models.ELoanRequest;

namespace IntellaLendAPI.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoanController : ApiController
    {
        #region Loan

        [HttpPost]
        public TokenResponse GetLoans(LoanSearchRequest loan)
        {
            Logger.WriteTraceLog($"Start GetLoans()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loan)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                bool isWorkFlow = false;
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(loan.TableSchema).GetLoans(
                     loan.FromDate,
                     loan.ToDate,
                     loan.CurrentUserID,
                     loan.LoanNumber,
                     loan.LoanType,
                     loan.BorrowerName,
                     loan.LoanAmount,
                     loan.ReviewStatus,
                     loan.AuditMonthYear,
                     loan.ReviewType,
                     loan.Customer,
                     loan.PropertyAddress,
                     loan.InvestorLoanNumber,
                     loan.PostCloser,
                     loan.LoanOfficer,
                     loan.UnderWriter,
                     isWorkFlow,
                     loan.AuditDueDate,
                     loan.SelectedLoanStatus
                    ));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End GetLoans()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetWorkFlowLoans(LoanSearchRequest loan)
        {
            Logger.WriteTraceLog($"Start GetWorkFlowLoans()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loan)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                bool isWorkFlow = true;
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(loan.TableSchema).GetLoans(
                     loan.FromDate,
                     loan.ToDate,
                     loan.CurrentUserID,
                     loan.LoanNumber,
                     loan.LoanType,
                     loan.BorrowerName,
                     loan.LoanAmount,
                     loan.ReviewStatus,
                     loan.AuditMonthYear,
                     loan.ReviewType,
                     loan.Customer,
                     loan.PropertyAddress,
                     loan.InvestorLoanNumber,
                     loan.PostCloser,
                     loan.LoanOfficer,
                     loan.UnderWriter,
                     isWorkFlow,
                     loan.AuditDueDate,
                     loan.SelectedLoanStatus
                    ));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End GetWorkFlowLoans()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetRetentionLoans(LoanSearchRequest loansearch)
        {
            Logger.WriteTraceLog($"Start GetRetentionLoans()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loansearch)}");
            TokenResponse response = new TokenResponse();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(loansearch.TableSchema).GetRetentionLoans(loansearch.FromDate, loansearch.ToDate));
            }

            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetRetentionLoans()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetFannieMaeFields(GetFieldRequest _fannieMaeFields)
        {
            Logger.WriteTraceLog($"Start GetFannieMaeFields()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(_fannieMaeFields)}");
            TokenResponse response = new TokenResponse();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(_fannieMaeFields.TableSchema).GetFannieMaeFields(_fannieMaeFields.LoanID));
            }

            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetFannieMaeFields()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetDashRetentionLoans(DashSearchRequest req)
        {
            Logger.WriteTraceLog($"Start GetDashRetentionLoans()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(req.TableSchema).GetRetentionLoans(req.AuditMonthYear));
            }

            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetDashRetentionLoans()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetLoanAudit(LoanAuditRequest req)
        {
            Logger.WriteTraceLog($"Start GetLoanAudit()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(req.TableSchema).GetLoanAudit(req.LoanID));
            }

            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetLoanAudit()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetLoan(DirectLoanRequest req)
        {
            Logger.WriteTraceLog($"Start GetDashRetentionLoans()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(req.TableSchema).GetLoan(req.EncryptedLoanGUID));
            }

            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetDashRetentionLoans()");
            return response;
        }

        [HttpPost]
        public TokenResponse PurgeStaging(RequestLoanPurge reqloanpurge)
        {
            Logger.WriteTraceLog($"Start PurgeStaging()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqloanpurge)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(reqloanpurge.TableSchema).PurgeStaging(reqloanpurge.purgeStaging, reqloanpurge.LoanID, reqloanpurge.UserName));

            }


            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End PurgeStaging()");
            return response;
        }
        [HttpPost]

        public TokenResponse GetPurgeStagingDetails(RequestPurgeStagingDetails reqPurgeStagingDetails)
        {
            Logger.WriteTraceLog($"Start GetPurgeStagingDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqPurgeStagingDetails)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(reqPurgeStagingDetails.TableSchema).GetPurgeStagingDetails(reqPurgeStagingDetails.BatchID));
            }

            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetPurgeStagingDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetLoanDetails(LoanRequest loan)
        {
            Logger.WriteTraceLog($"Start GetLoanDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loan)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(loan.TableSchema).GetLoanDetails(loan.LoanID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End GetLoanDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetEvaluatedChecklist(LoanRequest loan)
        {
            Logger.WriteTraceLog($"Start GetEvaluatedChecklist()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loan)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(loan.TableSchema).GetEvaluatedChecklist(loan.LoanID, loan.ReRun));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End GetEvaluatedChecklist()");
            return response;
        }


        [HttpPost]
        public TokenResponse SetLoanPickUpUser(LoanPickUpUserRequest loanPickUp)
        {
            Logger.WriteTraceLog($"Start SetLoanPickUpUser()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loanPickUp)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                new LoanService(loanPickUp.TableSchema).SetLoanPickUpUser(loanPickUp.LoanID, loanPickUp.PickUpUserID);
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End SetLoanPickUpUser()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetPurgeMonitor(RequestPurgeMonitorData reqPurgeMdata)
        {
            Logger.WriteTraceLog($"Start GetPurgeMonitor()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqPurgeMdata)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(reqPurgeMdata.TableSchema).GetPurgeMonitor(reqPurgeMdata.FromDate, reqPurgeMdata.ToDate, reqPurgeMdata.WorkFlowStatus));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetPurgeMonitor()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetMissingDocStatus(LoanRequest req)
        {
            Logger.WriteTraceLog($"Start GetMissingDocStatus()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(req.TableSchema).GetMissingDocStatus(req.LoanID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetMissingDocStatus()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetMissingDocVersion(LoanDocVersionRequest req)
        {
            Logger.WriteTraceLog($"Start GetMissingDocVersion()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(req.TableSchema).GetMissingDocVersion(req.LoanID, req.DocumentID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetMissingDocVersion()");
            return response;
        }


        [HttpPost]
        public TokenResponse RemoveLoanLoggedUser(LoanRequest loan)
        {
            Logger.WriteTraceLog($"Start RemoveLoanLoggedUser()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loan)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                new LoanService(loan.TableSchema).RemoveLoanLoggedUser(loan.LoanID);
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End RemoveLoanLoggedUser()");
            return response;
        }

        [HttpPost]
        public TokenResponse CheckCurrentLoanUser(CheckCurrentLoanUserRequest loan)
        {
            Logger.WriteTraceLog($"Start CheckCurrentLoanUser()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loan)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(loan.TableSchema).CheckCurrentLoanUser(loan.LoanID, loan.CurrentUserID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End CheckCurrentLoanUser()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetImageByID(ImageRequest img)
        {
            Logger.WriteTraceLog($"Start GetImageByID()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(img)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                //response.data = new JWTToken().CreateJWTToken(new LoanService(img.TableSchema).GetImageByID(img.ImageID));
                response.data = JsonConvert.SerializeObject(new LoanService(img.TableSchema).GetImageByID(img.ImageID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End GetImageByID()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetLoanImageByID(ImageRequest img)
        {
            Logger.WriteTraceLog($"Start GetLoanImageByID()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(img)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                //response.data = new JWTToken().CreateJWTToken(new LoanService(img.TableSchema).GetImageByID(img.LoanID, img.DocumentID, img.ImageID));
                response.data = JsonConvert.SerializeObject(new LoanService(img.TableSchema).GetImageByID(img.LoanID, img.DocumentID, img.ImageID, img.VersionNumber));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End GetLoanImageByID()");
            return response;
        }


        [HttpPost]
        public TokenResponse GetLoanBase64ImageByPageNo(ImageRequest img)
        {
            Logger.WriteTraceLog($"Start GetLoanBase64ImageByPageNo()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(img)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = JsonConvert.SerializeObject(new LoanService(img.TableSchema).GetLoanBase64ImageByPageNo(img.LoanID, img.DocumentID, img.VersionNumber.ToString(), img.PageNo));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End GetLoanBase64ImageByPageNo()");
            return response;
        }


        [HttpPost]
        public TokenResponse GetLoanBase64Images(ImageRequest img)
        {
            Logger.WriteTraceLog($"Start GetLoanBase64Images()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(img)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = JsonConvert.SerializeObject(new LoanService(img.TableSchema).GetLoanBase64Images(img.LoanID, img.DocumentID, img.VersionNumber, img.PageNo, img.LastPageNumber, img.ShowAllDocs));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End GetLoanBase64Images()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetLoanDocInfo(LoanDocumentRequest loanDoc)
        {
            Logger.WriteTraceLog($"Start GetLoanDocInfo()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loanDoc)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(loanDoc.TableSchema).GetLoanDocInfo(loanDoc.LoanID, loanDoc.DocumentID, loanDoc.VersionNumber));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetLoanDocInfo()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetLoanReverifyDoc(LoanReverifyDocumentRequest loanDoc)
        {
            Logger.WriteTraceLog($"Start GetLoanReverifyDoc()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loanDoc)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(loanDoc.TableSchema).GetLoanReverifyDoc(loanDoc.LoanID, loanDoc.DocumentID, loanDoc.VersionNumber));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetLoanReverifyDoc()");
            return response;
        }


        [HttpPost]
        public TokenResponse GetLoanNotesDetails(LoanRequest loan)
        {
            Logger.WriteTraceLog($"Start GetLoanNotesDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loan)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(loan.TableSchema).GetLoanNotes(loan.LoanID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End GetLoanNotesDetails()");
            return response;
        }

        [HttpPost]
        public HttpResponseMessage DownloadLoanPDF(LoanRequest loan)
        {
            Logger.WriteTraceLog($"Start DownloadLoanPDF()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loan)}");
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                Loan loanObj = new LoanService(loan.TableSchema).GetLoanHeaderDeatils(loan.LoanID);
                response.Content = new ByteArrayContent(new LoanService(loan.TableSchema).GetLoanPDF(loan.LoanID));
                response.Content.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = String.Format("{0}.pdf", string.IsNullOrEmpty(loanObj.LoanNumber) ? string.Empty : loanObj.LoanNumber)
                    };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {

                response.StatusCode = HttpStatusCode.NoContent;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End DownloadLoanPDF()");
            return response;
        }


        [HttpPost]
        public HttpResponseMessage DownloadDocumentPDF(DocumentDownloadRequest doc)
        {
            Logger.WriteTraceLog($"Start DownloadDocumentPDF()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(doc)}");
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response.Content = new ByteArrayContent(new LoanService(doc.TableSchema).GetDocumentPDF(doc.LoanID, doc.DocumentID, doc.DocumentVersion));
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {

                response.StatusCode = HttpStatusCode.NoContent;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End DownloadDocumentPDF()");
            return response;
        }

        [HttpPost]
        public HttpResponseMessage DownloadCheckListExcel(ChecklistDownloadRequest req)
        {
            Logger.WriteTraceLog($"Start DownloadCheckListExcel()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                string xlsFileStr = new LoanService(req.TableSchema).GetXlFileString(req.LoanID, req.CheckList);
                response.Content = new ByteArrayContent(Encoding.UTF8.GetBytes(xlsFileStr));
                response.Content.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = ""
                    };
                //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {

                response.StatusCode = HttpStatusCode.NoContent;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End DownloadCheckListExcel()");
            return response;
        }


        [HttpPost]
        public TokenResponse UpdateLoanDocument(LoanDocUpdateRequest loan)
        {
            Logger.WriteTraceLog($"Start UpdateLoanDocument()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loan)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(loan.TableSchema).UpdateLoanDocument(loan.LoanID, loan.DocumentID, loan.CurrentUserID, loan.DocumentFields, loan.VersionNumber, loan.DocumentTables));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End UpdateLoanDocument()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateLoanMonitor(LoanTypeMonitorUpdateRequest loantypeReq)
        {
            Logger.WriteTraceLog($"Start UpdateLoanMonitor()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loantypeReq)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                string ephesoftPath = string.Empty;
                if (ConfigurationManager.AppSettings["ephesoftOutputPath"] != null && ConfigurationManager.AppSettings["ephesoftOutputPath"].ToString() != String.Empty)
                {
                    ephesoftPath = ConfigurationManager.AppSettings["ephesoftOutputPath"].ToString();
                }
                else
                {
                    throw new Exception("Ephesoft Output Path Not Found");
                }

                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(loantypeReq.TableSchema).UpdateLoanMonitor(loantypeReq.LoanID, loantypeReq.LoanTypeID, loantypeReq.UserName, ephesoftPath));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End UpdateLoanMonitor()");
            return response;
        }

        //LoanNotesUpdateRequest
        [HttpPost]
        public TokenResponse UpdateLoanNotesDetails(LoanNotesUpdateRequest loanNotes)
        {
            Logger.WriteTraceLog($"Start UpdateLoanNotesDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loanNotes)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(loanNotes.TableSchema).UpdateLoanNotes(loanNotes.LoanID, loanNotes.Notes));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateLoanNotesDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse DeleteOutputFolder(DeleteLoanRequest loantypeReq)
        {
            Logger.WriteTraceLog($"Start DeleteOutputFolder()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loantypeReq)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                string ephesoftPath = string.Empty;
                if (ConfigurationManager.AppSettings["ephesoftOutputPath"] != null && ConfigurationManager.AppSettings["ephesoftOutputPath"].ToString() != String.Empty)
                {
                    ephesoftPath = ConfigurationManager.AppSettings["ephesoftOutputPath"].ToString();
                }
                else
                {
                    throw new Exception("Ephesoft Output Path Not Found");
                }

                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(loantypeReq.TableSchema).DeleteOutputFolder(loantypeReq.LoanID, ephesoftPath));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End DeleteOutputFolder()");
            return response;
        }

        [HttpPost]
        public TokenResponse CheckLoanPDFExists(LoanRequest loan)
        {
            Logger.WriteTraceLog($"Start CheckLoanPDFExists()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loan)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(loan.TableSchema).CheckLoanPDFExists(loan.LoanID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End CheckLoanPDFExists()");
            return response;
        }

        [HttpPost]
        public TokenResponse ChangeDocumentType(DocChangeRequest doc)
        {
            Logger.WriteTraceLog($"Start ChangeDocumentType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(doc)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(doc.TableSchema).ChangeDocumentType(doc.LoanID, doc.OldDocumentID, doc.NewDocumentID, doc.VersionNumber, doc.CurrentUserID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End ChangeDocumentType()");
            return response;
        }

        [HttpPost]

        public TokenResponse RetryPurge(RequestRetryLoanPurge reqRetryLoanPurge)
        {
            Logger.WriteTraceLog($"Start RetryPurge()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqRetryLoanPurge)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                //response.data = new JWTToken().CreateJWTToken(new LoanService(reqRetryLoanPurge.TableSchema).RetryPurge(reqRetryLoanPurge.BatchIDs, reqRetryLoanPurge.LoanID));
                response.data = new JWTToken().CreateJWTToken(new LoanService(reqRetryLoanPurge.TableSchema).RetryPurge(reqRetryLoanPurge.BatchIDs));
            }

            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End RetryPurge()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateLoanQuestioner(LoanQuestionerUpdateRequest request)
        {
            Logger.WriteTraceLog($"Start UpdateLoanQuestioner()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).UpdateLoanQuestioner(request.LoanID, request.Questioners, request.CurrentUserID));
            }

            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateLoanQuestioner()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetLoanReverification(RequestLoanReverification req)
        {
            Logger.WriteTraceLog($"Start GetLoanReverification()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(req.TableSchema).GetLoanReverification(req.LoanID));
            }

            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetLoanReverification()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetLoanBasedReverification(RequestLoanReverification req)
        {
            Logger.WriteTraceLog($"Start GetLoanBasedReverification()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(req.TableSchema).GetLoanBasedReverification(req.LoanID));
            }

            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetLoanBasedReverification()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetFieldsByDocID(RequestDocFields req)
        {
            Logger.WriteTraceLog($"Start GetFieldsByDocID()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(req.TableSchema).GetFieldsByDocID(req.DocumentID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetFieldsByDocID()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetFieldValue(RequestFieldValue req)
        {
            Logger.WriteTraceLog($"Start GetFieldValue()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(req.TableSchema).GetFieldValue(req.LoanID, req.DocumentName, req.DocumentField));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetFieldValue()");
            return response;
        }

        [HttpPost]
        public TokenResponse DeleteLoan(DeleteLoans _reqloan)
        {
            Logger.WriteTraceLog($"Start DeleteLoan()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(_reqloan)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(_reqloan.TableSchema).DeleteLoan(_reqloan.LoanID, _reqloan.userName));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End DeleteLoan()");
            return response;
        }

        [HttpPost]
        public TokenResponse DeletedReverification(RequestDeletedReverification req)
        {
            Logger.WriteTraceLog($"Start DeletedReverification()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(req.TableSchema).DeletedReverification(req.LoanReverificationID, req.LoanID, req.ReverificationName, req.UserName));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End DeletedReverification()");
            return response;
        }

        [HttpPost]
        public HttpResponseMessage DownloadLoanReverification(RequestDownloadReverification req)
        {
            Logger.WriteTraceLog($"Start DownloadLoanReverification()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response.Content = new ByteArrayContent(new LoanService(req.TableSchema).GetReverificationLoanPDF(req.LoanID, req.ReverificationMappingID, req.TemplateFieldJson, req.UserID, req.RequiredDocIDs, req.IsCoverLetterReq, req.ReverificationName));
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {

                response.StatusCode = HttpStatusCode.NoContent;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End DownloadLoanReverification()");
            return response;
        }

        [HttpPost]
        public HttpResponseMessage DownloadReverification(RequestDownloadInitReverification req)
        {
            Logger.WriteTraceLog($"Start DownloadReverification()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response.Content = new ByteArrayContent(new LoanService(req.TableSchema).GetReverificationLoanPDF(req.LoanReverificationID));
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.NoContent;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End DownloadReverification()");
            return response;
        }

        [HttpPost]
        public TokenResponse LoanComplete(LoanRequest req)
        {
            Logger.WriteTraceLog($"Start LoanComplete()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                new LoanService(req.TableSchema).LoanComplete(req.LoanID, req.CompletedUserRoleID, req.CompletedUserID, req.CompleteNotes);
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End LoanComplete()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateLoanDetails(UpdateLoanNumberRequest updateReq)
        {
            Logger.WriteTraceLog($"Start UpdateLoanDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(updateReq)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(updateReq.TableSchema).UpdateLoanDetails(updateReq.LoanID, updateReq.LoanDetails, updateReq.UserName, updateReq.Type));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateLoanDetails()");
            return response;
        }
        [HttpPost]
        public TokenResponse UpdateLoanHeader(UpdateLoanHeader updateReq)
        {
            Logger.WriteTraceLog($"Start UpdateLoanHeader()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(updateReq)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(updateReq.TableSchema).UpdateLoanHeader(updateReq.LoanID, updateReq.LoanDetails, updateReq.UserName));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateLoanHeader()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetEmailTrackerDetails(EmailTrackRequest request)
        {
            Logger.WriteTraceLog($"Start GetEmailTrackerDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).GetEmailTrackerDetails(request.emailtracker));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetEmailTrackerDetails()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetEmailTracker(EmailTrackRequest request)
        {
            Logger.WriteTraceLog($"Start GetEmailTracker()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).GetEmailTracker());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetEmailTracker()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetCurrentData(EmailTrackRequest request)
        {
            Logger.WriteTraceLog($"Start GetCurrentData()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).GetCurrentData(request.ID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetCurrentData()");
            return response;
        }
        [HttpPost]
        public TokenResponse SendEmailDetails(SendEmailRequest request)
        {
            Logger.WriteTraceLog($"Start SendEmailDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).SendEmailDetails(request.To, request.Subject, request.Attachement, request.Body, request.UserID, request.SendBy, request.LoanID, request.AttachmentsName));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SendEmailDetails()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetEmailTrackerByLoanID(SendEmailRequest request)
        {
            Logger.WriteTraceLog($"Start GetEmailTrackerByLoanID()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).GetDataByLoanId(request.LoanID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetEmailTrackerByLoanID()");
            return response;
        }
        #endregion

        [HttpPost]
        public TokenResponse SaveLoanStipulations(LoanStipulationRequest request)
        {
            Logger.WriteTraceLog($"Start SaveLoanStipulations()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).SaveLoanStipulations(request.LoanID, request.LoanStipulationDetails, request.UserName));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SaveLoanStipulations()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetLoanStipulationDetails(LoanStipulationRequest request)
        {
            Logger.WriteTraceLog($"Start GetLoanStipulationDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).GetLoanStipulationDetails(request.LoanID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetLoanStipulationDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateLoanStipulations(LoanStipulationRequest request)
        {
            Logger.WriteTraceLog($"Start UpdateLoanStipulations()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).UpdateLoanStipulations(request.LoanID, request.LoanStipulationDetails, request.UserName));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateLoanStipulations()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetReviewTypeSearchCriteria(StuplationReportRequest req)
        {
            Logger.WriteTraceLog($"Start GetReviewTypeSearchCriteria()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(req.TableSchema).GetReviewTypeSearchCriteria(req.ReviewTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetReviewTypeSearchCriteria()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetLoanInvestorByAuditMonth(StuplationReportRequest req)
        {
            Logger.WriteTraceLog($"Start GetLoanInvestorByAuditMonth()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(req.TableSchema).GetReviewTypeSearchCriteria(req.ReviewTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetLoanInvestorByAuditMonth()");
            return response;
        }

        [HttpPost]
        public TokenResponse AssignUser(AssignUserRequest req)
        {
            Logger.WriteTraceLog($"Start AssignUser()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {

                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(req.TableSchema).AssignUser(req.LoanID, req.AssignedUserID, req.CurrentUserID, req.ServiceTypeName, req.AssignedBy, req.AssignedTo));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End AssignUser()");
            return response;
        }
        [HttpPost]
        public TokenResponse SaveLoanAuditDueDate(UpdateLoanAudit LoanRequest)
        {
            Logger.WriteTraceLog($"Start SaveLoanAuditDueDate()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(LoanRequest)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {

                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(LoanRequest.TableSchema).SaveLoanAuditDueDate(LoanRequest.LoanID, LoanRequest.AuditDueDate));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SaveLoanAuditDueDate()");
            return response;
        }
        [HttpPost]
        public TokenResponse ResendEmail(ResendEmailRequest resend)
        {
            Logger.WriteTraceLog($"Start ResendEmail()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(resend)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(resend.TableSchema).ResendEmail(resend.ID));

            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }


            Logger.WriteTraceLog($"End ResendEmail()");
            return response;

        }

        [HttpPost]
        public TokenResponse RevertToReadyForAudit(LoanRequest req)
        {
            Logger.WriteTraceLog($"Start RevertToReadyForAudit()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(req.TableSchema).UpdateLoanStatus(req.LoanID, req.UserName));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End RevertToReadyForAudit()");
            return response;
        }
        //[HttpPost]
        //public TokenResponse GetEncompassException(EncompassExceptionRequest request)
        //{
        //    TokenResponse response = new TokenResponse();
        //    response.ResponseMessage = new ResponseMessage();
        //    try
        //    {
        //        response.token = new JWTToken().CreateJWTToken();
        //        response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).GetEncompassException());
        //    }
        //    catch (Exception ex)
        //    {
        //        response.token = null;
        //        response.ResponseMessage.MessageDesc = ex.Message;
        //        MTSExceptionHandler.HandleException(ref ex);
        //    }
        //    return response;
        //}
        public TokenResponse RetryEncompassException(RetryEncompassExceptionRequest retry)
        {
            Logger.WriteTraceLog($"Start RetryEncompassException()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(retry)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(retry.TableSchema).RetryException(retry.EncompassExceptionID));

            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }


            Logger.WriteTraceLog($"End RetryEncompassException()");
            return response;

        }


        [HttpPost]
        public TokenResponse GetEncompassExceptionDetails(EncompassExceptionRequest request)
        {
            Logger.WriteTraceLog($"Start GetEncompassExceptionDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).GetEncompassExceptionDetails(request.encompassException));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetEncompassExceptionDetails()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetBoxDownloadExceptionDetails(BoxDownloadExceptionRequest request)
        {
            Logger.WriteTraceLog($"Start GetBoxDownloadExceptionDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).GetBoxDownloadExceptionDetails(request.boxDownloadException));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetBoxDownloadExceptionDetails()");
            return response;
        }
        [HttpPost]
        public TokenResponse RetryBoxException(RetryBoxExceptionRequest request)
        {
            Logger.WriteTraceLog($"Start RetryBoxException()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).RetryBoxException(request.ID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End RetryBoxException()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetLoanMissingDocuments(MissingDocRequest req)
        {
            Logger.WriteTraceLog($"Start GetLoanMissingDocuments()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(req.TableSchema).GetLoanMissingDocuments(req.LoanID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetLoanMissingDocuments()");
            return response;
        }

        [HttpPost]
        public TokenResponse DocumentObsolete(DocumetnObsoleteRequest req)
        {
            Logger.WriteTraceLog($"Start DocumentObsolete()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(req.TableSchema).DocumentObsolete(req.LoanID, req.DocumentID, req.DocumentVersion, req.IsObsolete, req.CurrentUserID, req.DocName));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End DocumentObsolete()");
            return response;
        }
        [HttpPost]
        public TokenResponse RetryEncompassLoanDownload(EDownloadRetryRequest request)
        {
            Logger.WriteTraceLog($"Start RetryEncompassLoanDownload()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).RetryEncompassLoanDownload(request.LoanID, request.EDownloadID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End RetryEncompassLoanDownload()");
            return response;
        }
    }
}