import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'fileextension'
})

export class CheckFileExtension implements PipeTransform {

  transform(values: any): any {
    if (values !== null) {

      if (values.match(/\.(jpe?g|pdf|tiff|tif)$/i)) {
        return true;
      } else {
        return false;
      }
    }
  }
}
