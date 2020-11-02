import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MalihuScrollbarModule } from 'ngx-malihu-scrollbar';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { DragulaModule } from '@mts-dragula';
import { CommonService } from 'src/app/shared/common/common.service';
import { Daterangepicker } from 'ng2-daterangepicker';
import { MentionModule } from '@mts-mention-plus';
import { customerRouting } from './customer.routing';
import { CustomerService } from './services/customer.service';
import { CustomerData } from './customer.data';
import { CustomerComponent } from './pages/customer/customer.page';
import { UpsertCustomerComponent } from './pages/upsert-customer/upsert-customer.page';
import { ServiceTypeMappingComponent } from './helper-components/service-type-mapping/service-type-mapping.page';
import { CustLoanTypeMappingComponent } from './helper-components/loan-type-mapping/loan-type-mapping.page';
import { CustomerChecklistComponent } from './helper-components/customer-checklist/customer-checklist.page';
import { CheckListItemNamePipe } from '../loantype/pipes';
import { RuleBuilderModule } from 'src/app/shared/common/common.module';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';
import { CustomerStackingOrderComponent } from './helper-components/customer-stacking-order/customer-stacking-order.page';
import { CustomerConfigComponent } from './helper-components/customer-config/customer-config.page';
import { ConditionGeneralRuleService } from '../loantype/service/condition-general-rule.service';
import { LoanDataAccess } from '../loantype/loantype.data';
import { LoanTypeService } from '../loantype/service/loantype.service';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        customerRouting,
        CommonModule,
        MalihuScrollbarModule,
        DataTablesModule,
        ModalModule.forRoot(),
        DragulaModule.forRoot(),
        TypeaheadModule.forRoot(),
        NgBusyModule,
        Daterangepicker,
        MentionModule.forRoot(),
        RuleBuilderModule
    ],
    providers: [
        CommonService,
        CustomerService,
        CustomerData,
        CheckListItemNamePipe,
        CommonRuleBuilderService,
        ConditionGeneralRuleService,
        LoanDataAccess,
        LoanTypeService
    ],
    declarations: [
        CustomerComponent,
        UpsertCustomerComponent,
        ServiceTypeMappingComponent,
        CustLoanTypeMappingComponent,
        CustomerChecklistComponent,
        CustomerStackingOrderComponent,
        CustomerConfigComponent
    ]
})
export class CustomerModule { }
