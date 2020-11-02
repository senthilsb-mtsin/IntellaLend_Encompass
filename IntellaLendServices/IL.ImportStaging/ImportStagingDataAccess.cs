using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IL.ImportStaging
{
    public class ImportStagingDataAccess
    {
        private static string TenantSchema;
        private static string SystemSchema = "IL";

        public ImportStagingDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        public static List<TenantMaster> GetAllTenants()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.TenantMaster.Where(m => m.Active == true).ToList();
            }
        }

        public ImportStagings GetLoanDetails(string _batchFolderName)
        {
            ImportStagings _importStage = null;
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = (from idc in db.AuditIDCFields.AsNoTracking()
                            join l in db.Loan.AsNoTracking() on idc.LoanID equals l.LoanID
                            where idc.IDCBatchInstanceID == _batchFolderName
                            select l).FirstOrDefault();

                if (_loan == null)
                {
                    _loan = (from idc in db.AuditLoanMissingDoc.AsNoTracking()
                                       join l in db.Loan.AsNoTracking() on idc.LoanID equals l.LoanID
                                       where idc.IDCBatchInstanceID == _batchFolderName
                                       select l).FirstOrDefault();
                }

                if (_loan != null)
                {
                    _importStage = new ImportStagings()
                    {
                        LoanId = _loan.LoanID,
                        ReviewTypeID = _loan.ReviewTypeID,
                        LoanCreatedDate = _loan.CreatedOn,
                        Status = ImportStagingConstant.Staged,
                        TenantSchema = TenantSchema,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };



                    ReviewTypeMaster _reviewTypeMaster = db.ReviewTypeMaster.AsNoTracking().Where(l => l.ReviewTypeID == _loan.ReviewTypeID).FirstOrDefault();

                    //Prakash : Default 4 (Lowest priority)
                    _importStage.Priority = _reviewTypeMaster != null ? _reviewTypeMaster.ReviewTypePriority : 4;

                    if (_loan.UploadType == 1)  // if (_loan.FromBox)
                    {
                        BoxDownloadQueue _boxDownloadQueue = db.BoxDownloadQueue.AsNoTracking().Where(l => l.LoanID == _importStage.LoanId).FirstOrDefault();
                        
                        _importStage.Priority = _boxDownloadQueue != null ? _boxDownloadQueue.Priority : _importStage.Priority;
                    }

                }
            }

            return _importStage;
        }

        public void InsertImportStaging(ImportStagings _importStaging)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                db.ImportStaging.Add(_importStaging);
                db.SaveChanges();
            }
        }


        
    }

}
