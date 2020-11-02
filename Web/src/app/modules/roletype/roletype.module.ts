import { CommonModule } from '@angular/common';
import { NgModule, ErrorHandler } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RoleTypeComponent } from './pages/roletype.page';
import { AddRoleTypeComponent } from './pages/add-roletype/add-roletype.page';
import { EditRoleTypeComponent } from './pages/edit-roletype/edit-roletype.page';
import { RoleTypeRouting } from './roletype.routing';
import { RoleTypeService } from './service/roletype.service';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { RoleTypeDataAccess } from './roletype.data';
import { DragulaModule } from '@mts-dragula';
import { ViewRoleTypeComponent } from './pages/view-roletype/view-roletype.page';
import { TrimspacePipe } from '@mts-pipe';
import { MalihuScrollbarModule } from 'ngx-malihu-scrollbar';
import { CustomExceptionHandler } from 'src/app/shared/handlers/ErrorHandler';
import { RoleTypeMenuComponent } from './helpercomponent/roletypemenu/roletypemenu.page';
@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RoleTypeRouting,
    CommonModule,
    DataTablesModule,
    ModalModule.forRoot(),
    DragulaModule.forRoot(),
    MalihuScrollbarModule.forRoot(),
    NgBusyModule
  ],
  providers: [
    { provide: ErrorHandler, useClass: CustomExceptionHandler },
    RoleTypeService,
    RoleTypeDataAccess, TrimspacePipe
  ],
  declarations: [RoleTypeComponent, AddRoleTypeComponent, EditRoleTypeComponent, ViewRoleTypeComponent, RoleTypeMenuComponent]
})
export class RoleTypeModule { }
