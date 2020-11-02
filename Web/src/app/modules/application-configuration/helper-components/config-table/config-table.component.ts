import {
  Component,
  OnInit,
  AfterViewInit,
  ViewChild,
  OnDestroy,
} from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { AppSettings } from '@mts-app-setting';
import { Subscription } from 'rxjs';
import { ApplicationConfigService } from '../../service/application-configuration.service';
import {
  TenantConfigType,
  ConfigAllRequestModel,
} from '../../models/get-all-configtype-request.model';

@Component({
  selector: 'mts-config-table',
  templateUrl: './config-table.component.html',
  styleUrls: ['./config-table.component.css'],
})
export class ConfigTableComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(DataTableDirective) configTable: DataTableDirective;
  dTable: any;
  promise: Subscription;
  dtOptions: any;
  constructor(private _appconfigservice: ApplicationConfigService) { }

  private subscription: Subscription[] = [];

  ngOnInit(): void {
    this.dtOptions = {
      iDisplayLength: 10,
      aaData: [],

      aLengthMenu: [
        [5, 10, 25, 50, -1],
        [5, 10, 25, 50, 'All'],
      ],
      aoColumns: [
        { sTitle: 'Configuration Name', mData: 'ConfigKey', sWidth: '40%' },
        { sTitle: 'Configuration Value', mData: 'ConfigValue', sWidth: '40%' },
        {
          sTitle: 'Active/Inactive',
          mData: 'Active',
          sClass: 'text-center',
          sWidth: '10%',
        },
        {
          sTitle: 'Delete',
          mData: 'ConfigID',
          sClass: 'text-center',
          sWidth: '10%',
        },
      ],
      aoColumnDefs: [
        {
          aTargets: [0],
          mRender: function (data, type, row) {
            let value = '';
            value = data;
            return value.replace(/_/g, ' ');
          },
        },
        {
          aTargets: [2],
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
          aTargets: [3],
          mRender: function (a, row, values) {
            if (values !== null && values !== '') {
              return '<span style=\'cursor:pointer\' class=\'delete-user material-icons txt-red\'>delete_forever</span>';
            }
          },
        },
      ],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
      //  const self = this;

        $('td .delete-user', row).unbind('click');
        $('td .delete-user', row).bind('click', () => {
          // self.getRowData(row, data);
        });
        return row;
      },
    };
    this.subscription.push(
      this._appconfigservice.appconfigtabledata$.subscribe((res: any) => {
        this.dTable.clear();
        this.dTable.rows.add(res).draw();
        this.dTable.search('').columns().search('').draw();
      })
    );
  }
  ngAfterViewInit(): void {
    this.configTable.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
      dtInstance.search('');
      this.GetAllTenantConfigTypes();
    });
  }
  GetAllTenantConfigTypes() {
    const tenantConfigData = new TenantConfigType();
    const inputs = new ConfigAllRequestModel(
      AppSettings.TenantSchema,
      tenantConfigData
    );

    this.promise = this._appconfigservice.GetAllTenantConfigTypes(inputs);
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
