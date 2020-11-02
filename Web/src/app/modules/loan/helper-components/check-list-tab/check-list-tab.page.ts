import { Component, ViewChild, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { LoanInfoService } from '../../services/loan-info.service';
import { LoanHeaders } from '../../models/loan-header.model';
import { DataTableDirective } from 'angular-datatables';
import { Subscription } from 'rxjs';
import { ChecklistRowItem } from '../../models/loan-checklist-row.model';
import { NotificationService } from '@mts-notification';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { SessionHelper } from '@mts-app-session';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Component({
    selector: 'mts-check-list-tab',
    templateUrl: 'check-list-tab.page.html',
    styleUrls: ['check-list-tab.page.scss']
})
export class CheckListTabComponent implements OnInit, OnDestroy, AfterViewInit {
    @ViewChild(DataTableDirective) datatableEl: DataTableDirective;
    @ViewChild('auditCompleteModal') _auditCompleteModal: ModalDirective;
    @ViewChild('revertToReadyforAudit') revertToReadyforAudit: ModalDirective;
    showHide: boolean[] = [false, false, false];
    _disableAudit = false;
    isAuditComplete = false;
    isRevertToReadyForAudit = false;
    testSlide = 'slide-in';
    dtChecklistOptions: any = {};
    checkListArryObj: any[] = [];
    promiseChk: Subscription;
    checkListItemRows: ChecklistRowItem = new ChecklistRowItem();
    enableQuestionerSave: any = 0;
    completeNotes: any = '';

    constructor(
        private _loanInfoService: LoanInfoService,
        private _notificationService: NotificationService
    ) {
        this.checkPermission('ReadonlyLoans', 0);
    }

    private _subscriptions: Subscription[] = [];
    private LoanHeaderInfo: LoanHeaders;
    private dTable: any;
    private loanManualQuestioner: any[] = [];

    ngOnInit(): void {

        this.LoanHeaderInfo = this._loanInfoService.GetLoanHeader();

        this._subscriptions.push(this._loanInfoService.disableCompleteAudit$.subscribe((res: boolean) => {
            if (isTruthy(res)) {
                this._disableAudit = res;
            }
        }));

        this._subscriptions.push(this._loanInfoService.checkListArryObj$.subscribe((res: { checkListArryObj: any[], loanManualQuestioner: any[] }) => {
            this.checkListArryObj = res.checkListArryObj;
            this.loanManualQuestioner = res.loanManualQuestioner;
            this.dTable.clear();
            this.dTable.rows.add(res.checkListArryObj);
            this.dTable.draw();
        }));

        this._subscriptions.push(this._loanInfoService.enableQuestionerSave$.subscribe((res: number) => {
            this.enableQuestionerSave = res;
        }));

        this._subscriptions.push(this._loanInfoService.revertToReadyforAuditModal$.subscribe((res: boolean) => {
            res ? this.revertToReadyforAudit.show() : this.revertToReadyforAudit.hide();
        }));

        this._subscriptions.push(this._loanInfoService.isAuditComplete$.subscribe((res: boolean) => {
            if (isTruthy(res)) {
                this.isAuditComplete = res;
            }
        }));

        this._subscriptions.push(this._loanInfoService.isRevertToReadyForAudit$.subscribe((res: boolean) => {
            if (isTruthy(res)) {
                this.isRevertToReadyForAudit = res;
            }
        }));

        this.dtChecklistOptions = {
            dom: 'Blfrtip',
            buttons: [{
                extend: 'excel',
                className: 'btn btn-sm btn-info waves-effect waves-light m10',
                text: '<i class="fa fa-file-excel-o"></i> Download',
                filename: (this.LoanHeaderInfo.LoanNumber !== undefined && this.LoanHeaderInfo.LoanNumber !== '' ? this.LoanHeaderInfo.LoanNumber : this.LoanHeaderInfo.LoanID) + '_Rule Findings',
                exportOptions: {
                    columns: [0, 1, 2, 10, 5, 8, 9]
                },
                title: 'Loan Checklist Details'
            }],
            aaData: [],
            'scrollY': 'calc(100vh - 510px)',
            'scrollX': true,
            'ordering': false,
            'paging': false,
            'fixedHeader': true,
            'autoWidth': false,
            'select': {
                style: 'single',
                info: false
            },
            aoColumns: [
                { sTitle: 'Category', mData: 'Category', bSortable: false, 'width': '20%' },
                { sTitle: 'CheckList Name', mData: 'CheckListName', bSortable: false, 'width': '53%' },
                { sTitle: 'Rule Type', mData: 'Type', bSortable: false, 'width': '13%' },
                { sTitle: 'Status', mData: 'Result', sClass: 'text-center', 'width': '9%', bSortable: false },
                { sTitle: 'Message', mData: 'Message', bVisible: false },
                { sTitle: 'Error', mData: 'Error', bVisible: false },
                { sTitle: 'AssociatedDoc', mData: 'AssociatedDoc', bVisible: false },
                { sTitle: 'SequenceID', mData: 'SequenceID', bVisible: false },
                { sTitle: 'Expression', mData: 'Expression', bVisible: false },
                { sTitle: 'Result', mData: 'Result', bVisible: false },
                { sTitle: 'Status', mData: 'Result', bVisible: false }
            ],
            aoColumnDefs: [
                {
                    'aTargets': [3],
                    'mRender': function (data, type, row) {
                        if (row['Type'] === 'Manual') {
                            return '<span style="display:none">man</span>';
                        }

                        if (data !== null && data !== '') {
                            if (row['Error'].length > 0) {
                                return '<span title="Error" class="material-icons" style="color: #ff0000;">error</span>';
                            }

                            if (data === 'False') {
                                return '<span title="Fail" class="material-icons" style="transform: rotate(45deg);color: #fc4b6c;">add_circle</span>';
                            } else if (data === 'True') {
                                return '<span title="Pass" class="material-icons" style="color: #64ab23;">check_circle</span>';
                            }
                        }
                        return '';
                    }
                }, {
                    'aTargets': [9, 10],
                    'mRender': function (data, type, row) {
                        if (row['Type'] === 'Manual') {
                            return '';
                        }

                        if (data !== null && data !== '') {
                            if (data === 'False') {
                                return 'Fail';
                            } else if (data === 'True') {
                                return 'Pass';
                            }
                        }
                    }
                }]
        };

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

    AuditComplete() {
         this._loanInfoService.SetNotesValue(this.completeNotes);

        if (this._loanInfoService.GetLoanTableDetails().AssignedUserID !== 0) {
            if (this._loanInfoService.GetLoanTableDetails().AssignedUserID === SessionHelper.UserDetails.UserID) {
                if (this._loanInfoService.GetStipulations().length > 0) {
                    const loanStipulation = this._loanInfoService.GetStipulations().filter(l => l.StipulationStatus === 1);
                    if (loanStipulation.length > 0) {
                        this._notificationService.showError('There are some pending Loan Stipulations');
                        this._auditCompleteModal.hide();
                    } else {
                        this._loanInfoService.CompleteLoan$.next(true);
                    }
                } else {
                    this._loanInfoService.CompleteLoan$.next(true);
                }

            } else {
                this._notificationService.showError('You cannot complete this loan');
            }
        } else {
            if (this._loanInfoService.GetStipulations().length > 0) {
                const loanStipulation = this._loanInfoService.GetStipulations().filter(l => l.StipulationStatus === 1);
                if (loanStipulation.length > 0) {
                    this._notificationService.showError('There are some pending Loan Stipulations');
                    this._auditCompleteModal.hide();
                } else {
                    this._loanInfoService.CompleteLoan$.next(true);
                }
            } else {
                this._loanInfoService.CompleteLoan$.next(true);
            }
        }
        this._auditCompleteModal.hide();

    }

    RevertToReadyForAudit() {
        this.completeNotes = '';
        this._loanInfoService.SetNotesValue(this.completeNotes);
        this._loanInfoService.RevertLoanComplete();
    }
CloseRevertAudit() {  this.completeNotes = '';
   this._loanInfoService.SetNotesValue(this.completeNotes);
   this.revertToReadyforAudit.hide();

}
    ToggleChecklist() {
        this.testSlide = this.testSlide === 'slide-in' ? 'slide-out' : 'slide-in';
    }

    FromImageDocLink(docName: string, fieldname: string) {
        const DocNameVersion = docName;
        const docindex = docName.lastIndexOf('-V');
        docName = (docindex !== -1) ? docName.substring(0, docindex) : docName;
        const docObjectList = this._loanInfoService.GetLoan().loanDocuments.filter(d => d.DocName === docName);
        let docObject;
        if (fieldname === 'ManualDocs') {
            docObject = docObjectList[0];
        } else {
            docObject = docObjectList.find((d) => d.DocNameVersion === DocNameVersion);
        }

        const doc = docObject;
        doc.lastPageNumber = 0;
        doc.pageNumberArray = [];
        doc.checkListState = true;
        this._loanInfoService.SetLoanViewDocument(doc);
        this._loanInfoService.ShowDocumentDetailView$.next(true);
        setTimeout(() => {
            this._loanInfoService.ShowDocumentDetailView$.next(true);
        }, 300);
    }

    SaveQuetionerAnswer() {
        this.promiseChk = this._loanInfoService.SaveQuetionerAnswer(this.loanManualQuestioner);
    }

    SetQuetionerAnswer(ruleID: any, optele: any, optType: any) {
        this.enableQuestionerSave = 1;
        if (optType === 'checkbox') {
            for (let i = 0; i < this.loanManualQuestioner.length; i++) {
                if (this.loanManualQuestioner[i].RuleID === ruleID) {
                    const itemInd = this.loanManualQuestioner[i].answer.indexOf(optele);
                    if (itemInd > -1) {
                        this.loanManualQuestioner[i].answer.splice(itemInd, 1);
                    } else {
                        this.loanManualQuestioner[i].answer.push(optele);
                    }
                }
            }
        } else {
            for (let i = 0; i < this.loanManualQuestioner.length; i++) {
                if (this.loanManualQuestioner[i].RuleID === ruleID) {
                    this.loanManualQuestioner[i].answer = [];
                    this.loanManualQuestioner[i].answer.push(optele);
                }
            }
        }
    }

    SetQuetionerNotes(ruleID: any, notes: any) {
        this.enableQuestionerSave = 1;
        for (let i = 0; i < this.loanManualQuestioner.length; i++) {
            if (this.loanManualQuestioner[i].RuleID === ruleID) {
                this.loanManualQuestioner[i].QuestionNotes = notes;
            }
        }
    }

    ngAfterViewInit() {
        this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
            this.dTable = dtInstance;
            this.GetChecklistDetails(false);
            dtInstance.on('select', (s) => {
                if (this.dTable.rows('.selected').data().length > 0) {
                    if (isTruthy(this.dTable.rows('.selected').data()[0])) {
                        this.checkListItemRows = this.dTable.rows('.selected').data()[0];
                        this.ToggleChecklist();
                    }
                }
            });
            dtInstance.on('deselect', (s) => {
                if (this.dTable.rows('.selected').data().length === 0) {
                    this.checkListItemRows = new ChecklistRowItem();
                }
            });
            const self = this;
            $('.checklistTable thead tr th').each(function (i) {
                const title = $(this).text();
                const thWidth = $(this).width() - 10;
                if (title !== 'Edit' && title !== 'View') {
                    if (title === 'Status') {
                        $(this).html(title + ' <br /> <select  style="height: 25.5px;width: ' + thWidth + 'px;" ><option value="">All</option><option value="Error">Error</option><option value="check_circle">Pass</option><option value="add_circle">Fail</option> <option value="man">Manual</option></select>');
                        $('select', this).on('change', function () {
                            const val = this['value'];
                            let thIndex = i;
                            const d = self.dTable;
                            let objIndex = 0;
                            self.dTable.columns().indexes('visible').each(function (element) {
                                if (element !== null && element === i) {
                                    thIndex = objIndex;
                                }
                                objIndex++;
                            });
                            if (self.dTable.column(thIndex).search() !== val) {
                                self.dTable
                                    .column(thIndex)
                                    .search(val)
                                    .draw();
                            }
                        });
                    } else {
                        $(this).html(title + ' <br /> <input type="text" style="width: ' + thWidth + 'px;" />');
                        $('input', this).on('keyup change', function () {
                            const val = this['value'];
                            let thIndex = i;
                            let objIndex = 0;
                            self.dTable.columns().indexes('visible').each(function (element) {
                                if (element !== null && element === i) {
                                    thIndex = objIndex;
                                }
                                objIndex++;
                            });
                            if (self.dTable.column(thIndex).search() !== val) {
                                self.dTable
                                    .column(thIndex)
                                    .search(val)
                                    .draw();
                            }
                        });
                    }
                }
            });
        });
    }

    DownloadChecklistDetails() {
        $('#loanChecklist .dt-button').click();
    }

    GetChecklistDetails(_reRun: boolean) {
        this.promiseChk = this._loanInfoService.GetChecklistDetails(_reRun);
    }

    ngOnDestroy(): void {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
