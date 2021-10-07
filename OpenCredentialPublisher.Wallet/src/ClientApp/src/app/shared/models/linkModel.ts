import { ClrModel } from "./clrModel";
import { ShareModel } from "./shareModel";

	/** Represents a Shareable link to a CLR. */
export class LinkModel {
  /** The primary key. */
  id: string;
  clrForeignKey: number;
  /** The CLR this link points to. */
  clr: ClrModel;
  /** The number of times this link has been used to display the CLR. */
  displayCount: number;
  /** A nickname for the link to help remember who it was shared with. */
  nickname: string;
  requiresAccessKey: boolean;
  /** Application user */
  userId: string;
  user: {
    displayName: string;
    profileImageUrl: string;
    addresses: any[];
    emails: any[];
    phoneNumbers: any[];
    paymentRequests: any[];
    credits: any[];
  };
  credentialRequestId?: number;
  credentialRequest: any;
  shares: ShareModel[];
  createdAt: Date;
  modifiedAt?: Date;
}
