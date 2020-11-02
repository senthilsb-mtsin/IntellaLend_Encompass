using IntellaLend.License;
using MTSEntBlocks.ExceptionBlock.Handlers;
using System;
using System.Collections.Generic;

namespace IntellaLendAPI
{
    public static class LicenseHelper
    {
        public static Dictionary<string, string> GetLicenseConfig()
        {
            try
            {
                return LicenseDataHelper.GetLicenseConfig();
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
            return new Dictionary<string, string>();
        }
    }
}