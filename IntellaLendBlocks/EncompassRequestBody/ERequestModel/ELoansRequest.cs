using Newtonsoft.Json;
using System.Collections.Generic;

namespace EncompassRequestBody.ERequestModel
{
    public class EPipelineLoanRequest
    {
        [JsonProperty(PropertyName = "filter")]
        public QueryFields Filter { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public List<string> Fields { get; set; }

        //[JsonProperty(PropertyName = "sortOrder")]
        //public List<SortOrder> SortOrder { get; set; }
    }

    public class ELoansRequest
    {
        [JsonProperty(PropertyName = "loanIds")]
        public List<string> LoanGUIDs { get; set; }

        [JsonProperty(PropertyName = "returnFields")]
        public List<string> ReturnFields { get; set; }

    }

    public class QueryFields
    {
        [JsonProperty(PropertyName = "operator")]
        public string Operator { get; set; }

        [JsonProperty(PropertyName = "terms")]
        public List<Fields> Terms { get; set; }

    }

    //public class SortOrder
    //{
    //    [JsonProperty(PropertyName = "canonicalName")]
    //    public string CanonicalName { get; set; }

    //    [JsonProperty(PropertyName = "order")]
    //    public string Order { get; set; }
    //}

    public class Fields
    {
        [JsonProperty(PropertyName = "canonicalName")]
        public string FieldID { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string FieldValue { get; set; }

        [JsonProperty(PropertyName = "matchType")]
        public string MatchType { get; set; }
    }

    public class LoanRequest
    {
        [JsonProperty(PropertyName = "queryFields")]
        public List<Fields> QueryFields { get; set; }

        [JsonProperty(PropertyName = "returnFields")]
        public List<string> ReturnFields { get; set; }

        [JsonProperty(PropertyName = "returnLoanLimit")]
        public string ReturnLoanLimit { get; set; }

    }

    public class GetLoanRequest
    {
        [JsonProperty(PropertyName = "loanIds")]
        public List<string> LoanGUIDs { get; set; }

        [JsonProperty(PropertyName = "returnFields")]
        public List<string> ReturnFields { get; set; }

        [JsonProperty(PropertyName = "returnLoanLimit")]
        public string ReturnLoanLimit { get; set; }

    }

}
