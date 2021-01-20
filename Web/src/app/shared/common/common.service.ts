import { CommonDataAccess } from '../common/common.data';
import { Injectable } from '@angular/core';
import { SessionHelper } from '@mts-app-session';
import { Router } from '@angular/router';
import { AppSettings } from '@mts-app-setting';
import { JwtHelperService } from '@auth0/angular-jwt';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { Subject, Subscription } from 'rxjs';
import { ReviewTypeModel } from 'src/app/modules/dashboard/models/review-type.model';

const jwtHelper = new JwtHelperService();
@Injectable({ providedIn: 'root' })
export class CommonService {
  SystemCheckListMaster = new Subject<any[]>();
  SystemStackingOrderMaster = new Subject<any[]>();
  SystemDocumentTypeMaster = new Subject<any[]>();
  SystemChecklistCategories = new Subject<any[]>();
  SystemActiveCustomerMaster = new Subject<any[]>();
  SystemLoanTypeMaster = new Subject<any[]>();
  SystemLOSFields = new Subject<any[]>();
  SystemParkingSpotItems = new Subject<any[]>();
  SystemLoanTypeItems = new Subject<any[]>();
  CustomerItems = new Subject<any[]>();
  UserItems = new Subject<any[]>();
  RoleItems = new Subject<any[]>();
  reviewTypeList = new Subject<any[]>();

  SystemDocumentTypeFieldMaster = new Subject<any[]>();
  constructor(private _commonData: CommonDataAccess, private _route: Router) { }

  private _sysChecklist: any[] = [];
  private _sysStackingOrder: any[] = [];
  private _sysDocumentTypes: any[] = [];
  private _sysDocumentTypeFields: any[] = [];
  private _sysChecklistCategories: any[] = [];
  private _sysLOSFields: any[] = [];
  private _sysParkingSpotData: any[] = [];
  private _sysActiveCustomers: any[] = [];
  private _sysLoantypeMasters: any[] = [];
  private _reviewTypeList: ReviewTypeModel[] = [];

  private _customerItems: any[] = [];
  private _roleItems: any[] = [];
  private _sysLoanTypes: any[] = [];
  UnLock(_routeURL: string) {
    if (isTruthy(SessionHelper.UserDetails)) {
      const reqBody = { TableSchema: AppSettings.TenantSchema, UserID: SessionHelper.UserDetails.UserID, Lock: false };
      this._commonData.unLockUser(reqBody).subscribe(
        res => {
          if (res !== null) {
            if (res.Data === 'True') {
              SessionHelper.cleanSessionVariables();
              this._route.navigate(['']);
            }
          }
        });
    } else {
      SessionHelper.cleanSessionVariables();
      if (_routeURL.includes('/view/loandetails/')) {
        const urls = _routeURL.split('/');
        this._route.navigate(['']);
        localStorage.setItem('loanURL', _routeURL);
      }
      this._route.navigate(['']);
    }
  }

  postError(reqBody: { Error: any }) {
    this._commonData.postError(reqBody).subscribe();
  }
  getReviewTypeList() {
    return this._commonData.GetReviewTypeList({ TableSchema: AppSettings.TenantSchema }).subscribe(res => {
      const result = jwtHelper.decodeToken(res.Data)['data'];
      this._reviewTypeList = [];
      if (result != null) {
        if (result.length > 0) {
          result.forEach(element => {
            this._reviewTypeList.push({ ReviewTypeID: element.ReviewTypeID, ReviewTypeName: element.ReviewTypeName });
          });
        }
      }
      this.reviewTypeList.next(this._reviewTypeList.slice());
    });
  }

  GetChecklistCategories() {
    if (isTruthy(this._sysChecklistCategories) && this._sysChecklistCategories.length > 0) {
      return this._sysChecklistCategories.slice();
    } else {
      this.GetAllSysCheckListCategories();
    }
  }

  GetSystemChecklistMaster(existing) {
    if (!existing) {
      this.GetAllSysCheckListMastersDatas();
    }

    if (isTruthy(this._sysChecklist) && this._sysChecklist.length > 0) {
      return this._sysChecklist;
    } else { this.GetAllSysCheckListMastersDatas(); }
  }

  GetSysStackingOrderGroup() {
    if (isTruthy(this._sysStackingOrder) && this._sysStackingOrder.length > 0) {
      return this._sysStackingOrder.slice();
    } else {
      this.GetAllSysStackingOrderDatas();
    }
  }

  ReloadSysStackingOrderData() {
    this.GetAllSysStackingOrderDatas();
  }

  GetSysLOSFields(searchValue: string) {
    const req = { Tableschema: AppSettings.TenantSchema, SearchCriteria: searchValue };
    this._commonData.GetSysLOSFields(req).subscribe(
      res => {
        const data = jwtHelper.decodeToken(res.Data)['data'];
        this._sysLOSFields = [];
        if (isTruthy(data)) {
          if (data.length > 0) {
            this._sysLOSFields = data;
          }
        }
        this.SystemLOSFields.next(this._sysLOSFields.slice());
      }
    );
  }
  GetActiveCustomerList(schema: string) {
    const reqBody = { Tableschema: schema };
    return this._commonData.GetActiveCustomerList(reqBody).subscribe(res => {
      if (isTruthy(res)) {
        this._sysActiveCustomers = [];
        const Result = jwtHelper.decodeToken(res.Data)['data'];
        if (Result.length > 0) {
          Result.forEach(element => {
            this._sysActiveCustomers.push({ id: element.CustomerID, text: element.CustomerName });
          });
        }
        this.SystemActiveCustomerMaster.next(this._sysActiveCustomers.slice());
      }
    });

  }
  GetAllLoantypeMaster(schema: string) {
    const reqBody = { Tableschema: schema };
    return this._commonData.GetAllLoantypeMaster(reqBody).subscribe(res => {
      if (isTruthy(res)) {
        this._sysLoantypeMasters = [];
        const Result = jwtHelper.decodeToken(res.Data)['data'];
        if (Result.length > 0) {
          Result.forEach(element => {
            this._sysLoantypeMasters.push({ id: element.LoanTypeID, text: element.LoanTypeName });
          });
        }
        this.SystemLoanTypeMaster.next(this._sysLoantypeMasters.slice());
      }
    });

  }
  GetSystemDocumentTypes() {
    return this._commonData.GetSystemDocumentTypes().subscribe(res => {
      if (res !== null) {
        this._sysDocumentTypes = [];
        const data = jwtHelper.decodeToken(res.Data)['data'];
        for (let i = 0; i < data.length; i++) {
          this._sysDocumentTypes.push(data[i]);
        }
        this.SystemDocumentTypeMaster.next(this._sysDocumentTypes.slice());
      }
    });

  }

  GetSystemDocumentFieldList() {
    if (this._sysDocumentTypeFields.length > 0) {
      this.SystemDocumentTypeFieldMaster.next(this._sysDocumentTypeFields.slice());
      return Subscription.EMPTY;
    } else {
      return this._commonData.GetSystemDocumentFieldList().subscribe(res => {
        if (res !== null) {
          this._sysDocumentTypeFields = [];
          const data = jwtHelper.decodeToken(res.Data)['data'];
          for (let i = 0; i < data.length; i++) {
            this._sysDocumentTypeFields.push(data[i]);
          }
          this.SystemDocumentTypeFieldMaster.next(this._sysDocumentTypeFields.slice());
        }
      });
    }
  }

  GetSystemLoanTypes() {
    this.GetSystemLoanTypeMaster();
  }

  SetSystemChecklistMaster(checkListGrp: { CheckListID: number, CheckListName: string }) {
    this._sysChecklist.push(checkListGrp);
  }

  GetCustomerList(schema: string) {
    const req = { TableSchema: schema };
    return this._commonData.GetCustomerData(req).subscribe((res) => {
      if (res !== null) {
        const data = jwtHelper.decodeToken(res.Data)['data'];
        this._customerItems = [];
        data.forEach(ele => {
          this._customerItems.push({ id: ele.CustomerID, text: ele.CustomerName });
        });
        this.CustomerItems.next(this._customerItems.slice());
      }
    });
  }

  GetRoleList(schema: string) {
    const req = { TableSchema: schema };
    return this._commonData.GetRoleData(req).subscribe((res) => {
      if (res !== null) {
        const data = jwtHelper.decodeToken(res.Data)['data'];
        this._roleItems = [];
        if (data.length > 0) {
          data.forEach(e => {
            this._roleItems.push({ id: e.RoleID, text: e.RoleName });
          });
        }

        this.RoleItems.next(this._roleItems.slice());
      }
    });
  }

  GetParkingSpotList(schema: string) {
    const req = { TableSchema: schema };
    return this._commonData.GetParkingSpotData(req).subscribe((res) => {
      if (res !== null) {
        const data = jwtHelper.decodeToken(res.Data)['data'];
        this._sysParkingSpotData = [];
        if (data.length > 0) {
          data.forEach(e => {
            this._sysParkingSpotData.push(e);
          });
        }
        this.SystemParkingSpotItems.next(this._sysParkingSpotData);
      }
    });
  }

  private GetAllSysCheckListCategories() {
    const req = { Tableschema: AppSettings.TenantSchema };
    this._commonData.GetAllSysCheckListCategories(req).subscribe(
      res => {
        const data = jwtHelper.decodeToken(res.Data)['data'];
        this._sysChecklistCategories = [];
        if (isTruthy(data)) {
          data.forEach(elt => {
            this._sysChecklistCategories.push(elt.Category);
          });
        }
        this.SystemChecklistCategories.next(this._sysChecklistCategories.slice());
      }
    );
  }

  private GetAllSysStackingOrderDatas() {
    this._commonData.GetAllSysStackingOrderMastersDatas().subscribe(
      res => {
        const data = jwtHelper.decodeToken(res.Data)['data'];
        if (isTruthy(data)) {
          this._sysStackingOrder = [];
          if (data.length > 0) {
            data.forEach(elt => {
              this._sysStackingOrder.push(elt);
            });
          }
        }
        this.SystemStackingOrderMaster.next(this._sysStackingOrder.slice());
      }
    );
  }

  private GetSystemLoanTypeMaster() {
    this._commonData.GetAllSysLoantypeDatas().subscribe(res => {
      if (res !== null) {
        const data = jwtHelper.decodeToken(res.Data)['data'];
        data.forEach(element => {
          this._sysLoanTypes.push({ LoanTypeID: element.LoanTypeID, LoanTypeName: element.LoanTypeName });
        });
        this.SystemLoanTypeItems.next(this._sysLoanTypes.slice());
      }
    });
  }
  private GetAllSysCheckListMastersDatas() {
    this._commonData.GetAllSysCheckListMastersDatas().subscribe(res => {
      const data = jwtHelper.decodeToken(res.Data)['data'];
      this._sysChecklist = [];
      if (data.length > 0) {
        data.forEach(elt => {
          this._sysChecklist.push(elt);
        });
      }
      this.SystemCheckListMaster.next(this._sysChecklist.slice());
    });
  }

}
