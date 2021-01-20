import { Component, ViewChild, OnInit, AfterViewInit, AfterContentChecked } from '@angular/core';
import { DashboardService } from '../../service/dashboard.service';
import { DataTableDirective } from 'angular-datatables';
import { Subscription } from 'rxjs';
import { convertDateTimewithTime } from '@mts-functions/convert-datetime.function';
import { NotificationService } from '@mts-notification';
import { AppSettings } from '@mts-app-setting';
import { DatePipe } from '@angular/common';
import { MonthYearPickerComponent } from '@mts-month-year-picker/component/MonthYearPicker/MonthYearPicker.component';
import { verifyHostBindings } from '@angular/compiler';

@Component({
    selector: 'mts-idc-workload-report',
    templateUrl: 'idc-workload-report.page.html',
    styleUrls: ['idc-workload-report.page.css']
})
export class IdcWorkloadReportComponent implements OnInit, AfterViewInit, AfterContentChecked {

    @ViewChild('dateRange') daterange;
    daterangeoptions: any;
    datevalue: any;
    TempSelectedYearDate: any = new Date();
    ocrSelectedvalue: any = -1;
    ocrSelect = [{ id: 1, fieldvalue: 'Review Completion Date' }, { id: 2, fieldvalue: 'Validation Completion Date' }, { id: 3, fieldvalue: 'Audit Completion Date' }];
    FromDate: any;
    ToDate: any;
   _classificationTable: any;
    tableAligned = false;
    @ViewChild(DataTableDirective) datatableEl: DataTableDirective;
    @ViewChild('_idcMonthYear')
    _idcMonthYear: MonthYearPickerComponent;
    promise: Subscription;
    _classificationOptions: any = {};
    _classificationAuditMonthYear: any;

    //#region  Constructor
    constructor(private _dashboardService: DashboardService,
        private datePipe: DatePipe,
        private _notificationService: NotificationService) {
    }
    //#endregion Constructor

    ngOnInit(): void {
        this.selectThisMonth(this.TempSelectedYearDate);
        this.daterangeoptions = {
            theme: 'default',
            previousIsDisable: false,
            nextIsDisable: false,
            range: 'tm',
            dayNames: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
            presetNames: ['Today', 'This Month', 'Last Month', 'This Week', 'Last Week', 'This Year', 'Last Year'],
            dateFormat: 'M/d/yyyy',
            outputFormat: 'DD/MM/YYYY',
            startOfWeek: 0,
            display: { to: 'block', tm: 'block', lm: 'block', lw: 'block', tw: 'block', ty: 'none', ly: 'none', custom: 'block', em: 'block' }
        };

        this._classificationOptions = {
            displayLength: 10,
            aaData: [],
            dom: 'Blfrtip',
            buttons: [
                {
                    extend: 'excel',
                    className: 'btn btn-sm btn-info waves-effect waves-light m10',
                    text: '<i class="fa fa-file-excel-o"></i> Download',
                    filename: 'IDC-Extraction Report',
                    exportOptions: {
                        // columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 17]
                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 18, 19, 20, 21, 14, 15, 16, 17]
                    },
                    title: 'IDC-Extraction Report'
                }
            ],
            'order': [[0, 'desc']],
            'iDisplayLength': 10,
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [
                { sTitle: 'LoanID', mData: 'LoanID', bVisible: false },
                { sTitle: AppSettings.AuthorityLabelSingular + ' Name', mData: 'CustomerName', sWidth: '13%' },
                { sTitle: 'Service Type', mData: 'ServiceTypeName', sWidth: '17%', bVisible: false },
                { sTitle: 'IDC ID', mData: 'EphesoftBatchInstanceID', sClass: 'text-center', sWidth: '8%' },
                { sTitle: 'Borrower Loan #', mData: 'LoanNumber', sClass: 'text-left', sWidth: '8%' },
                { sTitle: 'Loan Type', mData: 'LoanTypeName', sWidth: '27%' },
                { sTitle: 'Review Completion Date', mData: 'ReviewCompletionDate', sClass: 'text-center', sWidth: '9%' },
                { sTitle: 'Validation Completion Date', mData: 'ValidationCompletionDate', sClass: 'text-center', sWidth: '9%' },
                { sTitle: 'Audit Completion Date', mData: 'AuditCompletionDate', sClass: 'text-center', sWidth: '9%' },
                { sTitle: 'Audit Duration', mData: 'LoanDuration', sClass: 'text-center' },
                { sTitle: 'IDC Reviewer Name', mData: 'EphesoftReviewerName', sWidth: '10%' },
                { sTitle: 'IDC Reviewer Duration', mData: 'IDCReviewDuration', sWidth: '10%' },
                { sTitle: 'IDC Validator Name', mData: 'EphesoftValidatorName', sWidth: '10%' },
                { sTitle: 'IDC Validator Duration', mData: 'IDCValidationDuration', sWidth: '10%' },
                { sTitle: 'Auditor Name', mData: 'AuditorName', sWidth: '10%' },
                { sTitle: 'Page Count', mData: 'PageCount', sClass: 'text-center' },
                { sTitle: 'Classification Accuracy (%)', mData: 'ClassificationAccuracy', sClass: 'text-center' },
                { sTitle: 'Extraction Accuracy (%)', mData: 'OCRAccuracy', sClass: 'text-center', sWidth: '14%' },
                { sTitle: 'IDC Reviewer Name', mData: 'MaxEphesoftReviewerName', bVisible: false },
                { sTitle: 'IDC Reviewer Duration', mData: 'MaxIDCReviewDuration', bVisible: false },
                { sTitle: 'IDC Validator Name', mData: 'MaxEphesoftValidatorName', bVisible: false },
                { sTitle: 'IDC Validator Duration', mData: 'MaxIDCValidationDuration', bVisible: false }

            ], aoColumnDefs: [{
                'aTargets': [10, 11, 12, 13],
                'mRender': function (data, type, row) {
                    if (typeof data !== 'undefined' && data != null) {
                        const tetst = data.toString().split('|');
                        let str = '';
                        tetst.forEach(function (elt) {
                            str += elt + ' <br>';
                        });
                        return str;
                    } else {
                        return '';
                    }
                }
            }, {
                'aTargets': [17],
                'mRender': function (data, type, row) {
                    if (typeof data !== 'undefined' && data !== null && row['OCRAccuracyCalculated'] === true) {
                        return '<a href=\'javascript:void(0)\'  class=\'viewDocument\' style=\'text-decoration:underline;\'>' + data + '</a>';
                    } else {
                        return '<a href=\'javascript:void(0)\' class=\'\' style=\'text-decoration:underline;\'>0.00</a>';
                    }
                }
            }, {
                'aTargets': [16],
                'mRender': function (data, type, row) {
                    if (typeof data !== 'undefined' && data !== null && row['OCRAccuracyCalculated'] === true) {
                        return data;
                    } else {
                        return '0.00';
                    }
                }
            }, {
                'aTargets': [6, 7, 8],
                'mRender': function (date, type, row) {
                    if (date !== null && date !== '') {
                        return convertDateTimewithTime(date);
                    } else {
                        return date;
                    }
                }
            }],
            rowCallback: (row: Node, data: any[] | Object, index: number) => {
                const self = this;
                $('td .viewDocument', row).unbind('click');
                $('td .viewDocument', row).bind('click', () => {

                    if (this._classificationTable.row(row).child.isShown()) {
                        this._classificationTable.row(row).child.hide();
                    } else {
                        self.GetOCRClassificationInnerTable(row, data);
                    }
                });
                return row;
            }
        };
    }

    ngAfterContentChecked() {
        if (typeof this._classificationTable !== 'undefined' && !this.tableAligned) {
            $('.dataTables_info').addClass('col-md-6 p0');
            $('.dataTables_paginate').addClass('col-md-6 p0');
            $('.dataTables_filter').addClass('col-md-6 p0');
            $('.dataTables_length').addClass('col-md-6 p0');

            $('.dt-buttons').appendTo('#downloadButton');
            this.tableAligned = true;
        }
    }

    ngAfterViewInit() {
        this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
            this._classificationTable = dtInstance;
            // this.GetOCRClassification(-1, this.daterange.dateFrom, this.daterange.dateTo);
        });
    }
    getMonthYear(event: any) {
        this.TempSelectedYearDate = this.datePipe.transform(
            event.Value,
            AppSettings.dateFormat
        );
        this.FromDate = this.TempSelectedYearDate;
        this.selectThisMonth(this.FromDate);
    }
    getDate(year, month, day) {
        return new Date(year, month, day, 0, 0, 0, 0);
    }
    selectThisMonth(_idcMonth) {
        const _idcMonthDate = new Date(_idcMonth);
        this.FromDate =
            new Date(_idcMonthDate.getFullYear(), _idcMonthDate.getMonth(), 1);
        this.ToDate = new Date(_idcMonthDate.getFullYear(), _idcMonthDate.getMonth() + 1, 0);
    }

    GetOCRClassification(ocrSelecteddata: any, TempDate: any) {
        const dateFrom = this.FromDate;
        const dateTo = this.ToDate;
        this.promise = this._dashboardService.GetOCRClassification(ocrSelecteddata, dateFrom, dateTo, this._classificationTable);

    }

    monthDiff(startDate, endDate) {
        const days = (endDate - startDate) / (1000 * 60 * 60 * 24);
        return Math.round(days);
    }

    GetOCRClassificationInnerTable(rowIndex: Node, rowData: any) {
        this._dashboardService.GetOCRClassificationInnerTable(rowIndex, rowData, this._classificationAuditMonthYear, this._classificationTable);
    }

}
