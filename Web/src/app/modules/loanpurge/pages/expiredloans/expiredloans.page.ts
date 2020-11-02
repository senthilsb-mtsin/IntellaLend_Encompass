import {
  Component,
  OnInit,
  ViewChild,
  AfterViewInit,
  OnDestroy,
} from '@angular/core';
import { NgDateRangePickerOptions, NgDateRangePickerComponent } from '../../../../shared/custom-plugins/ng-daterangepicker-master/ng-daterangepicker.component';
import { Subscription } from 'rxjs';
import { LoanPurgeService } from '../../service/loanpurge.service';
import { DatePipe } from '@angular/common';
import { AppSettings } from '@mts-app-setting';
import { StatusConstant } from '@mts-status-constant';
import { DataTableDirective } from 'angular-datatables';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { SessionHelper } from '@mts-app-session';
import { DashboardCommonServiceModel } from 'src/app/modules/dashboard/models/dashboard-common.model';

@Component({
  selector: 'app-expiredloans',
  templateUrl: './expiredloans.page.html',
  styleUrls: ['./expiredloans.page.css'],
})
export class ExpiredloansComponent implements OnInit, AfterViewInit, OnDestroy {
  options: NgDateRangePickerOptions;
  promise: Subscription;

  isPurgeEnabled = true;
  isPurgeRowSelectEnabled = true;
  dtOptions: any = {};
  dTable: any;
  value: any;
  isPurgeRowSelectDeselect = true;
  @ViewChild(NgDateRangePickerComponent) receivedDate: NgDateRangePickerComponent;
  @ViewChild(DataTableDirective) dt: DataTableDirective;
  @ViewChild('purgeMsgModal') confirmModal: ModalDirective;
  constructor(
    private _purgeservice: LoanPurgeService,
    private datePipe: DatePipe,
    private _commonService: DashboardCommonServiceModel,
  ) { }
  private subscription: Subscription[] = [];
  private rowDatas: any = [];
  private fromdate: any;
  private todate: any;
  private searchData: any = [];
  ngOnInit(): void {
    this.subscription.push(
      this._purgeservice.LoanPurgeSearch.subscribe((res) => {
        if (res === 'retrypurge') {
          this.LoanPurgeSearch(this.fromdate, this.todate);
        } else {
          this.LoanPurgeSearch(this.fromdate, this.todate);
          this.confirmModal.hide();
        }
      })
    );
    this.subscription.push(
      this._purgeservice.searchData.subscribe((res: any) => {
        this.searchData = res;
      })
    );
    this.subscription.push(
      this._purgeservice.isRowSelectEnabled.subscribe((res: boolean) => {
        this.isPurgeRowSelectEnabled = res;
      })
    );
    this.subscription.push(
      this._purgeservice.isRowSelectrDeselect.subscribe((res: boolean) => {
        this.isPurgeRowSelectDeselect = res;
      })
    );
    this.subscription.push(
      this._purgeservice.getExpiredloandata.subscribe((res) => {
        this.dTable.clear();
        this.dTable.rows.add(res);
        this.dTable.draw();
      })
    );
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
    this.dtOptions = {
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
        { mData: 'LoanID', sClass: 'select-checkbox', bVisible: true },
        { mData: 'LoanID', bVisible: false },
        { sTitle: 'Borrower Loan #', mData: 'LoanNumber', sWidth: '12%'},
        { sTitle: 'Loan Type', mData: 'LoanType', sWidth: '32%' },
        { sTitle: AppSettings.AuthorityLabelSingular + ' Name', mData: 'CustomerName', sWidth: '23%', },
        { sTitle: 'Borrower Name', mData: 'BorrowerName', sWidth: '23%' },
        {  sTitle: 'Status',  mData: 'Status',  sWidth: '10%',  sClass: 'text-center', },
      ],
      aoColumnDefs: [
        {
          aTargets: [6],
          mRender: function (data, type, row) {

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
          aTargets: [0],
          orderable: false,
          mRender: function (date) {
            return '';
          },
        },
      ],
    };
  }
  ngAfterViewInit() {
    this.dt.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
      if (isTruthy(this.dTable)) {
        if (isTruthy(this._commonService.AuditMonthYear)) {
          this.receivedDate.dateFrom = this._commonService.FromDate;
          this.receivedDate.dateTo = this._commonService.ToDate;
          this._purgeservice.isRowSelectrDeselect.next(true);
          this._purgeservice.isRowSelectEnabled.next(false);
          this._purgeservice.getExpiredloandata.next(this._commonService.checklistitemResult);
          this._commonService.AuditMonthYear = null;
        } else {
          this.LoanPurgeSearch(new Date(), new Date());
        }
      }
      dtInstance.on('select', (s) => {
        this.isPurgeEnabled = !(this.dTable.rows('.selected').data().length > 0);
      });

      dtInstance.on('deselect', (s) => {
        this.isPurgeEnabled = !(this.dTable.rows('.selected').data().length > 0);
        this.isPurgeRowSelectDeselect = !(this.dTable.rows('.selected').data().length > 0);
      });
    });

  }
  LoanPurgeSearch(frmDate: any, toDate: any) {
    this.fromdate = frmDate;
    this.todate = toDate;
    const fromdate = this.datePipe.transform(frmDate, AppSettings.dateFormat);
    const todate = this.datePipe.transform(toDate, AppSettings.dateFormat);
    const inputReq = {
      TableSchema: AppSettings.TenantSchema,
      FromDate: fromdate,
      ToDate: todate,
    };
    this.promise = this._purgeservice.GetExpiredLoans(inputReq);
  }

  getSelectAllPurgeRowData() {
    this.rowDatas = '';
    if (this.isPurgeRowSelectDeselect) {
      this.isPurgeRowSelectDeselect = false;
      this.rowDatas = this.dTable.rows().select();
    } else {
      this.isPurgeRowSelectDeselect = false;
      this.rowDatas = this.dTable.rows().deselect();
    }
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
    const purgeStagingData = { BatchCount: rows.length, ErrMsg: '', Status: CurrentStatus };
    const inputReq = { TableSchema: AppSettings.TenantSchema, UserName: SessionHelper.UserDetails.FirstName + '' + SessionHelper.UserDetails.LastName, LoanID: loanID, purgeStaging: purgeStagingData };
    this.promise = this._purgeservice.RetentionPurge(inputReq);
  }
  LoanPurgeSearchFromDashboard() {
    const inputReq = { TableSchema: AppSettings.TenantSchema, AuditMonthYear: this._commonService.AuditMonthYear };
    this.promise = this._purgeservice.LoanPurgeSearchFromDashboard(inputReq);
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
