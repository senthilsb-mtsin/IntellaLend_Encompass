import { OrderByPipe } from './orderby.pipe';
import { NgModule } from '@angular/core';
import { DocMasterSearchPipe } from './documentmastersearch.pipe';

@NgModule({
  declarations: [OrderByPipe, DocMasterSearchPipe],
  exports: [OrderByPipe, DocMasterSearchPipe],
  imports: []
})
export class SharedPipeModule { }
