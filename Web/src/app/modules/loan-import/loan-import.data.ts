import { LoanImportConstant } from './../../shared/constant/api-url-constants/loan-import-api-url.constant';
import { Injectable } from '@angular/core';
import { APIService } from 'src/app/shared/service/api.service';
import { Observable } from 'rxjs';
import { MTSAPIResponse } from '@mts-api-response-model';
import { CheckBoxTokenModel, GetBoxTokenModel } from '../application-configuration/models/box-setting.model';
import { TenantCustomerRequestModel, TenantLoanRequestModel, TenantRequestModel } from './models/tenant-request.model';
import { UpdateLoanMonitorModel, CustomerReviewLoanTypeModel, CheckCurrentUserModel, OverideLoanUserModel, DeleteLoanModel } from './models/loan.import.model';
import { LoanSearchModel } from './models/loan.search.model';
import { RetryEncompassDownloadModel, GetEphesofturlModel } from './models/retry.encompass.model';
import { BoxFileListRequestModel, FolderItemCountRequestModel } from './models/box.import.model';

@Injectable()
export class LoanImportDataAccess {
  constructor(private _apiService: APIService) {

  }
  getMissingDoc(pmInputReq: TenantLoanRequestModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.GET_MISSING_LN_DOC,
      pmInputReq
    );
  }
  AssignLoanTypes(pmInputReq: CustomerReviewLoanTypeModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.CUSTOMER_REVIEW_LN_MAPPING,
      pmInputReq
    );
  }
  updateLoanMonitor(pmInputReq: UpdateLoanMonitorModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.UPDATE_LN_MONITOR,
      pmInputReq
    );
  }
  checkCurrentUser(pmInputReq: CheckCurrentUserModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.CHECK_LN_USER,
      pmInputReq
    );
  }
  setLoanPickUpUser(pmInputReq: OverideLoanUserModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.SET_LN_PICKUP_USER,
      pmInputReq
    );
  }
  retryFileUpload(pmInputReq: TenantLoanRequestModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.RETRY_FILE_UPLOAD,
      pmInputReq
    );
  }
  getUploadedItems(pmInputReq: LoanSearchModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.GET_UPLOADED_ITEMS,
      pmInputReq
    );
  }
  retryDownloadEncomLn(pmInputReq: RetryEncompassDownloadModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.RETRY_ENCOMPASS_LN_DOWNLD,
      pmInputReq
    );
  }
  getEphesofturl(pmInputReq: GetEphesofturlModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.GET_EPHESOFT_URL,
      pmInputReq
    );
  }
  DeleteLoans(pmInputReq: DeleteLoanModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.DELETE_LN,
      pmInputReq
    );
  }
  CheckUserBoxToken(pmInputReq: CheckBoxTokenModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.CHECK_USER_BOX_TOKEN,
      pmInputReq
    );
  }
  GetBoxToken(pmInputReq: GetBoxTokenModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.GET_BOX_TOKEN,
      pmInputReq
    );
  }
  getBoxFileList(pmInputReq: BoxFileListRequestModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.GET_FILE_LIST,
      pmInputReq
    );
  }
  getPriorityList(pmInputReq: TenantRequestModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.GET_REVIEW_PRIORITY_MASTER,
      pmInputReq
    );
  }
  customerReviewLoanType(pmInputReq: CustomerReviewLoanTypeModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.CUSTOMER_REVIEW_LN_MAPPING,
      pmInputReq
    );
  }
  customerReviewType(pmInputReq: TenantCustomerRequestModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.CUSTOMER_REVIEW_TYPE,
      pmInputReq
    );
  }
  GetFolderItemCount(pmInputReq: FolderItemCountRequestModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.GET_ITEMS_COUNT,
      pmInputReq
    );
  }
  UploadBoxFile(pmInputReq: any): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanImportConstant.UPLOAD_BOX_FILE,
      pmInputReq
    );
  }
}
