using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Linq;

namespace IntellaLend.WorkFlow
{
    public static class WorkFlowBaseDataAccess
    {
        public static string GetUserName(DBConnect db, Int64 UserID)
        {
            string _userName = string.Empty;

            User _user = db.Users.AsNoTracking().Where(u => u.UserID == UserID).FirstOrDefault();

            if (_user != null)
                _userName = $"{_user.LastName} {_user.FirstName}";

            return _userName;
        }
    }
}
