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
                            DocumentTypeName = dm.DisplayName
                        }

                        ).OrderBy(or=> or.SequenceID).ToList();

            }
            return data;
        }

        public object SaveStackingOrderDetails(int stackOrderID,List<StackingOrderDetailMaster> stackingOrderDetails)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    db.StackingOrderDetailMaster.RemoveRange(db.StackingOrderDetailMaster.Where(sodm => sodm.StackingOrderID == stackOrderID));
                    db.SaveChanges();

                    int sequence = 1;
                    foreach (var stackingoderdetail in stackingOrderDetails)
                    {
                        //StackingOrderDetailMaster stackorder = db.StackingOrderDetailMaster.AsNoTracking().Where(sodm => sodm.StackingOrderDetailID == stackingOrderDetails.StackingOrderDetailID)
                        db.StackingOrderDetailMaster.Add(new StackingOrderDetailMaster()
                        {
                            DocumentTypeID = stackingoderdetail.DocumentTypeID,
                            StackingOrderID = stackingoderdetail.StackingOrderID,
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
    }
}
