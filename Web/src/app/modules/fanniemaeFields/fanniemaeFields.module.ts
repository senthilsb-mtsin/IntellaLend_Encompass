import { NgModule } from '@angular/core';
import { DataTablesModule } from 'angular-datatables';
import { ModalDirective, ModalModule } from 'ngx-bootstrap/modal';
import { FannieMaeFieldsComponent } from './pages/fanniemaeFields.page';

@NgModule({
    imports: [
        DataTablesModule,
        ModalModule.forRoot()],
    providers: [

    ],

    declarations: [FannieMaeFieldsComponent],
    exports: [FannieMaeFieldsComponent]
})

export class FannieMaeFieldsModule {

}
