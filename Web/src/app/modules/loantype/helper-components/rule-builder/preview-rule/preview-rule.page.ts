import { Component, OnInit, OnDestroy } from '@angular/core';
import { RuleBuilderService } from '../../../service/rule-builder.service';
import { AddLoanTypeService } from '../../../service/add-loantype.service';
import { ChecklistItemRowData } from '../../../models/checklist-items-table.model';
import { FormulaBuilderTypesConstant } from '@mts-app-setting';
import { FormArray } from '@angular/forms';
import { RemoveDuplicateValues } from '../../../pipes/RemoveDuplicateValue.pipe';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';

@Component({
    selector: 'mts-preview-rule-builder',
    styleUrls: ['preview-rule.page.css'],
    templateUrl: 'preview-rule.page.html',
    providers: [RemoveDuplicateValues]
})
export class PreviewRuleComponent implements OnInit, OnDestroy {
    showInput = false;
    testResult = '';
    testResultExp = '';
    ErrorMSG = '';
    allOptions: any[] = [];
    EvalFields: { DocName: string, Fieldname: string, originalField: string }[] = [];
    rowData: ChecklistItemRowData = new ChecklistItemRowData();
    ruleTypes: { Name: string, Rule_Type: number, Evaluate: boolean, Description: string, Icon: string, Active: boolean, RuleJsonName: string, IconColor: string, FunctionName: string, SaveFieldName: string, SaveDocTypeName: string, DisabledFieldName: string }[] = FormulaBuilderTypesConstant.FormulaTypes.slice();
    promise: Subscription;
    showManualOptions = false;

    constructor(
        private _commonRuleBuilderService: CommonRuleBuilderService,
        private _ruleBuilderService: RuleBuilderService,
        private _removeDuplicatePipe: RemoveDuplicateValues) { }

    private _subscriptions: Subscription[] = [];

    ngOnInit(): void {
        this.rowData = this._commonRuleBuilderService.getEditChecklistItem();
        this.setEvalDocs();

        this._subscriptions.push(this._ruleBuilderService.EvalResult.subscribe((res: { TestResult: boolean, TestResultExp: string, ErrorMsg: string }) => {
            this.testResult = res.TestResult ? 'True' : 'False';
            this.testResultExp = res.TestResultExp;
            this.ErrorMSG = res.ErrorMsg;
        }));
    }

    setEvalDocs() {
        const _ruleType = this.ruleTypes.filter(x => x.Name === this.rowData.RuleJsonObject.mainOperator)[0];
        const _rule = this._ruleBuilderService.getRuleFormGroup().get(_ruleType.RuleJsonName) as FormArray;
        if (_ruleType.Rule_Type !== 1 && isTruthy(_ruleType.FunctionName)) {
            const _evalFields = this._ruleBuilderService[_ruleType.FunctionName](_rule, _ruleType.SaveDocTypeName, _ruleType.SaveFieldName, _ruleType.DisabledFieldName);
            this.EvalFields = this._removeDuplicatePipe.transform(_evalFields.slice(), 'ComparisionFields');
        } else if (_ruleType.Rule_Type === 1) {
            this.showManualOptions = true;
            this.allOptions = this.GetManualOptions(_rule);
        }
        this.showInput = _ruleType.Evaluate;

    }

    GetManualOptions(_rule: FormArray) {
        const options = [];
        const CheckBoxChoicesContols = _rule.controls[0].get('CheckBoxChoices') as FormArray;
        const RadioOptionsControls = _rule.controls[0].get('RadioChoices') as FormArray;
        if (CheckBoxChoicesContols.controls.length > 0) {
            for (let i = 0; i < CheckBoxChoicesContols.length; i++) {
                options.push(CheckBoxChoicesContols.controls[i].get('checkboxoptions').value);
            }
        } else if (RadioOptionsControls.controls.length > 0) {
            for (let i = 0; i < RadioOptionsControls.length; i++) {
                options.push(RadioOptionsControls.controls[i].get('radiooptions').value);
            }
        }
        return options;
    }

    EvaluateRules(val) {
        const input = { RuleFormula: this.rowData.RuleDescription, CheckListItemValues: val };
        this._ruleBuilderService.EvaluateRules(input);
    }

    Save() {
        this.promise = this._ruleBuilderService.SaveChecklistItem();
    }

    GotoEditChecklistGroup() {
        this._commonRuleBuilderService.EnableRuleBuilder.next(false);
    }

    ngOnDestroy(): void {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }

}
