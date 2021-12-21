import { ApplicationUser } from "./applicationUser";
import { LinkModel } from "./linkModel";
import { MessageModel } from "./messageModel";
import { RecipientModel } from "./recipientModel";

export class ShareModel {
  id: number;
  linkId: string;
  shareTypeId: any;
  recipientId?: number;
  accessKey: string;
  useCount: number;
  createdAt: Date;
  modifiedAt: Date;
  isDeleted: boolean;
  statusId: any;
  messages: MessageModel[];
  recipient: RecipientModel;
  link: LinkModel;
}
