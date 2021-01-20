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
import { LoanTypeMappingModel } from '../models/loan-type-mapping.model';

const jwtHelper = new JwtHelperService();

@Injectable()
export class AddServiceTypeService {

  //#region Public Variables
  setNextStep = new Subject<ServiceTypeWizardStepModel>();
  assignedLoanTypes = new Subject<number[]>();
  isRowNotSelected = new Subject<boolean>();
  AddServiceTypeSteps: any = { ServiceType: 1, AssignLoanType: 2, AssignLenders: 3 };
  Loading = new Subject<boolean>();
  priorityList = new Subject<ServiceTypePriorityModel[]>();
  roleList = new Subject<ServiceTypeRoleModel[]>();
  loanRetainConfirm$ = new Subject<boolean>();
  loanConfirmModal$ = new Subject<boolean>();
  allAssignedLoanTypes = new Subject<any>();
  allLenders$ = new Subject<any>();
  allAssignedLenders = new Subject<any>();
  CurrentLoanType$ = new Subject<string>();
  IsAdd$ = new Subject<boolean>();
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
  private _currentLoanType: LoanTypeMappingModel;
  private _loanTypeIds: any[] = [];
  private _allLoanTypes: any[] = [];
  private _assignedLoanTypes: any[] = [];

  // Lender variables
  private _allLenders: any[] = [];
  private _assignedLenders: any[] = [];

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

  SaveReviewLoanLenderMapping(_isAdd: boolean) {
    const _allLenderIDs = [];
    this._allLenders.forEach(element => {
      _allLenderIDs.push(element.CustomerID);
    });
    const _assignedlenderIDs = [];
    this._assignedLenders.forEach(element => {
      _assignedlenderIDs.push(element.CustomerID);
    });
    // const _IsAdd = this.IsAdd$;
    const req = { TableSchema: AppSettings.TenantSchema, ReviewTypeID : this._reviewDetails.ReviewTypeID, LoanTypeID: this._currentLoanType.LoanTypeID, AllLendersIDs : _allLenderIDs, AssignedLendersIDs: _assignedlenderIDs, IsAdd : _isAdd};
    this._ServiceTypeData.SaveReviewLoanLenderMapping(req).subscribe(res => {
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

  setCurrentReviewDetails(serviceTypModel: ServiceTypeModel) {
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

  getAllLenders() {
    return this._allLenders.slice();
  }

  getAllAssignedLenders() {
    const _lenderIDs = [];
    this._assignedLenders.forEach(element => {
      _lenderIDs.push({CustomerID: element.CustomerID, CustomerName: element.CustomerName});
    });
    this.allAssignedLenders.next(_lenderIDs);
    return this._assignedLenders.slice();
  }

  setLenders(_assignedLenders: any[], _allLenders: any[]) {
    const _lenderIDs = [];
    _assignedLenders.forEach(element => {
      _lenderIDs.push({CustomerID: element.CustomerID, CustomerName: element.CustomerName});
    });
    this._assignedLenders = _assignedLenders.slice();
    this._allLenders = _allLenders.slice();
    this.allAssignedLenders.next(_lenderIDs);
  }

  GoToServiceType() {
    this.setNextStep.next(new ServiceTypeWizardStepModel(this.AddServiceTypeSteps.ServiceType, 'active', '', ''));
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
  getSysLoanTypes() {
    this._ServiceTypeData.GetSysLoanTypes({ ReviewTypeID: this._serviceTypeID }).subscribe(
      res => {
        const result = jwtHelper.decodeToken(res.Data)['data'];
        this._allLoanTypes = result.AllLoanTypes;
        this.allAssignedLoanTypes = result.AssignedLoanTypes;
        this._assignedLoanTypes = result.AssignedLoanTypes;
        this.setNextStep.next(new ServiceTypeWizardStepModel(this.AddServiceTypeSteps.AssignLoanType, 'active complete', 'active', ''));
      }
    );
  }

  getAssignedLenders() {
    this._ServiceTypeData.GetAssignedLenders({TableSchema: AppSettings.TenantSchema, ReviewTypeID: this._serviceTypeID, LoanTypeID: this._currentLoanType.LoanTypeID }).subscribe(
      res => {
        const result = jwtHelper.decodeToken(res.Data)['data'];
        this.allLenders$.next(result.AllLenders);
        this._allLenders = result.AllLenders;
        this.allAssignedLenders.next(result.AssignedLenders);
        this._assignedLenders = result.AssignedLenders;
      }
    );
  }

  SetSelectedLoanType(vals: LoanTypeMappingModel) {

    this._currentLoanType = vals;
    this.CurrentLoanType$.next(this._currentLoanType.LoanTypeName);
    this.setNextStep.next(new ServiceTypeWizardStepModel(this.AddServiceTypeSteps.AssignLenders, 'active complete', 'active complete', 'active'));
  }

  CheckCustReviewLoanMapping(vals: LoanTypeMappingModel) {

    const req = { TableSchema: AppSettings.TenantSchema, ReviewTypeID: this._serviceTypeID, LoanTypeID: vals.LoanTypeID };
    this._ServiceTypeData.CheckCustReviewLoanMapping(req).subscribe(res => {
      const Result = jwtHelper.decodeToken(res.Data)['data'];
      if (Result) {
        vals.loading = false;
        this.loanRetainConfirm$.next(true);
      } else {
        this.SetCustReviewLoanMapping(vals);
      }
    });
  }

  SetCustReviewLoanMapping(vals: LoanTypeMappingModel) {
    this.loanRetainConfirm$.next(false);
    vals.loading = true;
    this._loanTypeIds = [];
    this._loanTypeIds.push(vals.LoanTypeID);
    const req = new AssignLoanTypesRequestModel(
      this._serviceTypeID,
      this._loanTypeIds,
    );
    this._ServiceTypeData.SaveLoanMapping(req).subscribe(res => {
      const Result = jwtHelper.decodeToken(res.Data)['data'];
      if (Result) {
        vals.loading = false;
        if (!vals.DBMapped) {
          vals.DBMapped = !vals.DBMapped;
        }
        this._notificationService.showSuccess('Mapping Added Successfully');
      }
    });
  }

  RemoveReviewLoanMapping(vals: LoanTypeMappingModel) {
    this._loanTypeIds = [];
    this._loanTypeIds.push(vals.LoanTypeID);
    const req = new AssignLoanTypesRequestModel(
      this._serviceTypeID,
      this._loanTypeIds,
    );
    this._ServiceTypeData.RemoveLoanMapping(req).subscribe(res => {
        const Result = jwtHelper.decodeToken(res.Data)['data'];
        if (Result) {
            this.loanConfirmModal$.next(false);
            this._notificationService.showSuccess('Mapping Removed Successfully');
        }
    });
}

  private GotoMaster() {
    this._location.back();
  }
  //#endregion Private Methods

}
