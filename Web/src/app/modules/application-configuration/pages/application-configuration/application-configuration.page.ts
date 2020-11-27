
import { Subscription } from 'rxjs';
import { ApplicationConfigService } from './../../service/application-configuration.service';
import {
  Component,
  OnInit,
  OnDestroy,
} from '@angular/core';
import { AppSettings, WebHookSubscriptionEventTypesConstants } from '@mts-app-setting';

import { ConfigTypeModel } from '../../models/config-type.model';
import { CheckWebHookEventTypeExistModal } from '../../models/webhook-subscription';
@Component({
  selector: 'mts-application-configuration',
  templateUrl: './application-configuration.page.html',
  styleUrls: ['./application-configuration.page.css'],
})
export class ApplicationConfigurationComponent
  implements OnInit, OnDestroy {
  TenantConfigValues: any = 0;
  dtOptions: any;
  configTypeItems: ConfigTypeModel[] = [];
  dTable: any;
  belowcomponent = 'configtable';
  isconfigsettings = false;
  configvaluess: any;
  WebHookSubscriptionEventTypes: any = []
  SelectedEventTypeExist: boolean = false;
  SelectedEventType: number;
  constructor(private _appconfigservice: ApplicationConfigService,
  ) { }
  private subscription: Subscription[] = [];
  ngOnInit() {
    AppSettings.TenantConfigType.forEach((element: ConfigTypeModel) => {
      this.configTypeItems.push(element);
    });
    WebHookSubscriptionEventTypesConstants.EventTypesDropdown.forEach((element) => {
      this.WebHookSubscriptionEventTypes.push({Value: element, Text: WebHookSubscriptionEventTypesConstants.EventTypesDescription[element]});
    });
    this.subscription.push(
      this._appconfigservice.refresh$.subscribe(
        (result: any) => {
          this.onConfigTypeChange(true);
          this.TenantConfigValues = 0;
        }
      )
    );
    this.subscription.push(
      this._appconfigservice.WebHookSubscriptionEventTypeExist$.subscribe(
        (result: boolean) => {
          this.SelectedEventTypeExist = result;
        }
      )
    );
    this.SelectedEventType = WebHookSubscriptionEventTypesConstants.DocumentLog;
    this.onChangeEventType();
  }

  onConfigTypeChange(value: any) {
    this.isconfigsettings = false;
    if (value.ConfigType === 'checkbox') {
      this.isconfigsettings = true;
      this._appconfigservice.SetConfigType(value);
      this.configvaluess = value;

      if (value !== 0) {
        this.belowcomponent = 'configtable';
      }
    } else if (value.ConfigType === 'text') {
      this.isconfigsettings = true;
      this.configvaluess = value;
      this._appconfigservice.SetConfigType(value);
      if (value !== 0) {
        this.belowcomponent = 'configtable';
      }
    } else if (value.ConfigType === 'smtp') {
      this.belowcomponent = 'smtp';

    } else if (value.ConfigType === 'Box') {

      this.belowcomponent = 'Box';
    } else if (value.ConfigType === 'Table') {

      this.belowcomponent = 'Table';
    } else if (value.ConfigType === 'List') {

      this.belowcomponent = 'List';
    } else if (value.ConfigType === 'Report') {

      this.belowcomponent = 'Report';
    } else if (value.ConfigType === 'Stipulation') {
      this.belowcomponent = 'Stipulation';

    } else if (value.ConfigType === 'LoanSearchFilter') {

      this.belowcomponent = 'LoanSearchFilter';
    } else if (value.ConfigType === 'password') {

      this.belowcomponent = 'password';
    } else if (value.ConfigType === 'WebHook') {

      this.belowcomponent = 'WebHook';
    } else {
      this.belowcomponent = 'configtable';
    }

  }
  AddCategoryLists() {
    this._appconfigservice.addCategory$.next(true);
  }

  AddInvestor() {
    this._appconfigservice.addStipluation$.next(true);
  }

  AddReportMaster() {
    this._appconfigservice.addReport$.next(true);
  }
  onChangeEventType(){
    const req: CheckWebHookEventTypeExistModal = new CheckWebHookEventTypeExistModal(AppSettings.TenantSchema, this.SelectedEventType);
    this._appconfigservice.CheckWebHookSubscriptionEventTypeExist(req);
  }
  createWebHookSubscription(){

  }
  deleteWebHookSubscription(){

  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });

  }
}
