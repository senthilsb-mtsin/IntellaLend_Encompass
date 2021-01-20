import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { AppSettings, FormulaBuilderTypesConstant } from '@mts-app-setting';
import { ChecklistItemRowData } from 'src/app/modules/loantype/models/checklist-items-table.model';
import { RuleBuilderService } from 'src/app/modules/loantype/service/rule-builder.service';
import { AddLoanTypeService } from 'src/app/modules/loantype/service/add-loantype.service';
import { RuleCheckValidation, SingleOperatorExistforRule } from 'src/app/modules/loantype/pipes';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';
import { element } from 'protractor';

@Component({
    selector: 'mts-groupby-formula-builder',
    styleUrls: ['groupby-formula-builder.page.css'],
    templateUrl: 'groupby-formula-builder.page.html',
    providers: [RuleCheckValidation, SingleOperatorExistforRule]
})
export class GroupByFormulaBuilderComponent implements OnInit, OnDestroy {
    rulesFrmGrp: FormGroup;
    docFieldMasters: any[] = [];
    genDocTypes: any[] = [];
    RuleOperator: any[] = FormulaBuilderTypesConstant.GeneralRuleOperators.slice();

    FieldErrorMsg = 'The value entered needs to match exactly with the selected Field value';
    isErrMsgs = false;
    ErrorMsg = '';
    currtDocFields: any[] = [];
    currtGroupByField: any[] = [];
    currtOrderByField: any[] = [];
    currtGroupField: any[] = [];
    groupByRuleOperator: any = [{ id: 'sum', value: 'sum' }, { id: 'avg', value: 'avg' }];

    LosDocumentFields: any[] = [];
    FannieMaeDocName: string = AppSettings.RuleFannieMaeDocName;

    rowData: ChecklistItemRowData = new ChecklistItemRowData();
    waitingForData = true;
    constructor(
        private _ruleBuilderService: RuleBuilderService,
        private _commonRuleBuilderService: CommonRuleBuilderService,
        private _checkrule: RuleCheckValidation,
        private _checkruleop: SingleOperatorExistforRule,
        private _fb: FormBuilder
    ) {
        this._ruleBuilderService.setRuleFormGroup('groupby');
    }

    private _subscriptions: Subscription[] = [];
    private operator: any[] = FormulaBuilderTypesConstant.Operators.slice();

    get formData() { return this.rulesFrmGrp.get('groupby') as FormArray; }

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
                this.waitingForData = !this.waitingForData;

                if (isTruthy(this.rowData.RuleJsonObject) && isTruthy(this.rowData.RuleJsonObject.groupby) && this.rowData.RuleJsonObject.groupby.length > 0) {
                    this.GenerateFormValues();
                }
            }));
        this._ruleBuilderService.getSysLoanTypeDocuments();

        this._subscriptions.push(this.rulesFrmGrp.valueChanges.subscribe((form) => {
            let str = '';
            let ruleFormationValues = '([Rule])';
            let docTypeFieldFlag = false;
            let groupByOperatorCheck;
            form.groupby.forEach(elements => {
                if (elements.generalDocumentTypes !== '' && typeof (elements.docField) === 'string' && elements.docField !== '' && elements.generalDocumentTypes !== AppSettings.RuleFannieMaeDocName) {
                    str += elements.openBrace + '[' + elements.generalDocumentTypes + '.' + elements.docField + ']' + elements.closeBrace + elements.docFieldOperator;
                } else if (elements.generalDocumentTypes !== '' && Array.isArray(elements.docField) && elements.docField.length > 0 && elements.generalDocumentTypes !== AppSettings.RuleFannieMaeDocName) {
                    str += elements.openBrace + '[' + elements.generalDocumentTypes + '.' + elements.docField[0].text + ']' + elements.closeBrace + elements.docFieldOperator;
                } else if (elements.generalDocumentTypes !== '' && typeof (elements.LosdocField) === 'string' && elements.LosdocField !== '' && elements.generalDocumentTypes === AppSettings.RuleFannieMaeDocName) {
                    str += elements.openBrace + '[' + AppSettings.FannieMaeDocDisplayName + '.' + elements.LosdocField + ']' + elements.closeBrace + elements.docFieldOperator;
                } else if (elements.generalDocumentTypes !== '' && Array.isArray(elements.LosdocField) && elements.LosdocField.length > 0 && elements.generalDocumentTypes === AppSettings.RuleFannieMaeDocName) {
                    str += elements.openBrace + '[' + AppSettings.FannieMaeDocDisplayName + '.' + elements.LosdocField[0].text + ']' + elements.closeBrace + elements.docFieldOperator;
                } else if (elements.valueDocField !== '') {
                    str += elements.openBrace + elements.valueDocField + elements.closeBrace + elements.docFieldOperator;
                } else if (elements.fieldOrGroupSelect === 'Group' && elements.groupByField !== '' && elements.orderByField !== '' && elements.groupField !== '') {
                    str += elements.openBrace + '{' + elements.groupByMainOperator + '([' + elements.generalDocumentTypes + ']' + '|' + 'groupby' + '([' + elements.groupByField + '])' + '|' + 'orderby' + '([' + elements.orderByField + '])' + '|' + 'field' + '([' + elements.groupField + '])' + ')}' + elements.closeBrace + elements.docFieldOperator;
                } else if (elements.generalDocumentTypes === '' || (elements.docField === '' && elements.LosdocField === '') || elements.docFieldOperator === '') {
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
                    if (strReplaced.length > 1 && this.operator.includes(strReplaced[strReplaced.length - 1])) { groupByOperatorCheck = false; } else if (form.groupby.length === 1) { groupByOperatorCheck = false; } else {
                        groupByOperatorCheck = this._checkrule.transform(str, this.operator);
                        groupByOperatorCheck = this.checkOperatorFunction(form.groupby);
                    }
                    const isValidRule = ruleFormationValues.match(/\(/g).length === ruleFormationValues.match(/\)/g).length;
                    const totalOperatorCount = this._checkruleop.transform(str, this.operator);
                    const getAllRuleOperatorsCount = this._checkruleop.transform(totalOperatorCount[0]['Vals'].join(), this.operator);
                    const finalOperatorCount = totalOperatorCount[1]['operatorsCount'] - getAllRuleOperatorsCount[1]['operatorsCount'];

                    if (isValidRule && groupByOperatorCheck && finalOperatorCount === totalOperatorCount[0]['Vals'].length - 1 && totalOperatorCount[0]['Vals']['length'] !== 1 && totalOperatorCount[1]['operatorsCount'] !== 0) {
                        this.ErrorMsg = '';
                        this.rowData.RuleDescription = ruleFormationValues;
                        this._commonRuleBuilderService.ruleBuilderNext.next(false);
                    } else {
                        if (!isValidRule) {
                            this.ErrorMsg = 'Brackets are not closed properly, please check the Rule';
                            this._commonRuleBuilderService.ruleBuilderNext.next(true);
                        }
                        if (groupByOperatorCheck === false) {
                            this.ErrorMsg = 'Rule must not end with an Operator, please check the Rule';
                            this._commonRuleBuilderService.ruleBuilderNext.next(true);
                        }
                        if (finalOperatorCount !== totalOperatorCount[0]['Vals'].length - 1 || totalOperatorCount[0]['Vals']['length'] === 1 && finalOperatorCount === 0) {
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

    GenerateFormValues() {
        let i = 0;
        this.rowData.RuleJsonObject.groupby.forEach(ele => {
            this.addRules();
            if (ele.fieldOrGroupSelect === 'Field') {
                this.formData.controls[i].get('fieldOrGroupSelect').setValue(ele.fieldOrGroupSelect);
                this.formData.controls[i].get('openBrace').setValue(ele.openBrace);
                this.GroupByAllInitEditDocTypesChanged(ele.generalDocumentTypes, i);
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
            if (ele.fieldOrGroupSelect === 'Value') {
                this.isErrMsgs = true;
                this.formData.controls[i].get('openBrace').setValue(ele.openBrace);
                this.formData.controls[i].get('fieldOrGroupSelect').setValue(ele.fieldOrGroupSelect);
                this.formData.controls[i].get('valueDocField').setValue(ele.valueDocField);
                this.formData.controls[i].get('closeBrace').setValue(ele.closeBrace);
                this.formData.controls[i].get('docFieldOperator').setValue(ele.docFieldOperator);
            }

            if (ele.fieldOrGroupSelect === 'Group') {
                this.formData.controls[i].get('openBrace').setValue(ele.openBrace);
                this.formData.controls[i].get('fieldOrGroupSelect').setValue(ele.fieldOrGroupSelect);
                this.formData.controls[i].get('groupByMainOperator').setValue(ele.groupByMainOperator);
                this.GroupByAllInitEditDocTypesChanged(ele.generalDocumentTypes, i);
                this.formData.controls[i].get('generalDocumentTypes').setValue(ele.generalDocumentTypes);
                this.formData.controls[i].get('groupByField').setValue(ele.groupByField[0].id);
                this.formData.controls[i].get('orderByField').setValue(ele.orderByField[0].id);
                this.formData.controls[i].get('groupField').setValue(ele.groupField[0].id);
                this.formData.controls[i].get('closeBrace').setValue(ele.closeBrace);
                this.formData.controls[i].get('docFieldOperator').setValue(ele.docFieldOperator);
            }
            i++;
        });
    }

    GroupByAllInitEditDocTypesChanged(value: any, index: any) {
        const tempGeneralEditDocFieldMasters = [];
        this.formData.controls[index].get('groupByField').setValue('');
        this.formData.controls[index].get('orderByField').setValue('');
        this.formData.controls[index].get('groupField').setValue('');

        let newVals = '';
        if (typeof (value) !== 'string' && (value.currentTarget.value !== '' && value.currentTarget.value !== 'Select')) {
            newVals = value.target.selectedOptions[0].innerText.replace(/[\s]/g, '');
        } else if (value !== '' && value !== 'Select') {
            newVals = value.replace(/[\s]/g, '');
        }

        if (isTruthy(newVals)) {
            for (let i = 0; i < this.docFieldMasters.length; i++) {
                const trmmiedSpaceDocName = this.docFieldMasters[i].DocName.replace(/[\s]/g, '');
                if (newVals === trmmiedSpaceDocName) {
                    tempGeneralEditDocFieldMasters.push(this.docFieldMasters[i].Name);
                }
            }
            this.currtDocFields[index] = tempGeneralEditDocFieldMasters;
            this.currtOrderByField[index] = tempGeneralEditDocFieldMasters;
            this.currtGroupField[index] = tempGeneralEditDocFieldMasters;
            this.currtGroupByField[index] = tempGeneralEditDocFieldMasters;
        }
    }

    ChangeFieldOrGroup(vals: any, index: any) {
        if (vals.target.value === 'Value') {
            this.isErrMsgs = true;
            this.formData.controls[index].get('fieldOrGroupSelect').setValue('Value');
            this.formData.controls[index].get('docField').setValue('');
            this.formData.controls[index].get('LosdocField').setValue('');
            this.formData.controls[index].get('generalDocumentTypes').setValue('');
            this.formData.controls[index].get('fieldsCustomValues').setValue(true);
            this.formData.controls[index].get('docFieldOperator').setValue('');
        } else if (vals.target.value === 'Field') {
            this.formData.controls[index].get('fieldOrGroupSelect').setValue('Field');
            this.formData.controls[index].get('valueDocField').setValue('');
            this.formData.controls[index].get('fieldsCustomValues').setValue('');
            this.formData.controls[index].get('generalDocumentTypes').setValue('');
            this.formData.controls[index].get('docFieldOperator').setValue('');
        } else if (vals.target.value === 'Group') {
            this.formData.controls[index].get('fieldOrGroupSelect').setValue('Group');
            this.formData.controls[index].get('groupByMainOperator').setValue('');
            this.formData.controls[index].get('groupByField').setValue('');
            this.formData.controls[index].get('orderByField').setValue('');
            this.formData.controls[index].get('groupField').setValue('');
            this.formData.controls[index].get('docFieldOperator').setValue('');
            this.formData.controls[index].get('generalDocumentTypes').setValue('');
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

    openCheckboxChange(OpenBrace: any, i: any) {
        if (OpenBrace.currentTarget.checked === true) {
            if (OpenBrace.currentTarget.value === '(') {
                this.formData.controls[i].get('openBrace').setValue('(');
            }
        } else {
            OpenBrace.currentTarget.checked = false;
            this.formData.controls[i].get('openBrace').setValue('');
        }
    }

    closeCheckboxChange(CloseBrace: any, i: any) {
        if (CloseBrace.currentTarget.checked === true) {
            if (CloseBrace.currentTarget.value === ')') {
                this.formData.controls[i].get('closeBrace').setValue(')');
            }
        } else {
            CloseBrace.currentTarget.checked = false;
            this.formData.controls[i].get('closeBrace').setValue('');
        }
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
            groupByField: [''],
            orderByField: [''],
            groupField: [''],
            fieldOrGroupSelect: ['Field'],
            groupByMainOperator: [''],
            LosdocField: ['']
        }));
        this.currtDocFields.push([]);
        this.currtGroupByField.push([]);
        this.currtOrderByField.push([]);
        this.currtGroupField.push([]);
    }

    removeRules(i: any) {
        this.formData.removeAt(i);
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
