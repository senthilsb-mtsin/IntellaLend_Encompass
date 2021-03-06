﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace EncompassRequestBody.ERequestModel
{
    public class ELoansRequest
    {
        [JsonProperty(PropertyName = "filter")]
        public QueryFields Filter { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public List<string> Fields { get; set; }

        //[JsonProperty(PropertyName = "sortOrder")]
        //public List<SortOrder> SortOrder { get; set; }
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
        [JsonProperty(PropertyName = "fieldID")]
        public string fieldID { get; set; }

        [JsonProperty(PropertyName = "fieldValue")]
        public string fieldValue { get; set; }

        [JsonProperty(PropertyName = "matchType")]
        public string matchType { get; set; }
    }

    public class LoanRequest
    {
        [JsonProperty(PropertyName = "queryFields")]
        public List<Fields> queryFields { get; set; }

        [JsonProperty(PropertyName = "returnFields")]
        public List<string> returnFields { get; set; }

        [JsonProperty(PropertyName = "returnLoanLimit")]
        public string returnLoanLimit { get; set; }

    }
}
