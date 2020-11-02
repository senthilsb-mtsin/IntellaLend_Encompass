import { Component, QueryList, ViewChildren, ViewChild, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Subscription } from 'rxjs';
import { LoanInfoService } from '../../services/loan-info.service';
import { convertDateTimewithTime, convertDateTime } from '@mts-functions/convert-datetime.function';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { LoanAudit } from '../../models/loan-details-sub.model';

@Component({
    selector: 'mts-history-tab',
    templateUrl: 'history-tab.page.html',
    styleUrls: ['history-tab.page.scss']
})
export class HistoryTabComponent  implements OnInit, OnDestroy, AfterViewInit {
    @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;
    @ViewChild('AuditEmailconfirmModal') AuditEmailconfirmModal: ModalDirective;
    @ViewChild('retryModal') retryModal: ModalDirective;
    dtOptions: any;
    addEmailHisdtOptions: any;
    dTable: any[] = [];
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

    constructor(
        private _loanInfoService: LoanInfoService
    ) { }
    private _subscriptions: Subscription[] = [];
    private _tableFunction: string[] = ['GetLoanAudit', 'GetEmailTrackerDetails'];
    private _resendID: any = 0;

    ngOnInit(): void {
        this._subscriptions.push(this._loanInfoService.AuditTableData$.subscribe((res: LoanAudit[]) => {
            this.dTable[0].clear();
            this.dTable[0].rows.add(res);
            this.dTable[0].draw();
            // this.dTable[0].columns.adjust().draw();
        }));

        this._subscriptions.push(this._loanInfoService.EmailTrackerTableData$.subscribe((res: LoanAudit[]) => {
            this.dTable[1].clear();
            this.dTable[1].rows.add(res);
            this.dTable[1].draw();
            // this.dTable[1].columns.adjust().draw();
        }));

        this._subscriptions.push(this._loanInfoService.AuditEmailConfirmModal$.subscribe((res: boolean) => {
            res ? this.AuditEmailconfirmModal.show() : this.AuditEmailconfirmModal.hide();
        }));
        this._subscriptions.push(this._loanInfoService.EmailRetryModal$.subscribe((res: boolean) => {
            res ? this.retryModal.show() : this.retryModal.hide();
        }));
        this._subscriptions.push(this._loanInfoService.EmailModalData$.subscribe((res: { AttachmentsName: string, To: string, Attachement: string, Subject: string, Body: string, txtTOAlrt: string, txtAttachAlrt: string, txtSubAlrt: string, showEmailSendBtn: boolean }) => {
            this.Email = res;
        }));

        this.dtOptions = {
            dom: 'ltp',
            displayLength: 5,
            aaData: [],
            'select': {
                style: 'single',
                info: false
            },
            // "scrollY": 'calc(100vh - 557px)',
            // "scrollX": true,
            'iDisplayLength': 10,
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [
                { sTitle: 'Audit Date', 'type': 'date', mData: 'AuditDateTime', 'sWidth': '50%' },
                { sTitle: 'Description', mData: 'AuditDescription', 'sWidth': '50%' }
            ],
            aoColumnDefs: [
                {
                    'aTargets': [0],
                    'mRender': function (date) {
                        if (date !== null && date !== '') {
                            return convertDateTimewithTime(date);
                        } else {
                            return date;
                        }
                    }
                }
            ]
        };

        this.addEmailHisdtOptions = {
            displayLength: 5,
            aaData: [],
            'select': {
                style: 'single',
                info: false
            },
            // "scrollY": 'calc(100vh - 591px)',
            // "scrollX": true,
            'iDisplayLength': 10,
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [
                { sTitle: 'Date', 'type': 'date', mData: 'CreatedOn', sClass: 'text-center' },
                { sTitle: 'Loan #', mData: 'LoanNumber', sClass: 'text-center' },
                { sTitle: 'Status', mData: 'Delivered', sClass: 'text-center', sWidth: '10%' },
                { sTitle: 'Sent By', mData: 'SendBy', sClass: 'text-center' },
                { sTitle: 'Error Message', mData: 'ErrorMessage', sClass: 'text=center' },
                { sTitle: 'View', mData: 'ID', sClass: 'text-center' },
                { sTitle: 'Retry', mData: 'ID', sClass: 'text-center' },
            ],
            aoColumnDefs: [
                {
                    'aTargets': [2],
                    'mRender': function (data, type, row) {
                        let statusFlag = '';
                        if (data === 1) {
                            statusFlag = 'Success';
                            return '<label class=\'label label-success ' + ' label-table\'' + '>' + statusFlag + '</label></td>';
                        } else if (data === 0) {
                            statusFlag = 'Pending';
                            return '<label class=\'label label-warning ' + ' label-table\'' + '>' + statusFlag + '</label></td>';
                        } else {
                            statusFlag = 'Failed';
                            return '<label class=\'label label-danger ' + ' label-table\'' + '>' + statusFlag + '</label></td>';
                        }
                    }
                },
                {
                    'aTargets': [5],
                    'mRender': function (data, type, row) {
                        return '<span style=\'cursor: pointer;\' class=\'viewemailtracker material-icons txt-info\'>pageview</span>';
                    }
                },
                {
                    'aTargets': [6],
                    'mRender': function (data, type, row) {
                        if (row['Delivered'] !== 1) {
                            return '<span style=\'cursor: pointer;\' class=\'refreshemailtracker material-icons txt-info\'>resend</span>';
                        } else {
                            return '';
                        }
                    }
                },
                {
                    'aTargets': [0],
                    'mRender': function (date) {
                        if (date !== null && date !== '') {
                            return convertDateTime(date);
                        } else {
                            return '';
                        }
                    }
                }
            ],
            rowCallback: (row: Node, data: any[] | Object, index: number) => {
                const self = this;
                $('td .viewemailtracker', row).unbind('click');
                $('td .viewemailtracker', row).bind('click', () => {
                    self.getRowData(row, data);
                });
                $('td .refreshemailtracker', row).unbind('click');
                $('td .refreshemailtracker', row).bind('click', () => {
                    self.resendMailData(row, data);
                });
                return row;
            }
        };
    }

    getRowData(row: Node, rowData: any): void {
        if (isTruthy(rowData)) {
            this.GetCurrentData(rowData.ID);
        }
    }

    GetCurrentData(ID: any) {
        this._loanInfoService.GetEmailDetails(ID);
    }

    resendMailData(row: any, data: any) {
        this._loanInfoService.EmailRetryModal$.next(true);
        this._resendID = data.ID;
    }

    ResendMail() {
        this._loanInfoService.RetryEmail(this._resendID);
    }

    GetLoanAudit() {
        this._loanInfoService.GetLoanAudit();
    }

    GetEmailTrackerDetails() {
        this._loanInfoService.GetEmailTrackerData();
    }

    historyTabClick() {
        setTimeout(() => {
            this.dTable[0].columns.adjust().draw();
        }, 300);
    }

    emailTabClick() {
        setTimeout(() => {
            this.dTable[1].columns.adjust().draw();
        }, 300);
    }

    ngAfterViewInit(): void {
        this.dtElements.forEach((dtElement: DataTableDirective, index: number) => {
            dtElement.dtInstance.then((dtInstance: any) => {
                this.dTable[index] = dtInstance;
                this[this._tableFunction[index]]();
            });
        });
    }

    ngOnDestroy(): void {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
