using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IL.LenderImport
{
    public class IntellaLendDataAccess
    {
        #region Private Variable

        private static string TenantSchema;

        private static string SystemSchema = "IL";

        #endregion

        #region Constructor

        public IntellaLendDataAccess() { }

        public IntellaLendDataAccess(string tenantSchema)
        {

            TenantSchema = tenantSchema;

        }

        #endregion

        #region Public Methods

        public StackingOrderMaster GetSystemLoanStackingOrder(Int64 LoanTypeID)
        {
            StackingOrderMaster _stackingOrderMaster = null;

            using (var db = new DBConnect(SystemSchema))
            {
                CustReviewLoanStackMapping _StackMapping = db.CustReviewLoanStackMapping.AsNoTracking().Where(r => r.CustomerID == 1 && r.ReviewTypeID == 0 && r.LoanTypeID == LoanTypeID).FirstOrDefault();

                if (_StackMapping != null)
                {
                    _stackingOrderMaster = db.StackingOrderMaster.AsNoTracking().Where(c => c.StackingOrderID == _StackMapping.StackingOrderID).FirstOrDefault();

                    if (_stackingOrderMaster != null)
                    {
                        _stackingOrderMaster.StackingOrderDetailMasters = GetSystemStackingOrderDetails(db, _stackingOrderMaster.StackingOrderID);
                        _stackingOrderMaster.StackingOrderGroupmasters = db.StackingOrderGroupmasters.AsNoTracking().Where(s => s.StackingOrderID == _stackingOrderMaster.StackingOrderID).FirstOrDefault();
                    }
                }
            }

            return _stackingOrderMaster;
        }

        public List<StackingOrderDetailMaster> GetSystemStackingOrderDetails(DBConnect db, Int64 StackingOrderID)
        {
            return db.StackingOrderDetailMaster.AsNoTracking().Where(r => r.StackingOrderID == StackingOrderID).ToList();
        }

        public CheckListMaster GetSystemLoanCheckList(Int64 LoanTypeID)
        {
            CheckListMaster _checkListMaster = null;

            using (var db = new DBConnect(SystemSchema))
            {
                CustReviewLoanCheckMapping _custReviewLoanCheckMapping = db.CustReviewLoanCheckMapping.AsNoTracking().Where(r => r.CustomerID == 1 && r.ReviewTypeID == 0 && r.LoanTypeID == LoanTypeID).FirstOrDefault();

                if (_custReviewLoanCheckMapping != null)
                {
                    _checkListMaster = db.CheckListMaster.AsNoTracking().Where(c => c.CheckListID == _custReviewLoanCheckMapping.CheckListID).FirstOrDefault();

                    if (_checkListMaster != null)
                    {

                        _checkListMaster.CheckListDetailMasters = GetSystemChecklistDetails(db, _checkListMaster.CheckListID);

                        if (_checkListMaster.CheckListDetailMasters != null)
                        {
                            foreach (CheckListDetailMaster item in _checkListMaster.CheckListDetailMasters)
                            {
                                item.RuleMasters = GetSystemCheckRuleMaster(db, item.CheckListDetailID);
                            }
                        }
                    }
                }
            }

            return _checkListMaster;
        }

        public List<CheckListDetailMaster> GetSystemChecklistDetails(DBConnect db, Int64 CheckListID)
        {
            return db.CheckListDetailMaster.AsNoTracking().Where(r => r.CheckListID == CheckListID).ToList();
        }

        public RuleMaster GetSystemCheckRuleMaster(DBConnect db, Int64 ChecklistDetailID)
        {
            return db.RuleMaster.AsNoTracking().Where(r => r.CheckListDetailID == ChecklistDetailID).FirstOrDefault();
        }



        public List<CustReviewLoanReverifyMapping> GetSystemReVerifyMappings(Int64 loanTypeID)
        {
            List<CustReviewLoanReverifyMapping> dm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                dm = new List<CustReviewLoanReverifyMapping>();
                dm = db.CustReviewLoanReverifyMapping.AsNoTracking().Where(r => r.CustomerID == 1 && r.ReviewTypeID == 0 && r.LoanTypeID == loanTypeID).ToList();
            }

            return dm;
        }

        public LoanTypeMaster GetSystemLoanTypes(Int64 loanTypeID)
        {
            LoanTypeMaster lm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                lm = db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeID == loanTypeID && l.Active == true && l.Type == 0).FirstOrDefault();
            }

            return lm;
        }

        public List<CustReverificationDocMapping> GetSystemCustReverifyDocMapping(Int64 ReverifyID)
        {
            List<CustReverificationDocMapping> lm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                lm = db.CustReverificationDocMapping.AsNoTracking().Where(l => l.CustomerID == 1 && l.ReverificationID == ReverifyID).ToList();
            }

            return lm;
        }

        public StackingOrderGroupmasters AddStackingOrderMasterGroup(Int64? StackingOrderGroupId)
        {
            StackingOrderGroupmasters lsStackingOrderGroupMaster = null;
            using (var db = new DBConnect(SystemSchema))
            {

                lsStackingOrderGroupMaster = db.StackingOrderGroupmasters.AsNoTracking().Where(a => a.StackingOrderGroupID == StackingOrderGroupId).FirstOrDefault();
            }
            return lsStackingOrderGroupMaster;
        }

        public List<DocumentTypeMaster> GetAllSysDocTypeMasters()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.DocumentTypeMaster.AsNoTracking().ToList();
            }
        }

        public SystemReverificationMasters GetSystemReverification(Int64 ReverifyID)
        {
            SystemReverificationMasters lm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                lm = db.SystemReverificationMasters.AsNoTracking().Where(l => l.ReverificationID == ReverifyID).FirstOrDefault();
            }

            return lm;
        }

        public List<DocumentTypeMaster> GetSystemDocumentTypes(Int64 loanTypeID)
        {
            List<DocumentTypeMaster> dm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                List<CustLoanDocMapping> cldm = db.CustLoanDocMapping.AsNoTracking().Where(cld => cld.LoanTypeID == loanTypeID && cld.Active == true).ToList();

                dm = new List<DocumentTypeMaster>();

                foreach (CustLoanDocMapping loanDocMap in cldm)
                {
                    DocumentTypeMaster doc = db.DocumentTypeMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID && ld.Active == true).FirstOrDefault();
                    if (doc != null)
                        dm.Add(doc);
                }
            }

            return dm;
        }


        public List<DocumentTypeMaster> GetSystemDocumentTypesWithAllFields(Int64 loanTypeID)
        {
            List<DocumentTypeMaster> dm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                List<CustLoanDocMapping> cldm = db.CustLoanDocMapping.AsNoTracking().Where(cld => cld.LoanTypeID == loanTypeID && cld.Active == true).ToList();

                dm = new List<DocumentTypeMaster>();

                if (cldm != null)
                {

                    dm = (from cl in cldm
                          join dms in db.DocumentTypeMaster.AsNoTracking() on cl.DocumentTypeID equals dms.DocumentTypeID
                          where dms.Active == true
                          select new DocumentTypeMaster()
                          {
                              DocumentTypeID = dms.DocumentTypeID,
                              Name = dms.Name,
                              DisplayName = dms.DisplayName,
                              Active = dms.Active,
                              DocumentLevel = dms.DocumentLevel,
                              CreatedOn = dms.CreatedOn,
                              ModifiedOn = dms.ModifiedOn,
                              DocumentFieldMasters = db.DocumentFieldMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == dms.DocumentTypeID).ToList(),
                              DocumetTypeTables = db.DocumetTypeTables.AsNoTracking().Where(ld => ld.DocumentTypeID == dms.DocumentTypeID).ToList(),
                              DocumentLevelDesc = dms.DocumentLevelDesc,
                              RuleDocumentTables = db.RuleDocumentTables.AsNoTracking().Where(rd => rd.DocumentID == dms.DocumentTypeID).ToList(),
                              Condition = cl.Condition,
                              CustDocumentLevel = cl.DocumentLevel
                          }).ToList();
                }
            }

            return dm.OrderBy(a => a.Name).ToList();
        }

        public List<DocumentTypeMaster> GetSystemDocumentTypesWithFields(Int64 loanTypeID)
        {
            List<DocumentTypeMaster> dm = null;

            using (var db = new DBConnect(SystemSchema))
            {
                List<CustLoanDocMapping> cldm = db.CustLoanDocMapping.AsNoTracking().Where(cld => cld.LoanTypeID == loanTypeID && cld.Active == true).ToList();

                dm = new List<DocumentTypeMaster>();

                if (cldm != null)
                {

                    dm = (from cl in cldm
                          join dms in db.DocumentTypeMaster.AsNoTracking() on cl.DocumentTypeID equals dms.DocumentTypeID
                          where dms.Active == true
                          select new DocumentTypeMaster()
                          {
                              DocumentTypeID = dms.DocumentTypeID,
                              Name = dms.Name,
                              DisplayName = dms.DisplayName,
                              Active = dms.Active,
                              DocumentLevel = dms.DocumentLevel,
                              CreatedOn = dms.CreatedOn,
                              ModifiedOn = dms.ModifiedOn,
                              DocumentFieldMasters = db.DocumentFieldMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == dms.DocumentTypeID && ld.Active == true).ToList(),
                              DocumetTypeTables = db.DocumetTypeTables.AsNoTracking().Where(ld => ld.DocumentTypeID == dms.DocumentTypeID).ToList(),
                              DocumentLevelDesc = dms.DocumentLevelDesc,
                              RuleDocumentTables = db.RuleDocumentTables.AsNoTracking().Where(rd => rd.DocumentID == dms.DocumentTypeID).ToList(),
                              Condition = cl.Condition,
                              CustDocumentLevel = cl.DocumentLevel
                          }).ToList();
                }
            }

            return dm.OrderBy(a => a.Name).ToList();
        }

        #endregion
    }

}
