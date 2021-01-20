import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { ChecklistItemRowData } from 'src/app/modules/loantype/models/checklist-items-table.model';
import { AddLoanTypeService } from 'src/app/modules/loantype/service/add-loantype.service';
import { NotificationService } from '@mts-notification';
import { RuleBuilderService } from 'src/app/modules/loantype/service/rule-builder.service';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { AppSettings, FormulaBuilderTypesConstant } from '@mts-app-setting';
import { CommonService } from 'src/app/shared/common';
import { RuleCheckValidation, SingleOperatorExistforRule } from 'src/app/modules/loantype/pipes';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';
import { element } from 'protractor';

@Component({
    selector: 'mts-losrule-formula-builder',
    styleUrls: ['losrule-formula-builder.page.css'],
    templateUrl: 'losrule-formula-builder.page.html',
    providers: [RuleCheckValidation, SingleOperatorExistforRule]
})
export class LOSRuleFormulaBuilderComponent implements OnInit, OnDestroy {

    LOSTypeNames: string[] = ['Encompass'];
    LOSRuleType = '';
    rulesFrmGrp: FormGroup;
    docFieldMasters: any[] = [];
    genDocTypes: any[] = [];
    currtDocFields: any[] = [];
    rowData: ChecklistItemRowData = new ChecklistItemRowData();
    waitingForData = true;
    FieldErrorMsg = 'The value entered needs to match exactly with the selected Field value';
    isErrMsgs = false;
    RuleOperator: any[] = FormulaBuilderTypesConstant.GeneralRuleOperators.slice();
    losEncompassFields: any[] = [];
    headerFieldID = '';
    ErrorMsg = '';

    LosDocumentFields: any[] = [];
    FannieMaeDocName: string = AppSettings.RuleFannieMaeDocName;

    constructor(
        private _commonRuleBuilderService: CommonRuleBuilderService,
        private _ruleBuilderService: RuleBuilderService,
        private _checkrule: RuleCheckValidation,
        private _commonService: CommonService,
        private _singOperatorCheck: SingleOperatorExistforRule,
        private _fb: FormBuilder
    ) {
        this._ruleBuilderService.setRuleFormGroup('losrule');
    }

    private _subscriptions: Subscription[] = [];
    private _operators: any[] = FormulaBuilderTypesConstant.Operators.slice();
    get formData() { return this.rulesFrmGrp.get('losRule') as FormArray; }

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
                this.waitingForData = false;

                if (isTruthy(this.rowData.RuleJsonObject) && isTruthy(this.rowData.RuleJsonObject.losRule) && this.rowData.RuleJsonObject.losRule.length > 0) {
                    this.GenerateFormValues();
                }
            }));
        this._subscriptions.push(this._commonService.SystemLOSFields.subscribe((res: any[]) => {
            this.losEncompassFields = res;
        }));
        this._ruleBuilderService.getSysLoanTypeDocuments();

        this._subscriptions.push(this.rulesFrmGrp.valueChanges.subscribe((form) => {
            let str = '';
            let ruleFormationValues = this.LOSRuleType + '([Rule])';
            let filterResult = false;
            let losRuleOperatorCheck;
            let totalEditOperatorCount;
            let getAllEditRuleOperatorsCount;
            let finalEditOperatorCount;

            let losdocTypeFieldFlag = false;
            form.losRule.forEach(elements => {
                if (elements.loslookupvalueDocField !== undefined && elements.loslookupvalueDocField !== '') {
                    str += elements.openBrace + elements.loslookupvalueDocField + elements.closeBrace + elements.losDocFieldOperator;
                } else if (elements.isLOSFieldsSelected && elements.losDocFieldId !== '') {
                    str += elements.openBrace + '[' + elements.losDocFieldId + ']' + elements.closeBrace + elements.losDocFieldOperator;
                } else if (elements.losLookUpDocumentTypes !== '' && isTruthy(elements.losValuesField) && typeof (elements.losValuesField) === 'string' && elements.losValuesField !== '' && elements.losLookUpDocumentTypes !== AppSettings.RuleFannieMaeDocName) {
                    str += elements.openBrace + '[' + elements.losLookUpDocumentTypes + '.' + elements.losValuesField + ']' + elements.closeBrace + elements.losDocFieldOperator;
                } else if (elements.losLookUpDocumentTypes !== '' && isTruthy(elements.losValuesField) && Array.isArray(elements.losValuesField) && elements.losValuesField.length > 0 && elements.losLookUpDocumentTypes !== AppSettings.RuleFannieMaeDocName) {
                    str += elements.openBrace + '[' + elements.losLookUpDocumentTypes + '.' + elements.losValuesField[0].text + ']' + elements.closeBrace + elements.losDocFieldOperator;
                } else if (elements.losLookUpDocumentTypes !== '' && isTruthy(elements.LosdocField) && typeof (elements.LosdocField) === 'string' && elements.LosdocField !== '' && elements.losLookUpDocumentTypes === AppSettings.RuleFannieMaeDocName) {
                    str += elements.openBrace + '[' + AppSettings.FannieMaeDocDisplayName + '.' + elements.LosdocField + ']' + elements.closeBrace + elements.losDocFieldOperator;
                } else if (elements.losLookUpDocumentTypes !== '' && isTruthy(elements.LosdocField) && Array.isArray(elements.LosdocField) && elements.LosdocField.length > 0 && elements.losLookUpDocumentTypes === AppSettings.RuleFannieMaeDocName) {
                    str += elements.openBrace + '[' + AppSettings.FannieMaeDocDisplayName + '.' + elements.LosdocField[0].text + ']' + elements.closeBrace + elements.losDocFieldOperator;
                } else if (elements.losLookUpDocumentTypes === '' || (elements.losValuesField === '' && elements.LosdocField === '') || elements.losDocFieldOperator === '') {
                    this._commonRuleBuilderService.ruleBuilderNext.next(true);
                    losdocTypeFieldFlag = true;
                } else {
                    this._commonRuleBuilderService.ruleBuilderNext.next(true);
                }
            });
            if (str === '') {
                this._commonRuleBuilderService.ruleBuilderNext.next(true);
            } else {
                if (!losdocTypeFieldFlag) {
                    ruleFormationValues = this._ruleBuilderService.replaceFormula(ruleFormationValues, str);
                    const strReplaced = str.split(']');

                    if (strReplaced.length > 1 && this._operators.includes(strReplaced[strReplaced.length - 1])) {
                        losRuleOperatorCheck = false;
                    } else if (form.losRule.length === 1) {
                        losRuleOperatorCheck = false;
                    } else {
                        losRuleOperatorCheck = this._checkrule.transform(str, this._operators);
                        losRuleOperatorCheck = this.CheckLOSOperatorFunction(form.losRule);
                    }

                    totalEditOperatorCount = this._singOperatorCheck.transform(str, this._operators);
                    getAllEditRuleOperatorsCount = this._singOperatorCheck.transform(totalEditOperatorCount[0]['Vals'].join(), this._operators);
                    finalEditOperatorCount = totalEditOperatorCount[1]['operatorsCount'] - getAllEditRuleOperatorsCount[1]['operatorsCount'];
                    filterResult = ruleFormationValues.match(/\(/g).length === ruleFormationValues.match(/\)/g).length;
                    if (filterResult && losRuleOperatorCheck === true && finalEditOperatorCount === totalEditOperatorCount[0]['Vals'].length - 1 && totalEditOperatorCount[0]['Vals']['length'] !== 1 && totalEditOperatorCount[1]['operatorsCount'] !== 0) {
                        this.ErrorMsg = '';
                        this.rowData.RuleDescription = ruleFormationValues;
                        this._commonRuleBuilderService.ruleBuilderNext.next(false);
                    } else {
                        if (!filterResult) {
                            this.ErrorMsg = 'Brackets are not closed properly, please check the Rule';
                            this._commonRuleBuilderService.ruleBuilderNext.next(true);
                        }
                        if (!losRuleOperatorCheck) {
                            this.ErrorMsg = 'Rule must not end with an operator, please check the rule';
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

    CheckLOSOperatorFunction(formData: any) {
        const operatorArray = [];
        formData.forEach(ele => {
            if (ele.losDocFieldOperator !== '') {
                operatorArray.push(ele.losDocFieldOperator);
            }
        });
        return operatorArray.length > 0 && operatorArray.length === formData.length - 1;
    }

    OnChangeLOSHeaderFieldValue(i: any) {
        const losFieldID = this.formData.controls[i].get('losDocFieldId').value;
        if (isTruthy(losFieldID)) {
            this._commonService.GetSysLOSFields(losFieldID);
        }
    }

    EncompassFieldsOnSelect(field, i: any) {
        this.formData.controls[i].get('losDocFieldId').setValue(field.value);
        this.formData.controls[i].get('isLOSFieldsSelected').setValue(true);
    }

    removeRules(i: any) {
        this.formData.removeAt(i);
    }

    onLOSEditCheckboxChange(OpenBrace: any, i: any) {
        if (OpenBrace.currentTarget.checked === true) {
            if (OpenBrace.currentTarget.value === '(') {
                this.formData.controls[i].get('openBrace').setValue('(');
            }
        } else {
            OpenBrace.currentTarget.checked = false;
            this.formData.controls[i].get('openBrace').setValue('');
        }
    }

    onLOSCloseEditCheckboxChange(CloseBrace: any, i: any) {
        if (CloseBrace.currentTarget.checked === true) {
            if (CloseBrace.currentTarget.value === ')') {
                this.formData.controls[i].get('closeBrace').setValue(')');
            }
        } else {
            CloseBrace.currentTarget.checked = false;
            this.formData.controls[i].get('closeBrace').setValue('');
        }
    }

    DocTypesChanged(genVals: any, i: any) {
        const DocName: string = ((typeof (genVals) === 'string') ? genVals : genVals.target.selectedOptions[0].innerText);
        if (DocName !== AppSettings.RuleFannieMaeDocName) {
            this._ruleBuilderService.docFieldsInitChange(this.formData.controls[i], this.currtDocFields, genVals, 'losValuesField', i);
        }
    }

    ChangeFieldOrLOS(vals: any, index: any) {
        if (vals.target.value === 'Value') {
            this.isErrMsgs = true;
            this.formData.controls[index].get('fieldOrLOSSelect').setValue('Value');
            $('#valueDisplay_' + index).show();
            $('#fieldDisplay_' + index).hide();
            $('#losDisplay_' + index).hide();
            this.formData.controls[index].get('losDocFieldId').setValue('');
            this.formData.controls[index].get('losLookUpDocumentTypes').setValue('');
            this.formData.controls[index].get('loslookupvalueDocField').setValue('');
            this.formData.controls[index].get('losDocFieldOperator').setValue('');

        } else if (vals.target.value === 'Field') {
            this.formData.controls[index].get('fieldOrLOSSelect').setValue('Field');
            $('#fieldDisplay_' + index).show();
            $('#valueDisplay_' + index).hide();
            $('#losDisplay_' + index).hide();
            this.formData.controls[index].get('loslookupvalueDocField').setValue('');
            this.formData.controls[index].get('losDocFieldId').setValue('');
            this.formData.controls[index].get('losDocFieldOperator').setValue('');
            this.formData.controls[index].get('losLookUpDocumentTypes').setValue('');
        } else if (vals.target.value === 'LOS') {
            this.formData.controls[index].get('fieldOrLOSSelect').setValue('LOS');
            $('#fieldDisplay_' + index).hide();
            $('#valueDisplay_' + index).hide();
            $('#losDisplay_' + index).show();
            this.formData.controls[index].get('loslookupvalueDocField').setValue('');
            this.formData.controls[index].get('losDocFieldId').setValue('');
            this.formData.controls[index].get('losDocFieldOperator').setValue('');
            this.formData.controls[index].get('losLookUpDocumentTypes').setValue('');
        }
    }

    GenerateFormValues() {
        let i = 0;
        this.rowData.RuleJsonObject.losRule.forEach(ele => {
            this.addRules();
            this.formData.controls[i].get('losType').setValue(ele.losType);
            if (ele.fieldOrLOSSelect === 'LOS') {
                this.LOSRuleType = 'Encompass';
                this.formData.controls[i].get('isLOSFieldsSelected').setValue(true);
                this.formData.controls[i].get('openBrace').setValue(ele.openBrace);
                this.formData.controls[i].get('closeBrace').setValue(ele.closeBrace);
                this.formData.controls[i].get('fieldOrLOSSelect').setValue(ele.fieldOrLOSSelect);
                this.formData.controls[i].get('losDocFieldId').setValue(ele.losDocFieldId);
                this.formData.controls[i].get('losDocFieldOperator').setValue(ele.losDocFieldOperator);
            }
            if (ele.fieldOrLOSSelect === 'Field') {
                this.formData.controls[i].get('openBrace').setValue(ele.openBrace);
                this.formData.controls[i].get('closeBrace').setValue(ele.closeBrace);
                this.formData.controls[i].get('fieldOrLOSSelect').setValue(ele.fieldOrLOSSelect);
                this.formData.controls[i].get('losLookUpDocumentTypes').setValue(ele.losLookUpDocumentTypes);
                this.DocTypesChanged(ele.losLookUpDocumentTypes, i);

                if (isTruthy(ele.losValuesField)) {
                    this.formData.controls[i].get('losValuesField').setValue(ele.losValuesField[0].id);
                }
                if (isTruthy(ele.LosdocField)) {
                    this.formData.controls[i].get('LosdocField').setValue(ele.LosdocField);
                }
                this.formData.controls[i].get('losDocFieldOperator').setValue(ele.losDocFieldOperator);
            }
            if (ele.fieldOrLOSSelect === 'Value') {
                this.formData.controls[i].get('openBrace').setValue(ele.openBrace);
                this.formData.controls[i].get('closeBrace').setValue(ele.closeBrace);
                this.formData.controls[i].get('fieldOrLOSSelect').setValue(ele.fieldOrLOSSelect);
                this.formData.controls[i].get('loslookupvalueDocField').setValue(ele.loslookupvalueDocField);
                this.formData.controls[i].get('losDocFieldOperator').setValue(ele.losDocFieldOperator);
            }
            i++;
        });
        this.LOSRuleType = this.formData.controls[0].get('losType').value;
    }

    addRules() {
        this.formData.push(this._fb.group({
            losType: [''],
            losfieldsCustomValues: [''],
            losvalueDocField: [''],
            losDocumentTypes: [''],
            losDocField: [''],
            losDocFieldId: [''],
            loslookupfieldsCustomValues: [''],
            losLookUpDocumentTypes: [''],
            loslookupvalueDocField: [''],
            losValuesField: [''],
            losDisValField: [''],
            losDocFieldOperator: [''],
            fieldOrLOSSelect: ['LOS'],
            openBrace: [''],
            closeBrace: [''],
            isLOSFieldsSelected: [''],
            LosdocField: ['']
        }));
        this.currtDocFields.push([]);
    }

    ChangeTypeOfLos() {
        switch (this.LOSRuleType) {
            case 'Encompass':
                this._commonRuleBuilderService.ruleExpression.next('Encompass([Rule])');
                this.addRules();
                this.formData.controls[0].get('losType').setValue(this.LOSRuleType);
                break;
            case '':
                this.currtDocFields = [];
                const formLength = this.formData.controls.length;
                for (let index = 0; index < formLength; index++) {
                    this.formData.removeAt(0);
                }

                this._commonRuleBuilderService.ruleExpression.next('los[(Rule)]');
                break;
            default:
                break;
        }
    }

    OnChangeFieldValue(index: number) {
        const SearchValue = this.formData.controls[index].get('LosdocField').value;
        const LosDocumentName = this.formData.controls[index].get('losLookUpDocumentTypes').value;
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
