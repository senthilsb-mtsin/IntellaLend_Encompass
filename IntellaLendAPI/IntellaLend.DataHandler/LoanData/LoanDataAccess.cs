using IntellaLend.Audit;
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
    public class LoanDataAccess
    {
        private string TenantSchema;

        #region Constructor

        public LoanDataAccess()
        { }

        public LoanDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion
        
        #region Public Methods

        public object GetLoans(DateTime FromDate, DateTime ToDate, Int64 CurrentUserID)
        {
            //List<LoanSearch> loans = null;
            object loan;
            using (var db = new DBConnect(TenantSchema))
            {
                //loans = db.LoanSearch
                //    .Include(s => s.LoanTypeMaster)
                //    .Include(s => s.WorkFlowStatusMaster)
                //    .Where(l => (l.ReceivedDate >= FromDate && l.ReceivedDate < ToDate.AddDays(1))).ToList();

                ToDate = ToDate.AddDays(1);

                var loans = (from search in db.LoanSearch
                             join L in db.Loan on search.LoanID equals L.LoanID
                             join LTM in db.LoanTypeMaster on search.LoanTypeID equals LTM.LoanTypeID
                             where search.ReceivedDate >= FromDate && search.ReceivedDate < ToDate
                             select new
                             {
                                 LoanID = search.LoanID,
                                 LoanNumber = search.LoanNumber,
                                 LoanTypeID = search.LoanTypeID,
                                 ReceivedDate = search.ReceivedDate,
                                 Status = search.Status,
                                 LoanAmount = search.LoanAmount,
                                 LoanTypeName = LTM.LoanTypeName,
                                 BorrowerName = search.BorrowerName,
                                 StatusDescription = "",
                                 LoggedUserID = L.LoggedUserID
                             }).ToList();


                List<WorkFlowStatusMaster> wfMaster = new IntellaLendDataAccess().GetWorkFlowMaster();

                loans = (from l in loans.AsEnumerable()
                         join wm in wfMaster on l.Status equals wm.StatusID
                         select new
                         {
                             LoanID = l.LoanID,
                             LoanNumber = l.LoanNumber,
                             LoanTypeID = l.LoanTypeID,
                             ReceivedDate = l.ReceivedDate,
                             Status = l.Status,
                             LoanAmount = l.LoanAmount,
                             LoanTypeName = l.LoanTypeName,
                             BorrowerName = l.BorrowerName,
                             StatusDescription = wm.StatusDescription,
                             LoggedUserID = l.LoggedUserID
                         }).ToList();

                loan = (from l in loans.AsEnumerable()
                        join u in db.Users on l.LoggedUserID equals u.UserID into lu
                        from ul in lu.DefaultIfEmpty()
                        select new
                        {
                            LoanID = l.LoanID,
                            LoanNumber = l.LoanNumber,
                            LoanTypeID = l.LoanTypeID,
                            ReceivedDate = l.ReceivedDate,
                            Status = l.Status,
                            LoanAmount = l.LoanAmount,
                            LoanTypeName = l.LoanTypeName,
                            BorrowerName = l.BorrowerName,
                            StatusDescription = l.StatusDescription,
                            LoggedUserID = l.LoggedUserID,
                            LoggerUserFirstName = ul?.FirstName ?? String.Empty,
                            LoggerUserLastName = ul?.LastName ?? String.Empty,
                            CurrentUserID = CurrentUserID
                        }).ToList();


            }

            return loan;
        }

        public bool UpdateLoanDocument(Int64 LoanID, Int64 DocumentID, Int64 CurrentUserID, List<DocumentLevelFields> DocumentFields)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    Loan loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
                    LoanDetail loanDetail = db.LoanDetail.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                    if (loanDetail != null)
                    {
                        Batch loanBatch = JsonConvert.DeserializeObject<Batch>(loanDetail.LoanObject);

                        bool _docUpdated = false;

                        foreach (Documents doc in loanBatch.Documents.ToArray())
                        {
                            if (doc.DocumentTypeID.Equals(DocumentID))
                            {
                                doc.DocumentLevelFields = DocumentFields;

                                loanDetail.LoanObject = JsonConvert.SerializeObject(loanBatch);

                                db.Entry(loanDetail).State = EntityState.Modified;
                                db.SaveChanges();

                                User user = db.Users.AsNoTracking().Where(u => u.UserID == CurrentUserID).FirstOrDefault();

                                string UserName = string.Empty;

                                if (user != null)
                                    UserName = string.Format("{0} {1}", user.LastName, user.FirstName);

                                LoanAudit.InsertLoanDetailsAudit(db, loanDetail, CurrentUserID, string.Format("Loan Details Updated by {0}", UserName));

                                _docUpdated = true;

                                break;
                            }
                        }
                        tran.Commit();
                        return _docUpdated;
                    }
                    return false;
                }
            }
        }

        public void SetLoanPickUpUser(Int64 LoanID, Int64 PickUpUserID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    Loan loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                    if (loan != null)
                    {
                        loan.LoggedUserID = PickUpUserID;
                        loan.LastAccessedUserID = PickUpUserID;
                        loan.ModifiedOn = DateTime.Now;
                        db.Entry(loan).State = EntityState.Modified;
                        db.SaveChanges();

                        var user = db.Users.AsNoTracking().Where(U => U.UserID == PickUpUserID).FirstOrDefault();

                        LoanAudit.InsertLoanAudit(db, loan, string.Format("Loan picked by the user : {0} {1}", user.LastName, user.FirstName));
                    }

                    tran.Commit();
                }
            }
        }

        public bool RemoveLoanLoggedUser(Int64 LoanID)
        {
            bool result = false;
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    Loan loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                    if (loan != null)
                    {
                        loan.LoggedUserID = 0;
                        loan.ModifiedOn = DateTime.Now;
                        db.Entry(loan).State = EntityState.Modified;
                        db.SaveChanges();

                        result = true;                       
                    }

                    tran.Commit();
                }
            }

            return result;
        }

        public object CheckCurrentLoanUser(Int64 LoanID, Int64 CurrentUserID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan loan = db.Loan.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();

                if ((loan.LoggedUserID == CurrentUserID) || (loan.LoggedUserID == 0))
                {
                    return new { CurrentUser = true };
                }
                else
                {
                    var loggerUser = db.Users.Where(u => u.UserID == loan.LoggedUserID).AsNoTracking().FirstOrDefault();

                    return new { CurrentUser = false, LoggerUserName = string.Format("{0} {1}", loggerUser.LastName, loggerUser.FirstName) };
                }
            }
            return null;
        }

        public bool CheckLoanPDFExists(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanPDF loanPdf = db.LoanPDF.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                if (loanPdf != null)
                    return !string.IsNullOrEmpty(loanPdf.LoanPDFPath);
            }
            return false;
        }

        public string GetLoanPDF(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanPDF loanPdf = db.LoanPDF.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();

                if (loanPdf != null)
                    return string.IsNullOrEmpty(loanPdf.LoanPDFPath) ? string.Empty : loanPdf.LoanPDFPath;
                
            }
            return string.Empty;
        }

        //this method created by Manikandan
        public string GetLoanNotes(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan loanNotes = db.Loan.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();

                if (loanNotes != null)
                    return string.IsNullOrEmpty(loanNotes.Notes) ? string.Empty : loanNotes.Notes;

            }
            return string.Empty;
        }

        
        public bool UpdateLoanNotes(Int64 LoanID,string LoanNotes)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    Loan dbObject = db.Loan.Where(l => l.LoanID == LoanID).First();
                    dbObject.Notes = LoanNotes;
                    db.Entry(dbObject).State = EntityState.Modified;
                    db.SaveChanges();
                    tran.Commit();
                    return true;
                }
            }
        }

        public Loan GetLoanHeaderDeatils(Int64 LoanID)
        {
            Loan loan = null;
            using (var db = new DBConnect(TenantSchema))
            {
                loan = new Loan();
                loan = db.Loan.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();
            }
            return loan;
        }

        public object GetLoanDocInfo(Int64 LoanID, Int64 DocumentID)
        {
            object docFields = null;
            using (var db = new DBConnect(TenantSchema))
            {
                Int64 _loanImageCount = 0;
                Int64 _docFirstImgID = 0;
                Int64 _docSecondImgID = 0;
                List<DocumentLevelFields> _docLevelFields = new List<DocumentLevelFields>();

                LoanDetail _loanDetail = db.LoanDetail.AsNoTracking().Where(ld => ld.LoanID == LoanID).FirstOrDefault();
                List<LoanImage> _loanImages = db.LoanImage.AsNoTracking().Where(ld => ld.LoanID == LoanID && ld.DocumentTypeID == DocumentID).OrderBy(im => im.PageNo).ToList();
                if (_loanImages != null)
                {
                    _loanImageCount = _loanImages.Count();
                    LoanImage lImg = _loanImages.OrderBy(im => im.PageNo).FirstOrDefault();
                    _docFirstImgID = lImg.LoanImageID;
                    if (_loanImageCount > 1)
                    {
                        int nxtIndex = _loanImages.IndexOf(lImg) + 1;
                        lImg = _loanImages[nxtIndex];
                        _docSecondImgID = lImg.LoanImageID;
                    }
                }
                if (_loanDetail != null)
                {
                    Batch loanBatch = JsonConvert.DeserializeObject<Batch>(_loanDetail.LoanObject);
                    _docLevelFields = loanBatch.Documents.Where(d => d.DocumentTypeID == DocumentID).FirstOrDefault().DocumentLevelFields;

                    if (_docLevelFields != null)
                    {
                        foreach (DocumentLevelFields _field in _docLevelFields)
                        {
                            string _fieldDisplayName = GetFieldDisplayName(_field.FieldID);
                            _field.FieldDisplayName = _fieldDisplayName.Equals(string.Empty) ? _field.Name : _fieldDisplayName;
                        }
                    }
                }

                docFields = new { DocLevelFields = _docLevelFields.ToArray(), ImageCount = _loanImageCount, CurrentPage = 1, ImageID = _docFirstImgID, NextImageID = _docSecondImgID };
            }
            return docFields;
        }

        public object GetImageByID(Int64 ImageID)
        {
            object _img = null;
            using (var db = new DBConnect(TenantSchema))
            {
                string bs64Image = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(db.LoanImage.AsNoTracking().Where(i => i.LoanImageID == ImageID).FirstOrDefault().Image));

                if (!string.IsNullOrEmpty(bs64Image))
                    _img = new { Image = bs64Image };
            }
            return _img;
        }

        public object GetImageByID(Int64 LoanID, Int64 DocumentID, Int64 ImageID)
        {
            object docFields = null;
            using (var db = new DBConnect(TenantSchema))
            {
                Int64 _loanImageCount = 0;
                Int64 _docPreImgID = 0;
                Int64 _docNextImgID = 0;
                string bs64Image = String.Empty;
                List<LoanImage> _loanImages = db.LoanImage.AsNoTracking().Where(ld => ld.LoanID == LoanID && ld.DocumentTypeID == DocumentID).OrderBy(im => im.PageNo).ToList();
                if (_loanImages != null)
                {
                    _loanImageCount = _loanImages.Count();
                    LoanImage lImg = _loanImages.Where(i => i.LoanImageID == ImageID).FirstOrDefault();
                    bs64Image = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(lImg.Image));

                    if (_loanImageCount > 1)
                    {
                        int nxtIndex = _loanImages.IndexOf(lImg) + 1;
                        lImg = _loanImages[nxtIndex];
                        _docNextImgID = lImg.LoanImageID;
                        int preIndex = _loanImages.IndexOf(lImg) - 2;
                        lImg = _loanImages[preIndex];
                        _docPreImgID = lImg.LoanImageID;
                    }
                }
                if (!string.IsNullOrEmpty(bs64Image))
                    docFields = new { Image = bs64Image, PreImageID = _docPreImgID, NextImageID = _docNextImgID, CurrentImageID = ImageID };
            }
            return docFields;
        }

        #endregion

        #region Private Methods

        private string GetFieldDisplayName(Int64 FieldID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                DocumentFieldMaster docField = db.DocumentFieldMaster.AsNoTracking().Where(f => f.FieldID == FieldID).FirstOrDefault();

                if (docField != null)
                    return docField.DisplayName;
            }

            return string.Empty;
        }

        #endregion

    }
}
