using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EncompassWrapperInterseptor
{
    public class EncompassInterseptorDataAccess
    {
        public string TenantSchema;
        private static string SystemSchema = "IL";

        public EncompassInterseptorDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        public EncompassAccessToken GetDBToken()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.EncompassAccessToken.AsNoTracking().Where(m => m.Active == true).FirstOrDefault();
            }
        }

        public void UpdateNewToken(string tokenType, string token)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                EncompassAccessToken _accessToken = db.EncompassAccessToken.AsNoTracking().Where(m => m.Active == true).FirstOrDefault();
                if (_accessToken != null)
                {
                    _accessToken.AccessToken = token;
                    _accessToken.TokenType = tokenType;
                    _accessToken.ModifiedOn = DateTime.Now;

                    db.Entry(_accessToken).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    _accessToken = new EncompassAccessToken()
                    {
                        AccessToken = token,
                        Active = true,
                        TokenType = tokenType,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };
                    db.EncompassAccessToken.Add(_accessToken);
                    db.SaveChanges();
                }
            }
        }

        public List<EncompassConfig> GetEncompassConfig()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.EncompassConfig.AsNoTracking().ToList();
            }
        }
    }
}
