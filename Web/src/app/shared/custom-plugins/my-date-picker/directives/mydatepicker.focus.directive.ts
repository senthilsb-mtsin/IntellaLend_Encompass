import { Directive, ElementRef, AfterViewInit, Input } from '@angular/core';

@Directive({
    selector: '[mydpfocus]'
})

export class FocusDirective implements AfterViewInit {
    @Input('mydpfocus') value: string;

    constructor(private el: ElementRef) {}

    // Focus to element: if value 0 = don't set focus, 1 = set focus
    ngAfterViewInit() {
        if (this.value === '0') {
            return;
        }
        this.el.nativeElement.focus();
    }
}
