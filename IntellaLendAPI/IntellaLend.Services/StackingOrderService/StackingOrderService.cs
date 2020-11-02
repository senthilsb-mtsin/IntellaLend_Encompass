using IntellaLend.EntityDataHandler;
using IntellaLend.Model;
using System;
using System.Collections.Generic;

namespace IntellaLend.CommonServices
{
    public class StackingOrderService
    {
        public StackingOrderService() { }

        public static string TableSchema;

        public StackingOrderService(string tableschema)
        {
            TableSchema = tableschema;
        }

        public object SearchStackingOrder(long stackingOrderID)
        {
            return new StackingOrderDataAccess(TableSchema).SearchStackingOrder(stackingOrderID);
        }

        public object SaveStackingOrderDetails(int stackOrderID,List<GetStackOrder> stackingOrderDetails)
        {
            return new StackingOrderDataAccess(TableSchema).SaveStackingOrderDetails(stackOrderID,stackingOrderDetails);
        }

        public object SaveSystemStackingOrderDetails(int stackOrderID, string StackOrderName,bool IsActive, List<GetStackOrder> stackingOrderDetails)
        {
            return new IntellaLendDataAccess().SaveSystemStackingOrderDetails(stackOrderID, StackOrderName, IsActive, stackingOrderDetails);
        }

        public object GetAllSystemStackingOrderDetails()
        {
            return new IntellaLendDataAccess().GetAllSystemStackingOrderDetails();
        }

        public object GetSystemStackingOrderDetailMaster(long stackingOrderID)
        {
            return new IntellaLendDataAccess().GetSystemStackingOrderDetailMaster(stackingOrderID);
        }

        public bool SetOrderByField(Int64 DocumentTypeID, Int64 FieldID)
        {
            return new IntellaLendDataAccess().SetOrderByField(DocumentTypeID, FieldID);
        }

        public bool SetTenantOrderByField(Int64 DocumentTypeID, Int64 FieldID)
        {
            return new StackingOrderDataAccess(TableSchema).SetTenantOrderByField(DocumentTypeID, FieldID);
        }

        public object GetSystemDocumentTypes()
        {
            return new IntellaLendDataAccess().GetSystemDocumentTypes();
        }

        public List<DocumentTypeMaster> GetSystemDocumentTypesWithDocFields(Int64 loanTypeID)
        {
            return new IntellaLendDataAccess().GetSystemDocumentTypesWithFields(loanTypeID);
        }
        
        
        public object GetLoanStackDocs(Int64 LoanTypeID)
        {
            return new IntellaLendDataAccess().GetLoanStackDocs(LoanTypeID);
        }

        public List<object> GetStackSystemDocumentTypes()
        {
            return new IntellaLendDataAccess().GetStackSystemDocumentTypes();
        }
        public List<DocumentFieldMaster> GetSystemDocumentFields()
        {
            return new IntellaLendDataAccess().GetSystemDocumentFields();
        }

        public bool SetTenantDocFielValue(Int64 DocumentTypeID, Int64 FieldID)
        {
            return new StackingOrderDataAccess(TableSchema).SetTenantDocFielValue(DocumentTypeID, FieldID);
        }
        public bool SetDocFieldValue(Int64 DocumentTypeID, Int64 FieldID)
        {
            return new IntellaLendDataAccess().SetDocFieldValue(DocumentTypeID, FieldID);
        }
        
        public bool SetDocGroupFieldValue(GetStackOrder StackOrder, List<StackingOrderDocumentFieldMaster> StackingOrderDetails)
        {
            return new IntellaLendDataAccess(TableSchema).SetDocGroupFieldValue(StackOrder, StackingOrderDetails);
        }
        public bool SetTenantDocGroupFieldValue(GetStackOrder StackOrder, List<StackingOrderDocumentFieldMaster> StackingOrderDetails)
        {
            return new StackingOrderDataAccess(TableSchema).SetTenantDocGroupFieldValue(StackOrder, StackingOrderDetails);
        }
    }
}
