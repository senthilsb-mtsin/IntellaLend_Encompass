export class ParkingSpotRowData {
 ParkingSpotName: string;
 Active: boolean;
 ParkingSpotID?: number;
 constructor(ParkinSpotName: string, ParkingActive: boolean, ParkingSpotID?: number) {
this.ParkingSpotName = ParkinSpotName;
this.Active = ParkingActive;
this.ParkingSpotID = ParkingSpotID;

 }
}
