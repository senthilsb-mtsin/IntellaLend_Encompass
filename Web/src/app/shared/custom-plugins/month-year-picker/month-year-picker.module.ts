import { MonthYearPickerComponent } from './component/MonthYearPicker/MonthYearPicker.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [MonthYearPickerComponent],
  imports: [
    CommonModule
  ],
  exports: [MonthYearPickerComponent]
})
export class MonthYearPickerModule { }
