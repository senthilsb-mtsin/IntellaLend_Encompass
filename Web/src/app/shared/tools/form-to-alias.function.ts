// import { isTruthy } from './is-truthy.function';
// import { IJsonMemberOptions } from 'typedjson/js/typedjson/json-member';
// import { jsonMember, jsonObject } from 'typedjson/js/typedjson';

// export function mtsJsonMember(name: string, options?: IJsonMemberOptions): PropertyDecorator {
//     return function (target: Function, propertyKey: string | symbol) {
//         if (!isTruthy(options)) {
//             options = {};
//         }
//         options = {
//             ...options,
//             name: name
//         };
//         const jsonMemberFunction = jsonMember(options);
//         jsonMemberFunction(target, propertyKey);
//     };
// }

// export function mtsJsonObject<T>() {
//     return function (type: (new () => T)): void {
//         const jsonObjectFunction = jsonObject();
//         jsonObjectFunction(type);
//     };
// }

// export function alias(name: string, isDate = false) {
//     return function (target: Object, propertyKey: string | symbol) {
//         if (!target['constructor'].hasOwnProperty('_alias')) {
//             target['constructor']['_alias'] = Object.assign({}, target['constructor']['_alias']);
//         }
//         target['constructor']['_alias'][propertyKey] = { name: name, isDate: isDate };
//     };
// }