import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { LoginDataAccess } from '../login.data';
import { NotificationService } from '@mts-notification';

@Injectable()
export class ForgetPasswordService {

  //region Forget Password
  showThankYouDiv = new Subject<boolean>();
  // end

  constructor(private loginData: LoginDataAccess,
    private _notificationService: NotificationService) { }

  submitUserNameForm(_resBody: { TableSchema: string, UserName: string }) {
    this.loginData.submitUserNameForm(_resBody).subscribe(
      res => {
        if (res !== null) {
          if (res.Data !== null) {
            this.submitDirect(_resBody);
          } else {
            this._notificationService.showError('Enter Valid Username');
          }
        }
      }
    );
  }

  submitDirect(_resBody: { TableSchema: string, UserName: string }) {
    this.loginData.submitDirect(_resBody).subscribe(res => {
      if (res !== null) {
        if (res.Data === 'True') {
          this.showThankYouDiv.next(true);
        } else {
          this.showThankYouDiv.next(false);
          this._notificationService.showError('Enter Valid Username');
        }
      }
    });
  }
}
