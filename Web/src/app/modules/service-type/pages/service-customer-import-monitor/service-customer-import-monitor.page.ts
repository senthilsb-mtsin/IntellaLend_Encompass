import { Component, QueryList, ViewChild, ViewChildren, AfterViewInit, OnDestroy, OnInit} from '@angular/core';
import { NgDateRangePickerComponent, NgDateRangePickerOptions } from '@mts-daterangepicker/ng-daterangepicker.component';
import { convertDateTimewithTime } from '@mts-functions/convert-datetime.function';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { NotificationService } from '@mts-notification';
import { CustomerImportStatusConstants } from 'src/app/shared/constant/status-constants/customer-import-constants';
import { Location } from '@angular/common';
import { AppSettings, CustomerImportAssignTypeConstant } from '@mts-app-setting';
import { DataTableDirective } from 'angular-datatables';
import { ServiceTypeService } from '../../service/service-type.service';
import { ServiceTypeDataAccess } from '../../service-type.data';
import { Subscription } from 'rxjs';
import { CustomerImportStagingDetailsRequestModel, CustomerImportStagingRequestModel } from 'src/app/modules/customer/models/customer-import.model';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { CharcCheckPipe, CheckDuplicateName, ValidateZipcodePipe } from '@mts-pipe';

@Component({
    selector: 'mts-service-customer-import-monitor',
    styleUrls: ['service-customer-import-monitor.page.css'],
    templateUrl: 'service-customer-import-monitor.page.html',
    providers: [ServiceTypeService, CharcCheckPipe, ValidateZipcodePipe, CheckDuplicateName]
})
export class ServiceCustomerImportMonitorComponent implements OnInit, OnDestroy, AfterViewInit {
    Dateoptions: NgDateRangePickerOptions;
    dtOptionsStaging: any = {};
    SelectedCustomerImportStatus = 0;
    dtOptionsStagingDetails: any = {};
    CustomerImportStatusDropdown: any = [];
   AuthorityLabelSingular: string = AppSettings.AuthorityLabelSingular;
    ServiceCustomerImportStagingDataTable: any = [];
    promise: Subscription;
    ServiceCustomerImportStagingDetailsDataTable: any = [];
    @ViewChild(NgDateRangePickerComponent) ImportDate: NgDateRangePickerComponent;
    @ViewChildren(DataTableDirective) dataTableElement: QueryList<DataTableDirective>;
    @ViewChild('CustomerImportStagingDetailsModal') CustomerImportStagingDetailsModal: ModalDirective;

    constructor(
        private _serviceTypeService: ServiceTypeService,
        private _serviceTypeData: ServiceTypeDataAccess,
        private _notificationService: NotificationService,
        private location: Location
    ) {

    }
    private _subscriptions: Subscription[] = [];

    ngOnInit(): void {
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
        this.dtOptionsStaging = {
            displayLength: 10,
            'iDisplayLength': 10,
            'order': [[0, 'desc']],
            scrollX: true,
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [
                { mData: 'ID', sTitle: 'ID', sClass: 'text-left', bVisible: false },
                { mData: 'FilePath', sTitle: 'File path', sClass: 'text-left', sWidth: '35%' },
                { mData: 'ImportCount', sTitle: 'Import count', sClass: 'text-right', sWidth: '15%' },
                { mData: 'CreatedOn', sTitle: 'Created on', sClass: 'text-center', bVisible: false },
                { mData: 'ModifiedOn', sTitle: 'Imported on', sClass: 'text-center', sWidth: '10%' },
                { mData: 'ErrorMsg', sTitle: 'Error message', sClass: 'text-left', sWidth: '20%' },
                { mData: 'Status', sTitle: 'Status', sClass: 'text-center', sWidth: '15%' },
                { mData: 'AssignType', sTitle: 'Assign Type', sClass: 'text-right', sWidth: '15%', bVisible: false},
                { mData: 'ID', sTitle: 'Details', sClass: 'text-center', sWidth: '10%' }
            ],
            aoColumnDefs: [
                {
                    'aTargets': [3, 4],
                    'mRender': function (data, type, row) {
                        return isTruthy(data) ? convertDateTimewithTime(data) : '';
                    }
                },
                {
                    'aTargets': [6],
                    'mRender': function (data, type, row) {
                        return '<label class=\'label ' + CustomerImportStatusConstants.StatusColor[data] + ' label-table\'>' + CustomerImportStatusConstants.Description[data] + '</label>';
                    }
                },
                {
                    'aTargets': [8],
                    'mRender': function (data, type, row) {
                        return '<span class="ViewCustomerImportStagingDetails material-icons txt-info">pageview</span>';
                    }
                }
            ],
            rowCallback: (row: Node, data: any, index: number) => {
                const self = this;

                $('td .ViewCustomerImportStagingDetails', row).unbind('click');
                $('td .ViewCustomerImportStagingDetails', row).bind('click', () => {
                    self.GetServiceCustomerImportStagingDetails(data['ID']);
                });

                return row;
            }
        };
        this.dtOptionsStagingDetails = {
            displayLength: 10,
            'iDisplayLength': 10,
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [
                { mData: 'ID', sTitle: 'ID', sClass: 'text-left', bVisible: false },
                { mData: 'CustomerImportStagingID', sTitle: this.AuthorityLabelSingular + ' Import Staging ID', sClass: 'text-left', bVisible: false },
                { mData: 'CustomerName', sTitle: this.AuthorityLabelSingular + ' Name', sClass: 'text-left', sWidth: '15%' },
                { mData: 'CustomerCode', sTitle: this.AuthorityLabelSingular + ' Code', sClass: 'text-left', sWidth: '15%' },
                { mData: 'State', sTitle: 'State', sClass: 'text-left', sWidth: '10%' },
                { mData: 'Country', sTitle: 'Country', sClass: 'text-left', sWidth: '10%' },
                { mData: 'Zip', sTitle: 'Zip', sClass: 'text-left', sWidth: '10%' },
                { mData: 'ServiceType', sTitle: 'Service Type', sClass: 'text-left', sWidth: '10%' },
                { mData: 'LoanType', sTitle: 'Loan Type', sClass: 'text-left', sWidth: '10%' },
                { mData: 'CreatedOn', sTitle: 'Created on', sClass: 'text-center', bVisible: false },
                { mData: 'ModifiedOn', sTitle: 'Imported on', sClass: 'text-center', bVisible: false },
                { mData: 'ErrorMsg', sTitle: 'Error Message', sClass: 'text-left', sWidth: '10%' },
                { mData: 'Status', sTitle: 'Status', sClass: 'text-center', sWidth: '10%' },

            ],
            aoColumnDefs: [
                {
                    'aTargets': [9, 10],
                    'mRender': function (data, type, row) {
                        return isTruthy(data) ? convertDateTimewithTime(data) : '';
                    }
                },
                {
                    'aTargets': [12],
                    'mRender': function (data, type, row) {
                        return '<label class=\'label ' + CustomerImportStatusConstants.StatusColor[data] + ' label-table\'>' + CustomerImportStatusConstants.Description[data] + '</label>';
                    }
                }
            ],
            rowCallback: (row: Node, data: any, index: number) => {

                return row;
            }
        };
        this._subscriptions.push(this._serviceTypeService.ServiceCustomerImportStagingDatatable$.subscribe(res => {
            this.ServiceCustomerImportStagingDataTable.clear();
            this.ServiceCustomerImportStagingDataTable.rows.add(res);
            this.ServiceCustomerImportStagingDataTable.draw();
        }));

        this._subscriptions.push(this._serviceTypeService.ServiceCustomerImportStagingDetailsDatatable$.subscribe(res => {
            this.ServiceCustomerImportStagingDetailsDataTable.clear();
            this.ServiceCustomerImportStagingDetailsDataTable.rows.add(res);
            this.ServiceCustomerImportStagingDetailsDataTable.draw();

            this.CustomerImportStagingDetailsModal.show();
        }));

        CustomerImportStatusConstants.StatusDropdown.forEach((element) => {
            this.CustomerImportStatusDropdown.push({ Value: element, Text: CustomerImportStatusConstants.Description[element] });
        });

    }
    ngAfterViewInit(): void {
        this.dataTableElement.forEach((dtElement: DataTableDirective) => {
            dtElement.dtInstance.then((dtInstance: any) => {
                if (dtInstance.context[0].sTableId === 'ServiceCustomerImportStagingDataTable') {
                    this.ServiceCustomerImportStagingDataTable = dtInstance;
                    if (isTruthy(this.ServiceCustomerImportStagingDataTable)) {
                        this.SearchCustomerImportStaging();
                    }
                } else if (dtInstance.context[0].sTableId === 'ServiceCustomerImportStagingDetailsDataTable') {
                    this.ServiceCustomerImportStagingDetailsDataTable = dtInstance;
                }
            });
        });

    }
    GetServiceCustomerImportStagingDetails(CustomerImportStagingID: number) {
        const req: CustomerImportStagingDetailsRequestModel = new CustomerImportStagingDetailsRequestModel(AppSettings.TenantSchema, CustomerImportStagingID);
        this.promise = this._serviceTypeService.GetServiceCustomeImportStagingDetails(req);
    }
    SearchCustomerImportStaging() {
        const req: CustomerImportStagingRequestModel = new CustomerImportStagingRequestModel(
            AppSettings.TenantSchema,
            this.SelectedCustomerImportStatus,
            this.ImportDate.dateFrom,
            this.ImportDate.dateTo,
            CustomerImportAssignTypeConstant.SERVICE_LENDER_IMPORT);

        this.promise = this._serviceTypeService.GetServiceCustomeImportStaging(req);
    }

    ResetSearch() {
        this.SelectedCustomerImportStatus = 0;
        this.ImportDate.selectRange('to');
        this.SearchCustomerImportStaging();
    }

    GotoPrevious() {
        this.location.back();
    }
    ngOnDestroy(): void {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
