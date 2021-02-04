using System.Collections.Generic;

namespace EncompassRequestBody.WrapperRequestModel
{
    public class WebHookRequestModel
    {
        public List<string> events { get; set; }
        public string endpoint { get; set; }
        public string resource { get; set; }
        public string signingkey { get; set; }
        public EWebHookFilter filters { get; set; }

    }

    public class EWebHookFilter
    {
        public List<string> attributes { get; set; }
    }
}
