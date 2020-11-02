import { Component, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import { NotificationService } from '@mts-notification';
import { DocumentFieldService } from '../../../services/documentfield.service';
import { AddDocumentFieldRequestModel } from '../../../models/adddocument-field-request.model';
import { DocumentFieldTable } from '../../../models/document-field-table.model';
import { DocumentTypeService } from '../../../services/documenttype.service';

@Component({
  selector: 'mts-adddocumentfield',
  templateUrl: 'adddocumentfield.page.html',
  styleUrls: ['adddocumentfield.page.css'],
})
export class AddDocumentFieldComponent implements OnInit, AfterViewInit {
  @ViewChild(DataTableDirective) datatableEl: DataTableDirective;

  dTable: any;
  dtOptions: any = {};
  DocumentTypeName: string;
  DocumentDisplayName: string;
  DocumentLevel: string;
  DocumentTypeID: number;
  fieldName: string;
  fieldDisplayName = '';
  _docTypeID = 0;
  _fieldID = 0;
  constructor(private _docFieldService: DocumentFieldService, private _docTypeService: DocumentTypeService, private _notificationService: NotificationService) {

  }
  private _deleteConfirmation: any;
  private subscription: Subscription[] = [];

  ngOnInit(): void {
    this.subscription.push(this._docFieldService.setDocFieldTableData.subscribe((res: any) => {
      this.dTable.clear();
      this.dTable.rows.add(res);
      this.dTable.draw();
    }));
    this.subscription.push(this._docTypeService.DocumentTypeID.subscribe((res: any) => {
      this._docTypeID = res;
    }));
    this.subscription.push(this._docFieldService.FieldId.subscribe((res: any) => {
      this._fieldID = res;
      this.SetFieldValue(this._fieldID);
    }));
    this.subscription.push(this._docFieldService.DeleteConfirmation.subscribe((res: any) => {
      this._deleteConfirmation = res;
    }));

    this.dtOptions = {
      displayLength: 5,
      aaData: [],
      'order': [[0, 'desc']],
      'iDisplayLength': 10,
      'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
      aoColumns: [
        { sTitle: 'FieldID', mData: 'FieldID', bVisible: false },
        { sTitle: 'Field Name', mData: 'FieldName' },
        { sTitle: 'Field Display Name', mData: 'FieldDisplayName' },
        { sTitle: '', mData: 'FieldID', sClass: 'text-center' }
      ], aoColumnDefs: [{
        'aTargets': [3],
        'mRender': function (date, type, row) {
          return '<span style="cursor:pointer" class="deleteField material-icons txt-themeRed">delete_forever</span>';
        }
      }],
      rowCallback: (row: Node, data: DocumentFieldTable, index: number) => {
        const self = this;
        $('td .deleteField', row).unbind('click');
        $('td .deleteField', row).bind('click', () => {
          self.DeleteField(row, data);
        });
        return row;
      }
    };
  }

  AddField() {
    if (this.fieldName !== '' && this.fieldDisplayName !== '') {
      let dupField = false;
      const dDataLength = this.dTable.rows().data().length;
      const dData = this.dTable.rows().data();
      for (let i = 0; i < dDataLength; i++) {
        if (dData[i].FieldName === this.fieldName) {
          dupField = true;
          this._notificationService.showError('Field Already Exist');
          this.fieldName = '';
          this.fieldDisplayName = '';
          break;
        }
      }
      if (!dupField) {
        const req = new AddDocumentFieldRequestModel(this._docTypeService._documentTypeID, this.fieldName, this.fieldDisplayName);
        this._docFieldService.AddField(req);
      }
    }
  }

  ngAfterViewInit() {
    this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
    });
  }

  DeleteField(rowNode: any, rowData: any) {
    const Req = { FieldID: rowData.FieldID };
    this._docFieldService.DeleteField(Req);
    if (this._deleteConfirmation === true) {
      this.dTable.row(rowNode).remove();
      this.dTable.draw();
      this._notificationService.showSuccess('Field Removed Successfully');
    }
  }

  SetFieldValue(val: any) {
    if (val !== 0) {
      this.dTable.row.add({ FieldID: this._fieldID, FieldName: this.fieldName, FieldDisplayName: this.fieldDisplayName });
      this.dTable.draw();
      this.fieldName = '';
      this.fieldDisplayName = '';
    }
  }
  ngDestroy() {
    this.subscription.forEach(element => {
      element.unsubscribe();
    });
  }

}
