import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AppSettings } from '@mts-app-setting';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { LoanSearchTableModel } from '../../loansearch/models/loan-search-table.model';
import { FannieMaeFieldsDataAccess } from '../fanniemaeFields.data';
const jwtHelper = new JwtHelperService();

@Injectable()
export class FannieMaeFieldsService {

    _fannieMaeFields$ = new Subject<any>();
    _fannieMaeFields = [];
    showModal$ = new Subject<boolean>();
    yourModal: ModalDirective;
    constructor(private _fannieMaeFieldsDataAccess: FannieMaeFieldsDataAccess) {

    }

    setModal(modal: ModalDirective) {

        this.yourModal = modal;
        this.showModal();
    }

    showModal() {

        this._fannieMaeFields$.next(this._fannieMaeFields);

    }
    GetFannieMaeFields(LoanID: any) {
        if (this._fannieMaeFields.length !== 0) {
            this._fannieMaeFields$.next(this._fannieMaeFields);
        } else {
            const req = { TableSchema: AppSettings.TenantSchema, LoanID: LoanID };
            this._fannieMaeFieldsDataAccess.GetFannieMaeFields(req).subscribe(res => {
                const data = jwtHelper.decodeToken(res.Data)['data'];
                if (data !== null) {
                    this._fannieMaeFields$.next(data);
                    this._fannieMaeFields = data;
                }
            });
        }

    }

}
