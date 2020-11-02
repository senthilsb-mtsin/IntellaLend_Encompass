using IntellaLend.EntityDataHandler;
using IntellaLend.Model;
using MTSRuleEngine;
using System;
using System.Collections.Generic;

namespace IntellaLend.CommonServices
{
    public  class CheckListItemService
    {
        public CheckListItemService() { }

        public static string TableSchema;

        public CheckListItemService(string tableschema)
        {
            TableSchema = tableschema;
        }

        public CheckListDetailMaster CheckListDetails(CheckListDetailMaster checklistmaster, RuleMaster rulemasters)
        {
            checklistmaster.CreatedOn = DateTime.Now;
            checklistmaster.ModifiedOn = DateTime.Now;
            rulemasters.CreatedOn = DateTime.Now;
            rulemasters.ModifiedOn = DateTime.Now;
            return new CheckListItemDataAccess(TableSchema).CheckListDetails(checklistmaster, rulemasters);
        }

        public object SearchCheckListItem(Int64 checkListDetailID)
        {
            return new CheckListItemDataAccess(TableSchema).SearchCheckListItem(checkListDetailID);
        }

        public object UpdateCheckListDetails(CheckListDetailMaster checkListDetailMaster, RuleMaster rulemasters)
        {
            //checkListDetailMaster.ModifiedOn = DateTime.Now;
            return new CheckListItemDataAccess(TableSchema).UpdateCheckListDetails(checkListDetailMaster, rulemasters);
        }

        public object DeleteCheckListItem(Int64[] checkListDetailsID)
        {
            return new CheckListItemDataAccess(TableSchema).DeleteCheckListItem(checkListDetailsID);
        }

        public object CloneCheckListItem(Int64[] checkListDetailsID, string modifiedCheckListName)
        {
            return new CheckListItemDataAccess(TableSchema).CloneCheckListItem(checkListDetailsID, modifiedCheckListName);
        }
        public object TestCheckListItemValues(Dictionary<string, object> checklistitemvalues, string ruleformula)
        {
            MTSRules lRules = new MTSRules();
            lRules.Add("rule1", ruleformula);
            if (checklistitemvalues==null)
            {
                checklistitemvalues = new Dictionary<string, object>();
            }
            Dictionary<string, MTSRuleResult> output= lRules.Eval(checklistitemvalues);
            return new { Result = output["rule1"].Result, EvalExp= output["rule1"].Expressions, Message=output["rule1"].Message };
        }
        public object GetAllSysCheckListDetails()
        {
            return new IntellaLendDataAccess().GetAllSysCheckListDetails();
        }

        public object AddSysCheckList(CheckListMaster checkListMaster)
        {
            checkListMaster.CreatedOn = DateTime.Now;
            checkListMaster.ModifiedOn = DateTime.Now;
            return new IntellaLendDataAccess().AddSysCheckList(checkListMaster);
        }

        public object GetAllSysDocTypeMasters()
        {
            return new IntellaLendDataAccess().GetAllSysDocTypeMasters();
        }

        public List<object> GetSysDocumentFieldMasters(long documentTypeID)
        {
            return new IntellaLendDataAccess().GetSysDocumentFieldMasters(documentTypeID);
        }

        public object SaveSysCheckListDetails(CheckListDetailMaster checkListDetailMaster, RuleMaster rulemasters, Int64 LoanTypeID)
        {
            checkListDetailMaster.CreatedOn = DateTime.Now;
            checkListDetailMaster.ModifiedOn = DateTime.Now;
            rulemasters.CreatedOn = DateTime.Now;
            rulemasters.ModifiedOn = DateTime.Now;
            return new IntellaLendDataAccess().SaveSysCheckListDetails(checkListDetailMaster, rulemasters, LoanTypeID);
        }

        public object GetEditSysDocTypeMasters(long CheckListDetailID, long loanTypeID)
        {
          return new IntellaLendDataAccess().GetEditSysDocTypeMasters(CheckListDetailID, loanTypeID);
        }

        public object UpdateSysCheckListDetails(CheckListDetailMaster checkListDetailMaster, RuleMaster rulemasters,Int64 LoanTypeID)
        {
            return new IntellaLendDataAccess().UpdateSysCheckListDetails(checkListDetailMaster, rulemasters, LoanTypeID);
        }

        public object GetAllCheckListItems(Int64 checkListID)
        {
            return new IntellaLendDataAccess().GetAllCheckListItems(checkListID);
        }

        public object SaveSysEditCheckListDetails(CheckListDetailMaster checkListDetailMaster, RuleMaster rulemasters, Int64 LoanTypId)
        {
            checkListDetailMaster.CreatedOn = DateTime.Now;
            checkListDetailMaster.ModifiedOn = DateTime.Now;
            rulemasters.CreatedOn = DateTime.Now;
            rulemasters.ModifiedOn = DateTime.Now;
            return new IntellaLendDataAccess().SaveSysEditCheckListDetails(checkListDetailMaster, rulemasters, LoanTypId);
        }

        public bool SaveSysCheckListName(string checkListName, long checkListID, bool Active,bool Sync)
        {
            return new IntellaLendDataAccess().SaveSysCheckListName(checkListName, checkListID, Active,Sync);
        }

        public object AssignChecklist(long LoaTypeID, string checkListName, long checkListID)
        {
            return new IntellaLendDataAccess().AssignChecklist(LoaTypeID, checkListName, checkListID);
        }

        public object AssignStackingOrder(long LoaTypeID, string StackingOrderName, long StackingOrderID)
        {
            return new IntellaLendDataAccess().AssignStackingOrder(LoaTypeID, StackingOrderName, StackingOrderID);
        }
        

        public object CloneSysCheckListItem(long[] checkListDetailsID, string modifiedCheckListName,Int64 LoanTypeID)
        {
            return new IntellaLendDataAccess().CloneSysCheckListItem(checkListDetailsID, modifiedCheckListName ,LoanTypeID);
        }

        public object DeleteSysCheckListItem(long[] checkListDetailsID, Int64 LoanTypeID)
        {
            return new IntellaLendDataAccess().DeleteSysCheckListItem(checkListDetailsID,LoanTypeID);
        }

        public List<CategoryLists> GetChecklistCategories()
        {
            return new IntellaLendDataAccess().GetChecklistCategories();
        }
        public bool SaveTenantCheckListDetails(Int64 CheckListID, Int64 LoanTypeID)
        {
            return new IntellaLendDataAccess().SaveTenantCheckListDetails(CheckListID, LoanTypeID);
        }

    }
}
