using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;

namespace IntellaLend.Audit
{
    public class LoanAudit
    {

        public static void InsertLoanAudit(DBConnect db, Loan loan, string auditDesc, string systemDesc)
        {
            db.AuditLoan.Add(new AuditLoan()
            {
                AuditDescription = auditDesc,
                SystemAuditDescription = systemDesc,
                AuditDateTime = DateTime.Now,
                LoanID = loan.LoanID,
                ReviewTypeID = loan.ReviewTypeID,
                LoanTypeID = loan.LoanTypeID,
                CustomerID = loan.CustomerID,
                UploadedUserID = loan.UploadedUserID,
                LoggedUserID = loan.LoggedUserID,
                LoanCreatedOn = loan.CreatedOn,
                LoanLastModifiedOn = loan.ModifiedOn,
                FileName = loan.FileName,
                Status = loan.Status,
                SubStatus = loan.SubStatus,
                LoanNumber = loan.LoanNumber,
                LastAccessedUserID = loan.LastAccessedUserID,
                Notes = loan.Notes,
                //     EphesoftBatchInstanceID = loan.EphesoftBatchInstanceID,
                //    EphesoftOCRAccuracy = loan.EphesoftOCRAccuracy,
                //FromBox = loan.FromBox,
                UploadType = loan.UploadType,
                //     EphesoftStatusID = loan.EphesoftStatusID,
                AuditMonthYear = loan.AuditMonthYear
            });

            db.SaveChanges();
        }

        public static void InsertLoanIDCFieldAudit(DBConnect db, IDCFields loan, string auditDesc, string systemDesc)
        {
            db.AuditIDCFields.Add(new AuditIDCFields()
            {
                AuditDescription = auditDesc,
                AuditDateTime = DateTime.Now,
                LoanID = loan.LoanID,
                ClassificationAccuracy = loan.ClassificationAccuracy,
                Createdon = loan.Createdon,
                IDCBatchInstanceID = loan.IDCBatchInstanceID,
                IDCCompletionDate = loan.IDCCompletionDate,
                IDCExtractionAccuracy = loan.IDCExtractionAccuracy,
                IDCFileRemovedStatus = loan.IDCFileRemovedStatus,
                IDCLevelOneCompletionDate = loan.IDCLevelOneCompletionDate,
                IDCLevelOneDuration = loan.IDCLevelOneDuration,
                IDCLevelTwoCompletionDate = loan.IDCLevelTwoCompletionDate,
                IDCLevelTwoDuration = loan.IDCLevelTwoDuration,
                IDCOCRAccuracy = loan.IDCOCRAccuracy,
                IDCReviewerName = loan.IDCReviewerName,
                IDCStatusID = loan.IDCStatusID,
                IDCValidatorName = loan.IDCValidatorName,
                ModifiedOn = loan.ModifiedOn,
                OCRAccuracyCalculated = loan.OCRAccuracyCalculated,
                IDCBatchClassID = loan.IDCBatchClassID,
                IDCBatchClassName = loan.IDCBatchClassName
            });

            db.SaveChanges();
        }


        public static void InsertLoanSearchAudit(DBConnect db, LoanSearch loanSearch, string auditDesc, string systemDesc)
        {
            db.AuditLoanSearch.Add(new AuditLoanSearch()
            {
                LoanSearchID = loanSearch.ID,
                LoanID = loanSearch.LoanID,
                BorrowerName = loanSearch.BorrowerName,
                LoanAmount = loanSearch.LoanAmount,
                LoanNumber = loanSearch.LoanNumber,
                LoanTypeID = loanSearch.LoanTypeID,
                ReceivedDate = loanSearch.ReceivedDate,
                SSN = loanSearch.SSN,
                Status = loanSearch.Status,
                CustomerID = loanSearch.CustomerID,
                AuditDateTime = DateTime.Now,
                AuditDescription = auditDesc,
                SystemAuditDescription = systemDesc
            });

            db.SaveChanges();
        }

        public static void InsertLoanDetailsAudit(DBConnect db, LoanDetail loanDetails, Int64 UpdatedUserID, string auditDesc, string systemDesc)
        {
            db.AuditLoanDetail.Add(new AuditLoanDetail()
            {
                AuditDescription = auditDesc,
                SystemAuditDescription = systemDesc,
                AuditDateTime = DateTime.Now,
                LoanID = loanDetails.LoanID,
                LoanDetailID = loanDetails.LoanDetailID,
                LoanObject = loanDetails.LoanObject,
                UpdatedUserID = UpdatedUserID,
                ManualQuestioners = loanDetails.ManualQuestioners,
                TotalDocCount = loanDetails.TotalDocCount,
                MissingDocCount = loanDetails.MissingDocCount,
                MissingCriticalDocCount = loanDetails.MissingCriticalDocCount,
                MissingNonCriticalDocCount = loanDetails.MissingNonCriticalDocCount
            });

            db.SaveChanges();
        }

        public static void InsertLoanMissingDocAudit(DBConnect db, Dictionary<string, object> missingDocAuditInfo, Int64 StatusID, string auditDesc, string systemDesc)
        {
            if (missingDocAuditInfo.ContainsKey("LOANID") && missingDocAuditInfo.ContainsKey("DOCID") && missingDocAuditInfo.ContainsKey("USERID"))
            {
                db.AuditLoanMissingDoc.Add(new AuditLoanMissingDoc()
                {
                    AuditDescription = auditDesc,
                    SystemAuditDescription = systemDesc,
                    AuditDateTime = DateTime.Now,
                    LoanID = Convert.ToInt64(missingDocAuditInfo["LOANID"]),
                    DocID = Convert.ToInt64(missingDocAuditInfo["DOCID"]),
                    UserID = Convert.ToInt64(missingDocAuditInfo["USERID"]),
                    FileName = Convert.ToString(missingDocAuditInfo["FILENAME"]),
                    Status = StatusID,
                    ModifiedOn = DateTime.Now,
                    IDCBatchInstanceID = string.Empty,
                    EDownloadStagingID = missingDocAuditInfo.ContainsKey("EDOWNLOADSTAGINGID") ? Convert.ToInt64(missingDocAuditInfo["EDOWNLOADSTAGINGID"]) : 0
                });

                db.SaveChanges();
            }
        }

    }
}
