using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntBlocks.LoggerBlock;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IL.LOSExceptionExport
{
    public class LOSExceptionExportDataAccess
    {
        public static string SystemSchema = "IL";
        public static string TenantSchema;

        public LOSExceptionExportDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        public static List<TenantMaster> GetTenantList()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.TenantMaster.AsNoTracking().Where(m => m.Active == true).ToList();
            }
        }

        public List<LOSExportFileStaging> GetLOSLoanStagingFiles()
        {
            List<LOSExportFileStaging> fileStagings = null;
            using (var db = new DBConnect(TenantSchema))
            {
                fileStagings = db.LOSExportFileStaging.AsNoTracking().Where(fs => fs.Status == LOSExportStatusConstant.LOS_LOAN_STAGED && fs.FileType != LOSExportFileTypeConstant.LOS_LOAN_EXPORT).ToList();
            }
            return fileStagings;
        }

        public void UpdateLOSLoanStagingFileStatus(Int64 _fileStagingID, Int32 _status, string _errorMessage = "")
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Logger.WriteTraceLog($"Start UpdateLOSStagingFileStatus() fileStagingID : {_fileStagingID}, status : {_status}, errorMessage : {_errorMessage} ");
                LOSExportFileStaging _stagingFile = db.LOSExportFileStaging.AsNoTracking().Where(fs => fs.ID == _fileStagingID).FirstOrDefault();

                if (_stagingFile != null)
                {
                    _stagingFile.ErrorMsg = _errorMessage;
                    _stagingFile.Status = _status;
                    _stagingFile.ModifiedOn = DateTime.Now;

                    db.Entry(_stagingFile).State = EntityState.Modified;
                    db.SaveChanges();
                }
                Logger.WriteTraceLog($"End UpdateLOSStagingFileStatus() fileStagingID : {_fileStagingID}, status : {_status}, errorMessage : {_errorMessage} ");
            }
        }
    }
}
