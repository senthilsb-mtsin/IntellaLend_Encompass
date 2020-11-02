using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanPDFGenerator
{
    public class LoanDataAccess
    {
        #region Private Variables

        private static string TenantSchema;

        #endregion

        #region Constructor

        public LoanDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion


        public Loan GetLoandetails(Int64 loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                var loanList = db.Loan.AsNoTracking().Where(m => m.LoanID == loanID).ToList();
                if (loanList.Count > 0)
                    return loanList[0];
            }

            return null;
        }

        public AuditLoanDetail GetLoanObject(Int64 loanId)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                var loanList = db.AuditLoanDetail.AsNoTracking().Where(m => m.LoanID == loanId).OrderBy(m => m.AuditID).ToList();
                if (loanList.Count > 0)
                    return loanList[0];
            }

            return null;
        }
        

        public List<LoanCustomImage> GetLoanImages(Int64 loanId, Int64 documentTypeId)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                var imageList = db.LoanImage.AsNoTracking().Where(m => (m.LoanID == loanId) && (m.DocumentTypeID == documentTypeId)).ToList();
                List<LoanCustomImage>  imageCustList = (from i in imageList
                             select new LoanCustomImage
                             {
                                 CreatedOn = i.CreatedOn,
                                 DocumentTypeID = i.DocumentTypeID,
                                // Image = i.Image,
                                 LoanID = i.LoanID,
                                 LoanImageID = i.LoanImageID,
                                 ModifiedOn = i.ModifiedOn,
                                 PageNo = i.PageNo,
                                 Version = Convert.ToInt64(i.Version)
                             }).OrderByDescending(x => x.Version).ThenBy(i => i.PageNo).ToList();

                //.OrderByDescending(x => x.Version).ThenBy(i => i.PageNo).ToList();
                // var maxItem = imageList.OrderByDescending(x => Convert.ToInt32(x.Version)).FirstOrDefault();
                return imageCustList;
            }
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
            using (var db = new DBConnect("t1"))
            {
                return db.StackingOrderDetailMaster.AsNoTracking().Where(m => m.StackingOrderID == stackingOrderId).OrderBy(m => m.SequenceID).ToList();
            }
        }

        public LoanPDF GetExistingLoanPdfDetails(Int64 loanId)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanPDF loanPdf = db.LoanPDF.AsNoTracking().Where(l => l.LoanID == loanId).FirstOrDefault();
                return loanPdf;
            }
        }

        public void InsertLoanPdf(Int64 loanId, string pdfpath)
        {
            using (var db = new DBConnect(TenantSchema))
            {

                LoanPDF loanPdf = db.LoanPDF.AsNoTracking().Where(l => l.LoanID == loanId).FirstOrDefault();

                if (loanPdf != null)
                {
                    if (File.Exists(loanPdf.LoanPDFPath))
                        File.Delete(loanPdf.LoanPDFPath);

                    loanPdf.LoanPDFPath = pdfpath;
                    loanPdf.ModifiedOn = DateTime.Now;
                    db.Entry(loanPdf).State = EntityState.Modified;
                }
                else
                {
                    db.LoanPDF.Add(new LoanPDF() { LoanID = loanId, LoanPDFPath = pdfpath, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now });
                }

                db.SaveChanges();
            }
        }
    }

    public class LoanCustomImage
    {
        public Int64 LoanImageID { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 DocumentTypeID { get; set; }
        public Int64 PageNo { get; set; }
        public byte[] Image { get; set; }
        public Int64 Version { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
