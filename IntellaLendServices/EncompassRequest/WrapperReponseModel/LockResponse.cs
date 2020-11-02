using Newtonsoft.Json;

namespace EncompassRequestBody.WrapperReponseModel
{
    public class LockResponse
    {
        [JsonProperty(PropertyName = "lockID")]
        public string LockID { get; set; }
    }
}
