using IntellaLend.Constance;
using IntellaLend.Hashing;
using IntellaLend.Model;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IntellaLend.EntityDataHandler
{
    public class UserDataAccess
    {
        protected static string TableSchema;
        private static string SystemSchema = "IL";

        #region Constructor

        public UserDataAccess()
        { }

        public UserDataAccess(string tableSchema)
        {
            TableSchema = tableSchema;
        }

        #endregion

        #region Public Methods

        public UserRoleMappingTemp GetADRoleMapping(string groupName)
        {
            using (var db = new DBConnect(TableSchema))
            {
                return (from r in db.Roles.AsNoTracking()
                        join ad in db.ADGroupMasters.AsNoTracking() on r.ADGroupID equals ad.ADGroupID
                        where ad.ADGroupName == groupName
                        select new UserRoleMappingTemp()
                        {
                            RoleID = r.RoleID,
                            RoleName = r.RoleName
                        }).FirstOrDefault();
            }
        }

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
                    user.UserRoleMapping = db.UserRoleMapping.Where(c => c.UserID == user.UserID).OrderBy(x => x.RoleID).ToList();
                }
            }
            return user;
        }

        public bool UpateNoOfAttempts(User user, Int64 NofAttempts)
        {
            using (var db = new DBConnect(TableSchema))
            {
                if (user != null)
                {
                    user.NoOfAttempts = user.NoOfAttempts + 1;

                    if (user.NoOfAttempts > NofAttempts)
                    {
                        user.Locked = true;
                    }

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    return user.Locked;
                }

                return false;
            }
        }

        public void AddUserSession(Int64 userID, bool isActive)
        {
            using (var db = new DBConnect(TableSchema))
            {
                var userSession = db.UserSession.AsNoTracking().Where(u => u.UserID == userID).FirstOrDefault();
                if (userSession != null && userSession.UserID != 0)
                {
                    userSession.Active = isActive;
                    userSession.LastAccessedTime = DateTime.Now;
                    db.Entry(userSession).State = EntityState.Modified;
                    db.SaveChanges();

                }
                else
                {
                    UserSession newUser = new UserSession()
                    {
                        UserID = userID,
                        CreatedOn = DateTime.Now
                    };
                    newUser.Active = isActive;
                    newUser.LastAccessedTime = DateTime.Now;
                    db.UserSession.Add(newUser);
                    db.SaveChanges();
                }



            }
        }

        public bool PasswordExpiryCheck(User user)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                var Passwordpolicy = db.PasswordPolicy.AsNoTracking().FirstOrDefault();
                if (user != null)
                {
                    DateTime PwdExpiryDate = user.LastPwdModifiedDate.AddDays(Passwordpolicy.PasswordExpiryDays);
                    if (DateTime.Now > PwdExpiryDate)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<UserRoleMapping> GetUserRoleMapping(Int64 UserID)
        {
            using (var db = new DBConnect(TableSchema))
            {
                return db.UserRoleMapping.AsNoTracking().Where(c => c.UserID == UserID).ToList();
            }
        }


        public User GetUserWithId(Int64 UserID)
        {
            User user = null;
            using (var db = new DBConnect(TableSchema))
            {
                user = db.Users.AsNoTracking().Where(u => u.UserID == UserID).FirstOrDefault();

                if (user != null)
                {
                    user.CustomAddressDetails = db.CustomAddressDetail.AsNoTracking().Where(c => c.UserID == user.UserID).ToList();
                    user.UserAddressDetail = db.UserAddressDetail.AsNoTracking().Where(c => c.UserID == user.UserID).FirstOrDefault();
                    user.UserRoleMapping = db.UserRoleMapping.AsNoTracking().Where(c => c.UserID == user.UserID).ToList();
                }
            }
            return user;
        }

        public object GetUserSecurityQuestion(string userName)
        {
            User user = null;
            using (var db = new DBConnect(TableSchema))
            {
                user = db.Users.AsNoTracking().Where(u => u.UserName == userName).FirstOrDefault();

                if (user != null)
                {
                    user.userSecurityQuestion = db.UserSecurityQuestion.AsNoTracking().Where(c => c.UserID == user.UserID).FirstOrDefault();
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
                user = db.Users.AsNoTracking().Where(u => u.UserName == userName).FirstOrDefault();

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
                user = db.Users.AsNoTracking().Where(u => u.UserName == userName).FirstOrDefault();

                if (user != null)
                {
                    user.UserAddressDetail = db.UserAddressDetail.AsNoTracking().Where(c => c.UserID == user.UserID).FirstOrDefault();
                    user.UserRoleMapping = db.UserRoleMapping.AsNoTracking().Where(c => c.UserID == user.UserID).ToList();
                }
            }

            return user;

        }

        public bool updateKPIConfig(Int64 id, Int64 goal, DateTime? from, DateTime? to, Int64 UserGroupGoal)
        {
            bool result = false;
            using (var db = new DBConnect(TableSchema))
            {
                KPIGoalConfig goalConfgi = db.KPIGoalConfig.AsNoTracking().Where(k => k.ID == id).FirstOrDefault();
                if (goalConfgi != null)
                {
                    KpiUserGroupConfig _usergroupConfig = db.KpiUserGroupConfig.AsNoTracking().Where(x => x.GroupID == goalConfgi.UserGroupID && x.Goal != UserGroupGoal).FirstOrDefault();

                    goalConfgi.Goal = goal;
                    // goalConfgi.PeriodFrom = from;
                    // goalConfgi.PeriodTo = to;
                    goalConfgi.ModifiedOn = DateTime.Now;
                    db.Entry(goalConfgi).State = EntityState.Modified;
                    db.SaveChanges();

                    if (_usergroupConfig != null)
                    {
                        _usergroupConfig.Goal = UserGroupGoal;
                        _usergroupConfig.ModifiedOn = DateTime.Now;
                        db.Entry(_usergroupConfig).State = EntityState.Modified;
                        db.SaveChanges();
                    }


                    result = true;
                }

            }
            return result;
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


                    user.CustomAddressDetails = db.CustomAddressDetail.AsNoTracking().Where(c => c.UserID == user.UserID).ToList();
                    user.UserAddressDetail = db.UserAddressDetail.AsNoTracking().Where(c => c.UserID == user.UserID).FirstOrDefault();
                    user.UserRoleMapping = db.UserRoleMapping.AsNoTracking().Where(c => c.UserID == user.UserID).ToList();

                    foreach (UserRoleMapping item in user.UserRoleMapping)
                        item.RoleMaster = db.Roles.AsNoTracking().Where(r => r.RoleID == item.RoleID).FirstOrDefault();
                }
            }
            return users;
        }
        public object GetServiceBasedUserDetails(Int64 loanId, string serviceTypeName)
        {
            object user = null;

            List<UserRoleMapping> userRole = new List<UserRoleMapping>();
            using (var db = new DBConnect(TableSchema))
            {
                Int64 _assignedUserId = db.Loan.AsNoTracking().Where(l => l.LoanID == loanId).Select(l => l.AssignedUserID).FirstOrDefault();
                List<User> users = db.Users.AsNoTracking().Where(x => x.Active == true).ToList();
                //if (ConfigurationManager.AppSettings["PreClosing"].ToString().Equals(serviceTypeName))
                //{
                //    userRole = db.UserRoleMapping.AsNoTracking().Where(u => u.RoleName == RoleConstant.UNDERWRITER).ToList();
                //}
                //else if(ConfigurationManager.AppSettings["PostClosing"].ToString().Equals(serviceTypeName))
                //{
                //    userRole = db.UserRoleMapping.AsNoTracking().Where(u => u.RoleName == RoleConstant.POST_CLOSER).ToList();
                //}
                //userRole = (from RT in db.ReviewTypeMaster.AsNoTracking()
                //            join userrole in db.UserRoleMapping.AsNoTracking() on RT.UserRoleID equals userrole.RoleID
                //            where RT.ReviewTypeName.Trim().ToLower() == serviceTypeName.Trim().ToLower()
                //            select userrole).ToList();
                user = (from u in users
                        join userrole in db.UserRoleMapping.AsNoTracking() on u.UserID equals userrole.UserID
                        join RT in db.ReviewTypeMaster.AsNoTracking() on userrole.RoleID equals RT.UserRoleID
                        where RT.ReviewTypeName.Trim().ToLower() == serviceTypeName.Trim().ToLower()
                        && userrole.RoleID == RT.UserRoleID
                        select new
                        {
                            UserID = u.UserID,
                            UserName = u.FirstName + " " + u.LastName,
                            AssignedUserID = _assignedUserId
                        }).ToList();
            }
            return user;
        }

        public bool IsUserLimitExceeded()
        {
            bool result = false;

            using (var db = new DBConnect(TableSchema))
            {
                AppConfig _appConfig = db.AppConfig.AsNoTracking().Where(c => c.ConfigKey == "TOTAL_USERS").FirstOrDefault();

                if (_appConfig != null)
                {
                    List<User> _lsUsers = db.Users.AsNoTracking().ToList();

                    result = !(_lsUsers.Count() < Convert.ToInt32(_appConfig.ConfigValue));
                }
            }
            return result;
        }

        public bool AddADUser(User user)
        {
            using (var db = new DBConnect(TableSchema))
            {
                db.Users.Add(user);
                db.SaveChanges();

                return true;
            }
        }

        public bool AddUser(User user)
        {
            bool isUserNotExist;
            KPIGoalConfig goal = new KPIGoalConfig();
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

                        if (user.UserAddressDetail != null)
                        {
                            user.UserAddressDetail.UserID = user.UserID;
                            db.UserAddressDetail.Add(user.UserAddressDetail);
                            db.SaveChanges();
                        }

                        if (user.CustomAddressDetails != null)
                        {
                            foreach (var customAddressDetails in user.CustomAddressDetails)
                            {
                                customAddressDetails.UserID = user.UserID;
                                db.CustomAddressDetail.Add(customAddressDetails);
                                db.SaveChanges();
                            }
                        }

                        if (user.UserRoleMapping != null)
                        {
                            foreach (var userRoleMapping in user.UserRoleMapping)
                            {
                                userRoleMapping.UserID = user.UserID;
                                db.UserRoleMapping.Add(userRoleMapping);
                                db.SaveChanges();

                                KpiUserGroupConfig _kpiGroup = db.KpiUserGroupConfig.AsNoTracking().Where(k => k.RoleID == userRoleMapping.RoleID).OrderByDescending(k => k.GroupID).FirstOrDefault();

                                if (_kpiGroup != null)
                                {
                                    goal.UserID = user.UserID;
                                    goal.Goal = 0;
                                    goal.UserGroupID = _kpiGroup.GroupID;
                                    goal.PeriodFrom = _kpiGroup.PeriodFrom;
                                    goal.PeriodTo = _kpiGroup.PeriodTo;
                                    goal.CreatedOn = DateTime.Now;
                                    goal.ModifiedOn = DateTime.Now;
                                    db.KPIGoalConfig.Add(goal);
                                    db.SaveChanges();
                                }

                            }
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

                    if (dbUser.Locked && !user.Locked)
                        user.NoOfAttempts = 0;

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

                    KPIGoalConfig goal = db.KPIGoalConfig.AsNoTracking().Where(g => g.UserID == user.UserID).FirstOrDefault();
                    UserRoleMapping filter = user.UserRoleMapping.Find(r => r.RoleID == RoleConstant.POST_CLOSER_ROLEID);

                    if (goal != null)
                    {
                        db.Entry(goal).State = EntityState.Deleted;
                        db.SaveChanges();
                    }

                    if (filter != null)
                    {
                        KPIGoalConfig newGoal = new KPIGoalConfig();
                        newGoal.UserID = user.UserID;
                        newGoal.Goal = 0;
                        newGoal.CreatedOn = DateTime.Now;
                        newGoal.ModifiedOn = DateTime.Now;
                        db.KPIGoalConfig.Add(newGoal);
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
                    user.PasswordCreatedDate = DateTime.Now;
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
                UserSession userSession = db.UserSession.AsNoTracking().Where(u => u.UserID == UserID).FirstOrDefault();

                if (userSession != null && userSession.UserID != 0)
                {
                    userSession.Active = Lock;
                    userSession.LastAccessedTime = DateTime.Now;
                    db.Entry(userSession).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return true;
        }

        public bool UpdateNewPasswordForExpiry(Int64 UserID, string NewPassword)
        {
            using (var db = new DBConnect(TableSchema))
            {
                UserPassword userCheck = db.UserPassword.AsNoTracking().Where(i => i.UserID == UserID).FirstOrDefault();
                User user = db.Users.AsNoTracking().Where(i => i.UserID == UserID).FirstOrDefault();
                Int64 OldPasswordCount = NoOfOldPassword();
                string newPwd = MD5Hashing.Create(NewPassword + user.CreatedOn.ToString(DateConstance.LongDateFormart));
                if (userCheck != null && !string.IsNullOrEmpty(userCheck.Password))
                {
                    FixedSizedQueue<string> password = new FixedSizedQueue<string>(JsonConvert.DeserializeObject<IEnumerable<string>>(userCheck.Password), Convert.ToInt32(OldPasswordCount));
                    if (!password.Any(i => i.Equals(newPwd)))
                    {
                        password.Enqueue(newPwd);
                    }
                    else
                    {
                        return false;
                    }
                    userCheck.Password = JsonConvert.SerializeObject(password);
                    userCheck.ModifiedOn = DateTime.Now;
                    db.Entry(userCheck).State = EntityState.Modified;
                }
                else
                {
                    FixedSizedQueue<string> password = new FixedSizedQueue<string>(Convert.ToInt32(OldPasswordCount));
                    password.Enqueue(newPwd);
                    db.UserPassword.Add(new UserPassword()
                    {
                        Password = JsonConvert.SerializeObject(password),
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        UserID = UserID
                    });
                    db.SaveChanges();
                }

                user.Password = newPwd;
                user.LastPwdModifiedDate = DateTime.Now;
                user.LastModified = DateTime.Now;
                db.Entry(user).State = EntityState.Modified;

                db.SaveChanges();

            }

            return true;
        }

        public Int64 NoOfOldPassword()
        {
            Int64 NoOfOldPassword = 0;
            using (var db = new DBConnect(SystemSchema))
            {
                var Passwordpolicy = db.PasswordPolicy.AsNoTracking().FirstOrDefault();
                if (Passwordpolicy != null)
                {
                    NoOfOldPassword = Passwordpolicy.NoOfOldPassword;

                }

            }
            return NoOfOldPassword;

        }

        public object getKPIConfig()
        {
            List<KPIGoalConfig> goal = new List<KPIGoalConfig>();
            //var obj;
            using (var db = new DBConnect(TableSchema))
            {
                //goal = db.KPIGoalConfig.AsNoTracking().ToList();
                return (from kpi in db.KPIGoalConfig
                        join urs in db.Users on kpi.UserID equals urs.UserID
                        select new
                        {
                            UserName = urs.FirstName + " " + urs.LastName,
                            UserID = kpi.UserID,
                            ID = kpi.ID,
                            Goal = kpi.Goal,
                            PeriodFrom = kpi.PeriodFrom,
                            PeriodTo = kpi.PeriodTo,
                            CreatedOn = kpi.CreatedOn,
                            ModifiedOn = kpi.ModifiedOn
                        }).ToList();
            }
            //return obj;
        }

        #endregion

        #region Private Methods

        #endregion
    }
    //public class UserDetails
    //{
    //  public Int64 UserID { get; set; }
    //  public string UserName { get; set; }
    //}
}
