import { Component, OnInit, Input, ViewChild, OnDestroy, AfterViewInit } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Subscription } from 'rxjs';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { LoanInfoService } from '../../services/loan-info.service';
import { NotificationService } from '@mts-notification';
import { convertDateTime } from '@mts-functions/convert-datetime.function';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { StipulationDetails } from '../../models/loan-header.model';
@Component({
  selector: 'mts-loan-stipulation',
  templateUrl: 'loan-stipulation.page.html',
  styleUrls: ['loan-stipulation.page.css']
})

export class LoanStipulationComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(DataTableDirective) datatableEl: DataTableDirective;
  @ViewChild('addStipulationModal') addStipulationModal: ModalDirective;
  dTable: any;
  dtOptions: any = {};
  sCategoryNames: { id: any, text: any }[] = [];
  promise: Subscription;
  LoanStipulation: { addOrEdit: any, ID: any, StipulationID: any, StipulationCategoryID: any, StipulationDescription: any, StipulationStatus: any, StipulationNotes: any, Active: any, isSaveOrUpdate: any } = {
    addOrEdit: 'Add', ID: 0, StipulationCategoryID: 0, StipulationID: 0, StipulationDescription: '', StipulationStatus: 0, StipulationNotes: '', Active: true, isSaveOrUpdate: false
  };
  rowSelected = true;
  sStatus: any = [{ id: 1, text: 'Pending' }, { id: 2, text: 'Completed' }, { id: 3, text: 'Cancelled' }];

  constructor(
    private _loanInfoService: LoanInfoService,
    public _notificationService: NotificationService) { }

  private _subscriptions: Subscription[] = [];
  private _selectedRow: any;

  ngOnInit() {

    this._subscriptions.push(this._loanInfoService.StipulationsTable$.subscribe((res: StipulationDetails[]) => {
      this.dTable.clear();
      this.dTable.rows.add(res);
      this.dTable.draw();
    }));
    this._subscriptions.push(this._loanInfoService.StipulationCategoryNames$.subscribe((res: { id: any, text: any }[]) => {
      this.sCategoryNames = res;
    }));
    this._subscriptions.push(this._loanInfoService.StipulationModal$.subscribe((res: boolean) => {
      res ? this.addStipulationModal.show() : this.addStipulationModal.hide();
    }));

    this._loanInfoService.GetStipulationList();

    this.dtOptions = {
      displayLength: 10,
      'bPaginate': false,
      'scrollY': 'calc(100vh - 520px)',
      'aaData': '',
      'select': {
        style: 'single',
        info: false
      },
      'iDisplayLength': 10,
      'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
      'aoColumns': [
        { mData: 'ID', bVisible: false },
        { sTitle: 'StipulationCategoryID', mData: 'StipulationCategoryID', bVisible: false },
        { sTitle: 'Category Name', mData: 'StipulationCategoryName', sClass: 'text-center', sWidth: '20%' },
        { sTitle: 'Description', mData: 'StipulationDescription', sClass: 'text-center', sWidth: '20%' },
        { sTitle: 'Notes', mData: 'Notes', sClass: 'text-center', sWidth: '35%' },
        { sTitle: 'Created On', mData: 'CreatedOn', sClass: 'text-center', sWidth: '15%' },
        { sTitle: 'Status', mData: 'StipulationStatus', sClass: 'text-center', sWidth: '10%' }
      ],
      'aoColumnDefs': [
        {
          'aTargets': [5],
          'mRender': function (data, type, row) {
            if (data !== null) {
              return convertDateTime(data);
            }
          }
        },
        {
          'aTargets': [6],
          'mRender': function (data, type, row) {
            if (data !== null) {
              if (data === 1) {
                return '<label class=\'label bcEllipsis ' + 'label-danger' + ' label-table\' title=\'' + 'Pending' + '\'>' + 'Pending' + '</label>';
              } else if (data === 2) {
                return '<label class=\'label bcEllipsis ' + 'label-success' + ' label-table\' title=\'' + 'Completed' + '\'>' + 'Completed' + '</label>';
              } else {
                return '<label class=\'label bcEllipsis ' + 'label-warning' + ' label-table\' title=\'' + 'Cancelled' + '\'>' + 'Cancelled' + '</label>';
              }
            }
          }

        }
      ],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        const self = this;
        $('td', row).unbind('click');
        $('td', row).bind('click', () => {
          self.getRowData(row, data);
        });
        return row;
      }
    };
  }

  ngAfterViewInit() {
    this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
      if (isTruthy(this.dTable)) {
        this.promise = this._loanInfoService.GetLoanStipulationDetails();
      }
    });
  }

  getRowData(rowIndex: Node, rowData: any): void {
    this.rowSelected = $(rowIndex).hasClass('selected');
    this._selectedRow = rowData;
  }

  AddStipulation() {
    this.LoanStipulation = {
      addOrEdit: 'Add', ID: 0, StipulationCategoryID: 0, StipulationID: 0, StipulationDescription: '', StipulationStatus: 0, StipulationNotes: '', Active: true, isSaveOrUpdate: false
    };
  }

  EditStipulation() {
    const val = JSON.parse(JSON.stringify(this._selectedRow));
    this.LoanStipulation.addOrEdit = 'Edit';
    this.LoanStipulation.isSaveOrUpdate = true;
    this._loanInfoService.StipulationModal$.next(true);
    this.LoanStipulation.StipulationID = val.ID;
    this.LoanStipulation.StipulationCategoryID = val.StipulationCategoryID;
    this.LoanStipulation.StipulationDescription = val.StipulationDescription;
    this.LoanStipulation.StipulationStatus = val.StipulationStatus;
    this.LoanStipulation.StipulationNotes = val.Notes;

  }

  SaveStipulationDetails() {
    if (!this._loanInfoService.ValidateStipulation(this.LoanStipulation)) {
      this._loanInfoService.SaveStipulation(this.LoanStipulation);
    }
  }

  UpdateStipulationDetails() {
    this.LoanStipulation.ID = this.LoanStipulation.StipulationID;
    if (!this._loanInfoService.ValidateStipulation(this.LoanStipulation)) {
      this._loanInfoService.UpdateStipulation(this.LoanStipulation);
    }
  }

  ngOnDestroy(): void {
    this._subscriptions.forEach(element => {
      element.unsubscribe();
    });
  }
}
