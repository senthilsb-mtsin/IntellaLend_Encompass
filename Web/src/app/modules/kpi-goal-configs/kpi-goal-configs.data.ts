import { Injectable } from '@angular/core';
import { APIService } from 'src/app/shared/service/api.service';
import { MTSAPIResponse } from 'src/app/shared/model/mts-success-response.model';
import { Observable } from 'rxjs/internal/Observable';
import { EmailTrackerApiUrlConstant } from '@mts-api-url';
import { KpiGoalConfigApiUrlConstant } from 'src/app/shared/constant/api-url-constants/kpi-goal-config-api-url.constant';

@Injectable()

export class KpiGoalConfigsDataAccess {
    constructor(private _api: APIService) { }

    GetUserRoleList(_reqBody: any): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(KpiGoalConfigApiUrlConstant.GET_USER_ROLE_LIST, _reqBody);
    }
    GetKPIGoalConfigStagingDetails(_reqBody: any): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(KpiGoalConfigApiUrlConstant.GET_KPI_GOAL_CONFIG_STAGING_DETAILS, _reqBody);
    }
    SaveKPIConfigStagingDetails(_reqBody: any): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(KpiGoalConfigApiUrlConstant.ADD_KPI_GOAL_CONFIG_STAGING_DETAILS, _reqBody);
    }
    GetAllRoles(_reqBody: any): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(KpiGoalConfigApiUrlConstant.GET_ALL_ROLE_MASTER_LIST, _reqBody);
    }

}
