import { Subscription } from 'rxjs';
import { LoanPurgeService } from '../../service/loanpurge.service';
import {
  Component,
  OnInit,
  ViewChild,
  OnDestroy,
} from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { DatePipe } from '@angular/common';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-purgeloan',
  templateUrl: './purgeloan.page.html',
  styleUrls: ['./purgeloan.page.css'],
})
export class PurgeloanComponent implements OnInit, OnDestroy {
  promise: Subscription;
  dtOptions: any = {};
  isPurgeMonitorTab = false;

  dTable: any;
  @ViewChild(DataTableDirective) dt: DataTableDirective;
  constructor(
    private _purgeservice: LoanPurgeService,
    private datePipe: DatePipe
  ) { }

  private ReviewStatus: any = 0;
  private purgeMonitorFromdate: any;
  private purgeMonitorTodate: any;
  private subscription: Subscription[] = [];
  private isRowSelectEnabled = true;
  private isRowSelectrDeselect = true;

  ngOnInit(): void {
    this.subscription.push(
      this._purgeservice.isRowSelectEnabled.subscribe((res: boolean) => {
        this.isRowSelectEnabled = res;
      })
    );
    this.subscription.push(
      this._purgeservice.isRowSelectrDeselect.subscribe((res: boolean) => {
        this.isRowSelectrDeselect = res;
      })
    );
    this.subscription.push(
      this._purgeservice.isPurgeMonitorTab.pipe(first()).subscribe((res: boolean) => {
        this.isPurgeMonitorTab = res;
        this._purgeservice.isPurgeMonitorTab.next(false);
      })
    );

  }

  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
