import { Component, ViewChild, OnInit, OnDestroy, AfterViewInit, QueryList, ViewChildren } from '@angular/core';
import { AppSettings, CustomerImportAssignTypeConstant } from '@mts-app-setting';
import { CustomerImportStatusConstants } from 'src/app/shared/constant/status-constants/customer-import-constants';
import { NgDateRangePickerComponent } from '@mts-daterangepicker/ng-daterangepicker.component';
import { NgDateRangePickerOptions } from '../../../../shared/custom-plugins/ng-daterangepicker-master/ng-daterangepicker.component';
import { CustomerImportStagingDetailsRequestModel, CustomerImportStagingRequestModel } from '../../models/customer-import.model';
import { UpsertCustomerService } from '../../services/upsert-customer.service';
import { Subscription } from 'rxjs/internal/Subscription';
import { CharcCheckPipe, CheckDuplicateName, ValidateZipcodePipe } from '@mts-pipe';
import { CustomerService } from '../../services/customer.service';
import { DataTableDirective } from 'angular-datatables';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { convertDateTimewithTime } from '@mts-functions/convert-datetime.function';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { CustomerApiUrlConstant } from 'src/app/shared/constant/api-url-constants/customer-api-url.constant';
import { JwtHelperService } from '@auth0/angular-jwt';
import { SessionHelper } from '@mts-app-session';
import { NotificationService } from '@mts-notification';
import { Location } from '@angular/common';

const jwtHelper = new JwtHelperService();
@Component({
    selector: 'mts-customer-import-moniter',
    styleUrls: ['customer-import-moniter.page.css'],
    templateUrl: 'customer-import-moniter.page.html',
    providers: [UpsertCustomerService, CharcCheckPipe, ValidateZipcodePipe, CheckDuplicateName]
})
export class CustomerImportMoniterComponent implements OnInit, OnDestroy, AfterViewInit {
    promise: Subscription;
    dtOptionsStaging: any = {};
    dtOptionsStagingDetails: any = {};
    Dateoptions: NgDateRangePickerOptions;
    AuthorityLabelSingular: string = AppSettings.AuthorityLabelSingular;
    SelectedCustomerImportStatus = 0;
    CustomerImportStatusDropdown: any = [];
    CustomerImportStagingDataTable: any = [];
    CustomerImportStagingDetailsDataTable: any = [];
    uploader: FileUploader;
    target: any;

    @ViewChild(NgDateRangePickerComponent) ImportDate: NgDateRangePickerComponent;
    @ViewChildren(DataTableDirective) dataTableElement: QueryList<DataTableDirective>;
    @ViewChild('CustomerImportStagingDetailsModal') CustomerImportStagingDetailsModal: ModalDirective;

    constructor(
        private _upsertCustomerService: UpsertCustomerService,
        private _customerService: CustomerService,
        private _notificationService: NotificationService,
        private location: Location
    ) {

    }

    private _subscriptions: Subscription[] = [];
    private foptions: FileUploaderOptions = {
        url: environment.apiURL + CustomerApiUrlConstant.UPLOAD_CUSTOMER_IMPORT_FILE,
        authTokenHeader: 'Authorization',
        authToken: 'Bearer ' + localStorage.getItem('id_token'),
        headers: [
            { name: 'UploadFileName', value: '' },
            { name: 'UserId', value: '' }
        ]
    };

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
                { mData: 'AssignType', sTitle: 'Assign Type', sClass: 'text-right', sWidth: '15%', bVisible: false },
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
                    self.GetCustomerImportStagingDetails(data['ID']);
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

        this._subscriptions.push(this._upsertCustomerService.CustomerImportStagingDatatable$.subscribe(res => {
            this.CustomerImportStagingDataTable.clear();
            this.CustomerImportStagingDataTable.rows.add(res);
            this.CustomerImportStagingDataTable.draw();
        }));

        this._subscriptions.push(this._upsertCustomerService.CustomerImportStagingDetailsDatatable$.subscribe(res => {
            this.CustomerImportStagingDetailsDataTable.clear();
            this.CustomerImportStagingDetailsDataTable.rows.add(res);
            this.CustomerImportStagingDetailsDataTable.draw();

            this.CustomerImportStagingDetailsModal.show();
        }));

        this.uploader = new FileUploader(this.foptions);

        this.uploader.onBeforeUploadItem = (item: FileItem) => {
            if (SessionHelper.UserDetails.UserID > 0) {
                item.withCredentials = false;
                this.uploader.options.headers[0].value = item.file.name;
                this.uploader.options.headers[1].value = SessionHelper.UserDetails.UserID;
                item.upload();
            } else {
                this.uploader.clearQueue();
            }
        };

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

        CustomerImportStatusConstants.StatusDropdown.forEach((element) => {
            this.CustomerImportStatusDropdown.push({ Value: element, Text: CustomerImportStatusConstants.Description[element] });
        });
    }
    GotoPrevious() {
        this.location.back();
    }

    ngAfterViewInit(): void {
        this.dataTableElement.forEach((dtElement: DataTableDirective) => {
            dtElement.dtInstance.then((dtInstance: any) => {
                if (dtInstance.context[0].sTableId === 'CustomerImportStagingTable') {
                    this.CustomerImportStagingDataTable = dtInstance;
                    if (isTruthy(this.CustomerImportStagingDataTable)) {
                        this.SearchCustomerImportStaging();
                    }
                } else if (dtInstance.context[0].sTableId === 'CustomerImportStagingDetailsTable') {
                    this.CustomerImportStagingDetailsDataTable = dtInstance;
                }
            });
        });

    }

    SearchCustomerImportStaging() {
        const req: CustomerImportStagingRequestModel = new CustomerImportStagingRequestModel(
            AppSettings.TenantSchema,
            this.SelectedCustomerImportStatus,
            this.ImportDate.dateFrom,
            this.ImportDate.dateTo,
            CustomerImportAssignTypeConstant.LENDER_IMPORT
            );

        this.promise = this._upsertCustomerService.GetCustomeImportStaging(req);
    }

    ResetSearch() {
        this.SelectedCustomerImportStatus = 0;
        this.ImportDate.selectRange('to');
        this.SearchCustomerImportStaging();
    }

    GetCustomerImportStagingDetails(CustomerImportStagingID: number) {
        const req: CustomerImportStagingDetailsRequestModel = new CustomerImportStagingDetailsRequestModel(AppSettings.TenantSchema, CustomerImportStagingID);
        this.promise = this._upsertCustomerService.GetCustomeImportStagingDetails(req);
    }

    UploadCustomerFile(event) {
        if (event.target.files[0].name.toLowerCase().endsWith('.csv')) {
            this.target = event.target || event.srcElement;
            this.uploader.uploadAll();
        } else {
            this._notificationService.showWarning('Kindly choose .csv file');
            event.target.value = '';
        }
    }

    ngOnDestroy(): void {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
