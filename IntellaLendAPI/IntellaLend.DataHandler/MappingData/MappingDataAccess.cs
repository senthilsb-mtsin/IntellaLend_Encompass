using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntellaLend.EntityDataHandler
{
    public class MappingDataAccess
    {
        protected static string TableSchema;

        #region Constructor

        public MappingDataAccess()
        { }

        public MappingDataAccess(string tableSchema)
        {
            TableSchema = tableSchema;
        }

        #endregion

        public List<ReviewTypeMaster> CustReviewTypeMapping(Int64 CustId)
        {
            List<ReviewTypeMaster> reviewType = null;

            using (var db = new DBConnect(TableSchema))
            {
                List<CustReviewMapping> custLoanList = db.CustReviewMapping.Where(u => u.CustomerID == CustId).ToList();

                if (custLoanList != null)
                {
                    reviewType = new List<ReviewTypeMaster>();

                    foreach (var loanList in custLoanList)
                    {
                        reviewType.Add(db.ReviewTypeMaster.AsNoTracking().Where(lt => lt.ReviewTypeID == loanList.ReviewTypeID && lt.Type == 0).FirstOrDefault());
                    }
                }
            }

            return reviewType;
        }

        public List<DocumentTypeMaster> CustLoanDocTypeMapping(Int64 customerID, Int64 loanTypeID)
        {
            List<DocumentTypeMaster> dm = null;

            using (var db = new DBConnect(TableSchema))
            {
                List<CustLoanDocMapping> cldm = db.CustLoanDocMapping.AsNoTracking().Where(cld => cld.CustomerID == customerID && cld.LoanTypeID == loanTypeID).ToList();

                dm = new List<DocumentTypeMaster>();

                foreach (CustLoanDocMapping loanDocMap in cldm)
                {
                    DocumentTypeMaster doc = db.DocumentTypeMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == loanDocMap.DocumentTypeID).FirstOrDefault();

                    dm.Add(doc);
                }
            }

            return dm;
        }

        public List<object> GetDocumentFieldMasters(Int64[] documentTypeID)
        {
            List<object> docFiledMasters = null;

            using (var db = new DBConnect(TableSchema))
            {
                docFiledMasters = (from dm in db.DocumentTypeMaster
                                   where (documentTypeID.Contains((int)dm.DocumentTypeID))
                                   select new
                                   {
                                       DocID = dm.DocumentTypeID,
                                       DocName = dm.Name,
                                       Fields = (from f in db.DocumentFieldMaster
                                                 where f.DocumentTypeID == dm.DocumentTypeID
                                                 select f).ToList()
                                   }).ToList<object>();
            }

            return docFiledMasters;
        }

        public List<object> GetDocumentTypesBasedonLoanType(Int64 customerID, Int64 loanTypeID)
        {
            List<object> docTypeMasters = null;

            using (var db = new DBConnect(TableSchema))
            {
                docTypeMasters = (from dm in db.CustLoanDocMapping
                                   where (customerID==dm.CustomerID && loanTypeID == dm.LoanTypeID)
                                   select new
                                   {
                                       DocID = dm.DocumentTypeID,
                                       
                                       DocumentName = (from f in db.DocumentTypeMaster
                                                 where f.DocumentTypeID == dm.DocumentTypeID
                                                 select f).ToList()
                                   }).ToList<object>();
            }

            return docTypeMasters;
        }

        public List<StackingOrderMaster> CustReviewLoanStackMapping(int customerID, int loanTypeID, int reviewTypeID)
        {
            List<StackingOrderMaster> stackMaster = null;
            using (var db = new DBConnect(TableSchema))
            {
                List<CustReviewLoanStackMapping> CLRStackList = db.CustReviewLoanStackMapping.
                     Where(u => u.CustomerID == customerID && u.LoanTypeID == loanTypeID && u.ReviewTypeID == reviewTypeID).ToList();
                if (CLRStackList != null)
                {
                    stackMaster = new List<StackingOrderMaster>();
                    foreach (var getStackingOrder in CLRStackList)
                    {
                        stackMaster.Add(db.StackingOrderMaster.Where(SO => SO.StackingOrderID == getStackingOrder.StackingOrderID).FirstOrDefault());
                    }
                    stackMaster = stackMaster.Distinct().ToList();
                }
            }
            return stackMaster;
        }
        public List<CheckListMaster> CustReviewLoanCheckListMapping(Int64 customerID, Int64 reviewTypeID, Int64 loanTypeID)
        {
            List<CheckListMaster> chklstmaster = null;

            using (var db = new DBConnect(TableSchema))
            {
                List<CustReviewLoanCheckMapping> custrevloanchkmap = db.CustReviewLoanCheckMapping.AsNoTracking()
                    .Where(u => u.CustomerID == customerID && u.LoanTypeID == loanTypeID && u.ReviewTypeID == reviewTypeID).ToList();

                if (custrevloanchkmap != null)
                {
                    chklstmaster = new List<CheckListMaster>();

                    foreach (var mappedCheckList in custrevloanchkmap)
                    {
                        chklstmaster.Add(db.CheckListMaster.AsNoTracking().Where(cm => cm.CheckListID == mappedCheckList.CheckListID).FirstOrDefault());
                    }

                    chklstmaster = chklstmaster.Distinct().ToList();
                }
            }
            return chklstmaster;
        }

        public List<LoanTypeMaster> CustReviewLoanMapping(Int64 customerID, Int64 ReviewTypeId)
        {
            List<LoanTypeMaster> custloanreviewmaster;

            using (var db = new DBConnect(TableSchema))
            {
                List<CustReviewLoanMapping> custloanreviewmap = db.CustReviewLoanMapping.Where(u => u.CustomerID == customerID && u.ReviewTypeID == ReviewTypeId).ToList();

                custloanreviewmaster = new List<LoanTypeMaster>();

                if (custloanreviewmap != null)
                {
                    foreach (var revmaster in custloanreviewmap)
                    {
                        custloanreviewmaster.Add(db.LoanTypeMaster.AsNoTracking().Where(rt => rt.LoanTypeID == revmaster.LoanTypeID && rt.Type == 0).FirstOrDefault());
                    }
                    custloanreviewmaster = custloanreviewmaster.Distinct().ToList();
                }

            }
            return custloanreviewmaster;
        }

        #region Mapping Page DataAccess

        public List<ReviewTypeMaster> GetSystemReviewTypes(Int64 CustomerID)
        {
            List<ReviewTypeMaster> systemReviewTypes = new IntellaLendDataAccess().GetSystemReviewTypes();
            List<LoanTypeMaster> SystmerLoanTypeMaster = new IntellaLendDataAccess().GetSystemLoanTypes();

            if (systemReviewTypes != null)
            {
                using (var db = new DBConnect(TableSchema))
                {
                    var custReviewTypes = db.CustReviewMapping.Where(cl => cl.CustomerID == CustomerID).AsNoTracking().ToList();

                    foreach (var item in custReviewTypes)
                    {
                        var custReviewMapping = db.CustReviewLoanMapping.Where(x => x.CustomerID == CustomerID && x.ReviewTypeID == item.ReviewTypeID).AsNoTracking().ToList();
                        var result = SystmerLoanTypeMaster.Where(p => !custReviewMapping.Any(p2 => p2.LoanTypeID == p.LoanTypeID)).ToList();
                        if (result.Count == 0)
                        {
                            systemReviewTypes.Remove(systemReviewTypes.Where(x => x.ReviewTypeID == item.ReviewTypeID).FirstOrDefault());
                        }
                    }
                }
            }
            else
            {
                systemReviewTypes = new List<ReviewTypeMaster>();
            }


            return systemReviewTypes;
        }

        public object GetCustReviewLoanTypes(Int64 customerID, Int64 reviewTypeID)
        {
            IntellaLendDataAccess ilAccess = new IntellaLendDataAccess();
            List<LoanTypeMaster> SystmerLoanTypeMaster = ilAccess.GetSystemLoanTypesByMapping(reviewTypeID);
            List<LoanTypeMaster> LoanTypeMaster = null;
            List<LoanTypeMaster> ClonedLoanTypes = null;

            if (SystmerLoanTypeMaster != null)
            {
                using (var db = new DBConnect(TableSchema))
                {
                    var reviewType = db.ReviewTypeMaster.AsNoTracking().Where(l => l.ReviewTypeID == reviewTypeID && l.Type == 0).FirstOrDefault();

                    if (reviewType != null)
                    {
                        var custReviewLoanTypes = db.CustReviewLoanMapping.Where(cl => cl.CustomerID == customerID && cl.ReviewTypeID == reviewType.ReviewTypeID).AsNoTracking().ToList();

                        foreach (var item in custReviewLoanTypes)
                        {
                            item.LoanTypeMaster = db.LoanTypeMaster.AsNoTracking().Where(rm => rm.LoanTypeID == item.LoanTypeID && rm.Type == 0).FirstOrDefault();
                        }

                        LoanTypeMaster = SystmerLoanTypeMaster.Where(x => !(custReviewLoanTypes.Any(y => x.LoanTypeName == y.LoanTypeMaster.LoanTypeName))).ToList();

                        ClonedLoanTypes = SystmerLoanTypeMaster.Where(x => (custReviewLoanTypes.Any(y => x.LoanTypeName == y.LoanTypeMaster.LoanTypeName))).ToList();
                    }
                    else
                    {
                        LoanTypeMaster = SystmerLoanTypeMaster;
                        ClonedLoanTypes = new List<LoanTypeMaster>();
                    }
                }
            }

            return new { UnMappedLoanTypes = LoanTypeMaster, MappedLoanTypes = ClonedLoanTypes };
        }

        public bool CloneFromSystem(Int64 customerID, Int64 reviewTypeID, Int64[] loanTypeIDs)
        {
            IntellaLendDataAccess ilAccess = new IntellaLendDataAccess();

            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    //Insert review                            
                    if (db.ReviewTypeMaster.AsNoTracking().Where(l => l.ReviewTypeID == reviewTypeID).FirstOrDefault() == null)
                    {
                        ReviewTypeMaster lm = ilAccess.GetSystemReviewType(reviewTypeID);
                        lm.ReviewTypeID = reviewTypeID;
                        lm.CreatedOn = DateTime.Now;
                        lm.ModifiedOn = DateTime.Now;
                        lm.Active = true;
                        db.ReviewTypeMaster.Add(lm);
                        db.SaveChanges();
                    }

                    //Customer Review Mapping
                    if (db.CustReviewMapping.AsNoTracking().Where(l => l.ReviewTypeID == reviewTypeID && l.CustomerID == customerID).FirstOrDefault() == null)
                    {
                        CustReviewMapping lm = new CustReviewMapping();
                        lm.CustReviewMappingID = 0;
                        lm.CustomerID = customerID;
                        lm.ReviewTypeID = reviewTypeID;
                        lm.CreatedOn = DateTime.Now;
                        lm.ModifiedOn = DateTime.Now;
                        lm.Active = true;
                        db.CustReviewMapping.Add(lm);
                        db.SaveChanges();
                    }

                    foreach (Int64 loanID in loanTypeIDs)
                    {
                        //Insert Loan Type     
                        if (db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeID == loanID).FirstOrDefault() == null)
                        {
                            LoanTypeMaster lm = ilAccess.GetSystemLoanTypes(loanID);
                            lm.LoanTypeID = loanID;
                            lm.CreatedOn = DateTime.Now;
                            lm.ModifiedOn = DateTime.Now;
                            lm.Active = true;
                            db.LoanTypeMaster.Add(lm);
                            db.SaveChanges();

                            //Insert Document Master
                            List<DocumentTypeMaster> docMaster = new IntellaLendDataAccess().GetSystemDocumentTypesWithFields(loanID);
                            foreach (DocumentTypeMaster doc in docMaster)
                            {

                                doc.DocumentTypeID = 0;
                                doc.Active = true;
                                doc.CreatedOn = DateTime.Now;
                                doc.ModifiedOn = DateTime.Now;
                                db.DocumentTypeMaster.Add(doc);
                                db.SaveChanges();

                                //Insert  Cust->Loan->Document Master
                                CustLoanDocMapping docLoanMap = new CustLoanDocMapping();
                                docLoanMap.ID = 0;
                                docLoanMap.CustomerID = customerID;
                                docLoanMap.LoanTypeID = loanID;
                                docLoanMap.DocumentTypeID = doc.DocumentTypeID;
                                docLoanMap.CreatedOn = DateTime.Now;
                                docLoanMap.ModifiedOn = DateTime.Now;
                                docLoanMap.Active = true;
                                db.CustLoanDocMapping.Add(docLoanMap);
                                db.SaveChanges();

                                //Insert Document Fields
                                foreach (var field in doc.DocumentFieldMasters)
                                {
                                    field.FieldID = 0;
                                    field.Active = true;
                                    field.DocumentTypeID = doc.DocumentTypeID;
                                    field.CreatedOn = DateTime.Now;
                                    field.ModifiedOn = DateTime.Now;
                                    db.DocumentFieldMaster.Add(field);
                                    db.SaveChanges();
                                }
                            }
                        }

                        //Customer->Review->loan Mapping
                        if (db.CustReviewLoanMapping.AsNoTracking().Where(l => l.ReviewTypeID == reviewTypeID && l.CustomerID == customerID && l.LoanTypeID == loanID).FirstOrDefault() == null)
                        {
                            CustReviewLoanMapping crl = new CustReviewLoanMapping();
                            crl.ID = 0;
                            crl.CustomerID = customerID;
                            crl.ReviewTypeID = reviewTypeID;
                            crl.LoanTypeID = loanID;
                            crl.Active = true;
                            crl.CreatedOn = DateTime.Now;
                            crl.ModifiedOn = DateTime.Now;
                            db.CustReviewLoanMapping.Add(crl);
                            db.SaveChanges();
                        }

                        //Check Lists
                        CheckListMaster sysCheckListMaster = new IntellaLendDataAccess().GetSystemCheckLists(loanID, reviewTypeID);
                        if (sysCheckListMaster != null)
                        {
                            //Insert  CheckList Master Table
                            CheckListMaster checkListMaster = new CheckListMaster();
                            checkListMaster.CheckListID = 0;
                            checkListMaster.CheckListName = sysCheckListMaster.CheckListName;
                            checkListMaster.Active = true;
                            checkListMaster.CreatedOn = DateTime.Now;
                            checkListMaster.ModifiedOn = DateTime.Now;
                            db.CheckListMaster.Add(checkListMaster);
                            db.SaveChanges();

                            if (sysCheckListMaster.CheckListDetailMasters != null)
                            {
                                foreach (CheckListDetailMaster sysCheckListDetailMasters in sysCheckListMaster.CheckListDetailMasters)
                                {
                                    //Insert  CheckList Master Table
                                    CheckListDetailMaster checkListDetailMaster = new CheckListDetailMaster();
                                    checkListDetailMaster.CheckListDetailID = 0;
                                    checkListDetailMaster.CheckListID = checkListMaster.CheckListID;
                                    checkListDetailMaster.Description = sysCheckListDetailMasters.Description;
                                    checkListDetailMaster.Active = true;
                                    checkListDetailMaster.Name = sysCheckListDetailMasters.Name;
                                    checkListDetailMaster.CreatedOn = DateTime.Now;
                                    checkListDetailMaster.ModifiedOn = DateTime.Now;
                                    db.CheckListDetailMaster.Add(checkListDetailMaster);
                                    db.SaveChanges();

                                    if (sysCheckListDetailMasters.RuleMasters != null)
                                    {
                                        RuleMaster ruleMaster = new RuleMaster();
                                        ruleMaster.RuleID = 0;
                                        ruleMaster.CheckListDetailID = checkListDetailMaster.CheckListDetailID;
                                        ruleMaster.RuleDescription = sysCheckListDetailMasters.RuleMasters.RuleDescription;
                                        ruleMaster.RuleJson = sysCheckListDetailMasters.RuleMasters.RuleJson;
                                        ruleMaster.DocumentType = sysCheckListDetailMasters.RuleMasters.DocumentType;
                                        ruleMaster.ActiveDocumentType = sysCheckListDetailMasters.RuleMasters.ActiveDocumentType;
                                        ruleMaster.Active = true;
                                        ruleMaster.CreatedOn = DateTime.Now;
                                        ruleMaster.ModifiedOn = DateTime.Now;
                                        db.RuleMaster.Add(ruleMaster);
                                        db.SaveChanges();
                                    }
                                }
                            }

                            //Insert  Cust->Review->Loan->CheckList
                            CustReviewLoanCheckMapping custReviewLoanCheckMap = new CustReviewLoanCheckMapping();
                            custReviewLoanCheckMap.ID = 0;
                            custReviewLoanCheckMap.CustomerID = customerID;
                            custReviewLoanCheckMap.ReviewTypeID = reviewTypeID;
                            custReviewLoanCheckMap.LoanTypeID = loanID;
                            custReviewLoanCheckMap.CheckListID = checkListMaster.CheckListID;
                            custReviewLoanCheckMap.CreatedOn = DateTime.Now;
                            custReviewLoanCheckMap.ModifiedOn = DateTime.Now;
                            custReviewLoanCheckMap.Active = true;
                            db.CustReviewLoanCheckMapping.Add(custReviewLoanCheckMap);
                            db.SaveChanges();
                        }


                        //Stacking Order
                        StackingOrderMaster sysStackingOrder = new IntellaLendDataAccess().GetSystemStackingOrderMaster(loanID, reviewTypeID);
                        if (sysStackingOrder != null)
                        {
                            //Insert  CheckList Master Table
                            StackingOrderMaster stackingOrder = new StackingOrderMaster();
                            stackingOrder.StackingOrderID = 0;
                            stackingOrder.Description = sysStackingOrder.Description;
                            stackingOrder.Active = true;
                            stackingOrder.CreatedOn = DateTime.Now;
                            stackingOrder.ModifiedOn = DateTime.Now;
                            db.StackingOrderMaster.Add(stackingOrder);
                            db.SaveChanges();


                            if (sysStackingOrder.StackingOrderDetailMasters != null)
                            {
                                foreach (StackingOrderDetailMaster sysStackingOrderDetail in sysStackingOrder.StackingOrderDetailMasters)
                                {
                                    Int64 DocumentTypeID = sysStackingOrderDetail.DocumentTypeID;

                                    List<CustLoanDocMapping> lsLoanDocMapping = db.CustLoanDocMapping.AsNoTracking().Where(c => c.LoanTypeID == loanID).ToList();

                                    foreach (var item in lsLoanDocMapping)
                                    {
                                        DocumentTypeMaster sysDocType = new IntellaLendDataAccess().GetSystemDocumentType(sysStackingOrderDetail.DocumentTypeID);

                                        DocumentTypeMaster docType = db.DocumentTypeMaster.AsNoTracking().Where(d => d.DocumentTypeID == item.DocumentTypeID).FirstOrDefault();

                                        if (docType != null)
                                        {
                                            if (sysDocType.Name.Equals(docType.Name))
                                                DocumentTypeID = docType.DocumentTypeID;
                                        }
                                    }

                                    //Insert  StackingOrder Detail Table
                                    StackingOrderDetailMaster stackingOrderDetail = new StackingOrderDetailMaster();
                                    stackingOrderDetail.StackingOrderDetailID = 0;
                                    stackingOrderDetail.StackingOrderID = stackingOrder.StackingOrderID;
                                    stackingOrderDetail.DocumentTypeID = DocumentTypeID;
                                    stackingOrderDetail.SequenceID = sysStackingOrderDetail.SequenceID;
                                    stackingOrderDetail.Active = true;
                                    stackingOrderDetail.CreatedOn = DateTime.Now;
                                    stackingOrderDetail.ModifiedOn = DateTime.Now;
                                    db.StackingOrderDetailMaster.Add(stackingOrderDetail);
                                    db.SaveChanges();
                                }
                            }

                            //Insert  Cust->Review->Loan->StackingOrder
                            CustReviewLoanStackMapping custReviewLoanStackMap = new CustReviewLoanStackMapping();
                            custReviewLoanStackMap.ID = 0;
                            custReviewLoanStackMap.CustomerID = customerID;
                            custReviewLoanStackMap.ReviewTypeID = reviewTypeID;
                            custReviewLoanStackMap.LoanTypeID = loanID;
                            custReviewLoanStackMap.StackingOrderID = stackingOrder.StackingOrderID;
                            custReviewLoanStackMap.CreatedOn = DateTime.Now;
                            custReviewLoanStackMap.ModifiedOn = DateTime.Now;
                            custReviewLoanStackMap.Active = true;
                            db.CustReviewLoanStackMapping.Add(custReviewLoanStackMap);
                            db.SaveChanges();
                        }
                    }

                    tran.Commit();
                }

                return true;
            }

        }

        #endregion

    }

}
