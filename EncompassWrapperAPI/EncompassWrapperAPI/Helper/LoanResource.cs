using EncompassRequestBody.CustomModel;
using EncompassRequestBody.WrapperReponseModel;
using EncompassWrapperConstants;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;

namespace EncompassWrapperAPI
{
    public class LoanResource
    {
        public static LockResourceModel LockLoan(HttpClient client, string loanGuid)
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
                    LockType = LockType.EXCLUSIVE
                };

                var response = client.PostAsJsonAsync(EncompassURLConstant.LOCK_LOAN, _lockReq).Result;

                responseStream = response.Content.ReadAsStringAsync().Result;

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

        public static LockResourceModel UnLockLoan(HttpClient client, string LockID, string LoanGUID)
        {
            LockResourceModel lockResource = new LockResourceModel();
            try
            {
                string responseStream = string.Empty;

                var response = client.DeleteAsync(string.Format(EncompassURLConstant.UNLOCK_LOAN, LockID, EncompassEntityType.LOAN, LoanGUID)).Result;

                responseStream = response.Content.ReadAsStringAsync().Result;

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
