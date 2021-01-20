import { LayoutComponent } from './pages/layout/layout.page';
import { RouterModule, Routes } from '@angular/router';
import { ConnectionAuthGuard } from '@mts-auth-guard/authguard.connetion';
import { ResetPasswordComponent } from './pages/resetpassword/reset-password.page';
import { KpiGoalConfigsComponent } from '../kpi-goal-configs/pages/kpi-goal-configs.page';
import { ManagerDocumentTypeComponent } from '../managedoctype/pages/managedoctype/managedoctype.page';

const layoutRoutes: Routes = [
  {
    path: 'login',
    redirectTo: '/',
  },
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: 'dashboard',
        data: { routeURL: 'View\\Dashboard' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../dashboard/dashboard.module').then((m) => m.DashboardModule)
      },
      {
        path: 'resetpassword',
        component: ResetPasswordComponent,
        data: { routeURL: 'View\\ResetPassword' },
        canActivate: [ConnectionAuthGuard],
      },
      {
        path: 'loantype',
        data: { routeURL: 'View\\LoanType' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../loantype/loantype.module').then((m) => m.LoanTypeModule),
      },
      {
        path: 'loansearch',
        data: { routeURL: 'View\\LoanSearch' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../loansearch/loansearch.module').then((m) => m.LoanSearchModule)
      },
      {
        path: 'user',
        data: { routeURL: 'View\\User' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../user/user.module').then((m) => m.UserModule)
      },
      {
        path: 'emailtracker',
        data: { routeURL: 'View\\EmailTracker' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../emailtracker/emailtracker.module').then((m) => m.EmailTrackerModule)
      },
      {
        path: 'kpigoal-config',
        data: { routeURL: 'View\\kpigoal-config' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../kpi-goal-configs/kpi-goal-configs.module').then(
            (m) => m.KpiGoalConfigsModule
          ),
      },
      {
        path: 'loanpurge',
        data: { routeURL: 'View\\LoanPurge' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../loanpurge/loanpurge.module').then((m) => m.LoanpurgeModule)
      },
      {
        path: 'documenttype',
        data: { routeURL: 'View\\DocumentType' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../documenttype/documenttype.module').then((m) => m.DocumentTypeModule)
      },

      {
        path: 'reverification',
        data: { routeURL: 'View\\Reverification' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../reverification/reverification.module').then(
            (m) => m.ReverificationModule
          ),
      },
      {
        path: 'encompassparkingspot',
        data: { routeURL: 'View\\ParkingSpot' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../encompass-parkingspot/encompass-parkingspot.module').then((m) => m.EncompassParkingspotModule)
      },
      {
        path: 'tenantconfig',
        data: { routeURL: 'View\\TenantConfig' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../application-configuration/application-configuration.module').then((m) => m.ApplicationConfigurationModule)
      },
      {
        path: 'workqueue',
        data: { routeURL: 'View\\WorkQueue' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../workqueue/workqueue.module').then((m) => m.WorkQueueModule)
      },
      {
        path: 'mreverification',
        data: { routeURL: 'View\\ManagerReverification' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>

          import('../mreverification/mreverification.module').then(
            (m) => m.ManagerReverificationModule
          ),
      },
      {
        path: 'mdocumenttype',
        data: { routeURL: 'View\\ManagerDocumentType' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>

          import('../managedoctype/managedoctype.module').then(
            (m) => m.ManagerDocumentTypeModule
          ),
      },
      {
        path: 'customer',
        data: { routeURL: 'View\\Customer' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () => import('../customer/customer.module').then((m) => m.CustomerModule)
      },
      {
        path: 'roletype',
        data: { routeURL: 'View\\Roletype' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../roletype/roletype.module').then(
            (m) => m.RoleTypeModule
          ),
      },
      {
        path: 'loanimport',
        data: { routeURL: 'View\\LoanImport' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../loan-import/loan-import.module').then(
            (m) => m.LoanImportModule
          ),
      },
      {
        path: 'reviewtype',
        data: { routeURL: 'View\\ReviewType' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../service-type/service-type.module').then((m) => m.ServiceTypeModule),
      },
      {
        path: 'loandetails',
        data: { routeURL: 'View\\LoanSearch\\LoanInfo\\LoanDetails' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../loan/loan.module').then((m) => m.LoanModule),
      },

      {
        path: 'export',
        data: { routeURL: 'View\\ExportMonitor' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../export/export.module').then((m) => m.ExportModule),
      },
      {
        path: 'encompassexception',
        data: { routeURL: 'View\\EncompassException' },
        canActivate: [ConnectionAuthGuard],
        loadChildren: () =>
          import('../encompassexception/encompass-exception.module').then(
            (m) => m.EncompassDownloadModule
          ),
      },
    ],
  }
];

export const layoutRouting = RouterModule.forChild(layoutRoutes);
