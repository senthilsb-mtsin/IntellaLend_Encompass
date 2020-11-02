using IntellaLend.MinIOWrapper;
using IntellaLend.Model;
using MTSEntBlocks.DataBlock;
using MTSEntBlocks.UtilsBlock;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;

namespace IL.EmailTrackers
{
    public class EmailTrackerDataAccess
    {
        private static string TenantSchema;
        private static string SystemSchema = "IL";

        public EmailTrackerDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }
        public static List<TenantMaster> GetTenantList()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                List<TenantMaster> _tenantMs = db.TenantMaster.ToList();

                List<AppConfig> _appConfig = db.AppConfig.ToList();

                List<Int64> _ints = new List<long>();

                foreach (TenantMaster a in _tenantMs)
                {
                    foreach (AppConfig app in _appConfig)
                    {
                        if (a.ID == app.ID)
                            _ints.Add(a.ID);
                    }
                }

                _ints = _tenantMs.Where(a => _appConfig.Any(b => b.ID == a.ID)).Select(t => t.ID).ToList();
                return db.TenantMaster.Where(m => m.Active == true).ToList();
            }
        }
        public List<EmailTracker> GetEmailTrackerPendingData()
        {
            List<EmailTracker> _emailtracker = new List<EmailTracker>();
            using (var db = new DBConnect(TenantSchema))
            {

                _emailtracker = db.EmailTracker.AsNoTracking().Where(x => x.Delivered == 0).OrderByDescending(x => x.CreatedOn).ToList();
            }
            return _emailtracker;
        }
        public Dictionary<string, byte[]> GetEmailAttachment(Int64 LoanID, string Attachements,string AttachmentsName)
        {
            Dictionary<string, byte[]> data = new Dictionary<string, byte[]>();
            string _regexstring = @"\[(.*?)\]";
            var _docname = Attachements.Split(',');
            var _AttachmentName = Regex.Matches(AttachmentsName, _regexstring); 
            using (var db = new DBConnect(TenantSchema))
            {
                    var _loanDetail = db.LoanDetail.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                    if (_loanDetail != null)
                    {
                        Batch loanBatch = JsonConvert.DeserializeObject<Batch>(_loanDetail.LoanObject);
                        for (int i = 0; i < _docname.Count(); i++)
                        {
                            if (_docname[i] != "")
                            {
                                string _DName = string.Empty;
                                string _DVersion = string.Empty;
                                int _indexPos = _docname[i].LastIndexOf('-');
                                _DName = (_indexPos == -1) ? _docname[i] : _docname[i].Substring(0, _indexPos).Trim();
                                _DVersion = (_indexPos == -1) ? "1" : _docname[i].Substring(_indexPos).Replace("V", "").Replace("-", "");
                                Documents documentInfo = new Documents();
                                documentInfo = loanBatch.Documents.Where(d => d.Type == _DName && d.VersionNumber == Convert.ToInt64(_DVersion)).FirstOrDefault();
                                if (documentInfo != null)
                                {
                                    byte[] _imageBytes = GetDocumentPDF(LoanID, documentInfo.DocumentTypeID, Convert.ToString(documentInfo.VersionNumber));
                                    data.Add(_AttachmentName[i].Value.Replace("[", "").Replace("]", ""), _imageBytes);
                                }
                            }

                        }
                        //foreach (Match _name in _docname)
                        //{
                        //    if (_name.Value != "")
                        //    {
                        //        string _DName = string.Empty;
                        //        string _DVersion = string.Empty;
                        //        int _indexPos = _name.Value.Replace("[","").Replace("]","").LastIndexOf('-');
                        //         _DName = (_indexPos == -1) ? _name.Value.Replace("[","").Replace("]","") : _name.Value.Replace("[","").Replace("]","").Substring(0, _indexPos).Trim();
                        //         _DVersion = (_indexPos == -1) ? "1" : _name.Value.Replace("[","").Replace("]","").Substring(_indexPos).Replace("V", "").Replace("-", "");
                        //        Documents documentInfo = new Documents();
                        //        documentInfo = loanBatch.Documents.Where(d => d.Type == _DName && d.VersionNumber == Convert.ToInt64(_DVersion)).FirstOrDefault();
                        //        if (documentInfo != null)
                        //        {
                        //            byte[] _imageBytes = GetDocumentPDF(LoanID, documentInfo.DocumentTypeID, Convert.ToString(documentInfo.VersionNumber));
                        //             data.Add(_name.Value.Replace("[","").Replace("]",""), _imageBytes);
                        //        }
                        //    }
                        //}
                    }
            }
            return data;
        }

        public byte[] GetDocumentPDF(Int64 LoanID, Int64 DocumentID, string VersionNumber)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<LoanImage> _lsImages = db.LoanImage.AsNoTracking().Where(l => l.LoanID == LoanID && l.DocumentTypeID == DocumentID && l.Version == VersionNumber).OrderBy(o => o.PageNo).ToList();

                List<byte[]> _reverifyImg = new List<byte[]>();
                ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);
                foreach (LoanImage doc in _lsImages)
                {
                    byte[] _imageBytes = _imageWrapper.GetLoanImage(doc.LoanID, doc.ImageGUID.GetValueOrDefault());

                     _reverifyImg.Add(_imageBytes);
                }

                return CommonUtils.CreatePdfBytes(_reverifyImg);
            }
        }

        public static DataSet GetSTMPDetails()
        {
            return DataAccess.ExecuteDataset("[IL].GetSTMPDetails", null);
        }
        public static DataSet GetEmailTemplates()
        {
            return DataAccess.ExecuteDataset("[IL].GetEmailTemplate", null);
        }

        public void UpdateEmailStatus(EmailTracker _emailtracker)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                    db.Entry(_emailtracker).State = EntityState.Modified;
                    db.SaveChanges();
            }
        }
    }

}
