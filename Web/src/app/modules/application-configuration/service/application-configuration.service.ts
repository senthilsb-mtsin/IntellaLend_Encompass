import { Subject, ReplaySubject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ApplicationConfigDataAccess } from './../application-configuration.data';
import { Injectable } from '@angular/core';
import { ConfigTypeModel } from '../models/config-type.model';
import { ConfigAllRequestModel } from '../models/get-all-configtype-request.model';
import { CheckWebHookEventTypeExistModel } from '../models/webhook-subscription';
const jwtHelper = new JwtHelperService();
@Injectable()
export class ApplicationConfigService {
  appconfigtabledata$ = new Subject();
  configData$ = new Subject();
  isAdded$ = new Subject();
  isUpdated$ = new Subject();
  configTypevalue$ = new ReplaySubject(1);
  refresh$ = new Subject();
  addStipluation$ = new Subject();
  addCategory$ = new Subject();
  addReport$ = new Subject();
  WebHookSubscriptionEventTypeExist$ = new Subject();
  constructor(private _appconfigdata: ApplicationConfigDataAccess) { }
  private GetAllTenantConfig: ConfigAllRequestModel;
  SetConfigType(inputs: ConfigTypeModel) {
    this.configTypevalue$.next(inputs);
  }
  RefreshConfig() {
    this.refresh$.next(true);
    this.GetAllTenantConfigTypes(this.GetAllTenantConfig);
  }
  GetAllTenantConfigTypes(inputs: ConfigAllRequestModel) {
    this.GetAllTenantConfig = inputs;
    return this._appconfigdata
      .GetAllTenantConfigTypes(inputs)
      .subscribe((res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          this.appconfigtabledata$.next(result);
        }
      });
  }
  GetConfigvalues(inputs: any) {
    return this._appconfigdata.GetConfigvalues(inputs).subscribe(
      (res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          this.configData$.next(result);
        }
      }
    );
  }
  UpdateAppConfigData(inputs: any) {
    return this._appconfigdata.UpdateAppConfigData(inputs).subscribe(
      (res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          this.isUpdated$.next(result);
        }
      }
    );
  }
  AddAppConfigData(inputs: any) {
    return this._appconfigdata.AddAppConfigData(inputs).subscribe(
      (res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          this.isAdded$.next(result);
        }
      }
    );
  }
  /**
   * Function to check the selected Event type exists or not
   * @param req Parameter of type `CheckWebHookEventTypeExistModal`
   */
  CheckWebHookSubscriptionEventTypeExist(req: CheckWebHookEventTypeExistModel){
    return this._appconfigdata.CheckWebHookSubscriptionEventTypeExist(req).subscribe(
      (res)=>{
        if(res !== null){
          const Result = jwtHelper.decodeToken(res.Data)['data'];
          this.WebHookSubscriptionEventTypeExist$.next(Result.EventTypeExist);
        }
      }
    )
  }
}
