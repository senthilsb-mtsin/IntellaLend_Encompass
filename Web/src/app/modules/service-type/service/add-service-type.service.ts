import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ServiceTypeWizardStepModel } from '../models/service-type-wizard-steps.model';
import { Subject } from 'rxjs';
import { AddServiceTypeRequestModel } from '../models/add-service-type-request.model';
import { ServiceTypeDataAccess } from '../service-type.data';
import { NotificationService } from '@mts-notification';
import { CommonService } from 'src/app/shared/common';
import { AssignLoanTypesRequestModel } from '../models/assign-loan-types-request.model';
import { Location } from '@angular/common';
import { ServiceTypePriorityModel } from '../models/service-type-priority.model';
import { AppSettings } from '@mts-app-setting';
import { ServiceTypeModel } from '../models/service-type.model';
import { ServiceTypeRoleModel } from '../models/service-type-role.model';

const jwtHelper = new JwtHelperService();

@Injectable()
export class AddServiceTypeService {

  //#region Public Variables
  setNextStep = new Subject<ServiceTypeWizardStepModel>();
  assignedLoanTypes = new Subject<number[]>();
  isRowNotSelected = new Subject<boolean>();
  AddServiceTypeSteps: any = { ServiceType: 1, AssignLoanType: 2 };
  Loading = new Subject<boolean>();
  priorityList = new Subject<ServiceTypePriorityModel[]>();
  roleList = new Subject<ServiceTypeRoleModel[]>();
  //#endregion Public Variables

  //#region Constructor
  constructor(
    private _ServiceTypeData: ServiceTypeDataAccess,
    private _notificationService: NotificationService,
    private _commonService: CommonService,
    private _location: Location) {
      this.getServicePriorityList();
    }
  //#endregion Constructor

  //#region Private Variables
  private _allLoanTypes: any[] = [];
  private _assignedLoanTypes: any[] = [];
  private _serviceTypeID = 0;
  private _serviceTypeName = '';
  private _priorityList: ServiceTypePriorityModel[] = [];
  private _roleList: ServiceTypeRoleModel[] = [];
  private _reviewDetails: ServiceTypeModel = new ServiceTypeModel(0, '', 0, true, '', 0, 0);
  //#endregion Private Variables

  //#region Public Methods
  addServiceTypeSubmit(req: AddServiceTypeRequestModel) {
    this._ServiceTypeData.AddServiceTypeSubmit(req).subscribe(
      res => {
        const result = jwtHelper.decodeToken(res.Data)['data'];
        if (result !== null) {
          if (result.Success) {
            this._serviceTypeID = result.ReviewTypeID;
            this._serviceTypeName = req.ReviewType.ReviewTypeName;
            this._reviewDetails.ReviewTypeID = result.ReviewTypeID;
            this._notificationService.showSuccess('Service Type Added Successfully');
            this.getSysLoanTypes();
          } else {
            this._notificationService.showError('Service Type Name already exist');
          }
        }
        this.Loading.next(false);
      });
  }

  updateServiceTypeSubmit(req: AddServiceTypeRequestModel) {
    this._ServiceTypeData.UpdateServiceTypeSubmit(req).subscribe(
      res => {
        const result = jwtHelper.decodeToken(res.Data)['data'];
        if (result !== null) {
          if (result) {
            this._serviceTypeID = req.ReviewType.ReviewTypeID;
            this._serviceTypeName = req.ReviewType.ReviewTypeName;
            this._notificationService.showSuccess('Service Type Updated Successfully');
            this.getSysLoanTypes();
          } else {
            this._notificationService.showError('Service Type Name already exist');
          }
        }
        this.Loading.next(false);
      });
  }

  SaveLoanMapping(req: AssignLoanTypesRequestModel) {
    this._ServiceTypeData.SaveLoanMapping(req).subscribe(res => {
      const result = jwtHelper.decodeToken(res.Data)['data'];
      if (result) {
        this._notificationService.showSuccess('Service Types Mapping Updated Successfully');
        this.GotoMaster();
      }
      this.Loading.next(false);
    });
  }

  getCurrentServiceTypeID(): number {
    return this._serviceTypeID;
  }

  getCurrentServiceTypeName(): string {
    return this._serviceTypeName;
  }

  setCurrentReviewDetails(serviceTypModel: ServiceTypeModel ) {
    this._reviewDetails = serviceTypModel;
  }

  getCurrentReviewDetails() {
    return this._reviewDetails;
  }

  setCurrentServiceType(input: { ServiceTypeID: number, ServiceTypeName: string }) {
    this._serviceTypeID = input.ServiceTypeID;
    this._serviceTypeName = input.ServiceTypeName;
  }

  getAllLoanTypes() {
    return this._allLoanTypes.slice();
  }

  getAssignedLoanTypes() {
    const _loanIDs = [];
    this._assignedLoanTypes.forEach(element => {
      _loanIDs.push(element.LoanTypeID);
    });
    this.assignedLoanTypes.next(_loanIDs);
    return this._assignedLoanTypes.slice();
  }

  setLoanTypes(_assignedLoans: any[], _allLoans: any[]) {
    const _loanIDs = [];
    _assignedLoans.forEach(element => {
      _loanIDs.push(element.LoanTypeID);
    });
    this._assignedLoanTypes = _assignedLoans.slice();
    this._allLoanTypes = _allLoans.slice();
    this.assignedLoanTypes.next(_loanIDs);
  }

  GoToServiceType() {
    this.setNextStep.next(new ServiceTypeWizardStepModel(this.AddServiceTypeSteps.ServiceType, 'active', ''));
  }

  getServicePriorityList() {
    return this._ServiceTypeData.GetServicePriorityList({ TableSchema: AppSettings.TenantSchema }).subscribe(res => {
      const data = jwtHelper.decodeToken(res.Data)['data'];
      this._priorityList = data;
      this.modifyServicePriorityList();
      this.priorityList.next(this._priorityList.slice());
    });
  }

  modifyServicePriorityList() {
    this._priorityList.forEach(element => {
      switch (element.ReviewPriorityID) {
        case 1:
          element.ReviewPriorityName = 'Critical(1)';
          break;
        case 2:
          element.ReviewPriorityName = 'High(2)';
          break;
        case 3:
          element.ReviewPriorityName = 'Medium(3)';
          break;
        case 4:
          element.ReviewPriorityName = 'Low(4)';
          break;
        default:
          element.ReviewPriorityName = 'Low(4)';
          break;
      }
    });
  }

  getServiceRoleList() {
    return this._ServiceTypeData.GetServiceRoleList({ TableSchema: AppSettings.TenantSchema }).subscribe(res => {
      const roles = jwtHelper.decodeToken(res.Data)['data'];
      if (roles.length > 0) {
        const _index = roles.findIndex(x => x.RoleName === 'System Administrator');
        if (_index !== -1) {
          roles.splice(_index, 1);
        }
        this._roleList = roles;
      } else {
        this._roleList = [];
      }
      this.roleList.next(this._roleList.slice());
    });
  }

  //#endregion Public Methods

  //#region Private Methods
  private getSysLoanTypes() {
    this._ServiceTypeData.GetSysLoanTypes({ ReviewTypeID: this._serviceTypeID }).subscribe(
      res => {
        const result = jwtHelper.decodeToken(res.Data)['data'];
        this._allLoanTypes = result.AllLoanTypes;
        this._assignedLoanTypes = result.AssignedLoanTypes;
        this.setNextStep.next(new ServiceTypeWizardStepModel(this.AddServiceTypeSteps.AssignLoanType, 'active complete', 'active'));
      }
    );
  }

  private GotoMaster() {
    this._location.back();
  }
  //#endregion Private Methods

}
