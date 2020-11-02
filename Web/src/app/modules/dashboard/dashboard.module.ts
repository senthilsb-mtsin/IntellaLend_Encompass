import { CommonModule, DatePipe } from '@angular/common';
import { dashboardRouting } from './dashboard.routing';
import { DashboardComponent } from './pages/dashboard/dashboard.page';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { DashboardService } from './service/dashboard.service';
import { MalihuScrollbarModule } from 'ngx-malihu-scrollbar';
import { LoanQcIndexComponent } from './pages/loan-qc-index/loan-qc-index.page';
import { LoanDefectsComponent } from './pages/loan-defects/loan-defects.page';
import { HouseLoanManagementComponent } from './pages/house-loan-management/house-loan-management.page';
import { FailedRulesLoanComponent } from './pages/failed-rules-loan/failed-rules-loan.page';
import { MissingDocumentsLoanComponent } from './pages/missing-documents-loan/missing-documents-loan.page';
import { InvestorStipulationsComponent } from './pages/investor-stipulations/investor-stipulations.page';
import { KpiUserGroupsComponent } from './pages/kpi-user-groups/kpi-user-groups.page';
import { DocumentRetentionMonitorComponent } from './pages/document-retention-monitor/document-retention-monitor.page';
import { IdcWorkloadReportComponent } from './pages/idc-workload-report/idc-workload-report.page';
import { DashboardDataAccess } from './dashboard.data';
import { DataTablesModule } from 'angular-datatables';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { NgDateRangePickerModule } from '@mts-daterangepicker/ng-daterangepicker.module';
import { ChecklistloanComponent } from './pages/checklistloan/checklistloan.component';
import { ReportGridComponent } from './pages/report-grid/report-grid.component';
import { HighchartsChartModule } from 'highcharts-angular';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ReportStateModel } from './models/report-state.model';
import { ReportServiceModel } from './models/reporting.model';
import { DashboardCommonServiceModel } from './models/dashboard-common.model';
import { SharedPipeModule } from '@mts-pipe';
import { DataEntryWorkLoadReportComponent } from './pages/data-entry-workload/data-entry-workload.page';
import { SelectModule } from '@mts-select2';
import { MonthYearPickerModule } from '@mts-month-year-picker/month-year-picker.module';

@NgModule({
  imports: [FormsModule,
    ReactiveFormsModule,
    dashboardRouting,
    CommonModule,
    MalihuScrollbarModule,
    DataTablesModule,
    MonthYearPickerModule,
    SelectModule,
    NgBusyModule,
    NgDateRangePickerModule,
    HighchartsChartModule,
    SharedPipeModule,
    ModalModule.forRoot()],
  providers: [
    DashboardService,
    ReportStateModel,
    ReportServiceModel,
    DashboardDataAccess,
    DatePipe
  ],
  declarations: [DashboardComponent,
    LoanQcIndexComponent,
    LoanDefectsComponent,
    ChecklistloanComponent,
    HouseLoanManagementComponent,
    FailedRulesLoanComponent,
    MissingDocumentsLoanComponent,
    InvestorStipulationsComponent,
    DocumentRetentionMonitorComponent,
    IdcWorkloadReportComponent,
    KpiUserGroupsComponent,
    ReportGridComponent,
    DataEntryWorkLoadReportComponent
  ]
})
export class DashboardModule { }
