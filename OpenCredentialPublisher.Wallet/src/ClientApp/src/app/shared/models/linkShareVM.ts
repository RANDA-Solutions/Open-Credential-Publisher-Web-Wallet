import { SelOption } from "./options";

export class LinkShareVM {
  linkId: string;
  linkNickname: string;
  recipientId?: number;
  recipients: SelOption[];
  sendToBSC: boolean;
}
