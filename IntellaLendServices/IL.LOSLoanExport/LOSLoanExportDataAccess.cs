using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntBlocks.LoggerBlock;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace IL.LOSLoanExport
{
    public class LOSLoanExportDataAccess
    {
        public static string SystemSchema = "IL";
        public string TenantSchema;

        public LOSLoanExportDataAccess(string tenantSchema)
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

        public List<LOSExportFileStaging> GetLOSLoanStagingFiles()
        {
            List<LOSExportFileStaging> fileStagings = null;
            using (var db = new DBConnect(TenantSchema))
            {
                fileStagings = db.LOSExportFileStaging.AsNoTracking().Where(fs => fs.Status == LOSExportStatusConstant.LOS_LOAN_STAGED && fs.FileType == LOSExportFileTypeConstant.LOS_LOAN_EXPORT).ToList();
            }
            return fileStagings;
        }

        public string GetLoanInfo(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();

                if (_loan != null)
                    return _loan.LoanNumber;
            }
            return string.Empty;
        }

        public void UpdateLoanCompleteUserDetails(Int64 LoanID, Int64 completeUserRoleID, Int64 completeUserID, string CompleteNotes)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                _loan.CompletedUserID = completeUserID;
                _loan.CompletedUserRoleID = completeUserRoleID;
                _loan.CompleteNotes = CompleteNotes;
                _loan.Status = StatusConstant.COMPLETE;
                _loan.ModifiedOn = DateTime.Now;
                _loan.AuditCompletedDate = DateTime.Now;
                db.Entry(_loan).State = EntityState.Modified;
                db.SaveChanges();

                User _user = db.Users.AsNoTracking().Where(u => u.UserID == completeUserID).FirstOrDefault();
                string _userName = _user != null ? $"{_user.LastName} {_user.FirstName}" : "";
                string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.COMPLETED_BY);


                LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0].Replace(AuditConfigConstant.USERNAME, _userName), auditDescs[1].Replace(AuditConfigConstant.USERNAME, _userName));

                LoanSearch loanSearch = db.LoanSearch.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                if (loanSearch != null)
                {
                    loanSearch.Status = _loan.Status;
                    loanSearch.ModifiedOn = DateTime.Now;
                    db.Entry(loanSearch).State = EntityState.Modified;
                    db.SaveChanges();

                    string[] auditDescsearch = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.LOAN_STATUS_MODIFIED);

                    LoanAudit.InsertLoanSearchAudit(db, loanSearch, auditDescsearch[0], auditDescsearch[1]);
                }
            }
        }

        public bool DeleteOutputFolder(long[] loanID, string ephesoftPath)
        {
            bool result = false;
            using (var db = new DBConnect(TenantSchema))
            {
                foreach (var Loan_ID in loanID)
                {
                    List<string> batchIDs = db.AuditIDCFields.AsNoTracking().Where(x => x.LoanID == Loan_ID).Select(x => x.IDCBatchInstanceID).Distinct().ToList();

                    foreach (var batchID in batchIDs)
                    {
                        if (!String.IsNullOrEmpty(batchID))
                        {
                            String[] dirPath = Directory.GetDirectories(ephesoftPath, batchID, SearchOption.AllDirectories);
                            foreach (var item in dirPath)
                            {
                                if (Directory.Exists(item))
                                {
                                    Directory.Delete(item, true);
                                    result = true;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public void UpdateLOSLoanStagingDetailStatus(Int64 _fileStagingID, Int32 _status, string _errorMessage = "")
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LOSExportFileStagingDetail _stage = db.LOSExportFileStagingDetail.AsNoTracking().Where(fs => fs.ID == _fileStagingID).FirstOrDefault();

                if (_stage != null)
                {
                    _stage.Status = _status;
                    _stage.ModifiedOn = DateTime.Now;
                    _stage.ErrorMsg = _errorMessage;
                    db.Entry(_stage).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public List<LOSExportFileStagingDetail> UpdateLOSLoanStagingFileStatus(Int64 _fileStagingID, Int32 _status, string _errorMessage = "")
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Logger.WriteTraceLog($"Start UpdateLOSStagingFileStatus() fileStagingID : {_fileStagingID}, status : {_status}, errorMessage : {_errorMessage} ");
                LOSExportFileStaging _stagingFile = db.LOSExportFileStaging.AsNoTracking().Where(fs => fs.ID == _fileStagingID).FirstOrDefault();

                if (_stagingFile != null)
                {
                    _stagingFile.ErrorMsg = _errorMessage;
                    _stagingFile.Status = _status;
                    _stagingFile.ModifiedOn = DateTime.Now;

                    db.Entry(_stagingFile).State = EntityState.Modified;
                    db.SaveChanges();


                    List<LOSExportFileStagingDetail> _stagingDocs = db.LOSExportFileStagingDetail.AsNoTracking().Where(fs => fs.LOSExportFileStagingID == _fileStagingID).ToList();
                    LoanDetail _loanDetail = db.LoanDetail.AsNoTracking().Where(x => x.LoanID == _stagingFile.LoanID).FirstOrDefault();
                    List<LOSExportFileStagingDetail> _tempStagingDocs = new List<LOSExportFileStagingDetail>();
                    if (_loanDetail != null)
                    {
                        Batch batchObj = JsonConvert.DeserializeObject<Batch>(_loanDetail.LoanObject);
                        List<Documents> batchDocs = batchObj.Documents.Where(x => x.Obsolete == false).ToList();

                        foreach (Documents doc in batchDocs)
                        {
                            LOSExportFileStagingDetail _stage = _stagingDocs.Where(s => doc.DocumentTypeID == s.DocID && doc.VersionNumber == s.Version && s.FileType == LOSExportFileTypeConstant.LOS_LOAN_DOC_EXPORT).FirstOrDefault();

                            if (_stage == null)
                            {

                                LOSExportFileStagingDetail _stageDetail = new LOSExportFileStagingDetail()
                                {
                                    LOSExportFileStagingID = _fileStagingID,
                                    LoanID = _stagingFile.LoanID,
                                    DocID = doc.DocumentTypeID,
                                    Version = Convert.ToInt64(doc.VersionNumber),
                                    FileType = LOSExportFileTypeConstant.LOS_LOAN_DOC_EXPORT,
                                    Status = LOSExportStatusConstant.LOS_LOAN_STAGED,
                                    CreatedOn = DateTime.Now,
                                    ModifiedOn = DateTime.Now,
                                    ErrorMsg = string.Empty,
                                    FileName = (doc.DocumentTypeID == 0) ? $"{doc.Description}_{doc.DocumentTypeID}_V{doc.VersionNumber}.pdf" : $"{doc.Type}_{doc.DocumentTypeID}_V{doc.VersionNumber}.pdf",
                                    Pages = doc.Pages.Select(x => x.Replace("PG", "")).ToList().ConvertAll(int.Parse)
                                };
                                db.LOSExportFileStagingDetail.Add(_stageDetail);
                                db.SaveChanges();
                                _tempStagingDocs.Add(_stageDetail);
                            }
                            else if (_stage.Status != LOSExportStatusConstant.LOS_LOAN_PROCESSED)
                            {
                                _stage.Status = LOSExportStatusConstant.LOS_LOAN_STAGED;
                                _stage.ModifiedOn = DateTime.Now;
                                _stage.ErrorMsg = string.Empty;
                                _stage.Pages = doc.Pages.Select(x => x.Replace("PG", "")).ToList().ConvertAll(int.Parse);
                                db.Entry(_stage).State = EntityState.Modified;
                                db.SaveChanges();
                                _tempStagingDocs.Add(_stage);
                            }
                        }

                        LOSExportFileStagingDetail jsonStage = _stagingDocs.Where(s => s.DocID == 0 && s.FileType == LOSExportFileTypeConstant.LOS_LOAN_JSON_EXPORT).FirstOrDefault();

                        if (jsonStage == null)
                        {
                            LOSExportFileStagingDetail _stageDetail = new LOSExportFileStagingDetail()
                            {
                                LOSExportFileStagingID = _fileStagingID,
                                LoanID = _stagingFile.LoanID,
                                DocID = 0,
                                Version = 1,
                                FileType = LOSExportFileTypeConstant.LOS_LOAN_JSON_EXPORT,
                                Status = LOSExportStatusConstant.LOS_LOAN_STAGED,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                ErrorMsg = string.Empty,
                                FileName = _stagingFile.FileName
                            };
                            db.LOSExportFileStagingDetail.Add(_stageDetail);
                            db.SaveChanges();
                            _tempStagingDocs.Add(_stageDetail);
                        }
                        else
                        {
                            jsonStage.Status = LOSExportStatusConstant.LOS_LOAN_STAGED;
                            jsonStage.ModifiedOn = DateTime.Now;
                            jsonStage.ErrorMsg = string.Empty;
                            db.Entry(jsonStage).State = EntityState.Modified;
                            db.SaveChanges();
                            _tempStagingDocs.Add(jsonStage);
                        }
                    }

                    return _tempStagingDocs.Where(x => x.Status == LOSExportStatusConstant.LOS_LOAN_STAGED).ToList();
                }
                Logger.WriteTraceLog($"End UpdateLOSStagingFileStatus() fileStagingID : {_fileStagingID}, status : {_status}, errorMessage : {_errorMessage} ");
            }

            return new List<LOSExportFileStagingDetail>();
        }
    }
}
