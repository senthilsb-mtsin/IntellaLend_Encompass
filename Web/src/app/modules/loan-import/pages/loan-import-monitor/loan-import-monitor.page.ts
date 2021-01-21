import { CommonService } from 'src/app/shared/common/common.service';
import { NotificationService } from '@mts-notification';
import { LoanImportService } from './../../services/loan-import.service';
import { Component, OnInit, ViewChild, ElementRef, AfterContentChecked, OnDestroy, AfterViewInit, ViewChildren, QueryList } from '@angular/core';
import { AppSettings, LOSImportStatusConstant } from '@mts-app-setting';
import { StatusConstant } from '@mts-status-constant';
import { NgDateRangePickerComponent } from '@mts-daterangepicker/ng-daterangepicker.component';
import { Subscription } from 'rxjs';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DataTableDirective } from 'angular-datatables';
import { SessionHelper } from '@mts-app-session';
import { convertDateTime, formatDate } from '@mts-functions/convert-datetime.function';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { TenantLoanRequestModel } from '../../models/tenant-request.model';
import { UpdateLoanMonitorModel, DeleteLoanModel, CustomerReviewLoanTypeModel } from '../../models/loan.import.model';
import { LoanSearchModel } from '../../models/loan.search.model';
import { RetryEncompassDownloadModel, GetEphesofturlModel } from '../../models/retry.encompass.model';
@Component({
  selector: 'mts-loan-import-monitor',
  templateUrl: './loan-import-monitor.page.html',
  styleUrls: ['./loan-import-monitor.page.css']
})
export class LoanImportMonitorComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(NgDateRangePickerComponent) receivedDate: NgDateRangePickerComponent;
  @ViewChild('assignlTypemodal') _assignlTypemodal: ModalDirective;
  @ViewChild('confirmModal') confirmModal: ModalDirective;
  @ViewChild('missingDocModal') missingDocModal: ModalDirective;
  // @ViewChild(DataTableDirective) datatableEl: DataTableDirective;
  @ViewChildren(DataTableDirective)
  dtElements: QueryList<DataTableDirective>;
  @ViewChild('searchBtn') _searchBtn: ElementRef;
  @ViewChild('confirmDeleteModal') _confirmDeleteModal: ModalDirective;
  @ViewChild('RetryConfirmModel') RetryConfirmModel: ModalDirective;
  promise: Subscription;
  Dateoptions: any;
  dtOptions: any;
  selectAllBtn: any = true;
  isDeleteLoan = true;
  StatusSelect: any = -3;
  customerSelect: any = 0;
  selectedLoanTypes: any = 0;
  isAssignLoanType = true;
  AllActiveLoanTypes: any;
  LoanAlertMessage: any;
  tableAligned = false;
  missingDocDTOptions: any;
  commonActiveCustomerItems: any;
  _showHide: any = [true, true];
  _pendingBOXDownload = StatusConstant.PENDING_BOX_DOWNLOAD;
  _pendingIDC = StatusConstant.PENDING_IDC;
  _readyForAudit = StatusConstant.PENDING_AUDIT;
  AuthorityLabelSingular = AppSettings.AuthorityLabelSingular;
  value: any = '';
  isConfirmed = false;
  MissingdocTable: any;
  reviewTypeItems: any[];
  constructor(private _loanImportService: LoanImportService,
    private _notificationService: NotificationService,
    private commonmasterservice: CommonService) {
  }
  private dTable: any;
  private rowData: any;
  private subscription: Subscription[] = [];
  private multiRowData: any;
  ngOnInit() {
    this.Dateoptions = {
      theme: 'default',
      previousIsDisable: false,
      nextIsDisable: false,
      range: 'em',
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
      dom: 'Blfrtip',
      buttons: [
        {
          extend: 'excel',
          className: 'btn btn-sm btn-info waves-effect waves-light m-b-10 m-r-5 m-l-1',
          text: '<i class="fa fa-file-excel-o"></i> Download',
          filename: 'Loan Import',
          exportOptions: {
            columns: [24, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 14, 16, 17, 18, 19]
          },
          title: 'Loan Import'
        }
      ],
      'iDisplayLength': 10,
      'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
      'select': {
        style: 'multi',
        info: false,
        selector: 'td:first-child'
      },
      aoColumns: [
        { mData: 'LoanID', sClass: 'select-checkbox', bVisible: true, bSortable: false },
        { sTitle: 'Import Source', mData: 'UploadType', bSortable: true },
        { sTitle: 'Priority Level', mData: 'Priority', bsortable: true },
        { sTitle: 'Imported Date', mData: 'Uploaded', bSortable: true },
        { sTitle: 'Imported By', mData: 'LoanID', bSortable: true},
        { mData: 'LoanTypeName', sTitle: 'LoanType', bSortable: true },
        { mData: 'ReviewType', sTitle: 'Service Type', bSortable: true},
        { sTitle: 'IDC URL', mData: 'EphesoftBatchInstanceID', bSortable: true, sWidth: '15%' },
        { sTitle: 'Borrower Loan#', mData: 'LoanNumber', bSortable: true, sWidth: '10%', sClass: 'text-center' },
        { sTitle: AppSettings.AuthorityLabelSingular + ' Name', mData: 'Customer', bSortable: true, sClass: 'text-left' },
        { sTitle: 'Source File Name', mData: 'BoxFileName', bSortable: true, sWidth: '10%', sClass: 'text-left' },
        { sTitle: 'Trailing Documents', mData: 'IsMissingDocAvailable', bSortable: true, sClass: 'text-center' },
        { sTitle: 'Status', mData: 'Status', sClass: 'text-center', bSortable: true, sWidth: '10%' },
        { sTitle: '&nbsp;', mData: 'Status', sClass: 'text-center', 'sWidth': '3%', bSortable: false },
        { sTitle: 'Last Name', mData: 'LastName', bVisible: false, bSortable: true },
        { sTitle: 'First Name', mData: 'FirstName', bVisible: false, bSortable: true },
        { mData: 'LoanNumber', bVisible: false, sTitle: 'Loan Number', bSortable: true },
        { mData: 'StatusDescription', bVisible: false, sTitle: 'Status', bSortable: true },
        { mData: 'LoanAmount', bVisible: false, sTitle: 'Loan Amount($)', bSortable: true },
        { mData: 'AuditMonthYear', bVisible: false, sTitle: 'Audit Month & Year', bSortable: true },
        { mData: 'BorrowerName', bVisible: false, sTitle: 'Borrower Name', bSortable: true },
        { sTitle: 'Loan Type', mData: 'LoanType', bVisible: false, bSortable: true },
        { sTitle: 'Assigned User ID', mData: 'AssignedUserID', bVisible: false },
        { sTitle: 'EDownloadID', mData: 'EDownloadID', bVisible: false },
        { mData: 'LoanID',  sTitle: 'Loan ID',  bVisible: false, bSortable: false },
        { sTitle: 'ErrorMsg', mData: 'ErrorMsg', bVisible: false },

      ], aoColumnDefs: [
        {
          'aTargets': [0],
          'mRender': function (data) {
            return '';
          }
        }, {
          'aTargets': [11],
          'mRender': function (data) {
            if (data) {
              return '<span title=\'Trailing Document(s) Available\' class=\'missingdocclick material-icons txt-red\'>warning</span>';
            } else {
              return '';
            }
          }
        },

        {
          'aTargets': [1],
          'mRender': function (data, row) {

            if (data === 0) {
              return '<label class=\'label label-primary label-table\'>Ad-hoc</label>';
            } else if (data === 1) {
              return '<label class=\'label label-primary label-table\'>box</label>';
            } else if (data === 2) {
              return '<label class=\'label label-primary label-table\'>Encompass</label>';
            } else if (data === 4) {
              return '<label class=\'label label-primary label-table\'>LOS</label>';
            } else {
              return '<label class=\'label label-primary label-table\'>UNC</label>';
            }
          }
        },
        {
          'aTargets': [2],
          'mRender': function (data, type, row) {
            if (data === 1 || data === 2 || data === 3 || data === 4) {
              return StatusConstant.PRIORITY_LEVEL[row.Priority];
            } else {
              return 'Priority Unavailable';
            }
          }
        },
        {
          'aTargets': [12],
          'mRender': function (data, type, row) {
            if (data !== null && data !== '') {
              if (data === -2) {

                return '<label title="' + row['ErrorMsg'] + '" class="label label-danger label-table">Error</label>';
              } else if (data === StatusConstant.PENDING_IDC && row['SubStatus'] > 0) {
                return '<label title="' + StatusConstant.STATUS_DESCRIPTION[row['SubStatus']] + '" class="bcEllipsis label ' + StatusConstant.STATUS_COLOR[row['SubStatus']] + ' label-table">' + StatusConstant.STATUS_DESCRIPTION[row['SubStatus']] + '</label>';
              } else if (data === StatusConstant.FAILED_ENCOMPASS_DOWNLOAD) {
                return '<label title="' + row['ErrorMsg'] + '" class="bcEllipsis label ' + StatusConstant.STATUS_COLOR[row['Status']] + ' label-table">' + StatusConstant.STATUS_DESCRIPTION[row['Status']] + '</label>';
              } else if (data === LOSImportStatusConstant.LOS_IMPORT_STAGED) {
                return '<label title="' + LOSImportStatusConstant.LOS_IMPORT_STATUS_DESCRIPTION[row['Status']] + '" class="bcEllipsis label ' + LOSImportStatusConstant.LOS_IMPORT_STATUS_COLOR[row['Status']] + ' label-table">' + LOSImportStatusConstant.LOS_IMPORT_STATUS_DESCRIPTION[row['Status']] + '</label>';
              } else if (data === LOSImportStatusConstant.LOS_IMPORT_PROCESSING && row.LoanID === 0) {
                return '<label title="' + LOSImportStatusConstant.LOS_IMPORT_STATUS_DESCRIPTION[row['Status']] + '" class="bcEllipsis label ' + LOSImportStatusConstant.LOS_IMPORT_STATUS_COLOR[row['Status']] + ' label-table">' + LOSImportStatusConstant.LOS_IMPORT_STATUS_DESCRIPTION[row['Status']] + '</label>';
              } else if (data === StatusConstant.FAILED_ENCOMPASS_DOWNLOAD) {
                return '<label title="' + row['ErrorMsg'] + '" class="bcEllipsis label ' + StatusConstant.STATUS_COLOR[row['Status']] + ' label-table">' + StatusConstant.STATUS_DESCRIPTION[row['Status']] + '</label>';
              } else if (data === LOSImportStatusConstant.LOS_IMPORT_FAILED) {
                return '<label title="' + row['ErrorMsg'] + '" class="bcEllipsis label ' + LOSImportStatusConstant.LOS_IMPORT_STATUS_COLOR[row['Status']] + ' label-table">' + LOSImportStatusConstant.LOS_IMPORT_STATUS_DESCRIPTION[row['Status']] + '</label>';
              } else if (data === StatusConstant.IDC_ERROR && (row['SubStatus'] === StatusConstant.LOANTYPE_UNAVAILABLE || row['SubStatus'] === StatusConstant.LOAN_TYPE_NOT_FOUND)) {
                return '<label title="' + StatusConstant.STATUS_DESCRIPTION[row['SubStatus']] + '" class="bcEllipsis label ' + StatusConstant.STATUS_COLOR[row['Status']] + ' label-table">' + StatusConstant.STATUS_DESCRIPTION[row['Status']] + '</label>';
              } else {
                return '<label title="' + StatusConstant.STATUS_DESCRIPTION[row['Status']] + '" class="bcEllipsis label ' + StatusConstant.STATUS_COLOR[row['Status']] + ' label-table">' + StatusConstant.STATUS_DESCRIPTION[row['Status']] + '</label>';
              }
            } else {
              return '';
            }
          }
        }, {
          'aTargets': [7],
          'mRender': function (data) {
            if (data !== null && data !== '') {
              return '<a class=\'redirectUrl\' href=\'javascript:void(0)\' >' + data + '</a>';
            } else {
              return '';
            }
          }
        }, {
          'aTargets': [4],
          'mRender': function (data, type, row) {
            if (data !== null && data !== '') {
              return row['LastName'] + ' ' + row['FirstName'];
            } else {
              return '';
            }
          }
        }, {
          'aTargets': [10],
          'mRender': function (data, type, row) {
            const fileList = row['BoxFileName'].split('|');
            let minString = '';
            fileList.forEach(element => {
              if (minString === '' || element.length < minString.length) {
                minString = element;
              }
            });
            if (fileList.length > 1) {
              return '<a href=\'javascript:void(0)\' class=\'viewFiles\' style=\'text-decoration:underline;\'>' + minString.replace(/\/[^\/]+$/, '') + '</a>';
            } else {
              return row['BoxFileName'];
            }
          }
        },
        {
          'aTargets': [13],
          'mRender': function (data, type, row) {
            if (row['Status'] === -2) {
              return '<span style=\'font-size:21px;cursor: pointer;\' title=\'' + row['ErrorMsg'] + '\' class=\'retryUpload material-icons txt-info\'>settings_backup_restore</span>';
            } else if (row['Status'] === StatusConstant.PENDING_AUDIT || (row['Status'] === StatusConstant.COMPLETE)) {
              return '<span  title=\'' + 'View Loan' + '\' style=\'cursor: pointer;\' class=\'viewLoan material-icons txt-info\'>pageview</span>';
            } else if (row.Status === StatusConstant.FAILED_ENCOMPASS_DOWNLOAD) {
              return '<div  title=\'' + 'Retry Download' + '\' class =\'LoanDownloadRetry\'> <i class=\'fa fa-retweet\' aria-hidden=\'true\'></i></div>';
            } else {
              return '';
            }
          }
        },
        {
          'aTargets': [3],
          'mRender': function (date) {
            if (isTruthy(date)) {
              return convertDateTime(date);
            } else {
              return '';
            }
          }
        }],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        const self = this;
        $('td .retryUpload', row).unbind('click');
        $('td .retryUpload', row).bind('click', () => {
          self.getRowData(row, data);
        });
        $('td .LoanDownloadRetry', row).unbind('click');
        $('td .LoanDownloadRetry', row).bind('click', () => {
          this.RetryConfirmModel.show();
          self.LoanDownloadRetry(row, data);
        });
        $('td .viewFiles', row).unbind('click');
        $('td .viewFiles', row).bind('click', () => {
          self.setInnerTable(row, data);
        });
        $('td .missingdocclick', row).unbind('click');
        $('td .missingdocclick', row).bind('click', () => {
          self.getMissingDoc(row, data);
        });
        $('td .viewLoan', row).unbind('click');
        $('td .viewLoan', row).bind('click', () => {
          self.viewLoan(row, data);
        });
        $('td .redirectUrl', row).unbind('click');
        $('td .redirectUrl', row).bind('click', () => {
          if (data['Status'] === 999) {
            this._notificationService.showError('Batch is not available');
          } else if (data['Status'] === 91) {
            this._notificationService.showSuccess('Batch finished successfully');
          } else {
            self.getEphesofturlValue(data['EphesoftBatchInstanceID']);
          }
        });
        $('td:first-child', row).unbind('click');
        $('td:first-child', row).bind('click', () => {
          self.RowSelect(row, data);
        });
        return row;
      }
    };
    this.missingDocDTOptions = {
      'iDisplayLength': 10,
      'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
      aoColumns: [
        { mData: 'LoanID', bVisible: false },
        { sTitle: 'Imported Date', mData: 'Uploaded', sClass: 'text-center', bSortable: true },
        { sTitle: 'IDC URL', mData: 'EphesoftBatchInstanceID', sClass: 'text-center', bSortable: true },
        { sTitle: 'Exception', mData: 'ErrorMsg', bSortable: false },
        { sTitle: 'Status', mData: 'Status', sClass: 'text-center', bSortable: true },
        { sTitle: 'Retry', mData: 'StagingID', bVisible: true, bSortable: false }
      ], aoColumnDefs: [{
        'aTargets': [1],
        'mRender': function (date) {
          if (isTruthy(date)) {
            return convertDateTime(date);
          } else {
            return '';
          }
        }
      }, {
        'aTargets': [5],
        'mRender': function (data, type, row) {
          if (row.Status === StatusConstant.FAILED_ENCOMPASS_DOWNLOAD) {
            return '<div  title=\'' + 'Retry Download' + '\' class =\'LoanDownloadRetry\'> <i class=\'fa fa-retweet\' aria-hidden=\'true\'></i></div>';
          } else {
            return '';
          }
        }
      },
      {
        'aTargets': [4],
        'mRender': function (data, type, row) {
          return '<label title="' + StatusConstant.STATUS_DESCRIPTION[row['Status']] + '" class="bcEllipsis label ' + StatusConstant.STATUS_COLOR[row['Status']] + ' label-table">' + StatusConstant.STATUS_DESCRIPTION[row['Status']] + '</label>';
        }
      }, {
        'aTargets': [2],
        'mRender': function (data) {
          if (isTruthy(data)) {
            return '<a class=\'redirectUrl\' href=\'javascript:void(0)\' >' + data + '</a>';
          } else {
            return '';
          }
        }
      }
      ],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        const self = this;
        $('td .LoanDownloadRetry', row).unbind('click');
        $('td .LoanDownloadRetry', row).bind('click', () => {
          const rowIndex = self.dTable.row(row).index();
          self.RetryMissingDownloadELoan(row, data, rowIndex);
        });
        $('td .redirectUrl', row).unbind('click');
        $('td .redirectUrl', row).bind('click', () => {
          if (data['Status'] === 999) {
            this._notificationService.showError('Batch is not available');
          } else if (data['Status'] === 91) {
            this._notificationService.showSuccess('Batch finished successfully');
          } else {
            self.getEphesofturlValue(data['EphesoftBatchInstanceID']);
          }
        });
      }
    };
    this.subscription.push(
      this._loanImportService.loanimportdata$.subscribe(
        (result: any) => {
          this.dTable.clear();
          this.dTable.rows.add(result);
          this.dTable.draw();
        }
      )
    );
    this.subscription.push(
      this._loanImportService.ActiveLoanTypes$.subscribe(
        (result: any) => {
          this.AllActiveLoanTypes = result;
          this._assignlTypemodal.show();
        }
      )
    );
    this.subscription.push(
      this._loanImportService.isAssignLoanType$.subscribe(
        (result: boolean) => {
          if (result) {
            this.isAssignLoanType = true;
            this.isDeleteLoan = true;
            this.TriggerSearch();
            this._assignlTypemodal.hide();
          }
        }
      )
    );
    this.subscription.push(
      this._loanImportService._missingDocDTable$.subscribe(
        (result: any) => {

          this.MissingdocTable.clear();
          this.MissingdocTable.rows.add(result);
          this.MissingdocTable.draw();
          this.missingDocModal.show();

        }
      )
    );
    this.subscription.push(
      this._loanImportService.isRetryEncompass$.subscribe(
        (result: any) => {
          this.Search(this.receivedDate.dateFrom, this.receivedDate.dateTo);
          this.RetryConfirmModel.hide();
        }
      )
    );
    this.subscription.push(
      this._loanImportService.isDeleteLoan$.subscribe(
        (result: any) => {
          this.isAssignLoanType = true;
          this.isDeleteLoan = true;
         this.TriggerSearch();
          this._confirmDeleteModal.hide();

        }
      )
    );
    this.subscription.push(
      this._loanImportService.isconfirmModal$.subscribe(
        (result: any) => {
          this.LoanAlertMessage = result;
          this.confirmModal.show();
        }
      )
    );
    this.subscription.push(
      this.commonmasterservice.CustomerItems.subscribe(
        (result: any) => {
          this.commonActiveCustomerItems = result;
        }
      )
    );
    this.commonmasterservice.GetCustomerList(AppSettings.TenantSchema);
    this._loanImportService.enableLoanMonitor$.next(false);
  }
  getMissingDoc(rowIndex: Node, rowData: any) {
    const input = new TenantLoanRequestModel(AppSettings.TenantSchema, rowData.LoanID);
    this._loanImportService.getMissingDoc(input);
  }
  viewLoan(rowIndex: Node, rowData: any): void {
    if (isTruthy(rowData)) {
      this._loanImportService.setDataAndRoute(rowData);
    } else {
      this._notificationService.showError('Row Not Fetched');
    }
  }
  LoanDownloadRetry(rowIndex: Node, Data: any) {
    this.rowData = Data;
  }
  RowSelect(rowIndex: Node, rowData: any) {
    if (rowData.Status !== StatusConstant.DELETE_LOAN) {
      setTimeout(() => {
        this.isDeleteLoan = true;
        for (let i = 0; i < this.dTable.rows('.selected').data().length; i++) {
          if (this.dTable.rows('.selected').data()[i].Status === 999) {
            if (this.isDeleteLoan) {
              this.isDeleteLoan = true;
            }
          } else {
            this.isDeleteLoan = false;
          }

        }
      }, 500);

    }
    if (rowData.Status === StatusConstant.IDC_ERROR ) {
      this.isAssignLoanType = $(rowIndex).hasClass('selected');
      this.rowData = rowData;
      if (!($(rowIndex).hasClass('selected'))) {
        this.isAssignLoanType = rowData.LoanType > 0 && rowData.SubStatus !== StatusConstant.LOAN_TYPE_NOT_FOUND;
      }
    } else {

      setTimeout(() => {
        if (this.dTable.rows('.selected').count() === 1) {
          if (this.dTable.rows('.selected').data()[0].Status === 7) {
            this.isAssignLoanType = this.dTable.rows('.selected').data()[0].LoanType > 0;
            this.isDeleteLoan = false;
          } else if (this.dTable.rows('.selected').data()[0].Status === 999) {
            this.isDeleteLoan = true;
            this.isAssignLoanType = true;
          }
        } else if (this.dTable.rows('.selected').count() > 1) {

          if (rowData.Status === StatusConstant.DELETE_LOAN || rowData.Status === StatusConstant.IDC_ERROR) {
            for (let i = 0; i < this.dTable.rows('.selected').data().length; i++) {
              if (this.dTable.rows('.selected').data()[i].Status === 7) {

                this.isDeleteLoan = false;
                this.isAssignLoanType = false;
                this.isAssignLoanType = this.dTable.rows('.selected').data()[i].LoanType > 0;

              } else if (this.dTable.rows('.selected').data()[i].Status === 999) {
                if (this.isDeleteLoan) {
                  this.isDeleteLoan = true;
                  this.isAssignLoanType = true;
                }

              }
            }
          }
        }

      }, 500);

    }
  }

  AssignLoanTypes() {
    const assignDatas = this.dTable.rows('.selected').data();
    if (assignDatas.length > 1) {
      this._notificationService.showError('Please Select any one Row to Assign LoanType');
    } else {
      const input = new CustomerReviewLoanTypeModel(AppSettings.TenantSchema, this.rowData.CustomerID, this.rowData.ReviewTypeID);
      this._loanImportService.AssignLoanTypes(input);
    }
  }

  CancelAssign() {
    this._assignlTypemodal.hide();
    this.selectedLoanTypes = '';
  }

  Assign() {
    if (this.selectedLoanTypes !== 0) {
      this._assignlTypemodal.hide();
      const input = new UpdateLoanMonitorModel(AppSettings.TenantSchema, this.rowData.LoanID, this.selectedLoanTypes, SessionHelper.UserDetails.UserName);
      this._loanImportService.updateLoanMonitor(input);
      this.selectedLoanTypes = '';
    } else {
      this._notificationService.showError('Select a Loan Type');
    }
  }

  checkCurrentUser(row: any) {
    this._loanImportService.checkCurrentUser(row);
  }

  overrideLoanUser() {
    this._loanImportService.overrideLoanUser();
  }
  SelectAll() {
    const seletedDatas = this.dTable.rows().select().data();
    if (this.selectAllBtn) {

      this.dTable.rows().select();
      this.selectAllBtn = false;
      for (let i = 0; i < seletedDatas.length; i++) {
        if (seletedDatas[i].Status !== StatusConstant.DELETE_LOAN) {
          this.isDeleteLoan = false;
        } else {
          $($('.monitor tr')[i + 1]).removeClass('selected');
          if (this.isDeleteLoan) {
            this.isDeleteLoan = true;

          }
        }
      }
    } else {
      this.dTable.rows().deselect();
      this.selectAllBtn = true;
      for (let i = 0; i < seletedDatas.length; i++) {
        if (seletedDatas[i].Status !== StatusConstant.DELETE_LOAN) {
          this.isDeleteLoan = false;
        }
      }
      this.isDeleteLoan = true;
    }
  }

  setInnerTable(rowIndex: Node, rowData: any): void {
    let tempTable = '<table class="table table-striped table-bordered" width="100%">#TR#</table>';
    let tempTr = '<tr class="text-center"><th  class="text-center">File Name</th></tr>';

    rowData['BoxFileName'].split('|').forEach(element => {
      tempTr += '<tr  class="text-center"><td  class="text-center">' + element + '</td></tr>';
    });

    tempTable = tempTable.replace('#TR#', tempTr);

    if (this.dTable.row(rowIndex).child.isShown()) {
      this.dTable.row(rowIndex).child.hide();
    } else {
      this.dTable.row(rowIndex).child($(tempTable)).show();
    }
  }

  getRowData(row: Node, rowData: any): void {
    if (isTruthy(typeof rowData)) {
      this.RestartUpload(rowData['LoanID']);
    }
  }

  ngAfterViewInit() {
    this.StatusSelect = -3;
    this.reviewTypeItems = [];
    this.dtElements.forEach((dtElement: DataTableDirective) => {
      dtElement.dtInstance.then((dtInstance: any) => {
        if (dtInstance.context[0].sTableId === 'firstTable') {
          this.dTable = dtInstance;
          if (typeof this.dTable !== undefined && !this.tableAligned) {
            $('.monitor .dataTables_info').addClass('col-md-6 p0');
            $('.monitor .dataTables_paginate').addClass('col-md-6 p0');
            $('.monitor .dataTables_filter').addClass('col-md-6 p0');
            $('.monitor .dataTables_length').addClass('col-md-6 p0');
            $('.dt-buttons').appendTo('#downloadButton');
            this.tableAligned = true;
          }
        } else if (dtInstance.context[0].sTableId === 'secondTable') {
          this.MissingdocTable = dtInstance;
        }
      });
    });
    setTimeout(() => {
      this.receivedDate.selectRange('to');
      this.TriggerSearch();
    }, 0);
  }

  RestartUpload(loanid: number) {
    const inputData = new TenantLoanRequestModel(AppSettings.TenantSchema, loanid);
    this.promise = this._loanImportService.retryFileUpload(inputData);
  }

  TriggerSearch() {
    this._searchBtn.nativeElement.click();
  }
  Search(fromDate, toDate) {
    const fromDateStr = formatDate(fromDate);
    const toDatStr = formatDate(toDate);
    if (isTruthy(typeof this.receivedDate)) {
      const inputData = new LoanSearchModel(fromDateStr, toDatStr, AppSettings.TenantSchema, 1, this.StatusSelect, this.customerSelect.id, fromDate, toDate);
      this.promise = this._loanImportService.getUploadedItems(inputData);
    }
  }
  reset() {
    this.StatusSelect = -3;
    this.customerSelect = 0;
    this.receivedDate.dateFrom = null;
    this.receivedDate.dateTo = null;
    this.receivedDate.selectRange('to');
    this.dTable.clear().draw();
  }
  RetryMissingDownloadELoan(row: Node, rowData: any, rowIndex: any) {
    const InputData = new RetryEncompassDownloadModel(AppSettings.TenantSchema, rowData.LoanID, rowData.StagingID);
    this._loanImportService.retryDownloadEncomLn(InputData);
  }
  RetryDownloadELoan() {
    const InputData = new RetryEncompassDownloadModel(AppSettings.TenantSchema, this.rowData.LoanID, this.rowData.EDownloadID);
    this._loanImportService.retryDownloadEncomLn(InputData);
  }

  getEphesofturlValue(BatchID: any) {
    const inputs = new GetEphesofturlModel(AppSettings.TenantSchema, BatchID, AppSettings.TenantConfigType[1].ConfigKey, 0);
    this._loanImportService.getEphesofturl(inputs);

  }
  ConfirmDeleteLoans() {
    this._confirmDeleteModal.show();
  }

  DeleteLoans() {
    this.multiRowData = this.dTable.rows('.selected').data();
    const loanLists = [];
    let pendingIDCCount = 0;
    for (let i = 0; i < this.multiRowData.length; i++) {
      if (this.multiRowData[i].Status === StatusConstant.PENDING_IDC) {
        pendingIDCCount++;
      }
      loanLists.push(this.multiRowData[i].LoanID);
    }
    if (pendingIDCCount !== 0) {
      this._notificationService.showSuccess('Please Delete the Loan Documents from Ephesoft also');
    }
    const fullName = SessionHelper.UserDetails.LastName + SessionHelper.UserDetails.FirstName;
    const input = new DeleteLoanModel(AppSettings.TenantSchema, loanLists, fullName);
    this._loanImportService.DeleteLoans(input);
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
