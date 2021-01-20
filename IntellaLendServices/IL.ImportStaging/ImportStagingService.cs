using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace IL.ImportStaging
{
    public class ImportStagingService : IMTSServiceBase
    {
        #region Service Start DoTask
        private static string OutputPath = string.Empty;
        private static string LockExt = ".lck";
        private static string ImageErrorExt = ".imgerror";
        private static string ErrorExt = ".error";
        private static string NotILLoanExt = ".notILLoan";

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            OutputPath = Params.Find(f => f.Key == "EphesoftOutputPath").Value;
        }

        public bool DoTask()
        {

            try
            {
                string[] EphesoftOutputPaths = OutputPath.Split(',');

                foreach (string EphesoftOutputPath in EphesoftOutputPaths)
                {
                    //DirectoryInfo dic = new DirectoryInfo(Path.Combine(EphesoftOutputPath, "Output"));
                    MoveXmlToImportStaging(EphesoftOutputPath);
                    //bool result = true;
                    //do
                    //{
                    //    MoveXmlToImportStaging(EphesoftOutputPath);
                    //    FileInfo[] files = dic.GetFiles("*.xml", SearchOption.AllDirectories).Where(f => f.Length > 0).ToArray();
                    //    result = files.Length > 0;
                    //}
                    //while (result);
                }

                return true;
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return true;
            }
        }
        #endregion

        private bool MoveXmlToImportStaging(string EphesoftOutputPath)
        {
            string[] _xmlFiles = Directory.GetFiles(Path.Combine(EphesoftOutputPath, "Output"), "*.xml", SearchOption.AllDirectories);
            List<TenantMaster> TenantLists = ImportStagingDataAccess.GetAllTenants();
            foreach (string _xmlFile in _xmlFiles)
            {
                string lckpath = string.Empty;
                string errorPath = string.Empty;
                try
                {
                    string xml = File.ReadAllText(_xmlFile);

                    if (!IsValidXml(xml))
                        continue;

                    string BatchFolderName = Path.GetFileName(Path.GetDirectoryName(_xmlFile));

                    ImportStagingDataAccess dataAccess = null;
                    ImportStagings _importStagings = null;
                    bool _missingDoc = false;

                    foreach (TenantMaster _tenant in TenantLists)
                    {
                        dataAccess = new ImportStagingDataAccess(_tenant.TenantSchema);

                        _importStagings = dataAccess.GetLoanDetails(BatchFolderName, ref _missingDoc);

                        if (_importStagings != null)
                            break;
                    }

                    string notILLoanXML = Path.ChangeExtension(_xmlFile, NotILLoanExt);

                    if (_importStagings != null)
                    {
                        if (_missingDoc && !dataAccess.CheckLoanStatus(_importStagings.LoanId))
                            continue;

                        lckpath = Path.ChangeExtension(_xmlFile, LockExt);
                        string imageErrorpath = Path.ChangeExtension(_xmlFile, ImageErrorExt);
                        errorPath = Path.ChangeExtension(_xmlFile, ErrorExt);
                        bool imageError = false;
                        try
                        {
                            XMLBatchImage _batch = null;
                            try
                            {
                                _batch = new XMLBatchImage(GetXMLNavigator(xml), Path.GetDirectoryName(_xmlFile));
                            }
                            catch (ImagePathException ex)
                            {
                                Exception newEx = new Exception($"XML File : {_xmlFile}", ex);
                                MTSExceptionHandler.HandleException(ref newEx);
                                continue;
                            }
                            catch (Exception ex)
                            {
                                imageError = true;
                                //_importStagings.Status = ImportStagingConstant.Error;
                                //_importStagings.ErrorMessage = $"Error while ImportStaging for Image of the Loan : {ex.Message} {Environment.NewLine} {ex.StackTrace}";
                                //File.Move(_xmlFile, imageErrorpath);
                                Exception newEx = new Exception($"XML File : {_xmlFile}", ex);
                                MTSExceptionHandler.HandleException(ref newEx);
                                throw ex;
                            }

                            List<string> _imgPath = _batch.ImagePaths;

                            if (_imgPath.Count > 0)
                            {
                                try
                                {
                                    byte[] _imgBytes = File.ReadAllBytes(_imgPath[0]);
                                    File.Move(_xmlFile, lckpath);
                                }
                                catch (Exception ex)
                                {
                                    imageError = true;
                                    //_importStagings.Status = ImportStagingConstant.Error;
                                    //_importStagings.ErrorMessage = $"Error while ImportStaging : {ex.Message} {Environment.NewLine} {ex.StackTrace}";
                                    File.Move(_xmlFile, imageErrorpath);
                                    Exception newEx = new Exception($"XML File : {_xmlFile}", ex);
                                    MTSExceptionHandler.HandleException(ref newEx);
                                }
                            }
                        }
                        catch (IOException ex)
                        {
                            // throw ex;
                        }

                        if (!imageError)
                        {
                            _importStagings.Path = lckpath;
                            _importStagings.LoanPicked = false;
                            dataAccess.InsertImportStaging(_importStagings);
                        }
                    }
                    else
                    {
                        File.Move(_xmlFile, notILLoanXML);
                    }
                }
                catch (Exception ex)
                {
                    if (File.Exists(lckpath))
                        File.Move(lckpath, errorPath);

                    if (File.Exists(_xmlFile))
                        File.Move(_xmlFile, errorPath);

                    Exception newEx = new Exception($"XML File : {_xmlFile}", ex);
                    MTSExceptionHandler.HandleException(ref newEx);
                    throw newEx;
                }

            }
            return true;
        }

        private bool IsValidXml(string xml)
        {
            try
            {
                XDocument.Parse(xml);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private XPathNavigator GetXMLNavigator(string inputXML)
        {
            XPathNavigator inputXmlNavigator = null;
            string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            XmlDocument inputXmlDocument = new XmlDocument();
            inputXmlDocument.LoadXml(inputXML);
            inputXmlNavigator = inputXmlDocument.CreateNavigator();
            return inputXmlNavigator;
        }
    }
}
