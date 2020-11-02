
export class PurgeCommon {

   ReviewTypeID: number;
   AuditMonthYear: any;
   FromDate: any;
   ToDate: any;
   checklistitemResult: any;

   constructor(ReviewTypeID: number, AuditMonthYear: any, FromDate: any, ToDate: any,    checklistitemResult: any) {
    this.ReviewTypeID = ReviewTypeID ;
    this.AuditMonthYear = AuditMonthYear;
    this.FromDate = FromDate;
    this.ToDate = ToDate;
    this.checklistitemResult = checklistitemResult;

  }
}
