import { Injectable } from '@angular/core';
import { APIService } from 'src/app/shared/service/api.service';
import { Observable } from 'rxjs';
import { DashboardApiUrlConstant } from '@mts-api-url';
import { MTSAPIResponse } from '@mts-api-response-model';
import { DashboardReportModel } from './models/dashboard-report.model';
import { DashboardGraphRequestModel } from './models/dashboard-graph-request.model';
import { IDCWorkloadRequestModel } from './models/idc-workload-request.model';
import { IDCWorkloadDrillDownGridRequestModel } from './models/idc-workload-drilldowngrid.request.model';
import { AuditKpiGoalConfigRequestModel } from './models/audit-kpi-goalconfig-request.model';
import { DashboardGraphKPIRequestModel } from './models/kpi-dashboard-request.model';
import { CheckCurrentLoanUserRequestModel } from './models/check-current-loanuser-request.model';
import { SetLoanPickupUserRequestModel } from './models/set-loan-pickup-user-request.model';
import { ChecklistFailedLoansRequestModel } from './models/checklist-failed-loans-request.model';
import { KPIUserGroupDetailRequestModel } from './models/kpi-usergroup-detail-request.model';

@Injectable()
export class DashboardDataAccess {
    constructor(private _api: APIService) {
    }

    GetReviewTypeList(req: { TableSchema: string }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(DashboardApiUrlConstant.GET_ALL_REVIEWTYPE_MASTER, req);
    }

    GetActiveCustomersList(req: { TableSchema: string }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(DashboardApiUrlConstant.GET_ACTIVE_CUSTOMERS, req);
    }

    GetDashboardGraph(req: DashboardGraphRequestModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(DashboardApiUrlConstant.GET_DASHBOARD_GRAPH, req);
    }

    GetDashboardDatatable(req: IDCWorkloadRequestModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(DashboardApiUrlConstant.GET_DASHBOARD_GRAPH, req);
    }

    GetDashboardDrillDownGrid(req: IDCWorkloadDrillDownGridRequestModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(DashboardApiUrlConstant.GET_DRILLDOWN_GRID, req);
    }

    GetDashboardAuditKpiGoalConfigDetails(req: AuditKpiGoalConfigRequestModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(DashboardApiUrlConstant.GET_AUDIT_KPIGOALCONFIG_DETAILS, req);
    }

    GetDashboardKPIDetails(req: DashboardGraphKPIRequestModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(DashboardApiUrlConstant.GET_DASHBOARD_GRAPH, req);
    }

    GetKPIUserGroupDetails(req: KPIUserGroupDetailRequestModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(DashboardApiUrlConstant.GET_DASHBOARD_GRAPH, req);
    }

    CheckCurrentLoanUser(req: CheckCurrentLoanUserRequestModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(DashboardApiUrlConstant.CHECK_CURRENT_LOAN_USER, req);
    }

    SetLoanPickupUser(req: SetLoanPickupUserRequestModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(DashboardApiUrlConstant.SET_LOAN_PICKUP_USER, req);
    }

    GetChecklistFailedLoans(req: ChecklistFailedLoansRequestModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(DashboardApiUrlConstant.GET_DRILLDOWN_GRID, req);
    }
    GetRoleMasterList(req: { TableSchema: string }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(DashboardApiUrlConstant.GET_ROLE_LIST, req);
    }
}
