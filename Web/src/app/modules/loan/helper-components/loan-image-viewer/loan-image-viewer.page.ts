import { Component, HostListener, ViewChild, ElementRef, QueryList, ViewChildren, OnInit, OnDestroy } from '@angular/core';
import { LoanInfoService } from '../../services/loan-info.service';
import { Subscription } from 'rxjs';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { DocumentsearchPipe } from '../../pipes/stack-document-search.pipe';
import { NotificationService } from '@mts-notification';

@Component({
    selector: 'mts-loan-image-viewer',
    templateUrl: 'loan-image-viewer.page.html',
    animations: [
        trigger('rotatedState', [
            state('default', style({ transform: 'rotate(0)' })),
            state('90rotated', style({ transform: 'rotate(90deg)' })),
            state('-90rotated', style({ transform: 'rotate(-90deg)' })),
            state('180rotated', style({ transform: 'rotate(-180deg)' })),
            transition('rotated => default', animate('4000ms ease-out')),
            transition('default => rotated', animate('4000ms ease-in'))
        ])
    ],
    styleUrls: ['loan-image-viewer.page.scss'],
    providers: [DocumentsearchPipe]
})
export class LoanImageViewerComponent implements OnInit, OnDestroy {
    CurrentDocName = '';
    @ViewChild('imgDivList') imgDivList: ElementRef;
    @ViewChildren('listImages') listImages: QueryList<ElementRef>;
    @ViewChild('imgChild') imgChild: ElementRef;
    @ViewChild('leftImgScroll') leftImgScroll: ElementRef;
    @ViewChild('imgScroll') imgScroll: ElementRef;
    @ViewChild('imgScrollList') imgScrollList: ElementRef;
    @ViewChild('rightImgScroll') rightImgScroll: ElementRef;

    imageViewerType: any = 3;
    promiseImage: Subscription;
    CompareImageViewerType = false;
    showAllDocuments = false;
    multiImageSource: any[] = [];
    leftImageSource: any[] = [];
    availableDocuments: any[] = [];
    versionedDocs: { id: any, text: any }[] = [];
    selectedIndex: any = 0;
    _isReverifyImg: boolean;
    imgSrc: any = { PageNo: '', currentHeight: '', currentWidth: '' };
    ImageViewerHeight = 'calc(100vh - 360px)';
    imageStyle: any = 'block';
    doc: { DocID: any, VersionNumber: any, lastPageNumber: any, pageNumberArray: any[], _currentDocName: any, _totalImageCount: any, checkListState: any } = {
        DocID: 0, VersionNumber: 0, lastPageNumber: 0, pageNumberArray: [], _currentDocName: '', _totalImageCount: 0, checkListState: false
    };
    syncScroll = false;
    state: any = 'default';
    ImgDocName = '';
    CardHeight = '';

    constructor(
        private _loanInfoService: LoanInfoService,
        private _docSearch: DocumentsearchPipe,
        private _notificationService: NotificationService
    ) { }

    private _subscriptions: Subscription[] = [];
    private LastDrawPoint = { x0: 0, y0: 0, x1: 0, y1: 0, draw: false };
    private zoom: any = 10;
    private imgLastScrollTop: any;
    private _currentPage: any = 1;
    private lastViewerType: any = 3;

    private lastScroll = 0;

    @HostListener('window:keydown', ['$event'])
    handleClick(event: Event) {
        this.hotkeys(event);
    }

    ngOnInit(): void {

        this._subscriptions.push(this._loanInfoService.multiImageSource$.subscribe((res: any[]) => {
            this.multiImageSource = res;
            this.RestDrawBox();
            this.getImageByPageNo(0);
        }));

        this._subscriptions.push(this._loanInfoService.leftImageSource$.subscribe((res: any[]) => {
            this.leftImageSource = res;
        }));

        this._subscriptions.push(this._loanInfoService.ShowDocumentDetailView$.subscribe((res: boolean) => {
            if (res) {
                this.doc = this._loanInfoService.GetLoanViewDocument();
                this.getDocImage();
            }
        }));

        this._subscriptions.push(this._loanInfoService.RevifyShowDocumentDetailView$.subscribe((res: boolean) => {
            if (res) {
                this.doc = this._loanInfoService.GetLoanViewDocument();
                this.getDocImage();
            }
        }));

        this._subscriptions.push(this._loanInfoService.LoanPopOutImageViewerHeight$.subscribe((res: string) => {
            this.ImageViewerHeight = res;
        }));

        this._subscriptions.push(this._loanInfoService.LoanReverifyImageViewerHeight$.subscribe((res: string) => {
            this.ImageViewerHeight = res;
            this._isReverifyImg = false;
        }));

        this._subscriptions.push(this._loanInfoService.SetFieldDrawBox$.subscribe((res: { checklistState: boolean, pageData: { cords: any, pageNo: any } }) => {
            if (res.checklistState) {
                this.LoanInfoSetDrawBox(res.pageData);
            } else {
                this.SetDrawBox(res.pageData.cords.x0, res.pageData.cords.y0, res.pageData.cords.x1, res.pageData.cords.y1, res.pageData.pageNo);
            }
        }));

        this._subscriptions.push(this._loanInfoService.ImageFit$.subscribe((res: boolean) => {
            if (res) {
                this.ImageFitIn();
            }
        }));

        this.ImgDocName = this._loanInfoService.GetLoanViewDocument()._currentDocName;
    }

    LoanInfoSetDrawBox(_loan: { cords: any, pageNo: any }) {
        _loan.pageNo = parseInt(_loan.pageNo, 10);
        if (this.doc.pageNumberArray.includes(_loan.pageNo)) {
            this.ImageFitIn();
            if (_loan.pageNo < this.doc._totalImageCount && (_loan.cords.x0 !== 0 && _loan.cords.x1 !== 0)) {
                if (this._currentPage !== _loan.pageNo) {
                    this._currentPage = _loan.pageNo;
                    this.getImageByPageNo(this._currentPage);
                }
                this.RestDrawBox();
                this.LastDrawPoint.x0 = _loan.cords.x0;
                this.LastDrawPoint.y0 = _loan.cords.y0;
                this.LastDrawPoint.x1 = _loan.cords.x1;
                this.LastDrawPoint.y1 = _loan.cords.y1;
                this.LastDrawPoint.draw = true;
                setTimeout(() => {
                    this.DrawBox();
                    this.MoveToBox();
                }, 500);
            } else {
                this.RestDrawBox();
            }
        }
    }

    SetDrawBox(x0, y0, x1, y1, pageNo) {
        pageNo = parseInt(pageNo, 10);
        if (pageNo < this.doc._totalImageCount && (x0 !== 0 && x1 !== 0)) {
            if (this._currentPage !== pageNo) {
                this._currentPage = pageNo;
                this.getImageByPageNo(this._currentPage);
            }
            this.RestDrawBox();
            this.LastDrawPoint.x0 = x0;
            this.LastDrawPoint.y0 = y0;
            this.LastDrawPoint.x1 = x1;
            this.LastDrawPoint.y1 = y1;
            this.LastDrawPoint.draw = true;
            this.DrawBox();
            this.MoveToBox();
        } else {
            this.RestDrawBox();
        }
    }

    MoveToBox() {
        const preelem = document.getElementById('viewerPointShotDiv');
        let imgElement;
        let pageHeight = 0;
        if (this.imageViewerType === 1) {
            imgElement = this.imgScroll.nativeElement;
        } else {
            pageHeight = preelem.parentElement.offsetTop;
            imgElement = this.imgScrollList.nativeElement;
        }

        if (preelem !== undefined) {
            const divHeight = imgElement.clientHeight;
            const divWidth = imgElement.clientWidth;
            imgElement.scrollTop = (pageHeight + preelem.offsetTop) - (divHeight / 2);
            imgElement.scrollLeft = preelem.offsetLeft - (divWidth / 2);
        }
    }

    RestDrawBox() {
        this.RemoveBox();
        this.LastDrawPoint.x0 = 0;
        this.LastDrawPoint.y0 = 0;
        this.LastDrawPoint.x1 = 0;
        this.LastDrawPoint.y1 = 0;
        this.LastDrawPoint.draw = false;
    }

    RemoveBox() {
        const preelem = document.getElementById('viewerPointShotDiv');
        if (isTruthy(preelem)) {
            preelem.remove();
        }
    }

    getDocImage() {
        this.promiseImage = this._loanInfoService.getDocImage(this.doc.DocID, this.doc.VersionNumber, 0, this.imgDivList);
    }

    hotkeys(evt) {
        if (evt.altKey === true && evt.keyCode === 107) {
            this.ImageZoomIn();
        } else if (evt.altKey === true && evt.keyCode === 109) {
            this.ImageZoomOut();
        } else if (evt.keyCode === 37) {
            this.PreviousImage();
        } else if (evt.keyCode === 39) {
            this.NextImage();
        }
    }

    NextImage() {
        this._currentPage = this._currentPage + 1;
        this.getImageByPageNo(this._currentPage);
        this.state = 'default';
        this.RestDrawBox();
    }

    PreviousImage() {
        this._currentPage = this._currentPage - 1;
        this.getImageByPageNo(this._currentPage);
        this.state = 'default';
        this.RestDrawBox();
    }

    getImageByPageNo(pageNo) {
        if (this.multiImageSource.length > 0) {
            const imgItem = this.multiImageSource.filter(m => parseInt(m.PageNo, 10) === parseInt(pageNo, 10));
            this.imgSrc = imgItem[0];
        }
    }

    ImageZoomIn() {
        if (this.imageViewerType === 1) {
            // var currentImgeProp = this.imgSrc;
            // currentImgeProp.currentZoom += this.zoom;
            // if (currentImgeProp.currentZoom >= 800)
            //     currentImgeProp.currentZoom = 800;

            // currentImgeProp.currentHeight = (currentImgeProp.compressedImgHeight * currentImgeProp.currentZoom) / 100;
            // currentImgeProp.currentWidth = (currentImgeProp.compressedImgwidth * currentImgeProp.currentZoom) / 100;
            // this.DrawBox();
        } else if (this.imageViewerType === 2) {
            for (let i = 0; i < this.multiImageSource.length; i++) {
                const currentImgeProp = this.multiImageSource[i];
                currentImgeProp.currentZoom += this.zoom;
                if (currentImgeProp.currentZoom >= 400) {
                    currentImgeProp.currentZoom = 400;
                }

                currentImgeProp.currentHeight = (currentImgeProp.compressedImgHeight * currentImgeProp.currentZoom) / 100;
                currentImgeProp.currentWidth = (currentImgeProp.compressedImgwidth * currentImgeProp.currentZoom) / 100;
            }
            for (let i = 0; i < this.leftImageSource.length; i++) {
                const currentImgeProp = this.leftImageSource[i];
                currentImgeProp.currentZoom += this.zoom;
                if (currentImgeProp.currentZoom >= 400) {
                    currentImgeProp.currentZoom = 400;
                }

                currentImgeProp.currentHeight = (currentImgeProp.compressedImgHeight * currentImgeProp.currentZoom) / 100;
                currentImgeProp.currentWidth = (currentImgeProp.compressedImgwidth * currentImgeProp.currentZoom) / 100;
            }
        } else if (this.imageViewerType === 3) {
            for (let i = 0; i < this.multiImageSource.length; i++) {
                const currentImgeProp = this.multiImageSource[i];
                currentImgeProp.currentZoom += this.zoom;
                if (currentImgeProp.currentZoom >= 800) {
                    currentImgeProp.currentZoom = 800;
                }

                currentImgeProp.currentHeight = (currentImgeProp.compressedImgHeight * currentImgeProp.currentZoom) / 100;
                currentImgeProp.currentWidth = (currentImgeProp.compressedImgwidth * currentImgeProp.currentZoom) / 100;
            }
        }
    }

    ImageZoomOut() {
        if (this.imageViewerType === 1) {
            // var currentImgeProp = this.imgSrc;
            // currentImgeProp.currentZoom -= this.zoom;
            // if (currentImgeProp.currentZoom <= 10)
            //     currentImgeProp.currentZoom = 10;

            // currentImgeProp.currentHeight = (currentImgeProp.compressedImgHeight * currentImgeProp.currentZoom) / 100;
            // currentImgeProp.currentWidth = (currentImgeProp.compressedImgwidth * currentImgeProp.currentZoom) / 100;
            // this.DrawBox();
        } else if (this.imageViewerType === 3) {
            for (let i = 0; i < this.multiImageSource.length; i++) {
                const currentImgeProp = this.multiImageSource[i];
                currentImgeProp.currentZoom -= this.zoom;
                if (currentImgeProp.currentZoom <= 10) {
                    currentImgeProp.currentZoom = 10;
                }

                currentImgeProp.currentHeight = (currentImgeProp.compressedImgHeight * currentImgeProp.currentZoom) / 100;
                currentImgeProp.currentWidth = (currentImgeProp.compressedImgwidth * currentImgeProp.currentZoom) / 100;
            }
        } else if (this.imageViewerType === 2) {
            for (let i = 0; i < this.multiImageSource.length; i++) {
                const currentImgeProp = this.multiImageSource[i];
                currentImgeProp.currentZoom -= this.zoom;
                if (currentImgeProp.currentZoom <= 10) {
                    currentImgeProp.currentZoom = 10;
                }

                currentImgeProp.currentHeight = (currentImgeProp.compressedImgHeight * currentImgeProp.currentZoom) / 100;
                currentImgeProp.currentWidth = (currentImgeProp.compressedImgwidth * currentImgeProp.currentZoom) / 100;
            }

            for (let i = 0; i < this.leftImageSource.length; i++) {
                const currentImgeProp = this.leftImageSource[i];
                currentImgeProp.currentZoom -= this.zoom;
                if (currentImgeProp.currentZoom <= 10) {
                    currentImgeProp.currentZoom = 10;
                }

                currentImgeProp.currentHeight = (currentImgeProp.compressedImgHeight * currentImgeProp.currentZoom) / 100;
                currentImgeProp.currentWidth = (currentImgeProp.compressedImgwidth * currentImgeProp.currentZoom) / 100;
            }
        }
    }

    ImageFitIn() {
        if (this.imageViewerType === 1) {
            // var currentImgeProp = this.imgSrc;
            // currentImgeProp.currentZoom = (this.imgDiv.nativeElement.clientWidth / currentImgeProp.compressedImgwidth) * 100;
            // currentImgeProp.currentHeight = (currentImgeProp.compressedImgHeight * currentImgeProp.currentZoom) / 100;
            // currentImgeProp.currentWidth = (currentImgeProp.compressedImgwidth * currentImgeProp.currentZoom) / 100;
            // this.DrawBox();
        } else if (this.imageViewerType === 3) {
            for (let i = 0; i < this.multiImageSource.length; i++) {
                const currentImgeProp = this.multiImageSource[i];
                currentImgeProp.currentZoom = (this.imgDivList.nativeElement.clientWidth / currentImgeProp.compressedImgwidth) * 100;
                currentImgeProp.currentHeight = (currentImgeProp.compressedImgHeight * currentImgeProp.currentZoom) / 100;
                currentImgeProp.currentWidth = (currentImgeProp.compressedImgwidth * currentImgeProp.currentZoom) / 100;
            }
        }
    }

    ImageRotate() {
        if (this.state === 'default') {
            this.state = '90rotated';
        } else if (this.state === '90rotated') {
            this.state = '180rotated';
        } else if (this.state === '180rotated') {
            this.state = '-90rotated';
        } else if (this.state === '-90rotated') {
            this.state = 'default';
        }

        this.DrawBox();
    }

    ShowListView() {
        this.doc.lastPageNumber = 0;
        if (this.imageViewerType === 1) {
            this.imageViewerType = 3;
            // this._loan.onLoading = true;
            // this._loan.pageNumberArray = [];
            // this.loadingpage = 0;
        } else {
            // this._loan.pageNumberArray = [];
            this.imageViewerType = 1;
            // this._currentPage = 0;
            this.RestDrawBox();
        }
    }

    ToogleScrollSync() {
        this.syncScroll = !this.syncScroll;
        if (this.syncScroll) {
            this.lastScroll = this.leftImgScroll.nativeElement.scrollTop;
            this.scrollSync();
        }
    }

    scrollSync() {
        if (this.syncScroll) {
            const scrollcnt = (this.leftImgScroll.nativeElement.scrollTop - this.lastScroll);
            this.rightImgScroll.nativeElement.scrollTop += scrollcnt;
            this.lastScroll = this.leftImgScroll.nativeElement.scrollTop;
        }
    }

    ShowDocCompare() {
        if (this.imageViewerType !== 2) {
            this.lastViewerType = this.imageViewerType;
            this.imageViewerType = 2;
        } else {
            this.imageViewerType = this.lastViewerType;
        }

        if (this.imageViewerType === 2) {
            this.showAllDocuments = true;
            this.leftImageSource = [];
            this.syncScroll = false;
            this.lastScroll = 0;
            this._loanInfoService.GetCompareImages(this._loanInfoService.GetLoanViewDocument().DocID, this._loanInfoService.GetLoanViewDocument().VersionNumber, 0);
        }
    }

    show() {
        this.availableDocuments = this._docSearch.transform(this._loanInfoService.GetLoan().loanDocuments.slice(), [undefined, 'In Loan']);
        this.versionedDocs = [];
        this.imageViewerType = 2;
        this.showAllDocuments = true;
        this.leftImageSource = [];
        this.syncScroll = false;
        this.lastScroll = 0;
        this.lastViewerType = 3;
        setTimeout(() => {
            this.selectedIndex = 0;
        }, 200);
        this.showAllDocuments = true;
        this.availableDocuments.forEach(element => {
            this.versionedDocs.push({
                id: element.DocID + ',' + this._loanInfoService.GetLoan().LoanID + ',' + element.VersionNumber,
                text: this._loanInfoService.CheckMoreThanOnce(element.DocName) ? element.DocName + ' -V' + (element.FieldOrderBy === '' ? (element.VersionNumber).toString() : this._loanInfoService.GetFieldVersionNumber(element.DocName, element.FieldOrderVersion)) : element.DocName
            });
        });
    }

    SetDocumentName(val: { id: any, text: any }) {
        let docs: any = [];
        if (isTruthy(val)) {
            if (Array.isArray(val) && val.length !== 0) {
                docs = val[0].id.split(',');
            } else if (!Array.isArray(val)) {
                docs = val.id.split(',');
            }
        }

        if (isTruthy(docs) && docs.length > 0) {
            this.leftImageSource = [];
            this.selectedIndex = val.id;
            this._loanInfoService.GetCompareImages(docs[0], docs[2], 0);
        }
    }

    InitListViewer(pageNo: number) {
        let currentImgeProp;
        for (let i = 0; i < this.multiImageSource.length; i++) {
            if (this.multiImageSource[i].PageNo === pageNo) {
                currentImgeProp = this.multiImageSource[i];
                // imgElment = this.listImages.toArray()[i];
            }
        }

        if (currentImgeProp !== undefined) {
            const imgElement = this.listImages.toArray()[pageNo].nativeElement;
            currentImgeProp.orgImgHeight = imgElement.naturalHeight;
            currentImgeProp.orgImgwidth = imgElement.naturalWidth;
            currentImgeProp.compressedImgHeight = imgElement.naturalHeight;
            currentImgeProp.compressedImgwidth = imgElement.naturalWidth;

            currentImgeProp.currentZoom = (this.imgDivList.nativeElement.clientWidth / currentImgeProp.compressedImgwidth) * 100;
            currentImgeProp.currentHeight = (currentImgeProp.compressedImgHeight * currentImgeProp.currentZoom) / 100;
            currentImgeProp.currentWidth = (currentImgeProp.compressedImgwidth * currentImgeProp.currentZoom) / 100;

            currentImgeProp.ImageShown = true;
            this.doc.pageNumberArray.push(pageNo);
        }
    }

    InitViewer() {
        this.ImageFitIn();
        this.DrawBox();
        const rightImage = this.leftImgScroll.nativeElement;
        const imgElement = this.imgScroll.nativeElement;
        // var img1Element = this.imgScrollList.nativeElement;
        if (imgElement !== undefined && this.imgLastScrollTop !== undefined) {
            const increasedScrollTop = this.imgLastScrollTop;
            imgElement.scrollTop += increasedScrollTop;
        } else if (rightImage !== undefined && this.imgLastScrollTop !== undefined) {
            const increasedScrollTop = this.imgLastScrollTop;
            rightImage.scrollTop += increasedScrollTop;
        } else {
            imgElement.scrollTop += 0;
        }
    }

    DrawBox() {
        let imgElement;
        if (this.imageViewerType === 1) {
            imgElement = this.imgChild.nativeElement;
        } else if (this.imageViewerType === 3) {
            imgElement = this.listImages.toArray()[this._currentPage].nativeElement;
        }

        this.RemoveBox();

        if (this.LastDrawPoint.draw && this.imgSrc !== undefined && (this.state === 'default' || this.imageViewerType === 3)) {

            let x0 = this.LastDrawPoint.x0;
            let y0 = this.LastDrawPoint.y0;
            let x1 = this.LastDrawPoint.x1;
            let y1 = this.LastDrawPoint.y1;

            const zoomVal = (this.imgSrc.currentWidth / this.imgSrc.orgImgwidth) * 100;
            x0 = (x0 * zoomVal) / 100;
            y0 = (y0 * zoomVal) / 100;
            x1 = (x1 * zoomVal) / 100;
            y1 = (y1 * zoomVal) / 100;

            const imgLeft = imgElement.offsetLeft;
            const imgTop = imgElement.offsetTop;

            let pos1N = (x0);
            let pos2N = (y0);
            let dim1N = (x1 - x0);
            let dim2N = (y1 - y0);

            pos1N -= (((dim1N * 60) / 100) / 2);
            pos2N -= (((dim2N * 120) / 100) / 2);
            dim1N += ((dim1N * 60) / 100);
            dim2N += ((dim2N * 120) / 100);

            const pos1Style = 'left:' + pos1N.toString() + 'px;';
            const pos2Style = 'top:' + pos2N.toString() + 'px;';

            const dms1Style = 'width:' + dim1N.toString() + 'px;'; // based on x cords
            const dms2Style = 'height:' + dim2N.toString() + 'px;'; // based on y cords

            const mapBoxStyle = pos1Style + pos2Style + dms1Style + dms2Style + ';position: absolute;border: 2px solid rgb(221, 0, 0); background-color: yellow;';

            const iDiv = document.createElement('div');
            iDiv.id = 'viewerPointShotDiv';
            iDiv.style.width = dim1N.toString() + 'px';
            iDiv.style.height = dim2N.toString() + 'px';
            iDiv.style.left = pos1N.toString() + 'px';
            iDiv.style.top = pos2N.toString() + 'px';
            iDiv.style.position = 'absolute';
            iDiv.style.opacity = '0.5';
            iDiv.style.border = '3px solid rgb(221, 0, 0)';
            imgElement.parentNode.appendChild(iDiv);
        }
    }

    // showTracker() {
    //     this._loanInfoService.ShowDocumentDetailView$.next(false);
    //     this._loanInfoService.FIELDToggle$.next(true);
    // }

    // popOut() { }

    ngOnDestroy() {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
