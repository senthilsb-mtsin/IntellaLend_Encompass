import { Component, ViewChild, OnInit, OnDestroy, AfterViewInit, ViewChildren, QueryList } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { SessionHelper } from '@mts-app-session';
import { AppSettings } from '@mts-app-setting';
import { LoanTypeService } from '../../service/loantype.service';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { SynchronizeConstant } from '@mts-status-constant';
import { Subscription } from 'rxjs';
import { LoanTypeDatatableModel } from '../../models/loantype-datatable.model';
import { SyncCustomerRequest } from '../../models/sync-customer-request.model';
import { SyncDetailsRequest } from '../../models/sync-details-request.model';

@Component({
  selector: 'mts-loan-type',
  templateUrl: 'loantype.page.html',
  styleUrls: ['loantype.page.css'],
})
export class LoanTypeComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild('confirmModal') confirmModal: ModalDirective;
  @ViewChild('confirmdataModal') confirmdataModal: ModalDirective;

  dtOptions: any = {};
 @ViewChildren(DataTableDirective) dt: QueryList<DataTableDirective>;
  StagingdTable: any;
  StagingDetailsdTable: any;

  showHide: any = [false, false, false];
  rowSelected = true;
  LoanTypeName = '';
  promise: Subscription;
  SyncType: any = {};
  SyncRowData: any;
  Row: any;
  data: any = {};
  SyncDetailsOptions: any = {};
  AuthorityLabelPlural: string = AppSettings.AuthorityLabelPlural;
  constructor(
    private _loanService: LoanTypeService,
    private _route: Router
  ) {
    this.checkPermission('AddBtn', 0);
    this.checkPermission('EditBtn', 1);
    this.checkPermission('ViewBtn', 2);
  }

  private LoanTypeID: number;
  private subscriptions: Subscription[] = [];
  private LoanTypeActive: boolean;

  ngAfterViewInit() {
    this.dt.forEach((dtElement: DataTableDirective) => {
      dtElement.dtInstance.then((dtInstance: any) => {
        if (dtInstance.context[0].sTableId === 'StagingdTable') {
          this.StagingdTable = dtInstance;
          this.getLoanTypeList();
        } else if (dtInstance.context[0].sTableId === 'StagingViewDTble') {
          this.StagingDetailsdTable = dtInstance;
        }
      });
    });

  }

  ngOnInit() {

    this.subscriptions.push(this._loanService.setLoanMasterTableData.subscribe((res: LoanTypeDatatableModel[]) => {
      this.StagingdTable.clear();
      this.StagingdTable.rows.add(res);
      this.StagingdTable.draw();
    }));
    this.subscriptions.push(this._loanService.syncModalShow.subscribe((res: boolean) => {
     this.confirmModal.show();
    }));
    this.subscriptions.push(this._loanService.setSyncDetailsTableData.subscribe((res: any) => {
      this.StagingDetailsdTable.clear();
      this.StagingDetailsdTable.rows.add(res);
      this.StagingDetailsdTable.draw();
      this.confirmdataModal.show();
 }));

    this.dtOptions = {
      aaData: [],
      'select': {
        style: 'single',
        info: false
      },
      'iDisplayLength': 10,
      'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
      aoColumns: [
        { sTitle: 'LoanTypeID', mData: 'LoanTypeID', sClass: 'text-right', bVisible: false },
        { sTitle: 'Loan Type Name', mData: 'LoanTypeName', sWidth: '70%' },
        { sTitle: 'Active/Inactive', mData: 'Active', sClass: 'text-center', sWidth: '10%' },
        { sTitle: 'Sync', mData: 'SyncStatusID', sClass: 'text-center', sWidth: '20%' },
        { sTitle: 'View Sync Details', mData: 'LoanTypeID', sClass: 'text-center', sWidth: '20%' }

      ],
      aoColumnDefs: [
        {
          'aTargets': [2],
          'mRender': function (data, type, row) {
            const statusFlag = data === true ? 'Active' : 'Inactive';
            const statusColor = {
              'true': 'label-success',
              'false': 'label-danger'
            };

            return '<label class=\'label ' + statusColor[row['Active']] + ' label-table\'' + '>' + statusFlag + '</label></td>';
          }
        }, {
          'aTargets': [3],
          'mRender': function (data, type, row) {
            const DESCRIPTION = SynchronizeConstant.SYNCHRONIZE_DESCRIPTION[data];
            const COLOR = SynchronizeConstant.SYNCHRONIZE_COLOR[data];
            if (data === SynchronizeConstant.Completed || data === SynchronizeConstant.Failed || data === SynchronizeConstant.DefaultVal) {
              if (data !== SynchronizeConstant.Failed) {
                  return '<div class="btn-group" style="width: 90%;"><button type="button" style="width: 50%;" class="btn btn-info btn-sm waves-effect waves-light SyncAll waves-effect waves-light">Sync All</button> <button style="width: 50%;"  type="button" class="btn btn-info btn-sm waves-effect waves-light SyncChecklist waves-effect waves-light">Sync Checklist</button></div>';
              } else {
                  return '<div class="btn-group" style="width: 90%;" ><button type="button" style="width: 24%;"  class="btn btn-info btn-sm waves-effect waves-light SyncRetry waves-effect waves-light">Retry</button><button type="button" style="width: 26%;"  class="btn btn-info btn-sm waves-effect waves-light SyncAll btn-sm waves-effect waves-light">Sync All</button><button type="button" style="width: 50%;"  class="btn btn-info btn-sm waves-effect waves-light SyncChecklist bg-info-custom-left btn-sm waves-effect waves-light">Sync Checklist</button></div>';

              }

          } else {
              return '<label style=\'width: 90%;\' title = ' + DESCRIPTION + ' class=\'label ' + COLOR + ' label-table\'>' + DESCRIPTION + '</label>';
          }
          }
        },
        {
          'aTargets': [4],
          'mRender': function (data, type, row) {
              if (row['SyncStatusID'] === SynchronizeConstant.Failed) {
                  return '<span style=\'cursor:pointer\' title=\'Synchronize Failed\' class=\'SyncDetail material-icons txt-red\'>report_problem</span>';

              } else if (row['SyncStatusID'] === SynchronizeConstant.Process) {
                  return '<span style=\'cursor:pointer\' title=\'Synchronize Processing\' class=\'SyncDetail material-icons txt-orange\'>report_problem</span>';

               } else if (row['SyncStatusID'] === SynchronizeConstant.Completed) {
                   return '<span style=\'cursor:pointer\' title=\'Synchronize Completed\' class=\'SyncDetail material-icons txt-info\'>pageview</span>';
                } else {
                  return '';
              }
          }
      }

      ],
      rowCallback: (row: Node, data: LoanTypeDatatableModel, index: number) => {

        const self = this;
        $('td .SyncAll', row).unbind('click');
        $('td .SyncAll', row).bind('click', () => {
            self.getRowDataForSync(row, data, SynchronizeConstant.AllSync);
        });

        $('td .SyncRetry', row).unbind('click');
        $('td .SyncRetry', row).bind('click', () => {
            self.getRowDataForSync(row, data, SynchronizeConstant.RetrySync);
        });

        $('td .SyncChecklist', row).unbind('click');
        $('td .SyncChecklist', row).bind('click', () => {
            self.getRowDataForSync(row, data, SynchronizeConstant.ChecklistSync);
        });

        $('td', row).unbind('click');
        $('td', row).bind('click', () => {
          self.getRowData(row, data);
        });
        $('td .SyncDetail', row).unbind('click');
        $('td .SyncDetail', row).bind('click', () => {
            self.GetRowSyncdata(row, data);
        });

    return row;
      }
    };
    this.SyncDetailsOptions = {
      displayLength: 5,
      aaData: this.data,
      'select': {
          style: 'single',
          info: false
      },
      'iDisplayLength': 5,
      'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
      aoColumns: [
          { sTitle: 'ID', mData: 'ID', sClass: 'text-center', bVisible: false },
          { sTitle: AppSettings.AuthorityLabelSingular + ' Name', mData: 'CustomerName', sClass: 'text-left', sWidth: '10%' },
          { sTitle: 'Service Type Name', mData: 'ReviewTypeName', sClass: 'text-left' },
          { sTitle: 'Deal Type Name', mData: 'LoanTypeName', sClass: 'text-left' },
          { sTitle: 'Status', mData: 'Status', sClass: 'text-center' },
          { sTitle: 'Synced Date', mData: 'ModifiedOn', sClass: 'text-center' },
          { sTitle: 'ErrorMsg', mData: 'ErrorMsg', sClass: 'text-center' },

      ],
      aoColumnDefs: [
      {
          'aTargets': [4],
          'mRender': function (data, type, row) {
              const DESCRIPTION = SynchronizeConstant.SYNCHRONIZE_DESCRIPTION[data];
              const COLOR = SynchronizeConstant.SYNCHRONIZE_COLOR[data];
                  return '<label title = ' + DESCRIPTION + ' class=\'label ' + COLOR + ' label-table\'>' + DESCRIPTION + '</label>';

          }
      },
  ]

};
  }
  GetRowSyncdata(rowIndex: Node, Data: any) {
       this.Row = rowIndex;
       this.SyncRowData = Data;
       this.GetSyncLoanDetails(Data.LoanTypeID);

    }
    Close() {
        this.confirmdataModal.hide();
        this.getLoanTypeList();
    }
    GetSyncLoanDetails(LoanTypeID) {
      const inputData: any = { TableSchema: AppSettings.TenantSchema, LoanTypeID: LoanTypeID };
      this._loanService.GetSyncDetails(inputData);

    }
   getRowDataForSync(rowIndex: Node, rowData: LoanTypeDatatableModel, SyncType: any): void {
     this.LoanTypeName = rowData.LoanTypeName;
     this.LoanTypeID = rowData.LoanTypeID;
     this.LoanTypeActive = rowData.Active;
     this.SyncType = SyncType;
     this.confirmModal.show();
  }

  getLoanTypeList() {
    this.promise = this._loanService.getLoanTypeList();
  }
  closeSync() {
    this.confirmModal.hide();
    this.rowSelected = false;
  }
  SyncLoanType() {
    this.confirmModal.hide();
    const req = new SyncCustomerRequest(AppSettings.TenantSchema, this.LoanTypeID, SessionHelper.UserDetails.UserID, this.SyncType);
    this._loanService.SyncLoanType(req);
  }

  getRowData(rowIndex: any, rowData: LoanTypeDatatableModel): void {
    this.LoanTypeName = rowData.LoanTypeName;
    this.LoanTypeID = rowData.LoanTypeID;
    this.LoanTypeActive = rowData.Active;
    this.rowSelected = $(rowIndex).hasClass('selected');
  }

  checkPermission(component: string, index: number): void {
    const URL = 'View\\LoanType\\' + component;
    const AccessCheck = false;
    const AccessUrls = SessionHelper.RoleDetails.URLs;
    if (AccessCheck !== null) {
      AccessUrls.forEach((element) => {
        if (element.URL === URL) {
          this.showHide[index] = true;
          return false;
        }
      });
    }
  }

  ShowLoanTypeModal(modalType: number) {
    this._loanService.clearLoanType();
    if (modalType === 0) {
      this._route.navigate(['view/loantype/addloantype']);
    } else if (modalType === 1) {
      this._loanService.setLoanType({ Type: 'Edit', LoanTypeID: this.LoanTypeID, LoanTypeName: this.LoanTypeName, Active: this.LoanTypeActive });
      this._route.navigate(['view/loantype/editloantype']);
    }
    // else if (modalType === 2) {
    //   this._loanService.setLoanType({ Type: 'View', LoanTypeID: this.LoanTypeID, LoanTypeName: this.LoanTypeName, Active: this.LoanTypeActive });
    //   this._route.navigate(['view/loantype/viewloantype']);
    // }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(element => {
      element.unsubscribe();
    });
  }
}
