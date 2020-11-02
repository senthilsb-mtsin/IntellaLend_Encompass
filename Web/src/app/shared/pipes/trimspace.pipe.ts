import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'trimspace',
})
export class TrimspacePipe implements PipeTransform {
  transform(str: any): any {
    return str.replace(/[\s]/g, '');
  }
}
