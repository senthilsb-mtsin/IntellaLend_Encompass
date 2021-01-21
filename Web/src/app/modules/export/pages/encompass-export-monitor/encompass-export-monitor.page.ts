import { Subscription } from 'rxjs';
import { ExportService } from './../../service/export.service';
import { Component, OnInit, ViewChild, Input, AfterViewInit, OnDestroy, ViewChildren, QueryList } from '@angular/core';
import { convertDateTimewithTime, formatDate } from '@mts-functions/convert-datetime.function';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppSettings, EncompassExportStatusConstant, EncompassUploadStagingConstant } from '@mts-app-setting';
import { NgDateRangePickerComponent } from '@mts-daterangepicker/ng-daterangepicker.component';
import { CommonService } from 'src/app/shared/common';
import { ExportConfigModel } from '../../models/export-data.model';
import { DataTableDirective } from 'angular-datatables';
import { EncompassExportModel, EncompassSearchExportModel } from '../../models/encompass.export.model';

@Component({
  selector: 'mts-encompass-export-monitor',
  templateUrl: './encompass-export-monitor.page.html',
  styleUrls: ['./encompass-export-monitor.page.css']
})
export class EncompassExportMonitorComponent implements OnInit, AfterViewInit, OnDestroy {
  EncompassExportMonitorDTOptions: any; EUploadstagingDTOptions: any;
  EncompassExportMonitorDTble: any;
  dTable2: any;
  @ViewChildren(DataTableDirective) encompassdtElement: QueryList<DataTableDirective>;
  @ViewChild('EUploadViewmodel') EUploadViewmodel: ModalDirective;
  @ViewChild('RetryConfirmModel') RetryConfirmModel: ModalDirective;
  @ViewChild(NgDateRangePickerComponent) ExportMonitorReceivedDate: NgDateRangePickerComponent;
  endata = new ExportConfigModel();
  row: any;
  options: any;
  AuthorityLabelSingular: string = AppSettings.AuthorityLabelSingular;
  EncompassExportMonitorFromdate: any;
  constructor(private _exportservice: ExportService
    , private commonmasterservice: CommonService) { }
  private subscription: Subscription[] = [];
  ngOnInit(): void {
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
    this.EncompassExportMonitorDTOptions = {
      'select': {
        style: 'single',
        info: false
      },
      'iDisplayLength': 10,
      'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
      'order': [[7, 'desc']],
      aoColumns: [
        { sTitle: 'ID', mData: 'ID', bVisible: false },
        { sTitle: 'LoanID', mData: 'LoanID', bVisible: false },
        { sTitle:  AppSettings.AuthorityLabelSingular + ' Name', mData: 'CustomerName', sWidth: '10%'},
        { sTitle: 'ELoanGUID', mData: 'ELoanGUID', bVisible: false, sClass: 'text-center' },
        { sTitle: 'Type Of Upload', mData: 'TypeOfUpload', bVisible: false },
        { sTitle: 'LoanNumber', mData: 'LoanNumber', sClass: 'text-center' },
        { sTitle: 'Exception', mData: 'Error' },
        { sTitle: 'Uploaded On', 'type': 'date', mData: 'CreatedOn', sClass: 'text-center' },
        { sTitle: 'Status', mData: 'Status', sClass: 'text-center' },
        { sTitle: 'View', mData: 'ID', sClass: 'text-center' },
        { sTitle: 'Retry', mData: 'ID', sClass: 'text-center' },
      ],
      aoColumnDefs: [
        {
          'aTargets': [8],
          'mRender': function (data, type, row) {
            return '<label title="' + EncompassExportStatusConstant.ENCOMPASS_EXPORT_STATUS_DESCRIPTION[row['Status']] + '" class="bcEllipsis label ' + EncompassExportStatusConstant.ENCOMPASS_EXPORT_LOAN_STATUS_COLOR[row['Status']] + ' label-table">' + EncompassExportStatusConstant.ENCOMPASS_EXPORT_STATUS_DESCRIPTION[row['Status']] + '</label>';
          },
        },
        {
          'aTargets': [7],
          'mRender': function (date) {
            return isTruthy(date) ? convertDateTimewithTime(date) : '';
          }
        },
        {
          'aTargets': [9],
          'mRender': function (data, type, row) {
            if(row.Status !== 2){
              return '<span style=\'cursor: pointer;\' class=\'viewEUpload material-icons txt-info\'>pageview</span>';
            }
            else{
              return '';
            }
          }
        },
        {
          'aTargets': [10],
          'mRender': function (data, type, row) {
            if (row.Status === -1) {
              return '<div  style=\'cursor: pointer;\'class =\'EncompassExportRetry\'> <i class=\'fa fa-retweet\'aria-hidden=\'true\'></i></div>';
            } else {
              return '';
            }
          }
        }
      ],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        const self = this;
        $('td .viewEUpload', row).unbind('click');
        $('td .viewEUpload', row).bind('click', () => {
          self.GetEncompassExportRowData(row, data);
        });
        $('td .EncompassExportRetry', row).unbind('click');
        $('td .EncompassExportRetry', row).bind('click', () => {
          this.RetryConfirmModel.show();
          self.EncompassExportRetry(row, data);
        });
      }
    };
    this.EUploadstagingDTOptions = {
      'select': {
        style: 'single',
        info: false
      },
      'iDisplayLength': 10,
      'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
      aoColumns: [
        { sTitle: 'ID', mData: 'ID', sClass: 'text-center', bVisible: false },
        { sTitle: 'UploadStagingID', mData: 'UploadStagingID', sClass: 'text-center', sWidth: '10%', bVisible: false },
        { sTitle: 'LoanID', mData: 'LoanID', sClass: 'text-center', bVisible: false },
        { sTitle: 'Document Name', mData: 'Document' },
        { sTitle: 'EFolder Name', mData: 'EParkingSpot' },
        { sTitle: 'File Name', mData: 'FileName' },
        { sTitle: 'Exception', mData: 'ErrorMsg' },
        { sTitle: 'Status', mData: 'Status', sClass: 'text-center', sWidth: '30%' },
        { sTitle: 'Created On', mData: 'CreatedOn', sClass: 'text-center', bVisible: false },
        { sTitle: 'Modified On ', mData: 'ModifiedOn', sClass: 'text-center', bVisible: false },
      ],
      aoColumnDefs: [
        {
          'aTargets': [7],
          'mRender': function (data, type, row) {
            return '<label title="' + EncompassUploadStagingConstant.ENCOMPASS_UPLOAD_STAGING_STATUS_DESCRIPTION[row['Status']] + '" class="bcEllipsis label ' + EncompassUploadStagingConstant.ENCOMPASS_UPLOAD_STAGING_STATUS_COLOR[row['Status']] + ' label-table">' + EncompassUploadStagingConstant.ENCOMPASS_UPLOAD_STAGING_STATUS_DESCRIPTION[row['Status']] + '</label>';
          },
        },
      ],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        const self = this;
        $('td .EUploadStagingRetry', row).unbind('click');
        $('td .EUploadStagingRetry', row).bind('click', () => {
          self.RetryUploadStaging(row, data);
        });
      }
    };
    this.subscription.push(
      this.commonmasterservice.CustomerItems.subscribe(
        (result: any) => {
          this.endata.commonActiveCustomerItems = result;
        }
      )
    );
    this.subscription.push(
      this._exportservice.encompassmonitordata$.subscribe((result) => {
        this.EncompassExportMonitorDTble.clear();
        this.EncompassExportMonitorDTble.rows.add(result);
        this.EncompassExportMonitorDTble.draw();
      })
    );
    this.subscription.push(
      this._exportservice.encompasstagingdata$.subscribe((result) => {
        this.dTable2.clear();
        this.dTable2.rows.add(result);
        this.dTable2.draw();
        this.EUploadViewmodel.show();
      })
    );
    this.subscription.push(
      this._exportservice.retryEexport$.subscribe((result) => {
        this.RetryConfirmModel.hide();
        this.SearchEncompassExportDetails(this.ExportMonitorReceivedDate.dateFrom, this.ExportMonitorReceivedDate.dateTo);
      })
    );
  }
  RetryUploadStaging(row: any, rowData: any) {
    this.RetryEncompassUploadStaging(rowData);
  }

  EncompassExportRetry(rowIndex: Node, Data: any) {
    this.row = rowIndex;
    this.endata.rowData = Data;
  }

  GetEncompassExportRowData(row: any, rowData: any) {
    this.GetEncompassExportData(rowData);
  }

  GetEncompassExportData(rowData: any) {
    const inputdata = new EncompassExportModel(AppSettings.TenantSchema, rowData.ID, rowData.LoanID);
    this._exportservice.GetEncompassExportDetails(inputdata);
  }

  ngAfterViewInit() {
    this.commonmasterservice.GetCustomerList(AppSettings.TenantSchema);
    this.encompassdtElement.forEach((dtElement: DataTableDirective) => {
      dtElement.dtInstance.then((dtInstance: any) => {
        if (dtInstance.context[0].sTableId === 'encompassTable') {
          this.EncompassExportMonitorDTble = dtInstance;
          this.SearchEncompassExportDetails(this.ExportMonitorReceivedDate.dateFrom, this.ExportMonitorReceivedDate.dateTo);
        } else if (dtInstance.context[0].sTableId === 'EUploadStagingViewDTble') {
          this.dTable2 = dtInstance;
        }
      });
    });
    if (this.encompassdtElement !== undefined) {
      $('.dataTables_info').addClass('col-md-6 p0');
      $('.dataTables_paginate').addClass('col-md-6 p0');
      $('.dataTables_filter').addClass('col-md-6 p0');
      $('.dataTables_length').addClass('col-md-6 p0');
    }
  }

  SearchEncompassExportDetails(FrmDate: any, ToDate: any) {
    let fromDateStr, toDatStr;
    if (isTruthy(FrmDate) && isTruthy(ToDate)) {
      fromDateStr = formatDate(FrmDate);
      toDatStr = formatDate(ToDate);
    } else {
      fromDateStr = null;
      toDatStr = null;
    }
    this.EncompassExportMonitorFromdate = fromDateStr;
    this.EncompassExportMonitorFromdate = toDatStr;
    const encompassexportmodel = { FromDateStr: fromDateStr, ToDateStr: toDatStr };
    const InputReq = new EncompassSearchExportModel(AppSettings.TenantSchema, encompassexportmodel, this.endata.UploadStatus, this.endata.customerSelect?.id === undefined ? 0 : this.endata.customerSelect.id);
    this.endata.promise = this._exportservice.SearchEncompassExportDetails(InputReq);
  }

  RetryEncompassUploadStaging(rowData: any) {
    const InputData = new EncompassExportModel(AppSettings.TenantSchema, rowData.ID, rowData.LoanID);
    this._exportservice.RetryEncompassUploadStaging(InputData);
  }

  RetryEncompassExport() {
    const InputData = new EncompassExportModel(AppSettings.TenantSchema, this.endata.rowData.ID, this.endata.rowData.LoanID);
    this._exportservice.RetryEncompassExport(InputData);
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
  backToexport() {
    this.endata.customerSelect = 0;
    this.endata.UploadStatus = 5;
    this.EncompassExportMonitorDTble.search('');
    this.SearchEncompassExportDetails(this.ExportMonitorReceivedDate.dateFrom, this.ExportMonitorReceivedDate.dateTo);
  }
}
