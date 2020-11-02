import { SmtpSaveRequestModel } from './models/smtp-request-data.model';
import { AppConfigApiUrlConstant } from './../../shared/constant/api-url-constants/application-config-api-url.constant';
import { APIService } from './../../shared/service/api.service';
import { MTSAPIResponse } from '@mts-api-response-model';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { AddUpdateStipulationModel } from './models/stipulation.model';
import { ConfigRequestModel } from './models/config-request.model';
import { ReportMasterRequestModel } from './models/report-master-request.model';
import { ConfigTypeRequestModel } from './models/config-value.model';
import { UpdateLoanSearchModel } from './models/custom-loan-search.model';
import { UpdateAuditModel } from './models/save-audit-config.model';
import {
  GetBoxTokenModel,
  CheckBoxTokenModel,
  GetBoxValueModel,
} from './models/box-setting.model';
import { ConfigAllRequestModel } from './models/get-all-configtype-request.model';
import { SaveCategorymodel, UpdateCategorymodel } from './models/category-list.model';
@Injectable()
export class ApplicationConfigDataAccess {
  constructor(private _apiService: APIService) { }
  GetAllTenantConfigTypes(
    pmInputReq: ConfigAllRequestModel
  ): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.GET_ALL_CONFIG_TYPES,
      pmInputReq
    );
  }
  GetLoanSearchFilterConfigValue(
    pmInputReq: ConfigTypeRequestModel
  ): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.GET_LOANSEARCH_CONFIG_DATA,
      pmInputReq
    );
  }
  UpdateLoanSearchFilterStatus(
    pmInputReq: UpdateLoanSearchModel
  ): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.UPDATE_LOANSEARCH_CONFIG_DATA,
      pmInputReq
    );
  }
  GetStipulationList(
    pmInputReq: ConfigRequestModel
  ): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.GET_STIPULATION_LIST,
      pmInputReq
    );
  }
  UpdateStipulationData(
    pmInputReq: AddUpdateStipulationModel
  ): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.UPDATE_STIPULATION_DATA,
      pmInputReq
    );
  }
  SaveStipulationData(
    pmInputReq: AddUpdateStipulationModel
  ): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.SAVE_STIPULATION_DATA,
      pmInputReq
    );
  }
  GetAllCategory(pmInputReq: ConfigRequestModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.GET_CATEGORY_DATA_LIST,
      pmInputReq
    );
  }
  UpdateCategoryData(pmInputReq: UpdateCategorymodel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.UPDATE_CATEGORY_GROUP,
      pmInputReq
    );
  }
  SaveCategoryData(pmInputReq: SaveCategorymodel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.SAVE_CATEGORY_GROUP,
      pmInputReq
    );
  }
  GetAllAuditConfig(
    pmInputReq: ConfigRequestModel
  ): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.GET_AUDIT_CONFIG_DATA,
      pmInputReq
    );
  }
  UpdateAuditConfig(pmInputReq: UpdateAuditModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.UPDATE_AUDIT_CONFIG,
      pmInputReq
    );
  }

  GetPasswordPolicy(): Observable<MTSAPIResponse> {
    return this._apiService.authHttpGet(
      AppConfigApiUrlConstant.GET_PASSWORD_POLICY
    );
  }

  SavePasswordPolicy(pmInputReq: {
    PasswordPolicy: any;
  }): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.SAVE_PASSWORD_POLICY,
      pmInputReq
    );
  }
  GetBoxToken(pmInputReq: GetBoxTokenModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.GET_BOX_TOKEN,
      pmInputReq
    );
  }
  GetAllBoxSettings(
    pmInputReq: ConfigRequestModel
  ): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.GET_ALL_BOX_CONFIG,
      pmInputReq
    );
  }
  getboxvalues(pmInputReq: GetBoxValueModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.GET_BOX_CONFIG_VALUES,
      pmInputReq
    );
  }
  CheckUserBoxToken(
    pmInputReq: CheckBoxTokenModel
  ): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.CHECK_USER_BOX_TOKEN,
      pmInputReq
    );
  }
  GetReportMasterData(
    pmInputReq: ConfigRequestModel
  ): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.GET_MASTER_REPORT_DATA,
      pmInputReq
    );
  }
  GetDocsList(pmInputReq: ConfigRequestModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.GET_REPORT_DOCS_LIST,
      pmInputReq
    );
  }
  SaveEditDocs(
    pmInputReq: ReportMasterRequestModel
  ): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.SAVE_REPORT_CONFIG,
      pmInputReq
    );
  }
  DeleteReportMaster(
    pmInputReq: ReportMasterRequestModel
  ): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.DELETE_REPORT_CONFIG,
      pmInputReq
    );
  }

  GetConfigvalues(pmInputReq: any): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.GET_CONFIG_TYPE_VALUE,
      pmInputReq
    );
  }
  UpdateAppConfigData(pmInputReq: any): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.UPDATE_CONFIG_TYPE_VALUE,
      pmInputReq
    );
  }
  AddAppConfigData(pmInputReq: any): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.ADD_CONFIG_TYPE_VALUE,
      pmInputReq
    );
  }
  SaveSMTPSubmit(pmInputReq: SmtpSaveRequestModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      AppConfigApiUrlConstant.SAVE_SMTP_DETAILS,
      pmInputReq
    );
  }
  GetAllSMPTDetails(): Observable<MTSAPIResponse> {
    return this._apiService.authHttpGet(
      AppConfigApiUrlConstant.GET_SMTP_DETAILS
    );
  }
}
