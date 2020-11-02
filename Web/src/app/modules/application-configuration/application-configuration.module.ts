import { DataTablesModule } from 'angular-datatables';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApplicationConfigurationComponent } from './pages/application-configuration/application-configuration.page';
import { appconfigrouting } from './application-configuration.routing';
import { ApplicationConfigService } from './service/application-configuration.service';
import { ApplicationConfigDataAccess } from './application-configuration.data';
import { ConfigTableComponent } from './helper-components/config-table/config-table.component';
import { CustomLoansearchComponent } from './helper-components/custom-loansearch/custom-loansearch.component';
import { ModalModule } from 'ngx-bootstrap/modal';

import { StipulationCategoryComponent } from './helper-components/stipulation-category/stipulation-category.component';
import { CategoryListComponent } from './helper-components/category-list/category-list.component';
import { AuditConfigComponent } from './helper-components/audit-config/audit-config.component';
import { MentionModule } from '../../shared/custom-plugins/mention-plus';
import { PasswordPolicyComponent } from './helper-components/password-policy/password-policy.component';
import { SmtpSettingsComponent } from './helper-components/smtp-settings/smtp-settings.component';
import { BoxSettingsComponent } from './helper-components/box-settings/box-settings.component';
import { ReportMasterComponent } from './helper-components/report-master/report-master.component';
import { ConfigSettingsComponent } from './helper-components/config-settings/config-settings.component';

@NgModule({
  declarations: [
    ApplicationConfigurationComponent,
    ConfigTableComponent,
    CustomLoansearchComponent,
    StipulationCategoryComponent,
    CategoryListComponent,
    AuditConfigComponent,
    PasswordPolicyComponent,
    SmtpSettingsComponent,
    BoxSettingsComponent,
    ReportMasterComponent,
    ConfigSettingsComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    DataTablesModule,
    ModalModule.forRoot(),
    MentionModule,
    appconfigrouting,
  ],
  providers: [
    ApplicationConfigService,
    ApplicationConfigDataAccess,

  ],
})
export class ApplicationConfigurationModule { }
