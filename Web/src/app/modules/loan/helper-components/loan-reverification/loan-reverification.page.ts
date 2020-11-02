import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoanInfoService } from '../../services/loan-info.service';
import { NotificationService } from '@mts-notification';
import { SessionHelper } from '@mts-app-session';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { AppSettings } from '@mts-app-setting';
@Component({
  selector: 'mts-loan-reverification',
  templateUrl: 'loan-reverification.page.html',
  styleUrls: ['loan-reverification.page.css']
})

export class LoanReverificationComponent implements OnInit, OnDestroy {
  @ViewChild('confirmDeleteModal') confirmDeleteModal: ModalDirective;
  @ViewChild('chooseReverify') _chooseReverify: ModalDirective;
  @ViewChild('reverificationModal') _reverificationModal: ModalDirective;
  showHide: any = [false, false, false];
  ReverificationMaster: any[] = [];
  initReverification: { Download: boolean, LoanReverificationID: any, RevericationName: any, UserRevericationName: any }[] = [];
  ReverificationID: any = 0;
  ReverificationMappingID: any = 0;
  // reverificationGuid: any;
  _reverifyDownload: { Download: boolean, LoanReverificationID: any, RevericationName: any, UserRevericationName: any } = {
    Download: false, LoanReverificationID: 0, RevericationName: '', UserRevericationName: ''
  };
  _userReverificationName = '';
  _isCoverLetter = false;
  _availableReverifiyDocs: any[] = [];
  _missingReverifiyDocs: any[] = [];
  showDivTemplate = false;
  disabledDownloadInit = false;
  disableNext = true;
  disablePrevious = true;
  showDocument = false;
  DocumentIndex = 0;
  // mentionDocTypes: any[] = [];
  // mentionDocFields: any[] = [];
  templateName = '';
  Fields: any = {};

  constructor(
    private _loanInfoService: LoanInfoService,
    private _notificationService: NotificationService) { }

  private _subscriptions: Subscription[] = [];
  private _reverifyObj: any;

  ngOnInit() {
    this.checkPermission('ReadonlyLoans', 0);

    this._subscriptions.push(this._loanInfoService.LoanReverificationMasters$.subscribe((res: any[]) => {
      this.ReverificationMaster = res;
    }));

    this._subscriptions.push(this._loanInfoService.LoanReverification$.subscribe((res: { Download: boolean, LoanReverificationID: any, RevericationName: any, UserRevericationName: any }[]) => {
      this.initReverification = res;
    }));

    this._subscriptions.push(this._loanInfoService.confirmReverifyDeleteModal$.subscribe((res: boolean) => {
      res ? this.confirmDeleteModal.show() : this.confirmDeleteModal.hide();
    }));

    this._subscriptions.push(this._loanInfoService.ReverificationBlog$.subscribe((res: any) => {
      const blob = new Blob([res], { type: 'application/zip' });
      this._loanInfoService.saveDocumentPDFAs(blob, this._reverifyDownload.UserRevericationName);
      this._reverifyDownload.Download = false;
    }));

    this._subscriptions.push(this._loanInfoService.changeField$.subscribe((res: { fieldCount: any, returnFieldCount: any }) => {
      if ((res.fieldCount === res.returnFieldCount)) { this.changeField(); }
    }));

    this._loanInfoService.GetLoanBasedReverification();
    this._loanInfoService.GetReverification();
  }

  DeletedReverification() {
    this._loanInfoService.DeletedReverification(this._reverifyDownload.LoanReverificationID, this._reverifyDownload.UserRevericationName);
  }

  DownloadReverification(_reverify, index) {
    _reverify.Download = true;
    this._reverifyDownload = _reverify;
    this._loanInfoService.DownloadReverification(this._reverifyDownload.LoanReverificationID);
  }

  DeleteConfirmation(_reverify) {
    this._reverifyDownload = _reverify;
    this._loanInfoService.confirmReverifyDeleteModal$.next(true);
  }

  // SetReverificationDoc(docID, val, VersionNumber) {
  //   this._availableReverifiyDocs.forEach(element => {
  //     if (docID === element.DocumentTypeID && VersionNumber === element.Version) {
  //       element.Download = !val;
  //       return;
  //     }
  //   });
  // }

  ChooseReverifyDocuments() {
    if (this.ReverificationID !== 0) {
      this._userReverificationName = '';
      this._reverifyObj = this._loanInfoService.GetReverificationObj(this.ReverificationID);
      this.ReverificationMappingID = this._reverifyObj.MappingID;
      this._availableReverifiyDocs = [];
      this._missingReverifiyDocs = [];
      const MappedDocs = this._reverifyObj.MappedDocuments.slice();
      const inLoanDocs = this._loanInfoService.GetLoan().loanDocuments.filter(x => x.DocumentLevelIcon === 'Success');
      const _reverifyDocs = [];
      this._reverifyObj.MappedDocuments.forEach(element => {
        inLoanDocs.forEach(doc => {
          if (element.DocumentTypeID.toString() === doc.DocID && !doc.Obsolete) {
            _reverifyDocs.push(
              {
                DocumentTypeID: doc.DocID,
                DocumentName: element.DocumentName,
                DocVersion: doc.VersionNumber,
                DocNameVersion: doc.DocNameVersion,
                Version: ((this._loanInfoService.CheckMoreThanOnce(doc.DocName)) ? (doc.FieldOrderBy === '' ? (doc.VersionNumber).toString() : this._loanInfoService.GetFieldVersionNumber(doc.DocName, doc.FieldOrderVersion)) : doc.DocName),
                FieldOrderBy: doc.FieldOrderBy,
                OrderByFieldValue: doc.OrderByFieldValue,
                FieldOrderVersion: doc.FieldOrderVersion,
                Download: false
              });
          }
        });
      });

      MappedDocs.forEach(element => {
        element['Download'] = false;
      });
      this._availableReverifiyDocs = _reverifyDocs;
      this._missingReverifiyDocs = MappedDocs.filter(x => _reverifyDocs.filter(r => r.DocumentTypeID === x.DocumentTypeID.toString()).length === 0);
      _reverifyDocs.forEach(doc => {
        doc.DocumentName = (this._loanInfoService.CheckMoreThanOnce(doc.DocumentName)) ? doc.DocumentName + ' - V' + (doc.FieldOrderBy === '' ? (doc.Version).toString() : this._loanInfoService.GetFieldVersionNumber(doc.DocumentName, doc.FieldOrderVersion)) : doc.DocumentName;
      });
      this._isCoverLetter = false;
      this._chooseReverify.show();

    } else {
      this._notificationService.showError('Select Re-verification');
    }
  }

  Initiate() {
    this.DocumentIndex = 0;
    const _docIDs = this._availableReverifiyDocs.filter(a => a.Download === true).map(element => ({ DocumentID: element.DocumentTypeID, Version: element.Version }));
    const _selectedDocuments = this._availableReverifiyDocs.filter(a => a.Download === true);
    let _alreadyExists = false;
    this.initReverification.forEach(element => {
      if (this._userReverificationName === element.UserRevericationName) {
        _alreadyExists = true;
        return;
      }
    });

    if (!this._isCoverLetter && _docIDs.length === 0) {
      this._notificationService.showError('Select Atleast One Document');
    } else if (this._userReverificationName === '') {
      this._notificationService.showError('Enter Re-Verificaiton Name');
    } else if (_alreadyExists) {
      this._notificationService.showError('Re-Verificaiton Name already exists');
    } else if (this.ReverificationID > 0) {
      this.showReverification();
    }
  }

  showReverification() {
    this._chooseReverify.hide();
    this._reverificationModal.show();
    this.showDivTemplate = false;
    if (this._isCoverLetter) {
      this._reverifyObj = this._loanInfoService.GetReverificationObj(this.ReverificationID);
      let fieldCount = 0;
      const returnFieldCount = 0;
      if (isTruthy(this._reverifyObj.TemplateFieldValue)) {
        const jData = JSON.parse(this._reverifyObj.TemplateFieldValue);
        const jDataKeys = Object.keys(jData);
        jDataKeys.forEach(elementKey => {
          const str = jData[elementKey];
          let val = str.match(/\{.*?\}/g);
          if (val !== null) {
            val = val.map(function (match) { return match.slice(1, -1); });
            val.forEach(element => {
              const doc = element.split('.');
              fieldCount++;
              if (doc.length === 2) {
                this._loanInfoService.GetLoanDocumentFieldValue(jData, elementKey, str, element, returnFieldCount, fieldCount, this._reverifyObj, doc);
              }
            });
          }
        });
        this.Fields = jData;
      } else {
        fieldCount = -1;
        this.Fields = {};
        const tempFields = this._reverifyObj.TemplateFields.split(',');
        tempFields.forEach(element => {
          this.Fields[element.trim()] = '';
        });
      }
      this._loanInfoService.SetReverificationFields(this.Fields);
      if (fieldCount === -1 || fieldCount === 0) {
        this.changeField();
      }
    }
  }

  DownloadInitiated() {
    this._reverificationModal.hide();
    let jData = '';
    if (this._isCoverLetter) {
      jData = JSON.stringify(this.Fields);
    }
    const _chooseDocIDs = this._availableReverifiyDocs.filter(a => a.Download === true);
    const _docIDs = [];
    _chooseDocIDs.forEach(element => {
      if (element.Download) {
        _docIDs.push({ DocumentID: element.DocumentTypeID, Version: element.DocVersion });
      }
    });
    const req = {
      TableSchema: AppSettings.TenantSchema,
      LoanID: this._loanInfoService.GetLoan().LoanID,
      ReverificationMappingID: this.ReverificationMappingID,
      ReverificationID: this.ReverificationID,
      UserID: SessionHelper.UserDetails.UserID,
      TemplateFieldJson: jData,
      IsCoverLetterReq: this._isCoverLetter,
      RequiredDocIDs: JSON.stringify(_docIDs),
      ReverificationName: this._userReverificationName
    };
    this._loanInfoService.DownloadInitReverification(req);
  }

  GetNextDocument() {
    if (this.DocumentIndex >= 0 && this.showDocument) {
      this.DocumentIndex += 1;
    }
    const _selectedDocuments = this._availableReverifiyDocs.filter(a => a.Download === true);
    const doc = _selectedDocuments[this.DocumentIndex];
    doc.lastPageNumber = 0;
    doc.pageNumberArray = [];
    doc.checkListState = false;
    this._loanInfoService.SetLoanReverifyViewDocument(doc);
    setTimeout(() => {
      this._loanInfoService.RevifyShowDocumentDetailView$.next(true);
      this._loanInfoService.LoanReverifyImageViewerHeight$.next('calc(100vh - 240px)');
    }, 300);

    if (this.DocumentIndex === (_selectedDocuments.length - 1)) {
      this.disableNext = true;
    }

    if (this.DocumentIndex === 0 && !this._isCoverLetter) {
      this.disablePrevious = true;
    } else {
      this.disablePrevious = false;
    }

    this.showDocument = true;
  }

  GetPreviousDocument() {
    this.DocumentIndex = this.DocumentIndex - 1;
    if (this.DocumentIndex === -1 && this._isCoverLetter) {
      this.showDocument = false;
      this.disableNext = false;
      this.disablePrevious = true;
      this.DocumentIndex = 0;
    }
    const _selectedDocuments = this._availableReverifiyDocs.filter(a => a.Download === true);
    const doc = _selectedDocuments[this.DocumentIndex];
    doc.lastPageNumber = 0;
    doc.pageNumberArray = [];
    doc.checkListState = false;
    this._loanInfoService.SetLoanReverifyViewDocument(doc);
    setTimeout(() => {
      this._loanInfoService.RevifyShowDocumentDetailView$.next(true);
      this._loanInfoService.LoanReverifyImageViewerHeight$.next('calc(100vh - 240px)');
    }, 300);
  }

  changeField() {
    const AssignedDocFieldTypes = [];
    this._reverifyObj.MappedDocuments.map(e => e.DocumentName).forEach(doc => {
      const reField = this._reverifyObj.MappedDocuments.slice().filter(d => d.DocumentName === doc);
      let reDocFields = [];
      if (isTruthy(reField) && reField.length > 0) {
        reDocFields = reField[0].DocumentFields;
      }
      reDocFields.forEach(element => {
        AssignedDocFieldTypes.push({ DocID: doc, DocFieldName: element });
      });
    });
    this.templateName = this._reverifyObj.TemplateFileName;
    setTimeout(() => {
      this._loanInfoService.mentionDropOptions$.next({ mentionDocTypes: this._reverifyObj.MappedDocuments.map(e => e.DocumentName).slice(), mentionDocFields: AssignedDocFieldTypes.slice() });
      this._loanInfoService._mappingTemplate$.next(this._reverifyObj.LogoGuid);
    }, 400);
    this.showDivTemplate = true;
    this.disabledDownloadInit = false;
    const _selectedDocuments = this._availableReverifiyDocs.filter(a => a.Download === true);
    this.disableNext = (_selectedDocuments.length === 0);
    if (_selectedDocuments.length === 0) {
      this.disablePrevious = true;
    }

    this.showDocument = false;
  }

  checkPermission(component: string, index: number): void {
    let URL = '';
    if (index === 1 || index === 2) {
      URL = 'View\\LoanSearch\\LoanInfo\\' + component;
    } else {
      URL = 'View\\LoanDetails\\' + component;
    }

    const AccessCheck = false;
    const AccessUrls = SessionHelper.RoleDetails.URLs;
    if (AccessCheck !== null) {
      AccessUrls.forEach(element => {
        if (element.URL === URL) {
          this.showHide[index] = true;
          return false;
        }
      });
    }
  }

  CheckPermissionsforRoles(URL: any): boolean {
    let AccessCheck = false;
    const AccessUrls = SessionHelper.RoleDetails.URLs;
    if (AccessCheck !== null) {
      AccessUrls.forEach(elts => {
        if (elts.URL === URL) {
          AccessCheck = true;
          return false;
        }
      });
      return AccessCheck;
    }
  }

  ngOnDestroy(): void {
    this._subscriptions.forEach(element => {
      element.unsubscribe();
    });
  }
}
