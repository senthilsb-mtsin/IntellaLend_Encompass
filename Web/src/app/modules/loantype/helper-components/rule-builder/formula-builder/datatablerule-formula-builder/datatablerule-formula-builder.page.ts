import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { ChecklistItemRowData } from 'src/app/modules/loantype/models/checklist-items-table.model';
import { AppSettings, FormulaBuilderTypesConstant } from '@mts-app-setting';
import { AddLoanTypeService } from 'src/app/modules/loantype/service/add-loantype.service';
import { RuleBuilderService } from 'src/app/modules/loantype/service/rule-builder.service';
import { RuleCheckValidation, SingleOperatorExistforRule } from 'src/app/modules/loantype/pipes';
import { CommonService } from 'src/app/shared/common';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { NotificationService } from '@mts-notification';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';
import { element } from 'protractor';

@Component({
    selector: 'mts-datatablerule-formula-builder',
    styleUrls: ['datatablerule-formula-builder.page.css'],
    templateUrl: 'datatablerule-formula-builder.page.html',
    providers: [RuleCheckValidation, SingleOperatorExistforRule]
})
export class DataTableFormulaBuilderComponent implements OnInit, OnDestroy {
    rulesFrmGrp: FormGroup;
    docFieldMasters: any[] = [];
    genDocTypes: any[] = [];
    dataTableRuleMasters: any[] = [];
    currtDocFields: any[] = [];
    currtDocTables: any[] = [];
    currtDocTableColName: any[] = [];
    rowData: ChecklistItemRowData = new ChecklistItemRowData();
    waitingForData = true;
    FieldErrorMsg = 'The value entered needs to match exactly with the selected Field value';
    isErrMsgs = false;
    RuleOperator: any[] = FormulaBuilderTypesConstant.GeneralRuleOperators.slice();
    losEncompassFields: any[] = [];
    headerFieldID = '';
    ErrorMsg = '';
    rowKeyValue = '#SUM#';

    LosDocumentFields: any[] = [];
    FannieMaeDocName: string = AppSettings.RuleFannieMaeDocName;

    constructor(
        private _commonRuleBuilderService: CommonRuleBuilderService,
        private _ruleBuilderService: RuleBuilderService,
        private _checkrule: RuleCheckValidation,
        private _commonService: CommonService,
        private _singOperatorCheck: SingleOperatorExistforRule,
        private _notificationService: NotificationService,
        private _fb: FormBuilder
    ) {
        this._ruleBuilderService.setRuleFormGroup('datatablerule');
    }

    private _subscriptions: Subscription[] = [];
    private _operators: any[] = FormulaBuilderTypesConstant.Operators.slice();
    get formData() { return this.rulesFrmGrp.get('datatableRule') as FormArray; }

    ngOnInit(): void {
        this.rowData = this._commonRuleBuilderService.getEditChecklistItem();
        this.rulesFrmGrp = this._ruleBuilderService.getRuleFormGroup();
        this._subscriptions.push(this._ruleBuilderService.LoanTypeDocuments.subscribe(
            (res: {
                Documents: { id: number, text: string }[],
                Fields: { DocID: number, FieldID: number, Name: string, DocName: string }[],
                DataTables: { DocID: number, TableName: string, ColumnName: string }[]
            }) => {
                this.genDocTypes = res.Documents;
                this.docFieldMasters = res.Fields;
                this.dataTableRuleMasters = res.DataTables;
                this.waitingForData = false;

                if (isTruthy(this.rowData.RuleJsonObject) && isTruthy(this.rowData.RuleJsonObject.datatableRule) && this.rowData.RuleJsonObject.datatableRule.length > 0) {
                    this.GenerateFormValues();
                }
            }));
        this._subscriptions.push(this._commonService.SystemLOSFields.subscribe((res: any[]) => {
            this.losEncompassFields = res;
        }));
        this._ruleBuilderService.getSysLoanTypeDocuments();

        this._subscriptions.push(this.rulesFrmGrp.valueChanges.subscribe((form) => {
            let str = '';
            let ruleFormationValues = 'datatable([Rule])';
            let datatableRuleOperatorCheck;
            let totalEditOperatorCount;
            let getAllEditRuleOperatorsCount;
            let finalEditOperatorCount;
            let docTypeFieldFlag = false;

            form.datatableRule.forEach(elements => {
                if (elements.generalDocumentTypes !== '' && isTruthy(elements.docField) && typeof (elements.docField) === 'string' && elements.docField !== '' && elements.generalDocumentTypes !== AppSettings.RuleFannieMaeDocName) {
                    str += elements.openBrace + '[' + elements.generalDocumentTypes + '.' + elements.docField + ']' + elements.closeBrace + elements.docFieldOperator;
                } else if (elements.generalDocumentTypes !== '' && isTruthy(elements.docField) && Array.isArray(elements.docField) && elements.docField.length > 0 && elements.generalDocumentTypes !== AppSettings.RuleFannieMaeDocName) {
                    str += elements.openBrace + '[' + elements.generalDocumentTypes + '.' + elements.docField[0].text + ']' + elements.closeBrace + elements.docFieldOperator;
                } else if (elements.generalDocumentTypes !== '' && isTruthy(elements.LosdocField) && typeof (elements.LosdocField) === 'string' && elements.LosdocField !== '' && elements.generalDocumentTypes === AppSettings.RuleFannieMaeDocName) {
                    str += elements.openBrace + '[' + AppSettings.FannieMaeDocDisplayName + '.' + elements.LosdocField + ']' + elements.closeBrace + elements.docFieldOperator;
                } else if (elements.generalDocumentTypes !== '' && isTruthy(elements.LosdocField) && Array.isArray(elements.LosdocField) && elements.LosdocField.length > 0 && elements.generalDocumentTypes === AppSettings.RuleFannieMaeDocName) {
                    str += elements.openBrace + '[' + AppSettings.FannieMaeDocDisplayName + '.' + elements.LosdocField[0].text + ']' + elements.closeBrace + elements.docFieldOperator;
                } else if (elements.valueDocField !== '') {
                    str += elements.openBrace + elements.valueDocField + elements.closeBrace + elements.docFieldOperator;
                } else if (elements.fieldOrTableSelect === 'Table' && elements.tableName !== '' && elements.columnName && elements.isKeyColumnEnabled && elements.rowNumber !== '') {
                    str += elements.openBrace + '[' + elements.generalDocumentTypes + '.' + elements.tableName + '.' + elements.columnName + '.' + '|' + elements.rowNumber + '|' + ']' + elements.closeBrace + elements.docFieldOperator;
                } else if (elements.fieldOrTableSelect === 'Table' && elements.tableName !== '' && elements.columnName !== '' && !(elements.isKeyColumnEnabled)) {
                    str += elements.openBrace + '[' + elements.generalDocumentTypes + '.' + elements.tableName + '.' + elements.columnName + '.' + this.rowKeyValue + ']' + elements.closeBrace + elements.docFieldOperator;
                } else if (elements.generalDocumentTypes === '' || (elements.docField === '' && elements.LosdocField) || elements.docFieldOperator === '') {
                    this._commonRuleBuilderService.ruleBuilderNext.next(true);
                    docTypeFieldFlag = true;
                }
            });
            if (str === '') {
                this._commonRuleBuilderService.ruleBuilderNext.next(true);
            } else {
                if (!docTypeFieldFlag) {
                    ruleFormationValues = this._ruleBuilderService.replaceFormula(ruleFormationValues, str);
                    const strReplaced = str.split(']');

                    if (strReplaced.length > 1 && this._operators.includes(strReplaced[strReplaced.length - 1])) {
                        datatableRuleOperatorCheck = false;
                    } else if (form.datatableRule.length === 1) {
                        datatableRuleOperatorCheck = false;
                    } else {
                        datatableRuleOperatorCheck = this._checkrule.transform(str, this._operators);
                        datatableRuleOperatorCheck = this.checkOperatorFunction(form.datatableRule);
                    }
                    totalEditOperatorCount = this._singOperatorCheck.transform(str, this._operators);
                    getAllEditRuleOperatorsCount = this._singOperatorCheck.transform(totalEditOperatorCount[0]['Vals'].join(), this._operators);
                    finalEditOperatorCount = totalEditOperatorCount[1]['operatorsCount'] - getAllEditRuleOperatorsCount[1]['operatorsCount'];
                    const isValidRule = ruleFormationValues.match(/\(/g).length === ruleFormationValues.match(/\)/g).length;
                    if (isValidRule && datatableRuleOperatorCheck === true && finalEditOperatorCount === totalEditOperatorCount[0]['Vals'].length - 1 && totalEditOperatorCount[0]['Vals']['length'] !== 1 && totalEditOperatorCount[1]['operatorsCount'] !== 0) {
                        this.ErrorMsg = '';
                        this.rowData.RuleDescription = ruleFormationValues;
                        this._commonRuleBuilderService.ruleBuilderNext.next(false);
                    } else {
                        if (!isValidRule) {
                            this.ErrorMsg = 'Brackets are not closed properly, please check the Rule';
                            this._commonRuleBuilderService.ruleBuilderNext.next(true);
                        }
                        if (datatableRuleOperatorCheck === false) {
                            this.ErrorMsg = 'Rule must not end with an Operator, please check the Rule';
                            this._commonRuleBuilderService.ruleBuilderNext.next(true);
                        }
                        if (finalEditOperatorCount !== totalEditOperatorCount[0]['Vals'].length - 1 || totalEditOperatorCount[0]['Vals']['length'] === 1 && finalEditOperatorCount === 0) {
                            this.ErrorMsg = 'Please Select Atleast One Operator';
                            this._commonRuleBuilderService.ruleBuilderNext.next(true);
                        }
                    }
                }
            }
            this._commonRuleBuilderService.ruleExpression.next(ruleFormationValues);

        }));
        this._subscriptions.push(this._ruleBuilderService.LosDocumentFields.subscribe((elements: any[]) => {

            this.LosDocumentFields = [];
            elements.forEach((ele) => {
                this.LosDocumentFields.push(ele);
            });
        }));
    }

    ChangeFieldOrTable(vals: any, index: any) {
        if (vals.target.value === 'Value') {
            this.isErrMsgs = true;
            this.formData.controls[index].get('fieldOrTableSelect').setValue('Value');
            this.formData.controls[index].get('docField').setValue('');
            this.formData.controls[index].get('LosdocField').setValue('');
            this.formData.controls[index].get('generalDocumentTypes').setValue('');
            this.formData.controls[index].get('fieldsCustomValues').setValue(true);
            this.formData.controls[index].get('docFieldOperator').setValue('');

        } else if (vals.target.value === 'Field') {
            this.formData.controls[index].get('fieldOrTableSelect').setValue('Field');
            this.formData.controls[index].get('valueDocField').setValue('');
            this.formData.controls[index].get('fieldsCustomValues').setValue('');
            this.formData.controls[index].get('docFieldOperator').setValue('');
            this.formData.controls[index].get('generalDocumentTypes').setValue('');
        } else if (vals.target.value === 'Table') {
            this.formData.controls[index].get('fieldOrTableSelect').setValue('Table');
            this.formData.controls[index].get('valueDocField').setValue('');
            this.formData.controls[index].get('fieldsCustomValues').setValue('');
            this.formData.controls[index].get('columnName').setValue('');
            this.formData.controls[index].get('tableName').setValue('');
            this.formData.controls[index].get('isKeyColumnEnabled').setValue(false);
            this.formData.controls[index].get('rowNumber').setValue('');
            this.formData.controls[index].get('docFieldOperator').setValue('');
            this.formData.controls[index].get('generalDocumentTypes').setValue('');

        }
    }

    DocTypesChanged(genVals: any, i: any) {
        const DocName: string = ((typeof (genVals) === 'string') ? genVals : genVals.target.selectedOptions[0].innerText);
        if (DocName !== AppSettings.RuleFannieMaeDocName) {
            this._ruleBuilderService.docFieldsInitChange(this.formData.controls[i], this.currtDocFields, genVals, 'docField', i);
        }
    }

    removeRules(i: any) {
        this.formData.removeAt(i);
    }

    GenerateFormValues() {
        let i = 0;
        const rowKeysArry = [];
        this.rowData.RuleJsonObject.datatableRule.forEach(ele => {
            this.addRules();
            if (ele.fieldOrTableSelect === 'Field') {
                this.formData.controls[i].get('fieldOrTableSelect').setValue(ele.fieldOrTableSelect);
                this.formData.controls[i].get('openBrace').setValue(ele.openBrace);
                this.DocTypesChanged(ele.generalDocumentTypes, i);
                this.formData.controls[i].get('generalDocumentTypes').setValue(ele.generalDocumentTypes);

                if (isTruthy(ele.docField)) {
                    this.formData.controls[i].get('docField').setValue(ele.docField[0].id);
                }
                if (isTruthy(ele.LosdocField)) {
                    this.formData.controls[i].get('LosdocField').setValue(ele.LosdocField);
                }
                this.formData.controls[i].get('closeBrace').setValue(ele.closeBrace);
                this.formData.controls[i].get('docFieldOperator').setValue(ele.docFieldOperator);
            }
            if (ele.fieldOrTableSelect === 'Value') {
                this.formData.controls[i].get('openBrace').setValue(ele.openBrace);
                this.formData.controls[i].get('fieldOrTableSelect').setValue(ele.fieldOrTableSelect);
                this.formData.controls[i].get('valueDocField').setValue(ele.valueDocField);
                this.formData.controls[i].get('closeBrace').setValue(ele.closeBrace);
                this.formData.controls[i].get('docFieldOperator').setValue(ele.docFieldOperator);
            }
            if (ele.fieldOrTableSelect === 'Table') {
                this.formData.controls[i].get('openBrace').setValue(ele.openBrace);
                this.formData.controls[i].get('fieldOrTableSelect').setValue(ele.fieldOrTableSelect);
                this.DataTableInitRuleDocTypesChanged(ele.generalDocumentTypes, i);
                this.formData.controls[i].get('generalDocumentTypes').setValue(ele.generalDocumentTypes);
                this.formData.controls[i].get('tableName').setValue(ele.tableName[0].id);
                this.DataTableEditInitTableNameChanged(ele.tableName, i);
                if (ele.isKeyColumnEnabled) {
                    rowKeysArry.push(i);
                    this.formData.controls[i].get('isKeyColumnEnabled').setValue(ele.isKeyColumnEnabled);
                    this.formData.controls[i].get('rowNumber').setValue(ele.rowNumber);
                }
                this.formData.controls[i].get('columnName').setValue(ele.columnName[0].id);
                this.formData.controls[i].get('closeBrace').setValue(ele.closeBrace);
                this.formData.controls[i].get('docFieldOperator').setValue(ele.docFieldOperator);
            }
            i++;
        });

        setTimeout(() => {
            rowKeysArry.forEach(ele => {
                this.SetInitKeyValue(ele);
            });
        }, 1000);
    }

    SetInitKeyValue(index: any) {
        const keyInput = 'KeyType' + index;
        if (isTruthy(document.getElementById(keyInput))) {
            document.getElementById(keyInput).parentElement.classList.add('showinput');
        }
    }

    DataTableEditInitTableNameChanged(value: any, index: any) {
        const tableVal = this.formData.controls[index].get('tableName').value;
        const columnNames = [];
        this.formData.controls[index].get('columnName').setValue('');
        if (tableVal.length > 0 && tableVal !== '' && tableVal !== 'Select') {
            const newVals = tableVal.replace(/[\s]/g, '');
            for (let i = 0; i < this.dataTableRuleMasters.length; i++) {
                const trmmiedSpaceTableName = this.dataTableRuleMasters[i].TableName.replace(/[\s]/g, '');
                if (newVals === trmmiedSpaceTableName) {
                    columnNames.push({ id: this.dataTableRuleMasters[i].ColumnName, text: this.dataTableRuleMasters[i].ColumnName });
                }
            }
            this.currtDocTableColName[index] = columnNames;
        }
    }

    SetKeyValue(vals: any, index: any) {
        const keyValue = this.formData.controls[index].get('isKeyColumnEnabled').value;
        if (keyValue) {
            this.formData.controls[index].get('rowNumber').setValue('');
            vals.target.parentElement.classList.add('showinput');
        } else {
            vals.target.parentElement.classList.remove('showinput');
        }

    }

    DataTableInitRuleDocTypesChanged(value: any, index: any) {
        let docVal = '';
        if (isTruthy(value) && typeof (value) === 'string') {
            docVal = value;
        } else {
            docVal = this.formData.controls[index].get('generalDocumentTypes').value;
        }

        this.formData.controls[index].get('tableName').setValue('');
        this.formData.controls[index].get('columnName').setValue('');
        const tableNames = [];
        const items = this.genDocTypes.filter(x => x.text === docVal);
        const i = items[0].id;
        this.dataTableRuleMasters.forEach(ele => {
            if (i === ele.DocID) {
                const dupTable = tableNames.filter(x => x.text === ele.TableName);
                if (dupTable.length === 0) {
                    tableNames.push({ id: ele.TableName, text: ele.TableName });
                }
            }
        });
        this.currtDocTables[index] = tableNames;

        if (tableNames.length === 0) {
            this._notificationService.showError('No Tables Found');
        }
    }

    checkOperatorFunction(formData: any) {
        const operatorArray = [];
        formData.forEach(ele => {
            if (ele.docFieldOperator !== '') {
                operatorArray.push(ele.docFieldOperator);
            }
        });
        return (operatorArray.length > 0 && operatorArray.length === formData.length - 1);
    }

    addRules() {
        this.formData.push(this._fb.group({
            fieldsCustomValues: [''],
            openBrace: [''],
            generalDocumentTypes: [''],
            docField: [''],
            valueDocField: [''],
            closeBrace: [''],
            docFieldOperator: [''],
            tableName: [''],
            columnName: [''],
            rowNumber: [''],
            fieldOrTableSelect: ['Field'],
            isKeyColumnEnabled: [''],
            LosdocField: ['']
        }));
        this.currtDocFields.push([]);
        this.currtDocTables.push([]);
    }

    onDataTableEditCheckboxChange(evt: any, i: any) {
        if (evt.currentTarget.checked === true && evt.currentTarget.value === '(') {
            this.formData.controls[i].get('openBrace').setValue('(');
        } else {
            evt.currentTarget.checked = false;
            this.formData.controls[i].get('openBrace').setValue('');
        }
    }

    onDataTableCloseEditCheckboxChange(evt: any, i: any) {
        if (evt.currentTarget.checked === true && evt.currentTarget.value === ')') {
            this.formData.controls[i].get('closeBrace').setValue(')');
        } else {
            evt.currentTarget.checked = false;
            this.formData.controls[i].get('closeBrace').setValue('');
        }
    }

    OnChangeFieldValue(index: number) {
        const SearchValue = this.formData.controls[index].get('LosdocField').value;
        const LosDocumentName = this.formData.controls[index].get('generalDocumentTypes').value;
        let LosDocumentId;
        this.genDocTypes.forEach((a) => {
            if (a.text === LosDocumentName) {
                LosDocumentId = a.id;
            }
        });
        this._ruleBuilderService.GetLosDocFields(LosDocumentId, SearchValue);
    }

    ngOnDestroy(): void {
        this._subscriptions.forEach(ele => {
            ele.unsubscribe();
        });
    }
}
