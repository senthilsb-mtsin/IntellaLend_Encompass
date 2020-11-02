import { SelectModule } from '@mts-select2';
import { NgModule, ErrorHandler } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MyDatePickerModule } from '@mts-date-picker/mydatepicker.module';
import { NgDateRangePickerModule } from '@mts-daterangepicker/ng-daterangepicker.module';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { CustomExceptionHandler } from 'src/app/shared/handlers/ErrorHandler';
import { ModalModule } from 'ngx-bootstrap/modal';
import { DataTablesModule } from 'angular-datatables';
import { CheckFileExtension, TrimspacePipe } from '@mts-pipe';
import { DragulaModule } from '@mts-dragula';
import { SubDragulaService } from 'src/app/shared/custom-plugins/ng2-dragula/components/dragula.service';
import { MalihuScrollbarModule } from 'ngx-malihu-scrollbar';
import { FileUploadModule } from 'ng2-file-upload';
import { MentionModule } from '@mts-mention-plus';
import { ManagerReverificationDataAccess } from './mreverification.data';
import { ManagerReverificationComponent } from './pages/mreverification/mreverification.page';
import { ManagerReverificationRouting } from './mreverification.routing';
import { ManagerReverificationService } from './services/manager-reverification.service';
import { ViewManagerReverificationComponent } from './pages/viewmreverification/viewmreverification.page';
import { AssignDocumentsComponent } from '../reverification/helpercomponents/assign-documents/assign-documents.component';
import { EditReverificationComponent } from '../reverification/pages/editreverification/editreverification.page';
import { ManagerEditReverificationComponent } from './pages/editmreverification/editmreverification.page';
import { ReverificationService } from '../reverification/services/reverification.service';
import { ReverificationDataAccess } from '../reverification/reverification.data';
import { ManagerAssignDocumentsComponent } from './helpercomponents/manageassigndocs/manageassigndocs.page';
import { ManagerOccupancyLetterComponent } from './helpercomponents/manager-occupancy-letter/OccupancyLetter';
import { ManagerCPALetterComponent } from './helpercomponents/manager-cpa-letter/CPALetter';
import { ManagerVOELetterComponent } from './helpercomponents/manager-voe-letter/VOELetter';
import { ManagerVODLetterComponent } from './helpercomponents/manager-vod-letter/VODLetter';
import {  ManagerGiftLetterComponent } from './helpercomponents/manager-gift-letter/GiftLetter';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        SelectModule,
        ManagerReverificationRouting,
        MalihuScrollbarModule,
        DataTablesModule,
        MyDatePickerModule,
        NgDateRangePickerModule,
        NgBusyModule,
        DragulaModule.forRoot(),
        ModalModule.forRoot(),
        MentionModule.forRoot(),
        FileUploadModule
    ],
    providers: [
        { provide: ErrorHandler, useClass: CustomExceptionHandler },
        ManagerReverificationService,
        ReverificationDataAccess,
        ReverificationService,
        ManagerReverificationDataAccess,
        CheckFileExtension,
        TrimspacePipe

    ],
    declarations: [

        ManagerReverificationComponent,
        ManagerEditReverificationComponent,
        ViewManagerReverificationComponent,
        ManagerAssignDocumentsComponent,
        ManagerOccupancyLetterComponent,
        ManagerCPALetterComponent,
        ManagerVOELetterComponent,
        ManagerVODLetterComponent,
        ManagerGiftLetterComponent

    ],

})
export class ManagerReverificationModule {

}
