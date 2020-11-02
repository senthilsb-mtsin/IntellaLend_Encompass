import {
  Component,
  OnInit,
  OnDestroy,
  ViewChild,
  AfterContentChecked,
  AfterViewInit,
} from '@angular/core';
import { LoanSearchService } from '../service/loansearch.service';
import { AppSettings } from '@mts-app-setting';
import { GetLoanSearchFilterRequest } from '../models/get-loan-search-filters.request';
import { LoanSearchRequestModel } from '../models/loan-search-request.model';
import { DataTableDirective } from 'angular-datatables';
import {
  NgDateRangePickerComponent,
  NgDateRangePickerOptions,
} from '@mts-daterangepicker/ng-daterangepicker.component';
import { SelectComponent } from '@mts-select2';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { MonthYearPickerComponent } from '@mts-month-year-picker/component/MonthYearPicker/MonthYearPicker.component';
import { Subscription } from 'rxjs';
import { StatusConstant } from '@mts-status-constant';

import { IMyOptions } from '@mts-date-picker/interfaces';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { DatePipe } from '@angular/common';
import { SessionHelper } from '@mts-app-session';
import { NotificationService } from '@mts-notification';
import { DeleteLoanRequestModel } from '../models/loan-delete-request.model';
import { convertDateTime } from '@mts-functions/convert-datetime.function';
import { LoanSearchTableModel } from '../models/loan-search-table.model';
import { Router } from '@angular/router';
import { LoanInfoService } from '../../loan/services/loan-info.service';
import { EmailCheckPipe } from '@mts-pipe';

@Component({
  selector: 'mts-loan-search',
  templateUrl: 'loansearch.page.html',
  styleUrls: ['loansearch.page.css']
})
export class LoanSearchComponent
  implements OnInit, OnDestroy, AfterContentChecked, AfterViewInit {
  visibility = false;
  tableAligned = false;
  createTableInstance = false;
  loanFilterResult: any = {};
  isReceivedDate = 'none';
  Request: LoanSearchRequestModel = new LoanSearchRequestModel();
  loanSearchStatus: any = [];
  @ViewChild(DataTableDirective) datatableEl: DataTableDirective;
  @ViewChild(NgDateRangePickerComponent)
  receivedDate: NgDateRangePickerComponent;
  @ViewChild('confirmModal') confirmModal: ModalDirective;
  @ViewChild('workFlowStatusDropDown')
  _workFlowStatusDropDown: SelectComponent;
  @ViewChild('confirmPurgeModal') confirmPurgeModal: ModalDirective;
  @ViewChild('missingAuditMonthYear')
  _missingAuditMonthYear: MonthYearPickerComponent;
  @ViewChild('confirmDeleteModal') _confirmDeleteModal: ModalDirective;
  @ViewChild('auditPDFModal') auditPDFModal: ModalDirective;
  dtOptions: any = {};
  dTable: any;
  options: NgDateRangePickerOptions;
  value: any = '';
  promise: Subscription;
  AuthorityLabelSingular = AppSettings.AuthorityLabelSingular;

  TempSelectedYearDate: any = '';
  loanTypes: { id: any, text: any }[] = [];
  activeCustomerLists: { id: any, text: any }[] = [];
  reviewTypes: { ReviewTypeID: any, ReviewTypeName: any }[] = [];
  workFlowMaster: { id: any, text: any }[] = [];
  isDeleteLoan = true;
  selectAllBtn = true;

  myDatePickerOptions: IMyOptions = {
    dateFormat: 'mm/dd/yyyy',
    showClearDateBtn: false,
    editableDateField: false,
  };
  _AuditDueDate: any = {
    date: {
      year: new Date().getFullYear(),
      month: new Date().getMonth() + 1,
      day: new Date().getDate(),
    },
  };

  constructor(
    private _loanSearchService: LoanSearchService,
    private datePipe: DatePipe,
    private _notificationService: NotificationService,
    private _loanService: LoanInfoService,
    private _emailPipe: EmailCheckPipe,
    private _route: Router
  ) { }

  private subscribtion: Subscription[] = [];
  private _subscriptionCreated = false;

  ngAfterContentChecked() {
    if (isTruthy(this.dTable) && !this.tableAligned) {
      $('.dataTables_info').addClass('col-md-6 p0');
      $('.dataTables_paginate').addClass('col-md-6 p0');
      $('.dataTables_filter').addClass('col-md-6 p0');
      $('.dataTables_length').addClass('col-md-6 p0');

      $('.dt-buttons').appendTo('#downloadButton');
      this.tableAligned = true;
    }
  }

  ngAfterViewInit() {

    this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
      this.createTableInstance = true;
    });
  }

  ngOnInit() {
    this.GetLoanSearchFilterConfigValue();

    this.options = {
      theme: 'default',
      previousIsDisable: false,
      nextIsDisable: false,
      range: 'em',
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
      dateFormat: 'M/d/yyyy',
      outputFormat: 'DD/MM/YYYY',
      startOfWeek: 0,
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
      displayLength: 10,
      dom: 'Blfrtip',
      buttons: [
        {
          extend: 'excel',
          className: 'btn btn-sm btn-info waves-effect waves-light m10',
          text: '<i class="fa fa-file-excel-o"></i> Download',
          filename: 'Loan Search',
          exportOptions: {
            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9],
          },
          title: 'Loan Search',
        },
      ],
      aaData: [],
      select: {
        style: 'multi',
        info: false,
        selector: 'td:first-child',
      },
      iDisplayLength: 10,
      aLengthMenu: [
        [5, 10, 25, 50, -1],
        [5, 10, 25, 50, 'All'],
      ],
      aoColumns: [
        { mData: 'LoanID', sClass: 'select-checkbox', bVisible: true },
        { sTitle: AppSettings.AuthorityLabelSingular + '', mData: 'Customer' },
        {
          sTitle: 'Received Date',
          type: 'date',
          mData: 'ReceivedDate',
          sClass: 'text-center',
        },
        { sTitle: 'Borrower Name', mData: 'BorrowerName' },
        { sTitle: 'Borrower Loan #', mData: 'LoanNumber', sClass: 'text-left' },
        { sTitle: 'IDC Batch', mData: 'EphesoftBatchInstanceID' },
        { sTitle: 'Loan Type', mData: 'LoanTypeName' },
        { sTitle: 'Loan Amount($)', mData: 'LoanAmount', sClass: 'text-right' },
        {
          sTitle: 'Loan Status',
          mData: 'StatusDescription',
          sClass: 'text-center',
        },
        {
          sTitle: 'Assigned User',
          mData: 'AssignedUser',
          sClass: 'text-center',
        },
        {
          sTitle: 'Assigned User ID',
          mData: 'AssignedUserID',
          bVisible: false,
        },
        { sTitle: 'View', mData: 'LoanID', sClass: 'text-center' },
        { mData: 'Status', bVisible: false },
        { mData: 'CurrentUserID', bVisible: false },
        { mData: 'ServiceTypeName', bVisible: false, sTitle: 'Service Type' },
        {
          mData: 'AuditMonthYear',
          bVisible: false,
          sTitle: 'Audit Month & Year',
        },
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
          aTargets: [7],
          orderable: false,
          mRender: function (data) {
            return data.toLocaleString();
          },
        },
        {
          aTargets: [8],
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
          aTargets: [6],
          mRender: function (data, type, row) {
            if (row['LoanID'] === 50) {
              return 'Post-Close Conventional Purchase';
            } else {
              return data;
            }
          },
        },
        {
          aTargets: [11],
          mRender: function (data, type, row) {
            if (
              row['Status'] === StatusConstant.COMPLETE ||
              row['Status'] === StatusConstant.PENDING_AUDIT
            ) {
              return '<span style=\'cursor: pointer;\' title=\'Open Loan\' class=\'viewLoan material-icons txt-info\'>pageview</span>';
            } else {
              return '';
            }
          },
        },
      ],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        const self = this;

        $('td .viewLoan', row).unbind('click');
        $('td .viewLoan', row).bind('click', () => {
          self.viewLoan(row, data);
        });
        $('td:first-child', row).unbind('click');
        $('td:first-child', row).bind('click', () => {
          self.RowSelect(row, data);
        });

        return row;
      },
    };

    this.subscribtion.push(this._loanSearchService.loanFilterResult.subscribe((res) => {
      this.loanFilterResult = res;
      if (this.loanFilterResult.ReceivedDate) {
        this.isReceivedDate = 'block';
      } else {
        this.isReceivedDate = 'none';
      }

      if (!this._subscriptionCreated)
        this.createSubscription();
    }));

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

    this.subscribtion.push(this._loanSearchService.searchData.subscribe((res: LoanSearchTableModel[]) => {
      this.dTable.clear();
      this.dTable.rows.add(res);
      this.dTable.draw();
      this.dTable.columns.adjust();
      this.visibility = false;
    }));

    this.subscribtion.push(this._loanSearchService.confimModelHide.subscribe((res: boolean) => {
      this._confirmDeleteModal.hide();
    }));

    this.SearchValidate('', '');
  }

  createSubscription() {
    this._subscriptionCreated = true;
    this._loanSearchService.GetLoanTypeMaster();
    this._loanSearchService.GetCustomerMaster();
    this._loanSearchService.GeWorkFlowMaster();
    this._loanSearchService.GetReviewTypeMaster();
  }

  viewLoan(row: Node, rowData: any): void {
    if (isTruthy(rowData)) {
      this._loanService.SetLoanPageInfo(rowData);
      this._route.navigate(['view/loandetails']);
    } else {
      this._notificationService.showError('Row Not Fetched');
    }
  }

  RowSelect(rowIndex: Node, rowData: any) {
    if (rowData.Status !== StatusConstant.DELETE_LOAN) {
      this.isDeleteLoan = $(rowIndex).hasClass('selected');
    }
  }

  getMonthYear(event: any) {
    this.Request.AuditMonthYear = this.datePipe.transform(
      event.Value,
      AppSettings.dateFormat
    );
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
          this.isDeleteLoan = true;
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

  DeleteLoans() {
    const multiRowData = this.dTable.rows('.selected').data();
    const loanLists = [];

    for (let i = 0; i < multiRowData.length; i++) {
      if (multiRowData[i].Status !== StatusConstant.DELETE_LOAN) {
        loanLists.push(multiRowData[i].LoanID);
      }
    }

    const req = new DeleteLoanRequestModel(
      AppSettings.TenantSchema,
      loanLists,
      SessionHelper.UserDetails.LastName + SessionHelper.UserDetails.FirstName
    );

    this._loanSearchService.DeleteLoans(req);
  }

  reset() {
    this.Request = new LoanSearchRequestModel();
    this._AuditDueDate = {
      date: {
        year: new Date().getFullYear(),
        month: new Date().getMonth() + 1,
        day: new Date().getDate(),
      },
    };
    this.receivedDate.dateFrom = null;
    this.receivedDate.dateTo = null;
    this.receivedDate.selectRange('em');
    this.dTable.clear().draw();
    this._missingAuditMonthYear.setValue('');
  }

  SearchValidate(reveivedFrom: any, reveivedTo: any) {
    if (
      this.Request.LoanNumber !== '' ||
      this.Request.LoanType !== 0 ||
      (isTruthy(this.receivedDate) && (this.receivedDate.dateFrom !== null ||
        this.receivedDate.dateTo !== null)) ||
      this.Request.BorrowerName !== '' ||
      this.Request.LoanAmount !== '' ||
      this.Request.ReviewStatus !== 0 ||
      this.Request.Location !== 0 ||
      this.Request.ReviewType !== 0 ||
      this.Request.PropertyAddress !== '' ||
      this.Request.InvestorLoanNumber !== '' ||
      this.Request.PostCloser !== '' ||
      this.Request.LoanOfficer !== '' ||
      this.Request.UnderWriter !== '' ||
      this._AuditDueDate !== '' ||
      (this.Request.AuditMonthYear !== null &&
        this.Request.AuditMonthYear !== '')
    ) {
      this.Request['AuditDueDate'] =
        this._AuditDueDate === null ? null : this._AuditDueDate.formatted;
      if (reveivedFrom !== '' && reveivedTo !== '') {
        const fromDate = this.datePipe.transform(
          reveivedFrom,
          AppSettings.dateFormat
        );
        const toDate = this.datePipe.transform(
          reveivedTo,
          AppSettings.dateFormat
        );
        this.Request['FromDate'] = fromDate;
        this.Request['ToDate'] = toDate;
      } else {
        this.Request['FromDate'] = reveivedFrom;
        this.Request['ToDate'] = reveivedTo;
      }
      this.setLoanStatusValue();
      this.searchSubmit(this.Request);
    } else {
      this._notificationService.showError(
        'Atleast one field value is required'
      );
    }
  }

  searchSubmit(inputReq: any) {
    inputReq['TableSchema'] = AppSettings.TenantSchema;
    inputReq['CurrentUserID'] = SessionHelper.UserDetails.UserID;
    this.promise = this._loanSearchService.searchSubmit(inputReq);
  }

  setLoanStatusValue() {
    if (this.loanSearchStatus.length > 0) {
      const loanStatus = [];
      this.loanSearchStatus.forEach((element) => {
        loanStatus.push(element.id);
      });
      this.Request.SelectedLoanStatus = loanStatus;
    } else {
      this.Request.SelectedLoanStatus = [];
    }
  }

  GetLoanSearchFilterConfigValue() {
    const req = new GetLoanSearchFilterRequest(
      AppSettings.TenantSchema,
      0,
      'Search_Filter'
    );
    this._loanSearchService.GetLoanSearchFilterConfigValue(req);
  }

  ngOnDestroy() {
    this.subscribtion.forEach((element) => {
      element.unsubscribe();
    });
  }
}
