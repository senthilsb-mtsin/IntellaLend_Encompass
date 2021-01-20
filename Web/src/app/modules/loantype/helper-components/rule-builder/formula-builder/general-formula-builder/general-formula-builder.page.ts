import { Component, OnInit, OnDestroy } from '@angular/core';
import { RuleBuilderService } from 'src/app/modules/loantype/service/rule-builder.service';
import { AddLoanTypeService } from 'src/app/modules/loantype/service/add-loantype.service';
import { FormGroup, FormArray, FormBuilder } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AppSettings, FormulaBuilderTypesConstant } from '@mts-app-setting';
import { RuleCheckValidation, SingleOperatorExistforRule } from '../../../../pipes';
import { ChecklistItemRowData } from 'src/app/modules/loantype/models/checklist-items-table.model';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';
import { element } from 'protractor';

@Component({
    selector: 'mts-general-formula-builder',
    styleUrls: ['general-formula-builder.page.css'],
    templateUrl: 'general-formula-builder.page.html',
    providers: [RuleCheckValidation, SingleOperatorExistforRule]
})
export class GeneralFormulaBuilderComponent implements OnInit, OnDestroy {
    rulesFrmGrp: FormGroup;
    docFieldMasters: any[] = [];
    genDocTypes: any[] = [];
    RuleOperator: any[] = FormulaBuilderTypesConstant.GeneralRuleOperators.slice();

    FieldErrorMsg = '';
    isErrMsgs = false;
    ErrorMsg = '';
    currtDocFields: any[] = [];
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
        this._ruleBuilderService.setRuleFormGroup('general');
    }

    private _subscriptions: Subscription[] = [];
    private operator: any[] = FormulaBuilderTypesConstant.Operators.slice();

    get formData() { return <FormArray>this.rulesFrmGrp.get('generalRule'); }

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

                if (isTruthy(this.rowData.RuleJsonObject) && isTruthy(this.rowData.RuleJsonObject.generalRule)) {
                    this.GenerateFormValues();
                }
            }));
        this._ruleBuilderService.getSysLoanTypeDocuments();

        this._subscriptions.push(this.rulesFrmGrp.valueChanges.subscribe((form) => {

            let str = '';
            let ruleFormationValues = '([Rule])';
            let docTypeFieldFlag = false;
            let subStr = '';

            form.generalRule.forEach(elements => {
                if (elements.generalDocumentTypes !== '' && elements.docField !== '' && elements.generalDocumentTypes !== AppSettings.RuleFannieMaeDocName && !elements.isexist) {
                    str += elements.openBrace + '[' + elements.generalDocumentTypes + '.' + elements.docField + ']' + elements.closeBrace + elements.docFieldOperator;
                    subStr += elements.docFieldOperator;
                } else if (elements.generalDocumentTypes !== '' && elements.LosdocField !== '' && elements.generalDocumentTypes === AppSettings.RuleFannieMaeDocName && !elements.isexist) {
                    str += elements.openBrace + '[' + AppSettings.FannieMaeDocDisplayName + '.' + elements.LosdocField + ']' + elements.closeBrace + elements.docFieldOperator;
                    subStr += elements.docFieldOperator;
                } else if (elements.valueDocField !== '' && !elements.isexist) {
                    str += elements.openBrace + elements.valueDocField + elements.closeBrace + elements.docFieldOperator;
                    subStr += elements.docFieldOperator;
                } else if (elements.generalDocumentTypes !== '' && elements.isexist) {
                    str += elements.openBrace + 'isexist([' + elements.generalDocumentTypes + '])' + elements.closeBrace + elements.docFieldOperator;
                    subStr += 'isexist' + elements.docFieldOperator;
                } else if (elements.generalDocumentTypes === '' || elements.docField === '' || elements.docFieldOperator === '') {
                    this._commonRuleBuilderService.ruleBuilderNext.next(true);
                    docTypeFieldFlag = true;
                }
            });
            if (str === '') {
                this._commonRuleBuilderService.ruleBuilderNext.next(true);
            } else {
                if (!docTypeFieldFlag) {
                    ruleFormationValues = this._ruleBuilderService.replaceFormula(ruleFormationValues, str);
                    const generalRuleOperatorCheck = this._checkrule.transform(str, this.operator);
                    const filterResult = ruleFormationValues.match(/\(/g).length === ruleFormationValues.match(/\)/g).length;
                    const totalOperatorCount = this._checkruleop.transform(str, this.operator);
                    const getAllRuleOperatorsCount = this._checkruleop.transform(totalOperatorCount[0]['Vals'].join(), this.operator);
                    const finalOperatorCount = totalOperatorCount[1]['operatorsCount'] - getAllRuleOperatorsCount[1]['operatorsCount'];

                    if (!filterResult) {
                        this.ErrorMsg = 'Brackets are not closed properly, please check the rule';
                        this._commonRuleBuilderService.ruleBuilderNext.next(true);
                    } else if (!generalRuleOperatorCheck) {
                        this.ErrorMsg = 'Rule must not end with an operator, please check the rule';
                        this._commonRuleBuilderService.ruleBuilderNext.next(true);
                    } else if ((finalOperatorCount !== totalOperatorCount[0]['Vals'].length - 1) || (totalOperatorCount[0]['Vals']['length'] === 1) && (finalOperatorCount === 0)) {
                        this.ErrorMsg = 'Please select atleast one operator';
                        this._commonRuleBuilderService.ruleBuilderNext.next(true);
                    } else if (subStr.includes('isexist')) {
                        if (new RegExp(/^isexist$/).test(subStr) || !new RegExp(/[.]/).test(str)) {
                            this.ErrorMsg = 'Rule must not contain only isexist';
                            this._commonRuleBuilderService.ruleBuilderNext.next(true);
                        } else if (!(new RegExp(/[||,&&]+isexist/).test(subStr) || new RegExp(/isexist+[||,&&]/).test(subStr))) {
                            this.ErrorMsg = 'Isexist must be compared using boolean operator';
                            this._commonRuleBuilderService.ruleBuilderNext.next(true);
                        } else if (!(new RegExp(/[=,!=,>,<,>=,<=]+[||,&&]+isexist/).test(subStr) ||
                                    new RegExp(/isexist+[||,&&]+[=,!=,>,<,>=,<=]/).test(subStr)) ||
                                    (new RegExp(/isexist+[=,!=,>,<,>=,<=]/).test(subStr) ||
                                    new RegExp(/[=,!=,>,<,>=,<=]+isexist/).test(subStr))) {
                            this.ErrorMsg = 'Isexist must be compared with boolean value';
                            this._commonRuleBuilderService.ruleBuilderNext.next(true);
                        } else {
                            this._commonRuleBuilderService.ruleBuilderNext.next(false);
                            this.ErrorMsg = '';
                            this.rowData.RuleDescription = str;
                        }
                    } else {
                        this._commonRuleBuilderService.ruleBuilderNext.next(false);
                        this.ErrorMsg = '';
                        this.rowData.RuleDescription = str;
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
        this.rowData.RuleJsonObject.generalRule.forEach(ele => {
            this.addRules();
            if ((ele.fieldsCustomValues === '' || ele.fieldsCustomValues === false) && (ele.isexist === undefined || ele.isexist === '' || ele.isexist === false)) {
                this.formData.controls[i].get('fieldsCustomValues').setValue(ele.fieldsCustomValues);
                this.formData.controls[i].get('isexist').setValue(false);
                this.formData.controls[i].get('openBrace').setValue(ele.openBrace);
                this.formData.controls[i].get('generalDocumentTypes').setValue(ele.generalDocumentTypes);
                this.GeneralDocTypesChanged(ele.generalDocumentTypes, i);
                if (isTruthy(ele.LosdocField)) {
                    this.formData.controls[i].get('LosdocField').setValue(ele.LosdocField);
                }
                if (isTruthy(ele.docField)) {
                    this.formData.controls[i].get('docField').setValue(ele.docField[0].id);
                }

                this.formData.controls[i].get('closeBrace').setValue(ele.closeBrace);
                this.formData.controls[i].get('docFieldOperator').setValue(ele.docFieldOperator);
            }
            if (ele.fieldsCustomValues === true && (ele.isexist === undefined || ele.isexist === '' || ele.isexist === false)) {
                $('#valueDisplayed_' + i).show();
                $('#valueDisplay_' + i).hide();
                this.formData.controls[i].get('fieldsCustomValues').setValue(ele.fieldsCustomValues);
                this.formData.controls[i].get('isexist').setValue(false);
                this.formData.controls[i].get('openBrace').setValue(ele.openBrace);
                this.formData.controls[i].get('valueDocField').setValue(ele.valueDocField);
                this.formData.controls[i].get('closeBrace').setValue(ele.closeBrace);
                this.formData.controls[i].get('docFieldOperator').setValue(ele.docFieldOperator);
            }
            if ((ele.fieldsCustomValues === '' || ele.fieldsCustomValues === false) && ele.isexist === true) {
                this.formData.controls[i].get('fieldsCustomValues').setValue(ele.fieldsCustomValues);
                this.formData.controls[i].get('isexist').setValue(ele.isexist);
                this.formData.controls[i].get('openBrace').setValue(ele.openBrace);
                this.formData.controls[i].get('generalDocumentTypes').setValue(ele.generalDocumentTypes);
                this.formData.controls[i].get('closeBrace').setValue(ele.closeBrace);
                this.formData.controls[i].get('docFieldOperator').setValue(ele.docFieldOperator);
            }
            i++;
        });
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
            LosdocField: [''],
            isexist: ['']
        }));
        this.currtDocFields.push([]);
    }

    FieldsChange(vals: any, i: any) {
        if (vals.currentTarget.value === 'Value') {
            this.isErrMsgs = true;
            this.FieldErrorMsg = 'The value entered needs to match exactly with the selected Field value';
            this.formData.controls[i].get('fieldsCustomValues').setValue(true);
            this.formData.controls[i].get('isexist').setValue(false);
            $('#valueDisplayed_' + i).show();
            $('#valueDisplay_' + i).hide();
            this.formData.controls[i].get('docField').setValue('');
            this.formData.controls[i].get('LosdocField').setValue('');
            this.formData.controls[i].get('generalDocumentTypes').setValue('');
            this.formData.controls[i].get('fieldsCustomValues').setValue(true);
        } else if (vals.currentTarget.value === 'Field') {
            this.formData.controls[i].get('fieldsCustomValues').setValue(false);
            this.formData.controls[i].get('isexist').setValue(false);
            $('#valueDisplay_' + i).show();
            $('#valueDisplayed_' + i).hide();
            this.formData.controls[i].get('generalDocumentTypes').setValue('');
            this.formData.controls[i].get('docField').setValue('');
            this.formData.controls[i].get('LosdocField').setValue('');
            this.formData.controls[i].get('valueDocField').setValue('');
            this.formData.controls[i].get('fieldsCustomValues').setValue('');
        } else if (vals.currentTarget.value === 'isexist') {
            this.formData.controls[i].get('fieldsCustomValues').setValue(false);
            this.formData.controls[i].get('isexist').setValue(true);
            $('#valueDisplay_' + i).show();
            $('#valueDisplayed_' + i).hide();
            this.formData.controls[i].get('generalDocumentTypes').setValue('');
            this.formData.controls[i].get('docField').setValue('');
            this.formData.controls[i].get('LosdocField').setValue('');
            this.formData.controls[i].get('valueDocField').setValue('');
            this.formData.controls[i].get('fieldsCustomValues').setValue('');
        }
    }

    removeRules(i: number) {
        this.formData.removeAt(i);
    }

    onGeneralCheckboxChange(OpenBrace: any, i: any) {
        if (OpenBrace.currentTarget.checked === true) {
            if (OpenBrace.currentTarget.value === '(') {
                this.formData.controls[i].get('openBrace').setValue('(');
            }
        } else {
            OpenBrace.currentTarget.checked = false;
            this.formData.controls[i].get('openBrace').setValue('');
        }
    }

    onGeneralCloseCheckboxChange(CloseBrace: any, i: any) {
        if (CloseBrace.currentTarget.checked === true) {
            if (CloseBrace.currentTarget.value === ')') {
                this.formData.controls[i].get('closeBrace').setValue(')');
            }
        } else {
            CloseBrace.currentTarget.checked = false;
            this.formData.controls[i].get('closeBrace').setValue('');
        }
    }

    GeneralDocTypesChanged(genVals: any, i: any) {
        const DocName: string = ((typeof (genVals) === 'string') ? genVals : genVals.target.selectedOptions[0].innerText);
        if (DocName !== AppSettings.RuleFannieMaeDocName && !this.formData.controls[i].get('isexist').value) {
            this._ruleBuilderService.docFieldsInitChange(this.formData.controls[i], this.currtDocFields, genVals, 'docField', i);
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
        this._subscriptions.forEach(el => {
            el.unsubscribe();
        });
    }
}
