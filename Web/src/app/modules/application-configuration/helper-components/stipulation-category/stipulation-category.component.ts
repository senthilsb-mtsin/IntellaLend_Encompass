
import { StipulationCategoryService } from './../../service/stipulation-category.service';
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
import { Subscription } from 'rxjs';
import { DomSanitizer } from '@angular/platform-browser';
import { NotificationService } from '@mts-notification';

import { AddUpdateStipulationModel } from '../../models/stipulation.model';
import { ConfigRequestModel } from '../../models/config-request.model';
import { ApplicationConfigService } from '../../service/application-configuration.service';
@Component({
  selector: 'mts-stipulation-category',
  templateUrl: './stipulation-category.component.html',
  styleUrls: ['./stipulation-category.component.css'],
  providers: [StipulationCategoryService],
})
export class StipulationCategoryComponent
  implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(DataTableDirective) stipulationTable: DataTableDirective;
  @ViewChild('stipulationModal') _stipulationModal: ModalDirective;
  isRuleErrMsgs: boolean;
  errMsgStyle: any;
  ruleErrMsgs: any;
  dtOptions: any;
  dTable: DataTables.Api;
  label: string;
  Items: any = { StipulationCategory: '', Active: true };
  promise: Subscription;
  stipulationData: any = { StipulationCategory: '', Active: true };

  constructor(
    private _stipulationservice: StipulationCategoryService,
    private sanitizer: DomSanitizer,
    private _notificationservice: NotificationService,
    private _appconfigservice: ApplicationConfigService
  ) { }

  private subscription: Subscription[] = [];
  ngOnInit(): void {
    this.dtOptions = {
      iDisplayLength: 10,
      aLengthMenu: [
        [5, 10, 25, 50, -1],
        [5, 10, 25, 50, 'All'],
      ],
      aoColumns: [
        {
          sTitle: 'Stipulation Name',
          mData: 'StipulationCategory',
          sWidth: '40%',
        },
        {
          sTitle: 'Active/Inactive',
          mData: 'Active',
          sClass: 'text-center',
          sWidth: '10%',
        },
        {
          sTitle: 'Edit',
          mData: 'StipulationID',
          sClass: 'text-center',
          sWidth: '10%',
        },
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
      this._stipulationservice.stipulationTableData$.subscribe(
        (result: any) => {
          this.dTable.clear();
          this.dTable.rows.add(result);
          this.dTable.draw();
        }
      )
    );
    this.subscription.push(
      this._stipulationservice.isstipulationchanged$.subscribe((result) => {
        this.Items = [];
        this._stipulationModal.hide();
        this.GetStipulationList();
      })
    );
    this.subscription.push(
      this._appconfigservice.addStipluation$.subscribe((result: any) => {
        this.label = 'Add Investor Stipulaion';
        this.AddStipulation();
      })
    );
  }

  ngAfterViewInit() {
    this.stipulationTable.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
      this.GetStipulationList();
    });
  }

  getRowData(rowIndex: Node, rowData: any): void {
    this.stipulationData = { ...rowData };
    this.Items = [];
    this.Items = { ...this.stipulationData };

    this.label = 'Edit Stipulation';
    this._stipulationModal.show();
  }

  changeMode() {
    this.Items.Active = !this.Items.Active;
  }

  AddStipulation() {
    this.Items.Active = true;
    this.Items.StipulationCategory = '';
    this.stipulationData.StipulationCategory = '';
    this._stipulationModal.show();
  }

  modalHide() {
    this._stipulationModal.hide();
    this.Items = [];
    this.ruleErrMsgs = '';
    this.isRuleErrMsgs = false;
  }

  SaveData() {
    const inputs = new AddUpdateStipulationModel(
      AppSettings.SystemSchema,
      this.Items.StipulationCategory,
      this.Items.Active
    );

    this.promise = this._stipulationservice.SaveData(inputs);
  }

  AddEdit() {

    this.Items.StipulationCategory =
      this.Items.StipulationCategory.trim() === ''
        ? ''
        : this.Items.StipulationCategory;
    if (this.Items.StipulationCategory !== '') {
      this.findDuplicate();
    } else {
      this._notificationservice.showWarning('Enter Valid data');
    }

    // this.SaveData();

  }

  UpdateData() {
    const inputs = new AddUpdateStipulationModel(
      AppSettings.SystemSchema,
      this.Items.StipulationCategory,
      this.Items.Active,
      this.stipulationData.StipulationID
    );
    this.promise = this._stipulationservice.UpdateData(inputs);
  }
  GetStipulationList() {
    const inputs = new ConfigRequestModel(AppSettings.SystemSchema);

    this.promise = this._stipulationservice.GetStipulationList(inputs);
  }
  findDuplicate() {
    const fullTableDataArray = this.dTable.rows().data().toArray();
    const newData = [];

    fullTableDataArray.forEach((element) => {
      newData.push(element.StipulationCategory);
    });

    for (let i = 0; newData.length > i; i++) {
      if (newData[i].toLocaleUpperCase() === this.Items.StipulationCategory.toLocaleUpperCase() && this.stipulationData.StipulationCategory.toLocaleUpperCase() !== this.Items.StipulationCategory.toLocaleUpperCase()) {
        this.isRuleErrMsgs = true;
        this.errMsgStyle = this.sanitizer.bypassSecurityTrustStyle('red');
        this.ruleErrMsgs = 'Name already Exist in List';
        break;
      } else {
        this.isRuleErrMsgs = false;
      }
    }
    if (!this.isRuleErrMsgs) {
      if (this.label === 'Add Investor Stipulaion') {
        this.SaveData();
      } else {
        this.UpdateData();
      }
    }
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
