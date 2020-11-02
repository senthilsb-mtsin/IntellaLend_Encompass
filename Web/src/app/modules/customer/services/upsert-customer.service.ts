import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { CustomerData } from '../customer.data';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CustomerWizardStepModel } from '../models/customer-wizard-steps.model';
import { CustomerService } from './customer.service';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { AppSettings } from '@mts-app-setting';
import { NotificationService } from '@mts-notification';
import { CharcCheckPipe, ValidateZipcodePipe, CheckDuplicateName } from '@mts-pipe';
import { CustomerDatatableModel } from '../models/customer-datatable.model';
import { ReviewTypeMappingModel } from '../models/review-type-mapping.model';
import { LoanTypeMappingModel } from '../models/loan-type-mapping.model';
import { SaveCustReviewLoanMappingModel } from '../models/save-cust-reivew-loan-mapping.model';
import { ChecklistItemRowData } from '../../loantype/models/checklist-items-table.model';
import { CheckListItemNamePipe } from '../../loantype/pipes';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';
import { StackingOrderDetailTable } from '../models/stacking-order-detail-table.model';

const jwtHelper = new JwtHelperService();

@Injectable()
export class UpsertCustomerService {

    UpsertCustomerSteps: { Customer: number, ServiceType: number, LoanType: number, Checklist: number, StackingOrder: number, CustomerConfig: number } = { Customer: 1, ServiceType: 2, LoanType: 3, Checklist: 4, StackingOrder: 5, CustomerConfig: 6 };
    setNextStep$ = new Subject<CustomerWizardStepModel>();
    Loading$ = new Subject<boolean>();
    reviewTypesMapped$ = new Subject<ReviewTypeMappingModel[]>();
    loanTypesMapped$ = new Subject<LoanTypeMappingModel[]>();
    retainConfirm$ = new Subject<boolean>();
    confirmModal$ = new Subject<boolean>();
    loanConfirmModal$ = new Subject<boolean>();
    loanRetainConfirm$ = new Subject<boolean>();
    CurrentReviewType$ = new Subject<string>();
    CurrentLoanType$ = new Subject<string>();
    CurrentCheckList$ = new Subject<{ CheckListID: number, CheckListName: string, SyncEnabled: boolean }>();
    CheckListDetailTable$ = new Subject<any[]>();
    StackingOrderDetailTable$ = new Subject<StackingOrderDetailTable[]>();
    CloneModal$ = new Subject<boolean>();
    isRowNotSelected$ = new Subject<boolean>();
    CustomerConfig$ = new Subject<{ ConfigID: number, CustomerID: number, CustomerName: string, Configkey: string, ConfigValue: string, Active: boolean }[]>();

    constructor(
        private _customerData: CustomerData,
        private _customerService: CustomerService,
        private _notificationService: NotificationService,
        private _allowChar: CharcCheckPipe,
        private _zipcodevalidpipe: ValidateZipcodePipe,
        private _checklistItemValidator: CheckListItemNamePipe,
        private _commonRuleBuilderService: CommonRuleBuilderService,
        private _dupCheck: CheckDuplicateName
    ) { }

    private _reviewTypesMapped: ReviewTypeMappingModel[] = [];
    private _loanTypesMapped: LoanTypeMappingModel[] = [];
    private _currentCustomer: CustomerDatatableModel = new CustomerDatatableModel();
    private _currentReviewType: ReviewTypeMappingModel;
    private _currentLoanType: LoanTypeMappingModel;
    private _currentCheckList: { CheckListID: number, CheckListName: string, SyncEnabled: boolean } = { CheckListID: 0, CheckListName: '', SyncEnabled: false };
    private _currentStackingOrder: { StackingOrderID: number, StackingOrderName: string } = { StackingOrderID: 0, StackingOrderName: '' };
    private _checkListDetailTable: ChecklistItemRowData[] = [];
    private _stackingOrderDetailTable: StackingOrderDetailTable[] = [];
    private _customerConfig: { ConfigID: number, CustomerID: number, CustomerName: string, Configkey: string, ConfigValue: string, Active: boolean }[] = [];

    AddCustomeSubmit() {
        this._currentCustomer = this._customerService.getCurrentCustomer();
        const req = { TableSchema: AppSettings.TenantSchema, customerMaster: this._currentCustomer };
        this._customerData.AddCustomeSubmit(req).subscribe(res => {
            const Result = jwtHelper.decodeToken(res.Data)['data'];
            if (req.customerMaster.Type === 'Add' && Result.Success) {
                this._currentCustomer.CustomerID = Result.CustomerID;
            }
            if ((req.customerMaster.Type === 'Add' && Result.Success) || (req.customerMaster.Type === 'Edit' && Result)) {
                this._customerService.setCurrectCustomer(this._currentCustomer);
                this._commonRuleBuilderService.setCurrentCustomer(this._currentCustomer.CustomerID);
                this.setNextStep$.next(new CustomerWizardStepModel(this.UpsertCustomerSteps.ServiceType, 'active complete', 'active', '', '', '', ''));
                this._notificationService.showSuccess(AppSettings.AuthorityLabelSingular + (req.customerMaster.Type === 'Add' ? ' Added ' : ' Updated ') + 'Successfully');
            }

            if ((req.customerMaster.Type === 'Add' && !Result.Success) || (req.customerMaster.Type === 'Edit' && !Result)) {
                this._notificationService.showError(AppSettings.AuthorityLabelSingular + ' Name already exist');
            }

            this.Loading$.next(false);
        });
    }

    GetCheckListGroup() {
        const req = { TableSchema: AppSettings.TenantSchema, CustomerID: this._currentCustomer.CustomerID, ReviewTypeID: this._currentReviewType.ReviewTypeID, LoanTypeID: this._currentLoanType.LoanTypeID };
        return this._customerData.GetCheckAndStack(req).subscribe(res => {
            const Result = jwtHelper.decodeToken(res.Data)['data'];
            if (Result) {
                if (Result.CheckListID > 0) {
                    this._currentCheckList.CheckListID = Result.CheckListID;
                    this._currentCheckList.CheckListName = Result.CheckListName;
                    this._currentCheckList.SyncEnabled = Result.Sync;
                    this.CurrentCheckList$.next(this._currentCheckList);
                } else {
                    this.CurrentCheckList$.next(this._currentCheckList);
                    this._notificationService.showError('Checklist Not Mapped');
                }

                if (Result.StackingOrderID > 0) {
                    this._currentStackingOrder.StackingOrderID = Result.StackingOrderID;
                    this._currentStackingOrder.StackingOrderName = Result.StackingOrderName;
                } else {
                    this._notificationService.showError('Stacking Order Not Mapped');
                }
            } else {
                this._notificationService.showError('Checklist Not Mapped');
            }

        });
    }

    GetCheckListDetail() {
        const req = { TableSchema: AppSettings.TenantSchema, CheckListDetailID: this._currentCheckList.CheckListID };
        return this._customerData.GetCheckListDetail(req).subscribe(res => {
            const Result = jwtHelper.decodeToken(res.Data)['data'];
            if (Result !== null) {
                Result.forEach(element => {
                    element.ReviewTypeName = this._currentReviewType.ReviewTypeName;
                    element.LoanType = this._currentLoanType.LoanTypeName;
                    element.CustomerName = this._currentCustomer.CustomerName;
                });
                this._checkListDetailTable = Result;
                this._commonRuleBuilderService.setSystemChecklist(this._currentCheckList);
                this._commonRuleBuilderService.setCheckListItems(this._checkListDetailTable.slice());
                this.CheckListDetailTable$.next(this._checkListDetailTable.slice());
            }
        });
    }

    DeleteCheckListItem(itemIDs: number[]) {
        const req = { TableSchema: AppSettings.TenantSchema, CheckListDetailsID: itemIDs };
        return this._customerData.DeleteChecklistItems(req).subscribe(res => {
            const result = jwtHelper.decodeToken(res.Data)['data'];
            if (result !== null) {
                if (result) {
                    this.CurrentCheckList$.next(this._currentCheckList);
                    this._notificationService.showSuccess('Deleted SuccessFully');
                    this.isRowNotSelected$.next(true);
                } else {
                    this._notificationService.showError('Deleted Failed');
                }
            }
        });
    }

    CloneCheckListItem(clonecheckListItem: string, itemsIDs: number[]) {
        const req = { TableSchema: AppSettings.TenantSchema, CheckListDetailsID: itemsIDs, ModifiedCheckListName: clonecheckListItem };
        return this._customerData.CloneChecklistItem(req).subscribe(res => {
            const result = jwtHelper.decodeToken(res.Data)['data'];
            if (result !== null) {
                if (result) {
                    this.CurrentCheckList$.next(this._currentCheckList);
                    this._notificationService.showSuccess('Checklist Item Cloned Successfully');
                    this.CloneModal$.next(false);
                    this.isRowNotSelected$.next(true);
                }
            }
        });
    }

    GetStackingOrderDetails() {
        this._stackingOrderDetailTable = [];
        if (this._currentStackingOrder.StackingOrderID > 0) {
            const req = { TableSchema: AppSettings.TenantSchema, StackingOrderID: this._currentStackingOrder.StackingOrderID };
            return this._customerData.GetStackingOrderDetails(req).subscribe(res => {
                const result = jwtHelper.decodeToken(res.Data)['data'];
                if (result !== null) {
                    this._stackingOrderDetailTable = result;
                    this.StackingOrderDetailTable$.next(this._stackingOrderDetailTable.slice());
                }
            });
        }
    }

    GetCurrentStackingOrder() {
        return this._currentStackingOrder;
    }

    SetSelectedReviewType(vals: ReviewTypeMappingModel) {
        this._currentReviewType = vals;
        this.CurrentReviewType$.next(this._currentReviewType.ReviewTypeName);
        this.setNextStep$.next(new CustomerWizardStepModel(this.UpsertCustomerSteps.LoanType, 'active complete', 'active complete', 'active', '', '', ''));
    }

    SetSelectedLoanType(vals: LoanTypeMappingModel) {
        this._currentLoanType = vals;
        this.CurrentLoanType$.next(this._currentLoanType.LoanTypeName);
        this._commonRuleBuilderService.setCurrentLoanTypeID(this._currentLoanType.LoanTypeID);
        this.setNextStep$.next(new CustomerWizardStepModel(this.UpsertCustomerSteps.Checklist, 'active complete', 'active complete', 'active complete', 'active', '', ''));
    }

    CheckCustReviewMapping(vals: ReviewTypeMappingModel) {
        if (vals.ReviewTypeID > 0 && this._currentCustomer.CustomerID !== 0) {
            const req = { TableSchema: AppSettings.TenantSchema, CustomerID: this._currentCustomer.CustomerID, ReviewTypeID: vals.ReviewTypeID };
            this._customerData.CheckCustReviewMapping(req).subscribe(res => {
                const Result = jwtHelper.decodeToken(res.Data)['data'];
                if (Result) {
                    this.RetainCustReviewMapping(vals);
                    vals.loading = false;
                } else {
                    this.SetCustReviewMapping(vals);
                }
            });
        }
    }

    CheckCustReviewLoanMapping(vals: LoanTypeMappingModel) {
        const req = { TableSchema: AppSettings.TenantSchema, CustomerID: this._currentCustomer.CustomerID, ReviewTypeID: this._currentReviewType.ReviewTypeID, LoanTypeID: vals.LoanTypeID };
        this._customerData.CheckCustReviewLoanMapping(req).subscribe(res => {
            const Result = jwtHelper.decodeToken(res.Data)['data'];
            if (Result) {
                vals.loading = false;
                this.loanRetainConfirm$.next(true);
            } else {
                this.SetCustReviewLoanMapping(vals);
            }
        });
    }

    RetainLoanTypeMapping(vals: LoanTypeMappingModel) {
        this.loanRetainConfirm$.next(false);
        vals.loading = true;
        const req = new SaveCustReviewLoanMappingModel(AppSettings.TenantSchema, this._currentCustomer.CustomerID, this._currentReviewType.ReviewTypeID, vals.LoanTypeID, '', vals.LoanUploadPath, false);
        this._customerData.RetainLoanTypeMapping(req).subscribe(res => {
            this.SaveCustLoanUploadPath(vals, req);
            const Result = jwtHelper.decodeToken(res.Data)['data'];
            if (Result) {
                vals.loading = false;
                if (!vals.DBMapped) {
                    vals.DBMapped = !vals.DBMapped;
                }
                this._notificationService.showSuccess('Mapping Retained and Updated Successfully');
            }
        });
    }

    RemoveCustReviewLoanMapping(vals: LoanTypeMappingModel) {
        const req = { TableSchema: AppSettings.TenantSchema, CustomerID: this._currentCustomer.CustomerID, ReviewTypeID: this._currentReviewType.ReviewTypeID, LoanTypeID: vals.LoanTypeID };
        this._customerData.RemoveCustReviewLoanMapping(req).subscribe(res => {
            this._customerData.RemoveCustLoanUpload(req).subscribe();
            const Result = jwtHelper.decodeToken(res.Data)['data'];
            if (Result) {
                this.loanConfirmModal$.next(false);
                this._notificationService.showSuccess('Mapping Removed Successfully');
            }
        });
    }

    CheckCustumerLoanUploadpath(vals: LoanTypeMappingModel) {
        const req = new SaveCustReviewLoanMappingModel(AppSettings.TenantSchema, this._currentCustomer.CustomerID, this._currentReviewType.ReviewTypeID, vals.LoanTypeID, '', vals.LoanUploadPath, false);
        this._customerData.CheckCustLoanUploadPath(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data) {
                this.CheckCustReviewLoanMapping(vals);
            } else {
                vals.Mapped = !vals.Mapped;
                this._notificationService.showError('Loan Path is Already Exist');
            }
        });
    }

    SetCustReviewLoanMapping(vals: LoanTypeMappingModel) {
        this.loanRetainConfirm$.next(false);
        vals.loading = true;
        const req = new SaveCustReviewLoanMappingModel(
            AppSettings.TenantSchema,
            this._currentCustomer.CustomerID,
            this._currentReviewType.ReviewTypeID,
            vals.LoanTypeID,
            vals.BoxUploadPath,
            vals.LoanUploadPath,
            true
        );
        this._customerData.SaveCustReviewLoanMapping(req).subscribe(res => {
            this.SaveCustLoanUploadPath(vals, req);
            const Result = jwtHelper.decodeToken(res.Data)['data'];
            if (Result) {
                vals.loading = false;
                if (!vals.DBMapped) {
                    vals.DBMapped = !vals.DBMapped;
                }
                this._notificationService.showSuccess('Mapping Added Successfully');
            }
        });
    }

    SaveCustLoanUploadPath(vals: LoanTypeMappingModel, req: SaveCustReviewLoanMappingModel) {
        this._customerData.SaveCustLoanUploadPath(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data && isTruthy(vals.LoanUploadPath)) {
                this._notificationService.showSuccess('Loan Path Added Successfully');
            } else if (vals.LoanUploadPath !== '') {
                vals.Mapped = !vals.Mapped;
                this._notificationService.showError('The Loan Path is Exist. Enter New Loan Path');
            }
        });
    }

    RemoveCustReviewMapping(vals: ReviewTypeMappingModel) {
        this.confirmModal$.next(false);
        const req = { TableSchema: AppSettings.TenantSchema, CustomerID: this._currentCustomer.CustomerID, ReviewTypeID: vals.ReviewTypeID };
        this._customerData.RemoveCustReviewMapping(req).subscribe(res => {
            const Result = jwtHelper.decodeToken(res.Data)['data'];
            if (Result) {
                this._notificationService.showSuccess('Mapping Removed Successfully');
            }
        });
    }

    RetainCustReviewMapping(vals: ReviewTypeMappingModel) {
        this.retainConfirm$.next(false);
        const req = { TableSchema: AppSettings.TenantSchema, CustomerID: this._currentCustomer.CustomerID, ReviewTypeID: vals.ReviewTypeID };
        this._customerData.RetainCustReviewMapping(req).subscribe(res => {
            const Result = jwtHelper.decodeToken(res.Data)['data'];
            if (Result) {
                this._notificationService.showSuccess('Mapping Retained Successfully');
                vals.loading = false;
            }
        });
    }

    SetCustReviewMapping(vals: ReviewTypeMappingModel) {
        this.retainConfirm$.next(false);
        if (vals.Mapped) {
            vals.loading = true;
            const req = { TableSchema: AppSettings.TenantSchema, CustomerID: this._currentCustomer.CustomerID, ReviewTypeID: vals.ReviewTypeID };
            this._customerData.SaveCustReviewMapping(req).subscribe(res => {
                const Result = jwtHelper.decodeToken(res.Data)['data'];
                if (Result) {
                    vals.loading = false;
                    this._notificationService.showSuccess('Mapping Added Successfully');
                }
            });
        }
    }

    GetCurrentCustomer() {
        return this._currentCustomer;
    }

    GetCurrentReviewType() {
        return this._currentReviewType;
    }

    GetMappedLoanTypes() {
        if (this._currentCustomer.CustomerID > 0) {
            const req = { TableSchema: AppSettings.TenantSchema, CustomerID: this._currentCustomer.CustomerID, ReviewTypeID: this._currentReviewType.ReviewTypeID };
            this._customerData.GetMappedLoanTypes(req).subscribe(res => {
                const Result = jwtHelper.decodeToken(res.Data)['data'];
                if (Result !== null) {
                    this._loanTypesMapped = Result;
                    this._loanTypesMapped.forEach(element => {
                        element.loading = false;
                    });
                    this.loanTypesMapped$.next(this._loanTypesMapped.slice());
                }
            });
        }
    }

    GetMappedReviewTypes() {
        if (this._currentCustomer.CustomerID > 0) {
            const req = { TableSchema: AppSettings.TenantSchema, CustomerID: this._currentCustomer.CustomerID };
            this._customerData.GetMappedReviewTypes(req).subscribe(res => {
                const Result = jwtHelper.decodeToken(res.Data)['data'];
                if (Result !== null) {
                    this._reviewTypesMapped = Result;
                    this._reviewTypesMapped.forEach(element => {
                        element.loading = false;
                    });
                    this.reviewTypesMapped$.next(this._reviewTypesMapped.slice());
                }
            });
        }
    }

    validateCheckListItemName(cloneName: string): boolean {
        const fullTableData = this._checkListDetailTable.slice();
        const newTableData = [];
        fullTableData.forEach(elts => {
            newTableData.push({ id: elts.ChecklistGroupId, name: elts.CheckListName });
        });
        const filteredValues = this._checklistItemValidator.transform(newTableData, cloneName, this._currentCheckList.CheckListID);
        if (filteredValues === false) {
            this._notificationService.showError('Name already Exist in Group');
        } else {
            return true;
        }
        return false;
    }

    CheckDuplicate(): boolean {
        const customerTable = this._customerService.getCustomerMasterTable();
        let currentCustomerName = '';
        customerTable.forEach(element => {
            if (element.CustomerID === this._customerService.getCurrentCustomer().CustomerID) {
                currentCustomerName = element.CustomerName;
            }
        });
        return this._dupCheck.transform(this._customerService.getCustomerMasterTable(), currentCustomerName, this._customerService.getCurrentCustomer().CustomerName);
    }

    validate(): boolean {
        let returVal = false;
        const currtCustomer = this._customerService.getCurrentCustomer();
        const trimmedCustomer = currtCustomer.CustomerName.replace(/[\s]/g, '');
        if (currtCustomer.Type === 'Add') {
            if (!isTruthy(trimmedCustomer)) {
                this._notificationService.showError(AppSettings.AuthorityLabelSingular + ' Name Required');
                returVal = true;
            }
            if (isTruthy(trimmedCustomer) && !this._allowChar.transform(trimmedCustomer)) {
                this._notificationService.showError(AppSettings.AuthorityLabelSingular + ' name must be Character');
                returVal = true;
            }
            if (isTruthy(currtCustomer.ZipCode) && !this._zipcodevalidpipe.transform(currtCustomer.ZipCode)) {
                this._notificationService.showError('Enter Valid Zip Code');
                returVal = true;
            }
        } else if (currtCustomer.Type !== 'Add') {
            if (!isTruthy(trimmedCustomer)) {
                this._notificationService.showError(AppSettings.AuthorityLabelSingular + ' Name Required');
                returVal = true;
            }
            if (isTruthy(trimmedCustomer) && !this._allowChar.transform(trimmedCustomer)) {
                this._notificationService.showError(AppSettings.AuthorityLabelSingular + ' name must be Character');
                returVal = true;
            }
            if (isTruthy(currtCustomer.State) && !this._allowChar.transform(currtCustomer.State)) {
                this._notificationService.showError('State must be Character');
                returVal = true;
            }
            if (isTruthy(currtCustomer.Country) && !this._allowChar.transform(currtCustomer.Country)) {
                this._notificationService.showError('Country must be Character');
                returVal = true;
            }
            if (isTruthy(currtCustomer.ZipCode) && !this._zipcodevalidpipe.transform(currtCustomer.ZipCode)) {
                this._notificationService.showError('Enter Valid Zip Code');
                returVal = true;
            }
        }
        return returVal;
    }

    SetTenantOrderByField(_docID: number, _fieldID: number) {
        const req = { TableSchema: AppSettings.TenantSchema, DocumentTypeID: _docID, FieldID: _fieldID };
        this._customerData.SetTenantOrderByField(req).subscribe();
    }

    SetTenantDocFieldValue(_docID: number, _fieldID: number) {
        const req = { TableSchema: AppSettings.TenantSchema, DocumentTypeID: _docID, FieldID: _fieldID };
        this._customerData.SetTenantDocFieldValue(req).subscribe();
    }

    SetDocGroupFieldValue(docGroupStackingOrder, groupName, stackGroupID, value) {
        const docs = docGroupStackingOrder.filter(x => x.StackingOrderGroupDetails[0].StackingOrderGroupName === groupName);
        let displayname = '';
        const tempDisplayName = value.target.selectedOptions[0].text;
        let data = [];
        docs.forEach(el => {
            data = el.DocFieldList.filter(x => x.Name === tempDisplayName);
            if (data.length > 0) {
                el.StackingOrderGroupDetails[0].GroupSortField = data[0].DisplayName;
                displayname = data[0].DisplayName;
            }
        });
        const groupdetails = { ID: stackGroupID, Name: groupName, StackingOrderFieldName: displayname };
        const req = { TableSchema: AppSettings.TenantSchema, StackingOrderDetails: docs, StackOrder: groupdetails, };
        this._customerData.SetDocGroupFieldValue(req).subscribe();
    }

    SaveStackingOrder(docStackingOrder: StackingOrderDetailTable[], docGroupStackingOrder: StackingOrderDetailTable[]) {
        const groupDatas = docStackingOrder.filter(a => a.isGroup);
        let fieldValues = [];
        if (groupDatas.length > 0) {
            fieldValues = groupDatas.filter(aa => aa.OrderByFieldID === '0');
        }

        const sysStackingDetails = [];
        if (fieldValues.length === 0) {
            docStackingOrder.forEach(element => {
                if (!element.isGroup) {
                    sysStackingDetails.push({ isGroup: false, ID: element.DocumentTypeID, Name: 'None', StackingOrderFieldName: '' });
                } else {
                    const docs = docGroupStackingOrder.filter(ele => ele.StackingOrderGroupDetails[0].StackingOrderGroupName === element.StackingOrderGroupDetails[0].StackingOrderGroupName);
                    docs.forEach(el => {
                        sysStackingDetails.push({ isGroup: true, ID: el.DocumentTypeID, Name: el.StackingOrderGroupDetails[0].StackingOrderGroupName, StackingOrderFieldName: el.StackingOrderGroupDetails[0].GroupSortField });
                    });
                }
            });
            const req = { TableSchema: AppSettings.TenantSchema, StackOrderID: this._currentStackingOrder.StackingOrderID, StackOrder: sysStackingDetails };
            this._customerData.SaveCustomerStackingOrder(req).subscribe(res => {
                const Result = jwtHelper.decodeToken(res.Data)['data'];
                if (Result !== null) {
                    if (Result) {
                        this.GetStackingOrderDetails();
                        this._notificationService.showSuccess('Stacking Order Updated Successfully');
                    }
                } else { this._notificationService.showError('Error Contact Administrator'); }
            });
        } else {
            this._notificationService.showError('Select Group Sort Field');
        }
    }

    GetCustomerConfigData() {
        const req = { TableSchema: AppSettings.TenantSchema, Active: true };
        this._customerData.GetAllCustomerConfigData(req).subscribe((res) => {
            const result = jwtHelper.decodeToken(res.Data)['data'];
            this._customerConfig = [];
            if (result !== null) {
                if (result.length > 0) {
                    result.forEach(element => {
                        if ((element.Configkey === 'Retention_Policy' || element.Configkey === 'Export_Path') && this._currentCustomer.CustomerID === element.CustomerID) {
                            this._customerConfig.push(element);
                        }
                    });
                }
                this.CustomerConfig$.next(this._customerConfig.slice());
            }
        });
    }

    AddCustomerConfigData(_configItems) {
        const req = { TableSchema: AppSettings.TenantSchema, CustomerID: this._currentCustomer.CustomerID, custConfigItems: _configItems };
        this._customerData.AddCustomerConfigData(req).subscribe(res => {
            const result = jwtHelper.decodeToken(res.Data)['data'];
            if (result !== null) {
                if (result) {
                    this._notificationService.showSuccess('Configuration Updated Successfully');
                } else { this._notificationService.showError('Configuration Updated Failed'); }
            } else { this._notificationService.showError('Configuration Updated Failed'); }
        });
    }

}
