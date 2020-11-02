import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ConnectionAuthGuard } from '@mts-auth-guard/authguard.connetion';
import { SessionGuard } from '@mts-auth-guard/session-guard';

const routes: Routes = [
  {
    path: '',
    canActivate: [SessionGuard],
    loadChildren: () =>
      import('./modules/login/login.module').then((m) => m.LoginModule),
  },
  {
    path: 'view',
    data: { routeURL: 'View' },
    canActivate: [ConnectionAuthGuard],
    loadChildren: () =>
      import('./modules/layout/layout.module').then((m) => m.LayoutModule)
  },
  {
    path: 'loanpopout',
    data: { routeURL: 'View\\LoanSearch\\LoanInfo\\LoanDetails' },
    canActivate: [ConnectionAuthGuard],
    loadChildren: () =>
      import('./modules/loan/helper-components/loan-popout/loan-popout.module').then((m) => m.LoanPopOutModule)
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
