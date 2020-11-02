using IntellaLend.Constance;
using IntellaLend.EntityDataHandler;
using IntellaLend.Hashing;
using IntellaLend.License;
using IntellaLend.Model;
using System;

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

        public object GetLoginUser(string UserName, string Password, out string Hash)
        {
            CheckLicense _license = new CheckLicense(TableSchema);
            Hash = string.Empty;
            if (_license.IsLicenseValid)
            {
                if (!_license.IsLicenseExpired)
                {
                    if (!_license.IsCuncurrentExceeded)
                    {
                        User user = new UserDataAccess(TableSchema).GetUser(UserName);
                        bool userResult = userCheck(user, Password);
                        if (userResult && user.Status == 1 && user.UserRoleMapping.Count > 0)
                        {
                            user.Password = null;
                            Hash = SessionHashing.Create(user.UserID);
                            new logOnDataAccess(TableSchema).CreateDBSession(user.UserID, Hash);
                            bool ExpiryDays = new UserDataAccess().PasswordExpiryCheck(user);
                            return new { User = user, Success = true, ExpiryDays = ExpiryDays };
                        }
                        else if (userResult && user.Status == 1 && user.UserRoleMapping.Count == 0)
                        {
                            return new { Success = false, Message = "Login success but no roles assigned", Locked = false };
                        }
                        else if (userResult && user.Status == 0)
                        {
                            string configValue=1.ToString();
                            CustomerConfig _custConfig = new CustConfigDataAccess(TableSchema).GetCustomerConfiguraton(ConfigConstant.PASSWORDEXPIRYDAYS);
                            if (_custConfig != null)
                            {
                                configValue = string.IsNullOrEmpty(_custConfig.ConfigValue) ? 1.ToString() : _custConfig.ConfigValue;
                            }
                            double calculatedPassExpiryDays = DateTime.Now.Subtract(user.PasswordCreatedDate).Days;
                            if(calculatedPassExpiryDays > Convert.ToDouble(configValue))
                            {
                                return new { Success = false, Message = "Password Expired" };
                            }
                            else
                            {
                                user.Password = null;
                                Hash = SessionHashing.Create(user.UserID);
                                new logOnDataAccess(TableSchema).CreateDBSession(user.UserID, Hash);
                                return new { User = user, Success = true };
                            }
                        }
                        else if (!userResult)
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

        public object getRoleDetails(Int64 roleID, Int64 UserID)
        {            
            return new logOnDataAccess(TableSchema).getRoleDetails(roleID, UserID);
        }

        public Int64 GetUserHash(string Hash)
        {
            return new logOnDataAccess(TableSchema).GetUserHash(Hash);
        }

        //public Int64 GetUserHash(Int64 UserID)
        //{
        //    return new logOnDataAccess(TableSchema).GetUserHash(UserID);
        //}


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
        public object GetMenuAccessList(Int64 RoleID,bool IsMappedMenuView)
        {
            return new logOnDataAccess(TableSchema).GetMenuAccessList(RoleID, IsMappedMenuView);
        }
      public object  GetRoleMenuActive(Int64 RoleID,MenuMaster menus)
        {
            return new logOnDataAccess(TableSchema).GetRoleMenuActive(RoleID, menus);
        }
        #endregion
    }
}
