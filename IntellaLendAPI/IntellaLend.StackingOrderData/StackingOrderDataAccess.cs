using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IntellaLend.EntityDataHandler
{
    public class StackingOrderDataAccess
    {
        private string TableSchema;
        public StackingOrderDataAccess() { }
        public StackingOrderDataAccess(string tableSchema)
        {
            TableSchema = tableSchema;
        }

        public object SearchStackingOrder(long stackingOrderID)
        {
            object data;
            List<StackingOrderGroupmasters> stackGroup = new List<StackingOrderGroupmasters>();
            stackGroup.Add(new StackingOrderGroupmasters { Active = false });
            using (var db = new DBConnect(TableSchema))
            {
                data = (from so in db.StackingOrderDetailMaster
                        join dm in db.DocumentTypeMaster on so.DocumentTypeID equals dm.DocumentTypeID
                        where so.StackingOrderID == stackingOrderID
                        select new
                        {
                            StackingOrderDetailID = so.StackingOrderDetailID,
                            StackingOrderID = so.StackingOrderID,
                            DocumentTypeID = so.DocumentTypeID,
                            SequenceID = so.SequenceID,
                            DocumentTypeName = dm.DisplayName,
                            DocFieldList = db.DocumentFieldMaster.Where(d => d.DocumentTypeID == so.DocumentTypeID).ToList(),
                            OrderByFieldID = db.DocumentFieldMaster.Where(d => d.DocumentTypeID == so.DocumentTypeID && d.DocOrderByField != null).Select(f => f.FieldID).FirstOrDefault(),
                            DocFieldValueId = db.DocumentFieldMaster.Where(d => d.DocumentTypeID == so.DocumentTypeID && d.IsDocName == true).Select(f => f.FieldID).FirstOrDefault(),
                            isGroup = db.StackingOrderGroupmasters.Where(sog => sog.StackingOrderGroupID == so.StackingOrderGroupID).ToList().Count == 0 ? false : true,
                            StackingOrderGroupDetails = db.StackingOrderGroupmasters.Where(sog => sog.StackingOrderGroupID == so.StackingOrderGroupID).ToList()

                        }).OrderBy(s => s.SequenceID).ToList();
            }
            return data;
        }

        public bool SetTenantOrderByField(Int64 DocumentTypeID, Int64 FieldID)
        {
            bool result = false;
            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    //DocumentFieldMaster docField = db.DocumentFieldMaster.AsNoTracking().Where(d => d.FieldID == FieldID).FirstOrDefault();

                    //if (docField != null)
                    //{
                    List<DocumentFieldMaster> _docFields = db.DocumentFieldMaster.AsNoTracking().Where(d => d.DocumentTypeID == DocumentTypeID).ToList();

                    _docFields.ForEach(ele =>
                    {
                        if (ele.FieldID == FieldID)
                        {
                            ele.DocOrderByField = "Desc";
                        }
                        else
                        {
                            ele.DocOrderByField = null;
                        }
                        db.Entry(ele).State = EntityState.Modified;
                        db.SaveChanges();
                    });

                    //docField.DocOrderByField = "Desc";
                    //db.Entry(docField).State = EntityState.Modified;
                    //db.SaveChanges();
                    tran.Commit();
                    result = true;
                    //}
                }
            }

            return result;
        }

        public object SaveStackingOrderDetails(int stackOrderID, List<GetStackOrder> stackingOrderDetails)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    db.StackingOrderDetailMaster.RemoveRange(db.StackingOrderDetailMaster.Where(sodm => sodm.StackingOrderID == stackOrderID));
                    db.StackingOrderGroupmasters.RemoveRange(db.StackingOrderGroupmasters.Where(sogm => sogm.StackingOrderID == stackOrderID));
                    db.SaveChanges();
                    string dupName = "";
                    StackingOrderGroupmasters stackOrderGrp = null;
                    int sequence = 1;
                    foreach (var stackingoderdetail in stackingOrderDetails)
                    {
                        //stackOrderGrp = null;
                        
                        //StackingOrderDetailMaster stackorder = db.StackingOrderDetailMaster.AsNoTracking().Where(sodm => sodm.StackingOrderDetailID == stackingOrderDetails.StackingOrderDetailID)
                        if (stackingoderdetail.isGroup && (stackingoderdetail.Name != dupName))
                        {
                            stackOrderGrp = null;
                            dupName = stackingoderdetail.Name;
                            stackOrderGrp = db.StackingOrderGroupmasters.Add(new StackingOrderGroupmasters()
                            {
                                StackingOrderID = stackOrderID,
                                StackingOrderGroupName = stackingoderdetail.Name,
                                Active = true,
                                GroupSortField = stackingoderdetail.StackingOrderFieldName,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now
                            });
                            db.SaveChanges();
                        }

                        db.StackingOrderDetailMaster.Add(new StackingOrderDetailMaster()
                        {
                            DocumentTypeID = stackingoderdetail.ID,
                            StackingOrderID = stackOrderID,
                            StackingOrderGroupID = stackOrderGrp!=null && stackOrderGrp.StackingOrderGroupID > 0  && stackingoderdetail.isGroup ? stackOrderGrp.StackingOrderGroupID : 0,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now,
                            SequenceID = sequence,
                            Active = true,
                        });

                        db.SaveChanges();
                        sequence++;
                    }
                    trans.Commit();
                    return true;
                }
                return false;
            }
        }

        public bool SetTenantDocFielValue(Int64 DocumentTypeID, Int64 FieldID)
        {
            bool result = false;
            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    List<DocumentFieldMaster> _docFields = db.DocumentFieldMaster.AsNoTracking().Where(d => d.DocumentTypeID == DocumentTypeID).ToList();

                    _docFields.ForEach(ele =>
                    {
                        if (ele.FieldID == FieldID)
                        {
                            ele.IsDocName = true;
                        }
                        else
                        {
                            ele.IsDocName = false;
                        }
                        db.Entry(ele).State = EntityState.Modified;
                        db.SaveChanges();
                    });
                    tran.Commit();
                    result = true;
                }
            }

            return result;
        }

        public bool SetTenantDocGroupFieldValue(GetStackOrder StackGroupDetails, List<StackingOrderDocumentFieldMaster> StackingOrderDetails)
        {
            bool result = false;

            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    StackingOrderGroupmasters _stackgroupdetail = db.StackingOrderGroupmasters.AsNoTracking().Where(x => x.StackingOrderGroupName.Trim() == StackGroupDetails.Name.Trim()).FirstOrDefault();
                    if (_stackgroupdetail != null)
                    {
                        _stackgroupdetail.GroupSortField = StackGroupDetails.StackingOrderFieldName;
                        _stackgroupdetail.ModifiedOn = DateTime.Now;
                        db.Entry(_stackgroupdetail).State = EntityState.Modified;
                        db.SaveChanges();
                        foreach (StackingOrderDocumentFieldMaster _stackingorder in StackingOrderDetails)
                        {
                            List<DocumentFieldMaster> _docFields = db.DocumentFieldMaster.AsNoTracking().Where(d => d.DocumentTypeID == _stackingorder.DocumentTypeID).ToList();
                            _docFields.ForEach(ele =>
                            {
                                if (ele.Name.Trim() == StackGroupDetails.StackingOrderFieldName.Trim())
                                    ele.DocOrderByField = "Desc";
                                else
                                    ele.DocOrderByField = null;
                                db.Entry(ele).State = EntityState.Modified;
                                db.SaveChanges();
                            });
                        }
                    }
                    tran.Commit();
                    result = true;
                }

            }

            return result;
        }
    }
}
