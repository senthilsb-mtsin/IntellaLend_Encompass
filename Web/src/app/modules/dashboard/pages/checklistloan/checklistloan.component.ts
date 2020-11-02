import { AppSettings, ReportTypeConstant } from '@mts-app-setting';
import { StatusConstant } from '@mts-status-constant';
import { SessionHelper } from '@mts-app-session';
import { DashboardService } from '../../service/dashboard.service';
import { ViewChild, Component, OnInit, AfterViewInit } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Subscription } from 'rxjs';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Router } from '@angular/router';
import { NotificationService } from '@mts-notification';
import { ReportServiceModel } from '../../models/reporting.model';

@Component({
    selector: 'mts-checklistloan',
    templateUrl: 'checklistloan.component.html',
    styleUrls: ['checklistloan.component.scss']
})
export class ChecklistloanComponent implements OnInit, AfterViewInit {
    dtOptions: any = {};
    data: any;
    createTableInstance = false;
    @ViewChild(DataTableDirective) datatableEl: DataTableDirective;
    dTable: any;
    promise: Subscription;
    _btnSearchClick = false;
    tableAligned = false;

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
            displayLength: 10,
            dom: 'Blfrtip',
            buttons: [
                {
                    extend: 'excel',
                    className: 'btn btn-sm btn-info waves-effect waves-light',
                    text: '<i class="fa fa-file-excel-o"></i> Download',
                    filename: this._reportService.ReportDescription == null ? '' : this._reportService.ReportDescription,
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 7, 8, (this._reportService.ReportType === ReportTypeConstant.CRITICAL_RULES_FAILED.Name) ? 17 : 16 ]
                    },
                    title: this._reportService.ReportDescription == null ? '' : this._reportService.ReportDescription
                }
            ],
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
                { sTitle: 'Borrower Name', mData: 'BorrowerName', bVisible: (this._reportService.ReportType === ReportTypeConstant.CRITICAL_RULES_FAILED.Name) },
                { sTitle: 'Service Type', mData: 'ServiceTypeName', bVisible: (this._reportService.ReportType === ReportTypeConstant.LOAN_FAILED_RULES.Name) },
                { sTitle: 'Loan Type', mData: 'LoanTypeName' },
                { sTitle: 'Loan Amount', mData: 'LoanAmount', bVisible: (this._reportService.ReportType === ReportTypeConstant.CRITICAL_RULES_FAILED.Name) },
                { sTitle: 'Category Name', mData: 'CategoryName', bVisible: false },
                { sTitle: 'Audit Period', mData: 'AuditMonthYear', bVisible: false },
                { sTitle: ' Borrower Loan #', mData: 'LoanNumber' },
                { sTitle: 'DE Name', mData: 'DataEntryName', bVisible: (this._reportService.ReportType === ReportTypeConstant.LOANQC_INDEX.Name) },
                { sTitle: 'IDC Accuracy (%)', mData: 'OCRAccuracy', bVisible: false },
                { sTitle: 'Loan officer', mData: 'LoanOfficer', bVisible: (this._reportService.ReportType !== ReportTypeConstant.CRITICAL_RULES_FAILED.Name) },
                { sTitle: 'UW', mData: 'UnderWritter', bVisible: (this._reportService.ReportType !== ReportTypeConstant.CRITICAL_RULES_FAILED.Name) },
                { sTitle: 'Closer', mData: 'Closer', bVisible: (this._reportService.ReportType !== ReportTypeConstant.CRITICAL_RULES_FAILED.Name) },
                { sTitle: 'Post-closer', mData: 'PostCloser', bVisible: (this._reportService.ReportType !== ReportTypeConstant.CRITICAL_RULES_FAILED.Name) },
                { sTitle: 'Investor', mData: 'Investor', bVisible: false },
                { sTitle: 'Failed Rule Count', mData: 'FailedRulesCount', bVisible: (this._reportService.ReportType !== ReportTypeConstant.CRITICAL_RULES_FAILED.Name) },
                { sTitle: 'Failed Rule Count', mData: 'CriticalRulesCount', bVisible: (this._reportService.ReportType === ReportTypeConstant.CRITICAL_RULES_FAILED.Name) },
                { sTitle: 'View', mData: 'LoanID', sClass: 'text-center' },
                { sTitle: 'StatusDescription', mData: 'StatusDescription', bVisible: false },
                { sTitle: '', mData: 'StatusID', bVisible: false },
            ],
            aoColumnDefs: [
                {
                    'aTargets': [18],
                    'mRender': function (data, type, row) {
                        if (row['StatusID'] === StatusConstant.COMPLETE || row['StatusID'] === StatusConstant.PENDING_AUDIT) {
                            return '<span style=\'cursor: pointer;\' title=\'View Loan\' class=\'viewLoan material-icons txt-info\'>pageview</span>';
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

    ngAfterViewInit() {

        if (!this.createTableInstance) {
            this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
                this.dTable = dtInstance;
                this.createTableInstance = true;
                this._btnSearchClick = true;
                this.promise =  this._dashboardService.getcheckListFailedLoans(this._btnSearchClick, this.dTable);
                if (typeof this.dTable !== 'undefined' && !this.tableAligned) {
                    $('.dataTables_info').addClass('col-md-6 p0');
                    $('.dataTables_paginate').addClass('col-md-6 p0');
                    $('.dataTables_filter').addClass('col-md-6 p0');
                    $('.dataTables_length').addClass('col-md-6 p0');

                    $('.dt-buttons').appendTo('#downloadButton');
                    this.tableAligned = true;
                }
            });
        }
    }

    getRowData(row: Node, rowData: any): void {
        if (typeof rowData !== 'undefined') {
            this._dashboardService.setDataAndRoute(rowData);
            // this._dashboardService.checkCurrentUser(rowData, this.AlertMessage, this.currentLoanID, this.currentRow, this.confirmModal);
        } else {
            this._notificationService.showInfo('Row Not Fetched');
        }
    }

    back() {
        this._route.navigate(['view/dashboard']);
    }

}
