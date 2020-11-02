import { Component, ViewChild, ElementRef, ViewChildren, QueryList, OnInit, OnDestroy } from '@angular/core';
import { AddLoanTypeService } from '../../../service/add-loantype.service';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FormulaBuilderTypesConstant } from '@mts-app-setting';
import { RuleJObject } from '../../../models/checklist-items-table.model';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';

@Component({
    selector: 'mts-formula-builder',
    styleUrls: ['formula-builder.page.css'],
    templateUrl: 'formula-builder.page.html'
})
export class FormulaBuilderComponent implements OnInit, OnDestroy {
    @ViewChild('confirmMsgModal') confirmRuleModal: ModalDirective;
    @ViewChildren('ruleTab') ruleTabs: QueryList<ElementRef<HTMLElement>>;
    ruleTypes: { Name: string, ShowVersioning: boolean, Description: string, Icon: string, Active: boolean, RuleJsonName: string, IconColor: string }[] = FormulaBuilderTypesConstant.FormulaTypes;
    ruleType = '';
    errMsgStyle = '';
    ruleFormationValues = '([Rule])';
    ruleErrMsgs = '';
    isRuleErrMsgs = false;
    DocVersions = 3;
    DocTypeVersionID = 3;
    ruleNextButton = true;
    rowData = this._commonRuleBuilderService.getEditChecklistItem();
    editRule = false;
    ShowVersioning = true;
    currentRule = '';

    constructor(
        private _commonRuleBuilderService: CommonRuleBuilderService) { }

    private _subscription: Subscription[] = [];

    ngOnInit(): void {
        if (isTruthy(this.rowData.RuleJsonObject.mainOperator)) {
            this.setFormulaType(this.rowData.RuleJsonObject.mainOperator, 'onstart');
            this.editRule = true;
        } else {
            this.editRule = false;
            this.rowData.DocVersion = '3';
            this.setFormulaType('general', 'onstart');
        }
        this._subscription.push(this._commonRuleBuilderService.ruleBuilderNext.subscribe((res: boolean) => {
            this.ruleNextButton = res;
            if (res) {
                this.errMsgStyle = 'red';
            } else {
                this.errMsgStyle = 'green';
            }
        }));

        this._subscription.push(this._commonRuleBuilderService.ruleExpression.subscribe((res: string) => {
            this.ruleFormationValues = res;
        }));
    }

    setRuleTabActive() {
        if (isTruthy(this.ruleTabs)) {
            this.ruleTabs.forEach(element => {
                element.nativeElement.classList.remove('active');
            });
            this.ruleTabs.filter(r => r.nativeElement.id === this.rowData.RuleJsonObject.mainOperator)[0].nativeElement.classList.add('active');
        }
    }

    setFormulaType(type: string, event: string) {
        this.currentRule = type;
        if (event === 'onstart') {
            this.changeFormula(this.currentRule);
        } else if (event === 'menuclick' && this.editRule) {
            if (this.currentRule !== this.rowData.RuleJsonObject.mainOperator) {
                this.confirmRuleModal.show();
            }
        } else {
            this.rowData.RuleJsonObject = new RuleJObject();
            this.changeFormula(this.currentRule);
        }
    }

    changeFormula(type: string) {
        this.ruleType = type;
        this.rowData.RuleJsonObject.mainOperator = type;
        this.ruleTypes.filter(r => r.Name === type)[0].Active = true;
        this.ruleTypes.filter(r => r.Name !== type).forEach(element => { element.Active = false; });
        this.ShowVersioning = this.ruleTypes.filter(r => r.Name === type)[0].ShowVersioning;
        this._commonRuleBuilderService.ruleExpression.next('([Rule])');
        this.setRuleTabActive();
    }

    DocversionChange(event) {
        this.DocVersions = parseInt(event.target.value, 10);
    }

    GotoEditChecklistGroup() {
        this._commonRuleBuilderService.EnableRuleBuilder.next(false);
    }
    wizStep2() {
        this._commonRuleBuilderService.SetRuleStep.next({ Step1: false, Step2: false, Step3: true });
    }

    ngOnDestroy(): void {
        this._subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
