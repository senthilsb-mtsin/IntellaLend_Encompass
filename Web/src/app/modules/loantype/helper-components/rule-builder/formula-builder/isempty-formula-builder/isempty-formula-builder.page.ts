import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { ChecklistItemRowData } from 'src/app/modules/loantype/models/checklist-items-table.model';
import { AddLoanTypeService } from 'src/app/modules/loantype/service/add-loantype.service';
import { RuleBuilderService } from 'src/app/modules/loantype/service/rule-builder.service';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';
import { AppSettings } from '@mts-app-setting';

@Component({
    selector: 'mts-isempty-formula-builder',
    styleUrls: ['isempty-formula-builder.page.css'],
    templateUrl: 'isempty-formula-builder.page.html'
})
export class IsEmptyFormulaBuilderComponent implements OnInit, OnDestroy {
    rulesFrmGrp: FormGroup;
    docFieldMasters: any[] = [];
    genDocTypes: any[] = [];
    currtDocFields: any[] = [[]];
    rowData: ChecklistItemRowData = new ChecklistItemRowData();
    waitingForData = true;

    LosDocumentFields: any[] = [];
    FannieMaeDocName: string = AppSettings.RuleFannieMaeDocName;

    constructor(
        private _commonRuleBuilderService: CommonRuleBuilderService,
        private _ruleBuilderService: RuleBuilderService,
        private _fb: FormBuilder
    ) {
        this._ruleBuilderService.setRuleFormGroup('empty');
    }

    private _subscriptions: Subscription[] = [];

    get formData() { return this.rulesFrmGrp.get('isEmptyRule') as FormArray; }

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

                if (isTruthy(this.rowData.RuleJsonObject) && isTruthy(this.rowData.RuleJsonObject.isEmptyRule) && this.rowData.RuleJsonObject.isEmptyRule.length > 0) {
                    this.GenerateFormValues();
                } else if (this.formData.length === 0) {
                    this.addRules();
                }
            }));
        this._ruleBuilderService.getSysLoanTypeDocuments();
        this._subscriptions.push(this.rulesFrmGrp.valueChanges.subscribe((form) => {
            const myForm = this.rulesFrmGrp.getRawValue();
            let str = '';
            let ruleFormationValues = 'empty([Rule])';
            myForm.isEmptyRule.forEach(elt => {
                if (elt.EmptyDocTypes !== '' && typeof (elt.EmptyDocFieldTypes) === 'string' && elt.EmptyDocFieldTypes !== '' && elt.EmptyDocTypes !== AppSettings.RuleFannieMaeDocName) {
                    str += '[' + elt.EmptyDocTypes + '.' + elt.EmptyDocFieldTypes + ']';
                } else if (elt.EmptyDocTypes !== '' && elt.EmptyDocFieldTypes !== '' && Array.isArray(elt.EmptyDocFieldTypes) && elt.EmptyDocFieldTypes.length > 0 && elt.EmptyDocTypes !== AppSettings.RuleFannieMaeDocName) {
                    str += '[' + elt.EmptyDocTypes + '.' + elt.EmptyDocFieldTypes[0].text + ']';
                } else if (elt.EmptyDocTypes !== '' && typeof (elt.EmptyLosdocField) === 'string' && elt.EmptyLosdocField !== '' && elt.EmptyDocTypes === AppSettings.RuleFannieMaeDocName) {
                    str += '[' + AppSettings.FannieMaeDocDisplayName + '.' + elt.EmptyLosdocField + ']';
                } else if (elt.EmptyDocTypes !== '' && elt.EmptyLosdocField !== '' && Array.isArray(elt.EmptyLosdocField) && elt.EmptyLosdocField.length > 0 && elt.EmptyDocTypes === AppSettings.RuleFannieMaeDocName) {
                    str += '[' + AppSettings.FannieMaeDocDisplayName + '.' + elt.EmptyLosdocField[0].text + ']';
                } else {
                    this._commonRuleBuilderService.ruleBuilderNext.next(true);
                }
            });
            if (str !== '') {
                ruleFormationValues = this._ruleBuilderService.replaceFormula(ruleFormationValues, str);
                this.rowData.RuleDescription = ruleFormationValues;
                this._commonRuleBuilderService.ruleBuilderNext.next(false);
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

    GenerateFormValues() {
        let i = 0;
        this.rowData.RuleJsonObject.isEmptyRule.forEach(element => {
            if (element.EmptyDocTypes !== '' && element.EmptyDocFieldTypes !== '') {
                this.addRules();
                this.formData.controls[i].get('EmptyDocTypes').setValue(element.EmptyDocTypes);
                this._ruleBuilderService.docFieldsInitChange(this.formData.controls[i], this.currtDocFields, element.EmptyDocTypes, 'EmptyDocFieldTypes', i);

                if (isTruthy(element.EmptyDocFieldTypes)) {
                    this.formData.controls[i].get('EmptyDocFieldTypes').setValue(element.EmptyDocFieldTypes[0].id);
                }
                if (isTruthy(element.EmptyLosdocField)) {
                    this.formData.controls[i].get('EmptyLosdocField').setValue(element.EmptyLosdocField);
                }
            }
            i++;
        });
    }

    IsEditEmptyDocOnChange(genVals: any, id: any, field: string) {
        const DocName: string = ((typeof (genVals) === 'string') ? genVals : genVals.target.selectedOptions[0].innerText);
        if (DocName !== AppSettings.RuleFannieMaeDocName) {
            this._ruleBuilderService.docFieldsInitChange(this.formData.controls[id], this.currtDocFields, genVals, field, id);
        }
    }

    addRules() {
        this.formData.push(this._fb.group({
            EmptyDocTypes: [''],
            EmptyDocFieldTypes: [''],
            EmptyLosdocField: ['']
        }));
    }

    OnChangeFieldValue(index: number) {
        const SearchValue = this.formData.controls[index].get('EmptyLosdocField').value;
        const LosDocumentName = this.formData.controls[index].get('EmptyDocTypes').value;
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
