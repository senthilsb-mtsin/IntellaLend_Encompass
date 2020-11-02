
import { Injectable } from '@angular/core';
import { EmailDataAccess } from '../emailtracker.data';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Subject } from 'rxjs/internal/Subject';
import { SearchEmailTrackerDetailsRequest } from '../models/search-email-tracker-details-request.model';
import { GetRowDataRequest } from '../models/get-row-data.model';
import { EmailTrackerTableModel } from '../models/email-tracker-table.model';
const jwtHelper = new JwtHelperService();

@Injectable()
export class EmailTrackerService {

    setEmailTrackerTableData = new Subject<EmailTrackerTableModel[]>();
    _emailModal = new Subject<any>();

    constructor(private _emailData: EmailDataAccess) { }
    private ruleFindings: any = [];
    private _emailtrackerSearchata: any[] = [];
    private _emailModalValues: any[] = [];
    SearchEmailTracker(_resBody: SearchEmailTrackerDetailsRequest) {
        return this._emailData.SearchEmailTrackerData(_resBody).subscribe(
            res => {
                const data = jwtHelper.decodeToken(res.Data)['data'];
                this._emailtrackerSearchata = data;
                this.setEmailTrackerTableData.next(this._emailtrackerSearchata.slice());
            });
    }
    GetCurrentData(_resBody: GetRowDataRequest) {
        return this._emailData.GetCurrentData(_resBody).subscribe(
            res => {
                const data = jwtHelper.decodeToken(res.Data)['data'];
                if (data !== null) {
                    this._emailModal.next(data);
                }
            });
    }
}
