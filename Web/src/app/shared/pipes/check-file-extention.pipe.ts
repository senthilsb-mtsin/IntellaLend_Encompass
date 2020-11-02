import { Pipe, PipeTransform } from '@angular/core';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Pipe({
    name: 'fileextension'
})

export class CheckFileExtension implements PipeTransform {

    transform(values: any): any {
        const status = '';
        if (isTruthy(status)) {

            if (values.match(/\.(jpe?g|pdf|tiff|tif)$/i)) {
                return true;
            } else {
                return false;
            }
        }
    }
}
