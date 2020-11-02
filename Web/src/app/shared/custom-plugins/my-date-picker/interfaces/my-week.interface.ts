import { IMyCalendarDay } from './my-calendar-day.interface';

export interface IMyWeek {
    week: IMyCalendarDay[];
    weekNbr: number;
}
