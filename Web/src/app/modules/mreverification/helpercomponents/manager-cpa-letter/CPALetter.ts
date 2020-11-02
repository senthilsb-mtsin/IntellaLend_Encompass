import { Component, ViewChild, OnDestroy, AfterViewInit, OnInit } from '@angular/core';
import { TrimspacePipe } from '@mts-pipe';
import { DomSanitizer } from '@angular/platform-browser';
import { AppSettings } from '@mts-app-setting';
import { environment } from 'src/environments/environment';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { TemplateField } from 'src/app/modules/reverification/models/template-fields.model';
import { ManagerReverificationService } from '../../services/manager-reverification.service';
import { ReverificationMappingModel } from 'src/app/modules/reverification/models/reverification-mapping.model';
export abstract class ReverificationTemplate {
    separator = '#,.';
}

@Component({
    selector: 'mts-manager-cpa-letter',
    templateUrl: 'CPALetter.html'

})
export class ManagerCPALetterComponent implements OnInit, OnDestroy {
    @ViewChild('strContainer') _strContainer;
    _mappingTemplate: ReverificationMappingModel = new ReverificationMappingModel();
    Fields: TemplateField = new TemplateField();

    ImageURL: any;
    mentionConfig = {
        mentions: [
            {
                items: [],
                triggerChar: '#'
            },
            {
                items: [],
                triggerChar: '.'
            }
        ]
    };
    separator = '#,.';
    showReverifyMention = false;
    reverificationGuid: any = [];
    mentionDocFields: any = [];
    mentionDocTypes: any = [];
    TemplateFieldValue: any = [];
    TemplateFields: any = [];
    runOnlyOnce = false;

    constructor(private trim: TrimspacePipe, private sanitizer: DomSanitizer, private _managerreverifyService: ManagerReverificationService) { }
    private subscription: Subscription[] = [];
    selectedItem(docName) { }
    ngOnInit() {
        this.Fields = this._managerreverifyService.getFieldsForTemplate();
        this.mentionDocFields = this._managerreverifyService.getAssignedDocFields();
        this.mentionDocTypes = this._managerreverifyService.getAssignedDocTypeNames();
        this._mappingTemplate = this._managerreverifyService.GetMappingTemplateValues();
        this.mentionConfig.mentions[0].items = this.mentionDocTypes;
        this.mentionConfig.mentions[1].items = this.mentionDocFields;
        this.showReverifyMention = true;
        this.ImageURL = this.sanitizer.bypassSecurityTrustUrl(environment.apiURL + 'Image/GetReverificationImage/'  + AppSettings.TenantSchema + '/' + 1 + '/' + this._mappingTemplate.LogoGuid);
    }

    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
