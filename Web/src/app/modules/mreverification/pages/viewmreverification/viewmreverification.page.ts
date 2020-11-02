import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { ManagerReverificationdetailsModel } from '../../models/manager-reverification-details.model';
import { ReverificationService } from 'src/app/modules/reverification/services/reverification.service';
import { ManagerReverificationService } from '../../services/manager-reverification.service';
import { Router } from '@angular/router';
import { AppSettings } from '@mts-app-setting';

@Component({
    selector: 'mts-viewmreverification',
    templateUrl: 'viewmreverification.page.html',
    styleUrls: ['viewmreverification.page.css'],
})
export class ViewManagerReverificationComponent implements OnInit, OnDestroy {
    TemplateName: any = '';
    _templatemasters: any = [];
    _viewReverificationData: ManagerReverificationdetailsModel = new ManagerReverificationdetailsModel();
    AuthorityLabelSingular = AppSettings.AuthorityLabelSingular;
     constructor(private _managerReverificationService: ManagerReverificationService, private _route: Router) {

    }
    private subscription: Subscription[] = [];
    ngOnInit() {
        this.subscription.push(this._managerReverificationService.TemplateMaster.subscribe((res: any) => {
            this._templatemasters = res;
            this.GetTemplateName(this._viewReverificationData.TemplateID);
        }));
        this.subscription.push(this._managerReverificationService.ManagerReverificationData.subscribe((res: ManagerReverificationdetailsModel) => {
            this._viewReverificationData = res;
        }));
        this._managerReverificationService.GetRowdata();
        this._managerReverificationService.GetManagerReverificationTemplate();
    }
    CloseReverification() {
        this._route.navigate(['view/mreverification']);
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
