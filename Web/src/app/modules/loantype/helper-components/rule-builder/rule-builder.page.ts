import { Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { AddLoanTypeService } from '../../service/add-loantype.service';
import { ChecklistItemRowData } from '../../models/checklist-items-table.model';
import { CommonService } from 'src/app/shared/common/common.service';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { NotificationService } from '@mts-notification';
import { RuleBuilderService } from '../../service/rule-builder.service';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';

@Component({
    selector: 'mts-rule-builder',
    styleUrls: ['rule-builder.page.css'],
    templateUrl: 'rule-builder.page.html',
    providers: [RuleBuilderService]
})
export class RuleBuilderComponent implements OnInit, OnDestroy {
    @ViewChild('EncompassValuesModal') EncompassValuesModal: ModalDirective;
    CategoryGroups: any[] = [];
    headerFieldID = '';
    losEncompassFields: any[] = [];

    Headervalue = '';
    EncompassHeaderValues = '';
    headerFieldValue = '';
    tagItems: any[] = [];
    EditSteps: { Step1: boolean, Step2: boolean, Step3: boolean };
    inValidateChecklistName = true;
    editData = false;

    rowData: ChecklistItemRowData = new ChecklistItemRowData();

    constructor(
        private _commonRuleBuilderService: CommonRuleBuilderService,
        private _addLoanTypeService: AddLoanTypeService,
        private _commonService: CommonService,
        private _notificationService: NotificationService) { }

    private _subscription: Subscription[] = [];

    ngOnInit(): void {
        this.rowData = this._commonRuleBuilderService.getEditChecklistItem();
        this.rowData.ChecklistGroupId = this._commonRuleBuilderService.getChecklist().CheckListID;
        this.editData = isTruthy(this.rowData.CheckListName);
        this.inValidateChecklistName = !isTruthy(this.rowData.CheckListName);

        this.CategoryGroups = this._commonService.GetChecklistCategories();
        this.EditSteps = this._commonRuleBuilderService.getRuleBuilderSteps();

        this._subscription.push(this._commonService.SystemChecklistCategories.subscribe((res: any[]) => {
            this.CategoryGroups = res;
        }));

        this._subscription.push(this._commonService.SystemLOSFields.subscribe((res: any[]) => {
            this.losEncompassFields = res;
        }));

        this._subscription.push(this._commonRuleBuilderService.SetRuleStep.subscribe(res => {
            this.EditSteps = res;
        }));

    }

    wizStep1() {
        this._commonRuleBuilderService.SetRuleStep.next({ Step1: false, Step2: true, Step3: false });
        this._commonRuleBuilderService.setEditChecklistItem(this.rowData);
        this._addLoanTypeService.setEditChecklistItem(this.rowData);
    }

    GotoEditChecklistGroup() {
        this._commonRuleBuilderService.EnableRuleBuilder.next(false);
    }

    addEncompassValues() {
        if (isTruthy(this.Headervalue)) {
            this.tagItems.push(this.Headervalue);
            this.headerFieldValue = this.tagItems.join('|');
            this.rowData.LOSValueToEvalRule = this.headerFieldValue;
            this.EncompassHeaderValues = this.tagItems.join(' or ');
            this.Headervalue = '';
        }
    }

    ShowConfirmModal() {
        if (isTruthy(this.Headervalue)) {
            this.EncompassValuesModal.show();
        } else {
            this._notificationService.showError('Enter Encompass Field Value');
        }
    }

    checklistItemChange() {
        this.inValidateChecklistName = !this._commonRuleBuilderService.validateCheckListItemName(this.rowData.CheckListName);
    }

    removeTag(val) {
        const index = this.tagItems.indexOf(val);
        if (index > -1) {
            this.tagItems.splice(index, 1);
        }

        this.headerFieldValue = this.tagItems.join('|');
        this.rowData.LOSValueToEvalRule = this.headerFieldValue;
        this.EncompassHeaderValues = this.tagItems.join(' or ');
    }

    OnchangeLosField() {
        this.rowData.LosIsMatched = this.rowData.LosMatched ? 1 : 2;
    }

    EncompassHeaderFieldsChange(event) {
        this.rowData.LOSFieldToEvalRule = Number.parseInt(this.headerFieldID, 10);
        this.Headervalue = '';
    }

    OnChangeLOSHeaderFieldValue() {
        if (isTruthy(this.headerFieldID)) {
            this._commonService.GetSysLOSFields(this.headerFieldID);
        }
    }

    ngOnDestroy(): void {
        this._subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
