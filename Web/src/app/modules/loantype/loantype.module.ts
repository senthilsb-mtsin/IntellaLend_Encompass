import { CommonModule, JsonPipe } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MalihuScrollbarModule } from 'ngx-malihu-scrollbar';
import { LoanTypeComponent } from './pages/loantype/loantype.page';
import { AddLoanTypeComponent } from './pages/add-loantype/add-loantype.page';
import { loantypeRouting } from './loantype.routing';
import { LoanTypeService } from './service/loantype.service';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { LoanDataAccess } from './loantype.data';
import { DragulaModule } from '@mts-dragula';
import { AssignDocumentTypesComponent } from './helper-components/assign-document-type/assign-document-type.component';
import { DocMasterSearchPipe } from './pipes/documentmastersearch.pipe';
import { CloneChecklistComponent } from './helper-components/clone-checklist/clone-checklist.page';
import { CommonService } from 'src/app/shared/common/common.service';
import { EditChecklistGroupComponent } from './helper-components/edit-checklist-group/edit-checklist-group.page';
import { CloneStackingOrderComponent } from './helper-components/clone-stacking-order/clone-stacking-orderpage';
import { EditStackingOrderComponent } from './helper-components/edit-stacking-order/edit-stacking-order.page';
import { CheckListItemNamePipe } from './pipes/validateCheckListItemName.pipe';
import { AddLoanTypeService } from './service/add-loantype.service';
import { SelectModule } from '@mts-select2';
import { RemoveDuplicateValues } from './pipes/RemoveDuplicateValue.pipe';
import { Daterangepicker } from 'ng2-daterangepicker';
import { MentionModule } from '@mts-mention-plus';
import { RuleBuilderModule } from 'src/app/shared/common/common.module';
import { GeneralRuleComponent } from './helper-components/general-rule/general-rule.page';
import { RuleBuilderService } from './service/rule-builder.service';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';
import { ConditionGeneralRuleService } from './service/condition-general-rule.service';

@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    loantypeRouting,
    CommonModule,
    MalihuScrollbarModule,
    DataTablesModule,
    ModalModule.forRoot(),
    DragulaModule.forRoot(),
    TypeaheadModule.forRoot(),
    SelectModule,
    NgBusyModule,
    Daterangepicker,
    MentionModule.forRoot(),
    RuleBuilderModule
  ],
  providers: [
    LoanTypeService,
    AddLoanTypeService,
    LoanDataAccess,
    CommonService,
    RemoveDuplicateValues,
    JsonPipe,
    CheckListItemNamePipe,
    RuleBuilderService,
    CommonRuleBuilderService,
    ConditionGeneralRuleService
  ],
  declarations: [
    DocMasterSearchPipe,
    LoanTypeComponent,
    AddLoanTypeComponent,
    AssignDocumentTypesComponent,
    GeneralRuleComponent,
    CloneChecklistComponent,
    EditChecklistGroupComponent,
    CloneStackingOrderComponent,
    EditStackingOrderComponent,
  ],
})
export class LoanTypeModule { }
