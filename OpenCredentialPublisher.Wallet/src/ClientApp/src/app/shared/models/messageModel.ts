import { ShareModel } from "./shareModel";

export class MessageModel {
  id: number;
  recipient: string;
  body: string;
  subject: string;
  sendAttempts: number;
  statusId: any;
  createdAt: Date;
  modifiedAt: Date;
  isDeleted: boolean;
  shareId?: number;
  share: ShareModel;
}
