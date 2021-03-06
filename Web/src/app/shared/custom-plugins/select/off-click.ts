import { Directive, HostListener, Input, OnInit, OnDestroy } from '@angular/core';

@Directive({
  selector: '[offClick]'
})

export class OffClickDirective implements OnInit, OnDestroy {
  /* tslint:disable */
  @Input('offClick') public offClickHandler: any;
  /* tslint:enable */
  @HostListener('click', ['$event']) onClick($event: MouseEvent): void {
    $event.stopPropagation();
  }

  ngOnInit(): any {
    setTimeout(() => { if (typeof document !== 'undefined') { document.addEventListener('click', this.offClickHandler); } }, 0);
  }

  ngOnDestroy(): any {
    if (typeof document !== 'undefined') { document.removeEventListener('click', this.offClickHandler); }
  }
}
