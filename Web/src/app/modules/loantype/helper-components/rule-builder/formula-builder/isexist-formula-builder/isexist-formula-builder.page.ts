import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { ChecklistItemRowData } from 'src/app/modules/loantype/models/checklist-items-table.model';
import { AddLoanTypeService } from 'src/app/modules/loantype/service/add-loantype.service';
import { RuleBuilderService } from 'src/app/modules/loantype/service/rule-builder.service';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { NotificationService } from '@mts-notification';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';

@Component({
    selector: 'mts-isexist-formula-builder',
    styleUrls: ['isexist-formula-builder.page.css'],
    templateUrl: 'isexist-formula-builder.page.html'
})
export class IsExistFormulaBuilderComponent implements OnInit, OnDestroy {
    rulesFrmGrp: FormGroup;
    docFieldMasters: any[] = [];
    genDocTypes: any[] = [];
    currtDocFields: any[] = [[]];
    rowData: ChecklistItemRowData = new ChecklistItemRowData();
    waitingForData = true;
    MatchType = 'MatchAll';
    IsExistDocValues: any[] = [];
    constructor(
        private _commonRuleBuilderService: CommonRuleBuilderService,
        private _ruleBuilderService: RuleBuilderService,
        private _notificationService: NotificationService,
        private _fb: FormBuilder
    ) {
        this._ruleBuilderService.setRuleFormGroup('isexist');
    }

    private _subscriptions: Subscription[] = [];
    get formData() { return this.rulesFrmGrp.get('docIsExist') as FormArray; }

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

                if (isTruthy(this.rowData.RuleJsonObject) && isTruthy(this.rowData.RuleJsonObject.docIsExist) && this.rowData.RuleJsonObject.docIsExist.length > 0) {
                    this.GenerateFormValues();
                } else if (this.formData.length === 0) {
                    this.addRules();
                    this.formSubscription();
                }
            }));

        this._ruleBuilderService.getSysLoanTypeDocuments();
    }

    formSubscription() {
        this._subscriptions.push(this.formData.controls[0].get('IsExistDocTypes').valueChanges.subscribe((form) => {
            const myForm = this.rulesFrmGrp.getRawValue();
            let str = '';
            let ruleFormationValues = myForm.docIsExist[0].MatchAllType === true ? 'isexistAny([Rule])' : 'isexist([Rule])';
            let tempDocs = '';
            myForm.docIsExist.forEach(elt => {
                str += '[' + elt.IsExistDocTypes + ']';

                if (elt.IsExistDocTypes !== '' && this.IsExistDocValues.slice().filter(x => x === str).length === 0) {
                    this.IsExistDocValues.push(str);
                } else if (elt.IsExistDocTypes !== '' && this.IsExistDocValues.slice().filter(x => x === str).length > 0) {
                    this._notificationService.showError('Already Exist in the Collection');
                }
                elt.IsExistDocTypes = '';
                elt.IsExistDocuments = this.IsExistDocValues.join(',');
                tempDocs = elt.IsExistDocuments;
            });
            ruleFormationValues = this._ruleBuilderService.replaceFormula(ruleFormationValues, tempDocs);
            if (isTruthy(tempDocs)) {
                this._commonRuleBuilderService.ruleBuilderNext.next(false);
                this.rowData.RuleDescription = ruleFormationValues;
            } else {
                this._commonRuleBuilderService.ruleBuilderNext.next(true);
            }

            this.formData.controls[0].get('IsExistDocuments').setValue(this.IsExistDocValues.join(','));
            this._commonRuleBuilderService.ruleExpression.next(ruleFormationValues);
        }));

        this._subscriptions.push(this.formData.controls[0].get('MatchAllType').valueChanges.subscribe((form) => {
            const myForm = this.rulesFrmGrp.getRawValue();
            let ruleFormationValues = form === true ? 'isexistAny([Rule])' : 'isexist([Rule])';
            let tempDocs = '';
            myForm.docIsExist.forEach(elt => {
                elt.IsExistDocTypes = '';
                elt.IsExistDocuments = this.IsExistDocValues.join(',');
                tempDocs = elt.IsExistDocuments;
            });
            ruleFormationValues = this._ruleBuilderService.replaceFormula(ruleFormationValues, tempDocs);
            if (isTruthy(tempDocs)) {
                this._commonRuleBuilderService.ruleBuilderNext.next(false);
                this.rowData.RuleDescription = ruleFormationValues;
            } else {
                this._commonRuleBuilderService.ruleBuilderNext.next(true);
            }

            this._commonRuleBuilderService.ruleExpression.next(ruleFormationValues);
        }));
    }

    GenerateFormValues() {
        const ruleData = this.rowData.RuleJsonObject.docIsExist[0];
        this.addRules();
        this.formSubscription();
        this.IsExistDocValues = ruleData.IsExistDocuments.split(',').filter(item => item);
        this.formData.controls[0].get('IsExistDocuments').setValue(ruleData.IsExistDocuments);
        this.formData.controls[0].get('MatchAllType').setValue(ruleData.MatchAllType);
    }

    IsNotEditEmptyDocOnChange(genVals: any, id: any, field: string) {
        this._ruleBuilderService.docFieldsInitChange(this.formData.controls[id], this.currtDocFields, genVals, field, id);
    }

    addRules() {
        this.formData.push(this._fb.group({
            IsExistDocTypes: [''],
            IsExistDocuments: [''],
            MatchAllType: ['']
        }));
    }

    removesTag(tag) {
        const index = this.IsExistDocValues.indexOf(tag);
        this.IsExistDocValues.splice(index, 1);
        this.formData.controls[0].get('IsExistDocuments').setValue(this.IsExistDocValues.join(','));
        this.formData.controls[0].get('MatchAllType').setValue(this.formData.controls[0].get('MatchAllType').value);
    }

    ngOnDestroy(): void {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
