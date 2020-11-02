using MTSEntBlocks.DataBlock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetryExceptionLoanImages
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

        public List<Int64> GetLoanImageID()
        {
            List<Int64> _imgIDs = new List<long>();

            System.Data.DataTable dt = dataAccess.GetDataTable("GETEXCEPTIONIMAGELIST");

            if (dt.Rows.Count > 0) {
                foreach (DataRow dr in dt.Rows)
                {
                    _imgIDs.Add(Convert.ToInt64(dr["LoanImageID"]));
                }
            }

            return _imgIDs;
        }

        public ImageObj GetLoanImageDetail(Int64 _ImgID)
        {
            System.Data.DataTable dt = dataAccess.GetDataTable("GETLOANIMAGEDETAILS", _ImgID);

            ImageObj _objIm = null;

            if (dt.Rows.Count > 0)
            {
                _objIm = new ImageObj();
                _objIm.LoanID = Convert.ToInt64(dt.Rows[0]["LoanID"]);
                _objIm.ImageID = _ImgID;
                _objIm.ImageBytes = (byte[])(dt.Rows[0]["Image"]);
            }

            return _objIm;
        }

        public void UpdateLoanImageGUID(Int64 _imgID, Guid _imgGUID)
        {
            dataAccess.ExecuteNonQuery("UPDATELOANIMAGEGUID", _imgID, _imgGUID);            
        }
        #endregion
    }
}
