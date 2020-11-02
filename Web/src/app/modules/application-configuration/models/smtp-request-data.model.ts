export class SmtpDetailsData {
  SMTPNAME = '';
  SMTPCLIENTHOST: any = '';
  SMTPCLIENTPORT: any = '';
  USERNAME: any = '';
  PasswordString: any = '';
  DOMAIN: any = '';
  TIMEOUT: any = '';
  SMTPDELIVERYMETHOD: any = '';
  USEDEFAULTCREDENTIALS: any = '';
  ENABLESSL: any = '';
}

export class SmtpSaveRequestModel {
  SMTPMaster: SmtpDetailsData;
  constructor(SMTPMaster: SmtpDetailsData) {
    this.SMTPMaster = SMTPMaster;
  }
}
