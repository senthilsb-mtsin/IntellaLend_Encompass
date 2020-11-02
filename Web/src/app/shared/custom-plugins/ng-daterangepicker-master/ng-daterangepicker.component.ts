import { Component, OnInit, HostListener, ElementRef, forwardRef, Input, OnChanges, SimpleChange } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import * as dateFns from 'date-fns';
import { DatePipe } from '@angular/common';

export interface NgDateRangePickerOptions {
  theme: 'default' | 'green' | 'teal' | 'cyan' | 'grape' | 'red' | 'gray';
  range: 'to' | 'tm' | 'lm' | 'lw' | 'tw' | 'ty' | 'ly' | 'custom' | 'em';
  dayNames: string[];
  presetNames: string[];
  dateFormat: string;
  outputFormat: string;
  startOfWeek: number;
  display: any;
  previousIsDisable: boolean;
  nextIsDisable: boolean;
}

export interface IDay {
  date: Date;
  day: number;
  weekday: number;
  today: boolean;
  firstMonthDay: boolean;
  lastMonthDay: boolean;
  visible: boolean;
  from: boolean;
  to: boolean;
  isWithinRange: boolean;
}

export let DATERANGEPICKER_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => NgDateRangePickerComponent),
  multi: true
};

@Component({
  selector: 'ng-daterangepicker',
  templateUrl: './ng-daterangepicker.component.html',
  styleUrls: ['./ng-daterangepicker.component.sass'],
  providers: [DATERANGEPICKER_VALUE_ACCESSOR]
})
export class NgDateRangePickerComponent implements ControlValueAccessor, OnInit, OnChanges {
  get value(): string {
    return this.modelValue;
  }

  set value(value: string) {
    if (!value) { return; }
    this.modelValue = value;
    this.onChangeCallback(value);
  }
  @Input() options: NgDateRangePickerOptions;

  modelValue: string;
  opened: false | 'from' | 'to';
  date: Date;
  dateFrom: Date;
  dateTo: Date;
  dayNames: string[];
  days: IDay[];
  range: 'to' | 'tm' | 'lm' | 'lw' | 'tw' | 'ty' | 'ly' | 'custom' | 'em';
  defaultOptions: NgDateRangePickerOptions = {
    theme: 'default',
    previousIsDisable: false,
    nextIsDisable: false,
    range: 'to',
    dayNames: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
    presetNames: ['Today', 'This Month', 'Last Month', 'This Week', 'Last Week', 'This Year', 'Last Year', 'Start', 'End'],
    dateFormat: 'yMd',
    outputFormat: 'DD/MM/YYYY',
    startOfWeek: 0,
    display: { to: 'block', tm: 'block', lm: 'block', lw: 'block', tw: 'block', ty: 'block', ly: 'block', custom: 'block', em: 'block' }
  };

  constructor(private elementRef: ElementRef, public datePipe: DatePipe, ) { }

  writeValue(value: string) {
    if (!value) { return; }
    this.modelValue = value;
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

  ngOnInit() {
    this.opened = false;
    this.date = dateFns.startOfDay(new Date());
    this.options = this.options || this.defaultOptions;
    this.initNames();
    this.selectRange(this.options.range);
  }

  ngOnChanges(changes: { [propName: string]: SimpleChange }) {
    this.options = this.options || this.defaultOptions;
  }

  initNames(): void {
    this.dayNames = this.options.dayNames;
  }

  generateCalendar(): void {
    this.days = [];
    const start: Date = dateFns.startOfMonth(this.date);
    const end: Date = dateFns.endOfMonth(this.date);

    const days: IDay[] = dateFns.eachDay(start, end).map(d => {
      return {
        date: d,
        day: dateFns.getDate(d),
        weekday: dateFns.getDay(d),
        today: dateFns.isToday(d),
        firstMonthDay: dateFns.isFirstDayOfMonth(d),
        lastMonthDay: dateFns.isLastDayOfMonth(d),
        visible: true,
        from: dateFns.isSameDay(this.dateFrom, d),
        to: dateFns.isSameDay(this.dateTo, d),
        isWithinRange: dateFns.isWithinRange(d, this.dateFrom, this.dateTo)
      };
    });

    // let prevMonthDayNum = dateFns.getDay(start) - 1;
    const prevMonthDayNum = dateFns.getDay(start) === 0 ? 6 : dateFns.getDay(start) - 1;
    let prevMonthDays: IDay[] = [];
    if (prevMonthDayNum > 0) {
      prevMonthDays = Array.from(Array(prevMonthDayNum).keys()).map(i => {
        const d = dateFns.subDays(start, prevMonthDayNum - i);
        return {
          date: d,
          day: dateFns.getDate(d),
          weekday: dateFns.getDay(d),
          firstMonthDay: dateFns.isFirstDayOfMonth(d),
          lastMonthDay: dateFns.isLastDayOfMonth(d),
          today: false,
          visible: false,
          from: false,
          to: false,
          isWithinRange: false
        };
      });
    }

    this.days = prevMonthDays.concat(days);
    this.value = `${dateFns.format(this.dateFrom, this.options.outputFormat)}-${dateFns.format(this.dateTo, this.options.outputFormat)}`;
  }

  toggleCalendar(e: MouseEvent, selection: 'from' | 'to'): void {
    // //
    if (this.opened && this.opened !== selection) {
      this.opened = selection;
    } else {
      this.opened = this.opened ? false : selection;
    }
  }

  closeCalendar(e: MouseEvent): void {
    this.opened = false;
  }

  selectDate(e: MouseEvent, index: number): void {
    // //
    e.preventDefault();
    const selectedDate: Date = this.days[index].date;
    if (this.range === 'em' && this.dateTo === null && this.opened === 'from') {
      this.dateTo = selectedDate;
      // this.dateFrom =selectedDate
    } else if (this.range === 'em' && this.dateFrom === null && this.opened === 'to') {
      // this.dateTo =selectedDate
      this.dateFrom = selectedDate;
    }

    if ((this.opened === 'from' && dateFns.isAfter(selectedDate, this.dateTo)) ||
      (this.opened === 'to' && dateFns.isBefore(selectedDate, this.dateFrom))) {
      return;
    }
    // }

    if (this.opened === 'from') {
      this.dateFrom = selectedDate;
      this.opened = 'to';
    } else if (this.opened === 'to') {
      this.dateTo = selectedDate;
      this.opened = 'from';
    }
    this.generateCalendar();
  }

  prevMonth(): void {
    const calMonth = dateFns.getMonth(this.date);
    const currtMonth = dateFns.getMonth(Date());
    if (!this.options.previousIsDisable || currtMonth !== calMonth) {
      this.date = dateFns.subMonths(this.date, 1);
      this.generateCalendar();
    }
  }

  nextMonth(): void {
    const calMonth = dateFns.getMonth(this.date);
    const currtMonth = dateFns.getMonth(Date());
    if (!this.options.nextIsDisable || currtMonth !== calMonth) {
      this.date = dateFns.addMonths(this.date, 1);
      this.generateCalendar();
    }
  }

  selectRange(range: 'to' | 'tm' | 'lm' | 'lw' | 'tw' | 'ty' | 'ly' | 'custom' | 'em', dateRange?: any): void {
    let today = dateFns.startOfDay(new Date());

    switch (range) {
      case 'to':
        this.dateFrom = today;
        this.dateTo = today;
        break;
      case 'tm':
      // var makeDate = new Date(today);
      //   makeDate.setMonth(makeDate.getMonth() - 1);
      //   var ldate = makeDate.toString();
        this.dateFrom = dateFns.startOfMonth(today);
        this.dateTo = dateFns.endOfMonth(today);
        break;
      case 'lm':
        today = dateFns.subMonths(today, 1);
        this.dateFrom = dateFns.startOfMonth(today);
        this.dateTo = dateFns.endOfMonth(today);
        break;
      case 'lw':
        today = dateFns.subWeeks(today, 1);
        this.dateFrom = dateFns.startOfWeek(today, { weekStartsOn: this.options.startOfWeek });
        this.dateTo = dateFns.endOfWeek(today, { weekStartsOn: this.options.startOfWeek });
        break;
      case 'tw':
        this.dateFrom = dateFns.startOfWeek(today, { weekStartsOn: this.options.startOfWeek });
        this.dateTo = dateFns.endOfWeek(today, { weekStartsOn: this.options.startOfWeek });
        break;
      case 'ty':
        this.dateFrom = dateFns.startOfYear(today);
        this.dateTo = dateFns.endOfYear(today);
        break;
      case 'ly':
        today = dateFns.subYears(today, 1);
        this.dateFrom = dateFns.startOfYear(today);
        this.dateTo = dateFns.endOfYear(today);
        break;
      case 'custom':
        const makeDate = new Date(today);
        makeDate.setMonth(makeDate.getMonth() - 1);
        this.dateFrom = makeDate;
        this.dateTo = today;
        break;
      case 'em':
        this.dateFrom = null;
        this.dateTo = null;
        break;
    }

    this.range = range;
    this.generateCalendar();
  }

  @HostListener('document:click', ['$event'])
  handleBlurClick(e: MouseEvent) {
    const target = e.srcElement || e.target;
    if (!this.elementRef.nativeElement.contains(e.target) && !(<Element>target).classList.contains('day-num')) {
      this.opened = false;
    }
  }

  private onTouchedCallback: () => void = () => { };
  private onChangeCallback: (_: any) => void = () => { };
}
