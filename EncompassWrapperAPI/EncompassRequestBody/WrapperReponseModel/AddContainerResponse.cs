using Newtonsoft.Json;

namespace EncompassRequestBody.WrapperReponseModel
{
    public class AddContainerResponse
    {
        [JsonProperty(PropertyName = "documentID")]
        public string DocumentID { get; set; }
    }
}
