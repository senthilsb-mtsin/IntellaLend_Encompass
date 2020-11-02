import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { CommonService } from 'src/app/shared/common';
import { AppSettings } from '@mts-app-setting';
import { ManagerDocumentsDatatableModel } from '../../models/manager-documents-table.model';
import { ManagerDoctypeService } from '../../services/manage-doctype.service';
import { Router } from '@angular/router';

@Component({
    selector: 'mts-manageviewdoctype',
    templateUrl: 'manageviewdoctype.page.html',
    styleUrls: ['manageviewdoctype.page.css'],
})
export class ManageViewDocumentTypeComponent implements OnInit, OnDestroy {

    _manageViewDocumentTypeData: ManagerDocumentsDatatableModel = new ManagerDocumentsDatatableModel();
    _parkingSpotItems: any[] = [];
    AuthorityLabelSingular = AppSettings.AuthorityLabelSingular;
    IsView = false;
    constructor(private _managerDocumentTypeService: ManagerDoctypeService, private _commonService: CommonService, private _route: Router) {

    }
    private subscription: Subscription[] = [];

    ngOnInit() {
        this._manageViewDocumentTypeData = this._managerDocumentTypeService._manageDocumentTypeRowData;
        this._managerDocumentTypeService.getDocumentTypeRowData();

    }
    CloseViewDoc() {
        this._route.navigate(['view/mdocumenttype']);
        this._managerDocumentTypeService.SetLoading();

    }
    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }

}
