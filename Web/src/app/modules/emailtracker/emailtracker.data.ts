import { Injectable } from '@angular/core';
import { APIService } from 'src/app/shared/service/api.service';
import { MTSAPIResponse } from 'src/app/shared/model/mts-success-response.model';
import { Observable } from 'rxjs/internal/Observable';
import { SearchEmailTrackerDetailsRequest } from './models/search-email-tracker-details-request.model';
import { GetRowDataRequest } from './models/get-row-data.model';
import { EmailTrackerApiUrlConstant } from '@mts-api-url';

@Injectable()

export class EmailDataAccess {
    constructor(private _api: APIService) { }
    SearchEmailTrackerData(_reqBody: SearchEmailTrackerDetailsRequest): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(EmailTrackerApiUrlConstant.SEARCH_EMAIL_TRACKER_DETAILS, _reqBody);
    }
    GetCurrentData(_reqBody: GetRowDataRequest): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(EmailTrackerApiUrlConstant.GET_CURRENT_DATA, _reqBody);
    }
}
