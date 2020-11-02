import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { DocumentTypeService } from '../../services/documenttype.service';
import { DocumentTypeDatatableModel } from '../../models/document-type-datatable.model';
import { CommonService } from 'src/app/shared/common';
import { AppSettings } from '@mts-app-setting';

@Component({
    selector: 'mts-viewdocumenttype',
    templateUrl: 'viewdocumenttype.page.html',
    styleUrls: ['viewdocumenttype.page.css'],
})
export class ViewDocumentTypeComponent implements OnInit {

    _viewDocumentTypeData: DocumentTypeDatatableModel = new DocumentTypeDatatableModel();
    _parkingSpotItems: any[] = [];

    constructor(private _documentTypeService: DocumentTypeService, private _commonService: CommonService) {

    }
    private subscription: Subscription[] = [];

    ngOnInit() {
        this._commonService.GetParkingSpotList(AppSettings.TenantSchema);
        this.subscription.push(this._commonService.SystemParkingSpotItems.subscribe((res: any) => {
            this._parkingSpotItems = res;
        }));
        this._viewDocumentTypeData = this._documentTypeService._documentTypeRowData;
        this._documentTypeService.getDocumentTypeRowData();

    }
    CloseViewDoc() {
        this._documentTypeService.GotoPrevious();
    }
    ngDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }

}
