import { Injectable } from '@angular/core';
import { MTSAPIResponse } from '@mts-api-response-model';
import { FannieMaeApiUrlConstant } from '@mts-api-url';
import { Observable } from 'rxjs';
import { APIService } from 'src/app/shared/service/api.service';

@Injectable()

export class FannieMaeFieldsDataAccess {
    constructor(private _api: APIService) { }
    GetFannieMaeFields(req: { TableSchema: string, LoanID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(FannieMaeApiUrlConstant.GET_FANNIEMAE_FIELDS, req);
    }

}
