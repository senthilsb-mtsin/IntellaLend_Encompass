using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using System;
using System.Configuration;
using System.Net.Http;

namespace IL.EncompassUpload
{
    public class RuleEvaluatedResult
    {
        public RuleEvaluatedResult() { }

        public byte[] GeneratePDF(Int64 LoanID, string TenantSchema)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string baseUri = ConfigurationManager.AppSettings["BASE_URI"];
                    client.BaseAddress = new Uri(baseUri);

                    var response = client.GetAsync($"Image/DownloadLoanAuditReport/{TenantSchema}/{LoanID}").Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return response.Content.ReadAsByteArrayAsync().Result;
                    }
                    else
                        throw new Exception(response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }

            return null;
        }

        public void LogMessage(string _msg)
        {
            Logger.WriteTraceLog(_msg);
        }
    }
}
