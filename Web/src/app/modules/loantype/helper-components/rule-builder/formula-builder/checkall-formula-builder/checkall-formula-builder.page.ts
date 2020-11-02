import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { Subscription } from 'rxjs';
import { ChecklistItemRowData } from 'src/app/modules/loantype/models/checklist-items-table.model';
import { AddLoanTypeService } from 'src/app/modules/loantype/service/add-loantype.service';
import { RuleBuilderService } from 'src/app/modules/loantype/service/rule-builder.service';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';

@Component({
    selector: 'mts-checkall-formula-builder',
    styleUrls: ['checkall-formula-builder.page.css'],
    templateUrl: 'checkall-formula-builder.page.html'
})
export class CheckAllFormulaBuilderComponent implements OnInit, OnDestroy {
    rulesFrmGrp: FormGroup;
    docFieldMasters: any[] = [];
    genDocTypes: any[] = [];
    currtDocFields: any[] = [[]];
    rowData: ChecklistItemRowData = new ChecklistItemRowData();
    waitingForData = true;

    constructor(
        private _commonRuleBuilderService: CommonRuleBuilderService,
        private _ruleBuilderService: RuleBuilderService,
        private _fb: FormBuilder
    ) {
        this._ruleBuilderService.setRuleFormGroup('checkall');
    }

    private _subscriptions: Subscription[] = [];

    get formData() { return this.rulesFrmGrp.get('docCheckAll') as FormArray; }

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

                if (isTruthy(this.rowData.RuleJsonObject) && isTruthy(this.rowData.RuleJsonObject.docCheckAll) && this.rowData.RuleJsonObject.docCheckAll.length > 0) {
                    this.GenerateFormValues();
                } else if (this.formData.length === 0) {
                    this.addRules();
                }
            }));
        this._ruleBuilderService.getSysLoanTypeDocuments();
        this._subscriptions.push(this.rulesFrmGrp.valueChanges.subscribe((form) => {
            const myForm = this.rulesFrmGrp.getRawValue();
            let str = '';
            let ruleFormationValues = 'checkall([Rule])';

            myForm.docCheckAll.forEach(elt => {
                if (isTruthy(elt.CheckAllDocTypes) && typeof (elt.CheckAllDocFieldTypes) === 'string' && isTruthy(elt.CheckAllDocFieldTypes)) {
                    str += '[' + elt.CheckAllDocFieldTypes + ']';
                } else if (isTruthy(elt.CheckAllDocTypes) && isTruthy(elt.CheckAllDocFieldTypes) && Array.isArray(elt.CheckAllDocFieldTypes) && elt.CheckAllDocFieldTypes.length > 0) {
                    str += '[' + elt.CheckAllDocFieldTypes[0].text + ']';
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
    }
    GenerateFormValues() {
        let i = 0;
        this.rowData.RuleJsonObject.docCheckAll.forEach(element => {
            if (element.CheckAllDocTypes !== '' && element.CheckAllDocFieldTypes !== '') {
                this.addRules();
                this.formData.controls[i].get('CheckAllDocTypes').setValue(element.CheckAllDocTypes);
                this._ruleBuilderService.docFieldsInitChange(this.formData.controls[i], this.currtDocFields, element.CheckAllDocTypes, 'CheckAllDocFieldTypes', i);
                this.formData.controls[i].get('CheckAllDocFieldTypes').setValue(element.CheckAllDocFieldTypes[0].id);
            }
            i++;
        });
    }

    IsEditCheckAllDocOnChange(genVals: any, id: any, field: string) {
        this._ruleBuilderService.docFieldsInitChange(this.formData.controls[id], this.currtDocFields, genVals, field, id);
    }

    addRules() {
        this.formData.push(this._fb.group({
            CheckAllDocTypes: [''],
            CheckAllDocFieldTypes: ['']
        }));
    }

    ngOnDestroy(): void {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
