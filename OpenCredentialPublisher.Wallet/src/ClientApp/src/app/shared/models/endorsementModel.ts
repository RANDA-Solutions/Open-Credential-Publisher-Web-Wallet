import { EndorsementClaimModel } from "./endorsementClaimModel";
import { ProfileModel } from "./profileModel";
import { VerificationModel } from "./verificationModel";

export class EndorsementModel {
  signedEndorsement: string;
  /** Primary key. */
  endorsementId: number;
  /** IBaseEntity properties */
  isDeleted: boolean;
  createdAt: Date;
  modifiedAt: Date;
  isSigned: boolean;
  /** Globally unique IRI for the Endorsement. If this Endorsement will be verified using Hosted verification, the value should be the URL of the hosted version of the Endorsement. Model Primitive Datatype = NormalizedString. */
  id: string;
  /** The JSON-LD type of this entity. Normally 'Endorsement'. Model Primitive Datatype = NormalizedString. */
  type: string;
  /** Timestamp of when the endorsement was published. Model Primitive Datatype = DateTime. */
  issuedOn: Date;
  /** If revoked, optional reason for revocation. Model Primitive Datatype = String. */
  revocationReason: string;
  /** If True the endorsement is revoked. Model Primitive Datatype = Boolean. */
  revoked?: boolean;
  /** Additional properties of the object */
  additionalProperties: { [index: string]: any };
  /** End From EndorsementDTypeForeignKeys */
  issuerId: number;
  verificationId: number;
  endorsementClaimId: number;
  endorsementClaim: EndorsementClaimModel;
  issuer: ProfileModel;
  verification: VerificationModel;
}
