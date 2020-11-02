using MTSEntBlocks.DataBlock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationMoveExceptionPDF
{
    public class LoanMigrationDataAccess
    {
        // private static string TenantSchema = "T1";
        private DataAccess2 dataAccess;


        public LoanMigrationDataAccess()
        {
            dataAccess = new DataAccess2("AppConnectionName");
        }

        #region GET

        public List<PDFObj> GetLoanPDFDeatils()
        {
            List<PDFObj> _imgIDs = _imgIDs = new List<PDFObj>(); 

            System.Data.DataTable dt = dataAccess.GetDataTable("GETEXCEPTIONPDFLIST");

            if (dt.Rows.Count > 0) {
              
                foreach (DataRow dr in dt.Rows)
                {
                    PDFObj _pdfObj = new PDFObj()
                    {
                        LoanPDFID = Convert.ToInt64(dr["LoanPDFID"]),
                        LoanID = Convert.ToInt64(dr["LoanID"]),
                        LoanGUID = Guid.Parse(dr["LoanGUID"].ToString()),
                        LoanPDFPath = Convert.ToString(dr["LoanPDFPath"])
                    };
                    _imgIDs.Add(_pdfObj);
                }
            }

            return _imgIDs;
        }
        

        public void UpdateLoanPDFGUID(Int64 _loanPDFID, Guid _pdfGUID)
        {
            dataAccess.ExecuteNonQuery("UPDATELOANPDFGUID", _loanPDFID, _pdfGUID);            
        }
        #endregion
    }
}
