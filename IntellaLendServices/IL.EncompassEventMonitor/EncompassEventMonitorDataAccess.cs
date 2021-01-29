﻿using IntellaLend.Constance;
using IntellaLend.Model;
using IntellaLend.Model.Encompass;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IL.EncompassEventMonitor
{
    public class EncompassEventMonitorDataAccess
    {
        public string TenantSchema;
        private static string SystemSchema = "IL";

        public EncompassEventMonitorDataAccess(string tenantSchema)
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

        public List<AuditEWebhookEvents> GetAuditEvents()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.AuditEWebhookEvents.AsNoTracking().Where(m => m.Processed == false).ToList();
            }
        }

        public bool CheckLoanExist(Guid _loanGUID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.Loan.AsNoTracking().Any(x => x.EnCompassLoanGUID == _loanGUID);
            }
        }

        public bool CheckTrailingCompleted(string _eLoanGUID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Guid _loanGUID = new Guid(_eLoanGUID);
                Loan _loan = db.Loan.AsNoTracking().Where(x => x.EnCompassLoanGUID == _loanGUID).FirstOrDefault();
                List<AuditLoanMissingDoc> auditLoanMissingDoc = db.AuditLoanMissingDoc.AsNoTracking().Where(x => (x.LoanID == _loan.LoanID) && (x.Status != StatusConstant.OCR_COMPLETED)).ToList();
                List<EWebhookEvents> _events = db.EWebhookEvents.AsNoTracking().Where(x => x.Response.Contains(_eLoanGUID) && x.EventType == EWebHookEventsLogConstant.DOCUMENT_LOG && x.IsTrailing == true && (x.Status == EWebHookStatusConstant.EWEB_HOOK_STAGED || x.Status == EWebHookStatusConstant.EWEB_HOOK_PROCESSING)).ToList();
                return auditLoanMissingDoc.Count == 0 && _events.Count == 0;
            }
        }

        public void SetEWebhookEvents(Int64 _auditID, string _eLoanGUID, Int32 _eventType, bool isTrailingDoc = false)
        {
            using (var tenantDB = new DBConnect(TenantSchema))
            {
                tenantDB.EWebhookEvents.Add(new EWebhookEvents()
                {
                    AuditID = _auditID,
                    CreatedOn = DateTime.Now,
                    EventType = _eventType,
                    Status = EWebHookStatusConstant.EWEB_HOOK_STAGED,
                    Response = JsonConvert.SerializeObject(new { loanGUID = _eLoanGUID }),
                    IsTrailing = isTrailingDoc
                });
                tenantDB.SaveChanges();
            }
        }

        public List<IntellaAndEncompassFetchFields> GetIntellaAndEncompassFetchFields()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.IntellaAndEncompassFetchFields.AsNoTracking().Where(m => m.Active && m.TenantSchema == TenantSchema).ToList();
            }
        }

    }
}
