import { TrimspacePipe } from '@mts-pipe';
import { DomSanitizer } from '@angular/platform-browser';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AppSettings } from '@mts-app-setting';
import { Component, OnDestroy, AfterViewInit, OnInit } from '@angular/core';
import { ReverificationService } from '../../services/reverification.service';
import { TemplateField } from '../../models/template-fields.model';
import { ReverificationMappingModel } from '../../models/reverification-mapping.model';
export abstract class ReverificationTemplate {
    separator = '#,.';
}

@Component({
    selector: 'mts-occupancy-letter',
    templateUrl: 'OccupancyLetter.html'
})
export class OccupancyLetterComponent implements OnInit, OnDestroy {

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
    reverificationGuid: any = [];
    mentionDocFields: any = [];
    mentionDocTypes: any = [];
    _mappingTemplate: ReverificationMappingModel = new ReverificationMappingModel();

    runOnlyOnce = false;

    constructor(private trim: TrimspacePipe, private sanitizer: DomSanitizer, private _reverifyService: ReverificationService) { }
    private subscription: Subscription[] = [];

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
    selectedItem(docName) { }

    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
