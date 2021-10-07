import { ApplicationUser } from "./applicationUser";
import { LinkModel } from "./linkModel";
import { MessageModel } from "./messageModel";

export class ShareModel {
  id: number;
  linkId: string;
  shareTypeId: any;
  recipientId?: number;
  accessKey: string;
  useCount: number;
  createdOn: Date;
  statusId: any;
  messages: MessageModel[];
  recipient: {
    id: number;
    userId: string;
    user: ApplicationUser;
    name: string;
    email: string;
    createdOn: Date;
  };
  link: LinkModel;
}
