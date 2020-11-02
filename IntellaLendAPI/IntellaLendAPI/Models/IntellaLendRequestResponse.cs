using Newtonsoft.Json;
using System;


namespace IntellaLendAPI.Models
{
    public class IntellaLendRequest
    {
        public RequestUserInfo RequestUserInfo { get; set; }        
    }

    public class RequestUserInfo
    {
        public Int64 RequestUserID { get; set; }
        public string RequestUserTableSchema { get; set; }
    }

    public class IntellaLendResponse
    {
        [JsonProperty(PropertyName = "response-message")]
        public ResponseMessage ResponseMessage { get; set; }
    }

    public class ResponseMessage
    {

        [JsonProperty(PropertyName = "message-id")]
        public long MessageID { get; set; }

        [JsonProperty(PropertyName = "message-desc")]
        public string MessageDesc { get; set; }
    }
}