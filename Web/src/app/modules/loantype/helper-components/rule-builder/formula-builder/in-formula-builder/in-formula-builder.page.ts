import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormArray, FormBuilder } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AddLoanTypeService } from 'src/app/modules/loantype/service/add-loantype.service';
import { RuleBuilderService } from 'src/app/modules/loantype/service/rule-builder.service';
import { ChecklistItemRowData } from 'src/app/modules/loantype/models/checklist-items-table.model';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';
import { AppSettings } from '@mts-app-setting';

@Component({
    selector: 'mts-in-formula-builder',
    styleUrls: ['in-formula-builder.page.css'],
    templateUrl: 'in-formula-builder.page.html'
})
export class INFormulaBuilderComponent implements OnInit, OnDestroy {

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

    LosDocumentFields: any[] = [];
    FannieMaeDocName: string = AppSettings.RuleFannieMaeDocName;

    constructor(
        private _commonRuleBuilderService: CommonRuleBuilderService,
        private _ruleBuilderService: RuleBuilderService,
        private _fb: FormBuilder
    ) {
        this._ruleBuilderService.setRuleFormGroup('in');
    }

    private _subscriptions: Subscription[] = [];

    get formData() { return this.rulesFrmGrp.get('inRule') as FormArray; }

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

                if (isTruthy(this.rowData.RuleJsonObject) && isTruthy(this.rowData.RuleJsonObject.inRule) && this.rowData.RuleJsonObject.inRule.length > 0) {
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
            let ruleFormationValues = 'in([Rule])';
            myForm.inRule.forEach(elements => {
                const inDisvalField = isTruthy(elements.InDisValField) ? elements.InDisValField : '';
                if (elements.InDocumentTypes !== '' && elements.InDocField !== '' && Array.isArray(elements.InDocField) && elements.InDocField.length > 0 && elements.InDocumentTypes !== AppSettings.RuleFannieMaeDocName) {
                    str += '[' + elements.InDocumentTypes + '.' + elements.InDocField[0].text + ']' + ',' + inDisvalField;
                } else if (elements.InDocumentTypes !== '' && elements.InLosdocField !== '' && Array.isArray(elements.InLosdocField) && elements.InLosdocField.length > 0 && elements.InDocumentTypes === AppSettings.RuleFannieMaeDocName) {
                    str += '[' + AppSettings.FannieMaeDocDisplayName + '.' + elements.InLosdocField[0].text + ']' + ',' + inDisvalField;
                } else if (elements.InDocumentTypes !== '' && elements.InDocField !== '' && typeof (elements.InDocField) === 'string' && elements.InDocumentTypes !== AppSettings.RuleFannieMaeDocName) {
                    str += '[' + elements.InDocumentTypes + '.' + elements.InDocField + ']' + ',' + inDisvalField;
                } else if (elements.InDocumentTypes !== '' && elements.InLosdocField !== '' && typeof (elements.InLosdocField) === 'string' && elements.InDocumentTypes === AppSettings.RuleFannieMaeDocName) {
                    str += '[' + AppSettings.FannieMaeDocDisplayName + '.' + elements.InLosdocField + ']' + ',' + inDisvalField;
                } else if (elements.InDocumentTypes !== '' && elements.InDocField !== '' && typeof (elements.InDocField) === 'object' && elements.InDocumentTypes !== AppSettings.RuleFannieMaeDocName) {
                    str += '[' + elements.InDocumentTypes + '.' + elements.InDocField.text + ']' + ',' + inDisvalField;
                } else if (elements.InDocumentTypes !== '' && elements.InLosdocField !== '' && typeof (elements.InLosdocField) === 'object' && elements.InDocumentTypes === AppSettings.RuleFannieMaeDocName) {
                    str += '[' + AppSettings.FannieMaeDocDisplayName + '.' + elements.InLosdocField.text + ']' + ',' + inDisvalField;
                } else if (elements.invalueDocField !== '') {
                    str += elements.invalueDocField + ',' + inDisvalField;
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
        this._subscriptions.push(this._ruleBuilderService.LosDocumentFields.subscribe((elements: any[]) => {

            this.LosDocumentFields = [];
            elements.forEach((element) => {
                this.LosDocumentFields.push(element);
            });
        }));
    }

    InFieldsChange(vals: any, index: any) {
        if (vals.currentTarget.checked === true) {
            this.isErrMsgs = true;
            this.formData.controls[index].get('infieldsCustomValues').setValue(true);
            $('#invalueDisplayed_' + index).show();
            $('#invalueDisplay_' + index).hide();
            this.formData.controls[index].get('InDocField').setValue('');
            this.formData.controls[index].get('InLosdocField').setValue('');
        } else {
            this.formData.controls[index].get('infieldsCustomValues').setValue(false);
            $('#invalueDisplay_' + index).show();
            $('#invalueDisplayed_' + index).hide();
            this.formData.controls[index].get('invalueDocField').setValue('');
        }
    }

    InDocTypesChanged(genVals: any, i: any, field: string) {
        const DocName: string = ((typeof (genVals) === 'string') ? genVals : genVals.target.selectedOptions[0].innerText);
        if (DocName !== AppSettings.RuleFannieMaeDocName) {
            this._ruleBuilderService.docFieldsInitChange(this.formData.controls[0], this.currtDocFields, genVals, field, i);
        }
    }

    InClearField() {
        this.formData.controls[0].get('InDisValField').setValue('');
    }

    addInValues() {
        this.inLookUpDocumentFieldMasterTypes = [];
        if (this.formData.controls[0].get('InLookUpDocumentTypes').value !== '' && (this.formData.controls[0].get('InValuesField').value !== '' || this.formData.controls[0].get('InLookUpLosdocField').value !== '')) {
            if (this.formData.controls[0].get('InDisValField').value !== '') {
                if (this.formData.controls[0].get('InLookUpDocumentTypes').value !== AppSettings.RuleFannieMaeDocName) {
                    this.formData.controls[0].get('InDisValField').setValue(this.formData.controls[0].get('InDisValField').value + ',' + '[' + this.formData.controls[0].get('InLookUpDocumentTypes').value + '.' + this.formData.controls[0].get('InValuesField').value + ']');
                } else {
                    this.formData.controls[0].get('InDisValField').setValue(this.formData.controls[0].get('InDisValField').value + ',' + '[' + this.formData.controls[0].get('InLookUpDocumentTypes').value + '.' + this.formData.controls[0].get('InLookUpLosdocField').value + ']');
                }

            } else {

                if (this.formData.controls[0].get('InLookUpDocumentTypes').value !== AppSettings.RuleFannieMaeDocName) {
                    this.formData.controls[0].get('InDisValField').setValue('[' + this.formData.controls[0].get('InLookUpDocumentTypes').value + '.' + this.formData.controls[0].get('InValuesField').value + ']');
                } else {
                    this.formData.controls[0].get('InDisValField').setValue('[' + this.formData.controls[0].get('InLookUpDocumentTypes').value + '.' + this.formData.controls[0].get('InLookUpLosdocField').value + ']');
                }
            }
            this.formData.controls[0].get('InLookUpDocumentTypes').setValue('');
            this.formData.controls[0].get('InValuesField').setValue('');
            this.formData.controls[0].get('InLookUpLosdocField').setValue('');
        } else if (this.formData.controls[0].get('inlookupvalueDocField').value !== '') {

            if (this.formData.controls[0].get('InDisValField').value !== '') {

                this.formData.controls[0].get('InDisValField').setValue(this.formData.controls[0].get('InDisValField').value + ',' + this.formData.controls[0].get('inlookupvalueDocField').value);
                this.formData.controls[0].get('inlookupvalueDocField').setValue('');
            } else {
                this.formData.controls[0].get('InDisValField').setValue(this.formData.controls[0].get('inlookupvalueDocField').value);
                this.formData.controls[0].get('inlookupvalueDocField').setValue('');
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
        this.rowData.RuleJsonObject.inRule.forEach(element => {
            this.addRules();
            if (element.infieldsCustomValues === '') {
                this.formData.controls[i].get('InDocumentTypes').setValue(element.InDocumentTypes);
                this._ruleBuilderService.docFieldsInitChange(this.formData.controls[i], this.currtDocFields, element.InDocumentTypes, 'InDocField', i);

                if (isTruthy(element.InDocField)) {
                    this.formData.controls[i].get('InDocField').setValue(element.InDocField[0].id);
                }
                if (isTruthy(element.InLosdocField)) {
                    this.formData.controls[i].get('InLosdocField').setValue(element.InLosdocField);
                }
            }
            if (element.infieldsCustomValues === true) {
                $('#invalueDisplayed_' + i).show();
                $('#invalueDisplay_' + i).hide();
                this.formData.controls[i].get('infieldsCustomValues').setValue(element.infieldsCustomValues);
                this.formData.controls[i].get('invalueDocField').setValue(element.invalueDocField);
            }
            this.formData.controls[i].get('InDisValField').setValue(element.InDisValField);
            i++;
        });
    }

    addRules() {
        this.formData.push(this._fb.group({
            infieldsCustomValues: [''],
            invalueDocField: [''],
            InDocumentTypes: [''],
            InDocField: [''],
            InLosdocField: [''],
            inlookupfieldsCustomValues: [''],
            InLookUpDocumentTypes: [''],
            inlookupvalueDocField: [''],
            InLookUpLosdocField: [''],
            InValuesField: [''],
            InDisValField: [{ value: '', disabled: true }]
        }));
    }

    OnChangeFieldValue(index: number, LosDocField: string, DocType: string) {
        const SearchValue = this.formData.controls[index].get(LosDocField).value;
        const LosDocumentName = this.formData.controls[index].get(DocType).value;
        let LosDocumentId;
        this.genDocTypes.forEach((a) => {
            if (a.text === LosDocumentName) {
                LosDocumentId = a.id;
            }
        });
        this._ruleBuilderService.GetLosDocFields(LosDocumentId, SearchValue);
    }

    ngOnDestroy(): void {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }

}
