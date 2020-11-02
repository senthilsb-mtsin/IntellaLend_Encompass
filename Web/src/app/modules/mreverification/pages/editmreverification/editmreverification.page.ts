import { OnInit, OnDestroy, ElementRef, ViewChild, Component, AfterViewInit } from '@angular/core';
import { NotificationService } from '@mts-notification';
import { ManagerReverificationComponent } from '../mreverification/mreverification.page';
import { ManagerReverificationService } from '../../services/manager-reverification.service';
import { Subscription, Subject } from 'rxjs';
import { ManagerReverificationdetailsModel } from '../../models/manager-reverification-details.model';
import { ReverificationStepModel } from 'src/app/modules/reverification/models/reverification-step.model';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUploaderOptions, FileUploader, FileItem } from 'ng2-file-upload';
import { AppSettings } from '@mts-app-setting';
import { CheckFileExtension } from '@mts-pipe';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ReverificationService } from 'src/app/modules/reverification/services/reverification.service';
import { ReverificationMappingModel } from 'src/app/modules/reverification/models/reverification-mapping.model';
import { EditDocumentModel } from '../../models/edit-doc-submit.model';
import { environment } from 'src/environments/environment';
import { RequestTemplateModel } from '../../models/request-template.model';
import { debug } from 'console';
const jwtHelper = new JwtHelperService();
@Component({
    selector: 'mts-editmreverification',
    templateUrl: 'editmreverification.page.html',
    styleUrls: ['editmreverification.page.css'],
})
export class ManagerEditReverificationComponent implements OnDestroy, OnInit {
    @ViewChild('confirmationModal') confirmModal: ModalDirective;
    @ViewChild('strContainer') _strContainer;
    @ViewChild('slidesDiv') _slideDiv: ElementRef;
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
    Fields: any = {};
    _isShowCPA = false;
    _isShowVoe = false;
    _isShowVod = false;
    _isShowGIF = false;
    _isShowOccu = false;
    notValidate = false;
    isValidate = false;
    uploadcomplete = false;
    UploadExist = false;
    promise: Subscription;
    Reverifydata: any = [];
    reVerificationFileName = '';
    AuthorityLabelSingular = AppSettings.AuthorityLabelSingular;
    EditManagerReverifSteps = this._managerReverificationService.AddReverificationSteps;
    _editManagerReverificationData: ManagerReverificationdetailsModel = new ManagerReverificationdetailsModel();
    stepModel: ReverificationStepModel = new ReverificationStepModel(this.EditManagerReverifSteps.Reverification, 'active', '');
    _editReverifyRowData: EditDocumentModel = new EditDocumentModel();
    URL: string = environment.apiURL + 'IntellaLend/CustReverificationFileUploader';
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
    constructor(private _notificationService: NotificationService,
        private _managerReverificationService: ManagerReverificationService,
        private _route: Router,
        private _checkextension: CheckFileExtension,
        private _reverificationService: ReverificationService) {

    }
    private subscription: Subscription[] = [];
    ngOnInit() {
        this.subscription.push(this._managerReverificationService.TemplateMaster.subscribe((res: any) => {
            this._templatemasters = res;
            this.GetTemplateName(this._editManagerReverificationData.TemplateID);

        }));
        this.subscription.push(this._managerReverificationService.setNextStep.subscribe((res: ReverificationStepModel) => {
            this.stepModel = res;
        }));
        this.subscription.push(this._managerReverificationService.ManagerReverificationData.subscribe((res: ManagerReverificationdetailsModel) => {
            this._editManagerReverificationData = res;
        }));
        this.subscription.push(this._managerReverificationService.setManagerReverificationTableData.subscribe((res: any[]) => {
            this.Reverifydata = res;
        }));
        this.subscription.push(this._managerReverificationService.assignedDocuments.subscribe((res: number[]) => {
            this.AssignedDocTypes = res;
        }));
        this.subscription.push(this._managerReverificationService.ManagerReverificationData.subscribe((res: ManagerReverificationdetailsModel) => {
            this._editManagerReverificationData = res;

        }));
        this.subscription.push(this._managerReverificationService.isprevious.subscribe((res: any) => {
            this.isPrevious = res;

        }));
        this.subscription.push(this._managerReverificationService._mappingTemplate.subscribe((res: ReverificationMappingModel) => {
            this.TemplateName = res.TemplateName;
        }));
        this.subscription.push(this._managerReverificationService._editReverifyData.subscribe((res: EditDocumentModel) => {
            this._editReverifyRowData = res;

        }));
        this.subscription.push(this._managerReverificationService.isSetMapping.subscribe((res: any) => {
            if (res === true) {
                this.ShowReverifyTemplate();
                this.confirmModal.show();
            }
        }));
        this._managerReverificationService.GetRowdata();
        this._managerReverificationService.GetManagerReverificationTemplate();
        this.FileUpload();
    }

    fileEvent(e: any) {
        const fileSelected: File = e.target.files[0];
        const fileExtensionStatus = fileSelected.name;
        if (fileExtensionStatus.match(/\.((png|jpg|jpeg)?(m|x|))$/i)) {
            this.reVerificationFileName = fileSelected.name;
            this.UploadExist = true;

        } else {
            this.uploader.clearQueue();
            this._notificationService.showError('File Extension not Supported, Please Upload Image file');
        }

    }
    saveTemplate() {
        if (this._editManagerReverificationData.MappingID !== 0) {
            const jData = JSON.stringify(this._strContainer['Fields']);
            const input: RequestTemplateModel = new RequestTemplateModel(AppSettings.TenantSchema, this._editManagerReverificationData.MappingID, jData);
            this.promise = this._managerReverificationService.SaveTemplate(input);

        }
    }
    GetTemplateName(id) {
        const tempMas = this._templatemasters.filter(x => x.TemplateID === id);
        if (tempMas.length <= 0) { return ''; }
        this.TemplateName = tempMas[0].TemplateName;
    }
    GotoPrevious() {
        this._route.navigate(['view/mreverification']);

    }
    CloseReverification() {
        this._managerReverificationService.CloseReverification();

    }

    EditReverificationSubmit() {
        const req = { TableSchema: AppSettings.TenantSchema, ReverificationName: this._editManagerReverificationData.ReverificationName, ReverificationID: this._editManagerReverificationData.ReverificationID, Active: this._editManagerReverificationData.Active, MappingID: this._editManagerReverificationData.MappingID, CustomerID: this._editManagerReverificationData.CustomerID };
        this.isValidate = this.IsValidate();
        if (this.isValidate) {
            this._managerReverificationService.EditReverificationSubmit(req, this._editManagerReverificationData.TemplateID, this._editManagerReverificationData.LoanTypeID);
            this._slideDiv.nativeElement.scrollLeft = this._slideDiv.nativeElement.scrollLeft + this._slideDiv.nativeElement.offsetWidth;
        } else {
            this._notificationService.showError('Reverification Name Already Exists');
        }

    }
    CheckReverificationAvailableForEdit() {
        const req = { TableSchema: AppSettings.TenantSchema, ReverificationName: this._editManagerReverificationData.ReverificationName, ReverificationID: this._editManagerReverificationData.ReverificationID, Active: this._editManagerReverificationData.Active, MappingID: this._editManagerReverificationData.MappingID, CustomerID: this._editManagerReverificationData.CustomerID };
        this._managerReverificationService.CheckReverificationAvailableForEdit(req, this._editManagerReverificationData.TemplateID, this._editManagerReverificationData.LoanTypeID);
        this._slideDiv.nativeElement.scrollLeft = this._slideDiv.nativeElement.scrollLeft + this._slideDiv.nativeElement.offsetWidth;

    }
    GetManagerReverification() {
        const input = { TableSchema: AppSettings.TenantSchema };
        this.promise = this._managerReverificationService.GetManagerReverification(input);
    }
    IsValidate(): boolean {
        const _reveriList = [];
        this.GetManagerReverification();
        const obj = this._managerReverificationService.getReverificationList();
        if (obj.length > 0) {
            obj.forEach(element => {
                if (element.ReverificationName === this._editManagerReverificationData.ReverificationName) {
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
    SetPrevious() {
        if (this.stepModel.stepID === this.EditManagerReverifSteps.AssignDocTypes) {
            this.isPrevious = false;
        }
        this.notValidate = true;
        this._managerReverificationService.SetStepID(this.stepModel.stepID);
        this._slideDiv.nativeElement.scrollLeft = this._slideDiv.nativeElement.scrollLeft - this._slideDiv.nativeElement.offsetWidth;

    }
    GetLoanDocuments() {

        const _loandocsReq = { TableSchema: AppSettings.TenantSchema, CustomerID: this._editManagerReverificationData.CustomerID, LoanTypeID: this._editManagerReverificationData.LoanTypeID, ReverificationID: this._editManagerReverificationData.ReverificationID };
        this.promise = this._managerReverificationService.GetLoanDocuments(_loandocsReq);
    }
    GotoNextStep() {
        if (this.stepModel.stepID === this.EditManagerReverifSteps.Reverification) {
          this.CheckReverificationAvailableForEdit();

        } else if (this.stepModel.stepID === this.EditManagerReverifSteps.AssignDocTypes) {
            this.isPrevious = true;
            if (this.UploadExist) {
                this.singleFileUpload();

            } else {

                this.SaveManagerReverifyDocMapping();

            }
        }

    }
    EditSetNextStep() {
        this.isValidate = this.IsValidate();
        if (!this.isPrevious) {
            if (this.isValidate) {
                this._managerReverificationService.EditSetNextStep();

            } else {
                this._notificationService.showError('Reverification Name Already Exists');
            }

        }
    }
    FileUpload() {

        this.uploader = new FileUploader(this.fileUploadOptions);
        this.uploader.onBeforeUploadItem = (item: FileItem) => {
            const fileExtensionStatus = this._checkextension.transform(item.file.name);
            this.reVerificationFileName = item.file.name;
            item.withCredentials = false;
            this.uploader.options.headers[1].value = item.file.name;
            this.uploader.options.headers[2].value = this._editManagerReverificationData.ReverificationID.toString();
            this.uploadcomplete = true;
            item.upload();

        };
        this.uploader.onCompleteItem = (item: any, response: any, status: any, headers: any) => {

            let responsePath;
            if (response !== '') {
                const resParse = response = JSON.parse(response);
                responsePath = jwtHelper.decodeToken(resParse.data)['data'];
                if (responsePath.Result !== '') {
                    this.uploadcomplete = true;
                    this._notificationService.showSuccess('File has been Uploaded Successfully');
                    this.UploadExist = false;
                    this.SaveManagerReverifyDocMapping();

                    this.uploader.clearQueue();
                } else {
                    this._notificationService.showError('File Upload Failed');
                    this.uploader.queue[0].isUploaded = false;
                    this.uploader.clearQueue();
                }
            }
        };
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
    SaveManagerReverifyDocMapping() {

        const inputReq = { TableSchema: AppSettings.TenantSchema, CustomerID: this._editManagerReverificationData.CustomerID, ReverificationID: this._editManagerReverificationData.ReverificationID, DocTypeIDs: this.AssignedDocTypes.filter((v, i, a) => a.indexOf(v) === i).slice() };
        this._managerReverificationService.SaveManagerReverifyDocMapping(inputReq);

    }
    singleFileUpload() {
        this.uploader.uploadAll();
    }
    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
