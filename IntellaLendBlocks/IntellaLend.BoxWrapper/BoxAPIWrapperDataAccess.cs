using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Data.Entity;
using System.Linq;

namespace IntellaLend.BoxWrapper
{
    public class BoxAPIWrapperDataAccess
    {
        #region Private Variables

        private static string TenantSchema;
        public static string SystemSchema = "IL";

        #endregion

        #region Constructor

        public BoxAPIWrapperDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion

        public BoxUserToken GetUserToken(Int64 UserID)
        {            
            using (var db = new DBConnect(TenantSchema))
            {
                BoxUserToken userToken = db.BoxUserToken.AsNoTracking().Where(a => a.UserID == UserID).FirstOrDefault();

                if (userToken != null)
                    return userToken;
                else
                    return new BoxUserToken();
            }
        }


        public void UpdateUserToken(BoxUserToken updatedUserToken)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                BoxUserToken userToken = db.BoxUserToken.AsNoTracking().Where(a => a.UserID == updatedUserToken.UserID).FirstOrDefault();

                if (userToken != null)
                {
                    userToken.Token = updatedUserToken.Token;
                    userToken.RefreshToken = updatedUserToken.RefreshToken;
                    userToken.ExpireTime = updatedUserToken.ExpireTime;
                    userToken.TokenType = updatedUserToken.TokenType;
                    userToken.ModifiedOn = DateTime.Now;
                    db.Entry(userToken).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    updatedUserToken.CreatedOn = DateTime.Now;
                    db.BoxUserToken.Add(updatedUserToken);
                    db.SaveChanges();
                }
            }
        }


        public void RemoveUserToken(BoxUserToken updatedUserToken)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                BoxUserToken userToken = db.BoxUserToken.AsNoTracking().Where(a => a.UserID == updatedUserToken.UserID).FirstOrDefault();
                if (userToken != null)
                {
                    userToken.Token = "";
                    userToken.RefreshToken = "";
                    userToken.ModifiedOn = DateTime.Now;
                    db.Entry(userToken).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }


        public static string BoxClientID
        {
            get
            {
                using (var db = new DBConnect(SystemSchema))
                {
                    AppConfig appConfig = db.AppConfig.AsNoTracking().Where(a => a.ConfigKey == "BOX_CLIENTID").FirstOrDefault();

                    if (appConfig != null)
                        return appConfig.ConfigValue;
                }
                return string.Empty;
            }
        }

        


        public static string BoxClientSecretID
        {
            get
            {
                using (var db = new DBConnect(SystemSchema))
                {
                    AppConfig appConfig = db.AppConfig.AsNoTracking().Where(a => a.ConfigKey == "BOX_CLIENT_SECRETID").FirstOrDefault();

                    if (appConfig != null)
                        return appConfig.ConfigValue;
                }
                return string.Empty;
            }
        }


        public static string BoxUserName
        {
            get
            {
                using (var db = new DBConnect(SystemSchema))
                {
                    AppConfig appConfig = db.AppConfig.AsNoTracking().Where(a => a.ConfigKey == "BOX_USER_NAME").FirstOrDefault();

                    if (appConfig != null)
                        return appConfig.ConfigValue;
                }
                return string.Empty;
            }
        }

        public static string BoxRedirectURL
        {
            get
            {
                using (var db = new DBConnect(SystemSchema))
                {
                    AppConfig appConfig = db.AppConfig.AsNoTracking().Where(a => a.ConfigKey == "BOX_REDIRECT_URL").FirstOrDefault();

                    if (appConfig != null)
                        return appConfig.ConfigValue;
                }
                return string.Empty;
            }
        }
        
    }
}
