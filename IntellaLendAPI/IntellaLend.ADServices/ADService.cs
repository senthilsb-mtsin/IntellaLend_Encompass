using IntellaLend.Constance;
using IntellaLend.EntityDataHandler;
using IntellaLend.Model;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;

namespace IntellaLend.ADServices
{
    public class ADService
    {
        public ADService() { }

        public static string TableSchema;

        public ADService(string tableschema)
        {
            TableSchema = tableschema;
        }

        public object GetADGroups(string LDAPUrl)
        {
            List<string> adGroups = GetAllGroups(LDAPUrl);

            return new MasterDataAccess(TableSchema).AddADGroups(adGroups);
        }

        public List<UserRoleMapping> GetUserGroup(Int64 userID)
        {
            User _user = new UserDataAccess(TableSchema).GetUserWithId(userID);
            if (_user.UserType == UserLoginType.CredentialLogin)
                return new UserDataAccess(TableSchema).GetUserRoleMapping(userID);

            try
            {
                CustomerConfig _config = new CustConfigDataAccess(TableSchema).GetCustomerConfiguraton(ConfigConstant.LDAPURL);
                List<string> adGroups = GetAllGroupsOfUser(_config.ConfigValue, _user.UserName);
                foreach (string grpName in adGroups)
                {
                    UserRoleMappingTemp _role = new UserDataAccess(TableSchema).GetADRoleMapping(grpName);
                    if (_role != null)
                    {
                        _user.UserRoleMapping.Add(new UserRoleMapping { RoleID = _role.RoleID, RoleName = _role.RoleName, UserID = userID });
                    }
                }
            }
            catch (Exception EX)
            {
                MTSExceptionHandler.HandleException(ref EX);
            }

            return _user.UserRoleMapping;
        }


        public User GetUser(string userName, string LDAPUrl)
        {
            User _user = new User();
            _user.UserRoleMapping = new List<UserRoleMapping>();
            ADUserDetails adGroups = GetADUserInfo(LDAPUrl, userName);
            Logger.WriteTraceLog($"adGroups.Groups.count : {adGroups.Groups.Count}");
            foreach (string grpName in adGroups.Groups)
            {
                UserRoleMappingTemp _role = new UserDataAccess(TableSchema).GetADRoleMapping(grpName);

                if (_role != null)
                {
                    Logger.WriteTraceLog($"Add grpName : {grpName}");
                    _user.UserRoleMapping.Add(new UserRoleMapping { RoleID = _role.RoleID, RoleName = _role.RoleName });
                }
            }

            _user.UserName = userName;
            _user.Email = adGroups.Email;
            _user.FirstName = adGroups.GivenName;
            _user.LastName = adGroups.SN;
            _user.Status = 1;
            _user.Active = true;
            _user.UserType = UserLoginType.ADLogin;

            Logger.WriteTraceLog($"ADService: _user.UserRoleMapping != null : {_user.UserRoleMapping != null}");

            return _user;
        }

        private static DirectorySearcher BuildUserSearcher(DirectoryEntry de)
        {
            DirectorySearcher ds = new DirectorySearcher(de);
            ds.PropertiesToLoad.Add("name");
            ds.PropertiesToLoad.Add("mail");
            ds.PropertiesToLoad.Add("givenname");
            ds.PropertiesToLoad.Add("sn");
            ds.PropertiesToLoad.Add("memberof");
            ds.PropertiesToLoad.Add("userPrincipalName");
            ds.PropertiesToLoad.Add("distinguishedName");
            return ds;
        }

        public static bool LogonUser(string UserName, string Password, string DomainName)
        {
            bool isValid = false;
            NetworkCredential credentials = new NetworkCredential(UserName, Password, DomainName);
            LdapDirectoryIdentifier id = new LdapDirectoryIdentifier(DomainName);
            try
            {
                using (LdapConnection connection = new LdapConnection(id, credentials, AuthType.Kerberos))
                {
                    connection.SessionOptions.Sealing = true;
                    connection.SessionOptions.Signing = true;
                    connection.Bind();
                    isValid = true;
                }
                return isValid;
            }
            catch (Exception ex)
            {
                return isValid;
            }
        }

        private List<string> GetAllGroupsOfUser(string LDAPUrl, string Username)
        {
            List<string> group = new List<string>();
            SearchResultCollection results;
            DirectorySearcher ds = null;
            DirectoryEntry de = new DirectoryEntry(LDAPUrl);
            ds = new DirectorySearcher(de);
            ds.Sort = new SortOption("name", SortDirection.Ascending);
            ds.PropertiesToLoad.Add("name");
            ds.PropertiesToLoad.Add("member");
            ds.Filter = "(&(objectCategory=Group))";
            results = ds.FindAll();
            foreach (SearchResult sr in results)
            {
                string grpName = sr.GetPropertyValue("name");
                if (sr.Properties["member"].Count > 0)
                {
                    foreach (string item in sr.Properties["member"])
                    {
                        DirectoryEntry de2 = new DirectoryEntry($"LDAP://{item}");
                        DirectorySearcher ds2 = BuildUserSearcher(de2);
                        ds2.Filter = "(&(objectCategory=User)(objectClass=person))";
                        SearchResult sr2 = ds2.FindOne();
                        string _user = sr2.GetPropertyValue("userPrincipalName");
                        if (sr2 != null && _user.ToLower().Equals(Username.ToLower()))
                        {
                            group.Add(grpName);
                            break;
                        }
                    }
                }
            }
            return group;
        }

        private ADUserDetails GetADUserInfo(string LDAPUrl, string Username)
        {
            ADUserDetails _user = new ADUserDetails();
            _user.Groups = new List<string>();
            SearchResultCollection results;
            DirectorySearcher ds = null;
            DirectoryEntry de = new DirectoryEntry(LDAPUrl);
            ds = new DirectorySearcher(de);
            ds.Sort = new SortOption("name", SortDirection.Ascending);
            ds.PropertiesToLoad.Add("name");
            ds.PropertiesToLoad.Add("member");
            ds.Filter = "(&(objectCategory=Group))";
            results = ds.FindAll();
            foreach (SearchResult sr in results)
            {
                string grpName = sr.GetPropertyValue("name");
                if (sr.Properties["member"].Count > 0)
                {
                    foreach (string item in sr.Properties["member"])
                    {
                        DirectoryEntry de2 = new DirectoryEntry($"LDAP://{item}");
                        DirectorySearcher ds2 = BuildUserSearcher(de2);
                        ds2.Filter = "(&(objectCategory=User)(objectClass=person))";
                        SearchResult sr2 = ds2.FindOne();
                        string _userName = sr2.GetPropertyValue("userPrincipalName");
                        if (sr2 != null && _userName.ToLower().Equals(Username.ToLower()))
                        {
                            _user.Email = sr2.GetPropertyValue("mail");
                            _user.GivenName = sr2.GetPropertyValue("givenname");
                            _user.SN = sr2.GetPropertyValue("sn");
                            _user.Groups.Add(grpName);
                            break;
                        }
                    }
                }
            }
            return _user;
        }


        private List<string> GetAllGroups(string LDAPUrl)
        {
            List<string> group = new List<string>();
            SearchResultCollection results;
            DirectorySearcher ds = null;
            DirectoryEntry de = new DirectoryEntry(LDAPUrl);
            ds = new DirectorySearcher(de);
            ds.Sort = new SortOption("name", SortDirection.Ascending);
            ds.PropertiesToLoad.Add("name");
            ds.Filter = "(&(objectCategory=Group))";
            results = ds.FindAll();
            foreach (SearchResult sr in results)
            {
                group.Add(sr.GetPropertyValue("name"));
            }
            return group;
        }

        public List<CustomerConfig> GetADConfig()
        {
            return new CustConfigDataAccess(TableSchema).GetADConfiguration();
        }

        public object SaveADConfig(string adDomain, string ldapUrl)
        {
            return new CustConfigDataAccess(TableSchema).SaveADConfiguration(adDomain, ldapUrl);
        }

        public object CheckADGroupAssignedForRole(Int64 ADGroupID, Int64 RoleID)
        {
            return new CustConfigDataAccess(TableSchema).CheckADGroupAssignedForRole(ADGroupID, RoleID);
        }

    }

    public class ADUserDetails
    {
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string SN { get; set; }
        public List<string> Groups { get; set; }
    }

    public static class ADExtensionMethods
    {
        public static string GetPropertyValue(this SearchResult sr, string propertyName)
        {
            string ret = string.Empty;

            if (sr.Properties[propertyName].Count > 0)
                ret = sr.Properties[propertyName][0].ToString();

            return ret;
        }
    }



}
