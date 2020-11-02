import { Component, OnInit, OnDestroy } from '@angular/core';
import { LoanInfoService } from '../../services/loan-info.service';
import { Subscription } from 'rxjs';
import { SessionHelper } from '@mts-app-session';
import { CommonService } from 'src/app/shared/common';
import { convertDateTimewithOnlyTime } from '@mts-functions/convert-datetime.function';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { NotificationService } from '@mts-notification';
import { LoanHeaders } from '../../models/loan-header.model';
import { AppSettings } from '@mts-app-setting';
import { ILoanNote } from '../../models/loan-details-sub.model';

@Component({
    selector: 'mts-notes-tab',
    templateUrl: 'notes-tab.page.html',
    styleUrls: ['notes-tab.page.scss']
})
export class NotesTabComponent implements OnInit, OnDestroy {

    LoanNotes: ILoanNote[] = [];
    showMentionPlus = true;
    notesTxtArea = '';
    mentionConfig: any = {
        mentions: [
            {
                items: [],
                triggerChar: '@'
            },
            {
                items: [],
                triggerChar: '$'
            },
            {
                items: [],
                triggerChar: ':'
            },
        ]
    };
    UserID: any = SessionHelper.UserDetails.UserID;

    constructor(
        private _loanInfoService: LoanInfoService,
        private _commonService: CommonService,
        private _notificationService: NotificationService
    ) { }

    private _subscriptions: Subscription[] = [];
    private _loanHeader: LoanHeaders = new LoanHeaders();

    ngOnInit() {
        this._loanHeader = this._loanInfoService.GetLoanHeader();

        this._subscriptions.push(this._loanInfoService.LoanNotes$.subscribe((res: ILoanNote[]) => {
            this.LoanNotes = res;
        }));

        this._subscriptions.push(this._commonService.SystemDocumentTypeMaster.subscribe((documentTypesData: any[]) => {
            const documentTypesNotes = [];
            documentTypesData.forEach(element => {
                documentTypesNotes.push(element.DisplayName);
            });
            this.mentionConfig.mentions[0].items = this.mentionConfig.mentions[0].items.concat(documentTypesNotes.slice());
            this.showMentionPlus = this.mentionConfig.mentions.length === 1;
            this._commonService.GetSystemDocumentFieldList();
        }));

        this._subscriptions.push(this._commonService.SystemDocumentTypeFieldMaster.subscribe((documentFieldsData: any[]) => {
            const fieldAttributes = [];
            documentFieldsData.forEach(element => {
                fieldAttributes.push(element.DisplayName);
            });
            this.mentionConfig.mentions[1].items = this.mentionConfig.mentions[1].items.concat(fieldAttributes.slice());
            this.showMentionPlus = this.mentionConfig.mentions.length === 2;
            this._loanInfoService.GetUserMaster();
        }));

        this._subscriptions.push(this._loanInfoService.usernameNotes$.subscribe((res: any[]) => {
            this.mentionConfig.mentions[2].items = this.mentionConfig.mentions[2].items.concat(res);
            this.showMentionPlus = this.mentionConfig.mentions.length === 3;
        }));

        this._subscriptions.push(this._loanInfoService.notesTxtArea$.subscribe((res: string) => {
            this.notesTxtArea = res;
        }));

        this._commonService.GetSystemDocumentTypes();
        this._loanInfoService.GetLoanNotes();
    }

    addClasses(userid: string) {
        return SessionHelper.UserDetails.UserID === userid ? { receiver: true } : { sender: true };
    }

    updateLoannotes() {
        const timestampnow = new Date().getTime();
        const currenttimestamp = convertDateTimewithOnlyTime(timestampnow);
        if (isTruthy(this.notesTxtArea)) {
            this.LoanNotes.push({ UserId: SessionHelper.UserDetails.UserID, UserName: SessionHelper.UserDetails.LastName + ' ' + SessionHelper.UserDetails.FirstName, Timestamp: currenttimestamp, Note: this.notesTxtArea, Custom1: '', Custom2: '' });
            const req = { TableSchema: AppSettings.TenantSchema, LoanID: this._loanHeader.LoanID, Notes: JSON.stringify(this.LoanNotes) };
            this._loanInfoService.UpdateLoanNotes(req);
        } else { this._notificationService.showError('Enter Notes'); }
    }

    ngOnDestroy(): void {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
