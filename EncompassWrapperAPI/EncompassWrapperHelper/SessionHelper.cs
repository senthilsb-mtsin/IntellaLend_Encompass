
namespace EncompassWrapperHelper
{
    public class SessionHelper
    {
        public string Token { get; set; }
        public string TokenType { get; set; }
        //public string TenantSchema { get; set; }

        static SessionHelper()
        {
            //GetToken();
        }


        //private static void GetToken(string TenantSchema)
        //{
        //    DataAccess _dataAccess = new DataAccess("IL");

        //    HeaderToken _token = _dataAccess.GetEncompassToken(TenantSchema);
        //    if (_token != null)
        //    {
        //        Token = _token.AccessToken;
        //        TokenType = _token.TokenType;
        //        TenantSchema = _token.TokenTenant;
        //    }
        //}

    }
}
