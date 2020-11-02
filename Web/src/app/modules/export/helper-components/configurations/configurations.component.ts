import { Component } from '@angular/core';
import { ExportLoanService } from '../../service/export-loan.service';

@Component({
  selector: 'mts-configurations',
  templateUrl: './configurations.component.html',
  styleUrls: ['./configurations.component.css']
})
export class ConfigurationsComponent {
  scrollbarOptions = { axis: 'yx', theme: 'minimal-dark', scrollbarPosition: 'inside' };
  constructor(public _exportloanservice: ExportLoanService) { }
  SetPasswordConfig(isactive: any) {
    if (isactive === true) {
      this._exportloanservice.Password = '';
      this._exportloanservice.showmodel$.next('password');
    } else {
      this._exportloanservice.PasswordProtected = false;
      this._exportloanservice.Password = '';
    }
  }
  ShowCoverLetter() {
    if (this._exportloanservice.CoverLetter) {
      this._exportloanservice.showmodel$.next('cover');
    }
  }
}
