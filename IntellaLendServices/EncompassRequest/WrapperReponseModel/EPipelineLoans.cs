﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace EncompassRequestBody.WrapperReponseModel
{
    public class EPipelineLoans
    {
        [JsonProperty(PropertyName = "loanGuid")]
        public string LoanGuid { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public Dictionary<string, string> Fields { get; set; }
    }
}
