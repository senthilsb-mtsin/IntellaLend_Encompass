import { Component, ViewChild, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { SessionHelper } from '@mts-app-session';
import { AppSettings } from '@mts-app-setting';
import { RoleTypeService } from '../service/roletype.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';

import { RoleTypeRequest } from '../models/table-request.model';
import { Roletypemodel } from '../models/roletype-datatable.model';
import { environment } from 'src/environments/environment';
import { ADGroupMasterModel } from '../models/adgroupmaster.model';
@Component({
  selector: 'mts-role-type',
  templateUrl: 'roletype.page.html',
  styleUrls: ['roletype.page.css'],
})
export class RoleTypeComponent implements OnInit, OnDestroy, AfterViewInit {

  selectedRow: any;
  dtRoles: any = {};
  @ViewChild(DataTableDirective, { static: false }) dt: DataTableDirective;
  dTable: any;
  showHide: any = [false, false, false];
  rowSelected = true;
  promise: Subscription;
  AD_login: boolean = environment.ADAuthentication;
  ADGroupMasterList: any = [];

  constructor(
    private _roleTypeService: RoleTypeService,
    private router: Router, private route: ActivatedRoute
  ) {
    this.checkPermission('AddBtn', 0);
    this.checkPermission('EditBtn', 1);
    this.checkPermission('ViewBtn', 2);
  }

  private subscriptions: Subscription[] = [];

  ngAfterViewInit() {
    this.dt.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
      if (isTruthy(this.dTable)) {
        this.getRoleAdminList();
      }
    });
  }

  ngOnInit() {
    this._roleTypeService.getADGroupMasterList();
    this.subscriptions.push(this._roleTypeService.ADGroupMasterList$.subscribe((res: any) => {
      this.ADGroupMasterList = [...res];
    }));

    this.subscriptions.push(
      this._roleTypeService.setRoleAdminTableData.subscribe((res: any) => {
        this.dTable.clear();
        this.dTable.rows.add(res);
        this.dTable.draw();
      }));

    this.dtRoles = {
      aaData: [],
      'select': {
        style: 'single',
        info: false
      },
      'iDisplayLength': 10,
      'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
      aoColumns: [
        { sTitle: 'Role Id', mData: 'RoleID', sClass: 'text-right', bVisible: false },
        { sTitle: 'Role  Name', mData: 'RoleName', sWidth: '60%' },
        { sTitle: 'AD Group ID', mData: 'ADGroupID', sWidth: '30%', bVisible:  false},
        { sTitle: 'AD Group', mData: 'ADGroupName', sWidth: '30%', bVisible:  this.AD_login},
        { sTitle: 'Active/Inactive', mData: 'Active', sClass: 'text-center', sWidth: '10%' },
      ],
      aoColumnDefs: [
        {
          'aTargets': [4],
          'mRender': function (data, type, row) {
            let statusFlag = '';
            if (data === true) {
              statusFlag = 'Active';
            } else {
              statusFlag = 'Inactive';
            }
            const statusColor = {
              'true': 'label-success',
              'false': 'label-danger'
            };

            return '<label class=\'label ' + statusColor[row['Active']] + ' label-table\'' + '>' + statusFlag + '</label></td>';
          }
        }
      ],
      rowCallback: (row: Node, data: Roletypemodel, index: number) => {
        const self = this;
        $('td', row).unbind('click');
        $('td', row).bind('click', () => {
          self.getRowData(row, data);
        });
        return row;
      }
    };
  }

  getRoleAdminList() {
    const input = new RoleTypeRequest(
      AppSettings.TenantSchema,
    );
   this._roleTypeService.getRoleAdminList(input);
  }

  reloadDataTable() {
    this.getRoleAdminList();
  }
  getRowData(row: any, rowData: Roletypemodel) {
    this.rowSelected = $(row).hasClass('selected');
    this._roleTypeService.SetTableRowData(rowData);
    this.selectedRow = rowData;
  }

  checkPermission(component: string, index: number): void {
    const URL = 'View\\Ruletype\\' + component;
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

  ShowRoleModal(modalType: number) {
    let inputData: Roletypemodel;
    inputData = this.selectedRow;
    if (modalType === 0) {
      this.router.navigate(['view/roletype/addroletype']);
    } else if (modalType === 1) {
      this._roleTypeService.SetTableRowData(inputData);
      this.router.navigate(['view/roletype/editroletype']);
    } else if (modalType === 2) {
      this._roleTypeService.SetTableRowData(inputData);
      this.router.navigate(['view/roletype/viewroletype']);

    }

  }

  ngOnDestroy() {
    this.subscriptions.forEach(element => {
      element.unsubscribe();
    });
  }
}
