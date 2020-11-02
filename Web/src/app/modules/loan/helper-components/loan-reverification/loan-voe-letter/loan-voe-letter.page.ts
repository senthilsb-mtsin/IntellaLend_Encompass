import { Component, OnDestroy, AfterViewInit, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { TrimspacePipe } from '@mts-pipe';
import { environment } from 'src/environments/environment';
import { AppSettings } from '@mts-app-setting';
import { Subscription } from 'rxjs';
import { LoanTemplateField } from '../../../models/loan-template-fields.model';
import { LoanReverificationMappingModel } from '../../../models/loan-reverification-mapping.model';
import { LoanInfoService } from '../../../services/loan-info.service';
export abstract class ReverificationTemplate {
    separator = '#,.';
}

@Component({
    selector: 'mts-loan-voe-letter',
    templateUrl: 'loan-voe-letter.page.html'
})
export class LoanVOELetterComponent implements OnDestroy, OnInit {

    ImageURL: any;
    mentionConfig = {
        mentions: [
            {
                items: [],
                triggerChar: '#'
            },
            {
                items: [],
                triggerChar: '.',
                labelKey: 'DocFieldName'
            }
        ]
    };
    Fields: LoanTemplateField = new LoanTemplateField();
    separator = '#,.';
    showReverifyMention = false;

    runOnlyOnce = false;
    reverificationGuid: any = [];
    mentionDocFields: any = [];
    mentionDocTypes: any = [];

    constructor(private sanitizer: DomSanitizer, private _loanInfoService: LoanInfoService) { }
    private subscription: Subscription[] = [];
    selectedItem(docName) { }
    ngOnInit() {
        this.Fields = this._loanInfoService.GetReverificationFields();

        this.subscription.push(this._loanInfoService._mappingTemplate$.subscribe((res: any) => {
            this.ImageURL = this.sanitizer.bypassSecurityTrustUrl(environment.apiURL + 'Image/GetReverificationImage/' + AppSettings.TenantSchema + '/' + 1 + '/' + res);
        }));
        this.subscription.push(this._loanInfoService.mentionDropOptions$.subscribe((res: { mentionDocTypes: any, mentionDocFields: any }) => {
            this.showReverifyMention = false;
            this.mentionConfig.mentions[0].items = res.mentionDocTypes;
            this.mentionConfig.mentions[1].items = res.mentionDocFields;
            this.mentionDocFields = res.mentionDocFields;
            this.mentionDocTypes = res.mentionDocTypes;
            this.showReverifyMention = true;
        }));
    }
    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
