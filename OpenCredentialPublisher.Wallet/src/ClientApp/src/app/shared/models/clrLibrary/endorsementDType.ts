import { EndorsementClaimDType } from "./endorsementClaimDType";
import { EndorsementProfileDType } from "./endorsementProfileDType";
import { VerificationDType } from "./verificationDType";

/** An endorsement claim. */
export class EndorsementDType {
  /** Globally unique IRI for the Endorsement. If this Endorsement will be verified using Hosted verification, the value should be the URL of the hosted version of the Endorsement. Model Primitive Datatype = NormalizedString. */
  id: string;
  /** The JSON-LD type of this entity. Normally 'Endorsement'. Model Primitive Datatype = NormalizedString. */
  type: string;
  /** Gets or Sets Claim */
  claim: EndorsementClaimDType;
  /** Timestamp of when the endorsement was published. Model Primitive Datatype = DateTime. */
  issuedOn: Date;
  /** Gets or Sets Issuer */
  issuer: EndorsementProfileDType;
  /** If revoked, optional reason for revocation. Model Primitive Datatype = String. */
  revocationReason: string;
  /** If True the endorsement is revoked. Model Primitive Datatype = Boolean. */
  revoked?: boolean;
  /** Gets or Sets Verification */
  verification: VerificationDType;
  /** Additional properties of the object */
  additionalProperties: { [index: string]: any };
}
