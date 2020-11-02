import { Component, OnDestroy, AfterViewInit, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { TrimspacePipe } from '@mts-pipe';
import { environment } from 'src/environments/environment';
import { AppSettings } from '@mts-app-setting';
import { Subscription } from 'rxjs';
import { ReverificationMappingModel } from 'src/app/modules/reverification/models/reverification-mapping.model';
import { TemplateField } from 'src/app/modules/reverification/models/template-fields.model';
import { ManagerReverificationService } from '../../services/manager-reverification.service';

export abstract class ReverificationTemplate  {
    separator = '#,.';
}

@Component({
    selector: 'mts-manager-voe-letter',
    templateUrl: 'VOELetter.html'
})
export class ManagerVOELetterComponent implements OnInit, OnDestroy {
    _mappingTemplate: ReverificationMappingModel = new ReverificationMappingModel();
    Fields: TemplateField = new TemplateField();

     ImageURL: any ;
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

     runOnlyOnce = false;
     reverificationGuid: any = [];
     mentionDocFields: any = [];
     mentionDocTypes: any = [];

    constructor(private trim: TrimspacePipe, private sanitizer: DomSanitizer, private _managerReverifyService: ManagerReverificationService) { }
    private subscription: Subscription[] = [];
    selectedItem(docName) { }
    ngOnInit() {
        this.Fields = this._managerReverifyService.getFieldsForTemplate();
        this.mentionDocFields = this._managerReverifyService.getAssignedDocFields();
        this.mentionDocTypes = this._managerReverifyService.getAssignedDocTypeNames();
        this._mappingTemplate = this._managerReverifyService.GetMappingTemplateValues();
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
