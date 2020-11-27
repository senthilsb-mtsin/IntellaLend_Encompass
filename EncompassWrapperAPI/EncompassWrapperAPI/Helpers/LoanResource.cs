using EncompassRequestBody.CustomModel;
using EncompassRequestBody.WrapperReponseModel;
using EncompassWrapperConstants;
using MTS.Web.Helpers;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;

namespace EncompassWrapperAPI
{
    public class LoanResource
    {

        public static LockResourceModel LockLoan(RestWebClient client, string loanGuid)
        {
            LockResourceModel lockResource = new LockResourceModel();
            try
            {
                string responseStream = string.Empty;

                LockRequest _lockReq = new LockRequest()
                {
                    Resource = new Entity()
                    {
                        EntityId = loanGuid,
                        EntityType = EncompassEntityType.LOAN
                    },
                    LockType = LockType.SHARED
                };

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.LOCK_LOAN), Content = _lockReq, REQUESTTYPE = HeaderConstant.POST };

                IRestResponse response = client.Execute(reqObj);

                //var response = client.PostAsJsonAsync(EncompassURLConstant.LOCK_LOAN, _lockReq).Result;

                responseStream = response.Content;

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.Created)
                {
                    LockLoanResponse _res = JsonConvert.DeserializeObject<LockLoanResponse>(responseStream);
                    lockResource.Status = true;
                    lockResource.Message = _res.ID;
                }
                else
                {
                    ErrorResponse _badRequest = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                    lockResource.Status = false;
                    lockResource.Message = _badRequest.Details;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lockResource;
        }

        public static LockResourceModel UnLockLoan(RestWebClient client, string LockID, string LoanGUID)
        {
            LockResourceModel lockResource = new LockResourceModel();
            try
            {
                string responseStream = string.Empty;

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.UNLOCK_LOAN, LockID, EncompassEntityType.LOAN, LoanGUID), REQUESTTYPE = HeaderConstant.DELETE };

                IRestResponse response = client.Execute(reqObj);

                //var response = client.DeleteAsync(string.Format(EncompassURLConstant.UNLOCK_LOAN, LockID, EncompassEntityType.LOAN, LoanGUID)).Result;

                responseStream = response.Content;

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.Created)
                {
                    lockResource.Status = true;
                    lockResource.Message = string.Empty;
                }
                else
                {
                    ErrorResponse _badRequest = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                    lockResource.Status = false;
                    lockResource.Message = _badRequest.Details;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lockResource;
        }

    }
}
