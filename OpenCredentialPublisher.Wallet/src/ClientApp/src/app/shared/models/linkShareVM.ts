import { LinkModel } from "./linkModel";
import { SelOption } from "./options";

export class LinkShareVM {
  linkId: string;
  linkNickname: string;
  recipientId: string;
  recipients: SelOption[];
}
