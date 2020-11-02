using Box.V2;
using Box.V2.Auth;
using Box.V2.Config;
using Box.V2.Exceptions;
using Box.V2.Models;
using IntellaLend.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IntellaLend.BoxWrapper
{
    public class BoxAPIWrapper
    {
        #region Members
        private BoxConfig boxConfig;
        public BoxUserToken UserToken;
        private string AuthCode;
        private BoxClient boxClient;
        private OAuthSession Session;
        private BoxAPIWrapperDataAccess dataAccess;
        #endregion

        #region Public Members
        public bool IsValidToken
        {
            get
            {
                return ValidateToken();
            }
        }

        public string BoxAuthURL
        {
            get
            {
                return boxConfig.AuthCodeUri.AbsoluteUri;
            }
        }


        #endregion

        #region Constructors
        private BoxAPIWrapper()
        { }

        public BoxAPIWrapper(string tenantSchema, Int64 userID)
        {
            //  ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            userID = 1;
            dataAccess = new BoxAPIWrapperDataAccess(tenantSchema);
            UserToken = dataAccess.GetUserToken(userID);
            if (UserToken.UserID != 0)
            {
                boxConfig = new BoxConfig(BoxAPIWrapperDataAccess.BoxClientID, BoxAPIWrapperDataAccess.BoxClientSecretID, new Uri(BoxAPIWrapperDataAccess.BoxRedirectURL));
                //  ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Session = new OAuthSession(UserToken.Token, UserToken.RefreshToken, UserToken.ExpireTime, UserToken.TokenType);
                boxClient = new BoxClient(boxConfig, Session);
                boxClient.Auth.SessionAuthenticated += NewSessionEventHandler;
            }
            else
            {
                UserToken.UserID = userID;
                boxConfig = new BoxConfig(BoxAPIWrapperDataAccess.BoxClientID, BoxAPIWrapperDataAccess.BoxClientSecretID, new Uri(BoxAPIWrapperDataAccess.BoxRedirectURL));
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                boxClient = new BoxClient(boxConfig);
                boxClient.Auth.SessionAuthenticated += NewSessionEventHandler;
            }
        }

        #endregion

        #region events
        public void NewSessionEventHandler(object sender, EventArgs e)
        {
            UpdateUserToken();
        }

        private bool ValidateToken()
        {
            try
            {
                var test = Task.Run(() => boxClient.FoldersManager.GetFolderItemsAsync("0", 1, 0)).GetAwaiter().GetResult();
                return true;
            }
            catch (Exception ex)
            {
                if (HandleException(ex).GetType() == typeof(BoxSessionInvalid))
                    return false;
                else
                    throw HandleException(ex);
            }
        }

        private void UpdateUserToken()
        {
            if (boxClient.Auth.Session != null)
            {
                UserToken.Token = boxClient.Auth.Session.AccessToken;
                UserToken.RefreshToken = boxClient.Auth.Session.RefreshToken;
                UserToken.ExpireTime = boxClient.Auth.Session.ExpiresIn;
                UserToken.TokenType = boxClient.Auth.Session.TokenType;
                dataAccess.UpdateUserToken(UserToken);
            }
        }

        #endregion

        #region Methods

        public TokenValidation CheckUserBoxToken()
        {
            return new TokenValidation()
            {
                TokenStatus = string.IsNullOrEmpty(UserToken.Token) ? 0 : IsValidToken ? 1 : 2,
                BoxAuthURL = boxConfig.AuthCodeUri.AbsoluteUri
            };
        }

        public bool GenrateToken(string authCode)
        {
            try
            {
                AuthCode = authCode;
                Task.Run(() => boxClient.Auth.AuthenticateAsync(AuthCode)).GetAwaiter().GetResult();
                var result = IsValidToken;
                if (result)
                {
                    //Update here if event not triggered
                    UpdateUserToken();
                }

                if (IsValidToken)
                {
                    string userName = GetUserLogin();
                    if (!string.IsNullOrEmpty(userName))
                    {
                        if (userName.Trim().ToLower() != BoxAPIWrapperDataAccess.BoxUserName.Trim().ToLower())
                        {
                            if (UserToken != null)
                                dataAccess.RemoveUserToken(UserToken);
                            throw new Exception("Unable to authenticate! Box user does not match with the configured user.");
                        }
                    }
                    else
                    {
                        throw new Exception("Unable to retrive current user details from Box");
                    }
                }

                return IsValidToken;
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }

        }

        private string GetUserLogin()
        {
            BoxUser user = Task.Run(() => boxClient.UsersManager.GetCurrentUserInformationAsync()).GetAwaiter().GetResult();

            if (user != null)
                return user.Login;
            else
                return null;
        }

        public BoxFolderDetails GetFolder(string folderID, string folderName, int limit)
        {
            try
            {
                BoxFolderDetails collection = null;

                var items = Task.Run(() => boxClient.SearchManager.SearchAsync(folderID, limit, 0,
                    new List<string>() { BoxFolder.FieldName,
                        BoxFolder.FieldPathCollection,
                        BoxFolder.FieldModifiedAt ,
                     BoxFolder.FieldSize}
                    )).GetAwaiter().GetResult();

                foreach (var item in items.Entries)
                {
                    if (item.Name.ToUpper().Trim().Equals(folderName.ToUpper().Trim()))
                    {
                        BoxFolder folderDetails = Task.Run(() => boxClient.FoldersManager.GetInformationAsync(item.Id)).GetAwaiter().GetResult();
                        collection = new BoxFolderDetails()
                        {
                            Type = item.Type,
                            Id = item.Id,
                            Name = item.Name,
                            Size = item.Size,
                            ParentPath = GetParentPath(item.PathCollection),
                            ModifiedTime = item.ModifiedAt,
                            Path = GetFolderPath(folderDetails),
                            SubFolder = GetSubFolders(item.Id, 1000)
                        };
                        collection.Path.Add(new BoxEntity() { Id = folderDetails.Id, Name = folderDetails.Name });
                        break;
                    }
                }

                return collection;
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }


        public BoxFolderDetails SearchForFolder(string folderName, int limit)
        {
            try
            {
                BoxFolderDetails collection = null;

                var items = Task.Run(() => boxClient.SearchManager.SearchAsync(folderName, limit, 0,
                    new List<string>() { BoxFolder.FieldName,
                        BoxFolder.FieldPathCollection,
                        BoxFolder.FieldModifiedAt ,
                     BoxFolder.FieldSize}
                    )).GetAwaiter().GetResult();

                foreach (var item in items.Entries)
                {
                    if (item.Name.ToUpper().Trim().Equals(folderName.ToUpper().Trim()))
                    {
                        BoxFolder folderDetails = Task.Run(() => boxClient.FoldersManager.GetInformationAsync(item.Id)).GetAwaiter().GetResult();
                        collection = new BoxFolderDetails()
                        {
                            Type = item.Type,
                            Id = item.Id,
                            Name = item.Name,
                            Size = item.Size,
                            ParentPath = GetParentPath(item.PathCollection),
                            ModifiedTime = item.ModifiedAt,
                            Path = GetFolderPath(folderDetails),
                            SubFolder = GetSubFolders(item.Id, 1000)
                        };
                        collection.Path.Add(new BoxEntity() { Id = folderDetails.Id, Name = folderDetails.Name });
                        break;
                    }
                }

                return collection;
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public string MoveFile(string soruceFileID, string destinationFolderID)
        {
            try
            {
                var requestParams = new BoxFileRequest()
                {
                    Id = soruceFileID,
                    Parent = new BoxRequestEntity()
                    {
                        Id = destinationFolderID
                    }
                };

                BoxFile fileCopy = boxClient.FilesManager.CopyAsync(requestParams).GetAwaiter().GetResult();

                if (DeleteFile(soruceFileID))
                {
                    return fileCopy.Id;
                }
                else
                    return string.Empty;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string MoveFolder(string soruceFileID, string destinationFolderID)
        {
            try
            {
                var requestParams = new BoxFolderRequest()
                {
                    Id = soruceFileID,
                    Parent = new BoxRequestEntity()
                    {
                        Id = destinationFolderID
                    }
                };

                BoxFolder fileCopy = boxClient.FoldersManager.CopyAsync(requestParams).GetAwaiter().GetResult();

                if (DeleteFile(soruceFileID))
                {
                    return fileCopy.Id;
                }
                else
                    return string.Empty;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private bool DeleteFile(string fileID)
        {
            try
            {
                return boxClient.FilesManager.DeleteAsync(fileID).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Create File in Box
        /// <param name="folderID">Parent Folder ID, default 0 for base Box folder</param>
        /// <param name="fileName">Destination File Name with Extension</param>
        /// <param name="fileBytes">Uploading File bytes</param>
        /// <returns>Created File ID</returns>
        /// </summary>
        public string CreateFile(string fileName, byte[] fileBytes, string extension, string folderID = "0")
        {
            try
            {
                BoxCollection files = GetFolderDetails(folderID, 1000, 0, extension);

                Int32 fileCount = files.BoxEntities.Where(a => a.Type == "File").Count();

                string[] fileNames = fileName.Split('.');

                if (fileCount > 0)
                {
                    int lastIndex = fileNames.Length - 1;
                    List<string> fileN = new List<string>();
                    for (int i = 0; i < lastIndex; i++)
                    {
                        fileN.Add(fileNames[i]);
                    }

                    fileName = string.Join(".", fileN) + "_" + (fileCount + 1).ToString() + extension;
                }

                BoxFileRequest requestParams = null;
                using (MemoryStream ms = new MemoryStream(fileBytes))
                {
                    requestParams = new BoxFileRequest()
                    {
                        Name = fileName,
                        Parent = new BoxRequestEntity() { Id = folderID }
                    };

                    BoxFile file = boxClient.FilesManager.UploadAsync(requestParams, ms).GetAwaiter().GetResult();

                    return file.Id;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Create File in Box
        /// <param name="folderID">Parent Folder ID, default 0 for base Box folder</param>
        /// <param name="fileName">Destination File Name with Extension</param>
        /// <param name="fileBytes">Uploading File bytes</param>
        /// <returns>Created File ID</returns>
        /// </summary>
        public string CreateFile(string fileName, byte[] fileBytes, string folderID = "0")
        {
            try
            {
                BoxFileRequest requestParams = null;
                using (MemoryStream ms = new MemoryStream(fileBytes))
                {
                    requestParams = new BoxFileRequest()
                    {
                        Name = fileName,
                        Parent = new BoxRequestEntity() { Id = folderID }
                    };

                    BoxFile file = boxClient.FilesManager.UploadAsync(requestParams, ms).GetAwaiter().GetResult();

                    return file.Id;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string FolderExists(string folderName, string parentFolderID = "0")
        {
            try
            {
                List<BoxEntity> folderCollection = GetSubFolders(parentFolderID, 1000);

                BoxEntity folderExists = folderCollection.Where(f => f.Name.Trim().ToUpper() == folderName.Trim().ToUpper()).FirstOrDefault();

                if (folderExists != null)
                    return folderExists.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return string.Empty;
        }


        public bool CreateLoanCSV(string ExportFolderID, string CustomerName, string LoanTypeName, string LoanNumber, string fileExtension, string LoanJSONObject)
        {
            try
            {
                string customerID = CreateFolder(CustomerName, ExportFolderID);

                string LoanTypeID = CreateFolder(LoanTypeName, customerID);

                string LoanID = CreateFolder(LoanNumber, LoanTypeID);

                byte[] bytes = Encoding.ASCII.GetBytes(LoanJSONObject);

                string fileCreated = CreateFile(LoanNumber + fileExtension, bytes, fileExtension, LoanID);

                return !string.IsNullOrEmpty(fileCreated);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }


        private BoxEntity FolderExistsEntity(string folderName, string parentFolderID = "0")
        {
            try
            {
                List<BoxEntity> folderCollection = GetSubFolders(parentFolderID, 1000);

                BoxEntity folderExists = folderCollection.Where(f => f.Name.Trim().ToUpper() == folderName.Trim().ToUpper()).FirstOrDefault();

                if (folderExists != null)
                    return folderExists;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }

        /// <summary>
        /// To Create Folder in Box
        /// <param name="parentFolderID">Default 0 for base Box folder</param>
        /// <param name="folderName">Destination Folder Name</param>
        /// <returns>Created Folder ID</returns>
        /// </summary>
        public BoxEntity CreateFolderBoxEntity(string folderName, string parentFolderID = "0")
        {
            try
            {
                BoxEntity existingFolder = FolderExistsEntity(folderName, parentFolderID);

                if (existingFolder != null)
                    return existingFolder;

                var folderParams = new BoxFolderRequest()
                {
                    Name = folderName,
                    Parent = new BoxRequestEntity()
                    {
                        Id = parentFolderID
                    }
                };
                BoxFolder folder = boxClient.FoldersManager.CreateAsync(folderParams).GetAwaiter().GetResult();

                return new BoxEntity()
                {
                    Type = folder.Type,
                    Id = folder.Id,
                    Name = folder.Name,
                    ParentPath = GetParentPath(folder.PathCollection),
                    Size = folder.Size,
                    ModifiedTime = folder.ModifiedAt
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Create Folder in Box
        /// <param name="parentFolderID">Default 0 for base Box folder</param>
        /// <param name="folderName">Destination Folder Name</param>
        /// <returns>Created Folder ID</returns>
        /// </summary>
        public string CreateFolder(string folderName, string parentFolderID = "0")
        {
            try
            {
                string existingFolderID = FolderExists(folderName, parentFolderID);

                if (!string.IsNullOrEmpty(existingFolderID))
                    return existingFolderID;

                var folderParams = new BoxFolderRequest()
                {
                    Name = folderName,
                    Parent = new BoxRequestEntity()
                    {
                        Id = parentFolderID
                    }
                };
                BoxFolder folder = boxClient.FoldersManager.CreateAsync(folderParams).GetAwaiter().GetResult();

                return folder.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<BoxEntity> GetSubFolders(string id, int limit, string fileFilters = "")
        {
            List<BoxEntity> result = new List<BoxEntity>();
            try
            {
                var items = Task.Run(() => boxClient.FoldersManager.GetFolderItemsAsync(id, limit, 0,
                      new List<string>() { BoxFolder.FieldName,
                        BoxFolder.FieldPathCollection,
                        BoxFolder.FieldModifiedAt ,
                     BoxFolder.FieldSize}
                      )).GetAwaiter().GetResult();

                foreach (var item in items.Entries)
                {
                    BoxEntity boxEnt = new BoxEntity();

                    if (!string.IsNullOrEmpty(fileFilters))
                    {
                        if (item.Type == "file")
                        {
                            string fileExtension = Path.GetExtension(item.Name);
                            if (fileFilters.Split(',').Contains(fileExtension))
                            {
                                boxEnt = new BoxEntity()
                                {
                                    Type = item.Type,
                                    Id = item.Id,
                                    Name = item.Name,
                                    ParentPath = GetParentPath(item.PathCollection),
                                    Size = item.Size,
                                    ModifiedTime = item.ModifiedAt
                                };
                            }
                        }
                    }
                    else
                    {
                        boxEnt = new BoxEntity()
                        {
                            Type = item.Type,
                            Id = item.Id,
                            Name = item.Name,
                            ParentPath = GetParentPath(item.PathCollection),
                            Size = item.Size,
                            ModifiedTime = item.ModifiedAt
                        };
                    }

                    result.Add(boxEnt);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }

            return result;
        }

        public BoxCollection GetFolderDetails(string id, int limit, int offSet, string fileFilters = "")
        {
            try
            {
                BoxCollection collection = new BoxCollection();
                List<BoxEntity> result = new List<BoxEntity>();

                var items = Task.Run(() => boxClient.FoldersManager.GetFolderItemsAsync(id, limit, offSet,
                    new List<string>() { BoxFolder.FieldName,
                        BoxFolder.FieldPathCollection,
                        BoxFolder.FieldModifiedAt ,
                     BoxFolder.FieldSize}
                    )).GetAwaiter().GetResult();

                foreach (var item in items.Entries)
                {
                    if (item.Type == "file" && !string.IsNullOrEmpty(fileFilters))
                    {
                        string fileExtension = Path.GetExtension(item.Name);
                        if (!fileFilters.Split(',').Contains(fileExtension))
                        {
                            continue;
                        }
                    }

                    var boxEnt = new BoxEntity()
                    {
                        Type = item.Type,
                        Id = item.Id,
                        Name = item.Name,
                        ParentPath = GetParentPath(item.PathCollection),
                        Size = item.Size,
                        ModifiedTime = item.ModifiedAt

                    };


                    result.Add(boxEnt);

                }
                collection.BoxEntities = result;
                collection.Limit = items.Limit;
                collection.Offset = items.Offset;
                collection.TotalCount = items.TotalCount;
                BoxFolder folderDetails = Task.Run(() => boxClient.FoldersManager.GetInformationAsync(id)).GetAwaiter().GetResult();
                collection.Path = GetFolderPath(folderDetails);
                collection.Path.Add(new BoxEntity() { Id = folderDetails.Id, Name = folderDetails.Name });

                return collection;
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public BoxEntity GetFileDetails(string id)
        {
            try
            {
                BoxFile fileDetails = Task.Run(() => boxClient.FilesManager.GetInformationAsync(id)).GetAwaiter().GetResult();
                BoxEntity file = new BoxEntity();
                file.Id = fileDetails.Id;
                file.Name = fileDetails.Name;
                file.ModifiedTime = fileDetails.ModifiedAt;
                file.Size = fileDetails.Size;
                file.Id = fileDetails.Id;
                file.ParentPath = GetParentPath(fileDetails.PathCollection);
                return file;
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }

        }

        private string GetParentPath(BoxCollection<BoxFolder> folderCollextion)
        {
            string path = "";
            foreach (var item in folderCollextion.Entries)
            {
                path += "/" + item.Name;
            }
            return path;
        }

        private List<BoxEntity> GetFolderPath(BoxFolder folderDetails)
        {
            List<BoxEntity> paths = new List<BoxEntity>();

            foreach (var item in folderDetails.PathCollection.Entries)
            {
                paths.Add(new BoxEntity() { Name = item.Name, Id = item.Id });
            }
            return paths;
        }


        public static byte[] ReadStreamBytesFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public byte[] DownloadFile(string fileId)
        {
            try
            {
                return ReadStreamBytesFully(Task.Run(() => boxClient.FilesManager.DownloadStreamAsync(fileId)).GetAwaiter().GetResult());
            }
            catch (Exception ex)
            {
                throw HandleException(ex);
            }
        }

        public Exception HandleException(Exception ex)
        {

            if ((ex != null && ex.GetType() == typeof(BoxSessionInvalidatedException)) || string.IsNullOrEmpty(ex.Message))
            {
                return new BoxSessionInvalid("Box token releated exception.Please relogin to get new token");
            }

            try
            {
                BoxException bex = JsonConvert.DeserializeObject<BoxException>(ex.Message);
                if (bex != null && bex.ContextInfo != null && bex.ContextInfo.BoxErrors != null && bex.ContextInfo.BoxErrors.Count > 0)
                    return new Exception(bex.ContextInfo.BoxErrors[0].Message);
                else if (!string.IsNullOrEmpty(bex.Message))
                    return new Exception(bex.Message);
            }
            catch (Exception jex)
            {
            }
            try
            {
                BoxSingleException bex = JsonConvert.DeserializeObject<BoxSingleException>(ex.Message);
                if (bex != null && !string.IsNullOrEmpty(bex.Message))
                    return new Exception(bex.Message);
            }
            catch (Exception jex)
            {
            }


            return new Exception(ex.Message);
        }

        #endregion
    }

    #region Models


    public class BoxCollection
    {
        public List<BoxEntity> BoxEntities { get; set; }
        public Int64 TotalCount { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public List<BoxEntity> Path { get; set; }
    }

    public class BoxFolderDetails
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public string ParentPath { get; set; }
        public long? Size { get; set; }
        public List<BoxEntity> Path { get; set; }
        public List<BoxEntity> SubFolder { get; set; }
    }

    public class BoxEntity
    {
        public bool Selected { get; set; }
        public Int64 Priority { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public string ParentPath { get; set; }
        public long? Size { get; set; }
    }

    #endregion

    #region ExceptionClass

    public class TokenValidation
    {
        public int TokenStatus { get; set; }
        public string BoxAuthURL { get; set; }
    }

    public class BoxSingleException
    {
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
        [JsonProperty(PropertyName = "error_description")]
        public string Message { get; set; }
    }

    public class BoxException
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
        [JsonProperty(PropertyName = "context_info")]
        public BoxErrorCollection ContextInfo { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }

    public class BoxErrorCollection
    {
        [JsonProperty(PropertyName = "errors")]
        public List<BoxError> BoxErrors { get; set; }
    }

    public class BoxError
    {
        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }


    public class BoxSessionInvalid : Exception
    {
        public BoxSessionInvalid()
        : base() { }

        public BoxSessionInvalid(string message)
        : base(message) { }
    }
    #endregion
}
