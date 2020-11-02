export class ResponseMessage {
  MessageID: string;

  MessageDesc: string;

  constructor(messageID: string, messageDesc: string) {
    this.MessageID = messageID;
    this.MessageDesc = messageDesc;
  }
}

export class MTSAPIResponse {
  Token: string;

  Data: string;

  ResponseMessage: ResponseMessage;

  constructor(token: string, data: string, messageID: string, messageDesc: string) {
    this.Token = token;
    this.Data = data;
    this.ResponseMessage = new ResponseMessage(messageID, messageDesc);
  }
}
