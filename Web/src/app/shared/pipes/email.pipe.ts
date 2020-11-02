import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
    name: 'emailpipe'
})

export class EmailCheckPipe implements PipeTransform {

    transform(value: any): any {

        if (value === undefined) { return false; }

        if (value === '') { return false; }
        const exp = new RegExp('^[_a-z0-9]+(.[_a-z0-9]+)*@[a-zA-Z0-9.-]+[.]+[a-zA-Z.]{2,6}$');
        return exp.test(value);
    }

}
