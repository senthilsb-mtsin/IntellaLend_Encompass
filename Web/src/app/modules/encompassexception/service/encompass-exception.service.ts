import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AppSettings } from '@mts-app-setting';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { NotificationService } from '@mts-notification';
import { Subject } from 'rxjs';
import { EncompassExceptionDataAccess } from '../encompass-exception.data';

const jwtHelper = new JwtHelperService();

@Injectable()

export class EncompassExceptionService {
    encompassExceptionDtable = new Subject<any>();
    EncompassExceptionModalShow = new Subject<any>();
    constructor(private _encompassExceptionDataAccess: EncompassExceptionDataAccess,

        private _notificationService: NotificationService) {

    }
    private encompassExceptionData: any = [];

    SearchDownloadException(req: any) {
        return this._encompassExceptionDataAccess.SearchEncompassExceptionDetails(req).subscribe(res => {
            if (isTruthy(res)) {
                const data = jwtHelper.decodeToken(res.Data)['data'];
                this.encompassExceptionData = data;
                this.encompassExceptionDtable.next(this.encompassExceptionData);

            } else {
                this.encompassExceptionData = [];
                this.encompassExceptionDtable.next(this.encompassExceptionData);
            }
        });
    }

    RetryEncompassException(req: any) {

        return this._encompassExceptionDataAccess.RetryEncompassException(req).subscribe(res => {
            if (isTruthy(res)) {
                const data = jwtHelper.decodeToken(res.Data)['data'];
                if (data === true) {
                    this.EncompassExceptionModalShow.next(true);
                } else {
                    this.EncompassExceptionModalShow.next(false);
                }

            } else {
                this.EncompassExceptionModalShow.next(false);
            }
        });
    }

}
