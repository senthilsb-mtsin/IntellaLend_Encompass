import { NgModule, ErrorHandler } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule } from 'ngx-bootstrap/modal';
import { DragulaModule } from '@mts-dragula';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { ServiceTypeRouting } from './service-type.routing';
import { CustomExceptionHandler } from 'src/app/shared/handlers/ErrorHandler';
import { CommonService } from 'src/app/shared/common/common.service';
import { ServiceTypeDataAccess } from './service-type.data';
import { ServiceTypeService } from './service/service-type.service';
import { ServiceTypeComponent } from './pages/service-type/service-type.page';
import { AddServiceTypeComponent } from './pages/add-service-type/add-service-type.page';
import { AddServiceTypeService } from './service/add-service-type.service';
import { AssignLoanTypesComponent } from './helper-components/assign-loan-type/assign-loan-type.component';
import { LoanTypeSearchPipe } from './pipes/loantypesearch.pipe';
import { MalihuScrollbarModule } from 'ngx-malihu-scrollbar';
import { ViewServiceTypeComponent } from './pages/view-service-type/view-service-type.page';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        ServiceTypeRouting,
        CommonModule,
        MalihuScrollbarModule,
        DataTablesModule,
        ModalModule.forRoot(),
        DragulaModule.forRoot(),
        NgBusyModule
    ],
    providers: [
        ServiceTypeService,
        AddServiceTypeService,
        ViewServiceTypeComponent,
        ServiceTypeDataAccess,
        CommonService
    ],
    declarations: [
        ServiceTypeComponent,
        AddServiceTypeComponent,
        ViewServiceTypeComponent,
        AssignLoanTypesComponent,
        LoanTypeSearchPipe
    ]
})

export class ServiceTypeModule {
}
