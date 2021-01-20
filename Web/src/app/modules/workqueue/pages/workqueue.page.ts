import {
  Component,
  OnInit,
  OnDestroy,
  ViewChild,
  AfterViewInit,
} from '@angular/core';
import { AppSettings } from '@mts-app-setting';
import { DataTableDirective } from 'angular-datatables';
import { Subscription } from 'rxjs';
import { StatusConstant } from '@mts-status-constant';
import { WorkQueueService } from '../service/workqueue.service';
import { SessionHelper } from '@mts-app-session';
import { convertDateTime } from '@mts-functions/convert-datetime.function';
import { WorkQueueModel } from '../models/workqueue.model';
import { WorkQueueSearchModel } from '../models/workqueue.search.model';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { LoanInfoService } from '../../loan/services/loan-info.service';
import { Router } from '@angular/router';
import { NotificationService } from '@mts-notification';

@Component({
  selector: 'mts-work-queue',
  templateUrl: 'workqueue.page.html',
  styleUrls: ['workqueue.page.css'],
})
export class WorkQueueComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(DataTableDirective) datatableEl: DataTableDirective;
  workQueuedtOptions: any = {};
  dTable: any;
  promise: Subscription;
  AuthorityLabelSingular = AppSettings.AuthorityLabelSingular;

  constructor(
    private _workqueueService: WorkQueueService,
    private _loanService: LoanInfoService,
    private _route: Router,
    private _notificationService: NotificationService
  ) {
  }

  private subscribtion: Subscription[] = [];

  ngAfterViewInit() {
    this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
      this.Search();
    });
  }

  ngOnInit() {
    this.subscribtion.push(this._workqueueService.SearchData.subscribe((res: WorkQueueSearchModel[]) => {
      this.dTable.clear();
      this.dTable.rows.add(res);
      this.dTable.draw();
      this.dTable.columns.adjust();
    }));

    this.workQueuedtOptions = {
      displayLength: 10,
      data: [],
      iDisplayLength: 10,
      aLengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
      aoColumns: [
        { mData: 'LoanID', sClass: 'select-checkbox', bVisible: false },
        { sTitle: AppSettings.AuthorityLabelSingular + '', mData: 'Customer' },
        { sTitle: 'Received Date', type: 'date', mData: 'ReceivedDate', sClass: 'text-center' },
        { sTitle: 'Borrower Name', mData: 'BorrowerName' },
        { sTitle: 'IDC Batch', mData: 'EphesoftBatchInstanceID' },
        { sTitle: 'Loan Type', mData: 'LoanTypeName' },
        { sTitle: 'Loan Amount($)', mData: 'LoanAmount', sClass: 'text-right' },
        { sTitle: 'Loan Status', mData: 'StatusDescription', sClass: 'text-center' },
        { sTitle: 'Borrower Loan #', mData: 'LoanNumber', sClass: 'text-left' },
       // { sTitle: 'View', mData: 'LoanID', sClass: 'text-center' },
        { mData: 'Status', bVisible: false },
        { mData: 'CurrentUserID', bVisible: false },
        { mData: 'ServiceTypeName', bVisible: false, sTitle: 'Service Type' },
        { mData: 'AuditMonthYear', bVisible: false, sTitle: 'Audit Month & Year' },
        { mData: 'ReceivedDate', bVisible: false },
        { mData: 'AuditDueDate', bVisible: false }
      ],
      aoColumnDefs: [
        {
          aTargets: [0],
          orderable: false,
          mRender: function (data) {
            return '';
          }
        },
        {
          aTargets: [2],
          mRender: function (date) {
            if (date !== null && date !== '') {
              return convertDateTime(date);
            } else {
              return date;
            }
          },
        },
        {
          aTargets: [6],
          orderable: false,
          mRender: function (data) {
            return data.toLocaleString();
          },
        },
        {
          aTargets: [7],
          mRender: function (data, type, row) {
            return (
              '<label class=\'label bcEllipsis ' +
              StatusConstant.STATUS_COLOR[row['Status']] +
              ' label-table\' title=\'' +
              StatusConstant.STATUS_DESCRIPTION[row['Status']] +
              '\'>' +
              data +
              '</label>'
            );
          },
        },
        {
          aTargets: [5],
          mRender: function (data, type, row) {
            return row['LoanID'] === 50 ? 'Post-Close Conventional Purchase' : data;
          }
        },
        {
          'aTargets': [8],
          'mRender': function (data, type, row) {
            if (
              row['Status'] === StatusConstant.COMPLETE ||
              row['Status'] === StatusConstant.PENDING_AUDIT
            ) {
              return '<div style="text-decoration: underline;" class="viewLoan">' + data + '</div>';
            } else {
              return '';
            }

          }
        },

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

  getRowData(row: Node, rowData: any): void {
    if (isTruthy(rowData)) {
      this._loanService.SetLoanPageInfo(rowData);
      this._route.navigate(['view/loandetails']);
    } else {
      this._notificationService.showError('Row Not Fetched');
    }
  }

  Search() {
    const req = new WorkQueueModel();
    req.TableSchema = AppSettings.TenantSchema;
    req.CurrentUserID = SessionHelper.UserDetails.UserID;
    this.promise = this._workqueueService.searchSubmit(req);
  }

  ngOnDestroy() {
    this.subscribtion.forEach((element) => {
      element.unsubscribe();
    });
  }
}
