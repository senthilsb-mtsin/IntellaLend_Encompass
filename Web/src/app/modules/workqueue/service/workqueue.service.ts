import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Subject } from 'rxjs';
import { WorkQueueSearchModel } from '../models/workqueue.search.model';
import { WorkQueueDataAccess } from '../workqueue.data';
import { WorkQueueModel } from '../models/workqueue.model';
const jwtHelper = new JwtHelperService();

@Injectable()
export class WorkQueueService {

  SearchData = new Subject<WorkQueueSearchModel[]>();
  constructor(
    private _workqueueData: WorkQueueDataAccess
  ) { }

  private _searchData: any[] = [];

  searchSubmit(req: WorkQueueModel) {
    return this._workqueueData.searchSubmit(req).subscribe((res) => {
      const data = jwtHelper.decodeToken(res.Data)['data'];
      this._searchData = data;
      this.SearchData.next(this._searchData.slice());
    });
  }
}
