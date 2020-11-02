
import { RouterModule, Routes } from '@angular/router';
import { DocumentTypeComponent } from './pages/documenttype/documenttype.page';
import { AddDocumentTypeComponent } from './pages/adddocumenttype/adddocumenttype.page';
import { EditDocumentTypeComponent } from './pages/editdocumenttype/editdocumenttype.page';
import { ViewDocumentTypeComponent } from './pages/viewdocumenttype/viewdocumenttype.page';

const DocumentRoutes: Routes = [
    {
        path: 'login',
        redirectTo: '/'
    },
    {
        path: '',
        component: DocumentTypeComponent
    },
    {
        path: 'adddocumenttype',
        data: { routeURL: 'View\\DocumentType\\AddDocumentType' },
        component: AddDocumentTypeComponent
    },
    {
        path: 'editdocumenttype',
        data: { routeURL: 'View\\DocumentType\\EditBtn' },
        component: EditDocumentTypeComponent
    },
    {
        path: 'viewdocumenttype',
        data: { routeURL: 'View\\DocumentType\\ViewBtn' },
        component: ViewDocumentTypeComponent
    }
];

export const DocumentRouting = RouterModule.forChild(DocumentRoutes);
