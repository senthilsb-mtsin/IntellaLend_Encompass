using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace MTSEntBlocks.ExceptionBlock.Handlers
{
    public static class DAExceptionHandler
    {
        public static bool HandleException(ref System.Exception ex)
        {
            return true;
            //bool rethrow = false;
            //if ((ex is SqlException))
            //{
            //    SqlException dbExp = (SqlException)ex;
            //    if (dbExp.Number >= 50000)
            //    {
            //        rethrow = ExceptionPolicy.HandleException(ex, "DataAccessCustomPolicy");
            //        ex = new DACustomException(ex.Message);
            //    }
            //    else
            //    {
            //        rethrow = ExceptionPolicy.HandleException(ex, "DataAccessPolicy");
            //        if (rethrow)
            //        {
            //            throw ex;
            //        }
            //    }
            //}
            //else if (ex is System.Exception)
            //{
            //    rethrow = ExceptionPolicy.HandleException(ex, "DataAccessPolicy");
            //    if (rethrow)
            //    {
            //        throw ex;
            //    }
            //}
            //return rethrow;
        }
    }
}
