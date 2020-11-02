using IntellaLend.Model;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntellaLend.EntityDataHandler
{
    public class UserDataAccess
    {
        protected static string TableSchema;

        #region Constructor

        public UserDataAccess()
        { }

        public UserDataAccess(string tableSchema)
        {
            TableSchema = tableSchema;
        }

        #endregion

        #region Public Methods

        public User GetUser(string userName)
        {
            User user = null;
            using (var db = new DBConnect(TableSchema))
            {
                user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (user != null)
                {
                    if (user.CustomerID != 0)
                        user.customerDetail = db.CustomerMaster.Where(c => c.CustomerID == user.CustomerID).FirstOrDefault();

                    user.UserAddressDetail = db.UserAddressDetail.Where(c => c.UserID == user.UserID).FirstOrDefault();
                    user.UserRoleMapping = db.UserRoleMapping.Where(c => c.UserID == user.UserID).ToList();
                }
            }
            return user;
        }

        public User GetUserWithId(Int64 UserID)
        {
            User user = null;
            using (var db = new DBConnect(TableSchema))
            {

                user = db.Users.Where(u => u.UserID == UserID).FirstOrDefault();

                if (user != null)
                {
                    user.CustomAddressDetails = db.CustomAddressDetail.Where(c => c.UserID == user.UserID).ToList();
                    user.UserAddressDetail = db.UserAddressDetail.Where(c => c.UserID == user.UserID).FirstOrDefault();
                    user.UserRoleMapping = db.UserRoleMapping.Where(c => c.UserID == user.UserID).ToList();
                }
            }
            return user;
        }

        public object GetUserSecurityQuestion(string userName)
        {
            User user = null;
            using (var db = new DBConnect(TableSchema))
            {
                user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (user != null)
                {
                    user.userSecurityQuestion = db.UserSecurityQuestion.Where(c => c.UserID == user.UserID).FirstOrDefault();
                    user.Password = null;
                }
            }

            return user;
        }

        public User GetUserWithUserName(string userName)
        {
            User user = null;
            using (var db = new DBConnect(TableSchema))
            {
                user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (user != null)
                    user.Password = null;
            }

            return user;
        }

        public object GetUserDetails(string userName)
        {
            User user = null;
            using (var db = new DBConnect(TableSchema))
            {
                user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (user != null)
                {
                    user.UserAddressDetail = db.UserAddressDetail.Where(c => c.UserID == user.UserID).FirstOrDefault();
                    user.UserRoleMapping = db.UserRoleMapping.Where(c => c.UserID == user.UserID).ToList();
                }
            }

            return user;

        }

        public List<User> GetUsersList()
        {
            List<User> users = null;
            using (var db = new DBConnect(TableSchema))
            {

                users = db.Users.ToList();

                foreach (var user in users)
                {
                    if (user.CustomerID != 0)
                        user.customerDetail = db.CustomerMaster.AsNoTracking().SingleOrDefault(cm => cm.CustomerID == user.CustomerID);

                    user.CustomAddressDetails = db.CustomAddressDetail.Where(c => c.UserID == user.UserID).ToList();
                    user.UserAddressDetail = db.UserAddressDetail.Where(c => c.UserID == user.UserID).FirstOrDefault();
                    user.UserRoleMapping = db.UserRoleMapping.Where(c => c.UserID == user.UserID).ToList();

                    foreach (UserRoleMapping item in user.UserRoleMapping)
                        item.RoleMaster = db.Roles.AsNoTracking().Where(r => r.RoleID == item.RoleID).FirstOrDefault();
                }
            }
            return users;
        }

        public bool AddUser(User user)
        {
            bool isUserNotExist;
            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    if (!db.Users.Any(x => x.UserName.Equals(user.UserName)))
                    {
                        if (user.CustomerID == null)
                            user.CustomerID = 0;

                        db.Users.Add(user);
                        db.SaveChanges();

                        user.UserAddressDetail.UserID = user.UserID;
                        db.UserAddressDetail.Add(user.UserAddressDetail);
                        db.SaveChanges();

                        foreach (var customAddressDetails in user.CustomAddressDetails)
                        {
                            customAddressDetails.UserID = user.UserID;
                            db.CustomAddressDetail.Add(customAddressDetails);
                            db.SaveChanges();
                        }

                        foreach (var userRoleMapping in user.UserRoleMapping)
                        {
                            userRoleMapping.UserID = user.UserID;
                            db.UserRoleMapping.Add(userRoleMapping);
                            db.SaveChanges();
                        }

                        tran.Commit();
                        isUserNotExist = true;

                    }
                    else
                    {
                        isUserNotExist = false;
                    }
                }
            }
            return isUserNotExist;
        }

        public bool UpdateUser(User user)
        {

            using (var db = new DBConnect(TableSchema))
            {
                using (var transaction = db.Database.BeginTransaction())
                {

                    User dbUser = db.Users.AsNoTracking().Where(u => u.UserID == user.UserID).FirstOrDefault();

                    dbUser.UserRoleMapping = db.UserRoleMapping.AsNoTracking().Where(ur => ur.UserID == user.UserID).ToList();

                    user.CreatedOn = dbUser.CreatedOn;
                    user.LastModified = DateTime.Now;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    db.Entry(user.UserAddressDetail).State = EntityState.Modified;
                    db.SaveChanges();

                    foreach (var additionalAddressDetails in user.CustomAddressDetails)
                    {
                        db.Entry(additionalAddressDetails).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    foreach (var dbUserRoleMapping in dbUser.UserRoleMapping)
                    {
                        db.Entry(dbUserRoleMapping).State = EntityState.Deleted;
                        db.SaveChanges();
                    }

                    foreach (var userRoleMapping in user.UserRoleMapping)
                    {
                        db.UserRoleMapping.Add(userRoleMapping);
                        db.SaveChanges();
                    }


                    transaction.Commit();
                    return true;
                }
            }

        }

        public bool SetSecurityQuestion(UserSecurityQuestion SecurityQuestion)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    db.UserSecurityQuestion.Add(SecurityQuestion);
                    db.SaveChanges();

                    tran.Commit();
                }
            }

            return true;
        }

        public bool SetNewPassword(User user)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    user.LastModified = DateTime.Now;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    transaction.Commit();
                    return true;
                }
            }
        }

        public bool LockUnlockUser(Int64 UserID, bool Lock)
        {
            using (var db = new DBConnect(TableSchema))
            {
                User user = db.Users.Where(u => u.UserID == UserID).FirstOrDefault();

                user.Locked = Lock;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }

            return false;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
