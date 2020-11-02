import { Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { LoanInfoService } from '../../services/loan-info.service';
import { Subscription } from 'rxjs';
import { SessionHelper } from '@mts-app-session';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Component({
    selector: 'mts-loan-document-field',
    templateUrl: 'loan-document-field.page.html',
    styleUrls: ['loan-document-field.page.scss']
})
export class LoanDocumentFieldComponent implements OnInit, OnDestroy {

    @ViewChild('confirmModal') confirmModal: ModalDirective;
    FieldHeaderStyle: any = 'FIELD_title';
    FIELD_textShow: any = 'FIELD_textHide';
    FIELD_sub_icon: any = '';
    fieldSearchVal: any = '';
    showHide: boolean[] = [false, false, false];
    loanDocuments: { DocID: any, DocName: any }[] = [];
    newDocTypeID: any;
    fieldFrmGrp: FormGroup;
    CardHeight = 'full_height_field';

    constructor(
        private _loanInfoService: LoanInfoService,
        private fb: FormBuilder
    ) {
        this.checkPermission('ReadonlyLoans', 0);
        this.createForm();
    }

    private _subscriptions: Subscription[] = [];
    private _currentForm: any;

    get formData() { return this.fieldFrmGrp.get('fields') as FormArray; }

    ngOnInit(): void {

        this.newDocTypeID = this._loanInfoService.GetLoanViewDocument().DocID;

        this._subscriptions.push(this._loanInfoService.confirmChangeDocModal$.subscribe((res: boolean) => {
            res ? this.confirmModal.show() : this.confirmModal.hide();
        }));
        this._subscriptions.push(this._loanInfoService.SetDocField$.subscribe((res: { DocLevelFields: any, TempDocTables: any }) => {
            if (res !== null) {
                this.setFormFields(res.DocLevelFields);
            }
        }));
        this._subscriptions.push(this._loanInfoService.newDocTypeID$.subscribe((res: any) => {
            this.newDocTypeID = res;
        }));
        this._subscriptions.push(this._loanInfoService.LoanPopOutFieldHeight$.subscribe((res: string) => {
            this.CardHeight = res;
        }));
        this._subscriptions.push(this._loanInfoService.ShowDocumentDetailView$.subscribe((res: boolean) => {
            if (res) {
                this._loanInfoService.GetLoanDocInfo();
            }
        }));

        this.GetLoanDocuments();
    }

    SetFieldCordsInImage(ctrl: any) {
        const _cords = ctrl.get('CoordinatesList').value;
        const _pageNo = ctrl.get('PageNo').value;
        if (isTruthy(_cords)) {
            this._loanInfoService.SetFieldDrawBox$.next({ checklistState: this._loanInfoService.GetLoanViewDocument().checkListState, pageData: { cords: _cords, pageNo: _pageNo } });
        }
    }

    setFormFields(_docLevelFields) {
        const docFieldFGs = _docLevelFields.map(docField => {
            return this.fb.group(docField);
        });
        const docFieldFormArray = this.fb.array(docFieldFGs);
        this.fieldFrmGrp.setControl('fields', docFieldFormArray);

        this.fieldFrmGrp.enable();
        if (this.showHide[0]) {
            this.fieldFrmGrp.disable();
        }
    }

    createForm() {
        this.fieldFrmGrp = this.fb.group({
            fields: this.fb.array([])
        });

        this._subscriptions.push(this.fieldFrmGrp.valueChanges.subscribe((form) => {
            this._loanInfoService.SetCurrentForm(form, 'fields');
        }));
    }

    ChangeDocType() {
        this._loanInfoService.ChangeDocumentType$.next(this.newDocTypeID);
    }

    GetLoanDocuments() {
        let docName = '';
        this._loanInfoService.GetLoan().loanDocuments.forEach(element => {
            docName = this._loanInfoService.CheckMoreThanOnce(element.DocName) ? element.DocName + ' -V' + (element.FieldOrderBy === '' ? (element.VersionNumber).toString() : this._loanInfoService.GetFieldVersionNumber(element.DocName, element.FieldOrderVersion)) : element.DocName;
            this.loanDocuments.push({ DocID: element.DocID, DocName: docName });
        });
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

    FIELD_Toggle() {
        if (this.FIELD_textShow === '') {
            this.FIELD_textShow = 'FIELD_textHide FI_sub_icon';
            this.FieldHeaderStyle = 'FIELD_title';
            this.FIELD_sub_icon = '';
        } else {
            this.FIELD_textShow = '';
            this.FieldHeaderStyle = '';
            this.FIELD_sub_icon = 'FI_sub_icon';
        }

        this._loanInfoService.FIELDToggle$.next(true);
    }

    saveNReevaluate() {
        this._loanInfoService.SaveAndRevaluate$.next(true);
    }

    ngOnDestroy(): void {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
