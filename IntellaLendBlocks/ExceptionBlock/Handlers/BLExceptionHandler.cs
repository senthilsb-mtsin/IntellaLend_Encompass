using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace MTSEntBlocks.ExceptionBlock.Handlers
{
    public static class BLExceptionHandler
    {
        public static bool HandleException(ref System.Exception ex)
        {
            return true;
            //bool rethrow = false;
            //if ((ex is DAException) || (ex is DACustomException))
            //{
            //    rethrow = ExceptionPolicy.HandleException(ex, "PassThroughPolicy");
            //    ex = new PassThruException(ex.Message);
            //}
            //else if (ex is BLCustomException)
            //{
            //    rethrow = ExceptionPolicy.HandleException(ex, "BusinessLogicCustomPolicy");
            //}
            //else
            //{
            //    rethrow = ExceptionPolicy.HandleException(ex, "BusinessLogicPolicy");
            //}
            //if (rethrow)
            //{
            //    throw ex;
            //}
            //return rethrow;
        }

    }
}
