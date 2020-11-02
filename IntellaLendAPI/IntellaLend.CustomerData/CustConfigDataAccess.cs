using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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


        public object AddRetention(Int64 customerID, string configKey, string configValue, bool active, DateTime createdOn, DateTime modifiedOn)
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

        public object AddMultipleCustConfig(Int64 customerID, List<CustomerConfigItem> custConfigItems)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {

                    List<CustomerConfig> lsCustConfig = db.CustomerConfig.AsNoTracking().Where(cc => cc.CustomerID == customerID).ToList();

                    foreach (CustomerConfig item in lsCustConfig)
                    {
                        db.Entry(item).State = EntityState.Deleted;
                        db.SaveChanges();
                    }

                    foreach (CustomerConfigItem item in custConfigItems)
                    {
                        db.CustomerConfig.Add(new CustomerConfig()
                        {
                            CustomerID = customerID,
                            ConfigKey = item.ConfigKey,
                            ConfigValue = item.ConfigValue,
                            Active = item.Active,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        });
                        db.SaveChanges();
                    }
                    
                    trans.Commit();
                    return true;
                }
            }
            

        }

        public CustomerConfig GetCustomerConfiguraton(string key)
        {
            using (var db = new DBConnect(TableSchema))
            {
                return db.CustomerConfig.AsNoTracking().Where(cc => cc.CustomerID == 0 && cc.ConfigKey == key).FirstOrDefault();
            }
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

        public object DeleteCustomerConfig(long customerID)
        {
            using(var db= new DBConnect(TableSchema))
            {
                
                List<CustomerConfig> lsCustConfig = db.CustomerConfig.AsNoTracking().Where(cc => cc.CustomerID == customerID).ToList();

                foreach (CustomerConfig item in lsCustConfig)
                {
                    db.Entry(item).State = EntityState.Deleted;
                    db.SaveChanges();
                }
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

        public object GetAllDistinctCustConfig(bool Active)
        {
            object cusConfigDatas;
            using (var db = new DBConnect(TableSchema))
            {
                cusConfigDatas = (from cc in db.CustomerConfig
                                  join cm in db.CustomerMaster on cc.CustomerID equals cm.CustomerID
                                  select new
                                  {
                                      ConfigID = cc.ConfigID,
                                      CustomerID = cc.CustomerID,
                                      CustomerName = cm.CustomerName,
                                      //Configkey = cc.ConfigKey,
                                      //ConfigValue = cc.ConfigValue,
                                      Active = cc.Active,
                                      //CreatedOn = cc.CreatedOn,
                                      //ModifiedOn = cc.ModifiedOn
                                  }
                    ).GroupBy(x=>x.CustomerID).Select(z=>z.OrderBy(i=>i.CustomerName).FirstOrDefault()).ToList();
            }
            return cusConfigDatas;
        }
    }
}
