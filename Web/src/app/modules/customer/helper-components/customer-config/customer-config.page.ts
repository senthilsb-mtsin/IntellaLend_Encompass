import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { UpsertCustomerService } from '../../services/upsert-customer.service';
import { NotificationService } from '@mts-notification';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { CustConfigKeyConstant } from '@mts-status-constant';

@Component({
    selector: 'mts-customer-config',
    styleUrls: ['customer-config.page.css'],
    templateUrl: 'customer-config.page.html'
})
export class CustomerConfigComponent implements OnInit, OnDestroy {

    promise: Subscription;
    getCustConfigForm: FormGroup;
    months: any = [{ 'id': '', 'text': 'Select' }, { 'id': 1, 'text': 'Day (1)' }, { 'id': 30, 'text': 'Month (30)' }, { 'id': 365, 'text': 'Year (365)' }];
    configKeys = ['Retention_Policy', 'Export_Path'];
    isPathNotValid = true;

    constructor(
        private fb: FormBuilder,
        private _upsertCustomerService: UpsertCustomerService,
        private _notificationService: NotificationService
    ) {
        this.CreateForm();
    }

    private _subscriptions: Subscription[] = [];
    private _customerConfig: { ConfigID: number, CustomerID: number, CustomerName: string, Configkey: string, ConfigValue: string, Active: boolean }[];
    private retDays = 0;
    private days = 0;

    get formData() { return this.getCustConfigForm.get('ConfigItems') as FormArray; }

    ngOnInit() {
        this._subscriptions.push(this._upsertCustomerService.CustomerConfig$.subscribe(res => {
            this._customerConfig = res;
            this.generateConfigItem();
        }));

        this._upsertCustomerService.GetCustomerConfigData();
    }

    generateConfigItem(): void {
        if (this._customerConfig.length === 0) {
            this.configKeys.forEach(element => {
                this.formData.push(this.createConfigItem(element, '', true));
            });
        } else {
            this._customerConfig.forEach(element => {

                this.formData.push(this.createConfigItem(element.Configkey, element.ConfigValue, element.Active));
            });
        }
    }

    createConfigItem(_keyname: string, _keyvalue: string, _active: boolean): FormGroup {
        const vals = _keyvalue.split(',');
        if (vals.length === 3) {
            return this.fb.group({
                ConfigKey: _keyname,
                ConfigValue: [_keyvalue],
                inputValues: [vals[2]],
                daysValues: [vals[1]],
                Active: _active
            }, {
                validator: (formGroup: FormGroup) => {
                    return this.validateAllControls(formGroup);
                }
            });
        } else {
            return this.fb.group({
                ConfigKey: _keyname,
                ConfigValue: [_keyvalue],
                inputValues: [''],
                daysValues: [''],
                Active: _active
            },
                {
                    validator: (formGroup: FormGroup) => {
                        return this.validateAllControls(formGroup);
                    }
                });
        }
    }

    validateAllControls(form) {
        if (CustConfigKeyConstant.CONFIG_VALIDATION[form.controls.ConfigKey.value] === 'Path') {
            const vals = form.controls.ConfigValue.value.trim();
            const regEx = /^\\\\([^\\:\|\[\]\/";<>+=,?* _]+)\\([\u0020-\u0021\u0023-\u0029\u002D-\u002E\u0030-\u0039\u0040-\u005A\u005E-\u007B\u007E-\u00FF]{1,80})(((?:\\[\u0020-\u0021\u0023-\u0029\u002D-\u002E\u0030-\u0039\u0040-\u005A\u005E-\u007B\u007E-\u00FF]{1,255})+?|)(?:\\((?:[\u0020-\u0021\u0023-\u0029\u002B-\u002E\u0030-\u0039\u003B\u003D\u0040-\u005B\u005D-\u007B]{1,255}){1}(?:\:(?=[\u0001-\u002E\u0030-\u0039\u003B-\u005B\u005D-\u00FF]|\:)(?:([\u0001-\u002E\u0030-\u0039\u003B-\u005B\u005D-\u00FF]+(?!\:)|[\u0001-\u002E\u0030-\u0039\u003B-\u005B\u005D-\u00FF]*)(?:\:([\u0001-\u002E\u0030-\u0039\u003B-\u005B\u005D-\u00FF]+)|))|)))|)$/;
            const regEx1 = /^([a-zA-Z]:)?(\\[a-zA-Z0-9_\-]+)+\\?/;
            const RegTest = regEx.test(vals);
            const RegTest1 = regEx1.test(vals);
            const _pathValid = (RegTest || RegTest1) ? null : {
                validateEP: {
                    errors: true
                }
            };
            if (_pathValid === null) {
                // this.isPathNotValid = false;
                return false;
            } else {
                return true;
               // this.isPathNotValid = true;
                // this._notificationService.showError("Enter Valid Export Path");
            }
        }
    }

    CreateForm() {
        this.getCustConfigForm = this.fb.group({ CustomerID: this._upsertCustomerService.GetCurrentCustomer().CustomerID, ConfigItems: this.fb.array([]) });
    }

    DaysChanged(event) {
        this.formData.controls.forEach(element => {
            if (element.get('ConfigKey').value === 'Retention_Policy') {
                this.retDays = parseInt(element.get('inputValues').value, 10) * event.target.value;
                this.days = event.target.value;
            }
        });
    }

    SaveRetention() {
        this.formData.controls.forEach(element => {
            if (element.get('ConfigKey').value === 'Retention_Policy') {
                element.get('ConfigValue').setValue(this.retDays + ',' + this.days + ',' + element.get('inputValues').value);
            }
        });
        this._upsertCustomerService.AddCustomerConfigData(this.formData.value);
    }

    ngOnDestroy() {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
