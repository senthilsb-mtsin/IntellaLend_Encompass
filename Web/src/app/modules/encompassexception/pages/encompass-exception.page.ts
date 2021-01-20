import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { AppSettings } from '@mts-app-setting';
import { formatDate } from '@mts-functions/convert-datetime.function';
import { NotificationService } from '@mts-notification';
import { DataTableDirective } from 'angular-datatables';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { EncompassExceptionService } from '../service/encompass-exception.service';

@Component({
    selector: 'encompassexception',
    templateUrl: 'encompass-exception.page.html',
    styleUrls: ['encompass-exception.page.css']
})

export class EncompassDownloadExceptionComponent implements OnInit,AfterViewInit {

    daterangeoptions: any;
    dTable: any = {};
    dateFrom: any;
    dateTo: any;
    dtOptions: any = {};
    data: any = [];
    busy: Subscription;
    promise: Subscription;
    datevalue: any;
    retryData: any = [];
    EncompassExceptionModalShow = false;
    encompassExceptionDtable: any = [];
    _btnSearchClick = false;

    @ViewChild(DataTableDirective) dt: DataTableDirective;
    @ViewChild('encompassException') encompassException: ModalDirective;

    constructor(
        private _notifyService: NotificationService,
        private _encompassService: EncompassExceptionService

    ) { }
    private subscriptions: Subscription[] = [];

    ngOnInit() {

        this.subscriptions.push(this._encompassService.encompassExceptionDtable.subscribe((res: any) => {
            this.dTable.clear();
            this.dTable.rows.add(res);
            this.dTable.draw();
            this.encompassExceptionDtable = res;

        }));

        this.subscriptions.push(this._encompassService.EncompassExceptionModalShow.subscribe((res: boolean) => {
            if (res) {
                this.encompassException.hide();
                this.encompassExceptionDtable.forEach(element => {
                    if (element.EncompassExceptionID === this.retryData.EncompassExceptionID) {
                        element.Status = 0;
                        element.RetryCount += 1;
                    }
                });
                this.dTable.clear();
                this.dTable.rows.add(this.encompassExceptionDtable);
                this.dTable.draw();
                this._notifyService.showSuccess('Status Successfully Updated');
            } else {
                this.encompassException.hide();
                 this._notifyService.showSuccess('Updated Failed');

            }
        }));
        this.daterangeoptions = {
            theme: 'default',
            previousIsDisable: false,
            nextIsDisable: false,
            range: 'to',
            dayNames: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
            presetNames: ['Today', 'This Month', 'Last Month', 'This Week', 'Last Week', 'This Year', 'Last Year'],
            dateFormat: 'M/d/yyyy',
            outputFormat: 'DD/MM/YYYY',
            startOfWeek: 0,
            display: { to: 'block', tm: 'block', lm: 'block', lw: 'block', tw: 'block', ty: 'none', ly: 'none', custom: 'block', em: 'block' }

        };
        this.dtOptions = {
            displayLength: 2,
            aaData: this.data,
            'select': {
                style: 'single',
                info: false
            },
            'iDisplayLength': 10,
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [

                { sTitle: 'Encompass Loan #', mData: 'EncompassLoanNumber', sClass: 'text-center' },
                { sTitle: 'Encompass Guid', mData: 'EncompassGuid', sClass: 'text-center' },
                { sTitle: 'Exception Message', mData: 'ExceptionMessage', sClass: 'text-center' },
                { sTitle: 'Status', mData: 'Status', sClass: 'text-center', sWidth: '10%' },
                { sTitle: 'Retry Count', mData: 'RetryCount', sClass: 'text-center', sWidth: '10%' },
                { sTitle: 'ModifiedOn', mData: 'ModifiedOn', sClass: 'text-center', sWidth: '10%' },
                { sTitle: 'Retry', mData: 'EncompassExceptionID', sClass: 'text-center', sWidth: '2%' },
            ],
            aoColumnDefs: [
                {
                    'aTargets': [3],
                    'mRender': function (data, type, row) {

                        if (row.Status === -1) {

                            return '<div title="Failed to download" class="box-caption w100 title_ellipsis">' + '<span style="cursor:pointer" class="material-icons txt-red">error_outline</span>' + '</div>';

                        } else if (row.Status === 0) {

                            return '<div title="Waiting to be download" class="box-caption w100 title_ellipsis">' + '<span style="cursor:pointer" class="material-icons txt-green">settings_backup_restore</span>' + '</div>';
                        } else {
                            return '';
 }
                    }
                },
                {
                    'aTargets': [6],
                    'mRender': function (data, type, row) {
                        if (row.Status === -1) {
                            return '<span  style="cursor:pointer" class=" material-icons resetpwd" >refresh</span>';
                        } else {
                            return '';
                        }
                    }
                },
                {
                    'aTargets': [5],
                    'mRender': function (ModifiedOn) {

                        if (ModifiedOn !== null && ModifiedOn !== '') {
                            return eval('convertDateTime')(ModifiedOn);
                        } else {
                            return '';
                        }
                    }
                }

            ],
            rowCallback: (row: Node, data: any[] | Object, index: number) => {

                const self = this;
                $('td .resetpwd', row).unbind('click');
                $('td .resetpwd', row).bind('click', () => {
                    self.retryExceptionData(row, data);

                });
                return row;

            }

        };
        this.dateFrom = new Date();
        this.dateTo = new Date();
        this.SearchEncompassDownloadException(this.dateFrom, this.dateTo);

    }
    ngAfterViewInit() {
        this.dt.dtInstance.then((dtInstance: DataTables.Api) => {
            this.dTable = dtInstance;
        });
    }
    monthDiff(startDate, endDate) {
        const days = (endDate - startDate) / (1000 * 60 * 60 * 24);
        return Math.round(days);
    }
    retryExceptionData(row: any, data: any): void {

        this.retryData = data;
        this.encompassException.show();
    }

    RetryEncompassException() {
        const inputdata = { TableSchema: AppSettings.TenantSchema, EncompassExceptionID: this.retryData.EncompassExceptionID };
        this._encompassService.RetryEncompassException(inputdata);

    }
    SearchEncompassDownloadException(dateFrom, dateTo) {
        let inputData;
        if (this.monthDiff(dateFrom, dateTo) > 60) {
            this._notifyService.showWarning('Select date within 60 days range');
        } else {
             const fromDateStr = formatDate(dateFrom);
             const toDatStr = formatDate(dateTo);
             if ((fromDateStr !== undefined) || (fromDateStr !== '') && (toDatStr !== undefined) || (toDatStr !== '')) {
                const _encompassException = { FromDateStr: fromDateStr, ToDateStr: toDatStr };
                inputData = { TableSchema: AppSettings.TenantSchema, encompassException: _encompassException };
            }

        }
        this.promise = this._encompassService.SearchDownloadException(inputData);

    }
    ngDestroy() {
        this.subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
