using IntellaLend.ADServices;
using IntellaLend.Constance;
using IntellaLend.EntityDataHandler;
using IntellaLend.Hashing;
using IntellaLend.License;
using IntellaLend.Model;
using MTSEntBlocks.LoggerBlock;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace IntellaLend.CommonServices
{
    public class logOnService
    {
        protected static string TableSchema;

        #region Constructor

        public logOnService()
        { }

        public logOnService(string tableSchema)
        {
            TableSchema = tableSchema;
        }

        #endregion

        #region Public Methods

        public object GetLoginUser(string UserName, string Password, bool ADLogin, out string Hash)
        {
            CheckLicense _license = new CheckLicense(TableSchema);
            Hash = string.Empty;
            string requestPath = HttpContext.Current.Request.Url.AbsolutePath;
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string browser = HttpContext.Current.Request.Browser.Browser;
            string userHostName = HttpContext.Current.Request.UserHostName;

            string device;

            if (HttpContext.Current.Request.Browser.IsMobileDevice)
            {
                device = HttpContext.Current.Request.UserAgent;
            }
            else
            {
                device = HttpContext.Current.Request.Browser.Platform;
            }
            if (_license.IsLicenseValid)
            {
                if (!_license.IsLicenseExpired)
                {
                    if (!_license.IsCuncurrentExceeded)
                    {
                        bool userResult = false;
                        if (!UserName.Contains("@"))
                        {
                            UserName = UserName + "@" + ConfigurationManager.AppSettings["ADUserDomainName"].ToString(); // _config.ConfigValue;
                        }
                        User user = new UserDataAccess(TableSchema).GetUser(UserName);
                        Logger.WriteTraceLog($"UserName : {UserName}");
                        if (user != null && user.UserType == UserLoginType.CredentialLogin)
                        {
                            Password = ADLogin ? MD5Hashing.Create(Password) : Password;
                            userResult = userCheck(user, Password);
                        }
                        else if (ADLogin)
                        {
                            Logger.WriteTraceLog($"user != null : {user != null}");
                            CustomerConfig _config = new CustConfigDataAccess(TableSchema).GetCustomerConfiguraton(ConfigConstant.ADDOMAIN);
                            if (_config != null)
                            {
                                Logger.WriteTraceLog($" _config.ConfigValue  : { _config.ConfigValue}");
                                if (ADService.LogonUser(UserName, Password, _config.ConfigValue))
                                {
                                    Logger.WriteTraceLog($"Valid Login");
                                    _config = new CustConfigDataAccess(TableSchema).GetCustomerConfiguraton(ConfigConstant.LDAPURL);
                                    Logger.WriteTraceLog($" _config.ConfigValue  : { _config.ConfigValue}");
                                    User adUser = new ADService(TableSchema).GetUser(UserName, _config.ConfigValue);
                                    if (user == null)
                                    {
                                        Logger.WriteTraceLog($"AD UserRoleMapping Count Before: {adUser.UserRoleMapping.Count}");
                                        new UserService(TableSchema).AddADUser(adUser);
                                        user = adUser;
                                        Logger.WriteTraceLog($"AD UserRoleMapping Count After :  { adUser.UserRoleMapping.Count}");
                                    }

                                    Logger.WriteTraceLog($"user == null {user == null}");
                                    Logger.WriteTraceLog($"adUser != null {adUser != null}");
                                    userResult = true;
                                    if (adUser != null)
                                    {
                                        List<UserRoleMapping> userRoleMappings = adUser.UserRoleMapping.ToList();
                                        user.UserRoleMapping = new List<UserRoleMapping>();
                                        Logger.WriteTraceLog($" userRoleMappings.Count :  { userRoleMappings.Count}");
                                        foreach (var item in userRoleMappings)
                                        {
                                            user.UserRoleMapping.Add(new UserRoleMapping() { RoleID = item.RoleID, RoleName = item.RoleName, UserID = user.UserID });
                                        }
                                    }
                                }
                            }
                        }

                        Logger.WriteTraceLog($"2 user != null :  { user != null}");
                        if (user != null && userResult && user.Status == 1 && user.UserRoleMapping.Count > 0)
                        {
                            user.Password = null;
                            Hash = SessionHashing.Create(user.UserID);
                            new logOnDataAccess(TableSchema).CreateDBSession(user.UserID, Hash,requestPath,ipAddress, device,browser, userHostName);
                            bool ExpiryDays = new UserDataAccess().PasswordExpiryCheck(user);
                            return new { User = user, Success = true, ExpiryDays = ExpiryDays };
                        }
                        else if (user != null && userResult && user.Status == 1 && user.UserRoleMapping.Count == 0)
                        {
                            return new { Success = false, Message = "Login success but no roles assigned", Locked = false };
                        }
                        else if (user != null && userResult && user.Status == 0)
                        {
                            string configValue = 1.ToString();
                            CustomerConfig _custConfig = new CustConfigDataAccess(TableSchema).GetCustomerConfiguraton(ConfigConstant.PASSWORDEXPIRYDAYS);
                            if (_custConfig != null)
                            {
                                configValue = string.IsNullOrEmpty(_custConfig.ConfigValue) ? 1.ToString() : _custConfig.ConfigValue;
                            }
                            double calculatedPassExpiryDays = DateTime.Now.Subtract(user.PasswordCreatedDate).Days;
                            if (calculatedPassExpiryDays > Convert.ToDouble(configValue))
                            {
                                return new { Success = false, Message = "Password Expired" };
                            }
                            else
                            {
                                user.Password = null;
                                Hash = SessionHashing.Create(user.UserID);
                                new logOnDataAccess(TableSchema).CreateDBSession(user.UserID, Hash, requestPath, ipAddress, device, browser, userHostName);
                                return new { User = user, Success = true };
                            }
                        }
                        else if (user != null && !userResult && !ADLogin)
                        {
                            CustomerConfig _custPwdConfig = new CustConfigDataAccess(TableSchema).GetCustomerConfiguraton(ConfigConstant.NOOFATTEMPTSPWD);
                            Int64 configValue = 1;

                            if (_custPwdConfig != null)
                                Int64.TryParse(_custPwdConfig.ConfigValue, out configValue);

                            bool locked = new UserDataAccess(TableSchema).UpateNoOfAttempts(user, configValue);

                            return new { Success = false, Message = "User Null", Locked = locked };
                        }
                        else
                            return new { Success = false, Message = "User Null", Locked = false };
                    }
                    else
                        return new { Success = false, Message = "Cuncurrent User License Exceeded", Locked = false };
                }
                else
                    return new { Success = false, Message = "License Expired", Locked = false };
            }
            else
                return new { Success = false, Message = "License Not Valid", Locked = false };
        }

        public object getRoleDetails(Int64 roleID, Int64 UserID, bool ADLogin = false)
        {
            return new logOnDataAccess(TableSchema).getRoleDetails(roleID, UserID, ADLogin);
        }

        public Int64 GetUserHash(string Hash)
        {
            return new logOnDataAccess(TableSchema).GetUserHash(Hash);
        }

        //public Int64 GetUserHash(Int64 UserID)
        //{
        //    return new logOnDataAccess(TableSchema).GetUserHash(UserID);
        //}
        public Int64 GetAuditUserHash(string Hash)
        {
            return new logOnDataAccess(TableSchema).GetAuditUserHash(Hash);
        }


        public bool APIUserCheck(string UserName, string Password)
        {
            User user = new UserDataAccess(TableSchema).GetUser(UserName);

            Logger.WriteTraceLog($"UserName : {UserName}");
            Logger.WriteTraceLog($"user != null : {user != null}");

            if (user != null && user.UserType == UserLoginType.CredentialLogin)
                return userCheck(user, MD5Hashing.Create(Password));

            return false;
        }

        #endregion        

        #region private methods

        private bool userCheck(User user, string password)
        {
            return (user != null && MD5Hashing.Check(password, user.Password, user.CreatedOn));
        }

        #endregion

        #region MenuList

        public object GetMenuList()
        {
            return new logOnDataAccess(TableSchema).GetMenuList();
        }
        public object GetMenuAccessList(Int64 RoleID, bool IsMappedMenuView)
        {
            return new logOnDataAccess(TableSchema).GetMenuAccessList(RoleID, IsMappedMenuView);
        }
        public object GetRoleMenuActive(Int64 RoleID, MenuMaster menus)
        {
            return new logOnDataAccess(TableSchema).GetRoleMenuActive(RoleID, menus);
        }
        #endregion
    }
}
