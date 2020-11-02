using IntellaLend.Constance;
using IntellaLend.EntityDataHandler;
using IntellaLend.Hashing;
using IntellaLend.Model;
using System;
using System.Collections.Generic;

namespace IntellaLend.CommonServices
{
    public class UserService
    {
        protected static string TableSchema;
        protected static RandomPassword random = new RandomPassword();

        #region Constructor

        public UserService()
        { }

        public UserService(string tableSchema)
        {
            TableSchema = tableSchema;
        }

        #endregion

        #region Public Methods        

        public List<User> GetUsersList()
        {
            return new UserDataAccess(TableSchema).GetUsersList();
        }      

        public object GetUser(string UserName)
        {
            return new UserDataAccess(TableSchema).GetUserDetails(UserName);
        }
        public object GetServiceBasedUserDetails(Int64 loanId,string serviceTypeName)
        {
            return new UserDataAccess(TableSchema).GetServiceBasedUserDetails(loanId,serviceTypeName);
        }
        public object getKPIConfig()
        {
            return new UserDataAccess(TableSchema).getKPIConfig();
        }

        public bool updateKPIConfig(Int64 id,Int64 goal,DateTime? from, DateTime? to,Int64 UserGroupGoal)
        {
            return new UserDataAccess(TableSchema).updateKPIConfig(id, goal, from, to, UserGroupGoal);
        }

        public object GetUserSecurityQuestion(string UserName)
        {
            return new UserDataAccess(TableSchema).GetUserSecurityQuestion(UserName);
        }

        //public bool CheckLoggedUser(Int64 UserID, string Hash)
        //{
        //    return new UserDataAccess(TableSchema).GetUserSecurityQuestion(UserID, Hash);
        //}

        public bool ResetUserPassword(string UserName)
        {
            UserDataAccess userData = new UserDataAccess(TableSchema);

            User dbUser = userData.GetUserWithUserName(UserName);

            CustomerConfig _custConfig = new CustConfigDataAccess(TableSchema).GetCustomerConfiguraton(ConfigConstant.APPLICATIONURL);

            string password = random.GeneratePassword();

            dbUser.Password = MD5Hashing.Create(MD5Hashing.Create(password) + dbUser.CreatedOn.ToString(DateConstance.LongDateFormart));

            dbUser.Status = 0;

            bool result = userData.SetNewPassword(dbUser);

            if (result)
                setForgetPasswordEmailEntry(dbUser, password, _custConfig.ConfigValue);

            return result;
        }


        public bool LockUnlockUser(Int64 UserID, bool Lock)
        {
            return new UserDataAccess(TableSchema).LockUnlockUser(UserID, Lock);             
        }
        

        public bool AddUser(User user)
        {
            UserDataAccess userDataAccess = new UserDataAccess(TableSchema);

            user.CreatedOn = DateTime.Now;
            user.LastModified = DateTime.Now;
            user.PasswordCreatedDate = DateTime.Now;
            user.LastPwdModifiedDate = DateTime.Now;
            user.Status = 0; // 0 - For New User , 1 - Active User

            string password = random.GeneratePassword();
            CustomerConfig _custConfig = new CustConfigDataAccess(TableSchema).GetCustomerConfiguraton(ConfigConstant.APPLICATIONURL);
            user.Password = MD5Hashing.Create(MD5Hashing.Create(password) + user.CreatedOn.ToString(DateConstance.LongDateFormart));

            bool result = userDataAccess.AddUser(user);

            if (result)
                setNewUserEmailEntry(user, password, _custConfig.ConfigValue);

            return result;
        }

        public bool UpdateNewPasswordForExpiry(Int64 UserID, string NewPassword)
        {

            return new UserDataAccess(TableSchema).UpdateNewPasswordForExpiry(UserID, NewPassword);


        }

        public bool UpdateUser(User user)
        {
            return new UserDataAccess(TableSchema).UpdateUser(user);
        }

        public bool SetSecurityQuestion(UserSecurityQuestion SecurityQuestion, string NewPwd)
        {
            UserDataAccess userData = new UserDataAccess(TableSchema);

            //if (userData.SetSecurityQuestion(SecurityQuestion))
            //{
                User dbUser = userData.GetUserWithId(SecurityQuestion.UserID);
                               
                dbUser.Password = MD5Hashing.Create(NewPwd + dbUser.CreatedOn.ToString(DateConstance.LongDateFormart));

                dbUser.Status = 1;

                return userData.SetNewPassword(dbUser);
            //}

            return false;
        }


        public bool CheckCurrentPassword(Int64 UserId, string CurrentPwd)
        {
            UserDataAccess userData = new UserDataAccess(TableSchema);           

            User dbUser = userData.GetUserWithId(UserId);

            return dbUser.Password == MD5Hashing.Create(CurrentPwd + dbUser.CreatedOn.ToString(DateConstance.LongDateFormart));          
        }

        public bool UpdateNewPassword(Int64 UserId, string NewPwd)
        {
            UserDataAccess userData = new UserDataAccess(TableSchema);

            User dbUser = userData.GetUserWithId(UserId);
            
            dbUser.Password = MD5Hashing.Create(NewPwd + dbUser.CreatedOn.ToString(DateConstance.LongDateFormart));

            return userData.SetNewPassword(dbUser);
        }
        
        public void AddUserSession(Int64 userID, bool isActive)
        {
            UserDataAccess userData = new UserDataAccess(TableSchema);
            userData.AddUserSession(userID, isActive);
        }


        #endregion


        #region Private Methods

        private void setNewUserEmailEntry(User user, string password, string configValue)
        {
            //string appURL = String.Format(Application.URL, TableSchema);
            string emailContent = user.FirstName + " " + user.LastName + "," + user.UserName + "," + password + "," + user.UserName + "," + configValue;
            new IntellaLendDataAccess().SetEmailEntry(EmailTemplateConstants.NewUserTemplate, emailContent);
        }

        private void setForgetPasswordEmailEntry(User user, string password, string configValue)
        {

            string emailContent = user.FirstName + " " + user.LastName + "," + user.UserName + "," + password + "," + configValue;
            new IntellaLendDataAccess().SetEmailEntry(EmailTemplateConstants.ChangePasswordEmail, emailContent);
        }
        #endregion
    }
}
