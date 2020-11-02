import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { FormulaBuilderTypesConstant } from '@mts-app-setting';
import { ChecklistItemRowData } from 'src/app/modules/loantype/models/checklist-items-table.model';
import { RuleBuilderService } from 'src/app/modules/loantype/service/rule-builder.service';
import { AddLoanTypeService } from 'src/app/modules/loantype/service/add-loantype.service';
import { RuleCheckValidation } from 'src/app/modules/loantype/pipes';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';

@Component({
    selector: 'mts-if-formula-builder',
    styleUrls: ['if-formula-builder.page.css'],
    templateUrl: 'if-formula-builder.page.html',
    providers: [RuleCheckValidation]
})
export class IFFormulaBuilderComponent  implements OnInit, OnDestroy {
    rulesFrmGrp: FormGroup;
    docFieldMasters: any[] = [];
    genDocTypes: any[] = [];
    RuleOperator: any[] = FormulaBuilderTypesConstant.GeneralRuleOperators.slice();

    FieldErrorMsg = 'The value entered needs to match exactly with the selected Field value';
    isErrMsgs = false;
    ErrorMsg = '';
    currtDocFields: { formDataConditionalExtraFields: any[], formDataTrueConditionalExtraFields: any[], formDataFalseConditionalExtraFields: any[] } = {
        formDataConditionalExtraFields: [], formDataTrueConditionalExtraFields: [], formDataFalseConditionalExtraFields: []
    };

    rowData: ChecklistItemRowData = new ChecklistItemRowData();
    waitingForData = true;
    constructor(
        private _ruleBuilderService: RuleBuilderService,
        private _commonRuleBuilderService: CommonRuleBuilderService,
        private _checkrule: RuleCheckValidation,
        private _fb: FormBuilder
    ) {
        this._ruleBuilderService.setRuleFormGroup('if');
    }

    private _subscriptions: Subscription[] = [];
    private operator: any[] = FormulaBuilderTypesConstant.Operators.slice();

    private _formMetaData = [{
        jObjectName: 'ConditionalExtraFields',
        formType: 'formDataConditionalExtraFields',
        formulaExpression: '[condition]',
        formFields: {
            customField: 'fieldsConditionalCustomValues',
            docField: 'ifdocField',
            docTypeField: 'IfDocumentTypes',
            valueDocField: 'ifValueDocField',
            showDocTypeField: '#valueDisplayed_',
            showValueField: '#valueDisplay_',
            openBracketField: 'conditionalExtraFieldopenBrace',
            closeBracketField: 'conditionalExtraFieldcloseBrace',
            operator: 'ifdocFieldOperator'
        }
    }, {
        jObjectName: 'TrueConditionalExtraFields',
        formType: 'formDataTrueConditionalExtraFields',
        formulaExpression: '[true]',
        formFields: {
            customField: 'fieldsTrueCustomValues',
            docField: 'trueDocField',
            docTypeField: 'TrueDocumentTypes',
            valueDocField: 'trueValueDocField',
            showDocTypeField: '#truevalueDisplayed_',
            showValueField: '#truevalueDisplay_',
            openBracketField: 'trueExtraFieldopenBrace',
            closeBracketField: 'TrueExtraFieldcloseBrace',
            operator: 'trueDocFieldOperator'
        }
    }, {
        jObjectName: 'FalseConditionalExtraFields',
        formType: 'formDataFalseConditionalExtraFields',
        formulaExpression: '[false]',
        formFields: {
            customField: 'fieldsFalseCustomValues',
            docField: 'falseDocField',
            docTypeField: 'FalseDocumentTypes',
            valueDocField: 'falseValueDocField',
            showDocTypeField: '#falsevalueDisplayed_',
            showValueField: '#falsevalueDisplay_',
            openBracketField: 'falseExtraFieldopenBrace',
            closeBracketField: 'FalseExtraFieldcloseBrace',
            operator: 'FalseDocFieldOperator'
        }
    }];
    get formData() { return <FormArray>this.rulesFrmGrp.get('conditionalRule'); }

    get formDataConditionalExtraFields() { return this.formData.controls[0].get('ConditionalExtraFields') as FormArray; }

    get formDataTrueConditionalExtraFields() { return this.formData.controls[0].get('TrueConditionalExtraFields') as FormArray; }

    get formDataFalseConditionalExtraFields() { return this.formData.controls[0].get('FalseConditionalExtraFields') as FormArray; }

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

                if (isTruthy(this.rowData.RuleJsonObject) && isTruthy(this.rowData.RuleJsonObject.conditionalRule)) {
                    this._formMetaData.forEach(element => {
                        if (this.rowData.RuleJsonObject.conditionalRule.length > 0) {
                            this.GenerateFormValues(element.jObjectName);
                        }
                    });
                }
            }));
        this._ruleBuilderService.getSysLoanTypeDocuments();

        this._subscriptions.push(this.rulesFrmGrp.valueChanges.subscribe((form) => {
            let str = '';
            let ruleFormationValues = 'if([condition],[true],[false])';

            const outputVal = {
                formDataConditionalExtraFields: { docTypeIfFieldFlag: false, checkConditionResult: false, expression: '' },
                formDataTrueConditionalExtraFields: { docTypeIfFieldFlag: false, checkConditionResult: false, expression: '' },
                formDataFalseConditionalExtraFields: { docTypeIfFieldFlag: false, checkConditionResult: false, expression: '' }
            };

            this._formMetaData.forEach(_form => {
                str = '';
                if (form.conditionalRule[0][_form.jObjectName].length > 0) {
                    form.conditionalRule[0][_form.jObjectName].forEach(element => {
                        if (element[_form.formFields.docTypeField] !== '' && element[_form.formFields.docField] !== '') {
                            str += element[_form.formFields.openBracketField] + '[' + element[_form.formFields.docTypeField] + '.' + element[_form.formFields.docField] + ']' + element[_form.formFields.closeBracketField] + element[_form.formFields.operator];
                        } else if (element[_form.formFields.valueDocField] !== '') {
                            str += element[_form.formFields.openBracketField] + element[_form.formFields.valueDocField] + element[_form.formFields.closeBracketField] + element[_form.formFields.operator];
                        } else if (element[_form.formFields.docTypeField] === '' || element[_form.formFields.docField] === '' || element[_form.formFields.docTypeField] === 0) {
                            this._commonRuleBuilderService.ruleBuilderNext.next(true);
                            outputVal[_form.formType].docTypeIfFieldFlag = true;
                        }
                    });
                }
                outputVal[_form.formType].expression = str;
            });

            if (!isTruthy(outputVal.formDataConditionalExtraFields.expression) && !isTruthy(outputVal.formDataTrueConditionalExtraFields.expression) && !isTruthy(outputVal.formDataFalseConditionalExtraFields.expression)) {
                this._commonRuleBuilderService.ruleBuilderNext.next(true);
            } else {
                ruleFormationValues = this._ruleBuilderService.replaceIfFormula(ruleFormationValues, outputVal.formDataConditionalExtraFields.expression, outputVal.formDataTrueConditionalExtraFields.expression, outputVal.formDataFalseConditionalExtraFields.expression);
                const isValidRule = ruleFormationValues.match(/\(/g).length === ruleFormationValues.match(/\)/g).length;
                this._formMetaData.forEach(_form => {
                    outputVal[_form.formType].checkConditionResult = this._checkrule.transform(outputVal[_form.formType].expression, this.operator);
                    outputVal[_form.formType].checkConditionResult = this.CheckIfRuleOperatorFunction(_form.formType, form.conditionalRule[0][_form.jObjectName]);
                });

                if (!isValidRule) {
                    this.ErrorMsg = 'Brackets are not closed properly, please check the Rule';
                    this._commonRuleBuilderService.ruleBuilderNext.next(true);
                } else if (!outputVal.formDataConditionalExtraFields.checkConditionResult || !outputVal.formDataTrueConditionalExtraFields.checkConditionResult || !outputVal.formDataFalseConditionalExtraFields.checkConditionResult) {
                    this.ErrorMsg = 'Rule must not end with an Operator, please check the Rule';
                    this._commonRuleBuilderService.ruleBuilderNext.next(true);
                }

                if (isValidRule &&
                    !outputVal.formDataConditionalExtraFields.docTypeIfFieldFlag &&
                    !outputVal.formDataTrueConditionalExtraFields.docTypeIfFieldFlag &&
                    !outputVal.formDataFalseConditionalExtraFields.docTypeIfFieldFlag &&
                    outputVal.formDataConditionalExtraFields.checkConditionResult &&
                    outputVal.formDataTrueConditionalExtraFields.checkConditionResult &&
                    outputVal.formDataFalseConditionalExtraFields.checkConditionResult) {
                    this.ErrorMsg = '';
                    this.rowData.RuleDescription = ruleFormationValues;
                    this._commonRuleBuilderService.ruleBuilderNext.next(false);
                }
            }
            this._commonRuleBuilderService.ruleExpression.next(ruleFormationValues);
        }));
    }

    CheckIfRuleOperatorFunction(_formType: string, formData: any) {
        const form = this._formMetaData.filter(f => f.formType === _formType)[0];
        const operatorArray = [];
        formData.forEach(element => {
            if (element[form.formFields.operator] !== '') {
                operatorArray.push(element[form.formFields.operator]);
            }
        });
        return (operatorArray.length > 0 && operatorArray.length === formData.length - 1 && formData.length > 1);
    }

    GenerateFormValues(_jObjectType: string) {
        let i = 0;
        const form = this._formMetaData.filter(f => f.jObjectName === _jObjectType)[0];
        this.rowData.RuleJsonObject.conditionalRule[0][form.jObjectName].forEach(element => {
            this.addRule(form.formType);
            if (element[form.formFields.customField] === '') {
                this[form.formType].controls[i].get(form.formFields.openBracketField).setValue(element[form.formFields.openBracketField]);
                this[form.formType].controls[i].get(form.formFields.docTypeField).setValue(element[form.formFields.docTypeField]);
                this.DocTypesChanged(element[form.formFields.docTypeField], i, form.formType);
                this[form.formType].controls[i].get(form.formFields.docField).setValue(element[form.formFields.docField][0].id);
                this[form.formType].controls[i].get(form.formFields.closeBracketField).setValue(element[form.formFields.closeBracketField]);
                this[form.formType].controls[i].get(form.formFields.operator).setValue(element[form.formFields.operator]);
            }
            if (element[form.formFields.customField] === true) {
                this[form.formType].controls[i].get(form.formFields.openBracketField).setValue(element[form.formFields.openBracketField]);
                this[form.formType].controls[i].get(form.formFields.customField).setValue(element[form.formFields.customField]);
                this[form.formType].controls[i].get(form.formFields.closeBracketField).setValue(element[form.formFields.closeBracketField]);
                this[form.formType].controls[i].get(form.formFields.valueDocField).setValue(element[form.formFields.valueDocField]);
                this[form.formType].controls[i].get(form.formFields.operator).setValue(element[form.formFields.operator]);
            }
            i++;
        });
    }

    DocTypesChanged(genVals: any, i: any, _formType: string) {
        const form = this._formMetaData.filter(f => f.formType === _formType)[0];
        this._ruleBuilderService.docFieldsInitChange(this[_formType].controls[i], this.currtDocFields[_formType], genVals, form.formFields.docField, i);
    }

    removeRules(i: any, _formType: string) {
        this[_formType].removeAt(i);
    }

    openBracketCheckboxChanged(vals: any, i: any, _formType: string) {
        const form = this._formMetaData.filter(f => f.formType === _formType)[0];
        if (vals.currentTarget.checked === true && vals.currentTarget.value === '(') {
            this[_formType].controls[i].get(form.formFields.openBracketField).setValue('(');
        } else {
            vals.currentTarget.checked = false;
            this[_formType].controls[i].get(form.formFields.openBracketField).setValue('');
        }
    }

    closeBracketCheckboxChanged(vals: any, i: any, _formType: string) {
        const form = this._formMetaData.filter(f => f.formType === _formType)[0];
        if (vals.currentTarget.checked === true && vals.currentTarget.value === ')') {
            this[_formType].controls[i].get(form.formFields.closeBracketField).setValue(')');
        } else {
            vals.currentTarget.checked = false;
            this[_formType].controls[i].get(form.formFields.closeBracketField).setValue('');
        }
    }

    FieldDocTypeChange(vals: any, index: any, _formType: string) {
        const form = this._formMetaData.filter(f => f.formType === _formType)[0];
        if (vals.currentTarget.checked === true) {
            this.isErrMsgs = true;
            this[form.formType].controls[index].get(form.formFields.customField).setValue(true);
            this[form.formType].controls[index].get(form.formFields.docField).setValue('');
            this[form.formType].controls[index].get(form.formFields.docTypeField).setValue('');
        } else {
            this[form.formType].controls[index].get(form.formFields.customField).setValue(false);
            this[form.formType].controls[index].get(form.formFields.valueDocField).setValue('');
        }
    }

    addRule(_formType: string) {
        const _form = this._formMetaData.filter(f => f.formType === _formType)[0];
        const formFields = {};
        formFields[_form.formFields.openBracketField] = [''];
        formFields[_form.formFields.closeBracketField] = [''];
        formFields[_form.formFields.docField] = [''];
        formFields[_form.formFields.valueDocField] = [''];
        formFields[_form.formFields.docTypeField] = [''];
        formFields[_form.formFields.operator] = [''];
        formFields[_form.formFields.customField] = [''];
        this[_formType].push(this._fb.group(formFields));
        this.currtDocFields[_formType].push([]);
    }

    ngOnDestroy(): void {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
