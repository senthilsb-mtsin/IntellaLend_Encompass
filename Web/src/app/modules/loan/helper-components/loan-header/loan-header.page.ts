import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { LoanHeaders } from '../../models/loan-header.model';
import { IMyOptions, IMyDate } from '@mts-date-picker/interfaces';
import { SessionHelper } from '@mts-app-session';
import { LoanInfoService } from '../../services/loan-info.service';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { NotificationService } from '@mts-notification';
import { AppSettings } from '@mts-app-setting';
import { DatePipe, Location } from '@angular/common';
import { Subscription, Observable } from 'rxjs';
import { convertDateTime, convertDateTimewithTime } from '@mts-functions/convert-datetime.function';
import { environment } from 'src/environments/environment';
import { EmailCheckPipe } from '@mts-pipe';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Loan } from '../../models/loan-details.model';
import { Router } from '@angular/router';
import { FileUploaderOptions, FileUploader, FileItem } from 'ng2-file-upload';
import { JwtHelperService } from '@auth0/angular-jwt';

const jwtHelper = new JwtHelperService();
@Component({
    selector: 'mts-loan-header',
    templateUrl: 'loan-header.page.html',
    styleUrls: ['loan-header.page.css'],
    providers: [DatePipe]
})
export class LoanHeaderComponent implements OnInit, OnDestroy {
    @ViewChild('EmailconfirmModal') EmailconfirmModal: ModalDirective;
    LoanHeaderInfo: LoanHeaders = new LoanHeaders();
    myDatePickerOptions: IMyOptions = {
        dateFormat: 'mm/dd/yyyy',
        editableDateField: false
    };
    selDate: IMyDate = { year: new Date().getFullYear(), month: new Date().getMonth() + 1, day: new Date().getDate() };
    showHide: any = [false, false, false];
    visibility = true;
    userList: any[] = [];
    loanFilterResult: any = {};
    isReceivedDate = 'none';
    assignedUserID = 0;
    EnabledAssignUser = false;
    uploader: FileUploader;
    pdfSrc = '';
    promise: Subscription;
    Email: {
        AttachmentsName: string,
        To: string,
        Attachement: string,
        Subject: string,
        Body: string,
        txtTOAlrt: string,
        txtAttachAlrt: string,
        txtSubAlrt: string,
        showEmailSendBtn: boolean
    } = { AttachmentsName: '', To: '', Attachement: '', Subject: '', Body: '', txtTOAlrt: '', txtAttachAlrt: '', txtSubAlrt: '', showEmailSendBtn: true };
    itemsDocuments: any[] = [];
    availDocuments: any[] = [];
    showAttachmentMention = true;
    isRevertToReadyForAudit = false;
    AuditMonthyear: Date ;
    AuthorityLabelSingular = AppSettings.AuthorityLabelSingular;
  constructor(
        private _loanInfoService: LoanInfoService,
        private _notificationService: NotificationService,
        private _datePipe: DatePipe,
        private _emailPipe: EmailCheckPipe,
        private _location: Location,
        private _route: Router
    ) {
        this.checkPermission('ReadonlyLoans', 0);
        this.checkPermission('Loanassignmentmanagement', 1);
        this.checkPermission('LoanDetailExport', 2);
    }

    private _subscriptions: Subscription[] = [];
    private _loanDetails: Loan;
    private target: any;
    private frontEndVersion: { orginalVersion: any, fDocName: any }[] = [];
    private foptions: FileUploaderOptions = {
        url: environment.apiURL + 'FileUpload/MissingDocGlobalFileUploader',
        authToken: 'Bearer ' + localStorage.getItem('id_token'),
        authTokenHeader: 'Authorization',
        headers: [
            { name: 'TableSchema', value: AppSettings.TenantSchema },
            { name: 'UploadFileName', value: '' },
            { name: 'UserId', value: '' },
            { name: 'DocId', value: '' },
            { name: 'LoanId', value: '' }
        ]
    };

    ngOnInit() {
        this.GetLoanSearchFilterConfigValue();
        this.LoanHeaderInfo = this._loanInfoService.GetLoanHeader();
        this.pdfSrc = environment.apiURL + 'Image/DownloadLoanAuditReport/' + AppSettings.TenantSchema + '/' + this.LoanHeaderInfo.LoanID;

        this._subscriptions.push(this._loanInfoService.AssignUserList$.subscribe((res: { UserID: number, UserName: string, AssignedUserID: number }[]) => {
            if (res.length > 0) {
                this.assignedUserID = res[0].AssignedUserID;
            }

            this.userList.push({ UserID: 0, UserName: '--Select User--', AssignedUserID: 0 });
            res.forEach(element => {
                this.userList.push(element);
            });
        }));

        this._subscriptions.push(this._loanInfoService.loanFilterResult$.subscribe((res) => {
            this.loanFilterResult = res;
            if (this.loanFilterResult.ReceivedDate) {
                this.isReceivedDate = 'block';
            } else {
                this.isReceivedDate = 'none';
            }
        }));

        this._subscriptions.push(this._loanInfoService.isRevertToReadyForAudit$.subscribe((res: boolean) => {
            if (isTruthy(res)) {
                this.isRevertToReadyForAudit = res;
            }
        }));

        this._subscriptions.push(this._loanInfoService.LoanDetails$.subscribe((res: Loan) => {
            if (isTruthy(res) && isTruthy(res.LoanID) && res.LoanID > 0) {
                this._loanDetails = res;
                this.SetLoanHeaderDeatils();
            }
        }));

        this._subscriptions.push(this._loanInfoService.EnabledAssignUser$.subscribe((res: boolean) => {
            this.EnabledAssignUser = res;
        }));

        this._subscriptions.push(this._loanInfoService.AuditReportBlog$.subscribe((res: any) => {
            const blob = new Blob([res], { type: 'application/pdf' });
            this._loanInfoService.saveDocumentPDFAs(blob, 'AuditReport_' + this.LoanHeaderInfo.LoanNumber + '.pdf');
        }));

        this._subscriptions.push(this._loanInfoService.EmailConfirmModal$.subscribe((res: boolean) => {
            res ? this.EmailconfirmModal.show() : this.EmailconfirmModal.hide();
        }));

        // this.SetDateFormatting();
        this.uploader = new FileUploader(this.foptions);

        this.uploader.onBeforeUploadItem = (item: FileItem) => {
            if (this.LoanHeaderInfo.LoanID > 0 && SessionHelper.UserDetails.UserID > 0) {
                item.withCredentials = false;
                this.uploader.options.headers[1].value = item.file.name;
                this.uploader.options.headers[2].value = SessionHelper.UserDetails.UserID;
                this.uploader.options.headers[3].value = '0';
                this.uploader.options.headers[4].value = this.LoanHeaderInfo.LoanID.toString();
                item.upload();
            } else {
                this.uploader.clearQueue();
            }
        };

        // this.uploader.onAfterAddingFile = (item => {
        //     if (this.target) this.target.value = '';
        // });

        this.uploader.onCompleteItem = (item: any, response: any, status: any, headers: any) => {
            const res = JSON.parse(response);
            if (response !== null) {
                const data = jwtHelper.decodeToken(res.data)['data']['Result'];
                if (data.Result) {
                    this._notificationService.showSuccess('File Uploaded Successfully');
                    this.uploader.clearQueue();
                } else {
                    this._notificationService.showError('File Uploaded Failed!!!');
                    this.uploader.queue[0].isUploaded = false;
                    this.uploader.clearQueue();
                }
            } else { this._notificationService.showError('File Uploaded Failed!!!'); }
            if (this.target) { this.target.value = ''; }
        };

        this._loanInfoService.GetAssignUserList();
    }

    GetLoanSearchFilterConfigValue() {
        const req = {
            TableSchema: AppSettings.TenantSchema,
            CustomerID: 0,
            ConfigKey: 'Search_Filter'
        };
        this._loanInfoService.GetLoanSearchFilterConfigValue(req);
    }

    SetLoanHeaderDeatils() {
        this.LoanHeaderInfo.InvestorLoanNumber = this._loanDetails.LoanHeaderInfo.InvestorLoanNumber;
        this.LoanHeaderInfo.PropertyAddress = this._loanDetails.LoanHeaderInfo.PropertyAddress;
        this.LoanHeaderInfo.PostCloser = this._loanDetails.LoanHeaderInfo.PostCloser;
        this.LoanHeaderInfo.LoanOfficer = this._loanDetails.LoanHeaderInfo.LoanOfficer;
        this.LoanHeaderInfo.UnderWriter = this._loanDetails.LoanHeaderInfo.UnderWriter;
        this.LoanHeaderInfo.AuditDueDate = this._loanDetails.LoanHeaderInfo.AuditDueDate;
        this.LoanHeaderInfo.ReceivedDate = this._loanDetails.LoanHeaderInfo.ReceivedDate;
        this.SetDateFormatting();
    }

    EmailModal() {
        this.Email = {
            AttachmentsName: '',
            To: '',
            Attachement: '',
            Subject: '',
            Body: '',
            txtTOAlrt: '',
            txtAttachAlrt: '',
            txtSubAlrt: '',
            showEmailSendBtn: true
        };
        this.AvailableDocumentsVersioning();
    }

    GetDocOrginalVersion(DocAttach: any) {
        const attach = DocAttach.match(/(?=\[).+?(?=\])/g);
        const attachNames = [];
        if (isTruthy(attach)) {
            attach.forEach(element => {
                const _docname = element.replace('[', '');
                const _frontdocversion = this.frontEndVersion.filter(x => x.fDocName === _docname);
                if (typeof _frontdocversion !== 'undefined' && _frontdocversion !== null && _frontdocversion.length > 0) {
                    const index = _docname.lastIndexOf('-V');
                    const docnamesplit = (index === -1) ? '' : _docname.substring(0, index);
                    const name = ((docnamesplit !== '') ? docnamesplit + '-V' + _frontdocversion[0].orginalVersion : _docname);
                    attachNames.push(name);
                }
            });
        }
        this.Email.AttachmentsName = attachNames.join(',');
    }

    SendEmail() {
        if (!this._loanInfoService.ValidateEmail(this.Email)) {
            this.GetDocOrginalVersion(this.Email.Attachement);
            const req = {
                TableSchema: AppSettings.TenantSchema,
                To: this.Email.To,
                Subject: this.Email.Subject,
                Body: this.Email.Body,
                Attachement: this.Email.AttachmentsName,
                UserID: SessionHelper.UserDetails.UserID,
                SendBy: SessionHelper.UserDetails.FirstName + '' + SessionHelper.UserDetails.LastName,
                LoanID: this.LoanHeaderInfo.LoanID,
                AttachmentsName: this.Email.Attachement
            };
            this._loanInfoService.TriggerEmail(req);
        }
    }

    changeEmailfunc(currtItem: any) {
        // let availableDocuments = this._loanInfoService.GetAvailableDocuments();
        this.showAttachmentMention = false;
        const attach = this.Email.Attachement.match(/(?=\[).+?(?=\])/g);
        if (attach !== null && attach.length !== 0 && attach.length - 1 === this.availDocuments.length) { } else {
            if (attach !== null && attach.length > 0) {
                const tempArry = [];
                this.itemsDocuments = [];
                const _attach = [];
                attach.forEach(att =>
                    _attach.push(att.replace('[', ''))
                );
                this.availDocuments.forEach(ele => {
                    if (_attach.indexOf(ele) === -1) {
                        tempArry.push(ele);
                    }
                });
                this.itemsDocuments = tempArry;
            } else {
                this.itemsDocuments = this.availDocuments;
            }
        }
        this.showAttachmentMention = true;
    }

    AvailableDocumentsVersioning() {
        this.itemsDocuments = [];
        this.availDocuments = [];
        this.frontEndVersion = [];
        this._loanDetails.loanDocuments.forEach(element => {
            if (element.DocumentLevelIcon === 'Success' && element.DocID > 0) {
                const Docval = this._loanInfoService.CheckMoreThanOnce(element.DocName) ? element.DocName + ' -V' + (element.FieldOrderBy === '' ? (element.VersionNumber).toString() : this._loanInfoService.GetFieldVersionNumber(element.DocName, element.FieldOrderVersion)) : element.DocName;
                if (!element.Obsolete) {
                    this.itemsDocuments.push(Docval);
                    this.availDocuments.push(Docval);
                }
                this.frontEndVersion.push({ orginalVersion: element.VersionNumber, fDocName: Docval });
            }
        });
    }

    LoanDetailExport() {
        this._route.navigate(['view/loandetails/export']);
    }

    back() {
        if (this._loanInfoService.IsDirectLink) {
            this._route.navigate(['view/loansearch']);
        } else {
            this._location.back();
        }
    }

    showError() {
        this._notificationService.showError('Select User Name');
    }

    AssignUser() {
        if (isTruthy(this.assignedUserID)) {
            this._loanInfoService.AssignLoanUser(this.assignedUserID);
        } else {
            this._notificationService.showError('Select User Name');
        }
    }

    SetDateFormatting() {
        if (isTruthy(this.LoanHeaderInfo.AuditDueDate)) {
            this.LoanHeaderInfo.AuditDueDate = this.LoanHeaderInfo.AuditDueDate.replace('/Date(', '').replace(')/', '');
            const d = new Date(parseInt(this.LoanHeaderInfo.AuditDueDate, 10));
            this.selDate = {
                year: d.getFullYear(),
                month: d.getMonth() + 1,
                day: d.getDate()
            };
        } else { this.LoanHeaderInfo.AuditDueDate = ''; }

        this.LoanHeaderInfo.AuditDueDate = this._datePipe.transform(this.LoanHeaderInfo.AuditDueDate, AppSettings.dateFormat);
        this.LoanHeaderInfo.ReceivedDate = convertDateTimewithTime(this.LoanHeaderInfo.ReceivedDate);
        this.LoanHeaderInfo.AuditMonthYear = this._datePipe.transform(this.LoanHeaderInfo.AuditMonthYear, AppSettings.dateFormat);
        this.AuditMonthyear = new Date(this.LoanHeaderInfo.AuditMonthYear);
    }

    // SaveLoans(type: string) {
    //     if (!isTruthy(this.LoanHeaderInfo[type])) {
    //         this._notificationService.showError(type + ' is Required');
    //     } else {
    //         const fullName = SessionHelper.UserDetails.FirstName + '' + SessionHelper.UserDetails.LastName;
    //         const fieldType = type === 'LoanNumber' ? 0 : 1;
    //         const req = { TableSchema: AppSettings.TenantSchema, LoanID: this.LoanHeaderInfo.LoanID, LoanDetails: this.LoanHeaderInfo[type], Type: fieldType, UserName: fullName };
    //         this._loanInfoService.SaveLoanHeader(req);
    //     }
    // }
    SaveLoanHeader() {
        let LoanHeaderRequest = new LoanHeaders();

        if (this.checkHeadervalidation()) {
            LoanHeaderRequest = this.setHeaderRequest(LoanHeaderRequest);
            const fullName = SessionHelper.UserDetails.FirstName + '' + SessionHelper.UserDetails.LastName;
            const req = { TableSchema: AppSettings.TenantSchema, LoanID: this.LoanHeaderInfo.LoanID, LoanDetails: this.LoanHeaderInfo, UserName: fullName };
            this._loanInfoService.SaveLoanHeader(req);
        }
    }
    setHeaderRequest(LoanHeaderRequest: LoanHeaders) {

        LoanHeaderRequest.AuditDueDate = this.LoanHeaderInfo.AuditDueDate === null ? null : this.LoanHeaderInfo.AuditDueDate.formatted;

        LoanHeaderRequest.AuditMonthYear = this.LoanHeaderInfo.AuditMonthYear === null ? null : this.LoanHeaderInfo.AuditMonthYear.formatted;
        LoanHeaderRequest.LoanNumber = this.LoanHeaderInfo.LoanNumber;
        LoanHeaderRequest.PostCloser = this.LoanHeaderInfo.PostCloser;
        LoanHeaderRequest.LoanOfficer = this.LoanHeaderInfo.LoanOfficer;
        LoanHeaderRequest.UnderWriter = this.LoanHeaderInfo.UnderWriter;
        LoanHeaderRequest.LoanAmount = this.LoanHeaderInfo.LoanAmount;
        LoanHeaderRequest.InvestorLoanNumber = this.LoanHeaderInfo.InvestorLoanNumber;
        LoanHeaderRequest.PropertyAddress = this.LoanHeaderInfo.PropertyAddress;
        LoanHeaderRequest.BorrowerName = this.LoanHeaderInfo.BorrowerName;
        return LoanHeaderRequest;
    }
    UploadTrailingDocument(event) {
        this.target = event.target || event.srcElement;
        this.uploader.uploadAll();
    }
    getMonthYear(event: any) {
        this.LoanHeaderInfo.AuditMonthYear = this._datePipe.transform(
            event.Value,
            AppSettings.dateFormat
        );
    }

    checkHeadervalidation() {
        if (!isTruthy(this.LoanHeaderInfo['LoanNumber'])) {
            this._notificationService.showError('Loan Number is Required');
            return false;
        }
        return true;
    }
    getAuditReport() {
        this._loanInfoService.getAuditReport();
    }

    SaveAuditDueDate() {
        const req = { TableSchema: AppSettings.TenantSchema, LoanID: this.LoanHeaderInfo.LoanID, AuditDueDate: (this.LoanHeaderInfo.AuditDueDate !== null) ? this.LoanHeaderInfo.AuditDueDate.formatted : null };
        this._loanInfoService.SaveAuditDueDateHeader(req);
    }

    checkPermission(component: string, index: number): void {
        let URL = '';
        if (index === 1 || index === 2) {
            URL = 'View\\LoanSearch\\LoanInfo\\' + component;
        } else {
            URL = 'View\\LoanDetails\\' + component;
        }

        const AccessCheck = false;
        const AccessUrls = SessionHelper.RoleDetails.URLs;
        if (AccessCheck !== null) {
            AccessUrls.forEach(element => {
                if (element.URL === URL) {
                    this.showHide[index] = true;
                    return false;
                }
            });
        }
    }

    ngOnDestroy() {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
