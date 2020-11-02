using IntellaLend.CommonServices;
using IntellaLendAPI.Models;
using IntellaLendJWTToken;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IntellaLendAPI.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DashboardController : ApiController
    {
        #region Dashboard Request

        [HttpPost]
        public TokenResponse GetNeedsAttention(DashboardRequest req)
        {
            Logger.WriteTraceLog($"Start GetNeedsAttention()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new DashboardService(req.TableSchema).GetNeedsAttention(req.UserID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetNeedsAttention()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetAuditStatus(DashboardRequest req)
        {
            Logger.WriteTraceLog($"Start GetAuditStatus()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new DashboardService(req.TableSchema).GetAuditStatus(req.RoleID, req.UserID, req.CustomerID, req.FromDate, req.ToDate));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetAuditStatus()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetAuditStatusDrill(DashboardRequest req)
        {
            Logger.WriteTraceLog($"Start GetAuditStatusDrill()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new DashboardService(req.TableSchema).GetAuditStatusDrill(req.RoleID, req.UserID, req.CustomerID, req.FromDate, req.ToDate, req.Type, req.DrillStatusID, req.DrillCustomerID, req.DrillLoanTypeID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetAuditStatusDrill()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetByAuditorChart(DashboardRequest req)
        {
            Logger.WriteTraceLog($"Start GetByAuditorChart()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new DashboardService(req.TableSchema).GetByAuditorChart(req.RoleID, req.UserID, req.CustomerID, req.FromDate, req.ToDate));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetByAuditorChart()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetByAuditorDrillChart(DashboardRequest req)
        {
            Logger.WriteTraceLog($"Start GetByAuditorDrillChart()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new DashboardService(req.TableSchema).GetByAuditorDrillChart(req.RoleID, req.UserID, req.CustomerID, req.FromDate, req.ToDate, req.Type, req.DrillCustomerID, req.DrillAuditorID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetByAuditorDrillChart()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetAuditKpiGoalConfigDetails(AuditKpiRequest request)
        {
            Logger.WriteTraceLog($"Start GetAuditKpiGoalConfigDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new DashboardService(request.TableSchema).GetAuditKpiGoalConfigDetails(request.RoleID,request.UserGroupID,request.FromDate,request.ToDate,request.Flag,request.AuditGoalID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetAuditKpiGoalConfigDetails()");
            return response;
        }
        #endregion
    }
}