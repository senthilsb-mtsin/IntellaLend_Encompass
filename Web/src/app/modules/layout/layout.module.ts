import { CommonModule } from '@angular/common';
import { layoutRouting } from './layout.routing';
import { LayoutComponent } from './pages/layout/layout.page';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { LayoutService } from './service/layout.service';
import { UserprofileComponent } from './pages/userprofile/userprofile.page';
import { MalihuScrollbarModule } from 'ngx-malihu-scrollbar';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { OrderByPipe } from '../../shared/pipes/orderby.pipe';
import { ResetPasswordComponent } from './pages/resetpassword/reset-password.page';
import { LayoutDataAccess } from './layout.data';

@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    layoutRouting,
    CommonModule,
    MalihuScrollbarModule,
    TooltipModule.forRoot(),
    ModalModule.forRoot(),
    NgBusyModule
  ],
  providers: [
    LayoutService,
    LayoutDataAccess,
    OrderByPipe
  ],
  declarations: [LayoutComponent, UserprofileComponent, ResetPasswordComponent]
})
export class LayoutModule { }
