using IntellaLend.EntityDataHandler;
using IntellaLend.Model;
using System;
using System.Collections.Generic;

namespace IntellaLend.CommonServices
{
    public class MappingService
    {
        public MappingService() { }

        private static string TableSchema;

        public MappingService(string tableschema)
        {
            TableSchema = tableschema;
        }

        public List<ReviewTypeMaster> CustReviewTypeMapping(Int64 CustId)
        {
            return new MappingDataAccess(TableSchema).CustReviewTypeMapping(CustId);
        }

        public List<LoanTypeMaster> CustReviewLoanMapping(Int64 CustomerID, Int64 ReviewTypeId)
        {
            return new MappingDataAccess(TableSchema).CustReviewLoanMapping(CustomerID, ReviewTypeId);
        }
        public object GetSyncLoanDetails(Int64 LoanTypeID)
        {
            return new MappingDataAccess(TableSchema).GetSyncLoanDetails(LoanTypeID);
        }
        public bool SyncCustomerLoanType(Int64 LoanTypeID, Int64 UserID, Int64 SyncLevel)
        {
            return new MappingDataAccess(TableSchema).SyncCustomerLoanType(LoanTypeID, UserID, SyncLevel);
        }

        public bool SaveCustReviewLoanMapping(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID, string BoxUploadPath, string LoanUploadPath)
        {
            return new MappingDataAccess(TableSchema).SaveCustReviewLoanMapping(CustomerID, ReviewTypeID, LoanTypeID, BoxUploadPath, LoanUploadPath);
        }

        public bool SaveCustLoanUploadPath(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID, string BoxUploadPath, string LoanUploadPath, bool isRetainUpdate)
        {
            return new MappingDataAccess(TableSchema).SaveCustLoanUploadPath(CustomerID, ReviewTypeID, LoanTypeID, BoxUploadPath, LoanUploadPath, isRetainUpdate);
        }

        public bool CheckCustLoanUploadPath(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID, string LoanUploadPath)
        {
            return new MappingDataAccess(TableSchema).CheckCustLoanUploadPath(CustomerID, ReviewTypeID, LoanTypeID, LoanUploadPath);
        }

        public object GetCustReviewLoanCheckList(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID)
        {
            return new MappingDataAccess(TableSchema).GetCustReviewLoanCheckList(CustomerID, ReviewTypeID, LoanTypeID);
        }
        public object GetCustLoantypeMapping(Int64 CustomerID)
        {
            return new MappingDataAccess(TableSchema).GetCustLoantypeMapping(CustomerID);
        }
        public object SaveCustLoantypeMapping(List<CustLoantypeMapping> custLoantypeMappings)
        {
            return new MappingDataAccess(TableSchema).SaveCustLoantypeMapping(custLoantypeMappings);
        }

        public bool SaveCustReviewMapping(Int64 CustomerID, Int64 ReviewTypeId)
        {
            return new MappingDataAccess(TableSchema).SaveCustReviewMapping(CustomerID, ReviewTypeId);
        }
        public bool RetainCustReviewMapping(Int64 CustomerID, Int64 ReviewTypeId)
        {
            return new MappingDataAccess(TableSchema).RetainCustReviewMapping(CustomerID, ReviewTypeId);
        }

        public bool RetainCustReviewLoanMapping(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID)
        {
            return new MappingDataAccess(TableSchema).RetainCustReviewLoanMapping(CustomerID, ReviewTypeID, LoanTypeID);
        }


        public bool CheckCustReviewMapping(Int64 CustomerID, Int64 ReviewTypeId)
        {
            return new MappingDataAccess(TableSchema).CheckCustReviewMapping(CustomerID, ReviewTypeId);
        }

        public bool CheckCustReviewLoanMapping(Int64 CustomerID, Int64 ReviewTypeId, Int64 LoanTypeID)
        {
            return new MappingDataAccess(TableSchema).CheckCustReviewLoanMapping(CustomerID, ReviewTypeId, LoanTypeID);
        }

        public List<CheckListMaster> CustReviewLoanCheckListMapping(Int64 customerID, Int64 reviewTypeID, Int64 loanTypeID)
        {
            return new MappingDataAccess(TableSchema).CustReviewLoanCheckListMapping(customerID, reviewTypeID, loanTypeID);
        }

        public List<DocumentTypeMaster> CustLoanDocTypeMapping(Int64 customerID, Int64 loanTypeID)
        {
            return new MappingDataAccess(TableSchema).CustLoanDocTypeMapping(customerID, loanTypeID);
        }
        public List<object> GetCustLoanDocTypeMapping(Int64 loanTypeID, Int64 DocumentTypeID)
        {
            return new MappingDataAccess(TableSchema).GetCustLoanDocTypeMapping(loanTypeID, DocumentTypeID);
        }


        public List<object> GetDocumentFieldMasters(Int64[] documentTypeID)
        {
            return new MappingDataAccess(TableSchema).GetDocumentFieldMasters(documentTypeID);
        }
        public List<object> GetDocumentTypesBasedonLoanType()
        {
            return new MappingDataAccess(TableSchema).GetDocumentTypesBasedonLoanType();
        }

        public List<LoanTypeMaster> GetLoanTypeForCustomer(Int64 customerID)
        {
            return new MappingDataAccess(TableSchema).GetLoanTypeForCustomer(customerID);
        }

        public List<ReviewTypeMaster> GetSystemReviewTypes(Int64 CustomerID)
        {
            return new MappingDataAccess(TableSchema).GetSystemReviewTypes(CustomerID);
        }

        public object GetCustReviewTypes(Int64 CustomerID)
        {
            return new MappingDataAccess(TableSchema).GetCustReviewTypes(CustomerID);
        }

        public object GetCustReviewLoanTypes(Int64 customerID, Int64 reviewTypeID, bool isSaveEdit)
        {
            return new MappingDataAccess(TableSchema).CustReviewLoanTypes(customerID, reviewTypeID, isSaveEdit);
        }

        public object CustReviewLoanStackMapping(int customerID, int loanTypeID, int reviewTypeID)
        {
            return new MappingDataAccess(TableSchema).CustReviewLoanStackMapping(customerID, loanTypeID, reviewTypeID);
        }
        public bool CloneFromSystem(Int64 customerID, Int64 reviewTypeID, Int64[] loanTypeIDs)
        {
            return new MappingDataAccess(TableSchema).CloneFromSystem(customerID, reviewTypeID, loanTypeIDs);
        }

        public bool RemoveCustReviewMapping(Int64 customerID, Int64 reviewTypeID)
        {
            return new MappingDataAccess(TableSchema).RemoveCustReviewMapping(customerID, reviewTypeID);
        }

        public bool RemoveCustReviewLoanMapping(Int64 customerID, Int64 reviewTypeID, Int64 LoanTypeID)
        {
            return new MappingDataAccess(TableSchema).RemoveCustReviewLoanMapping(customerID, reviewTypeID, LoanTypeID);
        }
        public bool RemoveCustConfigUploadPath(Int64 customerID, Int64 reviewTypeID, Int64 LoanTypeID)
        {
            return new MappingDataAccess(TableSchema).RemoveCustConfigUploadPath(customerID, reviewTypeID, LoanTypeID);
        }
        public object GetReviewLoanLenderMapped( Int64 reviewTypeID, Int64 LoanTypeID)
        {
            return new MappingDataAccess(TableSchema).GetReviewLoanLenderMapped(reviewTypeID, LoanTypeID);
        }

        public object SaveReviewLoanLenderMapping(Int64 reviewTypeID, Int64 LoanTypeID,Int64[] allLenderIDs,Int64[] assignedLenderIDs,bool IsAdd)
        {
            return new MappingDataAccess(TableSchema).SaveReviewLoanLenderMapping(reviewTypeID, LoanTypeID, allLenderIDs,assignedLenderIDs,IsAdd);
        }
        
        //public object GetAllDocMappedCustomerandLoanTypes(long docTypeID)
        //{
        //    return new MappingDataAccess(TableSchema).GetAllDocMappedCustomerandLoanTypes(docTypeID);
        //}
    }
}
