import { Injectable, ErrorHandler, Injector, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { CommonService } from '../common/common.service';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class CustomExceptionHandler implements ErrorHandler {
  constructor(
    private injector: Injector,
    private _commonSerivce: CommonService
  ) { }
  //private _toastService?: ToastrService;

  handleError(_error: any) {
    // if (!isTruthy(this._toastService)) {
    //   this._toastService = this.injector.get(ToastrService);
    // }

   // this._toastService.error('Error Contact Administrator', null, { onActivateTick: true });
    // console.log(_error);
    if (isTruthy(_error)) {
      const _log = { Error: _error };
      this._commonSerivce.postError(_log);
    }
    // console.clear();
  }
}
