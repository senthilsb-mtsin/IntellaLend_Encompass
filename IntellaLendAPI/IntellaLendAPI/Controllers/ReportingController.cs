﻿using IntellaLend.ReportingService;
using IntellaLendAPI.Models;
using IntellaLendJWTToken;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;


namespace IntellaLendAPI.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReportingController : ApiController
    {


        [HttpPost]
        public async Task<TokenResponse> GetDashboardGraph(ReportRequest req)
        {
            Logger.WriteTraceLog($"Start GetDashboardGraph()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken(Request.Headers.GetValues("HashValue").FirstOrDefault().ToString(), Request.Headers.GetValues("TenantDBSchema").FirstOrDefault().ToString());
                object result = await new ReportingService(req.TableSchema).GetDashboardGraph(req.ReportType, req.ReportModel);
                response.data = new JWTToken().CreateJWTToken(result);
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetDashboardGraph()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetDrilldownGrid(ReportRequest req)
        {
            Logger.WriteTraceLog($"Start GetDrilldownGrid()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken(Request.Headers.GetValues("HashValue").FirstOrDefault().ToString(), Request.Headers.GetValues("TenantDBSchema").FirstOrDefault().ToString());
                response.data = new JWTToken().CreateJWTToken(new ReportingService(req.TableSchema).GetDrilldownGrid(req.ReportType, req.ReportModel));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetDrilldownGrid()");
            return response;
        }


    }
}