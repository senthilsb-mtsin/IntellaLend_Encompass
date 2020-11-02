import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { ChecklistItemRowData } from 'src/app/modules/loantype/models/checklist-items-table.model';
import { AddLoanTypeService } from 'src/app/modules/loantype/service/add-loantype.service';
import { Subscription } from 'rxjs';
import { RuleBuilderService } from 'src/app/modules/loantype/service/rule-builder.service';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';

@Component({
    selector: 'mts-compareall-formula-builder',
    styleUrls: ['compareall-formula-builder.page.css'],
    templateUrl: 'compareall-formula-builder.page.html'
})
export class CompareAllFormulaBuilderComponent implements OnInit, OnDestroy {
    rulesFrmGrp: FormGroup;
    docFieldMasters: any[] = [];
    genDocTypes: any[] = [];
    currtDocFields: any[] = [[], []];
    rowData: ChecklistItemRowData = new ChecklistItemRowData();
    waitingForData = true;
    ErrorMsg = '';
    FieldErrorMsg = 'The value entered needs to match exactly with the selected Field value';
    isErrMsgs = false;
    inLookUpDocumentFieldMasterTypes: any[] = [];

    constructor(
        private _commonRuleBuilderService: CommonRuleBuilderService,
        private _ruleBuilderService: RuleBuilderService,
        private _fb: FormBuilder
    ) {
        this._ruleBuilderService.setRuleFormGroup('compareall');
    }

    private _subscriptions: Subscription[] = [];

    get formData() { return this.rulesFrmGrp.get('compareAllRule') as FormArray; }

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

                if (isTruthy(this.rowData.RuleJsonObject) && isTruthy(this.rowData.RuleJsonObject.compareAllRule) && this.rowData.RuleJsonObject.compareAllRule.length > 0) {
                    this.GenerateFormValues();
                } else if (this.formData.length === 0) {
                    this.addRules();
                }
            }));

        this._ruleBuilderService.getSysLoanTypeDocuments();

        this._subscriptions.push(this.rulesFrmGrp.valueChanges.subscribe((form) => {
            const myForm = this.rulesFrmGrp.getRawValue();
            let str = '';
            this.ErrorMsg = '';
            let ruleFormationValues = 'compare([Rule])';
            myForm.compareAllRule.forEach(elements => {
                const compareallDisValField = isTruthy(elements.compareallDisValField) ? elements.compareallDisValField : '';
                if (elements.ComapreAllDocumentTypes !== '' && elements.CompareAllDocField !== '') {
                    if (typeof (elements.CompareAllDocField) === 'string') {
                        this._commonRuleBuilderService.ruleBuilderNext.next(false);
                        str += '[' + elements.ComapreAllDocumentTypes + '.' + elements.CompareAllDocField + ']' + ',' + compareallDisValField;
                    } else if (Array.isArray(elements.InDocField) && elements.InDocField.length > 0) {
                        this._commonRuleBuilderService.ruleBuilderNext.next(false);
                        str += '[' + elements.ComapreAllDocumentTypes + '.' + elements.CompareAllDocField[0].text + ']' + ',' + compareallDisValField;
                    } else if (typeof (elements.CompareAllDocField) === 'object') {
                        this._commonRuleBuilderService.ruleBuilderNext.next(false);
                        str += '[' + elements.ComapreAllDocumentTypes + '.' + elements.CompareAllDocField.text + ']' + ',' + compareallDisValField;
                    }
                } else if (elements.compareallvalueDocField !== '') {
                    str += elements.compareallvalueDocField + ',' + compareallDisValField;
                } else {
                    this._commonRuleBuilderService.ruleBuilderNext.next(true);
                }
            });
            const splitResult = str.split(',');
            if (splitResult[0] !== '' && splitResult[1] !== '') {
                this._commonRuleBuilderService.ruleBuilderNext.next(false);
                ruleFormationValues = this._ruleBuilderService.replaceFormula(ruleFormationValues, str);
                this.rowData.RuleDescription = ruleFormationValues;
            } else {
                this.ErrorMsg = 'Please check the Rule';
                ruleFormationValues = this._ruleBuilderService.replaceFormula(ruleFormationValues, str);
                this._commonRuleBuilderService.ruleBuilderNext.next(true);
            }

            this._commonRuleBuilderService.ruleExpression.next(ruleFormationValues);
        }));
    }

    CompareAllEditFieldsChange(vals: any, index: any) {
        if (vals.currentTarget.checked === true) {
            this.isErrMsgs = true;
            this.formData.controls[index].get('compareallfieldsCustomValues').setValue(true);
            $('#compareallvalueDisplayed_' + index).show();
            $('#compareallvalueDisplay_' + index).hide();
            this.formData.controls[index].get('CompareAllDocField').setValue('');
        } else {
            this.formData.controls[index].get('compareallfieldsCustomValues').setValue(false);
            $('#compareallvalueDisplay_' + index).show();
            $('#compareallvalueDisplayed_' + index).hide();
            this.formData.controls[index].get('compareallvalueDocField').setValue('');
        }
    }

    CompareAllEditLookUpFieldsChange(vals: any, index: any) {
        if (vals.currentTarget.checked === true) {
            this.isErrMsgs = true;
            this.formData.controls[index].get('comparealllookupfieldsCustomValues').setValue(true);
            $('#comparealllookupvalueDisplayed_' + index).show();
            $('#compareAlllookupvalueDisplay_' + index).hide();
        } else {
            this.formData.controls[index].get('comparealllookupfieldsCustomValues').setValue(false);
            $('#compareAlllookupvalueDisplay_' + index).show();
            $('#comparealllookupvalueDisplayed_' + index).hide();
            this.formData.controls[index].get('CompareAllValuesField').setValue('');
        }
    }

    CompareAllEditDocTypesChanged(genVals: any, i: any, field: string) {
        this._ruleBuilderService.docFieldsInitChange(this.formData.controls[0], this.currtDocFields, genVals, field, i);
    }

    CompareAllEditClearField() {
        this.formData.controls[0].get('compareallDisValField').setValue('');
    }

    addEditCompareAllValues() {
        this.currtDocFields[1] = [];
        if (this.formData.controls[0].get('CompareAllLookUpDocumentTypes').value !== '' && this.formData.controls[0].get('CompareAllValuesField').value !== '') {
            if (this.formData.controls[0].get('compareallDisValField').value !== '') {
                this.formData.controls[0].get('compareallDisValField').setValue(this.formData.controls[0].get('compareallDisValField').value + ',' + '[' + this.formData.controls[0].get('CompareAllLookUpDocumentTypes').value + '.' + this.formData.controls[0].get('CompareAllValuesField').value + ']');
            } else {
                this.formData.controls[0].get('compareallDisValField').setValue('[' + this.formData.controls[0].get('CompareAllLookUpDocumentTypes').value + '.' + this.formData.controls[0].get('CompareAllValuesField').value + ']');
            }
            this.formData.controls[0].get('CompareAllLookUpDocumentTypes').setValue('');
            this.formData.controls[0].get('CompareAllValuesField').setValue('');
        } else if (this.formData.controls[0].get('comparealllookupvalueDocField').value !== '') {
            if (this.formData.controls[0].get('compareallDisValField').value !== '') {
                this.formData.controls[0].get('compareallDisValField').setValue(this.formData.controls[0].get('compareallDisValField').value + ',' + this.formData.controls[0].get('comparealllookupvalueDocField').value);
                this.formData.controls[0].get('comparealllookupvalueDocField').setValue('');
            } else {
                this.formData.controls[0].get('compareallDisValField').setValue(this.formData.controls[0].get('comparealllookupvalueDocField').value);
                this.formData.controls[0].get('comparealllookupvalueDocField').setValue('');
            }
        }
    }

    InLookUpFieldsChange(vals: any, index: any) {
        if (vals.currentTarget.checked === true) {
            this.isErrMsgs = true;
            this.formData.controls[index].get('inlookupfieldsCustomValues').setValue(true);
            $('#inlookupvalueDisplayed_' + index).show();
            $('#inlookupvalueDisplay_' + index).hide();
        } else {
            this.formData.controls[index].get('inlookupfieldsCustomValues').setValue(false);
            $('#inlookupvalueDisplay_' + index).show();
            $('#inlookupvalueDisplayed_' + index).hide();
            this.formData.controls[index].get('InValuesField').setValue('');
        }
    }

    GenerateFormValues() {
        let i = 0;
        this.rowData.RuleJsonObject.compareAllRule.forEach(element => {
            this.addRules();
            if (element.compareallfieldsCustomValues === '' || element.compareallfieldsCustomValues === false) {
                this.formData.controls[i].get('ComapreAllDocumentTypes').setValue(element.ComapreAllDocumentTypes);
                this._ruleBuilderService.docFieldsInitChange(this.formData.controls[i], this.currtDocFields, element.ComapreAllDocumentTypes, 'CompareAllDocField', i);
                this.formData.controls[i].get('CompareAllDocField').setValue(element.CompareAllDocField[0].id);
            }
            if (element.compareallfieldsCustomValues) {
                $('#compareallvalueDisplayed_' + i).show();
                $('#compareallvalueDisplay_' + i).hide();
                this.formData.controls[i].get('compareallfieldsCustomValues').setValue(element.compareallfieldsCustomValues);
                this.formData.controls[i].get('compareallvalueDocField').setValue(element.compareallvalueDocField);
            }
            this.formData.controls[i].get('compareallDisValField').setValue(element.compareallDisValField);
            i++;
        });
    }

    addRules() {
        this.formData.push(this._fb.group({
            compareallfieldsCustomValues: [''],
            ComapreAllDocumentTypes: [''],
            CompareAllDocField: [''],
            compareallvalueDocField: [''],
            comparealllookupfieldsCustomValues: [''],
            CompareAllLookUpDocumentTypes: [''],
            CompareAllValuesField: [''],
            comparealllookupvalueDocField: [''],
            compareallDisValField: [{ value: '', disabled: true }]
        }));
    }

    ngOnDestroy(): void {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
