import { PipeTransform, Pipe } from '@angular/core';

@Pipe({
    name: 'checkzipcode'
})

export class ValidateZipcodePipe implements PipeTransform {

    transform(value: any): any {

        if (value === undefined) { return false; }

        if (value === '') { return false; }

        const exp = new RegExp('^[0-9]{5}(?:[-\s][0-9]{4})?$');

        return exp.test(value);
    }
}
