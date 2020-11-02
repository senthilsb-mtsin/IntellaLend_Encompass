using IntellaLend.Audit;
using IntellaLend.Constance;
using IntellaLend.MinIOWrapper;
using IntellaLend.Model;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.UtilsBlock;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveImageAndPDFToMinIO
{
    public class MoveToMinIODataAccess
    {
        private static string TenantSchema = "T1";

        public Loan GetLoan(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
            }
        }

        public AuditLoanDetail GetLoanDetail(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                var loanList = db.AuditLoanDetail.AsNoTracking().Where(m => m.LoanID == LoanID).OrderBy(m => m.AuditID).ToList();
                if (loanList.Count > 0)
                    return loanList[0];
            }
            return null;
        }

        public Int64 InsertImagesToDB(Batch batchObj, string ImageMaxHeight, string ImageMaxWidth)
        {

            ImageUtilities imgUtil = new ImageUtilities();

            Int64 _pageCount = 0;
            foreach (var doc in batchObj.Documents)
            {
                string DocType = doc.Type;
                try
                {
                    byte[] imageBytes = File.ReadAllBytes(doc.MultiPageTiffFile);
                    Int64 pageCount = imgUtil.GetByteDataPageCount(imageBytes, "image/tiff");
                    Log($"PageCount : {pageCount}");
                    Int32 _maxWidth = 1654;
                    Int32 _maxHeight = 2339;
                    Int32.TryParse(ImageMaxWidth, out _maxWidth);
                    Int32.TryParse(ImageMaxHeight, out _maxHeight);
                    for (int i = 1; i <= pageCount; i++)
                    {
                        Log($"PageCount : {pageCount}");
                        _pageCount = _pageCount + 1;
                        var img = imgUtil.ConvertTiffToJpeg(imageBytes, i, _maxWidth, _maxHeight);
                        InsertDocumentImages(batchObj.LoanID, doc.DocumentTypeID, (i - 1), img.Image, doc.VersionNumber);

                        UpdateFieldCoordinates(doc, img.OrginalImageWidth, img.OrginalImageWidth, img.CompressedImageWidth, img.CompressedImageHeight, (i - 1).ToString());

                    }
                }
                catch (Exception ex)
                {
                    MTSExceptionHandler.HandleException(ref ex);
                }
                
            }
            return _pageCount;
        }

        public void saveChange(Loan _loan)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                db.Entry(_loan).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        private void UpdateFieldCoordinates(Documents doc, int orgWidth, int orgHeight, int newWidth, int newHeight, string pageNo)
        {
            var zoomVal = (newWidth * 1f / orgWidth) * 100;
            var curFields = doc.DocumentLevelFields.Where(p => p.PageNo == pageNo);
            if (curFields != null)
            {
                foreach (var field in curFields)
                {
                    field.CoordinatesList.x0 = Convert.ToInt32((field.CoordinatesList.x0 * zoomVal) / 100);
                    field.CoordinatesList.x1 = Convert.ToInt32((field.CoordinatesList.x1 * zoomVal) / 100);
                    field.CoordinatesList.y0 = Convert.ToInt32((field.CoordinatesList.y0 * zoomVal) / 100);
                    field.CoordinatesList.y1 = Convert.ToInt32((field.CoordinatesList.y1 * zoomVal) / 100);
                }
            }
        }

        public void InsertDocumentImages(Int64 loanID, Int64 documentTypeID, int pageNo, byte[] imageBytes, int version)
        {
            ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);


            LoanImage _loanImage = new LoanImage()
            {
                LoanID = loanID,
                DocumentTypeID = documentTypeID,
                PageNo = pageNo,
                // Image = new byte[0],
                Version = version.ToString(),
                CreatedOn = DateTime.Now
            };

            _imageWrapper.InsertLoanImage(loanID, imageBytes, _loanImage);
        }

        public Int64 GetStackingOrderId(Int64 customerId, Int64 reviewTypeId, Int64 loanTypeId)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                var stackingorderList = db.CustReviewLoanStackMapping.AsNoTracking().Where(m => (m.CustomerID == customerId) && (m.ReviewTypeID == reviewTypeId) && (m.LoanTypeID == loanTypeId) && (m.Active == true)).ToList();
                if (stackingorderList.Count > 0)
                    return stackingorderList[0].StackingOrderID;
            }
            return 0;
        }

        public List<StackingOrderDetailMaster> GetStackingOrderInfo(Int64 stackingOrderId)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.StackingOrderDetailMaster.AsNoTracking().Where(m => m.StackingOrderID == stackingOrderId).OrderBy(m => m.SequenceID).ToList();
            }
        }

        private void GetPageNumberOrder(List<Documents> _listDocs, bool isMissingDocument, ref int pageSequence, ref List<Int32> pageNoList)
        {

            foreach (Documents doc in _listDocs)
            {
                for (int i = 0; i < doc.Pages.Count; i++)
                {
                    int pno = 0;

                    if (isMissingDocument && doc.Pages[i].ToUpper().StartsWith("PG"))
                        continue;

                    if (doc.Pages[i].ToUpper().StartsWith("PG"))
                    {
                        pno = ExtractPageNo(doc.Pages[i]);
                    }
                    else
                    {
                        pno = Convert.ToInt32(doc.Pages[i]);
                    }

                    if (!pageNoList.Contains(pno + 1))
                    {
                        pageNoList.Add(pno + 1);
                    }
                    //Update new Sequence Page no
                    doc.Pages[i] = pageSequence.ToString();
                    pageSequence++;
                }
            }
        }

        private int ExtractPageNo(string spageNo)
        {
            string extPattern = @"PG(?<PageNO>\d+)";
            int pageNo = 0;
            Int32.TryParse(CommonUtils.ExtractDataFromString(spageNo, extPattern)["PageNO"], out pageNo);
            return pageNo;
        }

        public bool GetIncLoantypeDocs()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                CustomerConfig cust = db.CustomerConfig.AsNoTracking().Where(m => m.ConfigKey == CustomerConfiguration.INCLUDE_LOANTYPE_DOCUMENTS && m.CustomerID == 0).FirstOrDefault();
                if (cust != null)
                {
                    return Convert.ToBoolean(cust.ConfigValue);
                }
                else { return false; }

            }
        }

        public Boolean CheckCustomerConfigKey()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                var customerconfigkeyList = db.CustomerConfig.AsNoTracking().Where(m => (m.CustomerID == 0) && (m.ConfigKey == "Delete_Loan_Source") && (m.ConfigValue.ToLower().Equals("true")) && (m.Active)).ToList();
                if (customerconfigkeyList.Count > 0)
                    return true;
            }
            return false;
        }

        public void GenerateLoanPdfByStackingOrder(Batch batchObj, string existingPDF)
        {

            List<int> pageNoList = new List<int>();

            Loan loan = GetLoan(batchObj.LoanID);

            if (loan != null)
            {
                if (!string.IsNullOrEmpty(existingPDF))
                {
                    //Get Stacking Order
                    int pageSequence = 0;
                    Int64 stackingOrderId = GetStackingOrderId(loan.CustomerID, loan.ReviewTypeID, loan.LoanTypeID);
                    List<StackingOrderDetailMaster> stackingOrderDetailList = GetStackingOrderInfo(stackingOrderId).OrderBy(sd => sd.SequenceID).ToList();
                    foreach (StackingOrderDetailMaster item in stackingOrderDetailList)
                    {
                        //List<LoanImage> loanImageDetails = dataAccess.GetLoanImages(batchObj.LoanID, item.DocumentTypeID);
                        var docList = batchObj.Documents.Where(X => X.DocumentTypeID == item.DocumentTypeID).OrderBy(sd => sd.VersionNumber).ToList();
                        GetPageNumberOrder(docList, false, ref pageSequence, ref pageNoList);

                    }

                    //
                    bool incAllDocs = GetIncLoantypeDocs();
                    if (incAllDocs)
                    {
                        List<Documents> listDoc = batchObj.Documents.Where(l => !stackingOrderDetailList.Any(ls => ls.DocumentTypeID == l.DocumentTypeID)).OrderBy(d => d.DocumentTypeID).ThenByDescending(d => d.VersionNumber).ToList();
                        GetPageNumberOrder(listDoc, false, ref pageSequence, ref pageNoList);
                    }
                    var isDeletOrgFile = CheckCustomerConfigKey();

                    Dictionary<Int32, string> pgLevelLS = new Dictionary<Int32, string>();
                    foreach (Documents doc in batchObj.Documents)
                    {
                        if (doc.PageLevelFields != null)
                        {
                            List<PageLevelFields> pgLs = doc.PageLevelFields.Where(p => p.IsRotated == true).ToList();

                            foreach (PageLevelFields pg in pgLs)
                                pgLevelLS[Convert.ToInt32(pg.PageNo)] = pg.Direction.ToUpper();

                        }
                    }

                    byte[] _pdfBytes = new byte[0];

                    if (pageNoList.Count > 0)
                    {
                        byte[] _oldPDFBytes = File.ReadAllBytes(existingPDF);

                        _pdfBytes = CommonUtils.ReOrderPDFPages(_oldPDFBytes, string.Empty, pageNoList.ToArray(), pgLevelLS);

                        new ImageMinIOWrapper(batchObj.Schema).InsertLoanPDF(batchObj.LoanID, _pdfBytes);

                        InsertLoanPdf(batchObj, string.Empty);
                    }
                }
            }
        }

        public void InsertLoanPdf(Batch batchObj, string pdfpath)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    if (batchObj != null)
                    {
                        LoanDetail lDetails = db.LoanDetail.AsNoTracking().Where(ld => ld.LoanID == batchObj.LoanID).FirstOrDefault();

                        if (lDetails != null)
                        {
                            lDetails.LoanObject = JsonConvert.SerializeObject(batchObj);
                            lDetails.ModifiedOn = DateTime.Now;
                            db.Entry(lDetails).State = EntityState.Modified;
                            db.SaveChanges();

                            LoanAudit.InsertLoanDetailsAudit(db, lDetails, 0, "Created Loan PDF by Stacking Order and Updated Loan Object Page No", "");
                        }
                    }

                    trans.Commit();
                }
            }
        }

        public void Log(string _msg)
        {
            Exception ex = new Exception(_msg);
            MTSExceptionHandler.HandleException(ref ex);
        }

    }
}
