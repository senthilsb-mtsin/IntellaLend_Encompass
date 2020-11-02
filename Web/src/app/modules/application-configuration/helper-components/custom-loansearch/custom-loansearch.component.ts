
import { Subscription } from 'rxjs';

import {
  Component,
  OnInit,
  ViewChild,
  AfterViewInit,
  OnDestroy,
} from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppSettings } from '@mts-app-setting';
import { CustomLoansearchService } from '../../service/custom-loan-search.service';
import { ConfigTypeRequestModel } from '../../models/config-value.model';
import { UpdateLoanSearchModel } from '../../models/custom-loan-search.model';
@Component({
  selector: 'mts-custom-loansearch',
  templateUrl: './custom-loansearch.component.html',
  styleUrls: ['./custom-loansearch.component.css'],
  providers: [CustomLoansearchService],
})
export class CustomLoansearchComponent
  implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(DataTableDirective) loansearchtable: DataTableDirective;

  @ViewChild('loanSearchFieldsModal') _loanSearchFieldsModal: ModalDirective;
  dtOptions: any;
  dTable: DataTables.Api;
  Items: any = [];
  label: string;
  promise: Subscription;
  constructor(
    private _loanservice: CustomLoansearchService,
  ) { }
  private subscription: Subscription[] = [];
  ngOnInit() {
    this.dtOptions = {
      iDisplayLength: 10,
      aLengthMenu: [
        [5, 10, 25, 50, -1],
        [5, 10, 25, 50, 'All'],
      ],
      aoColumns: [
        { sTitle: 'Loan Search Field ', mData: 'ConfigValue', sWidth: '40%' },
        {
          sTitle: 'Active/Inactive',
          mData: 'Active',
          sClass: 'text-center',
          sWidth: '10%',
        },
        { sTitle: 'Edit', mData: '', sClass: 'text-center', sWidth: '10%' },
      ],
      aoColumnDefs: [
        {
          aTargets: [1],
          mRender: function (data, type, row) {
            //
            let statusFlag = '';
            if (data === true) {
              statusFlag = 'Active';
            } else {
              statusFlag = 'Inactive';
            }
            const statusColor = {
              true: 'label-success',
              false: 'label-danger',
            };

            return (
              '<label class=\'label ' +
              statusColor[row['Active']] +
              ' label-table\'>' +
              statusFlag +
              '</label></td>'
            );
          },
        },
        {
          aTargets: [2],
          mRender: function (a, row, values) {
            if (values !== null && values !== '') {
              return '<span style=\'cursor:pointer;font-size: 15px;\' class=\'edit-config material-icons txt-info\'>border_color</span>';
            }
          },
        },
      ],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        const self = this;

        $('td .edit-config', row).unbind('click');
        $('td .edit-config', row).bind('click', () => {
          self.getRowData(row, data);
        });
        return row;
      },
    };
    this.subscription.push(
      this._loanservice.searchtabledata$.subscribe((res: any) => {
        this.dTable.clear();
        this.dTable.rows.add(res);
        this.dTable.draw();
      })
    );
    this.subscription.push(
      this._loanservice.isupdated$.subscribe((res: any) => {
        this.Items = [];
        this._loanSearchFieldsModal.hide();
        this.GetLoanSearchFilterConfigValue();
      })
    );
  }
  ngAfterViewInit() {
    this.loansearchtable.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
      this.GetLoanSearchFilterConfigValue();
    });
  }
  changeMode() {
    this.Items.Active = !this.Items.Active;
  }
  modalHide(items) {
    this._loanSearchFieldsModal.hide();
    this.Items = [];

    if (items.Active) {
      items.Active = false;
    } else {
      items.Active = true;
    }
  }
  getRowData(rowIndex: Node, rowData: any): void {
    this.Items = [];
    this.Items = rowData;
    this.label = 'Edit Loan Search Fields';
    this._loanSearchFieldsModal.show();
  }
  GetLoanSearchFilterConfigValue(): void {
    const inputs = new ConfigTypeRequestModel(
      0,
      'Search_Filter',
      AppSettings.TenantSchema
    );

    this.promise = this._loanservice.GetLoanSearchFilterConfigValue(inputs);
  }
  UpdateLoanSearchFilterStatus(configID, status, items) {
    const inputs = new UpdateLoanSearchModel(
      AppSettings.TenantSchema,
      configID,
      status
    );

    this.promise = this._loanservice.UpdateLoanSearchFilterStatus(inputs);
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
