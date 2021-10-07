import { AddressDType } from "../clrLibrary/addressDType";
import { VerificationVM } from "./verificationVM";

export class ProfileVM {
  profileId: number;
  isEndorsementProfile: boolean;
  /** IBaseEntity properties */
  isDeleted: boolean;
  createdAt: Date;
  modifiedAt: Date;
  id: string;
  type: string;
  additionalName: string;
  birthDate: Date;
  description: string;
  email: string;
  familyName: string;
  givenName: string;
  identifiers: any[];
  image: string;
  name: string;
  official: string;
  publicKey: {
    context: string;
    id: string;
    type: string;
    owner: string;
    publicKeyPem: string;
    additionalProperties: { [index: string]: any };
  };
  revocationList: string;
  sourcedId: string;
  studentId: string;
  telephone: string;
  url: string;
  verification: VerificationVM;
  address: AddressDType;
  additionalProperties: { [index: string]: any };
}
