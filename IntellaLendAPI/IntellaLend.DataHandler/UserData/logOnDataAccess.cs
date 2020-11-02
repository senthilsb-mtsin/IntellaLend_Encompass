using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntellaLend.Model;
using Newtonsoft.Json;
using MTSEntityDataAccess;

namespace IntellaLend.EntityDataHandler
{
    public class logOnDataAccess
    {
        protected static string TableSchema;

        #region Constructor

        public logOnDataAccess()
        { }

        public logOnDataAccess(string tableSchema)
        {
            TableSchema = tableSchema;
        }

        #endregion

        #region Public Methods        
        
        public object getRoleDetails(Int64 roleID)
        {  
            object data;
            using (var db = new DBConnect(TableSchema))
            {
                var startPage = (from r in db.Roles
                                 where r.RoleID == roleID
                                 select new { r.StartPage }).FirstOrDefault().StartPage;

                var roleDetails = (from r in db.Roles
                                   where r.RoleID == roleID
                                   select r).FirstOrDefault();

                var roleURLs = (from r in db.Roles
                                join au in db.AccessURLs on r.RoleID equals au.RoleID
                                where r.RoleID == roleID
                                select new { r.RoleID, r.RoleName, au.URL }).ToList();

                var roleMenus = (from rm in db.RoleMenuMapping
                                 join m in db.Menus on rm.MenuID equals m.MenuID
                                 where rm.RoleID == roleID
                                 orderby rm.MenuOrder
                                 select new { m.MenuID, m.MenuTitle, m.Icon, m.RouteLink, rm.MenuOrder, m.MenuGroupID }).ToList();

                var menuGroup = (from rm in roleMenus
                                 join mg in db.MenuGroupMaster on rm.MenuGroupID equals mg.MenuGroupID into mgrm
                                 from rmmg in mgrm.DefaultIfEmpty()
                                 group rmmg by new { MenuGroupID = rmmg?.MenuGroupID ?? 0, MenuGroupTitle = rmmg?.MenuGroupTitle ?? String.Empty, MenuGroupIcon = rmmg?.MenuGroupIcon ?? String.Empty } into g
                                 select new {
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
                                       SubMenus = (roleMenus.Where(r => r.MenuGroupID == mg.MenuGroupID)).ToList()
                                   });

                               
                data = new { URLs = roleURLs, Menus = roleObjects, StartPage = startPage, RoleDetails = roleDetails };
            }           
            return data;
        }

        #endregion

        #region Private Methods

        #endregion

    }
}
