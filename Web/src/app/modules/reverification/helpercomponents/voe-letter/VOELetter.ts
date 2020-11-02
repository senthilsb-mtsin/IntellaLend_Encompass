import { Component, OnDestroy, AfterViewInit, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { TrimspacePipe } from '@mts-pipe';
import { environment } from 'src/environments/environment';
import { AppSettings } from '@mts-app-setting';
import { Subscription } from 'rxjs';
import { ReverificationService } from '../../services/reverification.service';
import { TemplateField } from '../../models/template-fields.model';
import { ReverificationMappingModel } from '../../models/reverification-mapping.model';
export abstract class ReverificationTemplate {
    separator = '#,.';
}

@Component({
    selector: 'mts-voe-letter',
    templateUrl: 'VOELetter.html'
})
export class VOELetterComponent implements OnDestroy, OnInit {

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
    Fields: TemplateField = new TemplateField();
    separator = '#,.';
    showReverifyMention = false;

    runOnlyOnce = false;
    reverificationGuid: any = [];
    mentionDocFields: any = [];
    mentionDocTypes: any = [];
    _mappingTemplate: ReverificationMappingModel = new ReverificationMappingModel();

    constructor(private trim: TrimspacePipe, private sanitizer: DomSanitizer, private _reverifyService: ReverificationService) { }
    private subscription: Subscription[] = [];
    selectedItem(docName) { }
    ngOnInit() {
        this.Fields = this._reverifyService.getFieldsForTemplate();
        this.mentionDocFields = this._reverifyService.getAssignedDocFields();
        this.mentionDocTypes = this._reverifyService.getAssignedDocTypeNames();
        this._mappingTemplate = this._reverifyService.GetMappingTemplateValues();
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
