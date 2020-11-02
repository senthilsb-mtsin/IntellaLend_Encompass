import { Subscription } from 'rxjs';
import { SMTPService } from './../../service/smtp-setting.service';
import { Component, OnInit, AfterViewInit } from '@angular/core';
import {
  SmtpDetailsData,
  SmtpSaveRequestModel,
} from '../../models/smtp-request-data.model';
@Component({
  selector: 'mts-smtp-settings',
  templateUrl: './smtp-settings.component.html',
  styleUrls: ['./smtp-settings.component.css'],
  providers: [SMTPService],
})
export class SmtpSettingsComponent implements OnInit, AfterViewInit {
  getSMTPDetails: SmtpDetailsData = new SmtpDetailsData();
  constructor(
    private _smtpservice: SMTPService
  ) { }
  private subscription: Subscription[] = [];
  ngOnInit(): void {
    this.subscription.push(
      this._smtpservice.smtpDetails$.subscribe((res: any) => {
        this.getSMTPDetails = res;
      })
    );
  }
  ngAfterViewInit() {
    this.GetAllSMPTDetails();
  }
  SaveSMTPSubmit() {
    const input = new SmtpSaveRequestModel(this.getSMTPDetails);
    this._smtpservice.SaveSMTPSubmit(input);
  }
  GetAllSMPTDetails() {
    this._smtpservice.GetAllSMPTDetails();
  }
  editSMTPHide() {
    this._smtpservice.CloseSmtp();
  }
}
