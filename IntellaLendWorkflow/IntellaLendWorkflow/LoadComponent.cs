using IntellaLend.WorkFlow;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Reflection;

namespace IntellaLendWorkflow
{

    public sealed class LoadComponent : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<Dictionary<string, string>> Text { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            Dictionary<string, string> wfValues = new Dictionary<string, string>();

            var wfInputVal = context.DataContext.GetProperties()["InputValues"];
            wfValues = wfInputVal.GetValue(context.DataContext) as Dictionary<string, string>;
            Logger.WriteTraceLog($" wfInputVal :  {JsonConvert.SerializeObject(wfValues)}");
            var varDLLName = context.DataContext.GetProperties()["DLLName"];
            string DLLName = varDLLName.GetValue(context.DataContext) as string;
            Logger.WriteTraceLog($"DLLName  : {DLLName}");
            var varAction = context.DataContext.GetProperties()["Action"];
            string Action = varAction.GetValue(context.DataContext) as string;
            Logger.WriteTraceLog($"Action  : {Action}");
            object objInvoke;
            Type typeWorkFlow = null;

            Assembly ServiceDll = Assembly.LoadFile(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "bin\\" + DLLName);

            Logger.WriteTraceLog($"ServiceDll  : {ServiceDll.Location}");

            foreach (Type type in ServiceDll.GetTypes())
            {
                //if (typeof(IWorkFlow).GetType().IsAssignableFrom(type.GetType()))
                if (typeof(IWorkFlow).IsAssignableFrom(type))
                {
                    Logger.WriteTraceLog($"Valid Type");
                    typeWorkFlow = type;
                    break;
                }
            }

            Logger.WriteTraceLog($"Action  : {Action}");

            if (typeWorkFlow == null)
                throw new Exception("Component loaded is not of type IWorkFlow");

            objInvoke = Activator.CreateInstance(typeWorkFlow);

            string MethodName = string.Empty;

            if (Action.ToUpper().Equals("SETWORKFLOW"))
                MethodName = "SetWorkFlow";

            if (Action.ToUpper().Equals("SETWORKFLOWSTATE"))
                MethodName = "SetWorkFlowState";

            Logger.WriteTraceLog($"MethodName  : {MethodName}");

            //string strKeys = string.Join(",", wfValues.Select(a => a.Key).ToList());
            //string strValues = string.Join(",", wfValues.Select(a => a.Value).ToList());

            //Exception ex = new Exception($"Keys : {strKeys} | Values : {strValues}");
            //BaseExceptionHandler.HandleException(ref ex);

            MethodInfo method = typeWorkFlow.GetMethod(MethodName);
            method.Invoke(objInvoke, new object[] { wfValues });


            Logger.WriteTraceLog($"After Invoke  ");
        }
    }
}
