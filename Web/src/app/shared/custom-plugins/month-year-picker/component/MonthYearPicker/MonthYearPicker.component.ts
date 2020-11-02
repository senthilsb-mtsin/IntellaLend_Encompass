import { Component, OnInit, Input, Output, EventEmitter, ElementRef, HostListener, ChangeDetectorRef, AfterViewChecked, OnChanges } from '@angular/core';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'MonthYearPicker',
  templateUrl: './MonthYearPicker.component.html',
  styleUrls: ['./MonthYearPicker.component.css'],
  providers: [DatePipe]
})
export class MonthYearPickerComponent implements OnInit, OnChanges, AfterViewChecked {
  _switchToMonthCall = false;
  @Input() model: string | Date;
  @Input() config: ImonthPickerConfig;
  @Input('SelectedDate') _selectedDate: any;
  @Output() modelChange = new EventEmitter();
  _monthPicker: MonthPicker;
  _currtDate: IDatePickerSelectionItem = { text: '', value: 0, type: '' };
  constructor(private _elementRef: ElementRef, private cdRef: ChangeDetectorRef) {
    this._monthPicker = new MonthPicker();
  }

  ngAfterViewChecked() {
    this.cdRef.detectChanges();
  }

  ngOnInit() {
    if (this._selectedDate !== '') {
      // const tempDate = new Date();
      const tempMonths = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

      this._currtDate.text = tempMonths[(this._selectedDate.getMonth())];
      this._currtDate.value = this._selectedDate.getMonth();
      this._currtDate.type = 'm';
      this._monthPicker.displayYear = this._selectedDate.getFullYear();
      this.onSelectMonth(this._currtDate, true);
    }
  }

  setValue(dateValue) {
    let tempDate = new Date();
    const tempMonths = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    if (typeof dateValue !== 'undefined' && dateValue !== '') {
      tempDate = new Date(dateValue);
      this._currtDate.text = tempMonths[(tempDate.getMonth())];
      this._currtDate.value = tempDate.getMonth();
      this._currtDate.type = 'm';
      this._monthPicker.displayYear = tempDate.getFullYear();
      this.onSelectMonth(this._currtDate, true);
    } else if (typeof dateValue === 'undefined' || dateValue === '') {
      this._monthPicker = new MonthPicker();
      this.model = '';
      this.modelChange.next({ Value: this.model, OnInit: true });
    }

  }

  ngOnChanges(changes: any) {
    if (this.model) {
      this._monthPicker.setCurrentdate((typeof this.model === 'string') ? new Date(this.model) : this.model);
    }
  }

  onCalendarIconClick() {
    this.switchToMonthMode();
    this._monthPicker.setCurrentdate(this.model ? (typeof this.model === 'string') ? new Date(this.model) : this.model : new Date());
    this._monthPicker.toggleState();
  }
  switchToYearMode() {
    this._monthPicker.viewMode = 'y';
    this._monthPicker.fillYearsInSelectionList();
  }
  switchToMonthMode() {
    this._switchToMonthCall = true;
    this._monthPicker.viewMode = 'm';
    this._monthPicker.fillMonthsInSelectionList();
  }
  onselectionItemClick(item: IDatePickerSelectionItem) {
    if (item.type === 'y') {
      this._monthPicker.displayYear = item.value;
      this.switchToMonthMode();
    } else if (item.type === 'm') {
      this.onSelectMonth(item, false);
    }
  }
  onSelectMonth(item: IDatePickerSelectionItem, OnLoad: boolean) {
    this._monthPicker.displayMonth = item.text;
    this._monthPicker.displayMonthIndex = item.value;

    this._monthPicker.selectedMonth = item.text;
    this._monthPicker.selectedMonthIndex = item.value;
    this._monthPicker.selectedYear = this._monthPicker.displayYear;

    const monthIndex = (this._monthPicker.selectedMonthIndex + 1);

    this.model = ((monthIndex > 9) ? monthIndex : '0' + monthIndex) + '/01/' + this._monthPicker.selectedYear;
    // this.model = new Date(this._monthPicker.selectedYear, this._monthPicker.selectedMonthIndex, 1);
    this._monthPicker.state = 'closed';
    this.modelChange.next({ Value: this.model, OnInit: OnLoad });
  }

  onPrevYearSelection() {
    this._monthPicker.displayYear--;
    if (this._monthPicker.viewMode === 'y') { this._monthPicker.fillYearsInSelectionList(); }
  }
  onNextYearSelection() {
    this._monthPicker.displayYear++;
    if (this._monthPicker.viewMode === 'y') { this._monthPicker.fillYearsInSelectionList(); }
  }

  onCancel() {
    this._monthPicker.state = 'closed';
  }

  @HostListener('document:click', ['$event', '$event.target'])
  onClick(event: MouseEvent, targetElement: HTMLElement): void {
    if (!targetElement) {
      return;
    }
    if (!this._switchToMonthCall && this._monthPicker.viewMode !== 'm') {
      const clickedInside = this._elementRef.nativeElement.contains(targetElement);
      if (!clickedInside) {
        this._monthPicker.state = 'closed';
      }
    } else {
      if (this._monthPicker.state === 'open' && (targetElement.className.indexOf('myMonthAuditPicker') === -1)) {
        this._monthPicker.state = 'closed';
      }
    }
  }
}
export interface ImonthPickerConfig {
  readonly?: boolean;
  cssClass?: string;
  placeHolder?: string;
}
export interface IDatePickerSelectionItem {
  text: string;
  value: number;
  type: string;
}
class MonthPicker {
  state: string;
  selectionItems: IDatePickerSelectionItem[];
  selectedMonth: string;
  selectedMonthIndex: number;
  selectedYear: number;
  displayMonth: string;
  displayMonthIndex: number;
  displayYear: number;
  viewMode: string;
  constructor() {
    this.state = 'closed';
    this.viewMode = 'm';
    this.fillMonthsInSelectionList();
    const currentDate = new Date();
    this.setCurrentdate(currentDate);
  }
  private months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
  toggleState() {
    this.state = this.state === 'closed' ? 'open' : 'closed';
  }

  fillMonthsInSelectionList() {
    this.selectionItems = [];
    this.months.forEach((v: string, i: number) => this.selectionItems.push({ text: v, value: i, type: 'm' }));
  }
  fillYearsInSelectionList() {
    this.selectionItems = [];
    for (let start = this.displayYear - 6; start <= this.displayYear + 5; start++) {
      this.selectionItems.push({ text: start.toString(), value: start, type: 'y' });
    }
  }
  setCurrentdate(currentDate: Date) {
    this.displayMonth = this.months[(currentDate.getMonth())];
    this.displayMonthIndex = currentDate.getMonth();
    this.displayYear = currentDate.getFullYear();

    this.selectedMonth = this.displayMonth;
    this.selectedMonthIndex = this.displayMonthIndex;
    this.selectedYear = this.displayYear;
  }
}
