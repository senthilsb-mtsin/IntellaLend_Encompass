using IntellaLend.Constance;

namespace IntellaLendAPI
{
    public class LicenseRegistration
    {
        public static void RegisterLicense()
        {
            License.CONFIGURATION = LicenseHelper.GetLicenseConfig();
        }
    }
}