import { SessionHelper } from '@mts-app-session';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy, ViewChildren, QueryList } from '@angular/core';
import { IMyOptions, IMyDate } from '@mts-date-picker/interfaces';
import { CommonService } from 'src/app/shared/common';
import { Subscription } from 'rxjs';
import { AppSettings } from '@mts-app-setting';
import { convertDateTimewithTime, convertDateTime } from '@mts-functions/convert-datetime.function';
import { LoanImportService } from '../../services/loan-import.service';
import { DataTableDirective } from 'angular-datatables';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { CustomerReviewLoanTypeModel } from '../../models/loan.import.model';
import { CheckBoxTokenModel, GetBoxTokenModel } from 'src/app/modules/application-configuration/models/box-setting.model';
import { TenantRequestModel, TenantCustomerRequestModel } from '../../models/tenant-request.model';
import { BoxFileListRequestModel, FolderItemCountRequestModel } from '../../models/box.import.model';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ImportConfigModel } from '../../models/import.data.model';

@Component({
  selector: 'mts-box-loan-import',
  templateUrl: './box-loan-import.page.html',
  styleUrls: ['./box-loan-import.page.css']
})
export class BoxLoanImportComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;
  @ViewChild('confirmModal') confirmModal: ModalDirective;
  box = new ImportConfigModel();
  BoxAuditDateTemp: any = new Date();
  BoxAuditDate: any;
  BoxAuditDueDate: any = new Date();
  promise: Subscription;
  priorityValues = 0;
  selectallUpload = true;
  dtOptions: any;
  boxFiles: any = { BoxEntities: [] };
  dupBoxDTOptions: any;
  ModalType: string;
  priorityList: any = [];
  LastFolderID: any;
  duplicateFolders: any[];
  duplicateFileNames: any[];
  AlertMessage: string;
  isTable: boolean;
  isDuplicateMsg: string;
  isRowSelectrDeselect = true;
  rowSelected: any;
  constructor(private commonmasterservice: CommonService
    , private aroute: ActivatedRoute,
    private _loanImportService: LoanImportService) {
    // if (isTruthy(this.aroute.queryParams['value'].code)) {
    //   this.box.customerSelect = 0;
    //   this.box.reviewselect.active = [];
    //   this.box.reviewTypeId = 0;
    //   this.box.selectedPriority = 0;
    //   this.box.loanTypeItems = [];
    // }
    this.getPriorityList();
    this.userDetails = SessionHelper.UserDetails;
  }
  private dTable: any;
  private dupBoxDT: any;
  private BoxAuthURL: any;
  private subscription: Subscription[] = [];
  private userDetails: any;
  private isSelectEnabled: any;
  ngOnInit(): void {
    this.dtOptions = {
      aaData: this.boxFiles.BoxEntities,
      'select': {
        style: 'multi',
        info: false,
        selector: 'td:first-child'
      },
      'order': [[3, 'asc']],
      'iDisplayLength': 10,
      'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
      aoColumns: [
        { mData: 'Id', sClass: 'select-checkbox', bVisible: true, bSortable: false, 'sWidth': '2%' },
        { mData: 'Id', bVisible: false },
        { sTitle: 'Priority', mData: 'Priority', bVisible: true, bSortable: false, 'sWidth': '20%', sClass: 'text-center' },
        { sTitle: 'Name', mData: 'Name', 'sWidth': '50%' },
        { sTitle: 'Modified', mData: 'ModifiedTime', 'sWidth': '15%', sClass: 'text-center' },
        { sTitle: 'Size', mData: 'Size', 'sWidth': '15%', sClass: 'text-center' },
        { sTitle: 'Type', mData: 'Type', bVisible: false },
      ],
      aoColumnDefs: [
        {
          'aTargets': [2],
          'mRender': function (data, type, row) {
            return '#' + data;
          }
        },
        {
          'aTargets': [3],
          'mRender': function (data, type, row) {
            if (row['Type'] === 'folder') {
              return '<span style=\'cursor: pointer;\' class=\'fa fa-folder folder viewFolder\'></span>&nbsp;<span style=\'cursor: pointer;\' class=\'viewFolder\'>' + data + '</span>';

            } else {
              return '<span class=\'fa fa-file-text file \'></span>&nbsp;' + data;
            }
          }
        },
        {
          'aTargets': [4],
          'mRender': function (data) {
            if (data !== null && data !== '') {
              return convertDateTimewithTime(data);
            } else {
              return data;
            }
          }
        },
        {
          'aTargets': [5],
          'mRender': function (data, type, row) {

            if (data !== null && data !== '' && row['Type'] === 'file') {
              return (data / (1024 * 1024)).toFixed(2) + ' MB';
            } else { return ''; }
          }
        },
        {
          'aTargets': [0],
          'mRender': function (data) {
            return '';
          }
        }
      ],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        const self = this;

        let priVale = $($('td', row)[1]).text();

        if (priVale.startsWith('#')) {
          let dropText = ' <select class=\'SetPriority\'> ';
          priVale = priVale.replace('#', '');
          $($('td', row)[1]).text('');
          this.priorityList.forEach(element => {
            if (element.id.toString() === priVale) {
              dropText += ' <option selected=\'selected\' value=\'' + element.id + '\'>' + element.text + '</option>';
            } else {
              dropText += ' <option value=\'' + element.id + '\'>' + element.text + '</option>';
            }
          });
          dropText += ' </select>';
          $($('td', row)[1]).html(dropText);
        }

        $('td .viewFolder', row).unbind('click');
        $('td .viewFolder', row).bind('click', () => {
          self.getRowDataforViewFolder(row, data);
        });

        $('td .SetPriority', row).unbind('change');
        $('td .SetPriority', row).bind('change', (s) => {
          self.getRowDataforSetPriority(row, data, $(s.target).val());
        });
        return row;
      }
    };
    this.dupBoxDTOptions = {
      'iDisplayLength': 10,
      'select': {
        style: 'multi',
        info: false,
        selector: 'td:first-child'
      },
      'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
      aoColumns: [
        { mData: 'Id', sClass: 'select-checkbox', bVisible: true, bSortable: false },
        { sTitle: 'File Name', mData: 'FileName' },
        { sTitle: 'File Path', mData: 'FilePath' },
        { sTitle: 'UploadedBy', mData: 'UserName' },
        { sTitle: 'UploadedOn', mData: 'CreatedDate' },
        { sTitle: 'Priority', mData: 'Priority', bVisible: false },
        { sTitle: 'ItemType', mData: 'Type', bVisible: false }
      ],
      aoColumnDefs: [
        {
          'aTargets': [4],
          'mRender': function (date) {
            if (isTruthy(date)) {
              return convertDateTime(date);
            } else {
              return date;
            }
          }
        },
        {
          'aTargets': [0],
          'orderable': false,
          'mRender': function (date) {
            return '';
          }
        },
        {
          'aTargets': [1],
          'mRender': function (data, type, row) {
            if (row['Type'] === 'folder') {
              return '<span style=\'cursor: pointer;\' class=\'fa fa-folder folder viewFolder\'></span>&nbsp;<span style=\'cursor: pointer;\' class=\'viewFolder\'>' + data + '</span>';
            } else {
              return '<span class=\'fa fa-file-text file \'></span>&nbsp;' + data;
            }
          }
        }
      ]
    };
    this.subscription.push(
      this.commonmasterservice.CustomerItems.subscribe(
        (result: any) => { this.box.commonActiveCustomerItems = result; }
      )
    );
    this.subscription.push(
      this._loanImportService.loanTypeItems$.subscribe(
        (result: any) => { this.box.loanTypeItems = result; }
      )
    );
    this.subscription.push(
      this._loanImportService.reviewTypeItems$.subscribe(
        (result: any) => { this.box.reviewTypeItems = result; }
      )
    );
    this.subscription.push(
      this._loanImportService.boxfilelist$.subscribe(
        (result: any) => {
          this.boxFiles = result;
          this.boxFiles.BoxEntities.forEach(element => {
            element.Priority = this.box.selectedPriority;
          });
          this.dTable.clear();
          this.dTable.rows.add(this.boxFiles.BoxEntities);
          this.dTable.draw();
          this.dTable.columns.adjust().draw();
        }
      )
    );
    this.subscription.push(
      this._loanImportService.isuploadbox$.subscribe(
        (Result: any) => {
          this.isTable = false;
          this.box.enableUpload = true;
          // this.dupBoxDT.rows().deselect();
          this.dTable.rows().deselect();
        }
      )
    );
    this.subscription.push(
      this._loanImportService.CheckUserBoxToken$.subscribe(
        (Result: any) => {
          this.BoxAuthURL = Result.BoxAuthURL;
          if (Result.TokenStatus === 0) {
            this.ModalType = 'Link';
            this.AlertMessage = 'You are not yet linked to your Box account. Contact Administrator.';
            this.confirmModal.show();
            this.isTable = false;
          } else if (Result.TokenStatus === 2) {
            this.ModalType = 'Link';
            this.AlertMessage = 'Your Box token expired. Contact Administrator.';
            this.confirmModal.show();
            this.isTable = false;
          } else {
            this.ShowFolder('0');
          }
        }
      )
    );
    this.subscription.push(
      this._loanImportService.priorityList$.subscribe(
        (Result: any) => {
          Result.forEach(element => {
            this.priorityList.push({ id: element.ReviewPriorityID, text: element.ReviewPriorityName, priority: element.ReviewPriority });
          });
        }
      )
    );
    this.subscription.push(
      this._loanImportService.GetFolderItemCount$.subscribe(
        (Result: any) => {
          if (Result.isTable) {
            this.isTable = true;
            this.isDuplicateMsg = 'red';
            this.AlertMessage = Result.FolderandFiles.length + ' Duplicate Loan file(s) found.';
            this.dupBoxDT.clear();
            this.dupBoxDT.rows.add(Result.FolderandFiles);
            this.dupBoxDT.draw();
            this.ModalType = 'Upload';
            this.confirmModal.show();
          } else {
            this.isTable = false;
            this.isDuplicateMsg = 'black';
            this.isRowSelectrDeselect = false;
            this.AlertMessage = 'Do you want to upload ' + Result.itemCount[0].BoxItemCounts + ' file(s) from box';
            this.ModalType = 'Upload';
            this.confirmModal.show();
          }
        }
      )
    );
  }
  ngAfterViewInit() {
    const d: Date = new Date();
    this.box.selDate = {
      year: d.getFullYear(),
      month: d.getMonth() + 1,
      day: d.getDate()
    };
    this.commonmasterservice.GetCustomerList(AppSettings.TenantSchema);
    this.dtElements.forEach((dtElement: DataTableDirective) => {
      dtElement.dtInstance.then((dtInstance: any) => {
        if (dtInstance.context[0].sTableId === 'boxtable') {
          this.dTable = dtInstance;
          $(dtInstance).find('th').each(function (index, val) {
            if (index === 0) {
              if ($(this).hasClass('select-checkbox')) {
                $(this).removeClass('select-checkbox');
              }
              const headerChk = '<input type="checkbox"  id="selectAll" />';
              $(this).html(headerChk);
            }
          });

          dtInstance.on('select', (s) => {
            this.ValidateUploadFields();
          });

          dtInstance.on('deselect', (s) => {
            this.ValidateUploadFields();
          });

          this.ShowBoxBrowser();

        } else if (dtInstance.context[0].sTableId === 'duplicateboxtable') {
          dtInstance.on('select', (s) => {
            this.isRowSelectrDeselect = !(this.dupBoxDT.rows('.selected').data().length > 0);
          });
          dtInstance.on('deselect', (s) => {
            this.isRowSelectrDeselect = !(this.dupBoxDT.rows('.selected').data().length > 0);
          });
          this.dupBoxDT = dtInstance;
        }
      });
    });
  }
  custModelChange() {
    this.box.loanSelect = 0;
    this.box.reviewTypeId = 0;
    this.box.selectedPriority = 0;
    this.box.reviewselect.active = [];
    const inputData = new TenantCustomerRequestModel(AppSettings.TenantSchema, this.box.customerSelect.id);
    this._loanImportService.customerReviewType(inputData);
    this.ValidateUploadFields();
  }
  serviceTypesChange() {
    this.box.loanSelect = 0;
    if (isTruthy(this.box.reviewTypeItems) && this.box.reviewTypeItems.length > 0) {
      this.getReviewTypeDatas();
      this.selectallUpload = true;
    }
  }
  custLoanTypeChange() {
    this.box.loanTypeId = this.box.loanSelect;
    this.box.loanSelect = 0;
  }
  ShowFolder(id: any) {
    this.LastFolderID = id;
    const inputRequests = new BoxFileListRequestModel(AppSettings.TenantSchema, SessionHelper.UserDetails.UserID, id, 10000, 0, '.pdf,.PDF');
    this.promise = this._loanImportService.getBoxFileList(inputRequests);
  }
  UploadBoxFileConfirmation() {
    const rows = this.dTable.rows('.selected').data();
    const uploadItems = [];
    for (let i = 0; i < rows.length; i++) {
      let pri = 0;
      const ptype = this.priorityList.filter(x => x.id === rows[i].Priority)[0];
      if (isTruthy(ptype)) {
        pri = ptype.priority;
      } else {
        pri = rows[i].Priority;
      }
      uploadItems.push({ BoxID: rows[i].Id, ItemType: rows[i].Type, Priority: pri });
    }
    const itemCount = 0;
    const inputRequests = new FolderItemCountRequestModel(this.box.reviewTypeId, this.userDetails.UserID, this.box.customerSelect.id, AppSettings.TenantSchema, uploadItems, '.pdf,.PDF');
    this.GetFolderItemCount(inputRequests);
  }
  GetFolderItemCount(inputRequests: any): any {
    this.duplicateFolders = [];
    this.duplicateFileNames = [];
    this.promise = this._loanImportService.GetFolderItemCount(inputRequests);
  }
  SelectAll() {
    if (this.selectallUpload) {
      this.dTable.rows().select();
      this.ValidateUploadFields();
      this.selectallUpload = false;
    } else {
      this.dTable.rows().deselect();
      this.ValidateUploadFields();
      this.selectallUpload = true;
    }
  }
  PriorityVlauesChange() {
    if (isTruthy(this.priorityValues)) {
      this.box.selectedPriority = this.priorityValues;
    } else {
      this.box.selectedPriority = 0;
    }
    this.RefereshFolder();
    this.ValidateUploadFields();
    this.selectallUpload = true;
  }
  getRowDataforViewFolder(rowIndex: Node, rowData: any): void {
    const row = rowData;
    if (isTruthy(typeof row)) {
      this.ShowFolder(row['Id']);
    } else {
      console.log('Row Not Fetched');
    }
  }
  getRowDataforSetPriority(rowIndex: Node, rowData: any, val): void {
    const row = rowData;
    if (isTruthy(typeof row)) {
      row['Priority'] = val;
      this.dTable.row(rowIndex).data(row);
      this.dTable.draw();
    }
  }
  getBoxAuditMonthYear(value) {
    this.BoxAuditDate = value.Value;
  }
  ValidateUploadFields() {
    if (this.box.reviewTypeId > 0 && this.BoxAuditDate !== null && this.box.selectedPriority > 0 && this.box.customerSelect.id > 0 && this.dTable.rows('.selected').data().length > 0) {
      this.box.enableUpload = false;
    } else {
      this.box.enableUpload = true;
    }
  }
  RefereshFolder() {
    this.dTable.clear();
    this.boxFiles.BoxEntities.forEach(element => {
      element.Priority = this.box.selectedPriority;
    });
    this.dTable.rows.add(this.boxFiles.BoxEntities);
    this.dTable.draw();
  }
  getPriorityList(): any {
    const inputData = new TenantRequestModel(AppSettings.TenantSchema);
    this._loanImportService.getPriorityList(inputData);
  }
  ShowBoxBrowser(): void {
    this.dTable.columns.adjust().draw();
    const inputRequest = new CheckBoxTokenModel(AppSettings.TenantSchema, this.userDetails.UserID);
    this.promise = this._loanImportService.CheckUserBoxToken(inputRequest);
  }
  getReviewTypeDatas(): void {
    this.box.loanTypeId = '';
    this.RefereshFolder();
    this.getServicePriority();
    const input = new CustomerReviewLoanTypeModel(AppSettings.TenantSchema, this.box.customerSelect.id, this.box.reviewTypeId);
    this._loanImportService.customerReviewLoanType(input);
  }
  getServicePriority() {
    this.box.loanSelect = 0;
    if (isTruthy(this.box.reviewTypeItems) && this.box.reviewTypeItems.length > 0) {
      const reviewType = this.box.reviewTypeItems.filter(x => x.id === this.box.reviewTypeId)[0];
      if (isTruthy(reviewType)) {
        this.box.selectedPriority = reviewType.priority;
      } else {
        this.box.selectedPriority = 0;
      }
      this.priorityValues = this.box.selectedPriority;
      this.RefereshFolder();
    }
  }
  ConfirmAction() {
    if (this.ModalType !== 'Upload') { this.RedirectToBox(); } else {
      this.UploadBoxFile();
    }
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => { element.unsubscribe(); });
  }
  RedirectToBox(): void {
    window.location.href = this.BoxAuthURL;
  }
  UploadBoxFile() {
    this.confirmModal.hide();
    let rows = null;
    if (this.isTable) {
      if (this.isRowSelectrDeselect) {
        rows = this.getSelectedRowData();
      } else {
        this.getSelectedRowData();
        rows = this.rowSelected;
      }
    } else {
      rows = this.dTable.rows('.selected').data();
    }
    const uploadItems = [];
    for (let i = 0; i < rows.length; i++) {
      let pri = 0;
      const ptype = this.priorityList.filter(x => x.id === rows[i].Priority)[0];
      if (ptype !== undefined) {
        pri = ptype.priority;
      }
      else{
        pri= rows[i].Priority
      }
      uploadItems.push({ BoxID: rows[i].Id, ItemType: rows[i].Type, Priority: pri });
    }
    const inputRequests = {
      ReviewType: this.box.reviewTypeId,
      LoanType: this.box.loanTypeId,
      CustomerID: this.box.customerSelect.id,
      TableSchema: AppSettings.TenantSchema,
      UserID: this.userDetails.UserID,
      BoxItems: uploadItems,
      FileFilter: '.pdf,.PDF',
      AuditMonthYear: this.BoxAuditDate.formatted,
      AuditDueDate: this.BoxAuditDueDate.formatted
    };
    this.promise = this._loanImportService.UploadBoxFile(inputRequests);
  }
  getSelectedRowData() {
    const Data = this.dupBoxDT.rows().data();
    this.rowSelected = this.dupBoxDT.rows('.selected').data();
    if (this.isSelectEnabled) {
      this.dupBoxDT.rows().select();
      this.isSelectEnabled = false;
    } else {
      this.dupBoxDT.rows().deselect();
      this.isSelectEnabled = true;
    }
    return Data;
  }
}
