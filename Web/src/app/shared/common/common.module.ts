import { NgModule } from '@angular/core';
import { RuleBuilderComponent } from 'src/app/modules/loantype/helper-components/rule-builder/rule-builder.page';
import {
    FormulaBuilderComponent,
    GeneralFormulaBuilderComponent,
    IFFormulaBuilderComponent,
    INFormulaBuilderComponent,
    CheckAllFormulaBuilderComponent,
    CompareAllFormulaBuilderComponent,
    DataTableFormulaBuilderComponent,
    DateDiffFormulaBuilderComponent,
    GroupByFormulaBuilderComponent,
    IsEmptyFormulaBuilderComponent,
    IsExistFormulaBuilderComponent,
    IsNotEmptyFormulaBuilderComponent,
    LOSRuleFormulaBuilderComponent,
    ManualFormulaBuilderComponent
} from 'src/app/modules/loantype/helper-components/rule-builder/formula-builder';
import { PreviewRuleComponent } from 'src/app/modules/loantype/helper-components/rule-builder/preview-rule/preview-rule.page';
import { CommonModule } from '@angular/common';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { Daterangepicker } from 'ng2-daterangepicker';
import { MentionModule } from '@mts-mention-plus';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DataTablesModule } from 'angular-datatables';
import { DragulaModule } from '../custom-plugins/ng2-dragula/components/dragula.module';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        DataTablesModule,
        ModalModule.forRoot(),
        DragulaModule.forRoot(),
        TypeaheadModule.forRoot(),
        NgBusyModule,
        Daterangepicker,
        MentionModule.forRoot()
    ],
    declarations: [
        RuleBuilderComponent,
        FormulaBuilderComponent,
        GeneralFormulaBuilderComponent,
        IFFormulaBuilderComponent,
        INFormulaBuilderComponent,
        CheckAllFormulaBuilderComponent,
        CompareAllFormulaBuilderComponent,
        DataTableFormulaBuilderComponent,
        DateDiffFormulaBuilderComponent,
        GroupByFormulaBuilderComponent,
        IsEmptyFormulaBuilderComponent,
        IsExistFormulaBuilderComponent,
        IsNotEmptyFormulaBuilderComponent,
        LOSRuleFormulaBuilderComponent,
        ManualFormulaBuilderComponent,
        PreviewRuleComponent
    ],
    exports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        DataTablesModule,
        NgBusyModule,
        Daterangepicker,

        RuleBuilderComponent,
        FormulaBuilderComponent,
        GeneralFormulaBuilderComponent,
        IFFormulaBuilderComponent,
        INFormulaBuilderComponent,
        CheckAllFormulaBuilderComponent,
        CompareAllFormulaBuilderComponent,
        DataTableFormulaBuilderComponent,
        DateDiffFormulaBuilderComponent,
        GroupByFormulaBuilderComponent,
        IsEmptyFormulaBuilderComponent,
        IsExistFormulaBuilderComponent,
        IsNotEmptyFormulaBuilderComponent,
        LOSRuleFormulaBuilderComponent,
        ManualFormulaBuilderComponent,
        PreviewRuleComponent
    ]
})

export class RuleBuilderModule { }
