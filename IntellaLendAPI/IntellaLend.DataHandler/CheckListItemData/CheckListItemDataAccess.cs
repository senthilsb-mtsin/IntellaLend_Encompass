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
    public class CheckListItemDataAccess
    {
        private static string TableSchema;

        #region Constructor

        public CheckListItemDataAccess() { }

        public CheckListItemDataAccess(string tableschema)
        {
            TableSchema = tableschema;
        }

        #endregion

        public CheckListDetailMaster CheckListDetails(CheckListDetailMaster checklistmaster, RuleMaster rulemasters)
        {
            using (var db = new DBConnect(TableSchema))
            {

                using (var trans = db.Database.BeginTransaction())
                {
                    db.CheckListDetailMaster.Add(checklistmaster);
                    db.SaveChanges();
                    rulemasters.CheckListDetailID = checklistmaster.CheckListDetailID;
                    db.RuleMaster.Add(rulemasters);
                    db.SaveChanges();
                    trans.Commit();
                }
            }
            return checklistmaster;
        }

        public object SearchCheckListItem(Int64 checkListDetailID)
        {
            object data;
            using (var db = new DBConnect(TableSchema))
            {
                var dData = (from cl in db.CheckListDetailMaster
                             join rm in db.RuleMaster on cl.CheckListDetailID equals rm.CheckListDetailID into rmJoin
                             from rmGroup in rmJoin.DefaultIfEmpty()
                             where cl.CheckListID == checkListDetailID
                             select new
                             {
                                 CheckListDetailID = cl.CheckListDetailID,
                                 ChecklistActive = cl.Active,
                                 RuleID = rmGroup.RuleID == null ? 0 : rmGroup.RuleID,
                                 ChecklistGroupId = cl.CheckListID,
                                 CheckListName = cl.Name,
                                 CheckListDescription = cl.Description,
                                 CreatedOn = cl.CreatedOn,
                                 RuleDescription = rmGroup.RuleDescription == null ? String.Empty : rmGroup.RuleDescription,
                                 RuleJson = rmGroup.RuleJson == null ? String.Empty : rmGroup.RuleJson,
                                 DocumentType = rmGroup.DocumentType == null ? String.Empty : rmGroup.DocumentType,
                                 UserID = cl.UserID
                             }).ToList();

                data = (from d in dData
                        join r in db.Users on d.UserID equals r.UserID into lu
                        from ul in lu.DefaultIfEmpty()
                        select new
                        {
                            CheckListDetailID = d.CheckListDetailID,
                            ChecklistActive = d.ChecklistActive,
                            RuleID = d.RuleID,
                            ChecklistGroupId = d.ChecklistGroupId,
                            CheckListName = d.CheckListName,
                            CheckListDescription = d.CheckListDescription,
                            CreatedOn = d.CreatedOn,
                            RuleDescription = d.RuleDescription,
                            RuleJson = d.RuleJson,
                            DocumentType = d.DocumentType,
                            UserID = d.UserID,
                            FirstName = ul?.FirstName ?? "System",
                            LastName = ul?.LastName ?? String.Empty                           
                        }).ToList();

            }
            return data;
        }

        public object CloneCheckListItem(Int64[] checkListDetailsID, string modifiedCheckListName)
        {
            using (var db = new DBConnect(TableSchema))
            {
                foreach (var clonechecklistid in checkListDetailsID)
                {
                    CheckListDetailMaster checklistdetials = db.CheckListDetailMaster.AsNoTracking().Where(c => c.CheckListDetailID == clonechecklistid).FirstOrDefault();
                    if (checklistdetials != null)
                    {
                        RuleMaster rm = db.RuleMaster.AsNoTracking().Where(r => r.CheckListDetailID == checklistdetials.CheckListDetailID).FirstOrDefault();
                        checklistdetials.Name = modifiedCheckListName;
                        checklistdetials.CheckListDetailID = 0;
                        db.CheckListDetailMaster.Add(checklistdetials);
                        db.SaveChanges();
                        rm.CheckListDetailID = checklistdetials.CheckListDetailID;
                        rm.RuleID = 0;
                        db.RuleMaster.Add(rm);
                        db.SaveChanges();
                    }
                    //string serializeData = JsonConvert.SerializeObject(checklistdetials); Include(c => c.RuleMasters).AsNoTracking().Where(c => c.CheckListDetailID == clonechecklistid).FirstOrDefault();
                    //CheckListDetailMaster cm = JsonConvert.DeserializeObject<CheckListDetailMaster>(serializeData);

                }
                return true;
            }
            return false;
        }

        public object DeleteCheckListItem(Int64[] checkListDetailsID)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    foreach (var checklist in checkListDetailsID)
                    {
                        RuleMaster rulemaster = db.RuleMaster.AsNoTracking().Where(r => r.CheckListDetailID == checklist).FirstOrDefault();

                        if (rulemaster != null)
                        {
                            db.Entry(rulemaster).State = EntityState.Deleted;
                            db.RuleMaster.Remove(rulemaster);
                            db.SaveChanges();
                        }

                        CheckListDetailMaster checklistmaster = db.CheckListDetailMaster.AsNoTracking().Where(ch => ch.CheckListDetailID == checklist).FirstOrDefault();

                        if (checklistmaster != null)
                        {
                            db.Entry(checklistmaster).State = EntityState.Deleted;
                            db.CheckListDetailMaster.Remove(checklistmaster);
                            db.SaveChanges();
                        }
                    }
                    trans.Commit();
                    return true;
                }

            }
            return false;
        }

        public object UpdateCheckListDetails(CheckListDetailMaster checkListDetailMaster, RuleMaster rulemasters)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    CheckListDetailMaster updatechecklistdetailsmaster = db.CheckListDetailMaster.AsNoTracking().Where(up => up.CheckListDetailID == checkListDetailMaster.CheckListDetailID).FirstOrDefault();
                    if (updatechecklistdetailsmaster != null)
                    {
                        updatechecklistdetailsmaster.Name = checkListDetailMaster.Name;
                        updatechecklistdetailsmaster.Description = checkListDetailMaster.Description;
                        updatechecklistdetailsmaster.Active = checkListDetailMaster.Active;
                        updatechecklistdetailsmaster.ModifiedOn = DateTime.Now;
                        db.Entry(updatechecklistdetailsmaster).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    RuleMaster updaterulemasters = db.RuleMaster.AsNoTracking().Where(upd => upd.RuleID == rulemasters.RuleID).FirstOrDefault();
                    if (updaterulemasters != null)
                    {
                        updaterulemasters.RuleDescription = rulemasters.RuleDescription;
                        updaterulemasters.RuleJson = rulemasters.RuleJson;
                        updaterulemasters.Active = rulemasters.Active;
                        updaterulemasters.DocumentType = rulemasters.DocumentType;
                        updaterulemasters.ActiveDocumentType = rulemasters.ActiveDocumentType;
                        db.Entry(updaterulemasters).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    trans.Commit();
                    return true;
                }
            }
            return false;
        }
    }
}
