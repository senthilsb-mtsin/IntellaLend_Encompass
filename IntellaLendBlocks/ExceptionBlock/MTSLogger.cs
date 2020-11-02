using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace MTSEntBlocks.ExceptionBlock
{
    public static class MTSLogger
    {
        public static void Write(string message, string LogName)
        {
            Logger.Write(message, LogName);
        }
    }
}
