import { Component, ElementRef, ViewChild, OnDestroy, OnInit } from '@angular/core';
import { ReverificationService } from '../../services/reverification.service';
import { ReverificationStepModel } from '../../models/reverification-step.model';
import { Subscription } from 'rxjs';
import { CommonService } from 'src/app/shared/common';
import { Router } from '@angular/router';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { NotificationService } from '@mts-notification';
import { FileUploader, FileUploaderOptions, FileItem } from 'ng2-file-upload';
import { CheckFileExtension } from '@mts-pipe';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { HttpClient } from '@angular/common/http';
import { ReverificationMappingModel } from '../../models/reverification-mapping.model';
import { SelectComponent } from '@mts-select2';
import { AssignDocTypeRequestModel } from '../../models/assign-doc-request.model';
import { environment } from 'src/environments/environment';
import { AppSettings } from '@mts-app-setting';

const jwtHelper = new JwtHelperService();

@Component({
    selector: 'mts-addreverification',
    templateUrl: 'addreverification.page.html',
    styleUrls: ['addreverification.page.css'],
})
export class AddReverificationComponent implements OnInit, OnDestroy {
    @ViewChild('confirmModal') confirmModal: ModalDirective;
    @ViewChild('slidesDiv') _slideDiv: ElementRef;
    @ViewChild('loanSelect') select: SelectComponent;
    @ViewChild('strContainer') _strContainer;
    AddReverifSteps = this._reverifService.AddReverificationSteps;
    stepModel: ReverificationStepModel = new ReverificationStepModel(this.AddReverifSteps.Reverification, 'active', '');
    uploader: FileUploader;
    uploaderOptions: FileUploaderOptions;
    slideOneTranClass: any = 'transForm';
    ReverificationFileName = '';
    isPrevious = false;
    LoanTypeMaster: any[] = [];
    ReverificationName: any = '';
    TemplateMaster: any = [];
    TemplateID = 0;
    LoanTypeID = 0;
    _reverifyData: any = [];
    AssignedDocTypes: any = [];
    DocumentTypes: any = [];
    _reverificationID = 0;
    Fields: any = {};
    TemplateName = '';
    MappingID: any;
    promise: Subscription;
    TemplateFileName: any = [];
    template = 'Hi this is the Client name : <input type="text" [(ngModel)]="setValue" />';
    uploadcomplete = false;
    uploadExist = false;
    _isShowCPA = false;
    _isShowVoe = false;
    _isShowVod = false;
    _isShowGIF = false;
    _isShowOccu = false;
    isValidate = false;
    notValidate = false;
    assignDocs: any[] = [];
    URL: string = environment.apiURL + 'IntellaLend/ReverificationFileUploader';
    fileUploadOptions: FileUploaderOptions = {
        url: this.URL,
        authToken: 'Bearer ' + localStorage.getItem('id_token'),
        authTokenHeader: 'Authorization',
        headers: [
            { name: 'TableSchema', value: AppSettings.TenantSchema },
            { name: 'UploadFileName', value: '' },
            { name: 'ReverificationID', value: '' },
        ]
    };
    constructor(private _reverifService: ReverificationService, private http: HttpClient, private _checkFileextension: CheckFileExtension, private _commonService: CommonService, private _route: Router, private _notifyService: NotificationService) {
        this.RefreshLoanTypes();
    }
    private subscription: Subscription[] = [];
    ngOnInit() {
        this._commonService.GetSystemLoanTypes();
        this.subscription.push(this._commonService.SystemLoanTypeItems.subscribe((res: any[]) => {
            this.LoanTypeMaster = res;

        }));
        this._reverifService.GetReverificationTemplate();
        this._reverifService.getAssignedDocTypes();
        this.subscription.push(this._reverifService.TemplateMaster.subscribe((res: any) => {
            this.TemplateMaster = res;

        }));
        this.subscription.push(this._reverifService.assignedDocuments.subscribe((res: number[]) => {
            this.AssignedDocTypes = [...res];
        }));
        this.subscription.push(this._reverifService._mappingTemplate.subscribe((res: ReverificationMappingModel) => {
            this.TemplateFileName = res.TemplateFileName;
            this.TemplateName = res.TemplateName;
        }));
        this.subscription.push(this._reverifService.setNextStep.subscribe((res: ReverificationStepModel) => {
            this.stepModel = res;
        }));
        this.subscription.push(this._reverifService.setReverificationTableData.subscribe((res: any) => {
            this._reverifyData = res;
        }));
        this.subscription.push(this._reverifService.isprevious.subscribe((res: any) => {
            this.isPrevious = res;
        }));
        this.subscription.push(this._reverifService._addReverifyData.subscribe((res: any) => {
            this._reverificationID = res.ReverificationID,
                this.MappingID = res.MappingID;
         }));

        this.subscription.push(this._reverifService.isSetMapping.subscribe((res: any) => {
            if (res === true) {
                this.confirmModal.show();
                this.ShowReverifyTemplate();

            }
        }));
        this.FileUpload();

    }
    singleFileUpload() {
        this.uploader.uploadAll();
    }

    saveTemplate() {
        if (this._reverifyData.MappingID !== 0) {
            const jData = JSON.stringify(this._strContainer['Fields']);
            const input = { MappingID: this.MappingID, TemplateFieldJson: jData };
            this.promise = this._reverifService.SaveTemplate(input);

        }
    }
    FileUpload() {
        this.uploader = new FileUploader(this.fileUploadOptions);
        this.uploader.onBeforeUploadItem = (item: FileItem) => {
            const fileExtensionStatus = this._checkFileextension.transform(item.file.name);
            this.ReverificationFileName = item.file.name;
            item.withCredentials = false;
            this.uploader.options.headers[1].value = item.file.name;
            this.uploader.options.headers[2].value = this._reverificationID.toString();
            item.upload();

        };

        this.uploader.onCompleteItem = (item: any, response: any, status: any, headers: any) => {
            let responsePath;
            if (response !== '') {
                const resParse = response = JSON.parse(response);
                responsePath = jwtHelper.decodeToken(resParse.data)['data'];

                if (responsePath.Result !== '') {
                    this.SaveReverifyDocMapping();
                    this._notifyService.showSuccess('File has been Uploaded Successfully');
                    this.uploadExist = false;
                    this.uploader.clearQueue();
                    this.uploadcomplete = true;
                } else {
                    this._notifyService.showError('File Upload Failed');
                    this.uploader.queue[0].isUploaded = false;
                    this.uploader.clearQueue();
                }

            } else {
                this.uploader.clearQueue();
            }
        };
    }
    AddReverificationSubmit() {
        const req = { LoanTypeID: this.LoanTypeID, ReverificationName: this.ReverificationName, TemplateID: this.TemplateID };
        this.isValidate = this.IsValidate();
        if (!this.isPrevious) {
            if (this.isValidate) {
                if (this.ReverificationFileName !== '' && this.ReverificationFileName !== undefined) {
                    this._reverifService.AddReverificationSubmit(req);
                } else {
                    this._notifyService.showError('Template Logo Required');
                }
                this._slideDiv.nativeElement.scrollLeft = this._slideDiv.nativeElement.scrollLeft + this._slideDiv.nativeElement.offsetWidth;
            } else {
                this._notifyService.showError('Reverification Name Already Exists');
            }
        }
    }

    IsValidate(): boolean {
        const _reveriList = [];
        this._reverifService.GetReverification();
        const obj = this._reverifService.getReverificationList();
        if (obj.length > 0) {
            obj.forEach(element => {
                if (element.ReverificationName === this.ReverificationName) {
                    _reveriList.push(element);
                }
            });
        }
        if (_reveriList.length > 0 && !this.notValidate) {
            return false;
        } else if (_reveriList.length === 0 && !this.isPrevious) {
            return true;
        } else if (_reveriList.length === 1 && this.notValidate) {
            return true;
        }
    }

    SaveReverifyDocMapping() {
        const req = new AssignDocTypeRequestModel(this._reverificationID, this.AssignedDocTypes.filter((v, i, a) => a.indexOf(v) === i).slice());
        this._reverifService.SaveReverifyDocMapping(req);

    }

    fileEvent(e: any) {
        const fileSelected: File = e.target.files[0];
        const fileExtensionStatus = fileSelected.name;
        if (fileExtensionStatus.match(/\.((png|jpg|jpeg)?(m|x|))$/i)) {
            this.ReverificationFileName = fileSelected.name;
            this.uploadExist = true;
        } else {
            this.uploader.clearQueue();
            this._notifyService.showError('File Extension not Supported, Please Upload Image file');
        }
    }
    GotoNextStep() {
        if (this.stepModel.stepID === this.AddReverifSteps.Reverification) {
            if (this.notValidate) {
                this.CheckreverificationExistForEdit();
            } else {
                this.CheckreverificationAvailable();
            }

        } else if (this.stepModel.stepID === this.AddReverifSteps.AssignDocTypes) {
            this.isPrevious = true;
            if (this.uploadExist) {
                this.singleFileUpload();
            } else {
                this.SaveReverifyDocMapping();
            }
        }

    }
    CheckreverificationExistForEdit() {
        const req = { ReverificationName: this.ReverificationName, ReverificationID: this._reverificationID, Active: true, MappingID: this.MappingID};
        this._reverifService.CheckReverificationExistForEdit(req, this.LoanTypeID, this.TemplateID);
        this._slideDiv.nativeElement.scrollLeft = this._slideDiv.nativeElement.scrollLeft + this._slideDiv.nativeElement.offsetWidth;

    }
    CheckreverificationAvailable() {
        const req = { LoanTypeID: this.LoanTypeID, ReverificationName: this.ReverificationName, TemplateID: this.TemplateID };
        if (this.ReverificationFileName !== '' && this.ReverificationFileName !== undefined && !this.notValidate) {
            this._reverifService.CheckreverificationAvailable(req);
        } else {
            this._notifyService.showError('Template Logo Required');
        }
        this._slideDiv.nativeElement.scrollLeft = this._slideDiv.nativeElement.scrollLeft + this._slideDiv.nativeElement.offsetWidth;

    }
    GotoPrevious() {
        this._route.navigate(['view/reverification']);

    }
    SetPrevious() {
        if (this.stepModel.stepID === this.AddReverifSteps.AssignDocTypes) {
            this.isPrevious = false;
        }
        this.notValidate = true;

        this._reverifService.SetStepID(this.stepModel.stepID);
        this._slideDiv.nativeElement.scrollLeft = this._slideDiv.nativeElement.scrollLeft - this._slideDiv.nativeElement.offsetWidth;

    }
    RefreshLoanTypes() {
        this._commonService.GetSystemLoanTypes();
        this.subscription.push(this._commonService.SystemLoanTypeItems.subscribe((res: any[]) => {
            this.LoanTypeMaster = res;
        }));
    }
    ShowReverifyTemplate() {
        this._isShowVod = false;
        this._isShowOccu = false;
        this._isShowVoe = false;
        this._isShowGIF = false;
        this._isShowCPA = false;
        if (this.TemplateName === 'CPA Letter Template') {
            this._isShowCPA = true;
        } else if (this.TemplateName === 'Gift Letter Template') {
            this._isShowGIF = true;
        } else if (this.TemplateName === 'Occupancy Template') {
            this._isShowOccu = true;
        } else if (this.TemplateName === 'VOD Template') {
            this._isShowVod = true;
        } else if (this.TemplateName === 'VOE Template') {
            this._isShowVoe = true;
        }
    }
    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }

}
