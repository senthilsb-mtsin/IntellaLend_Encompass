import { Subscription } from 'rxjs';
import { SMTPService } from './../../service/smtp-setting.service';
import { Component, OnInit, AfterViewInit, ViewChild, OnDestroy } from '@angular/core';
import {
  SmtpDetailsData,
  SmtpSaveRequestModel,
} from '../../models/smtp-request-data.model';
import { ADConfigService } from '../../service/ad-configuration.service';
import { DataTableDirective } from 'angular-datatables';
import { Adconfigmodel } from '../../models/ad-configuration.model';
import { ApplicationConfigService } from '../../service/application-configuration.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
@Component({
  selector: 'mts-ad-configuration',
  templateUrl: './ad-configuration.component.html',
  styleUrls: ['./ad-configuration.component.css'],
  providers: [ADConfigService],
})
export class ADConfigurationComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(DataTableDirective) ADGroupdatatable: DataTableDirective;
  @ViewChild('ADGroupModal') ADGroupModal: ModalDirective;
  ADGroupsTable: DataTables.Api;
  ADGroupsdtOptions: any = {};
  adconfig: Adconfigmodel = new Adconfigmodel();
  btnEnabled = false;
  AdGroups: any = [];
  AdGroupsData: any = [];

  constructor(
    private _adconfigservice: ADConfigService,
    private _appconfigservice: ApplicationConfigService
  ) { }
  private subscription: Subscription[] = [];
  ngOnInit() {
    this.subscription.push(
      this._adconfigservice.adconfigdata$.subscribe((result: any) => {
        for (const i in result) {
          if (result[i].ConfigKey === 'AD_Domain') {
            this.adconfig.AD_Domain = result[i].ConfigValue;
          } else if (result[i].ConfigKey === 'LDAP_url') {
            this.adconfig.LDAP_url = result[i].ConfigValue;
            if (result[i].ConfigValue !== '' && result[i].ConfigValue !== null) {
              this.btnEnabled = true;
            }
          }
        }
        this.ADGroupModal.hide();
      })
    );
    this._adconfigservice.GetADConfig();
  }
  ngAfterViewInit() {
  }
  GetADGroups() {
    this.subscription.push(
      this._adconfigservice._ADGroupFields$.subscribe((result: any) => {
        if (result != null) {
          if (result.AvailableGroup.length > result.NewGroups.length) {
            for (let i = result.NewGroups.length; i < result.AvailableGroup.length; i++) {
              result.NewGroups[i] = '';
            }
          }
          if (result.AvailableGroup.length < result.NewGroups.length) {
            for (let i = result.AvailableGroup.length; i < result.NewGroups.length; i++) {
              result.AvailableGroup[i] = '';
            }
          }
          const dataLen = result.AvailableGroup.length;
          for (let i = 0; i < dataLen; i++) {
            this.AdGroupsData[i] = {AvailableGroup : result.AvailableGroup[i], NewGroups: result.NewGroups[i]};
          }
          this.AdGroups = this.AdGroupsData;
          this.ADGroupModal.show();
        }
      })
    );
    this._adconfigservice.GetADGroups(this.adconfig.LDAP_url);
  }
  saveADGroup() {
    this._adconfigservice.SaveADConfig(this.adconfig);
    if (this.adconfig.LDAP_url !== '' && this.adconfig.LDAP_url !== null) {
      this.btnEnabled = true;
    } else {
      this.btnEnabled = false;
    }
  }
  CancelAdConfig() {
    this._appconfigservice.RefreshConfig();
  }
  ngOnDestroy() {
    this.subscription.forEach(element => {
      element.unsubscribe();
    });
  }
}
