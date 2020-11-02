import {
  Component,
  OnInit,
  ViewChild,
  AfterViewInit,
  OnDestroy,
  ElementRef,
} from '@angular/core';
import { AppSettings } from '@mts-app-setting';
import { DatePipe } from '@angular/common';
import { PurgeMonitorRequest } from '../../models/get-purge-monitor-request.model';
import { LoanPurgeService } from '../../service/loanpurge.service';
import { StatusConstant } from '@mts-status-constant';
import { DataTableDirective } from 'angular-datatables';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { NgDateRangePickerOptions } from '../../../../shared/custom-plugins/ng-daterangepicker-master/ng-daterangepicker.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { convertDateTime } from '@mts-functions/convert-datetime.function';
import { SessionHelper } from '@mts-app-session';
import {
  RetentionPurge,
  PurgeStaging,
} from '../../models/retention-purge-model';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-purge',
  templateUrl: './purgemonitor.page.html',
  styleUrls: ['./purgemonitor.page.css'],
})
export class PurgeMonitorComponent implements OnInit, AfterViewInit, OnDestroy {
  ReviewStatusMaster: any = [];
  isRetryEnabled = true;
  isRowSelectEnabled = true;
  promise: any;
  rowDatas: any;
  isRowSelectrDeselect = true;
  purgeMonitorDTOptions: any = {};
  ReviewStatus: any = 0;
  value: any;
  isPurgeMonitorDataExists = false;
  @ViewChild('lp_btnSearch') _searchBtn: ElementRef;
  @ViewChild(DataTableDirective) dt: DataTableDirective;
  @ViewChild('confirmModal') confirmModal: ModalDirective;
  options: NgDateRangePickerOptions;
  dTable: any;

  rData: any;
  isPurgeMonitorTab: boolean;
  constructor(
    private datePipe: DatePipe,
    private _purgeservice: LoanPurgeService,
    private route: ActivatedRoute,
    private router: Router
  ) { }
  private purgeMonitorFromdate: any;
  private purgeMonitorTodate: any;
  private subscription: Subscription[] = [];
  private isSelectEnabled = true;
  ngOnInit(): void {
    this.options = {
      theme: 'default',
      previousIsDisable: false,
      nextIsDisable: false,
      range: 'to',
      dayNames: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
      presetNames: [
        'Today',
        'This Month',
        'Last Month',
        'This Week',
        'Last Week',
        'This Year',
        'Last Year',
      ],
      dateFormat: 'M/d/y',
      outputFormat: 'dd/MM/yyyy',
      startOfWeek: 1,
      display: {
        to: 'block',
        tm: 'block',
        lm: 'block',
        lw: 'block',
        tw: 'block',
        ty: 'block',
        ly: 'block',
        custom: 'block',
        em: 'block',
      },
    };
    this.purgeMonitorDTOptions = {
      select: {
        style: 'multi',
        info: false,
        selector: 'td:first-child',
      },
      iDisplayLength: 10,
      aaData: [],
      aLengthMenu: [
        [5, 10, 25, 50, -1],
        [5, 10, 25, 50, 'All'],
      ],
      aoColumns: [
        { mData: 'BatchID', sClass: 'select-checkbox', bVisible: true },
        { mData: 'BatchID', bVisible: false },
        { sTitle: 'Batch Count', mData: 'BatchCount', bVisible: true, sClass: 'text-center'},
        { sTitle: 'Status', mData: 'Status', sClass: 'text-center' },
        { sTitle: 'Created On', mData: 'CreatedOn', sClass: 'text-center' },
        { sTitle: 'Modified On', mData: 'ModifiedOn', sClass: 'text-center' },
        { sTitle: 'View', mData: 'BatchID', sClass: 'text-center' },
      ],
      aoColumnDefs: [
        {
          aTargets: [0],
          orderable: false,
          mRender: function (data) {
            return '';
          },
        },
        {
          aTargets: [3],
          mRender: function (data, row, type) {

            return (
              '<label class=\'label ' +
              StatusConstant.STATUS_COLOR[data] +
              ' label-table\'>' +
              StatusConstant.STATUS_DESCRIPTION[data] +
              '</label>'
            );
          },
        },
        {
          aTargets: [6],
          mRender: function (data, type, row) {
            return '<span style=\'cursor:pointer\' class=\'ViewPurgeDetails material-icons txt-info\'>pageview</span>';
          },
        },
        {
          aTargets: [5],
          mRender: function (date) {
            if (date !== null && date !== '') {
              return convertDateTime(date);
            } else {
              return date;
            }
          },
        },
        {
          aTargets: [4],
          mRender: function (date) {

            if (date !== null && date !== '') {

              return convertDateTime(date);
            } else { return date; }
          },
        },
      ],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        const self = this;
        $('td .ViewPurgeDetails', row).unbind('click');
        $('td .ViewPurgeDetails', row).bind('click', () => {
          self.ViewPurgeDetails(row, data);
        });
        return row;
      },
    };

    this.subscription.push(
      this._purgeservice.ReviewStatusMaster.subscribe((res: any) => {
        this.ReviewStatusMaster = [];
        this.ReviewStatusMaster.push({ id: 0, text: '--Select Loan Status--' });
        if (res !== null) {
          // tslint:disable-next-line:no-shadowed-variable
          res.forEach((element) => {
            this.ReviewStatusMaster.push({
              id: element.StatusID,
              text: element.StatusDescription,
            });
          });
        }
      })
    );
    this.subscription.push(
      this._purgeservice.purgeMonitorDTabledata.subscribe((res) => {

        this.dTable.clear();
        this.dTable.rows.add(res);

        if (this.dTable.rows().count() > 0) {
          this.isSelectEnabled = true;
        }
        this.dTable.draw();

      })
    );
    this.subscription.push(
      this._purgeservice.isRowSelectEnabled.subscribe((res: boolean) => {
        this.isRowSelectEnabled = res;
      })
    );
    this.subscription.push(
      this._purgeservice.isRowSelectrDeselect.subscribe((res: boolean) => {
        this.isRowSelectrDeselect = res;
      })
    );
    this._purgeservice.LoanPurgeSearch.subscribe((res) => {
      if (res === 'retrypurge') {
        this.TriggerSearch();
      }
    });
    this.GetLoanRetentionStatus();
  }

  ngAfterViewInit(): void {
    this.dt.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
      if (isTruthy(this.dTable)) {
        this.PurgeMonitorSearch(new Date(), new Date());
      }
      dtInstance.on('select', (s) => {
        const datas = this.dTable.rows('.selected').data();
        this.isSelectEnabled = !(this.dTable.rows('.selected').data().length > 0);
        for (let i = 0; i < datas.length; i++) {
          if (datas[i].Status === StatusConstant.PURGE_FAILED || datas[i].Status === StatusConstant.EXPORT_FAILED) {
            this.getSelectAllData(this.isSelectEnabled);
          } else {
            this.getSelectAllData(true);
          }
        }
      });
      dtInstance.on('deselect', (s) => {
        this.isSelectEnabled = !(this.dTable.rows('.selected').data().length > 0);
        this.getSelectAllData(this.isSelectEnabled);
      });
    });
  }

  TriggerSearch() {
    this._searchBtn.nativeElement.click();
  }
  GetLoanRetentionStatus() {
    this.promise = this._purgeservice.GetPurgeStatus();
  }
  getSelectAllData(value: any) {

    this.rowDatas = '';
    this.rowDatas = this.getSingleSelectedRowData();
    this.isRowSelectrDeselect = value;
    this.isRetryEnabled = value;
  }
  getSingleSelectedRowData() {
    let rSingleData: any = [];
    rSingleData = this.dTable.rows('.selected').data();
    return rSingleData;
  }
  getSelectAllRowData() {
    this.rowDatas = '';
    this.rowDatas = this.getSelectedRowData();
    if (this.rowDatas !== undefined) {
      for (let i = 0; i < this.rowDatas.length; i++) {
        if (
          this.rowDatas[i].Status === StatusConstant.PURGE_FAILED ||
          this.rowDatas[i].Status === StatusConstant.EXPORT_FAILED
        ) {
          this.isRetryEnabled = false;
        } else {
          this.isRetryEnabled = true;
        }
      }
      if (this.isRowSelectrDeselect) {
        this.isRowSelectrDeselect = false;
      } else {
        this.isRowSelectrDeselect = true;
      }
    }
  }

  getSelectedRowData() {
    if (this.isSelectEnabled) {
      this.dTable.rows().select();
      this.rData = this.dTable.rows().data();
      this.isSelectEnabled = false;
      return this.rData;
    } else {
      this.dTable.rows().deselect();
      this.isSelectEnabled = true;
    }
  }
  Retry() {
    const batchIDS = [];
    for (let i = 0; i < this.rowDatas.length; i++) {
      if (
        this.rowDatas[i].Status === StatusConstant.PURGE_FAILED ||
        this.rowDatas[i].Status === StatusConstant.EXPORT_FAILED
      ) {
        batchIDS.push(this.rowDatas[i].BatchID);
      }
    }
    const inputRequest = {
      TableSchema: AppSettings.TenantSchema,
      BatchIDs: batchIDS,
    };
    this.promise = this._purgeservice.RetryPurge(inputRequest);
  }
  PurgeMonitorSearch(pmFrmDate: any, pmToDate: any) {
    this.purgeMonitorFromdate = pmFrmDate;
    this.purgeMonitorTodate = pmToDate;
    const pMFD = this.datePipe.transform(pmFrmDate, AppSettings.dateFormat);
    const pMTD = this.datePipe.transform(pmToDate, AppSettings.dateFormat);
    const pmInputReq = new PurgeMonitorRequest(
      AppSettings.TenantSchema,
      pMFD,
      pMTD,
      this.ReviewStatus
    );
    this.promise = this._purgeservice.getPurgedLoans(pmInputReq);
  }
  RetentionPurge(types: any) {
    let CurrentStatus;
    if (types === 102) {
      CurrentStatus = StatusConstant.PURGE_WAITING;
    } else if (types === 104) {
      CurrentStatus = StatusConstant.EXPORT_WAITING;
    }
    const rows = this.dTable.rows('.selected').data();
    const loanID = [];
    for (let i = 0; i < rows.length; i++) {
      loanID.push(rows[i].LoanID);
    }
    const purgeStagingData = new PurgeStaging(rows.length, '', CurrentStatus);
    const inputReq = new RetentionPurge(AppSettings.TenantSchema, SessionHelper.UserDetails.FirstName + '' + SessionHelper.UserDetails.LastName, loanID, purgeStagingData);
    this.promise = this._purgeservice.RetentionPurge(inputReq);
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
  ViewPurgeDetails(rowNode, rowData) {
    this.router.navigate(['purgedetail'], { relativeTo: this.route });
    const inputRequest = {
      TableSchema: AppSettings.TenantSchema,
      BatchID: rowData.BatchID,
    };
    this.promise = this._purgeservice.GetPurgeBatchDetails(inputRequest);
  }
}
