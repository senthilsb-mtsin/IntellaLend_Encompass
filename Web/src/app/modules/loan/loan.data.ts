import { Injectable } from '@angular/core';
import { APIService } from 'src/app/shared/service/api.service';
import { Observable } from 'rxjs';
import { MTSAPIResponse } from '@mts-api-response-model';
import { LoanApiUrlConstant } from 'src/app/shared/constant/api-url-constants/loan-api-url.constant';
import { LoanSearchApiUrlConstant } from '@mts-api-url';

@Injectable({ providedIn: 'root' })
export class LoanInfoDataAccess {
    constructor(private _apiService: APIService) { }

    GetLoanDetails(req: { TableSchema: string, LoanID: number }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_LOAN_DETAILS, req);
    }

    CheckLoanPDF(req: { TableSchema: string, LoanID: number }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.CHECK_LOAN_PDF, req);
    }

    RevertLoanComplete(req: { TableSchema: string, LoanID: number, UserName: string }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.REVERT_LOAN_COMPLETE, req);
    }

    GetLoanNotes(req: { TableSchema: string, LoanID: number }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_LOAN_NOTES, req);
    }

    DeletedReverification(req: { TableSchema: string, LoanID: number, LoanReverificationID: number, ReverificationName: string, UserName: string }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.DELETE_LOAN_REVERIFICATION, req);
    }

    GetUserMaster(req: { TableSchema: string }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_USER_MASTER, req);
    }

    SaveExportBatchDetails(req: { TableSchema: string, JobName: any, ExportedBy: any, CustomerID: any, CoverLetter: any, TableOfContent: any, PasswordProtected: any, Password: any, CoverLetterContent: any, BatchLoanDoc: any }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.SAVE_EXPORT_BATCH_DETAILS, req);
    }

    GetLoanStipulationDetails(req: { TableSchema: string, LoanID: number }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_LOAN_STIPULATIONS, req);
    }

    LoanComplete(req: {
        TableSchema: string, LoanID: number, CompletedUserRoleID: number,
        CompletedUserID: number,
        CompleteNotes: string
    }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.LOAN_COMPLETE, req);
    }

    UpdateLoanQuestioner(req: { TableSchema: string, LoanID: number, Questioners: any, CurrentUserID: any }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.UPDATE_LOAN_QUESTIONER, req);
    }

    GetEmailTrackerData(req: { TableSchema: string, LoanID: number }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_EMAIL_TRACKER_DATA, req);
    }

    GetLoanDocumentFieldValue(req: { TableSchema: string, LoanID: number, DocumentName: any, DocumentField: any }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_LOAN_DOCUMENT_FIELD, req);
    }

    DownloadReverification(req: any): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPostWithoutParse(LoanApiUrlConstant.DOWNLOAD_REVERIFICATION, req);
    }

    GetLoanBasedReverification(req: { TableSchema: string, LoanID: number }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_LOAN_BASED_REVERIFICATION, req);
    }

    GetReverification(req: { TableSchema: string, LoanID: number }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_LOAN_REVERIFICATION, req);
    }

    SaveStipulation(req: { TableSchema: string, LoanID: number, LoanStipulationDetails: any }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.SAVE_LOAN_STIPULATION, req);
    }

    UpdateStipulation(req: { TableSchema: string, LoanID: number, LoanStipulationDetails: any }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.UPDATE_LOAN_STIPULATION, req);
    }

    GetLoanAudit(req: { TableSchema: string, LoanID: number }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_LOAN_AUDIT, req);
    }

    GetEmailDetails(req: { TableSchema: string, ID: any }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_CURRENT_EMAIL_DATA, req);
    }

    RetryEmail(req: { TableSchema: string, ID: any }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.RETRY_EMAIL, req);
    }

    GetStipulationList(req: { TableSchema: string }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_STIPULATION_LIST, req);
    }

    UpdateLoanNotes(req: { TableSchema: string, LoanID: number, Notes: any }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.UPDATE_LOAN_NOTES, req);
    }

    DocumentObsolete(req: { TableSchema: string, LoanID: number, DocumentID: any, DocumentVersion: any, IsObsolete: boolean, CurrentUserID: number, DocName: string }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.DOCUMENT_OBESOLETE, req);
    }

    GetEphesoftURL(req: { TableSchema: string, EphesoftBatchInstanceID: string, EphesoftURL: string, CustomerID: number }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_EPHESOFT_URL, req);
    }

    GetMissingDocStatus(req: { TableSchema: string, LoanID: number }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_MISSING_DOC_STATUS, req);
    }

    GetChecklistDetails(req: { TableSchema: string, LoanID: number, ReRun: boolean }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_LOAN_EVAL_CHECKLIST, req);
    }

    GetMissingDocVersion(req: { TableSchema: string, LoanID: number, DocumentID: number }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_MISSING_DOC_VERSION, req);
    }

    SaveLoanHeader(req: { TableSchema: string, LoanID: number, LoanDetails: any, UserName: string }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.UPDATE_LOAN_HEADERS, req);
    }

    SaveAndRevaluate(req: { TableSchema: string, LoanID: any, DocumentID: any, CurrentUserID: any, DocumentTables: any, DocumentFields: any, VersionNumber: any }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.UPDATE_LOAN_DOCUMENT, req);
    }

    GetLoanDocInfo(req: { TableSchema: string, LoanID: number, DocumentID: number, VersionNumber: string }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_LOAN_DOC_INFO, req);
    }

    ChangeDocumentType(req: { TableSchema: string, LoanID: number, OldDocumentID: number, NewDocumentID: number, VersionNumber: any, CurrentUserID: any }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.CHANGE_DOCUMENT_TYPE, req);
    }

    GetLoanImages(req: { TableSchema: any, LoanID: any, DocumentID: any, ImageID: any, PageNo: any, VersionNumber: any, ShowAllDocs: any, LastPageNumber: any }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_LOAN_IMAGES, req);
    }

    SaveAuditDueDateHeader(req: { TableSchema: string, LoanID: number, AuditDueDate: string }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.UPDATE_LOAN_AUDIT_DUE_DATE, req);
    }

    GetAssignableUserList(req: { TableSchema: string, LoanID: number, ServiceTypeName: string }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.GET_ASSIGNABLE_USER_LIST, req);
    }

    GetLoanSearchFilterConfigValue(req: any): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanSearchApiUrlConstant.GET_LOAN_SEARCH_FILTER_CONFIG, req);
    }

    AssignLoan(req: { TableSchema: string, LoanID: number, ServiceTypeName: string }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.ASSIGN_LOAN, req);
    }

    getAuditReport(reqURL: string): Observable<any> {
        return this._apiService.authHttpGetWithoutParse(reqURL);
    }

    DownloadDocumentPDF(req: { TableSchema: string, LoanID: any, DocumentID: any, DocumentVersion: any }): Observable<any> {
        return this._apiService.authHttpPostWithoutParse(LoanApiUrlConstant.DOWNLOAD_DOCUMENT_PDF, req);
    }

    DownloadLoanReverification(req: { TableSchema: string, LoanReverificationID: any }): Observable<any> {
        return this._apiService.authHttpPostWithoutParse(LoanApiUrlConstant.DOWNLOAD_LOAN_REVERIFICATION, req);
    }

    TriggerEmail(req: { TableSchema: string, To: string, Subject: string, Body: string, Attachement: string, UserID: number, SendBy: string, LoanID: number, AttachmentsName: string }): Observable<MTSAPIResponse> {
        return this._apiService.authHttpPost(LoanApiUrlConstant.SEND_EMAIL_DETAILS, req);
    }
}
