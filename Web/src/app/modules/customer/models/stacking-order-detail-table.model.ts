export class StackingOrderDetailTable {
    StackingOrderDetailID: number;
    StackingOrderID: number;
    DocumentTypeID: any;
    SequenceID: number;
    DocumentTypeName: string;
    DocFieldList: DocumentTypeFieldMaster[];
    OrderByFieldID: any;
    DocFieldValueId: number;
    isGroup: boolean;
    StackingOrderGroupDetails: StackingOrderGroupMasters[];
    isContainer: boolean;
}

export class DocumentTypeFieldMaster {
    FieldID: number;
    DocumentTypeID: number;
    Name: string;
    DisplayName: string;
    Active: boolean;
    DocOrderByField: string;
    AllowAccuracyCalc: boolean;
    IsDocName: boolean;
}

export class StackingOrderGroupMasters {
    StackingOrderGroupID: number;
    StackingOrderGroupName: string;
    TrimmedStackingOrderGroupName: string;
    StackingOrderID: number;
    GroupSortField: string;
    Active: boolean;

    constructor(_grpName: string) {
        this.StackingOrderGroupName = _grpName;
        this.GroupSortField = '';
    }
}
