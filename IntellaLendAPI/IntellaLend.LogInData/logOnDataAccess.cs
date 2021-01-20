using IntellaLend.ADServices;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntBlocks.LoggerBlock;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IntellaLend.EntityDataHandler
{
    public class logOnDataAccess
    {
        protected static string TableSchema;

        #region Constructor

        public logOnDataAccess()
        {
        }

        public logOnDataAccess(string tableSchema)
        {
            TableSchema = tableSchema;
        }

        #endregion

        #region Public Methods        

        public object getRoleDetails(Int64 roleID, Int64 UserID, bool ADLogin = false)
        {
            object data;
            using (var db = new DBConnect(TableSchema))
            {
                List<TempUserRoleMapping> UserRoleMapping = new List<TempUserRoleMapping>();
                if (!ADLogin)
                    UserRoleMapping = db.UserRoleMapping.AsNoTracking().Select(x => new TempUserRoleMapping() { RoleID = x.RoleID, UserID = x.UserID }).ToList();
                else
                    UserRoleMapping = new ADService(TableSchema).GetUserGroup(UserID).Select(x => new TempUserRoleMapping() { RoleID = x.RoleID, UserID = x.UserID }).ToList();

                Logger.WriteTraceLog($"UserRoleMapping : {UserRoleMapping.Count}");

                var role = (from r in db.Roles.AsNoTracking().AsEnumerable()
                            join ur in UserRoleMapping on r.RoleID equals ur.RoleID
                            where ur.UserID == UserID && r.RoleID == roleID
                            select r).FirstOrDefault();

                if (role != null)
                {
                    var startPage = (from r in db.Roles.AsNoTracking().AsEnumerable()
                                     join ur in UserRoleMapping on r.RoleID equals ur.RoleID
                                     where ur.UserID == UserID && r.RoleID == roleID
                                     select new { r.StartPage }).FirstOrDefault().StartPage;

                    var roleDetails = (from r in db.Roles.AsNoTracking().AsEnumerable()
                                       join ur in UserRoleMapping on r.RoleID equals ur.RoleID
                                       where ur.UserID == UserID && r.RoleID == roleID
                                       select r).FirstOrDefault();

                    var roleURLs = (from r in db.Roles.AsNoTracking().AsEnumerable()
                                    join ur in UserRoleMapping on r.RoleID equals ur.RoleID
                                    join au in db.AccessURLs.AsNoTracking() on r.RoleID equals au.RoleID
                                    where (ur.UserID == UserID && r.RoleID == roleID
                                        && !(ADLogin && au.URL.Contains("View\\User")))
                                    select new
                                    {
                                        r.RoleID,
                                        //r.RoleName,
                                        au.URL
                                    }).ToList().Union(from x in db.AccessURLs
                                                      where x.RoleID == RoleConstant.Common_Role_Urls
                                                      select new
                                                      {
                                                          x.RoleID,
                                                          x.URL
                                                      }).ToList();
                    var roleMenus = (from rm in db.RoleMenuMapping.AsNoTracking().AsEnumerable()
                                     join ur in UserRoleMapping on rm.RoleID equals ur.RoleID
                                     join m in db.Menus.AsNoTracking().AsEnumerable() on rm.MenuID equals m.MenuID
                                     where (ur.UserID == UserID && rm.RoleID == roleID
                                        && !(ADLogin && m.MenuTitle.Equals("User Administration")))
                                     orderby rm.MenuOrder, m.MenuGroupID
                                     select new { m.MenuID, m.MenuTitle, m.Icon, m.RouteLink, rm.MenuOrder, m.MenuGroupID, m.IsComponent }).ToList();

                    var menuGroup = (from rm in roleMenus
                                     join mg in db.MenuGroupMaster.AsNoTracking().AsEnumerable() on rm.MenuGroupID equals mg.MenuGroupID into mgrm
                                     from rmmg in mgrm.DefaultIfEmpty()
                                     group rmmg by new { MenuGroupID = rmmg?.MenuGroupID ?? 0, MenuGroupTitle = rmmg?.MenuGroupTitle ?? String.Empty, MenuGroupIcon = rmmg?.MenuGroupIcon ?? String.Empty } into g
                                     select new
                                     {
                                         MenuGroupID = g.Key.MenuGroupID,
                                         MenuGroupTitle = g.Key.MenuGroupTitle,
                                         MenuGroupIcon = g.Key.MenuGroupIcon
                                     }).ToList();

                    var roleObjects = (from mg in menuGroup
                                           //join rm in roleMenus on mg.MenuGroupID equals rm.MenuGroupID into roleSubMenus
                                       select new
                                       {
                                           MenuGroupID = mg.MenuGroupID,
                                           MenuGroupTitle = mg.MenuGroupTitle,
                                           MenuGroupIcon = mg.MenuGroupIcon,
                                           SubMenus = roleMenus.Where(r => r.MenuGroupID == mg.MenuGroupID).OrderBy(r => r.MenuOrder).ToList()
                                       });

                    data = new { URLs = roleURLs, Menus = roleObjects, StartPage = startPage, RoleDetails = roleDetails };
                }
                else
                {
                    throw new Exception("Role unavailable for logged user");
                }
            }
            return data;
        }

        public Int64 GetUserHash(string Hash)
        {
            using (var db = new DBConnect(TableSchema))
            {
                UserSession userSession = db.UserSession.AsNoTracking().Where(u => u.HashValidator == Hash && u.Active).FirstOrDefault();
                if (userSession != null)
                {
                    return userSession.UserID;
                }
            }
            return 0;
        }

        #endregion

        #region Private Methods

        public void CreateDBSession(Int64 UserID, string Hashing)
        {
            using (var db = new DBConnect(TableSchema))
            {
                var userSession = db.UserSession.AsNoTracking().Where(u => u.UserID == UserID).FirstOrDefault();
                if (userSession != null && userSession.UserID != 0)
                {
                    userSession.Active = true;
                    userSession.HashValidator = Hashing;
                    userSession.LastAccessedTime = DateTime.Now;
                    db.Entry(userSession).State = EntityState.Modified;
                }
                else
                {
                    UserSession newUser = new UserSession()
                    {
                        UserID = UserID,
                        CreatedOn = DateTime.Now,
                        Active = true,
                        HashValidator = Hashing,
                        LastAccessedTime = DateTime.Now
                    };
                    db.UserSession.Add(newUser);
                }

                User _loggedUser = db.Users.AsNoTracking().Where(u => u.UserID == UserID).FirstOrDefault();

                if (_loggedUser != null)
                {
                    _loggedUser.NoOfAttempts = 0;
                    _loggedUser.LastModified = DateTime.Now;
                    db.Entry(_loggedUser).State = EntityState.Modified;
                }

                db.SaveChanges();
            }
        }

        #endregion

        #region MenuList
        public object GetMenuList()
        {
            object data;
            using (var db = new DBConnect(TableSchema))
            {
                var _menulist = db.Menus.AsNoTracking().ToList();
                var menugroup = (from mn in _menulist
                                 join mg in db.MenuGroupMaster.AsNoTracking() on mn.MenuGroupID equals mg.MenuGroupID into mgrm
                                 from menugrp in mgrm.DefaultIfEmpty()
                                 group menugrp by new { MenuGroupID = menugrp?.MenuGroupID ?? 0, MenuGroupTitle = menugrp?.MenuGroupTitle ?? String.Empty } into g
                                 select new
                                 {
                                     MenuGroupID = g.Key.MenuGroupID,
                                     MenuGroupTitle = g.Key.MenuGroupTitle,
                                 }).ToList();
                data = (from mg in menugroup
                        select new
                        {
                            MenuGroupID = mg.MenuGroupID,
                            MenuGroupTitle = mg.MenuGroupTitle,
                            // SubMenus = _menulist.Where(r => r.MenuGroupID == mg.MenuGroupID).ToList()
                            SubMenus = (from ml in _menulist
                                        where ml.MenuGroupID == mg.MenuGroupID
                                        select new
                                        {
                                            MenuID = ml.MenuID,
                                            Icon = ml.Icon,
                                            MenuTitle = ml.MenuTitle,
                                            RouteLink = ml.RouteLink,
                                            MenuGroupID = ml.MenuGroupID,
                                            Accesslevel = ml.Accesslevel,
                                            IsComponent = ml.IsComponent,
                                            SubMenuID = _menulist.Where(m => m.MenuTitle == ml.MenuTitle && m.MenuID != ml.MenuID).Select(m => m.MenuID).ToArray()
                                        }).ToList()
                        });
            }

            return data;
        }

        public object GetMenuAccessList(Int64 RoleID, bool IsMappedMenuView)
        {
            object data;
            using (var db = new DBConnect(TableSchema))
            {
                var roleDetails = (from r in db.Roles
                                   where r.RoleID == RoleID
                                   select r).FirstOrDefault();

                var roleURLs = (from r in db.Roles
                                join au in db.AccessURLs on r.RoleID equals au.RoleID
                                where (r.RoleID == RoleID || r.RoleID == RoleConstant.Common_Role_Urls)
                                select new
                                {
                                    r.RoleID,
                                    //  r.RoleName,
                                    au.URL
                                }).ToList().Union(from x in db.AccessURLs
                                                  where x.RoleID == RoleConstant.Common_Role_Urls
                                                  select new
                                                  {
                                                      x.RoleID,
                                                      x.URL
                                                  }).ToList();
                if (IsMappedMenuView == false)
                {
                    var roleMenuAccess = (from mn in db.Menus
                                          let rolemenu = db.RoleMenuMapping.Where(x => x.MenuID == mn.MenuID).ToList()
                                          select new
                                          {
                                              mn.MenuID,
                                              mn.MenuTitle,
                                              mn.RouteLink,
                                              mn.MenuGroupID,
                                              IsMapped = rolemenu.Any(r => r.RoleID == RoleID),
                                              IsComponent = mn.IsComponent,
                                              Accesslevel = mn.Accesslevel
                                          }).Distinct().ToList();

                    var menuGroup = (from rm in roleMenuAccess
                                     join mg in db.MenuGroupMaster on rm.MenuGroupID equals mg.MenuGroupID into mgrm
                                     from rmmg in mgrm.DefaultIfEmpty()
                                     group rmmg by new { MenuGroupID = rmmg?.MenuGroupID ?? 0, MenuGroupTitle = rmmg?.MenuGroupTitle ?? String.Empty } into g
                                     select new
                                     {
                                         MenuGroupID = g.Key.MenuGroupID,
                                         MenuGroupTitle = g.Key.MenuGroupTitle,
                                     }).ToList();
                    var roleObjects = (from mg in menuGroup
                                       select new
                                       {
                                           MenuGroupID = mg.MenuGroupID,
                                           MenuGroupTitle = mg.MenuGroupTitle,
                                           // SubMenus = roleMenuAccess.Where(r => r.MenuGroupID == mg.MenuGroupID).OrderBy(r => r.MenuID).ToList(),
                                           SubMenus = (from ml in roleMenuAccess
                                                       where ml.MenuGroupID == mg.MenuGroupID
                                                       select new
                                                       {
                                                           MenuID = ml.MenuID,
                                                           MenuTitle = ml.MenuTitle,
                                                           RouteLink = ml.RouteLink,
                                                           MenuGroupID = ml.MenuGroupID,
                                                           Accesslevel = ml.Accesslevel,
                                                           IsComponent = ml.IsComponent,
                                                           IsMapped = ml.IsMapped,
                                                           SubMenuID = roleMenuAccess.Where(m => m.MenuTitle == ml.MenuTitle && m.MenuID != ml.MenuID).Select(m => m.MenuID).ToArray()
                                                       }).ToList()
                                       });

                    data = new { URLs = roleURLs, Menus = roleObjects, RoleDetails = roleDetails };
                }
                else
                {
                    var roleMenuAccess = (from rm in db.RoleMenuMapping
                                          join mn in db.Menus on rm.MenuID equals mn.MenuID
                                          where rm.RoleID == RoleID
                                          select mn).Distinct().ToList();

                    var menuGroup = (from rm in roleMenuAccess
                                     join mg in db.MenuGroupMaster on rm.MenuGroupID equals mg.MenuGroupID into mgrm
                                     from rmmg in mgrm.DefaultIfEmpty()
                                     group rmmg by new { MenuGroupID = rmmg?.MenuGroupID ?? 0, MenuGroupTitle = rmmg?.MenuGroupTitle ?? String.Empty } into g
                                     select new
                                     {
                                         MenuGroupID = g.Key.MenuGroupID,
                                         MenuGroupTitle = g.Key.MenuGroupTitle,
                                     }).ToList();
                    var roleObjects = (from mg in menuGroup
                                       select new
                                       {
                                           MenuGroupID = mg.MenuGroupID,
                                           MenuGroupTitle = mg.MenuGroupTitle,
                                           //SubMenus = roleMenuAccess.Where(r => r.MenuGroupID == mg.MenuGroupID).OrderBy(r => r.MenuID).ToList(),
                                           SubMenus = (from ml in roleMenuAccess
                                                       where ml.MenuGroupID == mg.MenuGroupID
                                                       select new
                                                       {
                                                           MenuID = ml.MenuID,
                                                           Icon = ml.Icon,
                                                           MenuTitle = ml.MenuTitle,
                                                           RouteLink = ml.RouteLink,
                                                           MenuGroupID = ml.MenuGroupID,
                                                           Accesslevel = ml.Accesslevel,
                                                           IsComponent = ml.IsComponent,
                                                           SubMenuID = roleMenuAccess.Where(m => m.MenuTitle == ml.MenuTitle && m.MenuID != ml.MenuID).Select(m => m.MenuID).ToArray()
                                                       }).ToList()
                                       });

                    data = new { URLs = roleURLs, Menus = roleObjects, RoleDetails = roleDetails };
                }
            }
            return data;
        }
        public object GetRoleMenuActive(Int64 RoleID, MenuMaster menus)
        {
            object data;
            using (var db = new DBConnect(TableSchema))
            {
                data = db.RoleMenuMapping.AsNoTracking().Where(x => x.MenuID == menus.MenuID && x.RoleID != RoleID).ToList();
            }
            return data;

        }
        #endregion

    }

    class TempUserRoleMapping
    {
        public Int64 RoleID { get; set; }
        public Int64 UserID { get; set; }
    }
}
