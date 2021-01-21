import { Injectable } from '@angular/core';
import { MTSAPIResponse } from '@mts-api-response-model';
import { EncompassDownloadExceptionUrlConstant, FannieMaeApiUrlConstant } from '@mts-api-url';
import { Observable } from 'rxjs';
import { APIService } from 'src/app/shared/service/api.service';

@Injectable()

export class EncompassExceptionDataAccess {
    constructor(private _api: APIService) { }
    SearchEncompassExceptionDetails(req: any): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(EncompassDownloadExceptionUrlConstant.GET_ENCOMPASSEXCEPTIONDETAILS, req);
    }
    RetryEncompassException(req: any): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(EncompassDownloadExceptionUrlConstant.RETRY_ENCOMPASS_EXCEPTION, req);
    }
}
