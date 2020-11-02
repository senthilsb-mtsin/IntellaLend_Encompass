
import { RouterModule, Routes } from '@angular/router';
import { KpiGoalConfigsComponent } from './pages/kpi-goal-configs.page';
const KpiRoutes: Routes = [
    {
        path: 'login',
        redirectTo: '/'
    },
    {
        path: '',
        component: KpiGoalConfigsComponent
    },
];

export const KpiRouting = RouterModule.forChild(KpiRoutes);
