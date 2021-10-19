export class logModel {
  id: number;
  message: string;
  messageTemplate: string;
  level: string;
  timestamp: Date;
  exception: string;
  properties: string;
  logEvent: string;

  constructor() {
    this.id = -1;
    this.message = '';
    this.messageTemplate = '';
    this.level = '';
    this.timestamp: Date;
    this.exception = '';
    this.properties = '';
    this.logEvent = '';
  }

}
