import { Subscription } from 'rxjs';
import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { IMyOptions, IMyDate } from '@mts-date-picker/interfaces';
import { AppSettings } from '@mts-app-setting';
import { CommonService } from 'src/app/shared/common';
import { FileUploader, FileUploaderOptions, FileItem } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { SelectComponent } from '@mts-select2';
import { NotificationService } from '@mts-notification';
import { LoanImportService } from '../../services/loan-import.service';
import { SessionHelper } from '@mts-app-session';
import { CheckFileExtension } from '../../pipes';
import { JwtHelperService } from '@auth0/angular-jwt';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { TenantCustomerRequestModel } from '../../models/tenant-request.model';
import { CustomerReviewLoanTypeModel } from '../../models/loan.import.model';
import { ImportConfigModel } from '../../models/import.data.model';
const jwtHelper = new JwtHelperService();
@Component({
  selector: 'mts-ad-hoc-loan-import',
  templateUrl: './ad-hoc-loan-import.page.html',
  styleUrls: ['./ad-hoc-loan-import.page.css']
})
export class AdHocLoanImportComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('LoanTypeDropDown') loanselect: SelectComponent;

  @ViewChild('ReviewItemsDrpDwn') reviewselect: SelectComponent;
  adhoc = new ImportConfigModel();
  SelectedAuditDateTemp: any = new Date();
  SelectedAuditDate: any;
  uploader: FileUploader;
  uploaderOptions: FileUploaderOptions;
  promise: Subscription;
  URL: string = environment.apiURL + 'FileUpload/FileUploader';
  hasBaseDropZoneOver = false;
  options: FileUploaderOptions = {
    url: this.URL,
    authToken: 'Bearer ' + localStorage.getItem('id_token'),
    authTokenHeader: 'Authorization',
    queueLimit: 1,
    headers: [
      { name: 'TableSchema', value: AppSettings.TenantSchema },
      { name: 'UploadFileName', value: '' },
      { name: 'ReviewType', value: '' },
      { name: 'LoanType', value: '' },
      { name: 'UserId', value: '' },
      { name: 'CustomerID', value: '' },
      { name: 'AuditMonthYear', value: '' },
      { name: 'PriorityType', value: '' },
      { name: 'AuditDueDate', value: '' }
    ]
  };

  AdhocAuditDate: any = new Date();
  fileUploadErrMsg: boolean;

  constructor(private commonmasterservice: CommonService
    , private _notificationService: NotificationService,
    private _loanImportService: LoanImportService,
    private checkExtn: CheckFileExtension) {

  }
  private subscription: Subscription[] = [];
  ngOnInit(): void {
    this.subscription.push(
      this.commonmasterservice.CustomerItems.subscribe(
        (result: any) => {
          this.adhoc.commonActiveCustomerItems = result;
        }
      )
    );
    this.subscription.push(
      this._loanImportService.loanTypeItems$.subscribe(
        (result: any) => {
          this.adhoc.loanTypeItems = result;
        }
      )
    );
    this.subscription.push(
      this._loanImportService.reviewTypeItems$.subscribe(
        (result: any) => {
          this.adhoc.reviewTypeItems = result;
        }
      )
    );
    this.uploader = new FileUploader(this.options);
    this.uploader.onBeforeUploadItem = (item: FileItem) => {
      if (this.reviewselect.active.length > 0 && this.adhoc.customerSelect !== 0 || isTruthy(this.adhoc.customerSelect.text)) {
        const fileExtensionStatus = this.checkExtn.transform(item.file.name);
        if (fileExtensionStatus === true) {
          item.withCredentials = false;
          this.uploader.options.headers[1].value = item.file.name;
          this.uploader.options.headers[2].value = this.adhoc.reviewTypeId;
          if (this.adhoc.loanTypeId === '' || this.adhoc.loanTypeId === null) {
            this.adhoc.loanTypeId = 0;
            this.uploader.options.headers[3].value = this.adhoc.loanTypeId;

          }
          this.uploader.options.headers[3].value = this.adhoc.loanTypeId;
          this.uploader.options.headers[4].value = SessionHelper.UserDetails.UserID;
          this.uploader.options.headers[5].value = this.adhoc.customerSelect.id;

          const adhocDate = new Date(this.SelectedAuditDate);
          const adhocDueDate = new Date(this.AdhocAuditDate.formatted);
          this.uploader.options.headers[6].value = adhocDate.toISOString();
          this.uploader.options.headers[7].value = this.adhoc.selectedPriority;
          this.uploader.options.headers[8].value = adhocDueDate.toISOString();
          item.upload();
        } else {
          this._notificationService.showError('File Extension not Supported, Please Upload .pdf,.tif,.tiff,jpg,jpeg');
        }
      } else {
        let i = 0;
        for (i = 1; i < this.uploader.options.headers.length; i++) {
          this.uploader.options.headers[i].value = '';
        }
        this._notificationService.showError('Upload Failed');
        this._notificationService.showError('Please Select ' + AppSettings.AuthorityLabelSingular + ' and Service Type');
        this.uploader.clearQueue();

      }
    };

    this.uploader.onCompleteItem = (item: any, response: any, status: any, headers: any) => {
      let responsePath;
      if (response !== '') {
        const resParse = response = JSON.parse(response);
        if (resParse.token !== null) {
          localStorage.setItem('id_token', resParse.token);
          responsePath = jwtHelper.decodeToken(resParse.data)['data'];
          if (responsePath.Result === true) {

            this._notificationService.showSuccess('File has been Uploaded Successfully');
            this.uploader.clearQueue();
          } else {

            this._notificationService.showError('File Upload Failed');
            this.uploader.queue[0].isUploaded = false;
            this.uploader.clearQueue();
          }
        }
      } else {
        this.uploader.clearQueue();
      }
    };
  }
  ngAfterViewInit() {
    const d: Date = new Date();
    this.adhoc.selDate = {
      year: d.getFullYear(),
      month: d.getMonth() + 1,
      day: d.getDate()
    };
    this.uploader.onAfterAddingFile = (item => {
      item.withCredentials = false;
    });
    this.commonmasterservice.GetCustomerList(AppSettings.TenantSchema);
  }
  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
    if (this.uploader.queue.length > 1) {
      this.fileUploadErrMsg = true;
    }
  }
  onFileDropUpload(e: any) {
    this.ValidateAdhocUploadFields();
    if (this.adhoc.enableUpload) {
      this._notificationService.showError('Please Select All The Fields');
      this.uploader.clearQueue();
    }
  }
  singleFileUpload(e: any): void {
    this.ValidateAdhocUploadFields();
    if (this.uploader.queue.length === 1) {
      this.uploader.clearQueue();
    }
  }
  ValidateAdhocUploadFields() {
    if (this.adhoc.reviewTypeId > 0 && this.AdhocAuditDate !== null && this.adhoc.selectedPriority > 0 && this.adhoc.customerSelect.id > 0) {
      this.adhoc.enableUpload = false;
    } else {
      this.adhoc.enableUpload = true;
    }

  }
  getAuditMonthYear(value) {
    this.SelectedAuditDate = value.Value;
  }
  custModelChange() {
    this.adhoc.loanSelect = 0;
    this.adhoc.reviewTypeId = 0;
    this.adhoc.selectedPriority = 0;
    this.reviewselect.active = [];
    const inputData = new TenantCustomerRequestModel(AppSettings.TenantSchema, this.adhoc.customerSelect.id);
    this._loanImportService.customerReviewType(inputData);

  }
  adHocLoanTypeChange() {
    this.ValidateAdhocUploadFields();
    this.adhoc.loanTypeId = this.adhoc.loanSelect;

  }
  getReviewTypeDatas(): void {
    if (this.uploader.queue.length > 0) {
      this.uploader.removeFromQueue(this.uploader.queue[0]);
    }
    this.adhoc.loanSelect = 0;
    this.loanselect.active = [];
    this.adhoc.loanTypeId = '';
    const reviewType = this.adhoc.reviewTypeItems.filter(x => x.id === this.adhoc.reviewTypeId)[0];
    if (isTruthy(reviewType)) {
      this.adhoc.selectedPriority = reviewType.priority;
      this.ValidateAdhocUploadFields();

    } else {
      this.adhoc.selectedPriority = 0;
      this.ValidateAdhocUploadFields();

    }
    const input = new CustomerReviewLoanTypeModel(AppSettings.TenantSchema, this.adhoc.customerSelect.id, this.adhoc.reviewTypeId);
    this._loanImportService.customerReviewLoanType(input);
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
