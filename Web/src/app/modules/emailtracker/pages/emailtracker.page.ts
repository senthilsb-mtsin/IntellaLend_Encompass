import { OnInit, Component, ViewChild, OnDestroy, AfterViewInit } from '@angular/core';
import { convertDateTime } from '@mts-functions/convert-datetime.function';
import { DataTableDirective } from 'angular-datatables';
import { AppSettings } from '@mts-app-setting';
import { EmailTrackerService } from '../service/emailtracker.service';
import { Subscription } from 'rxjs';
import { GetRowDataRequest } from '../models/get-row-data.model';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DatePipe } from '@angular/common';
import { EmailModalModel } from '../models/email-modal-model';
import { SearchEmailTrackerDetailsRequest } from '../models/search-email-tracker-details-request.model';

@Component({
    selector: 'mts-emailtracker',
    templateUrl: 'emailtracker.page.html',
    styleUrls: ['emailtracker.page.css']
})

export class EmailtrackerComponent implements OnInit, OnDestroy, AfterViewInit {
    @ViewChild('EmailconfirmModal') EmailconfirmModal: ModalDirective;
    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
    _emailModalValues: EmailModalModel = new EmailModalModel();
    RuleFindings: any = [];
    dtOptions: any = {};
    datevalue: any;
    daterangeoptions: any;
    IsRuleFindings = false;
    IsRulesAvailable = false;
    constructor(private _emailTrackerService: EmailTrackerService, private datePipe: DatePipe) { }
    private subscription: Subscription[] = [];
    private dTable: any = {};
    private dateFrom: any;
    private dateTo: any;
    private rowData: any = [];
    private selectedRow: any = {};

    ngOnInit(): void {
        this.subscription.push(this._emailTrackerService.setEmailTrackerTableData.subscribe((res: any) => {
            this.dTable.clear();
            this.dTable.rows.add(res);
            this.dTable.draw();
        }));
        this.subscription.push(this._emailTrackerService._emailModal.subscribe((res: any) => {
            this._emailModalValues = res;
            if (res.TemplateID === 4) {
                this.IsRuleFindings = true;
                this.IsRulesAvailable = true;
                this.RuleFindings = JSON.parse(res.Body);
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
            displayLength: 10,
            aaData: [],
            'select': {
                style: 'single',
                info: false
            },
            'iDisplayLength': 10,
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [
                { sTitle: 'Date', 'type': 'date', mData: 'CreatedOn', sClass: 'text-center' },
                { sTitle: 'Borrower Loan #', mData: 'LoanNumber', sClass: 'text-center' },
                { sTitle: 'Status', mData: 'Delivered', sClass: 'text-center', sWidth: '10%' },
                { sTitle: 'Sent By', mData: 'SendBy', sClass: 'text-center' },
                { sTitle: 'Error Message', mData: 'ErrorMessage', sClass: 'text=center' },
                { sTitle: 'View', mData: 'ID', sClass: 'text-center', sWidth: '2%' },
            ],
            aoColumnDefs: [
                {
                    'aTargets': [0],
                    'mRender': function (date) {
                        if (isTruthy(date)) {
                            return convertDateTime(date);
                        } else {
                            return date;
                        }
                    }
                },
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
                }

            ],
            rowCallback: (row: Node, data: any[] | Object, index: number) => {
                const self = this;
                $('td .viewemailtracker', row).unbind('click');
                $('td .viewemailtracker', row).bind('click', () => {
                    self.GetRowData(row, data);
                });
                $('td:first-child', row).unbind('click');
                $('td:first-child', row).bind('click', () => {
                    self.RowSelect(row, data);
                });
                return row;
            }
        };
    }

    ngAfterViewInit() {
        this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
            this.dTable = dtInstance;
            if (isTruthy(this.dTable)) {
                this.dateFrom = new Date();
                this.dateTo = new Date();
                this.SearchEmailTracker(this.dateFrom, this.dateTo);
            }
        });
    }

    SearchEmailTracker(dateFrom: any, dateTo: any) {
        const fromDate = this.datePipe.transform(dateFrom, AppSettings.dateFormat);
        const toDate = this.datePipe.transform(dateTo, AppSettings.dateFormat);
        const _emailtracker = { FromDateStr: fromDate, ToDateStr: toDate };
        const req = new SearchEmailTrackerDetailsRequest(AppSettings.TenantSchema, _emailtracker);
        this._emailTrackerService.SearchEmailTracker(req);

    }
    RowSelect(rowIndex: Node, rowData: any) {
        this.rowData = rowData;
    }
    GetCurrentData(row: any) {
        const req = new GetRowDataRequest(AppSettings.TenantSchema, row.ID);
        this._emailTrackerService.GetCurrentData(req);
        this.EmailconfirmModal.show();
    }

    GetRowData(row: Node, rowData: any): void {
        this.selectedRow = $(row).hasClass('selected');
        if (isTruthy(rowData)) {
            this.GetCurrentData(rowData);
        }
    }
    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
