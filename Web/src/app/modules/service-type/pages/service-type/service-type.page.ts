import { Component, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DataTableDirective } from 'angular-datatables';
import { Subscription } from 'rxjs';
import { ServiceTypeService } from '../../service/service-type.service';
import { Router } from '@angular/router';
import { SessionHelper } from '@mts-app-session';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { AddServiceTypeService } from '../../service/add-service-type.service';
import { ServiceTypeModel } from '../../models/service-type.model';

@Component({
  selector: 'mts-service-type',
  templateUrl: 'service-type.page.html',
  styleUrls: ['service-type.page.css'],
})
export class ServiceTypeComponent implements OnInit, AfterViewInit {
  //#region  Public Variables
  @ViewChild('confirmModal') confirmModal: ModalDirective;
  @ViewChild(DataTableDirective, { static: false }) dt: DataTableDirective;

  dtOptions: any = {};
  dTable: any;
  showHide: any = [false, false, false];
  rowSelected = true;
  ReviewTypeName = '';
  promise: Subscription;
  priorityList: any = [];
  //#endregion Public Variables

  //#region Constructor
  constructor(private _serviceTypeService: ServiceTypeService, private _addServiceTypeService: AddServiceTypeService, private _route: Router) {
    this.checkPermission('AddBtn', 0);
    this.checkPermission('EditBtn', 1);
    this.checkPermission('ViewBtn', 2);
  }
  //#endregion Constructor

  //#region  Private Variables
  private ReviewTypeID = 0;
  private subscriptions: Subscription[] = [];
  //#endregion Private Variables

  //#region  Public Methods
  ngAfterViewInit() {
    this.dt.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
      if (isTruthy(this.dTable)) {
        this.getServiceTypeList();
      }
    });
  }

  ngOnInit() {
    this.subscriptions.push(this._serviceTypeService.setServiceTypeMasterTableData.subscribe((res: ServiceTypeModel[]) => {
      this.dTable.clear();
      this.dTable.rows.add(res);
      this.dTable.draw();
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
        { sTitle: 'ReviewTypeID', mData: 'ReviewTypeID', sClass: 'text-right', bVisible: false },
        { sTitle: 'Service Type Name', mData: 'ReviewTypeName', sWidth: '75%' },
        { sTitle: 'Service Type Priority', mData: 'ReviewTypePriority', sClass: 'text-left', sWidth: '15%' },
        { sTitle: 'Active/Inactive', mData: 'Active', sClass: 'text-center', sWidth: '10%' },
        { sTitle: 'BatchClassInputPath', mData: 'BatchClassInputPath', bVisible: false },
        { mData: 'UserRoleID', bVisible: false }
      ],
      aoColumnDefs: [
        {
          'aTargets': [2],
          'mRender': function (data, type, row) {
            switch (data) {
              case 1:
                return 'Critical';
                break;
              case 2:
                return 'High';
                break;
              case 3:
                return 'Medium';
                break;
              case 4:
                return 'Low';
                break;
              default:
                return 'Pretty Low';
                break;
            }
          }
        },
        {
          'aTargets': [3],
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
            return '<label class=\'label ' + statusColor[row['Active']] + ' label-table\'>' + statusFlag + '</label></td>';
          }
        }],
      rowCallback: (row: Node, data: ServiceTypeModel, index: number) => {
        const self = this;
        $('td', row).unbind('click');
        $('td', row).bind('click', () => {
          self.getRowData(row, data);
        });
        // let priVale = $($('td', row)[1]).text();
        // if (priVale.startsWith('#')) {
        //   priVale = priVale.replace('#', '');
        //   $($('td', row)[1]).text('');
        //   this.priorityList.forEach(element => {
        //     if (element.id === priVale) {
        //       $($('td', row)[1]).text(element.text);
        //     }
        //   });
        // }
        return row;
      }
    };
  }

  getRowData(rowIndex: any, rowData: ServiceTypeModel): void {
    this.ReviewTypeName = rowData.ReviewTypeName;
    this.ReviewTypeID = rowData.ReviewTypeID;
    this._addServiceTypeService.setCurrentReviewDetails(rowData);
    this.rowSelected = $(rowIndex).hasClass('selected');
  }

  getRowDataForSync(rowData: ServiceTypeModel): void {
    this.ReviewTypeName = rowData.ReviewTypeName;
    this.ReviewTypeID = rowData.ReviewTypeID;
    this.confirmModal.show();
  }

  getServiceTypeList() {
    this.promise = this._serviceTypeService.getServiceTypeList();
  }

  ShowServiceTypeModal(modalType: number) {
    this._serviceTypeService.clearServiceType();
    this._addServiceTypeService.getServicePriorityList();
    this._addServiceTypeService.getServiceRoleList();
    if (modalType === 0) {
      this._addServiceTypeService.setCurrentReviewDetails(new ServiceTypeModel(0, '', 0, true, '', 0, 0));
      this._route.navigate(['view/reviewtype/addreviewtype']);
    } else if (modalType === 1) {
      this._serviceTypeService.setServiceType({ Type: 'Edit', ServiceTypeID: this.ReviewTypeID, ServiceTypeName: this.ReviewTypeName });
      this._route.navigate(['view/reviewtype/editreviewtype']);
    } else if (modalType === 2) {
      this._serviceTypeService.setServiceType({ Type: 'View', ServiceTypeID: this.ReviewTypeID, ServiceTypeName: this.ReviewTypeName });
      this._route.navigate(['view/reviewtype/viewreviewtype']);
    }
  }

  checkPermission(component: string, index: number): void {
    const URL = 'View\\ReviewType\\' + component;
    const AccessCheck = false;
    const AccessUrls = SessionHelper.RoleDetails.URLs;
    if (AccessCheck !== null) {
      AccessUrls.forEach(element => {
        if (element.URL === URL) {
          this.showHide[index] = true;
          return false;
        }
      });
    }
  }
  //#endregion Public Methods

}
