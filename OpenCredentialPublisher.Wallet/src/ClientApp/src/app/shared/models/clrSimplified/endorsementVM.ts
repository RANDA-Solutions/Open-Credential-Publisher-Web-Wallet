import { EndorsementClaimVM } from "./endorsementClaimVM";
import { ProfileVM } from "./profileVM";
import { VerificationVM } from "./verificationVM";

export class EndorsementVM {
  signedEndorsement: string;
  endorsementId: number;
  /** IBaseEntity properties */
  isDeleted: boolean;
  createdAt: Date;
  modifiedAt: Date;
  isSigned: boolean;
  id: string;
  type: string;
  issuedOn: Date;
  revocationReason: string;
  revoked?: boolean;
  additionalProperties: { [index: string]: any };
  endorsementClaim: EndorsementClaimVM;
  issuer: ProfileVM;
  verification: VerificationVM;
}
