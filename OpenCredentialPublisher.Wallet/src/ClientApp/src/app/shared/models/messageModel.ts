import { ShareModel } from "./shareModel";

export class MessageModel {
  id: number;
  recipient: string;
  body: string;
  subject: string;
  sendAttempts: number;
  statusId: any;
  createdOn: Date;
  shareId?: number;
  share: ShareModel;
}
