using IntellaLend.Constance;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace IntellaLend.WFProxy
{
    public class WFProxy
    {
        public static void ExecuteWorkFlow(Dictionary<string, string> wfValues)
        {
            try
            {
                TimeSpan timeOut = new TimeSpan(0, 15, 0);

                IntellaLendWF.ServiceClient client = null;

                BasicHttpBinding binding = new BasicHttpBinding()
                {
                    SendTimeout = timeOut,
                    ReceiveTimeout = timeOut
                };

                EndpointAddress endpoint = new EndpointAddress(new Uri(WFProxyDataAccess.WorkFlowURL));

                if (WFProxyDataAccess.WorkflowEnvironment.ToLower().Equals(EnvironmentConstant.HTTPS))
                    client = new IntellaLendWF.ServiceClient(new BasicHttpsBinding(BasicHttpsSecurityMode.Transport), endpoint);
                else
                {
                    client = new IntellaLendWF.ServiceClient(binding, endpoint);
                }

                client.ExecuteWorkFlow(wfValues);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
