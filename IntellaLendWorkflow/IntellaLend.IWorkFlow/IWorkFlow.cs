using System.Collections.Generic;

namespace IntellaLend.WorkFlow
{
    public interface IWorkFlow
    {
        void SetWorkFlowState(ref Dictionary<string, string> wfValues);
    }
}
