import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MyDatePickerComponent } from './mydatepicker.component';
import { FocusDirective } from './directives/mydatepicker.focus.directive';

@NgModule({
  declarations: [MyDatePickerComponent, FocusDirective],
  imports: [CommonModule, FormsModule],
  exports: [MyDatePickerComponent, FocusDirective]
})
export class MyDatePickerModule { }
