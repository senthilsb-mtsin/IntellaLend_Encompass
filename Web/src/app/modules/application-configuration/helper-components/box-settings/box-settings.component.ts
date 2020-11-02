
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { BoxSettingsService } from '../../service/box-setting.service';
import { AppSettings, BoxConfigConstant } from '@mts-app-setting';
import { SessionHelper } from '@mts-app-session';
import {
  BoxSettingData,
  GetBoxValueModel,
  CheckBoxTokenModel,
  GetBoxTokenModel,
} from '../../models/box-setting.model';
import { ConfigRequestModel } from '../../models/config-request.model';
import { ApplicationConfigService } from '../../service/application-configuration.service';

@Component({
  selector: 'mts-box-settings',
  templateUrl: './box-settings.component.html',
  styleUrls: ['./box-settings.component.css'],
  providers: [BoxSettingsService],
})
export class BoxSettingsComponent implements OnInit, OnDestroy {
  isSave = true;
  getAllVals: any = [];
  busy: Subscription;
  promise: Subscription;
  data: BoxSettingData = new BoxSettingData();

  constructor(
    private _boxservice: BoxSettingsService,
    private _route: ActivatedRoute,
    private _appconfigservice: ApplicationConfigService
  ) {
    this.GetAllBoxSettings();
    this.data.userDetails = SessionHelper.UserDetails;

    if (this._route.queryParams['value'].code !== undefined) {
      this.GetBoxToken();
    }

    this.CheckUserBoxToken();
  }
  private subscription: Subscription[] = [];
  private _getTokenCall: boolean;
  ngOnInit(): void {
    this.subscription.push(
      this._boxservice.boxToken$.subscribe((res: any) => {
        if (res !== null) {
          window.location.href = 'view/tenantconfig';
        } else {
          this.CheckUserBoxToken();
        }
      })
    );
    this.subscription.push(
      this._boxservice.allBoxSettings$.subscribe((Result: any) => {
        if (Result) {
          this.data.clientid = '';

          Result.forEach((elt) => {
            if (elt.ConfigKey === BoxConfigConstant.BOXCLIENTID) {
              this.data.clientid = elt.ConfigValue;
              this.getAllVals.push({ ClientId: elt.ConfigValue });
            }
            if (elt.ConfigKey === BoxConfigConstant.BOXCLIENTSECRETID) {
              this.data.clientsecretid = elt.ConfigValue;
              this.getAllVals.push({ ClientSecretId: elt.ConfigValue });
            }
            if (elt.ConfigKey === BoxConfigConstant.BOXUSERID) {
              this.data.boxUserID = elt.ConfigValue;
              this.getAllVals.push({ BoxUserID: elt.ConfigValue });
            }
            if (elt.ConfigKey === BoxConfigConstant.BOXREDIRECTURL) {
              this.data.boxURL = elt.ConfigValue;
            }
          });
        }
      })
    );
    this.subscription.push(
      this._boxservice.boxValues$.subscribe((Result: any) => {
        if (Result !== null) {
          this.GetAllBoxSettings();
          this._appconfigservice.RefreshConfig();
        }
      })
    );
    this.subscription.push(
      this._boxservice.isCheckedToken$.subscribe((Result: any) => {
        this.data.BoxAuthURL = Result.BoxAuthURL;
        if (Result.TokenStatus === 0) {
          this.data.boxConnectStatus = 'Connect';
          this.data.boxConnectLabel =
            'You are not yet linked to your Box account. Click on Link button to link your account.';
        } else if (Result.TokenStatus === 2) {
          this.data.boxConnectStatus = 'Connect';
          this.data.boxConnectLabel =
            'Your Box token expired. Click on Link button to relink your account.';
        } else {
          this.data.boxConnectStatus = 'Reconnect';
          this.data.boxConnectLabel =
            'Your are connected to Box.com. Click on Reset & Link button to relink your account.';
        }
      })
    );
  }
  GetBoxToken() {
    this._getTokenCall = true;
    // this.dTable.columns.adjust().draw();
    if (this._route.queryParams['value'].code !== undefined) {
      this.data.BoxAuthCode = this._route.queryParams['value'].code;
      const inputRequest = new GetBoxTokenModel(
        AppSettings.TenantSchema,
        this.data.userDetails.UserID,
        this.data.BoxAuthCode
      );

      this.promise = this._boxservice.GetBoxToken(inputRequest);
    }
  }

  GetAllBoxSettings() {
    const inputData = new ConfigRequestModel(AppSettings.SystemSchema);

    this.promise = this._boxservice.GetAllBoxSettings(inputData);
  }
  getboxvalues() {
    let isValueExist = true;
    if (this.getAllVals.length > 0) {
      isValueExist = false;
    }
    const inputData = new GetBoxValueModel(
      AppSettings.SystemSchema,
      this.data.clientid,
      this.data.clientsecretid,
      this.data.boxUserID,
      isValueExist
    );

    this._boxservice.getboxvalues(inputData);
  }
  CheckUserBoxToken(): void {

    const inputRequest = new CheckBoxTokenModel(
      AppSettings.TenantSchema,
      this.data.userDetails.UserID
    );

    this.promise = this._boxservice.CheckUserBoxToken(inputRequest);
  }
  RedirectToBox(): void {
    window.location.href = this.data.BoxAuthURL;
  }
  CloseModal() {
    this._appconfigservice.RefreshConfig();
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
