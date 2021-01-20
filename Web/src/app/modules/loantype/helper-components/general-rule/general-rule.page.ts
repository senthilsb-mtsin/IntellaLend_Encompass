import { OnDestroy, OnInit, Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { SingleOperatorExistforRule } from '../../pipes/checkAtleastSingleOperatorExist.pipe';
import { RuleCheckValidation } from '../../pipes/ruleValidation.pipe';
import { FormGroup, FormArray, FormBuilder } from '@angular/forms';
import { AssignDocumentsRuleRowData } from '../../models/assign-docs-rule.model';
import { ConditionGeneralRuleService } from '../../service/condition-general-rule.service';
import { FormulaBuilderTypesConstant } from '@mts-app-setting';
import { AddLoanTypeService } from '../../service/add-loantype.service';

@Component({
    selector: 'mts-general-rule',
    styleUrls: ['general-rule.page.css'],
    templateUrl: 'general-rule.page.html',
    providers: [RuleCheckValidation, SingleOperatorExistforRule]
})

export class GeneralRuleComponent implements OnInit, OnDestroy {

    get formData() { return <FormArray>this.rulesFrmGrp.get('schema'); }
    ErrorMsg = '';
    errMsgStyle = '';
    isErrMsgs = false;
    FieldErrorMsg = '';
    rulesFrmGrp: FormGroup;
    currtDocFields: any[] = [];
    genDocTypes: any[] = [];
    docFieldMasters: any[] = [];
     ruleFormationValues: any = '';
     loanType: { Type: string, LoanTypeID: number, LoanTypeName: string, Active: boolean };        rowData: AssignDocumentsRuleRowData = new AssignDocumentsRuleRowData();
    RuleOperator: any[] = FormulaBuilderTypesConstant.GeneralRuleOperators.slice();
    constructor(private _conditionGeneralRuleService: ConditionGeneralRuleService,
        private _addLoanTypeService: AddLoanTypeService,
        private _checkrule: RuleCheckValidation,
        private _checkruleop: SingleOperatorExistforRule,
        private _fb: FormBuilder,
         ) {

              this._conditionGeneralRuleService.setRuleFormGroup();

    }
private _subscriptions: Subscription[] = [];
    private operator: any[] = FormulaBuilderTypesConstant.Operators.slice();
    ngOnInit(): void {
        this.rulesFrmGrp = this._conditionGeneralRuleService.getRuleFormGroup();
        this._subscriptions.push(this._addLoanTypeService.LoanTypeDocuments.subscribe(

            (res: {
                Documents: { id: number, text: string }[],
                Fields: { DocID: number, FieldID: number, Name: string, DocName: string }[],
                DataTables: { DocID: number, TableName: string, ColumnName: string }[]
            }) => {
                this.genDocTypes = res.Documents;
                this.docFieldMasters = res.Fields;

            }));
            this._addLoanTypeService.GetSystemLoanDocs();
             this._subscriptions.push(this._conditionGeneralRuleService.InitializeForm.subscribe((res: any) => {

                if (res === true) { this.ruleFormationValues = '';
                   this.formData.controls = [];
                }
               }));
                    this._subscriptions.push(this._conditionGeneralRuleService.ConditionObjcet.subscribe((res: any) => {

                  this.rowData.ConditionObject = res;
                  if (this.rowData.ConditionObject !== null) {
                      this.formData.controls = [];
                      this._addLoanTypeService.getCurrentDocs.next(true);
                      this.GenerateEditRuleValues();
                  }
                 }));

        this._subscriptions.push(this.rulesFrmGrp.valueChanges.subscribe((form) => {
            let str = '';
            this.ruleFormationValues  = '([Rule])';
            let docTypeFieldFlag = false;

            form.schema.forEach(elements => {
                if (elements.generalDocumentTypes !== '' && elements.docField !== '') {
                    str += elements.openBrace + '[' + elements.generalDocumentTypes + '.' + elements.docField + ']' + elements.closeBrace + elements.docFieldOperator;
                } else if (elements.valueDocField !== '') {
                    str += elements.openBrace + elements.valueDocField + elements.closeBrace + elements.docFieldOperator;
                } else if (elements.generalDocumentTypes === '' || elements.docField === '' || elements.docFieldOperator === '') {
                    this._conditionGeneralRuleService.ruleSave.next(true);
                    docTypeFieldFlag = true;
                }
            });
            if (str === '') {
                this._conditionGeneralRuleService.ruleSave.next(true);
            } else {
                if (!docTypeFieldFlag) {
                     this.ruleFormationValues = this._conditionGeneralRuleService.replaceFormula( this.ruleFormationValues, str);
                    const generalRuleOperatorCheck = this._checkrule.transform(str, this.operator);
                    const filterResult =  this.ruleFormationValues.match(/\(/g).length ===  this.ruleFormationValues.match(/\)/g).length;
                    const totalOperatorCount = this._checkruleop.transform(str, this.operator);
                    const getAllRuleOperatorsCount = this._checkruleop.transform(totalOperatorCount[0]['Vals'].join(), this.operator);
                    const finalOperatorCount = totalOperatorCount[1]['operatorsCount'] - getAllRuleOperatorsCount[1]['operatorsCount'];
                    if ((filterResult) && generalRuleOperatorCheck && (finalOperatorCount === totalOperatorCount[0]['Vals'].length - 1) && (totalOperatorCount[0]['Vals']['length'] !== 1) && (totalOperatorCount[1]['operatorsCount'] !== 0)) {
                        this._conditionGeneralRuleService.ruleSave.next(false);
                        this.ErrorMsg = '';
                    } else {
                        if (!filterResult) {
                            this.ErrorMsg = 'Brackets are not closed properly, please check the rule';
                            this._conditionGeneralRuleService.ruleSave.next(true);
                        }
                        if (!generalRuleOperatorCheck) {
                            this.ErrorMsg = 'Rule must not end with an operator, please check the rule';
                            this._conditionGeneralRuleService.ruleSave.next(true);
                        }
                        if ((finalOperatorCount !== totalOperatorCount[0]['Vals'].length - 1) || (totalOperatorCount[0]['Vals']['length'] === 1) && (finalOperatorCount === 0)) {
                            this.ErrorMsg = 'Please select atleast one operator';
                            this._conditionGeneralRuleService.ruleSave.next(true);
                        }
                    }
                }
            }
            this._conditionGeneralRuleService.ruleFormula.next(this.ruleFormationValues);
            form.formula = this.ruleFormationValues;
        }));
    }
    GenerateEditRuleValues() {

        let i = 0;

        this.rowData.ConditionObject.schema.forEach(ele => {
           if (i === 0) {
            this.addRules();

           }
            if (ele.fieldsCustomValues === '') {
                this.formData.controls[i].get('openBrace').setValue(ele.openBrace);
                this.formData.controls[i].get('generalDocumentTypes').setValue(ele.generalDocumentTypes);
                this.GeneralDocTypesChanged(ele.generalDocumentTypes, i);
                this.formData.controls[i].get('docField').setValue(ele.docField);
                this.formData.controls[i].get('closeBrace').setValue(ele.closeBrace);
                this.formData.controls[i].get('docFieldOperator').setValue(ele.docFieldOperator);
            }
             if (ele.fieldsCustomValues === true) {
                 $('#valueDisplayed_' + i).show();
                 $('#valueDisplay_' + i).hide();
                 this.formData.controls[i].get('openBrace').setValue(ele.openBrace);
                 this.formData.controls[i].get('fieldsCustomValues').setValue(ele.fieldsCustomValues);
                this.formData.controls[i].get('valueDocField').setValue(ele.valueDocField);
                 this.formData.controls[i].get('closeBrace').setValue(ele.closeBrace);
                 this.formData.controls[i].get('docFieldOperator').setValue(ele.docFieldOperator);
            }
            if (i === this.rowData.ConditionObject.schema.length - 1) {

            } else {
                i++;
                this.addRules();
            }

        });
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

    addRules() {
        this.formData.push(this._fb.group({
            fieldsCustomValues: [''],
            openBrace: [''],
            generalDocumentTypes: [''],
            docField: [''],
            valueDocField: [''],
            closeBrace: [''],
            docFieldOperator: ['']
        }));
        this.currtDocFields.push([]);

    }

    FieldsChange(vals: any, i: any) {
        if (vals.currentTarget.checked === true) {
            this.isErrMsgs = true;
            this.FieldErrorMsg = 'The value entered needs to match exactly with the selected Field value';
            this.formData.controls[i].get('fieldsCustomValues').setValue(true);
            $('#valueDisplayed_' + i).show();
            $('#valueDisplay_' + i).hide();
            this.formData.controls[i].get('docField').setValue('');
            this.formData.controls[i].get('generalDocumentTypes').setValue('');
            this.formData.controls[i].get('fieldsCustomValues').setValue(true);
        } else {
            this.formData.controls[i].get('fieldsCustomValues').setValue(false);
            $('#valueDisplay_' + i).show();
            $('#valueDisplayed_' + i).hide();
            this.formData.controls[i].get('valueDocField').setValue('');
            this.formData.controls[i].get('fieldsCustomValues').setValue('');
        }
    }

    removeRules(i: number) {
        this.formData.removeAt(i);
    }

  neralCheckboxChange(OpenBrace: any, i: any) {
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
        this._addLoanTypeService.docFieldsInitChange(this.formData.controls[i], this.currtDocFields, genVals, 'docField', i);
    }

    ngOnDestroy(): void {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
