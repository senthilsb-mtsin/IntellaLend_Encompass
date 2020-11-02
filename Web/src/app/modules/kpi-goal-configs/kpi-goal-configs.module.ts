import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { DataTablesModule } from 'angular-datatables';
import { MyDatePickerModule } from '@mts-date-picker/mydatepicker.module';
import { NgDateRangePickerModule } from '@mts-daterangepicker/ng-daterangepicker.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { KpiRouting } from './kpi-goal-configs.routing';
import { KpiGoalConfigsComponent } from './pages/kpi-goal-configs.page';
import { CommonService } from 'src/app/shared/common';
import { KpiGoalconfigService } from './services/kpi-goal-configs.service';
import { KpiGoalConfigsDataAccess } from './kpi-goal-configs.data';
import { KpiUserGroupConfigModel } from './models/kpiuserconfig.model';
import { KpiUserConfigComponent } from './pages/kpi-user-cofig/kpi-user-config.page';
import { NgBusyModule } from '@mts-busy/ng-busy.module';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        KpiRouting,
        DataTablesModule,
        MyDatePickerModule,
        NgBusyModule,

        NgDateRangePickerModule,
        ModalModule.forRoot()],

    providers: [
        DatePipe,
        CommonService,
        KpiGoalConfigsComponent,
        KpiUserConfigComponent,
        KpiGoalconfigService,
        KpiGoalConfigsDataAccess

    ],

    declarations: [KpiGoalConfigsComponent, KpiUserConfigComponent],
    exports: [KpiGoalConfigsComponent]
})
export class KpiGoalConfigsModule {

}
