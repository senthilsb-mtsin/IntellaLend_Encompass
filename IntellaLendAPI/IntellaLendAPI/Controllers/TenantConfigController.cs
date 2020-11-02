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
    public class TenantConfigController : ApiController
    {

        [HttpPost]
        public TokenResponse AddTenantConfigType(TenantAddConfigRequest req)
        {
            Logger.WriteTraceLog($"Start AddTenantConfigType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(req.TableSchema).AddTenantConfigType(req.TenantConfigType));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End AddTenantConfigType()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetAllTenantConfigTypes(TenantAddConfigRequest reqTenantConfig)
        {
            Logger.WriteTraceLog($"Start GetAllTenantConfigTypes()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqTenantConfig)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(reqTenantConfig.TableSchema).GetAllTenantConfigTypes(reqTenantConfig.TenantConfigType.CustomerID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetAllTenantConfigTypes()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateTenantConfigType(TenantAddConfigRequest reqTenantConfig)
        {
            Logger.WriteTraceLog($"Start UpdateTenantConfigType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqTenantConfig)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(reqTenantConfig.TableSchema).UpdateTenantConfigType(reqTenantConfig.TenantConfigType));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End UpdateTenantConfigType()");
            return response;
        }


        [HttpPost]
        public TokenResponse DeleteTenantConfigType(TenantAddConfigRequest reqTenantConfig)
        {
            Logger.WriteTraceLog($"Start DeleteTenantConfigType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqTenantConfig)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(reqTenantConfig.TableSchema).DeleteTenantConfigType(reqTenantConfig.TenantConfigType.ConfigID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End DeleteTenantConfigType()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetConfigValues(TenantConfigRequest req)
        {
            Logger.WriteTraceLog($"Start GetConfigValues()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(req.TableSchema).GetConfigValues(req.CustomerID, req.ConfigKey));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetConfigValues()");
            return response;
        }

        [HttpPost]
        public TokenResponse BoxSettingsConfig(BoxSettingsConfig req)
        {
            Logger.WriteTraceLog($"Start BoxSettingsConfig()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(req.TableSchema).BoxSettingsConfig(req.ClientId, req.ClientSecretId, req.BoxUserID, req.isUpdate));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End BoxSettingsConfig()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetAllBoxSettingsConfig(CommonListRequest req)
        {
            Logger.WriteTraceLog($"Start GetAllBoxSettingsConfig()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(req.TableSchema).GetAllBoxSettingsConfig());
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetAllBoxSettingsConfig()");
            return response;
        }



        [HttpPost]
        public TokenResponse GetAllAuditConfig(RequestGetAllAuditConfig req)
        {
            Logger.WriteTraceLog($"Start GetAllAuditConfig()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(req.TableSchema).GetAllAuditConfig());
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetAllAuditConfig()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateAuditConfig(RequestUpdateAuditConfig req)
        {
            Logger.WriteTraceLog($"Start UpdateAuditConfig()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(req.TableSchema).UpdateAuditConfig(req.AuditConfig));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End UpdateAuditConfig()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetallCategory(GetAppConfig req)
        {
            Logger.WriteTraceLog($"Start GetallCategory()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(req.TableSchema).GetAllcategoryLists());
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetallCategory()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateCategoryGroup(CategoryList req)
        {
            Logger.WriteTraceLog($"Start UpdateCategoryGroup()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(req.TableSchema).SaveandUpdateCategory(req.categoryList));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End UpdateCategoryGroup()");
            return response;

        }

        [HttpPost]
        public TokenResponse SaveCategoryGroup(SaveCategoryList req)
        {
            Logger.WriteTraceLog($"Start SaveCategoryGroup()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(req.TableSchema).SaveCategoryGroup(req.Category, req.Active));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SaveCategoryGroup()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetMasterReport(GetAppConfig req)
        {
            Logger.WriteTraceLog($"Start GetMasterReport()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(req.TableSchema).GetReportMaster());
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetMasterReport()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetDocumentsList(GetAppConfig req)
        {
            Logger.WriteTraceLog($"Start GetDocumentsList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(req.TableSchema).GetDocsList());
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetDocumentsList()");
            return response;
        }

        [HttpPost]
        public TokenResponse SaveReportConfig(SaveReportConfig config)
        {
            Logger.WriteTraceLog($"Start SaveReportConfig()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(config)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(config.TableSchema).SaveReportConfigData(config.docName, config.MasterID, config.ServiceType));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SaveReportConfig()");
            return response;
        }

        [HttpPost]
        public TokenResponse DeleteReportConfig(SaveReportConfig config)
        {
            Logger.WriteTraceLog($"Start DeleteReportConfig()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(config)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(config.TableSchema).DeleteReportConfig(config.MasterID, config.docName));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End DeleteReportConfig()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetStipulationList(GetAppConfig req)
        {
            Logger.WriteTraceLog($"Start GetStipulationList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(req.TableSchema).GetInvestorStipulationList());
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetStipulationList()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetActiveInvestorStipulationList(GetAppConfig req)
        {
            Logger.WriteTraceLog($"Start GetActiveInvestorStipulationList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(req.TableSchema).GetActiveInvestorStipulationList());
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetActiveInvestorStipulationList()");
            return response;
        }

        [HttpPost]
        public TokenResponse SaveInvestorStipulation(SaveStipulation stipulation)
        {
            Logger.WriteTraceLog($"Start SaveInvestorStipulation()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(stipulation)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(stipulation.TableSchema).SaveInvestorStipulation(stipulation.SCategory, stipulation.Active));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SaveInvestorStipulation()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateInvestorStipulation(UpdateStipulation stipulation)
        {
            Logger.WriteTraceLog($"Start UpdateInvestorStipulation()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(stipulation)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(stipulation.TableSchema).UpdateInvestorStipulation(stipulation.ID, stipulation.SCategory, stipulation.Active));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End UpdateInvestorStipulation()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetLoanSearchFilterConfigValue(TenantConfigRequest req)
        {
            Logger.WriteTraceLog($"Start GetLoanSearchFilterConfigValue()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(req.TableSchema).GetLoanSearchFilterConfigValue(req.CustomerID, req.ConfigKey));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetLoanSearchFilterConfigValue()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetLoanSearchFilterValues(TenantConfigRequest req)
        {
            Logger.WriteTraceLog($"Start GetLoanSearchFilterValues()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(req.TableSchema).GetLoanSearchFilterValues(req.CustomerID, req.ConfigKey));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetLoanSearchFilterValues()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateLoanSearchFilterConfig(TenantUpdateConfigRequest req)
        {
            Logger.WriteTraceLog($"Start UpdateLoanSearchFilterConfig()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new TenantConfigService(req.TableSchema).UpdateLoanSearchFilterConfig(req.ConfigID, req.Active));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End UpdateLoanSearchFilterConfig()");
            return response;
        }

    }
}