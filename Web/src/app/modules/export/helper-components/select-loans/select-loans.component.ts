import { MonthYearPickerComponent } from '@mts-month-year-picker/component/MonthYearPicker/MonthYearPicker.component';
import { SessionHelper } from './../../../../shared/model/session-models/app-session';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs/internal/Subscription';
import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { LoanSearchRequestModel } from 'src/app/modules/loansearch/models/loan-search-request.model';
import { LoanSearchService } from 'src/app/modules/loansearch/service/loansearch.service';
import { AppSettings } from '@mts-app-setting';
import { convertDateTime } from '@mts-functions/convert-datetime.function';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { StatusConstant } from '@mts-status-constant';
import { LoanImportService } from 'src/app/modules/loan-import/services/loan-import.service';
import { DatePipe } from '@angular/common';
import { NgDateRangePickerComponent } from '@mts-daterangepicker/ng-daterangepicker.component';
import { NotificationService } from '@mts-notification';
import { DataTableDirective } from 'angular-datatables';
import { ExportLoanService } from '../../service/export-loan.service';
import { LoanSearchDataAccess } from 'src/app/modules/loansearch/loansearch.data';
import { LoanImportDataAccess } from 'src/app/modules/loan-import/loan-import.data';
import { ExportService } from '../../service/export.service';
@Component({
  selector: 'mts-select-loan',
  templateUrl: './select-loans.component.html',
  styleUrls: ['./select-loans.component.css'],
  providers: [LoanSearchService, LoanImportService, LoanSearchDataAccess, LoanImportDataAccess]
})
export class SelectLoansComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(DataTableDirective) datatable: DataTableDirective;
  dTable: any;
  visibility = false;
  Request = new LoanSearchRequestModel();
  loanFilterResult: any;
  loanTypes: { id: any, text: any }[] = [];
  activeCustomerLists: { id: any, text: any }[] = [];
  reviewTypes: { ReviewTypeID: any, ReviewTypeName: any }[] = [];
  workFlowMaster: { id: any, text: any }[] = [];
  options: any;
  dtOptions: any;
  rowData: any;
  TempSelectedYearDate: any = '';
  AuthorityLabelSingular = AppSettings.AuthorityLabelSingular;
  @ViewChild('missingAuditMonthYear') _missingAuditMonthYear: MonthYearPickerComponent;
  @ViewChild(NgDateRangePickerComponent) receivedDate: NgDateRangePickerComponent;
  selectAllBtn = true;
  userDetails: any;
  constructor(private _loanSearchService: LoanSearchService
    , private _loanImportService: LoanImportService,
    private datePipe: DatePipe,
    private _notificationservice: NotificationService,
    private _exportloanservice: ExportLoanService
    , private _route: Router
    , private _exportservice: ExportService) {
    this.userDetails = SessionHelper.UserDetails;
  }
  private subscribtion: Subscription[] = [];
  ngOnInit(): void {
    this.options = {
      theme: 'default',
      previousIsDisable: false,
      nextIsDisable: false,
      range: 'em',
      dayNames: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
      presetNames: ['Today', 'This Month', 'Last Month', 'This Week', 'Last Week', 'This Year', 'Last Year'],
      dateFormat: 'M/d/y',
      outputFormat: 'DD/MM/YYYY',
      startOfWeek: 0,
      display: { to: 'block', tm: 'block', lm: 'block', lw: 'block', tw: 'block', ty: 'block', ly: 'block', custom: 'block', em: 'block' }
    };

    this.dtOptions = {
      'select': {
        style: 'multi',
        info: false,
        selector: 'td:first-child'
      },
      'iDisplayLength': 10,
      'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
      aoColumns: [
        { mData: 'LoanID', sClass: 'select-checkbox', bVisible: true, bSortable: false },
        { sTitle: AppSettings.AuthorityLabelSingular + '', mData: 'Customer' },
        { sTitle: 'Received Date', 'type': 'date', mData: 'ReceivedDate', sClass: 'text-center' },
        { sTitle: 'Borrower Name', mData: 'BorrowerName'  },
        { sTitle: 'Borrower Loan #', mData: 'LoanNumber', sClass: 'text-center' },
        { sTitle: 'Loan Type', mData: 'LoanTypeName' },
        { sTitle: 'Loan Amount', mData: 'LoanAmount', sClass: 'text-right' },
        { sTitle: 'Loan Status', mData: 'StatusDescription', sClass: 'text-center' },
        { sTitle: 'View', mData: 'LoanID', sClass: 'text-center' },
        { mData: 'Status', bVisible: false },
        { sTitle: 'Service Type', mData: 'ServiceTypeName', bVisible: false },
        { sTitle: 'Audit Month & Year', mData: 'AuditMonthYear', bVisible: false, sClass: 'text-center' },
        { sTitle: 'LoanTypeID', mData: 'LoanTypeID', bVisible: false }
      ],
      aoColumnDefs: [
        {
          'aTargets': [0],
          'orderable': false,
          'mRender': function (data) {
            return '';
          }
        },
        {
          'aTargets': [2],
          'mRender': function (date) {
            return isTruthy(date) ? convertDateTime(date) : date;
          }
        },
        {
          'aTargets': [7],
          'mRender': function (data, type, row) {
            return '<label class=\'label ' + StatusConstant.STATUS_COLOR[row['Status']] + ' label-table\'>' + data + '</label>';
          }
        },
        {
          'aTargets': [5],
          'mRender': function (data, type, row) {
            return row['LoanID'] === 50 ? 'Post-Close Conventional Purchase' : data;
          }
        },
        {
          'aTargets': [8],
          'mRender': function (data, type, row) {

            if (row['Status'] === StatusConstant.COMPLETE || row['Status'] === StatusConstant.PENDING_AUDIT) {
              return '<span style=\'cursor: pointer;\' class=\'viewLoan material-icons txt-info\'>pageview</span>';
            } else {
              return '';
            }
          }
        }
      ],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        const self = this;
        $('td .viewLoan', row).unbind('click');
        $('td .viewLoan', row).bind('click', () => {
          self.getRowData(row, data);
        });
        $('td:first-child', row).unbind('click');
        $('td:first-child', row).bind('click', () => {
          self.RowSelect(row, data);
        });
        return row;
      }
    };
    this.subscribtion.push(this._loanSearchService.loanTypeMaster.subscribe((res: { id: any, text: any }[]) => {
      this.loanTypes = [];
      this.loanTypes.push({ id: 0, text: '--Select Loan Type--' });
      res.forEach(element => {
        this.loanTypes.push({ id: element.id, text: element.text });
      });
    }));

    this.subscribtion.push(this._loanSearchService.customerMaster.subscribe((res: { id: any, text: any }[]) => {
      this.activeCustomerLists = [];
      res.forEach(element => {
        this.activeCustomerLists.push({ id: element.id, text: element.text });
      });
    }));

    this.subscribtion.push(this._loanSearchService.workFlowMaster.subscribe((res: { id: any, text: any }[]) => {
      this.workFlowMaster = [];
      this.workFlowMaster.push({ id: 0, text: '--Select Loan Status--' });
      res.forEach(element => {
        this.workFlowMaster.push({ id: element.id, text: element.text });
      });
    }));

    this.subscribtion.push(this._loanSearchService.reviewTypeMaster.subscribe((res: { ReviewTypeID: any, ReviewTypeName: any }[]) => {
      this.reviewTypes = [];
      this.reviewTypes.push({ ReviewTypeID: 0, ReviewTypeName: '--Select Service Type--' });
      res.forEach(element => {
        this.reviewTypes.push({ ReviewTypeID: element.ReviewTypeID, ReviewTypeName: element.ReviewTypeName });
      });
    }));
    this.subscribtion.push(this._exportservice.AddbatchCustomer$.subscribe((res: any) => {
      this._exportloanservice.CustomerID = res.id;
    }));
    this.subscribtion.push(this._exportloanservice.searchData$.subscribe((res) => {
      this.dTable.clear();
      this.dTable.rows.add(res);
      this.dTable.draw();
      this.SelectLoanList();
      this.visibility = false;
    }));
    this._loanSearchService.GetLoanTypeMaster();
    this._loanSearchService.GetCustomerMaster();
    this._loanSearchService.GeWorkFlowMaster();
    this._loanSearchService.GetReviewTypeMaster();
  }
  RowSelect(rowIndex: Node, rowData: any) {
    this.rowData = rowData;
    setTimeout(() => { this.SetSelectedLoanData(); }, 50);

  }
  getRowData(row: Node, rowData: any): void {
    if (isTruthy(rowData)) {
      this._loanImportService.setDataAndRoute(rowData);
    }
  }
  Search(reveivedFrom: any, reveivedTo: any) {
    if (this.Request.LoanNumber !== '' || this.Request.LoanType !== 0 || this.receivedDate.dateFrom !== null || this.receivedDate.dateTo !== null || this.Request.BorrowerName !== '' || this.Request.LoanAmount !== '' || this.Request.ReviewStatus !== 0 || this.Request.Location !== 0 || this.Request.ReviewType !== 0 || (this.Request.AuditMonthYear !== null && this.Request.AuditMonthYear !== '' || this.Request.PropertyAddress !== '' || this.Request.InvestorLoanNumber !== '')) {
      if (reveivedFrom !== '' && reveivedTo !== '') {
        const fromDate = this.datePipe.transform(reveivedFrom, AppSettings.dateFormat);
        const toDate = this.datePipe.transform(reveivedTo, AppSettings.dateFormat);
        this.Request['FromDate'] = fromDate;
        this.Request['ToDate'] = toDate;
      } else {
        this.Request['FromDate'] = reveivedFrom;
        this.Request['ToDate'] = reveivedTo;
      }
      // need to check
      // this.getSearchValues();
      this.searchSubmit(this.Request);
    } else {
      this._notificationservice.showError('Atleast one field value is required');
    }
  }
  SetSelectedLoanData() {
    const data = this.dTable.rows('.selected').data().toArray();
    isTruthy(data) && data.length > 0 ? this._exportloanservice.SetSelectedLoanData(data) : this._exportloanservice.SetSelectedLoanData([]);
  }
  searchSubmit(inputReq: any) {
    inputReq['TableSchema'] = AppSettings.TenantSchema;
    inputReq['CurrentUserID'] = this.userDetails.UserID;
    inputReq['Customer'] = this._exportloanservice.CustomerID;
    this._exportloanservice.searchSubmit(inputReq);
  }
  getMonthYear(val) {
    this.Request.AuditMonthYear = this.datePipe.transform(val.Value, AppSettings.dateFormat);
  }
  reset() {
    this.Request = new LoanSearchRequestModel();
    this.receivedDate.dateFrom = null;
    this.receivedDate.dateTo = null;
    this.receivedDate.selectRange('em');
    this.dTable.clear().draw();
    this._missingAuditMonthYear.setValue('');
    this.Search('', '');
  }
  SelectAll() {
    if (this.selectAllBtn) {
      this.dTable.rows().select();
      this.selectAllBtn = false;
    } else {
      this.dTable.rows().deselect();
      this.selectAllBtn = true;
    }
    this.SetSelectedLoanData();
  }
  ngAfterViewInit() {
    this.datatable.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
      if (this.dTable) {
        this.searchSubmit(this.Request);
      }
    });
  }
  ngOnDestroy() {
    this.subscribtion.forEach((element) => {
      element.unsubscribe();
    });
  }
  checkselectAll() {
    if (this.dTable.rows().data().toArray().length === this.dTable.rows('.selected').data().toArray().length) {
      this.selectAllBtn = false;
    }
  }
  SelectLoanList() {
    if (this._exportloanservice.LoanDetail.length > 0) {
      const rows = this.dTable.rows().data();
      const loan = this._exportloanservice.LoanDetail;
      for (let j = 0; j < loan.length; j++) {
        for (let i = 0; i < rows.length; i++) {
          if (rows[i].LoanID === loan[j].LoanID) {
            this.dTable.rows(i).select();
          }
        }
      }
      setTimeout(() => {
        this.checkselectAll();
      }, 20);
    }
  }
}
