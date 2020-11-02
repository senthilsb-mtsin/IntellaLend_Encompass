import { Component, ViewChild, QueryList, ViewChildren, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { UserDatatableModel } from 'src/app/modules/user/models/user-datatable.model';
import { convertDateTime, convertDateTimewithTime, formatDate } from '@mts-functions/convert-datetime.function';
import { DataTableDirective } from 'angular-datatables';
import { ExportService } from '../../service/export.service';
import { Router } from '@angular/router';
import { LosExportdateModel } from '../../models/los.export.date.model';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { AppSettings, LOSExportStagingStatusConstant, LOSExportStatusConstant } from '@mts-app-setting';
import { NgDateRangePickerComponent } from '@mts-daterangepicker/ng-daterangepicker.component';
import { Subscription } from 'rxjs/internal/Subscription';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { LOsExportStagingModel } from '../../models/los.export.staging.model';
import { RetryLOSExportModel } from '../../models/retry.los.export.model';
import { CommonService } from 'src/app/shared/common';

@Component({
    selector: 'mts-los-export-monitor',
    templateUrl: 'los-export-monitor.page.html',
    styleUrls: ['los-export-monitor.page.css'],
})

export class LosExportMonitorComponent implements OnInit, OnDestroy, AfterViewInit {
    @ViewChild(NgDateRangePickerComponent) receivedDate: NgDateRangePickerComponent;
    @ViewChildren(DataTableDirective) dataTableElement: QueryList<DataTableDirective>;
    @ViewChild('LosExportDetailsModal') LosExportDetailsModal: ModalDirective;
    @ViewChild('RetryConfirmModel') RetryConfirmModel: ModalDirective;
    AuthorityLabelSingular: string = AppSettings.AuthorityLabelSingular;
    LosExportStagingdtOptions: any = {};
    customerSelect: any = 0;
    ReviewTypeSelect: any = 0;
    LoanTypeSelect: any = 0;
    dtOptions: any = {};
    Dateoptions: any;
    LosExportMonitorFromdate: any;
    LosExportMonitorTodate: any;
    LosExportMonitorTable: any = {};
    LosExportStagingMonitorTable: any = {};
    rowData: LOsExportStagingModel;
    promise: Subscription;
    RetryFileType = '';
    commonActiveCustomerItems: any = [];
    commonActiveLoanTypeItems: any = [];
    commonActiveReviewTypeItems: any = [];

    constructor(
        private _route: Router,
        private _exportservice: ExportService,
        private commonmasterservice: CommonService
    ) { }
    private subscription: Subscription[] = [];

    private dTable: any;

    ngOnInit() {
        this.Dateoptions = {
            theme: 'default',
            previousIsDisable: false,
            nextIsDisable: false,
            range: 'to',
            dayNames: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
            presetNames: ['Today', 'This Month', 'Last Month', 'This Week', 'Last Week', 'This Year', 'Last Year'],
            dateFormat: 'M/d/yyyy',
            outputFormat: 'DD/MM/YYYY',
            startOfWeek: 0,
            display: { to: 'block', tm: 'block', lm: 'block', lw: 'block', tw: 'block', ty: 'block', ly: 'block', custom: 'block', em: 'block' }
        };
        this.dtOptions = {
            displayLength: 10,
            aaData: [],
            'select': {
                style: 'single',
                info: false,
                selector: 'td:not(:last-child)'
            },
            'iDisplayLength': 10,
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [
                { sTitle: 'ID', mData: 'ID', bVisible: false, sClass: 'text-center' },
                { sTitle: 'LoanID', mData: 'LoanID', bVisible: false, sClass: 'text-center' },
                { sTitle: 'Loan Number', mData: 'LoanNumber', sClass: 'text-center' },
                { sTitle: 'IDC BatchID', mData: 'EphesoftBatchInstanceID', sClass: 'text-center' },
                { sTitle: 'File Name', mData: 'FileName'},
                { sTitle:  AppSettings.AuthorityLabelSingular + ' Name', mData: 'CustomerName' },
                { sTitle: 'Service Type', mData: 'ReviewTypeName' },
                { sTitle: 'LoanType', mData: 'LoanTypeName' },
                { sTitle: 'Status', mData: 'Status' },
                { sTitle: 'ModifiedOn', 'type': 'date', mData: 'ModifiedOn', bVisible: false },
                { sTitle: 'Error Message', mData: 'ErrorMsg' },
                { sTitle: 'View', mData: 'ID', sClass: 'text-center' },
                { sTitle: 'FileType', mData: 'ID', bVisible: false }

            ],
            aoColumnDefs: [

                 {
                     'aTargets': [8],
                     'mRender': function (data, type, row) {
                         return '<label title="' + LOSExportStagingStatusConstant.LOS_EXPORT_STAGING_STATUS_DESCRIPTION[row['Status']] + '" class="bcEllipsis label ' + LOSExportStagingStatusConstant.LOS_EXPORT__STAGING_STATUS_COLOR[row['Status']] + ' label-table">' + LOSExportStagingStatusConstant.LOS_EXPORT_STAGING_STATUS_DESCRIPTION[row['Status']] + '</label>';

                     }
                 },
                {
                    'aTargets': [11],
                    'mRender': function (date) {
                        return '<span class="ViewLOSExportDetails material-icons txt-info">pageview</span>';

                    }
                }
            ],
            rowCallback: (row: Node, data: object, index: number) => {
                const self = this;

                $('td .ViewLOSExportDetails', row).unbind('click');
                $('td .ViewLOSExportDetails', row).bind('click', () => {
                    self.GetCurrentLOSExportStagingDetails(row, data);
                });

                return row;
            }
        };
        this.LosExportStagingdtOptions = {
            displayLength: 10,
            aaData: [],
            'select': {
                style: 'single',
                info: false,
                selector: 'td:not(:last-child)'
            },
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            'iDisplayLength': 5,
            aoColumns: [
                { sTitle: 'ID', mData: 'ID', bVisible: false, sClass: 'text-center' },
                { sTitle: 'LoanID', mData: 'LoanID', bVisible: false, sClass: 'text-center' },
                { sTitle: 'File Name', mData: 'FileName', sClass: 'text-left' },
                { sTitle: 'File type', mData: 'FileTypeName', sClass: 'text-left'},
                { sTitle: 'Trailing batch id', mData: 'TrailingBatchId', sClass: 'text-center', sWidth: '20%'},
                { sTitle: 'Processed On', mData: 'ModifiedOn', sClass: 'text-center', sWidth: '20%' },
                { sTitle: 'Status', mData: 'Status', sClass: 'text-center table-panel', sWidth: '20%' },
                { sTitle: 'Retry', mData: 'ID', sClass: 'text-center', sWidth: '10%' },

            ],
            aoColumnDefs: [
                {
                    'aTargets': [5],
                    'mRender': function (date) {
                        return isTruthy(date) ? convertDateTimewithTime(date) : '';
                    }
                },
                {
                    'aTargets': [6],
                    'mRender': function (data, type, row) {
                        return '<label class=\'label ' + LOSExportStatusConstant.LOS_EXPORT_STATUS_COLOR[row['Status']] + ' label-table\'>' + LOSExportStatusConstant.LOS_EXPORT_STATUS_DESCRIPTION[row['Status']] + '</label>';

                    }
                },
                {
                    'aTargets': [7],
                    'mRender': function (data, type, row) {
                        if (row.Status === LOSExportStatusConstant.LOS_LOAN_ERROR) {
                            return '<span style="cursor:pointer" class="LosExportRetry btn fa fa-undo"></span>';
                        } else {
                            return '';
                        }

                    }
                }

            ],
            rowCallback: (row: Node, data: any[] | Object, index: number) => {
                const self = this;

                $('td .LosExportRetry', row).unbind('click');
                $('td .LosExportRetry', row).bind('click', () => {
                    self.LosExportRetry(row, data);
                });

                return row;
            }
        };
        this.commonmasterservice.getReviewTypeList();
        this.commonmasterservice.GetSystemLoanTypes();
        this.commonmasterservice.GetCustomerList(AppSettings.TenantSchema);
        this.subscription.push(
            this._exportservice.LosMonitorDetails$.subscribe((result) => {
                this.LosExportMonitorTable.clear();
                this.LosExportMonitorTable.rows.add(result);
                this.LosExportMonitorTable.draw();
            })
        );
        this.subscription.push(
            this._exportservice.retryLOSexport.subscribe((result) => {
                this.RetryConfirmModel.hide();
                this.LosExportDetailsModal.hide();
                this.SearchLOSExportDetails(this.receivedDate.dateFrom, this.receivedDate.dateTo);
            })
        );
        this.subscription.push(
            this.commonmasterservice.CustomerItems.subscribe(
                (result: any) => {
                    this.commonActiveCustomerItems = result;
                }
            )
        );
        this.subscription.push(
            this.commonmasterservice.SystemLoanTypeItems.subscribe(
                (result: any) => {
                    this.commonActiveLoanTypeItems = result;
                }
            )
        );
        this.subscription.push(
            this.commonmasterservice.reviewTypeList.subscribe(
                (result: any) => {
                    this.commonActiveReviewTypeItems = result;
                }
            )
        );
        this.subscription.push(
            this._exportservice.LosExportStagingDetails$.subscribe((result) => {
                this.LosExportStagingMonitorTable.clear();
                this.LosExportStagingMonitorTable.rows.add(result);
                this.LosExportStagingMonitorTable.draw();
                if (result != null) {
                    this.LosExportDetailsModal.show();

                }

            })
        );
    }
    LosExportRetry(rowIndex: Node, Data: any) {
        this.rowData = Data;
        this.RetryFileType = Data.FileTypeName;
        this.RetryConfirmModel.show();

    }
    RetryLOSexportDetails() {
        const InputReq = new RetryLOSExportModel(AppSettings.TenantSchema, this.rowData.ID, this.rowData.LoanID);
        this._exportservice.RetryLOSexportDetails(InputReq);

    }
    SearchLOSExportDetails(dateFrom: any, dateTo: any) {
        let fromDateStr, toDatStr;
        if (isTruthy(dateFrom) && isTruthy(dateTo)) {
            fromDateStr = formatDate(dateFrom);
            toDatStr = formatDate(dateTo);
        } else {
            fromDateStr = null;
            toDatStr = null;
        }
        this.LosExportMonitorFromdate = fromDateStr;
        this.LosExportMonitorTodate = toDatStr;
        const Losexportmodel = { FromDateStr: fromDateStr, ToDateStr: toDatStr };
        const InputReq = new LosExportdateModel(AppSettings.TenantSchema, this.customerSelect, Losexportmodel, this.LoanTypeSelect, this.ReviewTypeSelect);
        this.promise = this._exportservice.SearchLOSExportDetails(InputReq);

    }
    ngAfterViewInit() {
        this.dataTableElement.forEach((dtElement: DataTableDirective) => {
            dtElement.dtInstance.then((dtInstance: any) => {
                if (dtInstance.context[0].sTableId === 'LosExportDetails') {
                    this.LosExportMonitorTable = dtInstance;
                    if (isTruthy(this.LosExportMonitorTable)) {
                        this.SearchLOSExportDetails(this.receivedDate.dateFrom, this.receivedDate.dateTo);
                    }
                } else if (dtInstance.context[0].sTableId === 'LosExportStagingDetails') {
                    this.LosExportStagingMonitorTable = dtInstance;
                }
            });
        });
    }

    GetCurrentLOSExportStagingDetails(rowIndex: Node, Data: any) {
        this.rowData = Data;
        const InputReq = new RetryLOSExportModel(AppSettings.TenantSchema, this.rowData.ID, this.rowData.LoanID);
        this._exportservice.GetCurrentLOSExportStagingDetails(InputReq);
    }
    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
