import { Component, OnInit, OnDestroy } from '@angular/core';
import { ServiceTypeModel } from '../../models/service-type.model';
import { Router } from '@angular/router';
import { ServiceTypeService } from '../../service/service-type.service';
import { ServiceTypePriorityModel } from '../../models/service-type-priority.model';
import { ServiceTypeRoleModel } from '../../models/service-type-role.model';
import { AddServiceTypeService } from '../../service/add-service-type.service';
import { Subscription } from 'rxjs';
import { Location } from '@angular/common';

@Component({
    selector: 'mts-view-service-type',
    templateUrl: 'view-service-type.page.html',
    styleUrls: ['view-service-type.page.css'],
})
export class ViewServiceTypeComponent implements OnInit, OnDestroy {

    getReviewTypeData: ServiceTypeModel;
    priorityList: ServiceTypePriorityModel[] = [];
    RoleItems: ServiceTypeRoleModel[] = [];
    constructor(
        private _route: Router, private _location: Location,
        private _serviceTypeService: ServiceTypeService, private _addServiceTypeService: AddServiceTypeService) { }

    private subscriptions: Subscription[] = [];

    ngOnInit() {
        this.getReviewTypeData = this._addServiceTypeService.getCurrentReviewDetails();

        this.subscriptions.push(this._addServiceTypeService.priorityList.subscribe((res: ServiceTypePriorityModel[]) => {
            this.priorityList = [...res];
        }));
        this.subscriptions.push(this._addServiceTypeService.roleList.subscribe((res: ServiceTypeRoleModel[]) => {
            this.RoleItems = [...res];
        }));
    }

    GotoMaster() {
        this._location.back();
    }

    ngOnDestroy() {
        this.subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
