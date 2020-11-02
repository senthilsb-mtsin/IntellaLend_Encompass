using IntellaLend.BoxWrapper;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using MTSEntBlocks.UtilsBlock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace IL.AutomateBoxDownloder
{
    public class AutomateBoxDownloder : IMTSServiceBase
    {
        #region Private Variables

        private static string IntellaLendLoanUploadPath = string.Empty;
        //  private static string BoxDownloadRootFolderName = string.Empty;
        private static string BoxProcessedFolderID = string.Empty;
        private static int MaxRetryCount = 0;
        //  private static Int64 DefaultReviewTypeID = 0;
        private const Int32 PDF_CREATED = 0;
        private static string GhostScriptPath = string.Empty;
        private string LocalTenantName = string.Empty;

        #endregion

        #region Public Methods

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            //  BoxDownloadRootFolderName = Params.Find(f => f.Key == "BoxDownloadRootFolderName").Value;
            BoxProcessedFolderID = Params.Find(f => f.Key == "BoxProcessedFolderID").Value;
            MaxRetryCount = Convert.ToInt32(Params.Find(f => f.Key == "DownloadFailRetryCount").Value);
            GhostScriptPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "GhostScript", "gswin64c.exe");
            IntellaLendLoanUploadPath = Params.Find(f => f.Key == "IntellaLendLoanUploadPath").Value;
            //DefaultReviewTypeID = Convert.ToInt64(Params.Find(f => f.Key == "DefaultServiceTypeID").Value);
        }

        public bool DoTask()
        {
            try
            {
                var TenantList = AutomateBoxDownloderData.GetTenantList();
                foreach (var tenant in TenantList)
                {
                    Log($"Loop for TenantSchema : {tenant.TenantSchema}");
                    AutomateBoxDownloderData dataAccess = new AutomateBoxDownloderData(tenant.TenantSchema, MaxRetryCount);
                    LocalTenantName = tenant.TenantSchema;
                    DownloadFromBox(dataAccess, tenant.TenantSchema);
                    Log($"Download Complete for TenantSchema : {tenant.TenantSchema}");
                }

                return true;
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }

            return false;
        }

        #endregion

        #region Private Methods

        private void DownloadFromBox(AutomateBoxDownloderData dataAccess, string TenantSchema)
        {
            Log($"Inside DownloadFromBox()");
            //if (string.IsNullOrEmpty(BoxDownloadRootFolderID) || string.IsNullOrEmpty(BoxDownloadRootFolderName))
            //    throw new Exception("Box root path not configured");

            try
            {
                string exactPath = Path.Combine(IntellaLendLoanUploadPath, TenantSchema, "Input", DateTime.Now.ToString("yyyyMMdd"));
                Log($"exactPath : {exactPath}");
                if (!Directory.Exists(exactPath))
                    Directory.CreateDirectory(exactPath);

                Log($"Creating Box Client Instance");
                BoxAPIWrapper boxwrap = new BoxAPIWrapper(TenantSchema, 0);
                Log($"Box Client Instance Created");

                List<CustReviewLoanUploadPath> lsCustBoxFolderPath = dataAccess.GetBoxFolderPath();

                Log($"lsCustBoxFolderPath : {lsCustBoxFolderPath.Count}");
                foreach (CustReviewLoanUploadPath _custReviewLoanPath in lsCustBoxFolderPath)
                {
                    Int64 customerID = _custReviewLoanPath.CustomerID;
                    Int64 reviewTypeID = _custReviewLoanPath.ReviewTypeID;
                    Int64 loanTypeID = _custReviewLoanPath.LoanTypeID;

                    string[] boxFolderPath = _custReviewLoanPath.BoxUploadPath.Split(Path.DirectorySeparatorChar);

                    BoxFolderDetails boxFolderDetails = new BoxFolderDetails();

                    BoxCollection boxRootFolder = boxwrap.GetFolderDetails("0", 10000, 0);

                    for (int i = 0; i < boxFolderPath.Length; i++)
                    {
                        BoxEntity subFolder = boxRootFolder.BoxEntities.Where(b => b.Name == boxFolderPath[i]).FirstOrDefault();

                        if (subFolder != null)
                            boxRootFolder = boxwrap.GetFolderDetails(subFolder.Id, 10000, 0);
                    }

                    PDFMerger merger = null;

                    if (boxRootFolder.Path.Count == boxFolderPath.Length)
                    {
                        foreach (BoxEntity boxLoanFolder in boxRootFolder.BoxEntities)
                        {
                            Loan _loan = new Loan();
                            bool result = CreateLoan(_custReviewLoanPath, _loan, dataAccess);
                            string lockFileName = string.Empty;
                            try
                            {
                                if (result && _loan.LoanID > 0)
                                {
                                    string newFileName = TenantSchema + "_" + _loan.LoanID.ToString() + ".lck";
                                    string newDestFileName = TenantSchema + "_" + _loan.LoanID.ToString() + ".pdf";

                                    lockFileName = Path.Combine(exactPath, newFileName);
                                    string destFile = Path.Combine(exactPath, newFileName);

                                    BoxCollection boxLoanFolderDetail = boxwrap.GetFolderDetails(boxLoanFolder.Id, 10000, 0, ".pdf,.PDF,.fnm");

                                    if (boxLoanFolderDetail.BoxEntities.Any(b => (b.Name.EndsWith(".pdf") || b.Name.EndsWith(".PDF")) && b.Name.EndsWith(".fnm")))
                                    {
                                        merger = new PDFMerger(lockFileName);
                                        merger.OpenDocument();
                                        foreach (BoxEntity file in boxLoanFolderDetail.BoxEntities)
                                        {
                                            if (file.Type == "file")
                                            {
                                                if (file.Name.EndsWith(".fnm"))
                                                {
                                                    byte[] fnmBytes = boxwrap.DownloadFile(file.Id);
                                                    Dictionary<string, List<Dictionary<string, string>>> loanTapeDetails = ReadLoanTape(fnmBytes, dataAccess, file.Name);
                                                }
                                                else if (file.Name.EndsWith(".pdf") || file.Name.EndsWith(".PDF"))
                                                {
                                                    merger.AppendPDF(boxwrap.DownloadFile(file.Id));
                                                }
                                            }
                                        }
                                        merger.SaveDocument();
                                        MoveBoxFolder(boxwrap, dataAccess, _loan.LoanID, boxLoanFolder.Id, BoxProcessedFolderID);
                                        File.Move(lockFileName, destFile);
                                    }
                                    else
                                    {
                                        dataAccess.RemoveLoanEntries(_loan.LoanID);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                dataAccess.RemoveLoanEntries(_loan.LoanID);

                                if (!string.IsNullOrEmpty(lockFileName) && File.Exists(lockFileName))
                                    File.Delete(lockFileName);

                                MTSExceptionHandler.HandleException(ref ex);
                            }

                        }
                    }
                }


                //BoxFolderDetails rootPath = boxwrap.GetFolder(BoxDownloadRootFolderID, BoxDownloadRootFolderName, 1);

                //if (rootPath != null)
                //{
                //    if (rootPath.SubFolder != null && rootPath.SubFolder.Count > 0)
                //    {
                //        BoxEntity importFolder = rootPath.SubFolder.Where(b => b.Name == "Import").FirstOrDefault();
                //        BoxEntity processedFolder = rootPath.SubFolder.Where(b => b.Name == "Processed").FirstOrDefault();

                //        if (importFolder != null)
                //        {
                //            if (processedFolder == null)
                //                processedFolder = boxwrap.CreateFolderBoxEntity("Processed", rootPath.Id);

                //            List<BoxEntity> CustomerFolders = boxwrap.GetSubFolders(importFolder.Id, 10000);
                //            List<CustomerMaster> _ilCustomers = dataAccess.GetBoxCustomers();

                //            foreach (BoxEntity customer in CustomerFolders)
                //            {
                //                CustomerMaster cm = _ilCustomers.Where(c => c.CustomerName.Trim().ToUpper().Equals(customer.Name.Trim().ToUpper())).FirstOrDefault();
                //                if (cm != null)
                //                    DownloadFromCustomer(boxwrap, dataAccess, customer, processedFolder, cm, exactPath);
                //            }
                //        }
                //        else
                //            throw new Exception($"Import folder not found");
                //    }
                //    else
                //        throw new Exception($"Subfolders not found under the root path. RootFolderName : {BoxDownloadRootFolderName}");
                //}
                //else
                //    throw new Exception($"Box root path not found. FolderName : {BoxDownloadRootFolderName}");


                Log($"After DownloadFromBox()");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<string, List<Dictionary<string, string>>> ReadLoanTape(byte[] loanTapeBytes, AutomateBoxDownloderData dataAccess, string fileName)
        {
            try
            {
                List<string> lsTapeLines = new List<string>();
                using (StreamReader stream = new StreamReader(new MemoryStream(loanTapeBytes)))
                {
                    while (!stream.EndOfStream)
                    {
                        lsTapeLines.Add(stream.ReadLine());
                    }
                }

                List<LoanTapeDefinition> loanTypeDef = dataAccess.GetLoanTapeDefinition();

                List<string> distField = loanTypeDef.Select(l => l.FieldID.Split('-')[0]).Distinct().ToList();

                Dictionary<string, List<Dictionary<string, string>>> dic = new Dictionary<string, List<Dictionary<string, string>>>();

                foreach (string keyField in distField)
                {
                    List<LoanTapeDefinition> drs = loanTypeDef.Where(r => r.FieldID.StartsWith(keyField)).ToList();
                    List<string> lsFieldsofType = lsTapeLines.Where(x => x.StartsWith(keyField)).ToList();

                    foreach (string keyLine in lsFieldsofType)
                    {
                        Dictionary<string, string> dicKeyField = new Dictionary<string, string>();
                        foreach (LoanTapeDefinition dr in drs)
                        {
                            string fieldID = dr.FieldID.ToString();
                            int pos = Convert.ToInt32(dr.Position) - 1;
                            int len = Convert.ToInt32(dr.FieldLength);
                            dicKeyField[fieldID] = keyLine.Substring(pos, len);
                        }
                        dic[keyField].Add(dicKeyField);
                    }
                }

                return dic;
            }
            catch (Exception ex)
            {
                Exception exe = new Exception($"Error While Reading file : {fileName}", ex);
                throw exe;
            }

        }

        private bool CreateLoan(CustReviewLoanUploadPath _custReviewLoanPath, Loan loan, AutomateBoxDownloderData dataAccess)
        {
            Int64 Priority = dataAccess.GetReviewTypePriority(_custReviewLoanPath.ReviewTypeID);
            loan.ReviewTypeID = _custReviewLoanPath.ReviewTypeID;
            loan.LoanTypeID = _custReviewLoanPath.LoanTypeID;
            loan.UploadedUserID = 1;
            loan.CustomerID = _custReviewLoanPath.CustomerID;
            loan.LastAccessedUserID = 0;
            loan.UploadType = UploadConstant.BOX;
            loan.CreatedOn = DateTime.Now;
            loan.AuditMonthYear = Convert.ToDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(IntellaLend.Constance.DateConstance.AuditDateFormat));
            loan.ModifiedOn = DateTime.Now;
            loan.LoanGUID = Guid.NewGuid();
            loan.Priority = Priority;
            return dataAccess.AddBoxFileUploadDetails(loan);
        }


        //private void DownloadFromCustomer(BoxAPIWrapper boxwrap, AutomateBoxDownloderData dataAccess, BoxEntity customerFolder, BoxEntity processedFolder, CustomerMaster ilCustomer, string exactPath)
        //{
        //    try
        //    {
        //        List<BoxEntity> loanTypeFolders = boxwrap.GetSubFolders(customerFolder.Id, 10000);

        //        List<LoanTypeMaster> mappingLoanTypes = dataAccess.GetCustomerLoanTypes(ilCustomer.CustomerID, DefaultReviewTypeID);

        //        foreach (BoxEntity loanType in loanTypeFolders)
        //        {
        //            LoanTypeMaster ilLoanType = mappingLoanTypes.Where(m => m.LoanTypeName.Trim().ToUpper().Equals(loanType.Name.Trim().ToUpper())).FirstOrDefault();

        //            if (ilLoanType != null)
        //                DownloadFromLoanType(boxwrap, dataAccess, loanType, ilCustomer.CustomerID, ilLoanType.LoanTypeID, processedFolder, customerFolder, exactPath);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private void DownloadFromLoanType(BoxAPIWrapper boxwrap, AutomateBoxDownloderData dataAccess, BoxEntity srcLoanTypeFolder, Int64 CustomerID, Int64 LoanTypeID, BoxEntity processedFolder, BoxEntity customerFolder, string exactPath)
        //{
        //    try
        //    {
        //        List<BoxEntity> loanFiles = boxwrap.GetSubFolders(srcLoanTypeFolder.Id, 10000, ".pdf,.PDF");

        //        foreach (BoxEntity loanPDF in loanFiles)
        //        {
        //            string lockFileName = string.Empty;
        //            string OrgFileName = string.Empty;

        //            byte[] pdfBytes = boxwrap.DownloadFile(loanPDF.Id);

        //            Loan loan = null;

        //            CreateLoanObject(dataAccess, loan, CustomerID, LoanTypeID, loanPDF.Name);

        //            if (loan != null && loan.LoanID > 0)
        //            {

        //                string newFileName = LocalTenantName + "_" + loan.LoanID.ToString() + ".lck";

        //                lockFileName = Path.Combine(exactPath, newFileName);

        //                OrgFileName = Path.ChangeExtension(lockFileName, Path.GetExtension(loanPDF.Name));

        //                try
        //                {
        //                    File.WriteAllBytes(lockFileName, pdfBytes);
        //                }
        //                catch (Exception ex)
        //                {
        //                    dataAccess.RevertLoanInsert(loan.LoanID);
        //                    throw new Exception($"Error while writing file on the path '{lockFileName}'", ex);
        //                }

        //                string customerFolderID = boxwrap.CreateFolder(customerFolder.Name.Trim(), processedFolder.Id);

        //                string loanTypeFolderID = boxwrap.CreateFolder(srcLoanTypeFolder.Name.Trim(), customerFolderID);

        //                string newFileID = MoveBoxFile(boxwrap, dataAccess, loan.LoanID, loanPDF.Id, loanTypeFolderID);

        //                RenameLCKFile(boxwrap, dataAccess, lockFileName, OrgFileName, loan.LoanID, newFileID, srcLoanTypeFolder.Id);
        //            }
        //            else
        //                throw new Exception($"Loan record not created on the Database for the file '{loanPDF.Name}' on the folder path '{loanPDF.ParentPath}'");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void RenameLCKFile(BoxAPIWrapper boxwrap, AutomateBoxDownloderData dataAccess, string lockFileName, string OrgFileName, Int64 loanID, string fileID, string destinationFolderID)
        {
            try
            {
                File.Move(lockFileName, OrgFileName);
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(fileID))
                    boxwrap.MoveFile(fileID, destinationFolderID);

                dataAccess.RevertLoanInsert(loanID);
                throw new Exception($"Error while moving file from LCK to PDF, ", ex);
            }
        }

        private string MoveBoxFolder(BoxAPIWrapper boxwrap, AutomateBoxDownloderData dataAccess, Int64 loanID, string folderID, string destinationFolderID)
        {
            try
            {
                return boxwrap.MoveFolder(folderID, destinationFolderID);
            }
            catch (Exception ex)
            {
                dataAccess.RevertLoanInsert(loanID);
                throw new Exception($"Error while moving file on the Box, source folder id '{folderID}' Desitination folder id '{destinationFolderID}' ", ex);
            }

            return string.Empty;
        }

        //private Loan CreateLoanObject(AutomateBoxDownloderData dataAccess, Loan loan, Int64 CustomerID, Int64 LoanTypeID, string boxFileName)
        //{
        //    try
        //    {
        //        loan = new Loan();
        //        loan.ReviewTypeID = DefaultReviewTypeID;
        //        loan.LoanTypeID = LoanTypeID;
        //        loan.SubStatus = 0;
        //        loan.UploadedUserID = 1;
        //        loan.LoggedUserID = 0;
        //        loan.CustomerID = CustomerID;
        //        loan.UploadType = UploadConstant.BOX;
        //        loan.AuditMonthYear = Convert.ToDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(IntellaLend.Constance.DateConstance.AuditDateFormat));
        //        loan.CreatedOn = DateTime.Now;
        //        loan.ModifiedOn = DateTime.Now;
        //        loan.LastAccessedUserID = 0;
        //        loan.LoanGUID = Guid.NewGuid();
        //        loan.Priority = dataAccess.GetReviewTypePriority(DefaultReviewTypeID);
        //        loan.Status = StatusConstant.READY_FOR_IDC;
        //        loan.FileName = boxFileName;
        //        dataAccess.InsertLoan(loan);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error while creating Loan record for the box file '{boxFileName}'", ex);
        //    }

        //    return loan;
        //}

        private void Log(string msg)
        {
            Logger.WriteTraceLog(msg);
        }

        #endregion
    }
}
