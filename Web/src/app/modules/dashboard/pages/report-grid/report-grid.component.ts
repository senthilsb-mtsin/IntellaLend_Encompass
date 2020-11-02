import { Component, ViewChild, AfterViewInit, AfterContentChecked, OnInit } from '@angular/core';
import { DashboardService } from '../../service/dashboard.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ReportTypeConstant, AppSettings } from '@mts-app-setting';
import { StatusConstant } from '@mts-status-constant';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DataTableDirective } from 'angular-datatables';
import { convertDateTime } from '@mts-functions/convert-datetime.function';
import { NotificationService } from '@mts-notification';
import { ReportServiceModel } from '../../models/reporting.model';

@Component({
    moduleId: 'ReportGridComponent',
    selector: 'mts-report',
    templateUrl: 'report-grid.component.html',
    styleUrls: ['report-grid.component.css']
})
export class ReportGridComponent implements OnInit, AfterViewInit, AfterContentChecked {

    UserName  = '';
    promise: Subscription;
    dtOptions: any = {};
    data: any;

    _btnSearchClick = false;
    dTable: any;
    tableAligned: boolean;
    createTableInstance = false;
    @ViewChild(DataTableDirective) datatableEl: DataTableDirective;

    @ViewChild('confirmModal') confirmModal: ModalDirective;
    AlertMessage: string;
    currentLoanID = 0;
    currentRow: any;

    constructor(public _reportService: ReportServiceModel,
        private _dashboardService: DashboardService,
        private _route: Router,
        private _notificationService: NotificationService) {
    }

    ngOnInit() {

        this.dtOptions = {
            dom: 'Blfrtip',
            buttons: [
                {
                    extend: 'excel',
                    className: 'btn btn-sm btn-info waves-effect waves-light',
                    text: '<i class="fa fa-file-excel-o"></i> Download',
                    filename: this._reportService.ReportDescription == null ? '' : this._reportService.ReportDescription,
                    exportOptions: {
                        columns: this._reportService.ReportType === ReportTypeConstant.MISSING_CRITICAL_DOCUMENT.Name || this._reportService.ReportType === ReportTypeConstant.DATAENTRY_WORKLOAD.Name ? [1, 3, 5, 8] :
                            this._reportService.ReportType === ReportTypeConstant.MISSING_RECORDED_LOANS.Name ? [1, 3, 5, 8, 9] : [1, 3, 5, ]
                    },
                    title: this._reportService.ReportDescription == null ? '' : this._reportService.ReportDescription
                }
            ],
            displayLength: 10,
            aaData: this.data,
            'select': {
                style: 'single',
                info: false
            },
            'iDisplayLength': 10,
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [
                { sTitle: 'LoanID', mData: 'LoanID', bVisible: false },
                { sTitle: AppSettings.AuthorityLabelSingular + ' Name', mData: 'CustomerName' },
                { sTitle: 'Service Type', mData: 'ServiceTypeName', bVisible: false },
                { sTitle: 'Loan Type', mData: 'LoanTypeName' },
                { sTitle: 'Audit Period', mData: 'AuditMonthYear', bVisible: false },
                { sTitle: 'Borrower Loan #', mData: 'LoanNumber' },
                { sTitle: 'DE Name', mData: 'DataEntryName', bVisible: false },
                { sTitle: 'IDC Accuracy (%)', mData: 'OCRAccuracy', bVisible: false },
                {
                    sTitle: 'No of Days Aged', mData: 'NoofDaysAged', sClass: 'text-center', bVisible: (this._reportService.ReportType === ReportTypeConstant.MISSING_CRITICAL_DOCUMENT.Name || this._reportService.ReportType === ReportTypeConstant.DATAENTRY_WORKLOAD.Name ||
                        this._reportService.ReportType === ReportTypeConstant.MISSING_RECORDED_LOANS.Name)
                },
                { sTitle: 'Missing Document Names', mData: 'MissingDocumentNames', sClass: 'text-center', bVisible: this._reportService.ReportType === ReportTypeConstant.MISSING_RECORDED_LOANS.Name },
                { sTitle: 'Completed Audit Date', mData: 'LoanAuditCompleteDate', sClass: 'text-center', bVisible: this._reportService.ReportType === ReportTypeConstant.KPI_GOAL_CONFIGURATION.Name },
                { sTitle: 'View', mData: 'LoanID', sClass: 'text-center' },
                { sTitle: 'StatusDescription', mData: 'StatusDescription', bVisible: false },
                { sTitle: '', mData: 'StatusID', bVisible: false }
            ],
            aoColumnDefs: [
                {
                    'aTargets': [11],
                    'mRender': function (data, type, row) {
                        if (row['StatusID'] === StatusConstant.COMPLETE || row['StatusID'] === StatusConstant.PENDING_AUDIT) {
                            return '<span style=\'cursor: pointer;\' title=\'View Loan\' class=\'viewLoan material-icons txt-info\'>pageview</span>';
                        } else {
                            return '';
                        }
                    }
                },
                {
                    'aTargets': [9],
                    'mRender': function (data, type, row) {

                        let result = '';
                        const docs = data.toString().split('|');

                        docs.forEach(element => {
                            result += element + '<br>';
                        });

                        return result;
                    }
                },
                {
                    'aTargets': [10],
                    'mRender': function (data, type, row) {

                        if (data !== undefined && data !== null) {
                          return  convertDateTime(data);
                        } else {
                            return '';
                        }
                    }
                }
            ],
            rowCallback: (row: Node, data: any[] | Object, index: number) => {
                const self = this;
                $('td .viewLoan', row).unbind('click');
                $('td .viewLoan', row).bind('click', () => {
                    self.getRowData(row, data);
                });

                return row;
            }
        };
    }

    ngAfterContentChecked() {
        if (typeof this.dTable !== 'undefined' && !this.tableAligned) {
            $('.dataTables_info').addClass('col-md-6 p0');
            $('.dataTables_paginate').addClass('col-md-6 p0');
            $('.dataTables_filter').addClass('col-md-6 p0');
            $('.dataTables_length').addClass('col-md-6 p0');

            $('.dt-buttons').appendTo('#downloadButton');
            this.tableAligned = true;
        }
    }

    ngAfterViewInit() {
        if (!this.createTableInstance) {
            this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
                this.dTable = dtInstance;
                this.createTableInstance = true;
                this._btnSearchClick = true;
                this.UserName = this._reportService.ReportModel.UserName;
                this.promise = this._dashboardService.getcheckListFailedLoans(this._btnSearchClick, this.dTable);
            });
        }
    }

    getRowData(row: Node, rowData: any): void {
        if (typeof rowData !== 'undefined') {
            this._dashboardService.checkCurrentUser(rowData, this.AlertMessage, this.currentLoanID, this.currentRow, this.confirmModal);
        } else {
            this._notificationService.showInfo('Row Not Fetched');
        }
    }

    back() {
        this._route.navigate(['view/dashboard']);
    }

    overrideLoanUser() {
      this._dashboardService.setLoanPickUpUser(this.currentRow);
    }
}
