//import { NgDateRangePickerOptions, NgDateRangePickerComponent } from 'ng-daterangepicker';
import { NgDateRangePickerOptions, NgDateRangePickerComponent } from '../ng-daterangepicker-master/ng-daterangepicker.component';
import { Component, ViewChild, ViewChildren, QueryList, Input, Output, EventEmitter } from '@angular/core';

@Component({
    moduleId: 'DatepickerComponent',
    selector: 'mts-datepicker',
    templateUrl: 'datepicker.component.html'
})

export class DatepickerComponent {
    options: NgDateRangePickerOptions;
    @ViewChild(NgDateRangePickerComponent) receivedDate: NgDateRangePickerComponent
    @ViewChild(NgDateRangePickerComponent) laststatusupdatedate: NgDateRangePickerComponent
    @Output('updateddate') _updateddate: EventEmitter<any> = new EventEmitter<any>();

    ngOnInit() {  
        
        this.options = {
          theme: 'default',
          range: 'em',
          previousIsDisable: false,
        nextIsDisable: false,
          dayNames: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
          presetNames: ['Today', 'This Month', 'Last Month', 'This Week', 'Last Week', 'This Year', 'Last Year'],
          dateFormat: 'yMd',
          outputFormat: 'DD/MM/YYYY',
          startOfWeek: 0,
          display: { to: 'block', tm: 'block', lm: 'block', lw: 'block', tw: 'block', ty: 'none', ly: 'none', custom: 'block', em: 'block' }
        }; 

      }   
}
