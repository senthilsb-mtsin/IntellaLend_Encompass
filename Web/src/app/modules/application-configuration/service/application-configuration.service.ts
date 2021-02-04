import { Subject, ReplaySubject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ApplicationConfigDataAccess } from './../application-configuration.data';
import { Injectable } from '@angular/core';
import { ConfigTypeModel } from '../models/config-type.model';
import { ConfigAllRequestModel } from '../models/get-all-configtype-request.model';
import { CheckWebHookEventTypeExistModel, CreateWebHookEventTypeModel, DeleteWebHookEventTypeModel } from '../models/webhook-subscription';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { NotificationService } from '@mts-notification';
import { WebHookSubscriptionEventTypesConstants } from '@mts-app-setting';
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
  WebHookSubscriptionEventTypeExist$ = new Subject<boolean>();
  constructor(
    private _appconfigdata: ApplicationConfigDataAccess,
    private _notificationservice: NotificationService) { }
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
  CheckWebHookSubscriptionEventTypeExist(req: CheckWebHookEventTypeExistModel) {
    return this._appconfigdata.CheckWebHookSubscriptionEventTypeExist(req).subscribe(
      (res) => {
        if (res !== null) {
          const Result = jwtHelper.decodeToken(res.Data)['data'];
          this.WebHookSubscriptionEventTypeExist$.next(Result.EventTypeExist);
        }
      }
    );
  }
  /**
   * Function to `create` selected `WebHook Event type`
   * @param req Parameter of type `CreateWebHookEventTypeModel`
   */
  CreateWebHookSubscriptionEventType(req: CreateWebHookEventTypeModel) {
    return this._appconfigdata.CreateWebHookSubscriptionEventType(req).subscribe(
      (res) => {
        if (isTruthy(res)) {
          const Result = jwtHelper.decodeToken(res.Data)['data'];
          if(Result.Success) {
            this._notificationservice.showError("WebHook subscription for the Event type was created successfully");
            this.WebHookSubscriptionEventTypeExist$.next(true);
          } else {
            this._notificationservice.showError("Unable to create WebHook subscription for " + WebHookSubscriptionEventTypesConstants.EventTypesDescription[req.WebHookType]);
          }
        }
      }
    );
  }
  /**
   * Function to `delete` selected `WebHook Event type`
   * @param req Parameter of type `DeleteWebHookEventTypeModel`
   */
  DeleteWebHookSubscriptionEventType(req: DeleteWebHookEventTypeModel) {
    return this._appconfigdata.DeleteWebHookSubscriptionEventType(req).subscribe(
      (res) => {
        if (isTruthy(res)) {
          const Result = jwtHelper.decodeToken(res.Data)['data'];
          if(Result.Success) {
            this._notificationservice.showError("WebHook subscription for the Event type was deleted successfully");
            this.WebHookSubscriptionEventTypeExist$.next(false);
          } else {
            this._notificationservice.showError("Unable to delete WebHook subscription for " + WebHookSubscriptionEventTypesConstants.EventTypesDescription[req.WebHookType]);
          }
        }
      }
    );
  }
}
