using System;
using System.IO;
using System.Web;

namespace IntellaLend.License
{
    public class CheckLicense
    {
        private static string TenantSchema;

        public CheckLicense(string Tablechema)
        {
            TenantSchema = Tablechema;
        }

        public bool IsLicenseValid
        {
            get
            {
                return LicenseFileKey().Equals(LicenseDataHelper.GetLicenseKey());
            }
        }

        public bool IsCuncurrentExceeded
        {
            get {
                return !(LicenseDataHelper.GetCuncurrentUserCount(TenantSchema) < IntellaLend.Constance.License.TOTAL_CONCURRENT_USERS);
            }
        }

        public bool IsUserExceeded
        {
            get
            {
                return !(LicenseDataHelper.GetUserCount(TenantSchema) < IntellaLend.Constance.License.TOTAL_USERS);
            }
        }

        public bool IsLicenseExpired
        {
            get
            {
                return !(DateTime.Now < IntellaLend.Constance.License.LICENSE_EXPIRYDATE);
            }
        }

        private string LicenseFileKey()
        {
            return File.ReadAllText(HttpContext.Current.Server.MapPath("~/License/IntellaLend.lic"));
        }
    }
}
