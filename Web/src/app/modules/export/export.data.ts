import { ExportApiUrlConstant } from './../../shared/constant/api-url-constants/export-api-url.constant';
import { Injectable } from '@angular/core';
import { APIService } from 'src/app/shared/service/api.service';
import { MTSAPIResponse } from '@mts-api-response-model';
import { Observable } from 'rxjs';
import { LoanExportModel, LoanJobModel } from './models/loan-export.model';
import { TenantRequestModel } from '../loan-import/models/tenant-request.model';
import { SearchExportModel, EncompassExportModel, EncompassSearchExportModel } from './models/encompass.export.model';
import { SaveBatchModel } from './models/save-batch.model';
import { LoanSearchRequestModel } from '../loansearch/models/loan-search-request.model';
import { LosExportdateModel } from './models/los.export.date.model';
import { ReExportLOSModel } from './models/retry.los.export.model';
@Injectable()
export class ExportDataAccess {
  constructor(private _apiService: APIService) { }

  GetCurrentBatchData(pmInputReq: LoanJobModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      ExportApiUrlConstant.GET_CURRENT_JOB_DETAILS,
      pmInputReq
    );
  }
  SearchExportMonitorDetails(pmInputReq: SearchExportModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      ExportApiUrlConstant.SEARCH_EXPORT_MONITOR_DETAILS,
      pmInputReq
    );
  }
  GetExportMonitorDetails(pmInputReq: TenantRequestModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      ExportApiUrlConstant.GET_EXPORT_MONITOR_DETAILS,
      pmInputReq
    );
  }
  SearchEncompassExportDetails(pmInputReq: EncompassSearchExportModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      ExportApiUrlConstant.SEARCH_ENCOMPASS_EXPORT_DETAILS,
      pmInputReq
    );
  }
  SearchLosExportDetails(pmInputReq: LosExportdateModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      ExportApiUrlConstant.SEARCH_LOS_EXPORT_DETAILS,
      pmInputReq
    );
  }
  RetryLosExportDetails(pmInputReq: any): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      ExportApiUrlConstant.RETRY_LOS_EXPORT_DETAILS,
      pmInputReq
    );
  }
  ReExportLOSDetails(req: ReExportLOSModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      ExportApiUrlConstant.RE_EXPORT_LOS_DETAILS,
      req
    );
  }
  GetCurrentLosExportDetails(pmInputReq: any): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      ExportApiUrlConstant.GET_CURRENT_LOSEXPORT_DETAILS,
      pmInputReq
    );
  }
  GetEncompassExportDetails(pmInputReq: EncompassExportModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      ExportApiUrlConstant.GET_ENCOMPASS_EXPORT_DETAILS,
      pmInputReq
    );
  }
  RetryLoanExport(pmInputReq: LoanExportModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      ExportApiUrlConstant.RETRY_LOAN_EXPORT,
      pmInputReq
    );
  }
  DeleteBatch(pmInputReq: LoanJobModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      ExportApiUrlConstant.DELETE_BATCH,
      pmInputReq
    );
  }
  RetryEncompassUploadStaging(pmInputReq: EncompassExportModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      ExportApiUrlConstant.RETRY_ENCOMPASS_UPLOAD_STAGING,
      pmInputReq
    );
  }
  RetryEncompassExport(pmInputReq: EncompassExportModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      ExportApiUrlConstant.RETRY_ENCOMPASS_EXPORT,
      pmInputReq
    );
  }
  GetLoanDocuments(pmInputReq: any): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      ExportApiUrlConstant.GET_LOAN_DETAILS,
      pmInputReq
    );
  }
  SaveBatchDetails(pmInputReq: SaveBatchModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      ExportApiUrlConstant.SAVE_LOAN_JOB,
      pmInputReq
    );
  }
  searchSubmit(req: LoanSearchRequestModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      ExportApiUrlConstant.SEARCH_LOAN_EXPORT,
      req
    );
  }
}
