import { Injectable } from '@angular/core';
import { APIService } from 'src/app/shared/service/api.service';
import { Observable } from 'rxjs';
import { MTSAPIResponse } from '@mts-api-response-model';
import { WorkQueueApiUrlConstant } from 'src/app/shared/constant/api-url-constants/workqueue-api-url.constant';
import { WorkQueueModel } from './models/workqueue.model';
@Injectable()
export class WorkQueueDataAccess {
  constructor(private _apiService: APIService) { }

  searchSubmit(req: WorkQueueModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      WorkQueueApiUrlConstant.WORK_QUEUE_LOANS,
      req
    );
  }

}
