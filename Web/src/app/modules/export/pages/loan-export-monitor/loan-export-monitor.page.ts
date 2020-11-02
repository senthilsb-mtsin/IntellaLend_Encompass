import { Router } from '@angular/router';
import { Component, OnInit, ViewChild, OnDestroy, AfterViewInit, QueryList, ViewChildren } from '@angular/core';
import { CommonService } from 'src/app/shared/common';
import { NgDateRangePickerComponent } from '@mts-daterangepicker/ng-daterangepicker.component';
import { AppSettings, ExportLoanStatusConstant } from '@mts-app-setting';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { StatusConstant } from '@mts-status-constant';
import { convertDateTime, formatDate } from '@mts-functions/convert-datetime.function';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { ExportService } from '../../service/export.service';
import { Subscription } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import { ExportConfigModel } from '../../models/export-data.model';
import { SearchExportModel } from '../../models/encompass.export.model';
import { TenantRequestModel } from 'src/app/modules/loan-import/models/tenant-request.model';
import { LoanJobModel, LoanExportModel } from '../../models/loan-export.model';
@Component({
  selector: 'mts-loan-export-monitor',
  templateUrl: './loan-export-monitor.page.html',
  styleUrls: ['./loan-export-monitor.page.css']
})
export class LoanExportMonitorComponent implements OnInit, OnDestroy, AfterViewInit {
  dtOptions: any; BatchLoanDtOptions: any;
  @ViewChild('viewmodel') viewmodel: ModalDirective;
  @ViewChild('Deletemodel') Deletemodel: ModalDirective;
  @ViewChild('retryalert') retryalert: ModalDirective;
  @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;
  @ViewChild(NgDateRangePickerComponent) receivedDate: NgDateRangePickerComponent;
  @ViewChild('confirmModal') confirmModal: ModalDirective;
  edata = new ExportConfigModel();
  loanexporttable: any;
  batchloantable: any;
  DeleteData: any;
  RowData: any;
  options: any;
  AuthorityLabelSingular: string = AppSettings.AuthorityLabelSingular;

  constructor(private commonmasterservice: CommonService
    , private _exportservice: ExportService
    , private _route: Router
  ) {
  }
  private subscription = new Subscription();
  ngOnInit(): void {
    this.dtOptions = {
      displayLength: 10,
      'select': {
        style: 'single',
        info: false
      },
      'scrollX': true,
      'order': [0, 'desc'],
      'iDisplayLength': 10,
      'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
      aoColumns: [
        { sTitle: 'JobID', mData: 'JobID', sClass: 'text-center', bVisible: false },
        { sTitle:  AppSettings.AuthorityLabelSingular + ' Name', mData: 'CustomerName', sWidth: '10%' },
        { sTitle: 'Job Name', mData: 'JobName'},
        { sTitle: 'Exported By', mData: 'ExportedBy' },
        { sTitle: 'Job Date', 'type': 'date', mData: 'CreatedOn', sClass: 'text-center' },
        { sTitle: 'Status', mData: 'Status', sClass: 'text-center' },
        { mData: 'Status', bVisible: false },
        { sTitle: 'Loan Count', mData: 'LoanCount', sClass: 'text-center' },
        { sTitle: 'Export Path', mData: 'ExportPath', sWidth: '10%' },
        { sTitle: 'View', mData: 'JobID', sClass: 'text-center' },
        { sTitle: 'Password', mData: 'Password', sClass: 'text-center' },
        { sTitle: 'Error Message ', mData: 'ErrorMsg' },
        { sTitle: 'Delete', mData: 'JobID', sClass: 'text-center' },
        { sTitle: 'PasswordProtected', mData: 'PasswordProtected', sClass: 'text-center', bVisible: false },
      ],
      aoColumnDefs: [
        {
          'aTargets': [9],
          'mRender': function (data, type, row) {
            if (row['Status'] !== StatusConstant.JOB_DELETED) {
              return '<span style=\'cursor: pointer;\' class=\'viewBatch material-icons txt-info\'>pageview</span>';
            } else {
              return '';
            }
          }
        },
        {
          'aTargets': [4],
          'mRender': function (date) {
            return isTruthy(date) ? convertDateTime(date) : '';
          }
        },
        {
          'aTargets': [5],
          'mRender': function (data, type, row) {
            return '<label style=\'width: 140px\' class=\'label ' + StatusConstant.STATUS_COLOR[row['Status']] + ' label-table\'>' + StatusConstant.STATUS_DESCRIPTION[row['Status']] + '</label>';

          },
        },
        {
          'aTargets': [12],
          'mRender': function (data, type, row) {
            if (row['Status'] === StatusConstant.JOB_WAITING) {
              return '<button class=\'btn btn-sm btn-danger deletebatch\' value =' + data + '> Delete  </button>';
            } else {
              return '';
            }
          },
        },
        {
          'aTargets': [10],
          'mRender': function (data, type, row) {
            if (row['PasswordProtected'] === true) {
              return '<div class =\'hidepasswordIcon hidePwsdClass\'> <i style=\'cursor: pointer;\'class=\'fa fa-eye\'></i></div><div class =\'row\'><div class =\'showpasswordIcon\' style=\'display :none;\'><span> ' + data + ' &nbsp;&nbsp;</span> </div></div>';
            } else {
              return '';
            }
          },
        },
      ],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        const self = this;
        $('td .viewBatch', row).unbind('click');
        $('td .viewBatch', row).bind('click', () => {
          self.getRowData(row, data);
        });
        $('td:first-child', row).unbind('click');
        $('td:first-child', row).bind('click', () => {
          self.RowSelect(row, data);
        });
        $('td .deletebatch', row).unbind('click');
        $('td .deletebatch', row).bind('click', () => {
          self.DeleteRowData(row, data);
        });

        $('td .hidepasswordIcon', row).unbind('click');
        $('td .hidepasswordIcon', row).bind('click', (e) => {
          self.PasswordHide(row, data);
        });
        $('td .showpasswordIcon', row).unbind('click');
        $('td .showpasswordIcon', row).bind('click', (e) => {
          self.PasswordShow(row, data);
        });
        return row;
      }
    };
    this.options = {
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
    this.BatchLoanDtOptions = {
      'select': {
        style: 'single',
        info: false
      },
      'iDisplayLength': 10,
      'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
      aoColumns: [
        { sTitle: 'JobID', mData: 'JobID', sClass: 'text-center', bVisible: false },
        { sTitle: 'Customer Name', mData: 'CustomerName', sWidth: '10%' },
        { sTitle: 'Service Type Name', mData: 'ReviewTypeName' },
        { sTitle: 'Loan Type Name', mData: 'LoanTypeName'},
        { sTitle: 'Borrower Loan#', mData: 'LoanNumber', sClass: 'text-center' },
        { sTitle: 'Export Loan Status', mData: 'LoanStatus', sClass: 'text-center' },
        { sTitle: 'Error Message ', mData: 'LoanMessage', sClass: 'text-center' },
        { sTitle: 'Retry', mData: 'JobID', sClass: 'text-center' },
      ],
      aoColumnDefs: [
        {
          'aTargets': [5],
          'mRender': function (data, type, row) {
            return '<label class=\'label ' + ExportLoanStatusConstant.EXPORT_LOAN_STATUS_COLOR[row['LoanStatus']] + ' label-table\'>' + ExportLoanStatusConstant.EXPRT_LOAN_STATUS_DESCRIPTION[row['LoanStatus']] + '</label>';
          },
        },
        {
          'aTargets': [7],
          'mRender': function (data, type, row) {
            if (row.LoanStatus === -1) {
              return '<div class =\'JobExportRetry\'> <i class=\'fa fa-retweet\' aria-hidden=\'true\'></i></div>';
            } else {
              return '';
            }

          }
        }
      ],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        const self = this;
        $('td .JobExportRetry', row).unbind('click');
        $('td .JobExportRetry', row).bind('click', () => {
          this.retryalert.show();
          self.JobExportRetry(row, data);
        });
      }
    };
    this.subscription.add(
      this.commonmasterservice.CustomerItems.subscribe(
        (result: any) => {
          this.edata.commonActiveCustomerItems = result;
        }
      )
    );
    this.subscription.add(
      this._exportservice.BatchData$.subscribe((result) => {
        this.batchloantable.clear();
        this.batchloantable.rows.add(result);
        this.batchloantable.draw();
        this.viewmodel.show();
      })
    );
    this.subscription.add(
      this._exportservice.exportmonitordata$.subscribe((result: any) => {
        this.loanexporttable.clear();
        this.loanexporttable.rows.add(result);
        this.loanexporttable.draw();
      })
    );
    this.subscription.add(
      this._exportservice.DeleteBatch$.subscribe((result) => {
        this.GetExportBatchDetails();
        this.Deletemodel.hide();
      })
    );
    this.subscription.add(
      this._exportservice.retryExport$.subscribe((result) => {
        this.retryalert.hide();
        this.CloseViewModal();
        this.SearchBatchLoan(this.receivedDate.dateFrom, this.receivedDate.dateTo);
      })
    );
  }

  GetCurrentBatchData(rowData: any) {
    const inputdata = new LoanJobModel(AppSettings.TenantSchema, rowData.JobID);
    this._exportservice.GetCurrentBatchData(inputdata);
  }
  setCustomer(input: any) {
    this._exportservice.AddCustomer = input;
  }
  Addbatch() {
    this._exportservice.AddCustomer = 0;

    this.confirmModal.show();
  }
  RowSelect(row: any, rowData: any) {
    this.RowData = rowData;
  }
  getRowData(row: any, rowData: any) {
    this.GetCurrentBatchData(rowData);
  }
  DeleteRowData(row: any, rowData: any) {
    this.Deletemodel.show();
    this.DeleteData = rowData.JobID;
  }
  PasswordHide(rowIndex: Node, Data: any) {
    $(rowIndex).find('.hidepasswordIcon').hide();
    $(rowIndex).find('.showpasswordIcon').show();
  }
  PasswordShow(rowIndex: Node, Data: any) {
    $(rowIndex).find('.showpasswordIcon').hide();
    $(rowIndex).find('.hidepasswordIcon').show();
  }
  CloseViewModal() {
    this.viewmodel.hide();
    this.loanexporttable.search('');
  }
  SearchBatchLoan(dateFrom: any, dateTo: any) {
    let fromDateStr;
    let toDatStr;
    if (isTruthy(dateFrom) && isTruthy(dateTo)) {
      fromDateStr = formatDate(dateFrom);
      toDatStr = formatDate(dateTo);
    } else {
      fromDateStr = null;
      toDatStr = null;
    }
    const exportmodel = { FromDateStr: fromDateStr, ToDateStr: toDatStr };
    const inputdata = new SearchExportModel(AppSettings.TenantSchema, exportmodel, this.edata.ExportStatus, this.edata.customerSelect?.id === undefined ? 0 : this.edata.customerSelect.id);
    this._exportservice.SearchExportMonitorDetails(inputdata);
  }
  reset() {
    this.edata.customerSelect = 0;
    this.edata.ExportStatus = -1;
    this.receivedDate.dateFrom = null;
    this.receivedDate.dateTo = null;
    this.GetExportBatchDetails();
  }
  ngAfterViewInit() {
    this.commonmasterservice.GetCustomerList(AppSettings.TenantSchema);
    this.dtElements.forEach((dtElement: DataTableDirective) => {
      dtElement.dtInstance.then((dtInstance: any) => {
        if (dtInstance.context[0].sTableId === 'loanexportmonitor') {
          this.loanexporttable = dtInstance;
          if (isTruthy(this.loanexporttable)) {
            this.SearchBatchLoan(this.receivedDate.dateFrom, this.receivedDate.dateTo);
          }
        } else if (dtInstance.context[0].sTableId === 'batchloantable') {
          this.batchloantable = dtInstance;
        }
      });
    });
  }

  GetExportBatchDetails() {
    const inputdata = new TenantRequestModel(AppSettings.TenantSchema);
    this.edata.promise = this._exportservice.GetExportMonitorDetails(inputdata);
  }
  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
  BatchDeletemodel() {
    const inputdata = new LoanJobModel(AppSettings.TenantSchema, this.DeleteData);
    this._exportservice.DeleteBatch(inputdata);
  }
  JobExportRetry(rowIndex: Node, Data: any) {
    this.retryalert.show();
    this.edata.rowData = Data;
  }
  RetryExport() {
    const inputdata = new LoanExportModel(AppSettings.TenantSchema, this.edata.rowData.JobID, this.edata.rowData.LoanID);
    this._exportservice.RetryLoanExport(inputdata);
  }
  savebatch(CustomerId: any) {
    if (!this._exportservice.validate()) {
      this.confirmModal.hide();
      this._exportservice.AddbatchCustomer$.next(CustomerId);
      this._route.navigate(['view/export/addbatchloan']);
    }
  }
}
