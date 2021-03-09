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
  // private _toastService?: ToastrService;

  handleError(_error: any) {
    // if (!isTruthy(this._toastService)) {
    //   this._toastService = this.injector.get(ToastrService);
    // }

    // this._toastService.error('Error Contact Administrator', null, { onActivateTick: true });
     //console.log(_error);    
     const browserBaseUrl = window.location.href;
    if (isTruthy(_error)) {
      if (_error.message.toString().includes("Cannot match any routes")) {
        const routerService = this.injector.get(Router);
        const ngZone = this.injector.get(NgZone);
        ngZone.run(() => {
          routerService.navigate(['']);
        });
        console.clear();
      }
      const _log = { Error: { message: _error.message+', BrowserUrl : '+browserBaseUrl, stack: isTruthy(_error.stack) ? _error.stack : '' } };
      this._commonSerivce.postError(_log);
    }
    console.clear();
  }
}
