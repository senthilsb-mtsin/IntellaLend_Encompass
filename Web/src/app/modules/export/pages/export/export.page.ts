import { Component, OnInit } from '@angular/core';
import { SessionHelper } from '@mts-app-session';
@Component({
  selector: 'mts-export',
  templateUrl: './export.page.html',
  styleUrls: ['./export.page.css']
})
export class ExportComponent implements OnInit {
  isExportMonitorDataExists = false;
  isExportMonitorTab: any = [false, false, false];
  exportTab: any;
  constructor() {
    this.checkPermission('Adhoc.export', 0);
    this.checkPermission('Encompass.export', 1);
    this.checkPermission('Los.export', 2);

  }
  ngOnInit(): void {
    if (this.isExportMonitorTab[0]) {
      this.exportTab = 'ad_loanexport';
    } else if (!this.isExportMonitorTab[0] && this.isExportMonitorTab[1]) {
      this.exportTab = 'encompass_export';
    } else if (!this.isExportMonitorTab[0] && !this.isExportMonitorTab[1]) {
      this.exportTab = 'los_export';

    }

  }
  checkPermission(component: string, index: number): void {
    const URL = 'View\\ExportMonitor\\' + component;
    const AccessCheck = false;

    const AccessUrls = SessionHelper.RoleDetails.URLs;

    if (AccessCheck !== null) {
      AccessUrls.forEach(element => {
        if (element.URL === URL) {
          this.isExportMonitorTab[index] = true;
          return false;
        }
      });
    }
  }
}
