import {ParkingSpotRowData} from './parking-spot-rowdata.model' ;

export class AddParkingSpotdata extends ParkingSpotRowData {

  TableSchema: string ;

  constructor(TableSchema: string, ParkinSpotName: string , ParkingActive: boolean) {
    super(ParkinSpotName, ParkingActive);
  this.TableSchema = TableSchema;

  }

}

export class EditParkingSpotdata extends ParkingSpotRowData {
  TableSchema: string ;

  constructor(TableSchema: string, ParkinSpotName: string , ParkingActive: boolean, ParkingSpotID: number) {
    super(ParkinSpotName, ParkingActive, ParkingSpotID);
  this.TableSchema = TableSchema;
}
}
