import { NotificationService } from './../../../../shared/service/notification.service';
import { Subscription } from 'rxjs';
import { Component, OnInit, AfterViewInit, OnDestroy, } from '@angular/core';
import { AppSettings } from '@mts-app-setting';
import { ApplicationConfigService } from '../../service/application-configuration.service';
import { ConfigTypeModel } from '../../models/config-type.model';
import {
  ConfigValueModel,
  ConfigTypeRequestModel,
  AddEditConfigModel,
} from '../../models/config-value.model';

@Component({
  selector: 'mts-config-settings',
  templateUrl: './config-settings.component.html',
  styleUrls: ['./config-settings.component.css'],
})
export class ConfigSettingsComponent
  implements OnInit, AfterViewInit, OnDestroy {

  isAddConfig = false;
  TenantConfigValues: any = 0;
  isupdateConfig = false;
  ischeckbox = false;
  istextbox = false;
  isDaysErrMsgs = true;
  configvaluesTextBox: any;
  configvalues = true;
  Active: any;
  configID: any;

  isactive = false;

  constructor(
    private _appconfigservice: ApplicationConfigService,
    private _notificationservice: NotificationService
  ) { }

  private inputData: ConfigTypeModel;
  private subscription: Subscription[] = [];
  ngAfterViewInit(): void { }

  ngOnInit(): void {

    this.subscription.push(
      this._appconfigservice.configData$.subscribe(
        (result: ConfigValueModel) => {
          if (result !== null) {
            if (result.ConfigID > 0) {
              this.Active = result.Active;

              this.configID = result.ConfigID;
              this.isupdateConfig = true;

              if (this.inputData.ConfigType === 'checkbox') {
                this.istextbox = false;
                this.ischeckbox = true;
                this.isactive = false;
                this.configvalues = this.ParsingValue(result.ConfigValue);
              } else if (this.inputData.ConfigType === 'text') {
                this.istextbox = true;
                this.ischeckbox = false;
                this.isactive = true;
                this.configvaluesTextBox = this.ParsingValue(
                  result.ConfigValue
                );
              }
            }
          } else {
            this.configvaluesTextBox = '';
            this.configvalues = true;
            this.isAddConfig = true;
            this.isupdateConfig = false;
            this.isactive = false;
          }
        }
      )
    );
    this.subscription.push(
      this._appconfigservice.isUpdated$.subscribe((result) => {
        if (result === true) {

          this.reloadData();
          this.ResetData();
          this._notificationservice.showSuccess(
            'Configuration Updated successfully'
          );
        }
      })
    );
    this.subscription.push(
      this._appconfigservice.configTypevalue$.subscribe(
        (result: ConfigTypeModel) => {
          this.inputData = result;
          this.GetConfigvalues(result);
        }
      )
    );
    this.subscription.push(
      this._appconfigservice.isAdded$.subscribe((result) => {
        if (result === true) {

          this.reloadData();
          this.ResetData();
          this._notificationservice.showSuccess(
            'Configuration Added successfully'
          );
        } else {
          this._notificationservice.showWarning('Configuration already exist');
        }
      })
    );
  }
  reloadData() {
    this._appconfigservice.RefreshConfig();
  }
  GetConfigvalues(value: any): void {
    const inputs = new ConfigTypeRequestModel(
      0,
      value.ConfigKey,
      AppSettings.TenantSchema
    );

    this._appconfigservice.GetConfigvalues(inputs);
  }

  ParsingValue(val: any): any {
    let keyVal;
    if (this.inputData.ConfigType === 'checkbox') {
      keyVal = val === 'true';
    } else if (this.inputData.ConfigType === 'text') {
      keyVal = val;
    }
    return keyVal;
  }
  UpdateAppConfigData() {
    let edittenantData;
    if (this.inputData.ConfigType === 'checkbox') {
      edittenantData = new ConfigValueModel(
        0,
        this.configvalues,
        this.Active,
        this.inputData.ConfigKey,
        this.configID
      );
    } else if (this.inputData.ConfigType === 'text' && !this.validate()) {
      // need to check config id and key
      edittenantData = new ConfigValueModel(
        0,
        this.configvaluesTextBox,
        this.Active,
        this.inputData.ConfigKey,
        this.configID
      );
    }
    if (edittenantData !== null) {
      const inputs = new AddEditConfigModel(
        AppSettings.TenantSchema,
        edittenantData
      );

      this._appconfigservice.UpdateAppConfigData(inputs);
    }
  }
  AddAppConfigData() {
    let addtenantData;
    if (this.inputData.ConfigType === 'checkbox') {
      addtenantData = new ConfigValueModel(
        0,
        this.configvalues,
        this.configvalues,
        this.inputData.ConfigKey
      );
    } else if (this.inputData.ConfigType === 'text' && !this.validate()) {
      addtenantData = new ConfigValueModel(
        0,
        this.configvaluesTextBox,
        true,
        this.inputData.ConfigKey
      );
    }
    if (addtenantData !== null) {
      const inputs = new AddEditConfigModel(
        AppSettings.TenantSchema,
        addtenantData
      );

      this._appconfigservice.AddAppConfigData(inputs);
    }
  }
  validate(): boolean {

    let returVal = false;
    if (
      this.configvaluesTextBox === null ||
      this.configvaluesTextBox === '' ||
      this.configvaluesTextBox === 'undefined'
    ) {
      this._notificationservice.showWarning('Configuration value required');
      returVal = true;
    }
    return returVal;
  }
  ResetData() {
    this.isupdateConfig = false;
    this.ischeckbox = false;
    this.istextbox = false;
    this.isactive = false;
    this.TenantConfigValues = 0;
  }
  onvaluechange(value: any) {
    if (this.TenantConfigValues.ConfigKey === 'Password_Expiry') {
      if (value === undefined || value === '') {
        this.isDaysErrMsgs = false;
      } else {
        const exp = /\b^([1-9]|1[0-9]|2[0-9])$\b/;
        this.isDaysErrMsgs = exp.test(value);
      }
    }
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
