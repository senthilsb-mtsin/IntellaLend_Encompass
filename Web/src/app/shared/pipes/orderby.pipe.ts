import { Pipe, PipeTransform } from '@angular/core';
@Pipe({ name: 'orderBy' })
export class OrderByPipe implements PipeTransform {
  transform(records: any, args?: any): any {
    return records.sort(function (a, b) {
      if (
        a[args.property].toString().toLowerCase() <
        b[args.property].toString().toLowerCase()
      ) {
        return -1 * args.direction;
      } else if (
        a[args.property].toString().toLowerCase() >
        b[args.property].toString().toLowerCase()
      ) {
        return 1 * args.direction;
      } else {
        return 0;
      }
    });
  }
}
