using IntellaLend.EntityDataHandler;
using IntellaLend.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntellaLend.CommonServices
{
    public class IntellaLendServices
    {

        #region Constructor
        private static string TenantSchema;
        private static string SystemSchema = "IL";

        public IntellaLendServices()
        { }

        public IntellaLendServices(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion

        #region Public Methods
        public bool SetDocLoanTypeMapping(Int64 DocumentTypeID, Int64[] LoanTypeIDs)
        {
            return new IntellaLendDataAccess().SetDocLoanTypeMapping(DocumentTypeID, LoanTypeIDs);
        }

        public object GetEncompassToken(string _instanceID)
        {
            return new IntellaLendDataAccess().GetEncompassToken(_instanceID);
        }
        public object SetMileStoneEvent(string _loanGUID, string _instanceID)
        {
            return new IntellaLendDataAccess().SetDocumentEvent(_instanceID);
        }

        public object SetDocumentEvent(string _loanGUID, string _instanceID)
        {
            return new IntellaLendDataAccess().SetDocumentEvent(_instanceID);
        }


        public bool DeleteDocumentField(Int64 FieldID)
        {
            return new IntellaLendDataAccess().DeleteDocumentField(FieldID);
        }

        public Int64 AddDocumentField(Int64 DocumentTypeID, string FieldName, string FieldDisplayName)
        {
            return new IntellaLendDataAccess().AddDocumentField(DocumentTypeID, FieldName, FieldDisplayName);
        }

        public List<ReviewTypeMaster> GetSystemReviewTypes()
        {
            return new IntellaLendDataAccess().GetSystemAllReviewTypes();
        }

        public List<LoanTypeMaster> GetSystemLoanTypes()
        {
            return new IntellaLendDataAccess().GetSystemAllLoanTypes();
        }

        public object GetSystemLoanTypes(Int64 ReviewTypeID)
        {
            return new IntellaLendDataAccess().GetSystemAllLoanTypes(ReviewTypeID);
        }

        public object GetLoanTypes(Int64 ReviewTypeID, bool IsAdd)
        {
            return new IntellaLendDataAccess().GetSystemAllLoanTypes(ReviewTypeID, IsAdd);
        }

        public object GetAssignedSystemLoanTypes(Int64 DocumentTypeID)
        {
            return new IntellaLendDataAccess().GetAssignedSystemAllLoanTypes(DocumentTypeID);
        }
        public object GetSystemStackCheck(Int64 ReviewTypeID, Int64 LoanTypeID, bool IsAdd)
        {
            return new IntellaLendDataAccess().GetSystemStackCheck(ReviewTypeID, LoanTypeID, IsAdd);
        }

        public object SetLoanCheckMapping(Int64 LoanTypeID, Int64 CheckListID, List<ChecklistItemSequence> ChecklistItemSeq)
        {
            return new IntellaLendDataAccess().SetLoanCheckMapping(LoanTypeID, CheckListID, ChecklistItemSeq);
        }

        public object GetCheckList(Int64 LoanTypeID)
        {
            return new IntellaLendDataAccess().GetCheckList(LoanTypeID);
        }

        public List<string> GetLosSystemDocumentTypesWithDocFields(string losName)
        {
            return new IntellaLendDataAccess().GetLosSystemDocumentTypesWithDocFields(losName);
        }
        public List<LosFieldDetails> GetLosSystemDocFields(string docName)
        {
            return new IntellaLendDataAccess().GetLosSystemDocFields(docName);
        }


        public object GetStackingOrder(Int64 LoanTypeID)
        {
            return new IntellaLendDataAccess().GetStackingOrder(LoanTypeID);
        }

        public object SetLoanStackMapping(Int64 LoanTypeID, Int64 StackingOrderID)
        {
            return new IntellaLendDataAccess().SetLoanStackMapping(LoanTypeID, StackingOrderID);
        }

        public object SaveMapping(Int64 ReviewTypeID, Int64 LoanTypeID, Int64 CheckListID, Int64 StackingOrderID, bool IsAdd)
        {
            return new IntellaLendDataAccess().SaveMapping(ReviewTypeID, LoanTypeID, CheckListID, StackingOrderID, IsAdd);
        }

        public object GetReviewLoanMapped(Int64 ReviewTypeID)
        {
            return new IntellaLendDataAccess().GetReviewLoanMapped(ReviewTypeID);
        }

        public object SetReviewLoanMapping(Int64 ReviewTypeID, Int64[] LoanTypeIDs)
        {
            return new IntellaLendDataAccess().SetReviewLoanMapping(ReviewTypeID, LoanTypeIDs);
        }

        public string GetLosValues(string searchValue)
        {

            return new IntellaLendDataAccess().GetLosValue(searchValue);
        }

        public object GetSystemDocuments(Int64 LoanTypeID)
        {
            return new IntellaLendDataAccess().GetSystemDocuments(LoanTypeID);
        }

        public bool SaveConditionGeneralRule(Int64 DocumentID, string RuleValue, Int64 LoanTypeID)
        {
            return new IntellaLendDataAccess(TenantSchema).SaveConditionGeneralRule(DocumentID, RuleValue, LoanTypeID);
        }

        public bool SetLoanDocTypeMapping(Int64 LoanTypeID, List<DocMappingDetails> DocMappingDetails)
        {
            return new IntellaLendDataAccess().SetLoanDocTypeMapping(LoanTypeID, DocMappingDetails);
        }

        public bool SetReverifyDocMapping(Int64 ReverificationID, Int64[] DocTypeIDs)
        {
            return new IntellaLendDataAccess().SetReverifyDocMapping(ReverificationID, DocTypeIDs);
        }

        public bool SetCustReverifyDocMapping(Int64 CustomerID, Int64 ReverificationID, Int64[] DocTypeIDs)
        {
            return new IntellaLendDataAccess(TenantSchema).SetCustReverifyDocMapping(CustomerID, ReverificationID, DocTypeIDs);
        }


        public object GetMappedTemplate(Int64 TemplateID, Int64 MappingID, Int64 ReverificationID)
        {
            return new IntellaLendDataAccess().GetMappedTemplate(TemplateID, MappingID, ReverificationID);
        }

        public object GetCustMappedTemplate(Int64 TemplateID, Int64 MappingID, Int64 ReverificationID)
        {
            return new IntellaLendDataAccess(TenantSchema).GetCustMappedTemplate(TemplateID, MappingID, ReverificationID);
        }


        public object GetSystemDocumentTypes()
        {
            return new IntellaLendDataAccess().GetSystemDocumentTypes();
        }

        public List<object> GetReverificationMaster()
        {
            return new IntellaLendDataAccess().GetReverificationMaster();
        }

        public List<object> GetCustReverificationMaster()
        {
            return new IntellaLendDataAccess(TenantSchema).GetCustReverificationMaster();
        }

        public List<string> GetDocFieldsByName(string DocumentTypeName)
        {
            return new IntellaLendDataAccess().GetDocFieldsByName(DocumentTypeName);
        }
        public List<SystemReverificationMasters> CheckReverificationExistForEdit(string ReverificationName)
        {
            return new IntellaLendDataAccess().CheckReverificationExistForEdit(ReverificationName);
        }
        public List<SystemReverificationMasters> CheckManagerReverificationExistForEdit(string ReverificationName)
        {
            return new IntellaLendDataAccess(TenantSchema).CheckManagerReverificationExistForEdit(ReverificationName);
        }
        public bool UpdateMappingTemplateFields(Int64 MappingID, string TemplateFieldJson)
        {
            return new IntellaLendDataAccess().UpdateMappingTemplateFields(MappingID, TemplateFieldJson);
        }

        public bool UpdateCustMappingTemplateFields(Int64 MappingID, string TemplateFieldJson)
        {
            return new IntellaLendDataAccess(TenantSchema).UpdateCustMappingTemplateFields(MappingID, TemplateFieldJson);
        }

        //public bool SetDocLoanTypeMapping(Int64 DocumentTypeID, Int64[] LoanTypeIDs)
        //{
        //    return new IntellaLendDataAccess().SetDocLoanTypeMapping(DocumentTypeID, LoanTypeIDs);
        //}

        //public bool DeleteDocumentField(Int64 FieldID)
        //{
        //    return new IntellaLendDataAccess().DeleteDocumentField(FieldID);
        //}

        //public Int64 AddDocumentField(Int64 DocumentTypeID, string FieldName, string FieldDisplayName)
        //{
        //    return new IntellaLendDataAccess().AddDocumentField(DocumentTypeID, FieldName, FieldDisplayName);
        //}

        public List<SystemReverificationTemplate> GetReverificationTemplate()
        {
            return new IntellaLendDataAccess().GetReverificationTemplate();
        }

        public byte[] GetReverificationLogo(string Guid)
        {
            return new IntellaLendDataAccess(TenantSchema).GetReverificationLogo(Guid);
        }

        public async Task<Task<object>> ReverificationFileUploader(Dictionary<string, string> paramsValues, string filePath, MultipartMemoryStreamProvider provider)
        {
            object data = null;
            try
            {
                byte[] fileStream = null;
                foreach (var file in provider.Contents)
                {
                    fileStream = await file.ReadAsByteArrayAsync();
                }
                if (fileStream.Length == 0)
                    throw new InvalidOperationException("File Content Length Zero");

                if (fileStream != null)
                {
                    string ReverificationName = "";
                    Guid LogoGUID = Guid.NewGuid();
                    try
                    {
                        Int64 ReverificationID = Convert.ToInt64(paramsValues["ReverificationID"]);
                        string FileName = Convert.ToString(paramsValues["UploadFileName"]);
                        string TenantSchema = Convert.ToString(paramsValues["TableSchema"]);
                        //bool Active = true;

                        data = new IntellaLendDataAccess(TenantSchema).ReverificationFileUploader(ReverificationID, fileStream, LogoGUID, FileName);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidDataException(ex.Message, ex.InnerException);
                    }
                    //File.WriteAllBytes(Path.Combine(UploadPath, LogoGUID.ToString()), fileStream);

                    if (data != null)
                        return Task.FromResult<object>(data);
                    else
                        throw new Exception("DB Exception");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult<object>(data);
        }

        public async Task<Task<object>> CustReverificationFileUploader(Dictionary<string, string> paramsValues, string filePath, MultipartMemoryStreamProvider provider)
        {
            object data = null;
            try
            {
                byte[] fileStream = null;
                foreach (var file in provider.Contents)
                {
                    fileStream = await file.ReadAsByteArrayAsync();
                }
                if (fileStream.Length == 0)
                    throw new InvalidOperationException("File Content Length Zero");

                if (fileStream != null)
                {
                    string ReverificationName = "";
                    Guid LogoGUID = Guid.NewGuid();
                    try
                    {
                        Int64 ReverificationID = Convert.ToInt64(paramsValues["ReverificationID"]);
                        string FileName = Convert.ToString(paramsValues["UploadFileName"]);
                        string TenantSchema = Convert.ToString(paramsValues["TableSchema"]);
                        //bool Active = true;

                        data = new IntellaLendDataAccess(TenantSchema).CustReverificationFileUploader(ReverificationID, fileStream, LogoGUID, FileName);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidDataException(ex.Message, ex.InnerException);
                    }
                    //File.WriteAllBytes(Path.Combine(UploadPath, LogoGUID.ToString()), fileStream);

                    if (data != null)
                        return Task.FromResult<object>(data);
                    else
                        throw new Exception("DB Exception");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult<object>(data);
        }
        public object SaveReverification(Int64 LoanTypeID, string ReverificationName, Int64 TemplateID)
        {
            return new IntellaLendDataAccess().SaveReverification(LoanTypeID, ReverificationName, TemplateID);
        }

        public object UpdateReverification(Int64 LoanTypeID, string ReverificationName, Int64 TemplateID, Int64 MappingID, Int64 ReverificationID, bool Active)
        {
            return new IntellaLendDataAccess().UpdateReverification(LoanTypeID, ReverificationName, TemplateID, MappingID, ReverificationID, Active);
        }

        public object UpdateCustReverification(Int64 CustomerID, Int64 LoanTypeID, string ReverificationName, Int64 TemplateID, Int64 MappingID, Int64 ReverificationID, bool Active)
        {
            return new IntellaLendDataAccess(TenantSchema).UpdateCustReverification(CustomerID, LoanTypeID, ReverificationName, TemplateID, MappingID, ReverificationID, Active);
        }


        public object GetReverificationMappedDoc(Int64 LoanTypeID, Int64 ReverificationID)
        {
            return new IntellaLendDataAccess().GetReverificationMappedDoc(LoanTypeID, ReverificationID);
        }

        public object GetCustReverificationMappedDoc(Int64 CustomerID, Int64 LoanTypeID, Int64 ReverificationID)
        {
            return new IntellaLendDataAccess(TenantSchema).GetCustReverificationMappedDoc(CustomerID, LoanTypeID, ReverificationID);
        }


        public List<SMTPDETAILS> GetAllSMPTDetails()
        {
            return new IntellaLendDataAccess().GetAllSMPTDetails();
        }

        public bool SaveAllSMTPDetails(SMTPDETAILS sMTPMaster)
        {
            return new IntellaLendDataAccess().SaveAllSMTPDetails(sMTPMaster);
        }

        public List<EncompassDocFields> GetLOSDocFields()
        {
            return new IntellaLendDataAccess().GetLOSDocFields();
        }
        public bool SetLOSDocFieldValues(string losType)
        {
            return new IntellaLendDataAccess().SetLOSDocFieldValues(losType);
        }

        public PasswordPolicy GetPasswordPolicy()
        {
            return new IntellaLendDataAccess().GetPasswordPolicy();
        }
        public PasswordPolicy SavePasswordPolicy(PasswordPolicy _passwordPolicy)
        {
            return new IntellaLendDataAccess().SavePasswordPolicy(_passwordPolicy);
        }
        #endregion

        #region Private Methods



        #endregion
    }
}
