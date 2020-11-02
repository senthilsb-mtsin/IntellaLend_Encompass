import { DashboardComponent } from './pages/dashboard/dashboard.page';
import { RouterModule, Routes } from '@angular/router';
import { ChecklistloanComponent } from './pages/checklistloan/checklistloan.component';
import { ReportGridComponent } from './pages/report-grid/report-grid.component';

const dashboardRoutes: Routes = [
  {
    path: 'login',
    redirectTo: '/'
  },
  {
    path: '',
    component: DashboardComponent
  },
  {
    path: 'checklistloan',
    component: ChecklistloanComponent
  },
  {
    path: 'report',
    component: ReportGridComponent
  }
];

export const dashboardRouting = RouterModule.forChild(dashboardRoutes);
