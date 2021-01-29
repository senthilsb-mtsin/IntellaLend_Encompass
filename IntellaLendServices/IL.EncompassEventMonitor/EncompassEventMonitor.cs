using EncompassAPIHelper;
using EncompassRequestBody.EResponseModel;
using IntellaLend.Constance;
using IntellaLend.Model;
using IntellaLend.Model.Encompass;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace IL.EncompassEventMonitor
{
    public class EncompassEventMonitor : IMTSServiceBase
    {
        private static string EncompassWrapperAPIURL = string.Empty;
        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            EncompassWrapperAPIURL = Params.Find(f => f.Key == "EncompassWrapperAPIURL").Value;
        }

        public bool DoTask()
        {
            try
            {
                var TenantList = EncompassEventMonitorDataAccess.GetTenantList();
                foreach (var tenant in TenantList)
                {
                    EncompassEventMonitorDataAccess dataAccess = new EncompassEventMonitorDataAccess(tenant.TenantSchema);
                    EncompassWrapperAPI _api = new EncompassWrapperAPI(EncompassWrapperAPIURL, tenant.TenantSchema);
                    ProcessAuditEvents(_api, dataAccess);
                    _api.Dispose();
                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }

            return true;
        }

        private void ProcessAuditEvents(EncompassWrapperAPI _api, EncompassEventMonitorDataAccess dataAccess)
        {
            List<AuditEWebhookEvents> _events = dataAccess.GetAuditEvents();
            List<IntellaAndEncompassFetchFields> _enImportFields = dataAccess.GetIntellaAndEncompassFetchFields();

            foreach (AuditEWebhookEvents _event in _events)
            {
                dynamic obj = JsonConvert.DeserializeObject(_event.Response);
                string _eLoanGUID = obj.loanGUID;

                if (_event.EventType == EWebHookEventsLogConstant.MILESTONELOG)
                {
                    CheckMileStone(_event.ID, _api, _eLoanGUID, _enImportFields, dataAccess);
                }
                else
                {
                    CheckDocumentEvent(_event.ID, _api, _eLoanGUID, _enImportFields, dataAccess);
                }
            }
        }

        private void CheckMileStone(Int64 _auditID, EncompassWrapperAPI _api, string _eLoanGUID, List<IntellaAndEncompassFetchFields> _enImportFields, EncompassEventMonitorDataAccess dataAccess)
        {
            string[] FieldIDs = _enImportFields.Select(x => x.EncompassFieldID).ToArray();

            List<EFieldResponse> _response = _api.GetPredefinedFieldValues(_eLoanGUID, FieldIDs);

            IntellaAndEncompassFetchFields _serviceType = _enImportFields.Where(x => x.FieldType.Contains(LOSFieldType.SERVICETYPE)).FirstOrDefault();

            EFieldResponse _eServiceType = _response.Where(x => x.FieldId == _serviceType.EncompassFieldID).FirstOrDefault();

            Guid _loanGUID = new Guid(_eLoanGUID);

            bool loanExists = dataAccess.CheckLoanExist(_loanGUID);

            if (loanExists && !(_serviceType.EncompassFieldValue.Split(',').Contains(_eServiceType.Value)) && dataAccess.CheckTrailingCompleted(_eLoanGUID))
            {
                dataAccess.SetEWebhookEvents(_auditID, _eLoanGUID, EWebHookEventsLogConstant.MILESTONELOG);
            }
        }

        private void CheckDocumentEvent(Int64 _auditID, EncompassWrapperAPI _api, string _eLoanGUID, List<IntellaAndEncompassFetchFields> _enImportFields, EncompassEventMonitorDataAccess dataAccess)
        {
            string[] FieldIDs = _enImportFields.Select(x => x.EncompassFieldID).ToArray();

            List<EFieldResponse> _response = _api.GetPredefinedFieldValues(_eLoanGUID, FieldIDs);

            IntellaAndEncompassFetchFields _serviceType = _enImportFields.Where(x => x.FieldType.Contains(LOSFieldType.SERVICETYPE)).FirstOrDefault();

            EFieldResponse _eServiceType = _response.Where(x => x.FieldId == _serviceType.EncompassFieldID).FirstOrDefault();

            Guid _loanGUID = new Guid(_eLoanGUID);

            bool loanExists = dataAccess.CheckLoanExist(_loanGUID);

            if (_serviceType.EncompassFieldValue.Split(',').Contains(_eServiceType.Value))
            {
                dataAccess.SetEWebhookEvents(_auditID, _eLoanGUID, EWebHookEventsLogConstant.DOCUMENT_LOG, loanExists);
            }
        }
    }
}
