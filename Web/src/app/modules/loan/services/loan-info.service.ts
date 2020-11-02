
import { NotificationService } from '@mts-notification';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoanInfoDataAccess } from '../loan.data';
import { LoanHeaders, StipulationDetails } from '../models/loan-header.model';
import { Injectable } from '@angular/core';
import { LoanSearchTableModel } from '../../loansearch/models/loan-search-table.model';
import { Subject, BehaviorSubject, Observable } from 'rxjs';
import { AppSettings, EphesoftStatusConstant } from '@mts-app-setting';
import { SessionHelper } from '@mts-app-session';
import { AssignLoanUserRequest } from '../models/assign-loan-user-request.model';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { EmailCheckPipe } from '@mts-pipe';
import { environment } from 'src/environments/environment';
import { StatusConstant } from '@mts-status-constant';
import { DomSanitizer } from '@angular/platform-browser';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Loan } from '../models/loan-details.model';
import { ILoanNote, LoanAudit } from '../models/loan-details-sub.model';
import { LoanReverificationMappingModel } from '../models/loan-reverification-mapping.model';
import { LoanTemplateField } from '../models/loan-template-fields.model';

const jwtHelper = new JwtHelperService();

@Injectable({ providedIn: 'root' })
export class LoanInfoService {

    AssignUserList$ = new Subject<any[]>();
    EnabledAssignUser$ = new Subject<boolean>();
    ShowDocumentDetailView$ = new Subject<boolean>();
    RevifyShowDocumentDetailView$ = new Subject<boolean>();
    SetFieldDrawBox$ = new Subject<{ checklistState: boolean, pageData: { cords: any, pageNo: any } }>();
    FIELDToggle$ = new Subject<boolean>();
    SOPopToggle$ = new Subject<boolean>();
    STACKToggle$ = new Subject<boolean>();
    SOToggle$ = new Subject<boolean>();
    ImageFit$ = new Subject<boolean>();
    SO_width$ = new Subject<string>();

    _mappingTemplate$ = new Subject<any>();
    mentionDropOptions$ = new Subject<{ mentionDocTypes: any, mentionDocFields: any }>();

    AuditReportBlog$ = new Subject<any>();
    ReverificationBlog$ = new Subject<any>();
    DocumentDownloadBlog$ = new Subject<any>();
    newDocTypeID$ = new Subject<any>();
    EmailConfirmModal$ = new Subject<boolean>();
    EmailRetryModal$ = new Subject<boolean>();
    AuditEmailConfirmModal$ = new Subject<boolean>();
    LoanDetails$ = new BehaviorSubject<Loan>(null);
    LoanDetailsSub$ = new Subject<Loan>();
    loanFilterResult$ = new Subject<any>();
    LoanPDFCheck$ = new Subject<{ ShowDownload: boolean, PDFLink: string }>();
    showImgDiv$ = new Subject<boolean>();
    documentObsoleteModal$ = new Subject<boolean>();
    LoanNotes$ = new Subject<ILoanNote[]>();
    usernameNotes$ = new Subject<any[]>();
    notesTxtArea$ = new Subject<string>();
    StipulationsTable$ = new Subject<StipulationDetails[]>();
    StipulationCategoryNames$ = new Subject<{ id: any, text: any }[]>();
    StipulationModal$ = new Subject<boolean>();
    AuditTableData$ = new Subject<LoanAudit[]>();
    EmailTrackerTableData$ = new Subject<any[]>();
    EmailModalData$ = new Subject<any>();
    LoanReverificationMasters$ = new Subject<any[]>();
    LoanReverification$ = new Subject<{ Download: boolean, LoanReverificationID: any, RevericationName: any, UserRevericationName: any }[]>();
    confirmReverifyDeleteModal$ = new Subject<boolean>();
    disableCompleteAudit$ = new BehaviorSubject<boolean>(null);
    checkListArryObj$ = new Subject<{ checkListArryObj: any[], loanManualQuestioner: any[] }>();
    enableQuestionerSave$ = new Subject<number>();
    isAuditComplete$ = new BehaviorSubject<boolean>(null);
    isRevertToReadyForAudit$ = new BehaviorSubject<boolean>(null);
    revertToReadyforAuditModal$ = new Subject<boolean>();
    multiImageSource$ = new Subject<any[]>();
    leftImageSource$ = new Subject<any[]>();
    confirmChangeDocModal$ = new Subject<boolean>();
    ChangeDocumentType$ = new Subject<any>();
    SaveAndRevaluate$ = new Subject<any>();
    CompleteLoan$ = new Subject<any>();
    SetDocField$ = new BehaviorSubject<{ DocLevelFields: any, TempDocTables: any }>(null);
    LoanExportBack$ = new Subject<boolean>();

    LoanPopOutSOHeight$ = new Subject<string>();
    LoanPopOutFieldHeight$ = new Subject<string>();
    LoanPopOutImageViewerHeight$ = new Subject<string>();
    LoanReverifyImageViewerHeight$ = new Subject<string>();
    changeField$ = new Subject<{ fieldCount: any, returnFieldCount: any }>();

    constructor(
        private _loanInfoDataAccess: LoanInfoDataAccess,
        private _notificationService: NotificationService,
        private _emailPipe: EmailCheckPipe,
        private _sanitizer: DomSanitizer
    ) { }

    private _stipulationsTable: StipulationDetails[] = [];
    private _loanHeaderInfo: LoanHeaders = new LoanHeaders();
    private _loanData: LoanSearchTableModel;
    private _assignUserList: { UserID: number, UserName: string, AssignedUserID: number }[] = [];
    private _loanAPIResponse: Loan = null;
    private _userNames: any[] = [];
    private _currentDoc: { DocID: any, VersionNumber: any, lastPageNumber: any, pageNumberArray: any[], _currentDocName: any, _totalImageCount: any, checkListState: any };
    private fieldFrmGrp: FormGroup;
    private _currentForm: { datatables: any, fields: any } = { datatables: {}, fields: {} };
    private LoanDocuments: any[] = [];
    private ReverificationMaster: any[] = [];
    private _templateFields: LoanTemplateField = new LoanTemplateField();
    private CompleteNotes: any = '';
    SetNotesValue(completeNotes: any) {
        this.CompleteNotes = completeNotes;
    }
    CompleteLoan() {
        const req = {
            TableSchema: AppSettings.TenantSchema,
            LoanID: this._loanAPIResponse.LoanID,
            CompletedUserRoleID: SessionHelper.RoleDetails.RoleDetails.RoleID,
            CompletedUserID: SessionHelper.UserDetails.UserID,
            CompleteNotes: this.CompleteNotes
        };
        return this._loanInfoDataAccess.LoanComplete(req).subscribe(res => {
            this.isRevertToReadyForAudit$.next(true);
            this.isAuditComplete$.next(true);
            this.EnabledAssignUser$.next(true);
            this._notificationService.showSuccess('Audit Completed Successfully');
        });
    }

    GetLoanTableDetails() {
        return this._loanData;
    }

    GetLoan() {
        return this._loanAPIResponse;
    }

    SetLoan(_loanAPIResponse) {
        this._loanAPIResponse = _loanAPIResponse;
    }

    GetLoanDocInfo() {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanAPIResponse.LoanID, DocumentID: this._currentDoc.DocID, VersionNumber: this._currentDoc.VersionNumber };
        return this._loanInfoDataAccess.GetLoanDocInfo(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            this._currentDoc._totalImageCount = data.ImageCount;
            this.SetDocField$.next({ DocLevelFields: data.DocLevelFields, TempDocTables: data.TempDocTables });
        });
    }

    SetCurrentForm(currentForm, node) {
        this._currentForm[node] = currentForm[node];
    }

    saveNReevaluate() {
        const _dTables = [];
        this._currentForm.datatables.forEach(table => {
            const tableVal = table[0];
            const newTable = { Name: tableVal.Name, HeaderRow: tableVal.HeaderRow, Rows: [] };
            tableVal.Rows.getRawValue().forEach(element => {
                newTable.Rows.push({ RowCoordinates: element.RowCoordinates, RowColumns: element.columns });
            });
            _dTables.push(newTable);
        });

        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanAPIResponse.LoanID, DocumentID: this._currentDoc.DocID, CurrentUserID: SessionHelper.UserDetails.UserID, DocumentTables: _dTables, DocumentFields: this._currentForm.fields, VersionNumber: this._currentDoc.VersionNumber };
        return this._loanInfoDataAccess.SaveAndRevaluate(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data.success) {
                this.ReRunLoanStartPage(data);
                this._notificationService.showSuccess('Loan Updated Successfully');
            } else {
                this._notificationService.showError('Loan Updated Failed');
            }
        });
    }

    DownloadInitReverification(req) {
        this._loanInfoDataAccess.DownloadReverification(req).subscribe(res => {
            if (isTruthy(res)) {
                this.ReverificationBlog$.next(res);
                this.GetReverification();
            }
        });
    }

    GetLoanFieldForm() {
        return this.fieldFrmGrp;
    }

    SetLoanDocuments(_loanDocuments) {
        this.LoanDocuments = _loanDocuments;
    }

    GetLoanDocuments() {
        return this.LoanDocuments.slice();
    }

    GetReverificationFields() {
        return this._templateFields;
    }

    SetReverificationFields(_fields) {
        this._templateFields = _fields;
    }

    ChangeDocumentType(newDocTypeID) {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanAPIResponse.LoanID, OldDocumentID: this._currentDoc.DocID, NewDocumentID: newDocTypeID, VersionNumber: this._currentDoc.VersionNumber, CurrentUserID: SessionHelper.UserDetails.UserID };
        this.confirmChangeDocModal$.next(false);
        return this._loanInfoDataAccess.ChangeDocumentType(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data.success) {
                this.ReRunLoanStartPage(data);
                this.FIELDToggle$.next(true);
                this._notificationService.showSuccess('Loan Updated Successfully');
            } else {
                this._notificationService.showError('Loan Updated Failed');
            }
        });
    }

    GetLoanDetails() {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanData.LoanID };
        return this._loanInfoDataAccess.GetLoanDetails(req).subscribe(res => {
            if (res !== null) {
                this._loanAPIResponse = jwtHelper.decodeToken(res.Data)['data'];
                this._loanAPIResponse.loanDocuments.forEach(stackDoc => {
                    const docval = (stackDoc.IsDocName === null || !stackDoc.IsDocName) ? '' : (' -  ' + stackDoc.DocFieldName + ' : ' + stackDoc.DocValue);
                    stackDoc.ToolTipValue = ((this.CheckMoreThanOnce(stackDoc.DocName)) ? stackDoc.DocName + ' - V' + (stackDoc.FieldOrderBy === '' ? (stackDoc.VersionNumber).toString() : this.GetFieldVersionNumber(stackDoc.DocName, stackDoc.FieldOrderVersion)) : stackDoc.DocName) + '' + docval;
                    stackDoc.DocNameVersion = ((this.CheckMoreThanOnce(stackDoc.DocName)) ? stackDoc.DocName + '-V' + (stackDoc.FieldOrderBy === '' ? (stackDoc.VersionNumber).toString() : this.GetFieldVersionNumber(stackDoc.DocName, stackDoc.FieldOrderVersion)) : stackDoc.DocName);
                });
            }
            if (this._loanAPIResponse === null) {
                this._loanAPIResponse = new Loan();
                this._loanAPIResponse.LoanID = 0;
            }

            if (this._loanData.Status === StatusConstant.COMPLETE) {
                this.isAuditComplete$.next(true);
                this.isRevertToReadyForAudit$.next(true);
                this.EnabledAssignUser$.next(true);
            } else {
                this.isAuditComplete$.next(false);
                this.EnabledAssignUser$.next(false);
                this.isRevertToReadyForAudit$.next(false);
                if (isTruthy(this._loanData.AssignedUserID) && this._loanData.AssignedUserID > 0) {
                    this.disableCompleteAudit$.next(this._loanData.AssignedUserID !== SessionHelper.UserDetails.UserID);
                } else {
                    this.disableCompleteAudit$.next(false);
                }
            }
            this.LoanDetails$.next(this._loanAPIResponse);
        });
    }

    RevertLoanComplete() {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanAPIResponse.LoanID, UserName: SessionHelper.UserDetails.FirstName + '' + SessionHelper.UserDetails.LastName };
        this._loanInfoDataAccess.RevertLoanComplete(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data) {
                this.isAuditComplete$.next(false);
                this.isRevertToReadyForAudit$.next(false);
                this._loanData.Status = StatusConstant.PENDING_AUDIT;
                this.EnabledAssignUser$.next(false);
                this._notificationService.showSuccess('Loan Status Updated Successfully');
            } else {
                this._notificationService.showError('Loan Status Updated Failed');
            }

            this.revertToReadyforAuditModal$.next(false);
        });
    }

    GetStipulations() {
        return this._stipulationsTable.slice();
    }

    GetChecklistDetails(_reRun: boolean) {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanAPIResponse.LoanID, ReRun: _reRun };
        return this._loanInfoDataAccess.GetChecklistDetails(req).subscribe(res => {
            if (res !== null) {
                let checkListArryObj = [];
                let catogorylists: any = [];
                const getCategory = [];
                const responseObj = jwtHelper.decodeToken(res.Data)['data'];
                this._loanAPIResponse.allChecklist = responseObj.allChecklist;
                this._loanAPIResponse.loanQuestioner = responseObj.loanQuestioner;
                if (isTruthy(this._loanAPIResponse.allChecklist)) {
                    this._loanAPIResponse.allChecklist.forEach((checklist) => {
                        if (checklist.RoleType === '0') {
                            const ruleAssociatedDocs = this.GetDocList(checklist.Formula, checklist.RuleType);
                            const _docsfilter = [];
                            ruleAssociatedDocs.forEach(d => {
                                _docsfilter.push(d.DocumentName + '_' + d.MissingDoc + '_' + d.FieldName);
                            });
                            const docs = this.GetAssociatedDocs(_docsfilter);
                            checkListArryObj.push({
                                Category: checklist.Category,
                                CheckListName: checklist.CheckListName,
                                Result: checklist.Result,
                                Type: 'Automatic',
                                Message: checklist.Message,
                                Error: checklist.ErrorMessage,
                                Checklist: checklist,
                                Formula: ruleAssociatedDocs,
                                AssociatedDoc: docs.AssociatedDocuments,
                                DocumentTypes: docs.AssociatedDocType,
                                Expression: checklist.Expression,
                                SequenceID: checklist.SequenceID,
                                RuleType: checklist.RuleType,
                                LoanType: this._loanAPIResponse.LoanType,
                                ServiceType: this._loanData.ServiceTypeName
                            });
                        }
                        catogorylists.push(checklist.Category);
                    });
                }
                catogorylists = catogorylists.filter((x, i, a) => x && a.indexOf(x) === i);
                for (let i = 1; i <= catogorylists.length; i++) {
                    getCategory.push({ Id: i, text: catogorylists[i - 1] });
                }
                let loanManualQuestioner = [];
                if (isTruthy(this._loanAPIResponse.loanQuestioner)) {
                    loanManualQuestioner = this.ParseManualQuestioners(this._loanAPIResponse);
                    loanManualQuestioner.forEach((questioner) => {
                        const docs = this.GetManuaDocList(questioner.Question);
                        checkListArryObj.push({
                            Category: questioner.Category,
                            CheckListName: questioner.CheckListName,
                            Result: '',
                            Type: 'Manual',
                            Checklist: questioner,
                            Message: '',
                            Error: '',
                            qitem: questioner,
                            SequenceID: questioner.SequenceID,
                            Formula: docs,
                            AssociatedDoc: [],
                            DocumentTypes: docs.length > 0 ? docs.map(d => d.DocumentName).join() : '',
                            Expression: questioner.Question,
                            RuleType: '',
                            LoanType: this._loanAPIResponse.LoanType,
                            ServiceType: this._loanData.ServiceTypeName
                        });
                    });
                }
                checkListArryObj = this.OrderBy(checkListArryObj, { property: 'SequenceID', direction: 1 });
                this.checkListArryObj$.next({ checkListArryObj: checkListArryObj.slice(), loanManualQuestioner: loanManualQuestioner.slice() });
            }
        });
    }

    SaveQuetionerAnswer(loanManualQuestioner) {
        for (let i = 0; i < this._loanAPIResponse.loanQuestioner.length; i++) {
            const lquest = loanManualQuestioner.filter(x => x.RuleID === this._loanAPIResponse.loanQuestioner[i].RuleID)[0];
            if (isTruthy(lquest)) {
                this._loanAPIResponse.loanQuestioner[i].AnswerJson = JSON.stringify({ Answer: lquest.answer, Notes: lquest.QuestionNotes });
            }
        }
        for (let i = 0; i < loanManualQuestioner.length; i++) {
            loanManualQuestioner[i].inputList.forEach(option => {
                const answerEle = loanManualQuestioner[i].answer.filter(x => option.label === x);
                if (isTruthy(answerEle) && answerEle.length > 0) {
                    option.checked = 'checked';
                } else {
                    option.checked = '';
                }
            });
        }
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanAPIResponse.LoanID, Questioners: this._loanAPIResponse.loanQuestioner, CurrentUserID: SessionHelper.UserDetails.UserID };
        return this._loanInfoDataAccess.UpdateLoanQuestioner(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data) {
                this.enableQuestionerSave$.next(0);
                this._notificationService.showSuccess('Questioner Updated Successfully');
            } else {
                console.log('Questioner Updated Failed');
            }
        });

    }

    CheckLoanPDF() {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanData.LoanID };
        this._loanInfoDataAccess.CheckLoanPDF(req).subscribe(res => {
            if (res !== null && res.Data !== null) {
                const data = jwtHelper.decodeToken(res.Data)['data'];
                if (!data) {
                    this._notificationService.showError('Stacking Order PDF not Found');
                } else {
                    const _pdfLink = environment.apiURL + 'Image/DownloadLoanPDF/' + req.TableSchema + '/' + req.LoanID;
                    this.LoanPDFCheck$.next({ ShowDownload: data, PDFLink: _pdfLink });
                }
            } else { this._notificationService.showError('Stacking Order PDF not Found'); }
        });
    }

    GetStipulationList() {
        const req = { TableSchema: AppSettings.SystemSchema };
        this._loanInfoDataAccess.GetStipulationList(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            const sCategoryNames = [];
            data.forEach(element => {
                sCategoryNames.push({ id: element.StipulationID, text: element.StipulationCategory });
            });
            this.StipulationCategoryNames$.next(sCategoryNames.slice());
        });
    }

    SaveStipulation(LoanStipulation) {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanData.LoanID, LoanStipulationDetails: LoanStipulation };
        this._loanInfoDataAccess.SaveStipulation(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            this._stipulationsTable = [];
            if (data !== null) {
                this._stipulationsTable = data;
                this._notificationService.showSuccess('Stipulation Saved Successfully');
            } else {
                this._notificationService.showError('Stipulation Failed To Save');
            }
            this.StipulationsTable$.next(this._stipulationsTable.slice());
            this.StipulationModal$.next(false);
        });
    }

    ValidateStipulation(LoanStipulation) {
        let returValue = false;
        if (!isTruthy(LoanStipulation.StipulationCategoryID)) {
            this._notificationService.showError('Category Name Required');
            returValue = true;
        } else if (!isTruthy(LoanStipulation.StipulationDescription)) {
            this._notificationService.showError('Stipulation Description Required');
            returValue = true;
        } else if (!isTruthy(LoanStipulation.StipulationStatus)) {
            this._notificationService.showError('Stipulation Status Required');
            returValue = true;
        }
        return returValue;
    }

    UpdateStipulation(LoanStipulation) {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanData.LoanID, LoanStipulationDetails: LoanStipulation };
        this._loanInfoDataAccess.UpdateStipulation(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            this._stipulationsTable = [];
            if (data !== null) {
                this._stipulationsTable = data;
                this._notificationService.showSuccess('Stipulation Updated Successfully');
            } else {
                this._notificationService.showError('Stipulation Failed To Update');
            }
            this.StipulationsTable$.next(this._stipulationsTable.slice());
            this.StipulationModal$.next(false);
        });
    }

    GetLoanAudit() {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanAPIResponse.LoanID };
        this._loanInfoDataAccess.GetLoanAudit(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data === null) { this.AuditTableData$.next([]); } else { this.AuditTableData$.next(data); }
        });
    }

    DeletedReverification(_loanReverifyID, _userRevericationName) {
        const fullName = SessionHelper.UserDetails.LastName + SessionHelper.UserDetails.FirstName;
        const req = { TableSchema: AppSettings.TenantSchema, LoanReverificationID: _loanReverifyID, LoanID: this._loanAPIResponse.LoanID, ReverificationName: _userRevericationName, UserName: fullName };
        this._loanInfoDataAccess.DeletedReverification(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data) {
                this._notificationService.showSuccess('Reverification Deleted Successfully');
                this.GetReverification();
                this.confirmReverifyDeleteModal$.next(false);
            }
        });
    }

    GetReverification() {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanAPIResponse.LoanID };
        this._loanInfoDataAccess.GetReverification(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            let initReverification = [];
            if (data.length > 0) {
                initReverification = data;
            }

            initReverification.forEach(element => {
                element.Download = false;
            });

            this.LoanReverification$.next(initReverification.slice());
        });
    }

    DownloadReverification(_loanReverifyID) {
        const req = { TableSchema: AppSettings.TenantSchema, LoanReverificationID: _loanReverifyID };
        this._loanInfoDataAccess.DownloadLoanReverification(req).subscribe(res => {
            this.ReverificationBlog$.next(res);
        });
    }

    GetLoanBasedReverification() {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanAPIResponse.LoanID };
        this._loanInfoDataAccess.GetLoanBasedReverification(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            this.ReverificationMaster = [];
            this.ReverificationMaster.push({ MappingID: 0, MappedDocuments: [], TemplateID: 0, TemplateFileName: '', TemplateFieldValue: '', TemplateFields: '', ReverificationID: 0, ReverificationName: '--Select Re-verification--', LogoGuid: '' });
            if (data !== null) {
                if (data.length > 0) {
                    data.forEach(element => {
                        this.ReverificationMaster.push({ MappingID: element.MappingID, MappedDocuments: element.MappedDocuments, TemplateFieldValue: element.TemplateFieldValue, TemplateID: element.TemplateID, TemplateFileName: element.TemplateFileName, TemplateFields: element.TemplateFields, ReverificationID: element.ReverificationID, ReverificationName: element.ReverificationName, LogoGuid: element.LogoGuid });
                    });
                }
            }
            this.LoanReverificationMasters$.next(this.ReverificationMaster.slice());
        });
    }

    RetryEmail(rowID: any) {
        const req = { TableSchema: AppSettings.TenantSchema, ID: rowID };
        this._loanInfoDataAccess.RetryEmail(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data !== null && data) { this._notificationService.showSuccess('Email Successfully Initiated'); } else { this._notificationService.showError('Unable to retry'); }

            this.EmailRetryModal$.next(false);
        });
    }

    GetEmailTrackerData() {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanAPIResponse.LoanID };
        this._loanInfoDataAccess.GetEmailTrackerData(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data !== null) {
                this.EmailTrackerTableData$.next(data);
            } else {
                this.EmailTrackerTableData$.next([]);
            }
        });
    }

    GetEmailDetails(rowID: any) {
        const req = { TableSchema: AppSettings.TenantSchema, ID: rowID };
        this._loanInfoDataAccess.GetEmailDetails(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data !== null) {
                this.EmailModalData$.next({ To: data.To, Subject: data.Subject, Attachement: data.Attachments, Body: data.Body, showEmailSendBtn: false });
                this.AuditEmailConfirmModal$.next(true);
            }
        });
    }

    GetLoanStipulationDetails() {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanAPIResponse.LoanID };
        return this._loanInfoDataAccess.GetLoanStipulationDetails(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            this._stipulationsTable = [];
            if (data !== null) {
                this._stipulationsTable = data;
            }

            this.StipulationsTable$.next(this._stipulationsTable.slice());
        });
    }

    GetUserMaster() {
        if (this._userNames.length > 0) {
            this.usernameNotes$.next(this._userNames.slice());
        } else {
            const req = { TableSchema: AppSettings.TenantSchema };
            this._loanInfoDataAccess.GetUserMaster(req).subscribe(res => {
                const UsernameData = jwtHelper.decodeToken(res.Data)['data'];
                this._userNames = [];
                UsernameData.forEach(element => {
                    this._userNames.push(element.UserName);
                });
                this.usernameNotes$.next(this._userNames.slice());
            });
        }
    }

    UpdateLoanNotes(req: { TableSchema: string, LoanID: number, Notes: any }) {
        this._loanInfoDataAccess.UpdateLoanNotes(req).subscribe(res => {
            const Result = jwtHelper.decodeToken(res.Data)['data'];
            if (Result !== null) {
                this.notesTxtArea$.next('');
            }
        });
    }

    GetLoanNotes() {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanData.LoanID };
        this._loanInfoDataAccess.GetLoanNotes(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data !== '') {
                this.LoanNotes$.next(JSON.parse(data));
            } else {
                this.LoanNotes$.next([]);
            }
        });
    }

    SaveLoanBatchDetails(req: { TableSchema: string, JobName: any, ExportedBy: any, CustomerID: any, CoverLetter: any, TableOfContent: any, PasswordProtected: any, Password: any, CoverLetterContent: any, BatchLoanDoc: any }) {
        this._loanInfoDataAccess.SaveExportBatchDetails(req).subscribe(res => {
            const result = jwtHelper.decodeToken(res.Data)['data'];
            if (result === true) {
                this._notificationService.showSuccess('Job Created Successfully');
                this.LoanExportBack$.next(true);
            } else {
                this._notificationService.showError('Failed while creating job');
            }
        });
    }

    OpenIDCLink(BatchID: string) {
        const req = { TableSchema: AppSettings.TenantSchema, EphesoftBatchInstanceID: BatchID, EphesoftURL: AppSettings.TenantConfigType[1].ConfigKey, CustomerID: 0 };
        this._loanInfoDataAccess.GetEphesoftURL(req).subscribe(res => {
            const result = jwtHelper.decodeToken(res.Data)['data'];
            if (result !== null) {
                if ((result.Ephesoft_Status === EphesoftStatusConstant.READY_FOR_REVIEW || result.Ephesoft_Status === EphesoftStatusConstant.READY_FOR_VALIDATION) && result.Ephesoft_lock_owner === 'false') {
                    const EphesoftValue = result.Ephesoft_URL;
                    window.open(EphesoftValue, '_blank');
                } else if (result.Ephesoft_Status === EphesoftStatusConstant.FINISHED) {
                    this._notificationService.showSuccess('Batch FINISHED Successfully');
                } else if (result.Ephesoft_Status === EphesoftStatusConstant.ERROR) {
                    this._notificationService.showSuccess('Batch is in ERROR Status');
                } else if (result.Ephesoft_Status === EphesoftStatusConstant.DELETED) {
                    this._notificationService.showSuccess('Batch is in DELETE Status');
                } else if (result.Ephesoft_lock_owner === 'true') {
                    this._notificationService.showError('Locked by another user');
                } else {
                    this._notificationService.showError('Batch is not available');
                }
            }
        });
    }

    SetLoanPageInfo(loanData: LoanSearchTableModel) {
        this._loanHeaderInfo.LoanID = loanData.LoanID;
        this._loanHeaderInfo.LoanAmount = loanData.LoanAmount;
        this._loanHeaderInfo.BorrowerName = loanData.BorrowerName;
        this._loanHeaderInfo.ServiceType = loanData.ServiceTypeName;
        this._loanHeaderInfo.AuditMonthYear = loanData.AuditMonthYear;
        this._loanHeaderInfo.ReceivedDate = loanData.ReceivedDate;
        this._loanHeaderInfo.LoanNumber = loanData.LoanNumber;
        this._loanHeaderInfo.LoanType = loanData.LoanTypeName;
        this._loanHeaderInfo.LoanStatus = loanData.StatusDescription;
        this._loanHeaderInfo.AuditDueDate = loanData.AuditDueDate;
        this._loanData = loanData;
        if (isTruthy(loanData['StatusID'])) {
            this._loanData.Status = loanData['StatusID'];
        }

        if (isTruthy(loanData['Customer'])) {
            this._loanHeaderInfo.CustomerName = loanData['Customer'];
        }

        if (isTruthy(loanData['CustomerName'])) {
            this._loanHeaderInfo.CustomerName = loanData['CustomerName'];
        }
    }

    GetAssignUserList() {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanHeaderInfo.LoanID, ServiceTypeName: this._loanHeaderInfo.ServiceType };
        this._loanInfoDataAccess.GetAssignableUserList(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            this._assignUserList = [];
            if (data !== null && data.length > 0) {
                data.forEach(element => {
                    this._assignUserList.push({ UserID: element.UserID, UserName: element.UserName, AssignedUserID: element.AssignedUserID });
                });
            }
            this.AssignUserList$.next(this._assignUserList.slice());
        });
    }

    GetLoanSearchFilterConfigValue(req: any) {
        this._loanInfoDataAccess.GetLoanSearchFilterConfigValue(req).subscribe((res) => {
            const result = jwtHelper.decodeToken(res.Data)['data'];
            this.loanFilterResult$.next(result);
        });
    }

    GetReverificationObj(id) {
        const obj = this.ReverificationMaster.filter(x => x.ReverificationID === parseInt(id, 10));
        if (!isTruthy(obj)) { return ''; }
        if (obj.length <= 0) { return ''; }
        return obj[0];
    }

    GetLoanDocumentFieldValue(jData: any, elementKey: any, str: any, element: any, returnFieldCount: any, fieldCount: any, _reverifyObj: any, doc: any[]) {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanAPIResponse.LoanID, DocumentName: doc[0], DocumentField: doc[1] };
        this._loanInfoDataAccess.GetLoanDocumentFieldValue(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data !== null) {
                jData[elementKey] = str.replace('{' + element + '}', data);
            } else {
                jData[elementKey] = '';
            }
            returnFieldCount++;
            this.changeField$.next({ fieldCount: fieldCount, returnFieldCount: returnFieldCount });
        });
    }

    GetLoanHeader() {
        return this._loanHeaderInfo;
    }

    AssignLoanUser(_assignedUserID: any) {
        const userID = parseInt(_assignedUserID, 10);
        const AssignedTo = this._assignUserList.filter(x => x.UserID === userID)[0].UserName;
        const req = new AssignLoanUserRequest(
            AppSettings.TenantSchema,
            this._loanHeaderInfo.LoanID,
            userID,
            SessionHelper.UserDetails.UserID,
            this._loanHeaderInfo.ServiceType,
            SessionHelper.UserDetails.UserName,
            AssignedTo
        );
        this._loanInfoDataAccess.AssignLoan(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data > 0) {
                this._loanData.AssignedUserID = userID;
                this.disableCompleteAudit$.next(userID !== SessionHelper.UserDetails.UserID);
                this._notificationService.showSuccess('User Assigned Successfully');
            } else {
                this._notificationService.showError('Failed To Assign Loan');
            }
        });
    }

    getAuditReport() {
        const url = 'Image/DownloadLoanAuditReport/' + AppSettings.TenantSchema + '/' + this._loanHeaderInfo.LoanID;
        this._loanInfoDataAccess.getAuditReport(url).subscribe(res => {
            this.AuditReportBlog$.next(res);
        });
    }

    ValidateEmail(Email: { To: string, Subject: string }) {
        let returVal = false;
        if (!isTruthy(Email.To)) {
            this._notificationService.showError('Email Address Required');
            returVal = true;
        }
        if (isTruthy(Email.To) && !this._emailPipe.transform(Email.To)) {
            this._notificationService.showError('Enter Valid Email ID');
            returVal = true;
        }
        if (!isTruthy(Email.Subject)) {
            this._notificationService.showError('Email Subject Required');
            returVal = true;
        }
        return returVal;
    }

    SaveLoanHeader(req: { TableSchema: string, LoanID: number, LoanDetails: any, UserName: string }) {
        this._loanInfoDataAccess.SaveLoanHeader(req).subscribe(res => {
            const result = jwtHelper.decodeToken(res.Data)['data'];
            if (result) {
                this._notificationService.showSuccess('Loan Information Updated Successfully');
            } else {
                this._notificationService.showError('Loan Information Not Updated Successfully');
            }
        });
    }

    SaveAuditDueDateHeader(req: { TableSchema: string, LoanID: number, AuditDueDate: string }) {
        this._loanInfoDataAccess.SaveAuditDueDateHeader(req).subscribe(res => {
            const result = jwtHelper.decodeToken(res.Data)['data'];
            if (result) {
                this._notificationService.showSuccess('Loan Audit Due Date Added Successfully');
            } else {
                this._notificationService.showError('Loan Audit Due Date Added Failed');
            }
        });
    }

    TriggerEmail(req: { TableSchema: string, To: string, Subject: string, Body: string, Attachement: string, UserID: number, SendBy: string, LoanID: number, AttachmentsName: string }) {
        this._loanInfoDataAccess.TriggerEmail(req).subscribe(res => {
            const _emaildata = jwtHelper.decodeToken(res.Data)['data'];
            if (_emaildata.Success) {
                this._notificationService.showSuccess('Mail Sent Successfully');
            } else {
                this._notificationService.showError('Mail Trigger Failed');
            }

            this.EmailConfirmModal$.next(false);
        });
    }

    CheckMoreThanOnce(docName) {
        return this._loanAPIResponse.loanDocuments.filter(l => l.DocName === docName && l.DocumentLevelIcon === 'Success').length > 1;
    }

    GetFieldVersionNumber(docName, currentVersion): string {
        const docs = this._loanAPIResponse.loanDocuments.filter(l => l.DocName === docName).slice();
        if (docs.length !== currentVersion) {
            return ((docs.length + 1) - currentVersion).toString();
        } else {
            return '1';
        }
    }

    GetAvailableDocuments() {
        return this._loanAPIResponse.loanDocuments.filter(element => (element.DocumentLevelIcon === 'Success' && element.DocID !== 0 && !element.Obsolete)).slice();
    }

    RefreshStackDocs(loanDocuments) {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanData.LoanID };
        this._loanInfoDataAccess.GetMissingDocStatus(req).subscribe(res => {
            const responseObj = jwtHelper.decodeToken(res.Data)['data'];
            if (responseObj.length > 0) {
                responseObj.forEach(ele => {
                    const docStack = loanDocuments.filter(l => l.DocID === ele.DocID.toString() && l.DocumentLevelIcon !== 'Success');
                    if (docStack.length > 0) {
                        docStack[0].IDCStatus = ele.Status.toString();
                        docStack[0].IDCUrl = ele.IDCBatchInstanceID.toString();
                        if (ele.Status === 3) {
                            docStack[0].DocumentLevel = 'In Loan';
                            docStack[0].DocumentLevelIcon = 'Success';
                            docStack[0].DocumentLevelIconColor = 'Success';
                            const missDocReq = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanData.LoanID, DocumentID: docStack[0].DocID };
                            this._loanInfoDataAccess.GetMissingDocVersion(missDocReq).subscribe(missDocRes => {
                                const result = jwtHelper.decodeToken(missDocRes.Data)['data'];
                                if (result.VersionNumber !== null) {
                                    docStack[0].VersionNumber = result.VersionNumber;
                                }
                            });
                        }
                    }
                });
                this.LoanDocuments = loanDocuments;
                this._notificationService.showSuccess('Missing Documents Status Updated');
            } else {
                this._notificationService.showWarning('Missing Documents not uploaded');
            }
        });
    }

    saveDocumentPDFAs(blob, fileName) {
        const obj = new Observable(ob => {
            const link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileName;
            link.click();
        }).subscribe();
    }

    DownloadDocument(currentDoc) {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanAPIResponse.LoanID, DocumentID: currentDoc.DocID, DocumentVersion: currentDoc.VersionNumber };
        return this._loanInfoDataAccess.DownloadDocumentPDF(req).subscribe(res => {
            if (res !== null) {
                this.DocumentDownloadBlog$.next(res);
            } else {
                this._notificationService.showError('Error while downloading document');
            }

            currentDoc['Loading'] = false;
        });
    }

    DocumentObsolete(req) {
        this._loanInfoDataAccess.DocumentObsolete(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            this.documentObsoleteModal$.next(false);
            if (data) {
                this.GetLoanDetails();
            } else {
                this._notificationService.showError('Document Deleted Failed');
            }
        });
    }

    SetLoanViewDocument(doc: any) {
        doc._currentDocName = ((this.CheckMoreThanOnce(doc.DocName)) ? doc.DocName + ' - V' + (doc.FieldOrderBy === '' ? (doc.VersionNumber).toString() : this.GetFieldVersionNumber(doc.DocName, doc.FieldOrderVersion)) : doc.DocName); // +""+docval;
        this._currentDoc = doc;
    }

    SetLoanReverifyViewDocument(doc: any) {
        this._currentDoc = doc;
        this._currentDoc.DocID = doc.DocumentTypeID;
        this._currentDoc.VersionNumber = doc.DocVersion;
        this._currentDoc._currentDocName = doc.DocumentName;
        this._currentDoc._totalImageCount = 0;
    }

    GetLoanViewDocument() {
        return this._currentDoc;
    }

    GetCompareImages(DocID, versionNumber: number, defaultPageNo) {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanAPIResponse.LoanID, DocumentID: DocID, ImageID: 0, PageNo: defaultPageNo, VersionNumber: versionNumber, ShowAllDocs: true, LastPageNumber: 0 };
        return this._loanInfoDataAccess.GetLoanImages(req).subscribe(res => {
            const leftImageSource = [];
            const data = JSON.parse(res.Data);
            data.forEach(img => {
                if (img.ImageVersion === versionNumber) {
                    leftImageSource.push({
                        Image: environment.apiURL + 'Image/Get/' + AppSettings.TenantSchema + '/' + this._loanAPIResponse.LoanID + '/' + img.ImageGuid,
                        ImageVersion: '1',
                        PageNo: img.PageNo,
                        currentZoom: 0,
                        orgImgHeight: img.OrginalImageHeight,
                        orgImgwidth: img.OrginalImageWidth,
                        compressedImgHeight: img.CompressedImageHeight,
                        compressedImgwidth: img.CompressedImageWidth,
                        currentHeight: 0,
                        currentWidth: 0
                    });
                    // this.compareAllPageNo = img.PageNo;
                }
            });
            this.leftImageSource$.next(leftImageSource.slice());
        });
    }

    getDocImage(docID, versionNumber: string, defaultPageNo, imgDivList) {
        // this.loadingpage = defaultPageNo + 1;
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanAPIResponse.LoanID, DocumentID: docID, ImageID: 0, PageNo: defaultPageNo, VersionNumber: versionNumber, ShowAllDocs: true, LastPageNumber: this._currentDoc.lastPageNumber };
        return this._loanInfoDataAccess.GetLoanImages(req).subscribe(res => {
            const data = JSON.parse(res.Data);
            const multiImageSource = [];
            data.forEach(img => {
                const crtZoom = (imgDivList.nativeElement.clientWidth / img.CompressedImageWidth) * 100;
                const crtHeight = (img.CompressedImageHeight * crtZoom) / 100;
                const crtWidth = (img.CompressedImageWidth * crtZoom) / 100;
                multiImageSource.push({
                    Image: this._sanitizer.bypassSecurityTrustUrl(environment.apiURL + 'Image/Get/' + AppSettings.TenantSchema + '/' + this._loanAPIResponse.LoanID + '/' + img.ImageGuid),
                    ImageVersion: '1',
                    PageNo: img.PageNo,
                    currentZoom: crtZoom,
                    orgImgHeight: img.OrginalImageHeight,
                    orgImgwidth: img.OrginalImageWidth,
                    compressedImgHeight: img.CompressedImageHeight,
                    compressedImgwidth: img.CompressedImageWidth,
                    currentHeight: crtHeight,
                    currentWidth: crtWidth,
                    ImageShown: false
                });
            });
            this._currentDoc.lastPageNumber = multiImageSource[multiImageSource.length - 1]['PageNo'] + 1;
            this._currentDoc._totalImageCount = multiImageSource.length;
            // if (this.isDrawBox === true) {
            // let i;
            // let inputFields: any = [];
            // inputFields = this._fieldDiv.nativeElement.getElementsByTagName("input");
            // for (i = 0; i < inputFields.length; i++) {
            //     this._fieldDiv.nativeElement.getElementsByTagName("input")[i].disabled = false;
            // }
            //   this.isDrawBox = false;
            // }
            // this.getImageByPageNo(defaultPageNo);
            // this.RestDrawBox();
            // if (this._loan.checkListState && this.defaultState) {
            //     this._loan.imageState = false;
            // }
            // this.CompareImageViewerType = false;
            // this.showAllDocuments = false;
            this.multiImageSource$.next(multiImageSource.slice());
        });
    }

    private ReRunLoanStartPage(data) {
        this._loanAPIResponse = data;
        this._loanAPIResponse.loanDocuments.forEach(stackDoc => {
            const docval = (stackDoc.IsDocName === null || !stackDoc.IsDocName) ? ('') : (' -  ' + stackDoc.DocFieldName + ' : ' + stackDoc.DocValue);
            stackDoc.ToolTipValue = ((this.CheckMoreThanOnce(stackDoc.DocName)) ? stackDoc.DocName + ' - V' + (stackDoc.FieldOrderBy === '' ? (stackDoc.VersionNumber).toString() : this.GetFieldVersionNumber(stackDoc.DocName, stackDoc.FieldOrderVersion)) : stackDoc.DocName) + docval;
            stackDoc.DocNameVersion = ((this.CheckMoreThanOnce(stackDoc.DocName)) ? stackDoc.DocName + '-V' + (stackDoc.FieldOrderBy === '' ? (stackDoc.VersionNumber).toString() : this.GetFieldVersionNumber(stackDoc.DocName, stackDoc.FieldOrderVersion)) : stackDoc.DocName);
        });
        this.ShowDocumentDetailView$.next(false);
        this.GetChecklistDetails(true);
        if (this._loanData.Status === StatusConstant.COMPLETE) {
            this.isAuditComplete$.next(true);
            this.isRevertToReadyForAudit$.next(true);
            this.EnabledAssignUser$.next(true);
        } else {
            this.isAuditComplete$.next(false);
            this.EnabledAssignUser$.next(false);
            this.isRevertToReadyForAudit$.next(false);
        }
        this.LoanDetails$.next(this._loanAPIResponse);
    }

    private OrderBy(records: any, args?: any): any {
        return records.sort(function (a, b) {
            if (a[args.property].toString().toLowerCase() < b[args.property].toString().toLowerCase()) {
                return -1 * args.direction;
            } else if (a[args.property].toString().toLowerCase() > b[args.property].toString().toLowerCase()) {
                return 1 * args.direction;
            } else {
                return 0;
            }
        });
    }
    private GetManuaDocList(formula: string): { DocumentName: string, FieldName: string, MissingDoc: boolean }[] {
        const splits = formula.match(/(?=\[).+?(?=\])/g);
        const docs = [];
        const _associatedDocs = [];
        if (splits !== null && splits.length > 0) {
            splits.forEach((docField) => {
                if (docField !== '') {
                    docs.push({ Docname: docField.replace('[', ''), Fieldname: '' });
                }
            });
            const _docs = docs.filter((x, i, a) => x && a.indexOf(x) === i);
            _docs.forEach(element => {
                if (this._loanAPIResponse.loanDocuments.filter(d => d.DocName === element.Docname && d.DocumentLevelIcon !== 'Success').length === 1) {
                    _associatedDocs.push({ DocumentName: element.Docname, FieldName: element.Fieldname, MissingDoc: true });
                } else if (splits[0] === 'checkall') {
                    _associatedDocs.push({ DocumentName: '', FieldName: '' });
                } else {
                    _associatedDocs.push({ DocumentName: element.Docname, FieldName: element.Fieldname, MissingDoc: false });
                }
            });

        }
        return _associatedDocs;
    }

    private ParseManualQuestioners(loanobj: Loan) {
        const loanManualQuestioner = [];
        if (isTruthy(loanobj.loanQuestioner)) {
            loanobj.loanQuestioner.forEach(element => {
                const parsedOptions = JSON.parse(element.OptionJson);
                const answerObj = JSON.parse(element.AnswerJson);
                let parsedAnswers = [];
                let answerNotes = '';
                let NotesEnabled = false;
                if (isTruthy(answerObj) && (isTruthy(answerObj.Answer) || isTruthy(answerObj.Ansewer))) {
                    parsedAnswers = isTruthy(answerObj.Answer) ? answerObj.Answer : isTruthy(answerObj.Ansewer) ? answerObj.Ansewer : [];
                    answerNotes = answerObj.Notes;
                }
                const inputOptions = [];
                if (parsedOptions.manualGroup[0].QuestionsTypes[0].inputtypes === '1') {
                    parsedOptions.manualGroup[0].CheckBoxChoices.forEach(ele => {
                        let isChecked = '';
                        if (parsedAnswers.indexOf(ele.checkboxoptions) > -1) {
                            isChecked = 'checked';
                        }
                        const objinputOpt = { 'type': 'checkbox', 'label': ele.checkboxoptions, 'checked': isChecked };
                        inputOptions.push(objinputOpt);
                    });
                }
                if (parsedOptions.manualGroup[0].QuestionsTypes[0].inputtypes === '2') {
                    parsedOptions.manualGroup[0].RadioChoices.forEach(ele => {
                        let isChecked = '';
                        if (parsedAnswers.indexOf(ele.radiooptions) > -1) {
                            isChecked = 'checked';
                        }
                        const objinputOpt = { 'type': 'radio', 'label': ele.radiooptions, 'checked': isChecked };
                        inputOptions.push(objinputOpt);
                    });
                }
                if (typeof parsedOptions.manualGroup[0].QuestionsTypes[0].isNotesEnabled !== 'undefined' && parsedOptions.manualGroup[0].QuestionsTypes[0].isNotesEnabled === true) {
                    NotesEnabled = true;
                }
                const qobj = {
                    'Category': element.Category,
                    'RuleID': element.RuleID,
                    'CheckListDetailID': element.CheckListDetailID,
                    'CheckListName': element.CheckListName,
                    'Question': element.Question,
                    'inputList': inputOptions,
                    'answer': parsedAnswers,
                    'NotesEnabled': NotesEnabled,
                    'QuestionNotes': answerNotes,
                    'SequenceID': element.SequenceID
                };
                loanManualQuestioner.push(qobj);
            });
        }
        return loanManualQuestioner;
    }

    private GetDocList(formula: string, ruleType: string): any {
        const splits = formula.split('(');
        const _tempDocFields = formula.match(/(?=\[).+?(?=\])/g);
        const docs = [];
        if (_tempDocFields !== null) {
            _tempDocFields.forEach((docField) => {
                const _newDocField = docField.substring(1, docField.length);
                const docFieldArry = _newDocField.split('.');
                if (docFieldArry.length > 1) {
                    docs.push({ Docname: docFieldArry[0], Fieldname: docFieldArry[1].replace(/[\s]/g, '').toLowerCase() });
                } else if (docFieldArry.length > 0) {
                    docs.push({ Docname: docFieldArry[0], Fieldname: '' });
                }
            });
        }
        let _docs = docs.filter((x, i, a) => x && a.indexOf(x) === i);
        if (splits[0] === 'Encompass') {
            for (let i = 0; i < _docs.length; i++) {
                if (_docs[i].Docname.includes('#')) {
                    _docs.splice(i, 1);
                }
            }
        } else if (splits[0] === 'isexist' || splits[0] === 'isexistAny' || ruleType === 'groupby') {
            _docs = [];
        }

        const _associatedDocs = [];
        _docs.forEach(element => {
            if (this._loanAPIResponse.loanDocuments.filter(d => d.DocName === element.Docname && d.DocumentLevelIcon !== 'Success').length === 1) {
                _associatedDocs.push({ DocumentName: element.Docname, FieldName: element.Fieldname, MissingDoc: true });
            } else if (splits[0] === 'checkall') {
                _associatedDocs.push({ DocumentName: '', FieldName: '' });
            } else {
                _associatedDocs.push({ DocumentName: element.Docname, FieldName: element.Fieldname, MissingDoc: false });
            }
        });
        return _associatedDocs;
    }

    private GetAssociatedDocs(_docsfilter) {
        let _associatedDocType = '';
        const _associatedDocuments = [];
        _docsfilter = _docsfilter.filter((x, i, a) => x && a.indexOf(x) === i);
        _docsfilter.forEach(element => {
            const splitvals = element.split('_');
            _associatedDocuments.push({ DocumentName: splitvals[0], MissingDoc: splitvals[1].toLowerCase() === 'true' ? true : false, FieldName: splitvals[2] });
        });

        const _associatedDocVals = [];
        if (_associatedDocuments.length > 0) {
            _associatedDocuments.forEach(element => {
                _associatedDocVals.push(element.DocumentName);
            });
            _associatedDocType = _associatedDocVals.join();
        } else {
            _associatedDocType = '';
        }

        return { AssociatedDocType: _associatedDocType, AssociatedDocuments: _associatedDocuments };
    }
}
