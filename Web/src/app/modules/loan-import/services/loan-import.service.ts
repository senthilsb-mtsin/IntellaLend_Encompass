import { LoanImportDataAccess } from './../loan-import.data';
import { Subject, BehaviorSubject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Injectable } from '@angular/core';
import { NotificationService } from '@mts-notification';
import { EphesoftStatusConstant, AppSettings } from '@mts-app-setting';
import { SessionHelper } from '@mts-app-session';
import { CheckBoxTokenModel, GetBoxTokenModel } from '../../application-configuration/models/box-setting.model';
import { OverideLoanUserModel, CheckCurrentUserModel, CustomerReviewLoanTypeModel, UpdateLoanMonitorModel, DeleteLoanModel } from '../models/loan.import.model';
import { TenantCustomerRequestModel, TenantLoanRequestModel, TenantRequestModel } from '../models/tenant-request.model';
import { RetryEncompassDownloadModel, GetEphesofturlModel } from '../models/retry.encompass.model';
import { LoanSearchModel } from '../models/loan.search.model';
import { BoxFileListRequestModel, FolderItemCountRequestModel } from '../models/box.import.model';
import { OrderByPipe } from '@mts-pipe';
import { LoanInfoService } from '../../loan/services/loan-info.service';
import { Router } from '@angular/router';
const jwtHelper = new JwtHelperService();
@Injectable()
export class LoanImportService {
  enableLoanMonitor$ = new BehaviorSubject(false);
  loanimportdata$ = new Subject();
  ActiveLoanTypes$ = new Subject();
  isAssignLoanType$ = new Subject();
  isRetryEncompass$ = new Subject();
  isDeleteLoan$ = new Subject();
  isconfirmModal$ = new Subject();
  reviewTypeItems$ = new Subject();
  loanTypeItems$ = new Subject();
  priorityList$ = new Subject();
  CheckUserBoxToken$ = new Subject();
  GetFolderItemCount$ = new Subject();
  boxfilelist$ = new Subject();
  _missingDocDTable$ = new Subject();
  isuploadbox$ = new Subject();
  reviewTypeItems: any[];
  loanTypeItems: any[];

  constructor(private _loanImportDataAccess: LoanImportDataAccess,
    private _notificationService: NotificationService,
    private orderBy: OrderByPipe,
    private _loanService: LoanInfoService,
    private _route: Router) {
    this.userDetails = SessionHelper.UserDetails;
  }
  private currentLoanID: Number = 0;
  private currentRow: any;
  private userDetails: any;
  getMissingDoc(input: TenantLoanRequestModel) {
    return this._loanImportDataAccess.getMissingDoc(input).subscribe(
      res => {
        if (res !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          if (data !== null) {
            this._missingDocDTable$.next(data);
          }
        }
      }
    );
  }
  AssignLoanTypes(input: CustomerReviewLoanTypeModel) {
    return this._loanImportDataAccess.AssignLoanTypes(input).subscribe(
      res => {
        if (res !== null) {
         const data = jwtHelper.decodeToken(res.Data)['data'];
          if (data !== null) {
            this.ActiveLoanTypes$.next(data);
          }
        }
      }
    );

  }
  updateLoanMonitor(input: UpdateLoanMonitorModel) {
    this._loanImportDataAccess.updateLoanMonitor(input).subscribe(
      res => {
        if (res !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          if (data !== null && data) {

            this._notificationService.showSuccess('Mapped Successfully and Loan Reinitiated');
            //  this.TriggerSearch();
            this.isAssignLoanType$.next(true);

          } else {
            this._notificationService.showError('Mapping Failed');
          }

        }
      }
    );
  }
  checkCurrentUser(row: any) {
    row['CurrentUserID'] = this.userDetails.UserID;
    const inputReq = new CheckCurrentUserModel(AppSettings.TenantSchema, row.LoanID, this.userDetails.UserID);
    return this._loanImportDataAccess.checkCurrentUser(inputReq).subscribe(
      res => {
        if (res !== null) {

          const result = jwtHelper.decodeToken(res.Data)['data'];
          if (result.CurrentUser) {
            this.routeLoanInfo(row);
          } else {
            this.alertLoanPicked(row, result.LoggerUserName);
          }
        }
      }
    );
  }
  // setLoanPickUpUser(input: any) {

  // }
  overrideLoanUser() {
    if (this.currentLoanID !== 0) {
      const inputReq = new OverideLoanUserModel(AppSettings.TenantSchema, this.currentRow.LoanID, this.userDetails.UserID);
      return this._loanImportDataAccess.setLoanPickUpUser(inputReq).subscribe(
        res => {
          if (res !== null) {
            this.setDataAndRoute(this.currentRow);
          }
        }
      );

    } else {
      console.log('Loan not selected');
    }

  }
  retryFileUpload(input: TenantLoanRequestModel) {
    return this._loanImportDataAccess.retryFileUpload(input).subscribe(
      res => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          if (result) {
            //   this._searchBtn.nativeElement.click();
            this._notificationService.showSuccess('Retry Initated');
          }
        }
      }
    );
  }
  getUploadedItems(input: LoanSearchModel) {
    return this._loanImportDataAccess.getUploadedItems(input).subscribe(
      res => {
        if (res !== null) {
          const loanimportdata = jwtHelper.decodeToken(res.Data)['data'];
          this.loanimportdata$.next(loanimportdata);
        }
      }
    );
  }
  retryDownloadEncomLn(input: RetryEncompassDownloadModel) {
    return this._loanImportDataAccess.retryDownloadEncomLn(input).subscribe(
      res => {
        if (res !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          if (data !== null) {
            if (data === true) {
              this.isRetryEncompass$.next();
              this._notificationService.showSuccess('Status Successfully Updated');
            }
          }
        }
      });

  }
  getEphesofturl(input: GetEphesofturlModel) {
    return this._loanImportDataAccess.getEphesofturl(input).subscribe(res => {
      if (res !== null) {

        const result = jwtHelper.decodeToken(res.Data)['data'];
        if (result !== null) {
          if ((result.Ephesoft_Status === EphesoftStatusConstant.READY_FOR_REVIEW || result.Ephesoft_Status === EphesoftStatusConstant.READY_FOR_VALIDATION) && result.Ephesoft_lock_owner === 'false') {
            // this.EphesoftValue = result.Ephesoft_URL;
            window.open(result.Ephesoft_URL, '_blank');
          } else if (result.Ephesoft_Status === EphesoftStatusConstant.FINISHED) {
            this._notificationService.showSuccess('Batch FINISHED Successfully');
          } else if (result.Ephesoft_Status === EphesoftStatusConstant.ERROR) {
            this._notificationService.showError('Batch is in ERROR Status');
          } else if (result.Ephesoft_Status === EphesoftStatusConstant.DELETED) {
            this._notificationService.showError('Batch is in DELETE Status');
          } else if (result.Ephesoft_lock_owner === 'true') {
            this._notificationService.showError('Locked by another user');
          } else {
            this._notificationService.showError('Batch is not available');
          }
        }

      }
    });
  }
  DeleteLoans(input: DeleteLoanModel) {
    return this._loanImportDataAccess.DeleteLoans(input).subscribe(res => {
      if (res !== null) {
        const data = jwtHelper.decodeToken(res.Data)['data'];
        if (data) {
          this.isDeleteLoan$.next();
          this._notificationService.showSuccess('Loan Deleted Successfully');
        }
      }
    });
  }
  customerReviewType(inputData: TenantCustomerRequestModel) {
    return this._loanImportDataAccess.customerReviewType(inputData).subscribe(
      res => {
        if (res !== null) {

          const Result = jwtHelper.decodeToken(res.Data)['data'];
          if (Result !== null) {
            this.reviewTypeItems = [];
            if (Result.length > 0) {
              Result.forEach(element => {
                this.reviewTypeItems.push({ id: element.ReviewTypeID, text: element.ReviewTypeName, priority: element.ReviewTypePriority });
              });
              this.orderBy.transform(this.reviewTypeItems, { property: 'text', direction: 1 });
              this.reviewTypeItems$.next(this.reviewTypeItems);
            }
          }
        } else {
          this._notificationService.showError(res['response-message']['message-desc']);
        }
      }
    );
  }
  customerReviewLoanType(inputData: CustomerReviewLoanTypeModel) {
    return this._loanImportDataAccess.customerReviewLoanType(inputData).subscribe(
      res => {
        if (res !== null) {
          const Result = jwtHelper.decodeToken(res.Data)['data'];
          this.loanTypeItems = [];
          if (Result !== null) {
            if (Result.length > 0) {

              Result.forEach(element => {
                this.loanTypeItems.push({ id: element.LoanTypeID, text: element.LoanTypeName, priority: element.LoanTypePriority });
              });
              this.orderBy.transform(this.loanTypeItems, { property: 'text', direction: 1 });
              this.loanTypeItems$.next(this.loanTypeItems);
            }
          }
        }
      }
    );
  }
  getBoxFileList(inputData: BoxFileListRequestModel) {
    return this._loanImportDataAccess.getBoxFileList(inputData).subscribe(
      res => {
        if (res !== null) {

          const result = jwtHelper.decodeToken(res.Data)['data'];
          this.boxfilelist$.next(result);
        }
      }
    );

  }
  getPriorityList(inputData: TenantRequestModel) {
    return this._loanImportDataAccess.getPriorityList(inputData).subscribe(
      res => {
        if (res !== null) {
          const Result = jwtHelper.decodeToken(res.Data)['data'];
          if (Result !== null) {
            if (Result.length > 0) {
              this.priorityList$.next(Result);
            }
          }
        }
      }
    );
  }
  CheckUserBoxToken(inputRequest: CheckBoxTokenModel) {
    return this._loanImportDataAccess.CheckUserBoxToken(inputRequest).subscribe(
      (res) => {
        if (res !== null) {

          const Result = jwtHelper.decodeToken(res.Data)['data'];
          this.CheckUserBoxToken$.next(Result);

        }
      }
    );
  }
  GetBoxToken(inputRequest: GetBoxTokenModel) {
    return this._loanImportDataAccess.GetBoxToken(inputRequest).subscribe(
      (res) => {
        const Result = jwtHelper.decodeToken(res.Data)['data'];
        if (Result !== null) {
          window.location.href = 'view/loanimport';
        }

      }
    );
  }
  GetBoxTokenImport(inputRequest: GetBoxTokenModel) {
    return this._loanImportDataAccess.GetBoxToken(inputRequest).subscribe((res) => {
      const Result = jwtHelper.decodeToken(res.Data)['data'];
      if (Result !== null) {
        window.location.href = 'view/loanimport';
      }
    });
  }
  UploadBoxFile(inputRequest: any) {
    return this._loanImportDataAccess.UploadBoxFile(inputRequest).subscribe(
      (res) => {
        const Result = jwtHelper.decodeToken(res.Data)['data'];
        if (Result) {
          this._notificationService.showSuccess('Selected files are sent to uploaded Queue');
          this.isuploadbox$.next();
        }
      }
    );
  }
  GetFolderItemCount(inputRequest: FolderItemCountRequestModel) {
    return this._loanImportDataAccess.GetFolderItemCount(inputRequest).subscribe(
      (res) => {
        const Result = jwtHelper.decodeToken(res.Data)['data'];
        if (Result !== null) {
          let input;
          const duplicateFolders = [];
          let duplicateFileNames = [];
          const itemCount = jwtHelper.decodeToken(res.Data)['data'];
          if (itemCount[0].IsNonDupFileAdded) {
            this._notificationService.showSuccess('Non Duplicates File(s) are Successfully Uploaded');
          }
          if (itemCount[0].BoxDuplicateFileList[0].FolderFilesCount.length > 0 || itemCount[0].BoxDuplicateFileList[0].FilesExistsCount.length) {
            duplicateFileNames = itemCount[0].BoxDuplicateFileList[0].FolderFilesCount.concat(itemCount[0].BoxDuplicateFileList[0].FilesExistsCount);
            const FolderandFiles: any = [];
            for (let j = 0; j < duplicateFileNames.length; j++) {
              if (duplicateFileNames[j].FileType === 'file') {

                FolderandFiles.push(duplicateFileNames[j]);
                FolderandFiles[FolderandFiles.length - 1].Type = 'file';

              } else {
                if (!(duplicateFolders.includes(itemCount[0].BoxDuplicateFileList[0].FolderFilesCount[j].FilePath))) {
                  FolderandFiles.push(duplicateFileNames[j]);
                  const FilesArray = (itemCount[0].BoxDuplicateFileList[0].FolderFilesCount[j].FilePath).split('/');
                  const FileName = FilesArray[FilesArray.length - 1];
                  FolderandFiles[FolderandFiles.length - 1].Id = itemCount[0].BoxDuplicateFileList[0].FolderFilesCount[j].FolderID;
                  FolderandFiles[FolderandFiles.length - 1].FileName = FileName;
                  FolderandFiles[FolderandFiles.length - 1].Type = 'folder';
                  duplicateFolders.push(itemCount[0].BoxDuplicateFileList[0].FolderFilesCount[j].FilePath);
                }
              }

            }
            input = { 'isTable': true, 'FolderandFiles': FolderandFiles, 'itemCount': itemCount };
          } else {
            input = { 'isTable': false, 'FolderandFiles': [], 'itemCount': itemCount };

          }
          this.GetFolderItemCount$.next(input);
        }
      }
    );
  }
  routeLoanInfo(row: any) {
    this.setLoanPickUpUser(row);
    this.setDataAndRoute(row);
  }

  setLoanPickUpUser(row: any) {
    const inputReq = { TableSchema: AppSettings.TenantSchema, LoanID: row.LoanID, PickUpUserID: row.CurrentUserID };
    return this._loanImportDataAccess.setLoanPickUpUser(inputReq).subscribe(
      res => {
        if (res !== null) { }
      },
    );
  }

  setDataAndRoute(row: any) {
    if (row.ServiceTypeName === undefined) {
      row['ServiceTypeName'] = row.ReviewType;
      row['ReceivedDate'] = row.Uploaded;
     // row['AuditDueDate'] = row.AuditMonthYearDate;
    }
    this._loanService.SetLoanPageInfo(row);
    this._route.navigate(['view/loandetails']);
    this.enableLoanMonitor$.next(true);
  }
  alertLoanPicked(row: any, loggedUserName: string) {
    const LoanAlertMessage = 'This Loan is currently operated by : <b>' + loggedUserName + '</b>. Do you still want to view ?';
    this.currentLoanID = row.LoanID;
    this.currentRow = row;
    this.isconfirmModal$.next(LoanAlertMessage);

  }
}
