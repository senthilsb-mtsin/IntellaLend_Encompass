import { Component, OnInit, OnDestroy } from '@angular/core';
import { ReverificationService } from '../../services/reverification.service';
import { ReverificationdetailsModel } from '../../models/reverification-details.model';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Component({
    selector: 'mts-viewreverification',
    templateUrl: 'viewreverification.page.html',
    styleUrls: ['viewreverification.page.css'],
})
export class ViewReverificationComponent implements OnInit, OnDestroy {
    TemplateName: any = '';
    _templatemasters: any = [];
    _viewReverificationData: ReverificationdetailsModel = new ReverificationdetailsModel();
    constructor(private _reverificationService: ReverificationService) {

    }
    private subscription: Subscription[] = [];
    ngOnInit() {
        this.subscription.push(this._reverificationService.TemplateMaster.subscribe((res: any) => {
            this._templatemasters = res;
            this.GetTemplateName(this._viewReverificationData.TemplateID);
        }));
        this.subscription.push(this._reverificationService.ReverificationData.subscribe((res: ReverificationdetailsModel) => {
            this._viewReverificationData = res;
        }));
        this._reverificationService.GetRowdata();
        this._reverificationService.GetReverificationTemplate();
    }
    CloseReverification() {
        this._reverificationService.CloseReverification();

    }
    GetTemplateName(id) {
        const tempMas = this._templatemasters.filter(x => x.TemplateID === id);
        if (tempMas.length <= 0) { return ''; }
        this.TemplateName = tempMas[0].TemplateName;

    }

    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
