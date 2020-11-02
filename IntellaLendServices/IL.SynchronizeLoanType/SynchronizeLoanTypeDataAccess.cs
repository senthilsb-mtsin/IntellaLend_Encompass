using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace IL.SynchronizeLoanType
{
    public class SynchronizeLoanTypeDataAccess
    {
        #region Private Variables

        private static string TenantSchema;
        private static string SystemSchema = "IL";

        #endregion

        #region Constructor

        public SynchronizeLoanTypeDataAccess(string _tenantSchema)
        {
            TenantSchema = _tenantSchema;
        }

        #endregion


        #region Public Methods

        public static List<TenantMaster> GetTenantList()
        {
            try
            {
                using (var db = new DBConnect(SystemSchema))
                {
                    return db.TenantMaster.AsNoTracking().Where(m => m.Active == true).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RetainUpdateStaging> GetStagingLoanTypeSync()
        {
            List<RetainUpdateStaging> _lsRetainUpdateStaging = new List<RetainUpdateStaging>();

            using (var db = new DBConnect(TenantSchema))
            {
                _lsRetainUpdateStaging = db.RetainUpdateStaging.AsNoTracking().Where(r => r.Synchronized == SynchronizeConstant.Staged).ToList();
            }
            return _lsRetainUpdateStaging;
        }

        public void UpdateStagingLoanTypeSync(Int64 RetainUpdateID, int SyncStatus)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                RetainUpdateStaging _ruStaging = db.RetainUpdateStaging.AsNoTracking().Where(r => r.ID == RetainUpdateID).FirstOrDefault();
                if (_ruStaging != null)
                {
                    _ruStaging.Synchronized = SyncStatus;
                    _ruStaging.ModifiedOn = DateTime.Now;
                    db.Entry(_ruStaging).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public void UpdateStagingDetailLoanTypeSync(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID, int SyncStatus)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                RetainUpdateStagingDetails _rDetail = db.RetainUpdateStagingDetails.AsNoTracking().Where(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == LoanTypeID).FirstOrDefault();

                if (_rDetail != null)
                {
                    _rDetail.Synchronized = SyncStatus;
                    _rDetail.ModifiedOn = DateTime.Now;
                    db.Entry(_rDetail).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public void SynchDealType(string Schema, Int64 StagingID, Int64 LoanTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<object> parameterList = new List<object>();

                parameterList.Add(new SqlParameter("@StagingID", StagingID));
                parameterList.Add(new SqlParameter("@LoanTypeID", LoanTypeID));
                db.Database.ExecuteSqlCommand($"EXEC {Schema}.SYNCDEALTYPE @StagingID,@LoanTypeID", parameterList.ToArray());
            }
        }

        public void UpdateStagingLoanTypeSync(Int64 RetainUpdateID, int SyncStatus, List<CustReviewLoanMapping> _lsCustReviewLoanMapping)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                RetainUpdateStaging _ruStaging = db.RetainUpdateStaging.AsNoTracking().Where(r => r.ID == RetainUpdateID).FirstOrDefault();
                if (_ruStaging != null)
                {
                    _ruStaging.Synchronized = SyncStatus;
                    _ruStaging.ModifiedOn = DateTime.Now;
                    db.Entry(_ruStaging).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    db.RetainUpdateStagingDetails.RemoveRange(db.RetainUpdateStagingDetails.Where(r => r.LoanTypeID == _ruStaging.LoanTypeID));
                    db.SaveChanges();

                    foreach (CustReviewLoanMapping item in _lsCustReviewLoanMapping)
                    {
                        db.RetainUpdateStagingDetails.Add(new RetainUpdateStagingDetails
                        {
                            CustomerID = item.CustomerID,
                            ReviewTypeID = item.ReviewTypeID,
                            LoanTypeID = item.LoanTypeID,
                            Synchronized = SynchronizeConstant.Staged,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        });
                        db.SaveChanges();
                    }

                }
            }
        }


        public List<CustReviewLoanMapping> GetCustReviewLoanMapping(Int64 LoanTypeID)
        {
            List<CustReviewLoanMapping> _lsCustReviewLoanMapping = new List<CustReviewLoanMapping>();

            using (var db = new DBConnect(TenantSchema))
            {
                _lsCustReviewLoanMapping = db.CustReviewLoanMapping.AsNoTracking().Where(r => r.LoanTypeID == LoanTypeID).ToList();
            }
            return _lsCustReviewLoanMapping;
        }

        public CustomerConfig GetStagingOrderConfigForCustomer(Int64 _customerID)
        {
            CustomerConfig _CustomerConfig = new CustomerConfig();

            using (var db = new DBConnect(TenantSchema))
            {
                _CustomerConfig = db.CustomerConfig.AsNoTracking().Where(c => c.CustomerID == _customerID && c.ConfigKey == "Stacking_Order_Config").FirstOrDefault();
            }
            return _CustomerConfig;
        }
        #endregion
    }
}