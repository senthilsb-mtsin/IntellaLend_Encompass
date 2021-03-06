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
    selector: 'mts-isnotempty-formula-builder',
    styleUrls: ['isnotempty-formula-builder.page.css'],
    templateUrl: 'isnotempty-formula-builder.page.html'
})
export class IsNotEmptyFormulaBuilderComponent implements OnInit, OnDestroy {
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
        this._ruleBuilderService.setRuleFormGroup('isnotempty');
    }

    private _subscriptions: Subscription[] = [];

    get formData() { return this.rulesFrmGrp.get('isNotEmptyRule') as FormArray; }

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

                if (isTruthy(this.rowData.RuleJsonObject) && isTruthy(this.rowData.RuleJsonObject.isNotEmptyRule) && this.rowData.RuleJsonObject.isNotEmptyRule.length > 0) {
                    this.GenerateFormValues();
                } else if (this.formData.length === 0) {
                    this.addRules();
                }
            }));
        this._ruleBuilderService.getSysLoanTypeDocuments();
        this._subscriptions.push(this.rulesFrmGrp.valueChanges.subscribe((form) => {
            const myForm = this.rulesFrmGrp.getRawValue();
            let str = '';
            let ruleFormationValues = 'isnotempty([Rule])';

            myForm.isNotEmptyRule.forEach(elt => {
                if (elt.NotEmptyDocTypes !== '' && typeof (elt.NotEmptyDocFieldTypes) === 'string' && elt.NotEmptyDocFieldTypes !== '' && elt.NotEmptyDocTypes !== AppSettings.RuleFannieMaeDocName) {
                    str += '[' + elt.NotEmptyDocTypes + '.' + elt.NotEmptyDocFieldTypes + ']';
                } else if (elt.NotEmptyDocTypes !== '' && elt.NotEmptyDocFieldTypes !== '' && Array.isArray(elt.NotEmptyDocFieldTypes) && elt.NotEmptyDocFieldTypes.length > 0 && elt.NotEmptyDocTypes !== AppSettings.RuleFannieMaeDocName) {
                    str += '[' + elt.NotEmptyDocTypes + '.' + elt.NotEmptyDocFieldTypes[0].text + ']';
                } else if (elt.NotEmptyDocTypes !== '' && typeof (elt.NotEmptyLosdocField) === 'string' && elt.NotEmptyLosdocField !== '' && elt.NotEmptyDocTypes === AppSettings.RuleFannieMaeDocName) {
                    str += '[' + AppSettings.FannieMaeDocDisplayName + '.' + elt.NotEmptyLosdocField + ']';
                } else if (elt.NotEmptyDocTypes !== '' && elt.NotEmptyLosdocField !== '' && Array.isArray(elt.NotEmptyLosdocField) && elt.NotEmptyLosdocField.length > 0 && elt.NotEmptyDocTypes === AppSettings.RuleFannieMaeDocName) {
                    str += '[' + AppSettings.FannieMaeDocDisplayName + '.' + elt.NotEmptyLosdocField[0].text + ']';
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
        this.rowData.RuleJsonObject.isNotEmptyRule.forEach(element => {
            if (element.NotEmptyDocTypes !== '' && element.NotEmptyDocFieldTypes !== '') {
                this.addRules();
                this.formData.controls[i].get('NotEmptyDocTypes').setValue(element.NotEmptyDocTypes);
                this._ruleBuilderService.docFieldsInitChange(this.formData.controls[i], this.currtDocFields, element.NotEmptyDocTypes, 'NotEmptyDocFieldTypes', i);

                if (isTruthy(element.NotEmptyDocFieldTypes)) {
                    this.formData.controls[i].get('NotEmptyDocFieldTypes').setValue(element.NotEmptyDocFieldTypes[0].id);
                }
                if (isTruthy(element.NotEmptyLosdocField)) {
                    this.formData.controls[i].get('NotEmptyLosdocField').setValue(element.NotEmptyLosdocField);
                }
            }
            i++;
        });
    }

    IsNotEditEmptyDocOnChange(genVals: any, id: any, field: string) {
        const DocName: string = ((typeof (genVals) === 'string') ? genVals : genVals.target.selectedOptions[0].innerText);
        if (DocName !== AppSettings.RuleFannieMaeDocName) {
            this._ruleBuilderService.docFieldsInitChange(this.formData.controls[id], this.currtDocFields, genVals, field, id);
        }
    }

    addRules() {
        this.formData.push(this._fb.group({
            NotEmptyDocTypes: [''],
            NotEmptyDocFieldTypes: [''],
            NotEmptyLosdocField: ['']
        }));
    }

    OnChangeFieldValue(index: number) {
        const SearchValue = this.formData.controls[index].get('NotEmptyLosdocField').value;
        const LosDocumentName = this.formData.controls[index].get('NotEmptyDocTypes').value;
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
