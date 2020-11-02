using MTSEntBlocks.ExceptionBlock.Handlers;
using System;
using System.Activities;

namespace IntellaLendWorkflow
{
    public class ExceptionComponent : CodeActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            var ex = context.DataContext.GetProperties()["ex"];

            Exception mainException = ((Exception)ex.GetValue(context.DataContext)).InnerException;

            if (mainException != null)
            {
                MTSExceptionHandler.HandleException(ref mainException);
                throw mainException;
            }
        }
    }
}