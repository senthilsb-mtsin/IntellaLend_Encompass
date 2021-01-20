import { Component, ViewChild, OnInit, OnDestroy, ElementRef, HostListener } from '@angular/core';
import { LoanInfoService } from '../../services/loan-info.service';
import { Subscription } from 'rxjs';
import { Loan, Doc } from '../../models/loan-details.model';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { SessionHelper } from '@mts-app-session';
import { FileUploader, FileUploaderOptions, FileItem } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AppSettings } from '@mts-app-setting';
import { LoanHeaders } from '../../models/loan-header.model';
import { StatusConstant } from '@mts-status-constant';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { NotificationService } from '@mts-notification';
import { JwtHelperService } from '@auth0/angular-jwt';

const jwtHelper = new JwtHelperService();
@Component({
  selector: 'mts-loan-stacking-order',
  templateUrl: 'loan-stacking-order.page.html',
  styleUrls: ['loan-stacking-order.page.scss']
})
export class LoanStackingOrderComponent implements OnInit, OnDestroy {

  @ViewChild('documentObsolete') documentObsolete: ModalDirective;
  SO_textShow: any = '';
  SO_fullWidth: any = '';
  SO_sub_icon: any = '';
  SO_Icon: any = '';
  filterVal = 'In Loan';
  searchval = '';
  SO_CustHeight = '';
  FieldHeaderStyle = '';
  loan: Loan = new Loan();
  PanelShow: any = true;
  ShowDownload = false;
  showImgDiv = false;
  downloadLoanPDFLink = '';
  loanDocuments: any[] = [];
  repeatingLoanDatas: any[] = [];
  uploader: FileUploader;
  STATUS_DESCRIPTION: any = StatusConstant.STATUS_DESCRIPTION;
  IDC_STATUS_ICON: any = StatusConstant.IDC_STATUS_ICON;
  obsoleteDocId: any;
  obsoleteDocVersion: any;
  IsObsolete: boolean;
  ObsoleteDocName: any;
  ObsoleteMsg = '';
  CardHeight = '';

  constructor(
    private _loanInfoService: LoanInfoService,
    private _notificationService: NotificationService,
    private eRef: ElementRef
  ) { }
  private _subscriptions: Subscription[] = [];
  private _loanHeaderInfo: LoanHeaders;
  private currentDocID: any;
  // private mDocID: any;
  private DownloadDocumentName = '';
  private currentObselouteDoc: any;
  private foptions: FileUploaderOptions = {
    url: environment.apiURL + 'FileUpload/MissingDocFileUploader',
    authToken: 'Bearer ' + localStorage.getItem('id_token'),
    authTokenHeader: 'Authorization',
    headers: [
      { name: 'TableSchema', value: AppSettings.TenantSchema },
      { name: 'UploadFileName', value: '' },
      { name: 'UserId', value: '' },
      { name: 'DocId', value: '' },
      { name: 'LoanId', value: '' }
    ]
  };
  @HostListener('document:click', ['$event'])
  clickout(event) {
    if (this.eRef.nativeElement.contains(event.target)) {
      if (event.target.classList.contains('mDocID')) {
        this.currentDocID = parseInt(event.target.dataset['docid'], 10);
      }
    }
  }

  ngOnInit() {
    this._loanHeaderInfo = this._loanInfoService.GetLoanHeader();

    this._subscriptions.push(this._loanInfoService.LoanDetails$.subscribe((res: Loan) => {
      if (isTruthy(res) && isTruthy(res.LoanID) && res.LoanID > 0) {
        this.loan = res;
        this.GroupData();
      }
    }));

    this._subscriptions.push(this._loanInfoService.documentObsoleteModal$.subscribe((res: boolean) => {
      res ? this.documentObsolete.show() : this.documentObsolete.hide();
    }));

    this._subscriptions.push(this._loanInfoService.LoanPDFCheck$.subscribe((res: { ShowDownload: boolean, PDFLink: string }) => {
      this.ShowDownload = res.ShowDownload;
      this.loan.showDownload = res.ShowDownload;
      this.downloadLoanPDFLink = res.PDFLink;
    }));

    this._subscriptions.push(this._loanInfoService.DocumentDownloadBlog$.subscribe((res: any) => {
      const blob = new Blob([res], { type: 'application/pdf' });
      this._loanInfoService.saveDocumentPDFAs(blob, this.DownloadDocumentName);
    }));

    this._subscriptions.push(this._loanInfoService.ShowDocumentDetailView$.subscribe((res: boolean) => {
      this.showImgDiv = res;
    }));

    this._subscriptions.push(this._loanInfoService.LoanPopOutSOHeight$.subscribe((res: string) => {
      this.CardHeight = res;
    }));

    this._subscriptions.push(this._loanInfoService.SOToggle$.subscribe((res: boolean) => {
      if (res) {
        this.SOToggleShow();
      } else {
        this.SOToggleHide();
      }

      this._loanInfoService.STACKToggle$.next(true);
      setTimeout(() => {
        this._loanInfoService.ImageFit$.next(true);
      }, 300);
    }));

    this.uploader = new FileUploader(this.foptions);

    this.uploader.onBeforeUploadItem = (item: FileItem) => {
      if (this.loan.LoanID > 0 && SessionHelper.UserDetails.UserID > 0) {
        item.withCredentials = false;
        this.uploader.options.headers[1].value = item.file.name;
        this.uploader.options.headers[2].value = SessionHelper.UserDetails.UserID;
        this.uploader.options.headers[3].value = this.currentDocID;
        this.uploader.options.headers[4].value = this.loan.LoanID.toString();
        item.upload();
      } else {
        this.uploader.clearQueue();
      }
    };
    this.uploader.onCompleteItem = (item: any, response: any, status: any, headers: any) => {
      const res = JSON.parse(response);
      if (response !== null) {
        const data = jwtHelper.decodeToken(res.data)['data']['Result'];
        if (data.Result) {
          this._notificationService.showSuccess('File Uploaded Successfully');
          this.uploader.clearQueue();
          this.showUploadClick(data.DocID, '', '0', 'UpdateBack');
        } else {
          this._notificationService.showError('File Uploaded Failed!!!');
          this.uploader.queue[0].isUploaded = false;
          this.uploader.clearQueue();
        }
      } else { this._notificationService.showError('File Uploaded Failed!!!'); }
    };

    this.CheckLoanPDF();
  }

  showUploadClick(docID, docTypeName, docStatus, type) {
    if (docStatus === '0') {
      this.currentDocID = docID;
    }

    if (type === 'UpdateBack') {
      this.loan.loanDocuments.forEach(element => {
        if (element.DocID === docID.toString()) {
          element.IDCStatus = StatusConstant.MOVED_TO_IDC.toString();
        }
      });
    }
  }

  DownloadDocument(currentDoc: Doc) {
    this.DownloadDocumentName = currentDoc.DocName;
    currentDoc['Loading'] = true;
    this._loanInfoService.DownloadDocument(currentDoc);
  }

  docobsolete(currentDoc: Doc) {
    this.currentObselouteDoc = currentDoc;
    this.obsoleteDocId = currentDoc.DocID;
    this.obsoleteDocVersion = currentDoc.VersionNumber;
    this.ObsoleteDocName = currentDoc.DocName;
    this._loanInfoService.documentObsoleteModal$.next(true);
    currentDoc.Obsolete = !currentDoc.Obsolete;
    this.IsObsolete = currentDoc.Obsolete;
    this.ObsoleteMsg = currentDoc.Obsolete ? 'Obsolete' : 'Active';
  }

  getImage(doc) {
    doc.lastPageNumber = 0;
    doc.pageNumberArray = [];
    doc.checkListState = false;
    this._loanInfoService.SetLoanViewDocument(doc);
    this._loanInfoService.ShowDocumentDetailView$.next(true);
    setTimeout(() => {
      this._loanInfoService.newDocType$.next({DocID: doc.DocID, DocName: doc.DocName, DocNameVersion: doc.DocNameVersion});
      this._loanInfoService.ShowDocumentDetailView$.next(true);
    }, 300);
  }

  DocumentObsolete() {
    const tes = this.currentObselouteDoc;
    const req = { TableSchema: AppSettings.TenantSchema, LoanID: this.loan.LoanID, DocumentID: this.obsoleteDocId, DocumentVersion: this.obsoleteDocVersion, IsObsolete: this.IsObsolete, CurrentUserID: SessionHelper.UserDetails.UserID, DocName: this.ObsoleteDocName };
    this._loanInfoService.DocumentObsolete(req);
  }

  revertDocObsolete() {
    this.currentObselouteDoc.Obsolete = !this.currentObselouteDoc.Obsolete;
    this.documentObsolete.hide();
    // this._loanInfoService.GetLoanDetails();
  }

  onFileSelected() {
    this.uploader.uploadAll();
  }

  openIDCLink(BatchID) {
    this._loanInfoService.OpenIDCLink(BatchID);
  }

  GroupData() {
    this.loanDocuments = [];
    let docGroupedDatas = [];
    this.repeatingLoanDatas = [];
    this.loan.loanDocuments.forEach(element => {
      const duplength = this.repeatingLoanDatas.filter(rep => rep.VersionNumber === element.VersionNumber && rep.SequenceID === element.SequenceID).length;
      if (duplength === 0) {
        docGroupedDatas = [];
        const findGroup = this.loan.loanDocuments.filter(x => x.DocName === element.DocName);
        let loangrpDocuments = findGroup.filter(y => y.StackingGroupId > 0);
        if (loangrpDocuments.length > 0) {
          const groupName = loangrpDocuments[0].StackingOrderGroupName;
          loangrpDocuments = this.loan.loanDocuments.filter(z => z.StackingGroupId === loangrpDocuments[0].StackingGroupId);
          loangrpDocuments.forEach(arg => {
            const docs = docGroupedDatas.filter(dGData => dGData.DocName === arg.DocName && dGData.VersionNumber === arg.VersionNumber);
            if (docs.length === 0) {
              const subDocs = this.loan.loanDocuments.filter(ld => ld.DocName === arg.DocName);
              if (subDocs.length > 0) {
                subDocs.forEach(sD => {
                  docGroupedDatas.push(sD);
                });
              }
            }
          });
          if (docGroupedDatas.length > 0) {
            this.CompleteAllgroups(docGroupedDatas, groupName);
          }
        } else {
          this.loanDocuments.push(element);
          this.loanDocuments[this.loanDocuments.length - 1].StackingOrderGroupDetails = [];
        }
      }
    });

    this.loanDocuments.forEach(element => {
      if (element.StackingOrderGroupDetails.length > 0) {
        element.StackingOrderGroupDetails.forEach(gElement => {
          gElement.Loading = false;
        });
      } else {
        element.Loading = false;
      }
    });
    this._loanInfoService.SetLoanDocuments(this.loanDocuments);
  }

  CompleteAllgroups(docGroupedDatas, groupName) {
    let i = 1;
    const repeatingDatas = [];
    const MissingandCriticalDocuments = docGroupedDatas.filter(mc => mc.DocumentLevel === 'Critical' || mc.DocumentLevel === 'Non-Critical');
    docGroupedDatas.forEach(dg => {
      const duplength = repeatingDatas.filter(rep => rep.VersionNumber === dg.VersionNumber && rep.SequenceID === dg.SequenceID);
      if (duplength.length === 0) {
        repeatingDatas.push(dg);
        if (!(dg.DocumentLevel === 'Critical') && !(dg.DocumentLevel === 'Non-Critical')) {
          this.loanDocuments.push(dg);
          const tempgroupName = groupName + ' ' + (i++);
          this.loanDocuments[this.loanDocuments.length - 1].StackingOrderGroupDetails = [];
          const datas = docGroupedDatas.filter(q => q.StackingOrderFieldValue === dg.StackingOrderFieldValue);
          datas.forEach(el => {
            if (!(el.DocumentLevel === 'Critical') && !(el.DocumentLevel === 'Non-Critical')) {
              const docval = (el.IsDocName === null || el.IsDocName === false) ? ('') : (' -  ' + el.DocFieldName + ' : ' + el.DocValue);
              el.ToolTipGroup = ((this._loanInfoService.CheckMoreThanOnce(el.DocName)) ? el.DocName + ' - V' + (el.FieldOrderBy === '' ? (el.VersionNumber).toString() : this._loanInfoService.GetFieldVersionNumber(el.DocName, el.FieldOrderVersion)) : el.DocName) + docval;
              this.loanDocuments[this.loanDocuments.length - 1].StackingOrderGroupDetails.push(JSON.stringify(el));
              const len = this.loanDocuments[this.loanDocuments.length - 1].StackingOrderGroupDetails.length - 1;
              const parsedData = this.loanDocuments[this.loanDocuments.length - 1].StackingOrderGroupDetails[len];
              this.loanDocuments[this.loanDocuments.length - 1].StackingOrderGroupDetails[len] = JSON.parse(parsedData);
              this.loanDocuments[this.loanDocuments.length - 1].StackingOrderGroupDetails[len].StackingOrderGroupName = tempgroupName;
              repeatingDatas.push(el);
              this.repeatingLoanDatas.push(el);

            } else {
              repeatingDatas.push(dg);
              this.repeatingLoanDatas.push(dg);
            }
          });
          this.loanDocuments[this.loanDocuments.length - 1].IsGroup = true;
          this.loanDocuments[this.loanDocuments.length - 1].StackingOrderGroupName = tempgroupName;
          this.loanDocuments[this.loanDocuments.length - 1].StackingOrderFieldValue = dg.StackingOrderFieldValue;
        } else {
          this.repeatingLoanDatas.push(dg);
        }
      }
    });

    if (MissingandCriticalDocuments.length > 0) {
      docGroupedDatas = MissingandCriticalDocuments;
      this.loanDocuments.push(docGroupedDatas[0]);
      this.loanDocuments[this.loanDocuments.length - 1].StackingOrderGroupDetails = [];
      const tempgroupName = groupName + ' ' + (i++);
      docGroupedDatas.forEach(dg => {
        this.loanDocuments[this.loanDocuments.length - 1].StackingOrderGroupDetails.push(JSON.stringify(dg));
        const len = this.loanDocuments[this.loanDocuments.length - 1].StackingOrderGroupDetails.length - 1;
        const parsedData = this.loanDocuments[this.loanDocuments.length - 1].StackingOrderGroupDetails[len];
        this.loanDocuments[this.loanDocuments.length - 1].StackingOrderGroupDetails[len] = JSON.parse(parsedData);
        this.loanDocuments[this.loanDocuments.length - 1].StackingOrderGroupDetails[len].StackingOrderGroupName = tempgroupName;
      });
      this.loanDocuments[this.loanDocuments.length - 1].IsGroup = true;
      this.loanDocuments[this.loanDocuments.length - 1].StackingOrderGroupName = tempgroupName;
      this.loanDocuments[this.loanDocuments.length - 1].StackingOrderFieldValue = '';
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

  typeCheck(val): boolean {
    return (val === undefined);
  }

  SOToggleHide() {
    this.SO_textShow = 'SO_textHIde';
    this.SO_sub_icon = 'SO_sub_icon';
    this.SO_Icon = 'SO_Icon';
    this.SO_fullWidth = 'SO_fullWidth';
    this._loanInfoService.SO_width$.next('SO_width');
    this.SO_CustHeight = 'soCustHeight332';
    this.PanelShow = false;
    this.FieldHeaderStyle = 'FieldHeaderStyle';
  }

  SOToggleShow() {
    this.SO_sub_icon = '';
    this.SO_Icon = '';
    this.SO_fullWidth = '';
    this._loanInfoService.SO_width$.next('');
    this.SO_textShow = '';
    this.SO_CustHeight = 'soCustHeight368';
    this.PanelShow = true;
    this.FieldHeaderStyle = '';
  }

  SO_Toggle() {
    if (this.SO_sub_icon !== 'SO_sub_icon') {
      this.SO_textShow = 'SO_textHIde';
      this.SO_sub_icon = 'SO_sub_icon';
      this.SO_Icon = 'SO_Icon';
      this.SO_fullWidth = 'SO_fullWidth';
      this._loanInfoService.SO_width$.next('SO_width');
      this.SO_CustHeight = 'soCustHeight332';
      this.PanelShow = false;
      this.FieldHeaderStyle = 'FieldHeaderStyle';
    } else {
      this.SO_sub_icon = '';
      this.SO_Icon = '';
      this.SO_fullWidth = '';
      this._loanInfoService.SO_width$.next('');
      this.SO_textShow = '';
      this.SO_CustHeight = 'soCustHeight368';
      this.PanelShow = true;
      this.FieldHeaderStyle = '';
    }
    this._loanInfoService.STACKToggle$.next(true);
    setTimeout(() => {
      this._loanInfoService.ImageFit$.next(true);
    }, 300);
  }

  RefreshStackDocs() {
    this._loanInfoService.RefreshStackDocs(this.loanDocuments);
    this.filterVal = 'In Loan';
  }

  CheckLoanPDF() {
    this._loanInfoService.CheckLoanPDF();
  }

  ngOnDestroy() {
    this._subscriptions.forEach(element => {
      element.unsubscribe();
    });
  }
}
