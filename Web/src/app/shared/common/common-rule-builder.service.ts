
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { Subject } from 'rxjs';
import { ChecklistItemRowData } from '../model/checklist-item-row-data.model';
import { CheckListItemNamePipe } from 'src/app/modules/loantype/pipes';
import { NotificationService } from '@mts-notification';

const jwtHelper = new JwtHelperService();
@Injectable()
export class CommonRuleBuilderService {

    SetRuleStep = new Subject<{ Step1: boolean, Step2: boolean, Step3: boolean }>();
    EnableRuleBuilder = new Subject<boolean>();
    SysCheckListDetailTableData = new Subject<any[]>();
    ruleBuilderNext = new Subject<boolean>();
    ruleExpression = new Subject<string>();
    RuleAdded = new Subject<boolean>();

    constructor(
        private _checklistItemValidator: CheckListItemNamePipe,
        private _notificationService: NotificationService) {

    }

    private _tenantSchema = '';
    private _loanTypeID = 0;
    private _customerID = 1;
    private _checklistItemRowData: ChecklistItemRowData = new ChecklistItemRowData();
    private _checkList: { CheckListID: number, CheckListName: string } = { CheckListID: 0, CheckListName: '' };
    private _ruleBuilderSteps: { Step1: boolean, Step2: boolean, Step3: boolean };
    private _sysCheckListDetailTableData: any[] = [];

    setRuleBuilderSteps(steps: { Step1: boolean, Step2: boolean, Step3: boolean }) {
        this._ruleBuilderSteps = steps;
    }

    setSystemChecklist(checkList: { CheckListID: number, CheckListName: string }) {
        this._checkList = checkList;
    }

    clearSavedChecklistItem() {
        this._checklistItemRowData = new ChecklistItemRowData();
    }

    getCurrentLoanTypeID(): number {
        return this._loanTypeID;
    }

    getTenantSchema() {
        return this._tenantSchema;
    }

    getCurrentCustomer() {
        return this._customerID;
    }

    setCurrentCustomer(_customerID: number) {
        this._customerID = _customerID;
    }

    setTenantSchema(_schema: string) {
        this._tenantSchema = _schema;
    }

    setCurrentLoanTypeID(_loanTypeID: number) {
        this._loanTypeID = _loanTypeID;
    }

    getEditChecklistItem() {
        return this._checklistItemRowData;
    }

    setCheckListItems(checkListItems: any[]) {
        this._sysCheckListDetailTableData = checkListItems;
    }

    getChecklist() {
        return this._checkList;
    }

    getRuleBuilderSteps() {
        return this._ruleBuilderSteps;
    }

    setEditChecklistItem(rowData: ChecklistItemRowData) {
        rowData.LosMatched = isTruthy(rowData.LosIsMatched) && rowData.LosIsMatched === 1;
        if (isTruthy(rowData.RuleJson)) {
            rowData.RuleJsonObject = JSON.parse(rowData.RuleJson);
        }
        this._checklistItemRowData = rowData;
    }

    validateCheckListItemName(cloneName: string): boolean {
        const fullTableData = this._sysCheckListDetailTableData.slice();
        const newTableData = [];
        fullTableData.forEach(elts => {
            newTableData.push({ id: elts.ChecklistGroupId, name: elts.CheckListName });
        });
        const filteredValues = this._checklistItemValidator.transform(newTableData, cloneName, this._checkList.CheckListID);
        if (filteredValues === false) {
            this._notificationService.showError('Name already Exist in Group');
        } else {
            return true;
        }
        return false;
    }

}
