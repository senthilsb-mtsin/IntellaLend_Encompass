import {
  Component,
  OnInit,
  ViewChild,
  AfterViewInit,
  OnDestroy,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { AuditConfigService } from '../../service/audit-configuration.service';
import { DomSanitizer } from '@angular/platform-browser';

import { DataTableDirective } from 'angular-datatables';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppSettings } from '@mts-app-setting';
import { ConfigRequestModel } from '../../models/config-request.model';
import {
  AuditConfigdata,
  UpdateAuditModel,
} from '../../models/save-audit-config.model';

@Component({
  selector: 'mts-audit-config',
  templateUrl: './audit-config.component.html',
  styleUrls: ['./audit-config.component.css'],
  providers: [AuditConfigService],
})
export class AuditConfigComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(DataTableDirective) audittable: DataTableDirective;
  @ViewChild('auditConfigConfirmModal') auditConfigConfirmModal: ModalDirective;
  promise: Subscription;
  dTable: any;
  dtOptions: any = {};
  triggerchar = '@';
  items: any = [];
  auidtConfigRowDatas: AuditConfigdata = {
    Description: '',
    AuditDescriptionID: 0,
    Active: false,
  };
  constructor(
    private _auditservice: AuditConfigService,
    private sanitizer: DomSanitizer
  ) { }
  private subscription: Subscription[] = [];

  ngOnInit() {
    this.dtOptions = {
      aaData: [],
      iDisplayLength: 10,
      aLengthMenu: [
        [5, 10, 25, 50, -1],
        [5, 10, 25, 50, 'All'],
      ],
      aoColumns: [
        { sTitle: 'Audit Description', mData: 'Description', sWidth: '40%' },
        {
          sTitle: 'Active/Inactive',
          mData: 'Active',
          sClass: 'text-center',
          sWidth: '10%',
        },
        {
          sTitle: 'Edit',
          mData: 'AuditDescriptionID',
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
      this._auditservice.auditTableData$.subscribe((result) => {
        this.dTable.clear();
        this.dTable.rows.add(result);
        this.dTable.draw();
      })
    );
    this.subscription.push(
      this._auditservice.isUpdated$.subscribe((result) => {
        this.auditConfigConfirmModal.hide();
        this.GetAllAuditConfig();
      })
    );
  }

  ngAfterViewInit() {
    this.audittable.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
      this.GetAllAuditConfig();
    });
  }
  GetAllAuditConfig() {
    const inputs = new ConfigRequestModel(AppSettings.TenantSchema);
    this.promise = this._auditservice.GetAllAuditConfig(inputs);
  }

  getRowData(rowIndex: Node, rowData: any): void {
    this.auidtConfigRowDatas = rowData;
    this.items = [];
    if (this.auidtConfigRowDatas.ConcatenateFields === '') {
      this.items.push('No Computed Values');
    } else {
      const fields = this.auidtConfigRowDatas.ConcatenateFields.toString().split(
        ','
      );
      fields.forEach((element) => {
        this.items.push(element.trim());
      });
    }
    this.auditConfigConfirmModal.show();
  }

  Confirm() {

    const inputs = new UpdateAuditModel(
      AppSettings.TenantSchema,
      this.auidtConfigRowDatas
    );

    this.promise = this._auditservice.UpdateAuditConfig(inputs);
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
