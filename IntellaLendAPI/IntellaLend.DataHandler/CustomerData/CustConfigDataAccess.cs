using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntellaLend.Model;
using System.Data.Entity;

namespace IntellaLend.EntityDataHandler
{
    public class CustConfigDataAccess
    {
        private static string TableSchema;
        public CustConfigDataAccess() { }

        public CustConfigDataAccess(string tableschema)
        {
            TableSchema = tableschema;

        }


        public object AddRetention(int customerID, string configKey, string configValue, bool active, DateTime createdOn, DateTime modifiedOn)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    db.CustomerConfig.Add(new CustomerConfig()
                    {
                        CustomerID = customerID,
                        ConfigKey = configKey,
                        ConfigValue = configValue,
                        Active = active,
                        CreatedOn = createdOn,
                        ModifiedOn = modifiedOn
                    });
                    db.SaveChanges();
                    trans.Commit();
                    return true;
                }
            }

            return false;
        }

        public object EditRetention(CustomerConfig updateCustomerConfig)
        {
            using(var db= new DBConnect(TableSchema))
            {
                using(var trans = db.Database.BeginTransaction())
                {
                    CustomerConfig updateCustConfig = db.CustomerConfig.AsNoTracking().Single(cc => cc.ConfigID == updateCustomerConfig.ConfigID);
                    updateCustConfig.CustomerID = updateCustomerConfig.CustomerID;
                    updateCustConfig.ConfigValue = updateCustomerConfig.ConfigValue;
                    updateCustConfig.Active = updateCustomerConfig.Active;
                    updateCustomerConfig.ModifiedOn = DateTime.Now;
                    db.Entry(updateCustomerConfig).State = EntityState.Modified;
                    db.SaveChanges();
                    trans.Commit();
                    return true;
                }
            }
            return false;
        }

        public object DeleteRetention(long conFigID)
        {
            using(var db= new DBConnect(TableSchema))
            {
                CustomerConfig cusconf = db.CustomerConfig.AsNoTracking().Where(cc => cc.ConfigID == conFigID).FirstOrDefault();
                db.Entry(cusconf).State = EntityState.Deleted;
                db.CustomerConfig.Remove(cusconf);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public object GetAllCustConfig(bool Active)
        {
            object cusConfigDatas;
            using (var db = new DBConnect(TableSchema))
            {
                cusConfigDatas = (from cc in db.CustomerConfig
                                  join cm in db.CustomerMaster on cc.CustomerID equals cm.CustomerID
                                  select new
                                  {   ConfigID= cc.ConfigID,
                                      CustomerID = cc.CustomerID,
                                      CustomerName = cm.CustomerName,
                                      Configkey = cc.ConfigKey,
                                      ConfigValue = cc.ConfigValue,
                                      Active = cc.Active,
                                      CreatedOn = cc.CreatedOn,
                                      ModifiedOn=cc.ModifiedOn
                                  }
                    ).ToList();
            }
            return cusConfigDatas;
        }
    }
}
