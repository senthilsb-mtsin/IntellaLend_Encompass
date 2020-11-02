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
import { ReverificationRouting } from './reverification.routing';
import { ReverificationService } from './services/reverification.service';
import { ReverificationDataAccess } from './reverification.data';
import { ReverificationComponent } from './pages/reverification/reverification.page';
import { ViewReverificationComponent } from './pages/viewreverification/viewreverification.page';
import { EditReverificationComponent } from './pages/editreverification/editreverification.page';
import { AssignDocumentsComponent } from './helpercomponents/assign-documents/assign-documents.component';
import { DragulaModule } from '@mts-dragula';
import { SubDragulaService } from 'src/app/shared/custom-plugins/ng2-dragula/components/dragula.service';
import { MalihuScrollbarModule } from 'ngx-malihu-scrollbar';
import { AddReverificationComponent } from './pages/addreverification/addreverification.page';
import { FileUploadModule } from 'ng2-file-upload';
import { CPALetterComponent } from './helpercomponents/cpa-letter/CPALetter';
import { GiftLetterComponent } from './helpercomponents/gift-letter/GiftLetter';
import { OccupancyLetterComponent } from './helpercomponents/occupancy-letter/OccupancyLetter';
import { VOELetterComponent } from './helpercomponents/voe-letter/VOELetter';
import { VODLetterComponent } from './helpercomponents/vod-letter/VODLetter';
import { MentionModule } from '@mts-mention-plus';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    SelectModule,
    ReverificationRouting,
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
    ReverificationService, ReverificationDataAccess,
    CheckFileExtension,
    TrimspacePipe

  ],
  declarations: [
    ReverificationComponent,
    EditReverificationComponent,
    ViewReverificationComponent,
    AddReverificationComponent,
    AssignDocumentsComponent,
    CPALetterComponent,
    GiftLetterComponent,
    OccupancyLetterComponent,
    VODLetterComponent,
    VOELetterComponent
  ],

})
export class ReverificationModule {

}
