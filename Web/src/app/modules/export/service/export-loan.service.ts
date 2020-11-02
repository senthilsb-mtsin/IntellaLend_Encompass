import { Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Injectable } from '@angular/core';
import { NotificationService } from '@mts-notification';
import { ExportDataAccess } from '../export.data';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { AppSettings } from '@mts-app-setting';
import { SessionHelper } from '@mts-app-session';
import { SaveBatchModel } from '../models/save-batch.model';
import { LoanSearchRequestModel } from '../../loansearch/models/loan-search-request.model';
const jwtHelper = new JwtHelperService();
@Injectable()
export class ExportLoanService {
  showmodel$ = new Subject();
  setNextStep$ = new Subject();
  documentcount$ = new Subject();
  SetSelectedLoanData$ = new Subject();
  searchData$ = new Subject();
  selectAllDocBtn$ = new Subject();
  JobID: any;
  JobName: any = '';
  ExportedBy: any = '';
  CustomerID: any;
  CoverLetter = false;
  TableOfContent = false;
  PasswordProtected = false;
  Password: any = '';
  ConfirmPassword: any = '';
  CoverLetterContent: any = '';
  LoanDetail: any = [];
  loanExportSteps = { LoanSelect: 1, DocumentSelect: 2, 'Configuration': 3 };
  loan: any;
  constructor(private _exportdata: ExportDataAccess, private _notificationservice: NotificationService) {
  }
  private selectedLoanData: any = [];
  private _searchData: any;
  GetLoanDocuments(input: any) {
    return this._exportdata.GetLoanDocuments(input).subscribe(
      res => {
        if (res !== null) {
          this.loan = jwtHelper.decodeToken(res.Data)['data'];
          this.LoanDetail.forEach(dc => {
            if (dc.LoanID === input.LoanID) {
              dc.CurrentLoan = true;
              this.loan.loanDocuments.forEach(element => {
                if (element.DocumentLevelIcon === 'Success' && element.Obsolete === false) {
                  dc.DocumentDetails.push({
                    IsChecked: false,
                    DocumentTypeId: element.DocID,
                    VersionNumber: element.VersionNumber,
                    DocumentTypeName:
                      (this.CheckMoreThanOnce(element.DocName) ? element.DocName + ' -V' + (element.FieldOrderBy === '' ? (element.VersionNumber).toString()
                        : this.GetFieldVersionNumber(element.DocName, element.FieldOrderVersion)) : element.DocName)
                  });
                }
              });
              this.documentcount$.next(dc.DocumentDetails.length > 0);
            }
          });
        }
      });
  }
  VersionChangeFUnction(DocumentName, FieldOrderBy, VersionNUmber, FieldOrderVersion) {
    const _checkdocval = this.CheckMoreThanOnce(DocumentName);
    if (_checkdocval) {
      if (FieldOrderBy !== '') {
        DocumentName = DocumentName + ' - V' + VersionNUmber;
        return DocumentName;
      } else {
        DocumentName = DocumentName + ' - V' + this.GetFieldVersionNumber(DocumentName, VersionNUmber);
        return DocumentName;
      }
    } else {
      return DocumentName;
    }
  }
  CheckMoreThanOnce(docName) {
    if (!isTruthy(this.loan) || !isTruthy(this.loan.loanDocuments)) { return false; }
    return this.loan.loanDocuments.filter(l => l.DocName === docName).length > 1;

  }
  GetFieldVersionNumber(docName, currentVersion): string {
    if (!isTruthy(this.loan) || !isTruthy(this.loan.loanDocuments)) { return ''; }
    const docs = this.loan.loanDocuments.filter(l => l.DocName === docName);
    return docs.length !== 1 ? currentVersion.toString() : '1';

  }
  RemoveLoanList(LoanId: any, _index: any) {
    const index = this.LoanDetail.findIndex(l => l.LoanID === LoanId);
    this.LoanDetail.splice(index, 1);
    const selectedindex = this.selectedLoanData.findIndex(l => l.LoanID === LoanId);
    this.selectedLoanData.splice(selectedindex, 1);
  }
  SetSelectedLoanData(inputData: any) {
    this.selectedLoanData = inputData;
  }

  GetSelectedLoanData(): void {
    return this.selectedLoanData.slice();
  }
  loanvalidate() {
    if (this.selectedLoanData.length > 0) {
      const _docTypeList = [];
      const rows = [...this.selectedLoanData];
      for (let i = 0; i < rows.length; i++) {
        if (this.LoanDetail.length > 0) {
          const checkExistLoan = this.LoanDetail.filter(l => l.LoanID === rows[i].LoanID);
          if (typeof checkExistLoan !== undefined && checkExistLoan.length === 0) {
            this.LoanDetail.push({
              LoanID: rows[i].LoanID,
              LoanNumber: isTruthy(rows[i].LoanNumber) ? rows[i].LoanNumber : rows[i].LoanID
              , IsSelected: false,
              Documents: '',
              CurrentLoan: false,
              DocumentDetails: []
            });
          }
        } else {
          // need to check
          // this._selectDocuments = '';
          this.LoanDetail.push({
            LoanID: rows[i].LoanID,
            LoanNumber: isTruthy(rows[i].LoanNumber) ? rows[i].LoanNumber : rows[i].LoanID
            , IsSelected: false,
            Documents: '',
            CurrentLoan: false,
            DocumentDetails: []
          });
        }
      }
      const loan = this.GetSelectedLoanData();
      this.LoanDetail = this.removeMainArray(this.LoanDetail, loan);
      return true;
    } else {
      this._notificationservice.showError('Atleast Select one loan');
    }
    return false;
  }
  removeMainArray( mainArray: any, compareArray: any) {
    return mainArray.filter(function (objFromA) {
      return compareArray.find(function (objFromB) {
          return objFromA.LoanID === objFromB.LoanID;
        });
      });
  }
  SaveDocuments() {
    const DocArry = [];
    let docstring = [];
    const LoanDocCount = [];
    this.LoanDetail.forEach(element => {
      const detail = element.DocumentDetails.filter(d => d.IsChecked === true);
      if (typeof detail !== undefined && detail.length > 0) {
        docstring = [];
        detail.forEach(dd => {
          docstring.push(
            {
              DocumentTypeId: dd.DocumentTypeId,
              VersionNumber: dd.VersionNumber
            });
        });
        element.Documents = JSON.stringify(docstring);
        LoanDocCount.push(element.LoanID);
      }
    });
    if (this.LoanDetail.length <= 0) {

      this._notificationservice.showError('Select Atleast One Loan');
    } else if (LoanDocCount.length <= 0) {
      this._notificationservice.showError('Select Atleast one Document');
    } else if (LoanDocCount.length === this.LoanDetail.length) {
      return true;
    } else {
      this._notificationservice.showError('Document is not mapped with selected loan');
    }
    return false;
  }
  SaveBatchDetails() {
    const docdetails = this.LoanDetail;
    const requestData = new SaveBatchModel(
      AppSettings.TenantSchema,
      this.JobName,
      SessionHelper.UserDetails.UserID,
      this.CustomerID,
      this.CoverLetter,
      this.TableOfContent,
      this.PasswordProtected,
      this.Password,
      this.CoverLetterContent,
      docdetails
    );
    this._exportdata.SaveBatchDetails(requestData).subscribe(
      res => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          if (result === true) {
            this._notificationservice.showSuccess('Job Created Successfully');
            this.LoanDetail = [];
            this.JobName = '';
            this.CoverLetter = false;
            this.CustomerID = 0;
            this.TableOfContent = false;
            this.PasswordProtected = false;
            this.Password = '';
            this.CoverLetterContent = '';
          }
        }
      });
  }
  searchSubmit(req: LoanSearchRequestModel) {
    return this._exportdata.searchSubmit(req).subscribe((res) => {
      const loanData = jwtHelper.decodeToken(res.Data)['data'];
      this._searchData = loanData;
      this.searchData$.next(this._searchData.slice());
    });
  }
  validatedocument() {
    if (isTruthy(this.LoanDetail) && this.LoanDetail.length > 0) {
    this.LoanDetail.forEach(element => {
      if (element.CurrentLoan === true) {
        if (isTruthy(element.DocumentDetails) && element.DocumentDetails.length > 0 ) {
          this.documentcount$.next(true);
        const check = element.DocumentDetails.filter(x => x.IsChecked === false).length > 0 ? true : false;
          this.selectAllDocBtn$.next(check);
        } else {
          this.documentcount$.next(false);
        }
      } else {
        element.CurrentLoan = false;
      }
    });
  }
  }
}
