import { Component, ElementRef, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { ReverificationService } from '../../services/reverification.service';
import { ReverificationdetailsModel } from '../../models/reverification-details.model';
import { Subscription } from 'rxjs';
import { ReverificationStepModel } from '../../models/reverification-step.model';
import { Router } from '@angular/router';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { NotificationService } from '@mts-notification';
import { FileUploader, FileItem, FileUploaderOptions } from 'ng2-file-upload';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ReverificationMappingModel } from '../../models/reverification-mapping.model';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CheckFileExtension } from '@mts-pipe';
import { environment } from 'src/environments/environment';
import { AppSettings } from '@mts-app-setting';
const jwtHelper = new JwtHelperService();

@Component({
    selector: 'mts-editreverification',
    templateUrl: 'editreverification.page.html',
    styleUrls: ['editreverification.page.css'],
})
export class EditReverificationComponent implements OnInit, OnDestroy {
    @ViewChild('confirmModal') confirmModal: ModalDirective;
    @ViewChild('strContainer') _strContainer;
    @ViewChild('slidesDiv') _slideDiv: ElementRef;

    _editReverificationData: ReverificationdetailsModel = new ReverificationdetailsModel();
    EditReverifSteps = this._reverificationService.AddReverificationSteps;
    stepModel: ReverificationStepModel = new ReverificationStepModel(this.EditReverifSteps.Reverification, 'active', '');
    template = 'Hi this is the Client name : <input type="text" [(ngModel)]="setValue" />';
    TemplateName: any = '';
    _templatemasters: any = [];
    isPrevious = false;
    slideOneTranClass: any = 'transForm';
    ReverificationName: any = '';
    DocumentTypes: any = [];
    AssignedDocTypes: any = [];
    uploader: FileUploader;
    uploaderOptions: FileUploaderOptions;
    assignDocs: any = [];
    Fields: any = {};
    _isShowCPA = false;
    _isShowVoe = false;
    _isShowVod = false;
    _isShowGIF = false;
    _isShowOccu = false;
    isValidate = false;
    notValidate = false;
    UploadExist = false;
    uploadcomplete = false;
    promise: Subscription;
    Reverifydata: any = [];
    reverificationFileName = '';
    existingReverificationName = '';
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
    constructor(private _reverificationService: ReverificationService, private _route: Router, private _checkextension: CheckFileExtension, private _notificationService: NotificationService) {

    }
    private subscription: Subscription[] = [];
    ngOnInit() {
        this.subscription.push(this._reverificationService.TemplateMaster.subscribe((res: any) => {
            this._templatemasters = res;
            this.GetTemplateName(this._editReverificationData.TemplateID);

        }));
        this.subscription.push(this._reverificationService.setNextStep.subscribe((res: ReverificationStepModel) => {
            this.stepModel = res;
        }));
        this.subscription.push(this._reverificationService.assignedDocuments.subscribe((res: number[]) => {
            this.AssignedDocTypes = res;

        }));
        this.subscription.push(this._reverificationService.setReverificationTableData.subscribe((res: any[]) => {
            this.Reverifydata = res;
        }));

        this.subscription.push(this._reverificationService.ReverificationData.subscribe((res: ReverificationdetailsModel) => {
            this._editReverificationData = res;
            this.existingReverificationName = this._editReverificationData.ReverificationName;
        }));
        this.subscription.push(this._reverificationService.isprevious.subscribe((res: any) => {
            this.isPrevious = res;

        }));
        this.subscription.push(this._reverificationService._mappingTemplate.subscribe((res: ReverificationMappingModel) => {
            this.TemplateName = res.TemplateName;
            this.reverificationFileName = res.TemplateFileName;
        }));
        this.subscription.push(this._reverificationService.isSetMapping.subscribe((res: any) => {
            if (res === true) {
                this.ShowReverifyTemplate();
                this.confirmModal.show();
            }
        }));
        this._reverificationService.GetReverificationTemplate();
        this._reverificationService.GetRowdata();
        this.upload();
    }

    upload() {
        this.uploader = new FileUploader(this.fileUploadOptions);
        this.uploader.onBeforeUploadItem = (item: FileItem) => {
            const fileExtensionStatus = this._checkextension.transform(item.file.name);
            this.reverificationFileName = item.file.name;
            item.withCredentials = false;
            this.uploader.options.headers[1].value = item.file.name;
            this.uploader.options.headers[2].value = this._editReverificationData.ReverificationID.toString();
            item.upload();

        };
        this.uploader.onCompleteItem = (item: any, response: any, status: any, headers: any) => {
            let responsePath;
            if (response !== '') {
                const resParse = response = JSON.parse(response);
                if (resParse.token !== null) {
                    localStorage.setItem('id_token', resParse.token);
                    responsePath = jwtHelper.decodeToken(resParse.data)['data'];
                    if (responsePath.Result !== '') {
                        this.uploadcomplete = true;
                        this.SaveReverifyDocMapping();
                        this.UploadExist = false;
                        this._notificationService.showSuccess('File has been Uploaded Successfully');
                        this.uploader.clearQueue();
                    } else {

                        this._notificationService.showError('File Upload Failed');
                        this.uploader.queue[0].isUploaded = false;
                        this.uploader.clearQueue();
                    }
                }

            } else {
                this.uploader.clearQueue();
            }
        };
    }
    EditReverificationSubmit() {
        const input = { ReverificationName: this._editReverificationData.ReverificationName, ReverificationID: this._editReverificationData.ReverificationID, Active: this._editReverificationData.Active, MappingID: this._editReverificationData.MappingID };
        this.isValidate = this.IsValidate();
        if (this.isValidate) {
            this._reverificationService.EditReverificationSubmit(input, this._editReverificationData.LoanTypeID, this._editReverificationData.TemplateID);
            this._slideDiv.nativeElement.scrollLeft = this._slideDiv.nativeElement.scrollLeft + this._slideDiv.nativeElement.offsetWidth;
        } else {
            this._notificationService.showError('Reverification Name Already Exists');
        }
    }
    CheckReverificationExistForEdit() {
        const input = { ReverificationName: this._editReverificationData.ReverificationName, ReverificationID: this._editReverificationData.ReverificationID, Active: this._editReverificationData.Active, MappingID: this._editReverificationData.MappingID };

        if (this.existingReverificationName === this._editReverificationData.ReverificationName) {
            this._reverificationService.EditReverificationSubmit(input, this._editReverificationData.LoanTypeID, this._editReverificationData.TemplateID);
            // const req = { LoanTypeID: this._editReverificationData.LoanTypeID, ReverificationID: input.ReverificationID };
            // this._reverificationService.GetLoanDocuments(req);
        } else {
            this._reverificationService.CheckReverificationExistForEdit(input, this._editReverificationData.LoanTypeID, this._editReverificationData.TemplateID);
        }

        this._slideDiv.nativeElement.scrollLeft = this._slideDiv.nativeElement.scrollLeft + this._slideDiv.nativeElement.offsetWidth;

    }
    GotoPrevious() {
        this._route.navigate(['view/reverification']);

    }

    IsValidate(): boolean {
        const _reveriList = [];
        this._reverificationService.GetReverification();
        const obj = this._reverificationService.getReverificationList();
        if (obj.length > 0) {
            obj.forEach(element => {
                if (element.ReverificationName === this._editReverificationData.ReverificationName) {
                    _reveriList.push(element);
                }
            });
        }
        if (_reveriList.length > 1) {
            return false;
        } else if (_reveriList.length === 0 || _reveriList.length === 1 && !this.isPrevious) {
            return true;
        }
    }

    GetTemplateName(id) {
        const tempMas = this._templatemasters.filter(x => x.TemplateID === id);
        if (tempMas.length <= 0) { return ''; }
        this.TemplateName = tempMas[0].TemplateName;
    }
    singleFileUpload() {
        this.uploader.uploadAll();
    }
    GotoNextStep() {
        if (this.stepModel.stepID === this.EditReverifSteps.Reverification) {
            this.CheckReverificationExistForEdit();

        } else if (this.stepModel.stepID === this.EditReverifSteps.AssignDocTypes) {
            this.isPrevious = true;
            if (this.UploadExist) {
                this.singleFileUpload();
            } else {

                this.SaveReverifyDocMapping();

            }
        }
    }
    EditSetNextStep() {

        this.isValidate = this.IsValidate();
        if (!this.isPrevious) {
            if (this.isValidate) {
                this._reverificationService.EditSetNextStep();

            } else {
                this._notificationService.showError('Reverification Name Already Exists');
            }

        }
    }
    SaveReverifyDocMapping() {
        const inputReq = { ReverificationID: this._editReverificationData.ReverificationID, DocTypeIDs: this.AssignedDocTypes.filter((v, i, a) => a.indexOf(v) === i).slice() };
        this._reverificationService.SaveReverifyDocMapping(inputReq);

    }
    saveTemplate() {
        if (this._editReverificationData.MappingID !== 0) {
            const jData = JSON.stringify(this._strContainer['Fields']);
            const input = { MappingID: this._editReverificationData.MappingID, TemplateFieldJson: jData };
            this.promise = this._reverificationService.SaveTemplate(input);

        }
    }

    SetPrevious() {
        if (this.stepModel.stepID === this.EditReverifSteps.AssignDocTypes) {
            this.isPrevious = false;
        }
        this.notValidate = true;

        this._reverificationService.SetStepID(this.stepModel.stepID);
        this._slideDiv.nativeElement.scrollLeft = this._slideDiv.nativeElement.scrollLeft - this._slideDiv.nativeElement.offsetWidth;

    }

    CloseReverification() {
        this._reverificationService.CloseReverification();

    }
    fileEvent(e: any) {
        const fileSelected: File = e.target.files[0];
        const fileExtensionStatus = fileSelected.name;
        if (fileExtensionStatus.match(/\.((png|jpg|jpeg)?(m|x|))$/i)) {
            this.reverificationFileName = fileSelected.name;
            this.UploadExist = true;

        } else {
            this.uploader.clearQueue();
            this._notificationService.showError('File Extension not Supported, Please Upload Image file');
        }

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
