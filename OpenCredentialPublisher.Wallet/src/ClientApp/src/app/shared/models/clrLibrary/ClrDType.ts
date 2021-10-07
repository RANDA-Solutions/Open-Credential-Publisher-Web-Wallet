import { AchievementDType } from "./achievementDType";
import { AssertionDType } from "./assertionDType";
import { ProfileDType } from "./profileDType";
import { VerificationDType } from "./verificationDType";

/** A collection of assertions for a single person reported by a single publisher. */
export class ClrDType {
  /** URL to the JSON-LD context file. */
  context: string;
  /** Unique IRI for the CLR. If the CLR is meant to be verified using Hosted verification, the id must conform to the getClr endpoint format. Model Primitive Datatype = NormalizedString. */
  id: string;
  /** The JSON-LD type of this object. Normally 'CLR'. Model Primitive Datatype = NormalizedString. */
  type: string;
  /** Array of achievements that are related directly or indirectly through associations with the asserted achievements in the CLR. Primarily used to represent hierarchical pathways. Asserted achievements may appear in both this array and in the achievement assertion. If asserted achievements do appear in both places, they MUST match exactly. */
  achievements: AchievementDType[];
  /** The learner's asserted achievements. */
  assertions: AssertionDType[];
  /** Timestamp of when the CLR was published. Model Primitive Datatype = DateTime. */
  issuedOn: Date;
  /** Gets or Sets Learner */
  learner: ProfileDType;
  /** Optional name of the CLR. Model Primitive Datatype = String. */
  name: string;
  /** True if CLR does not contain all the assertions known by the publisher for the learner at the time the CLR is issued. Useful if you are sending a small set of achievements in real time when they are achieved. If False or omitted, the CLR SHOULD be interpreted as containing all the assertions for the learner known by the publisher at the time of issue. Model Primitive Datatype = Boolean. */
  partial?: boolean;
  /** Gets or Sets Publisher */
  publisher: ProfileDType;
  /** If revoked, optional reason for revocation. Model Primitive Datatype = String. */
  revocationReason: string;
  /** If True the CLR is revoked. Model Primitive Datatype = Boolean. */
  revoked?: boolean;
  /** Signed assertions in JWS Compact Serialization format. Model Primitive Datatype = String. */
  signedAssertions: string[];
  /** Signed endorsements in JWS Compact Serialization format. Model Primitive Datatype = String. */
  signedEndorsements: string[];
  /** Gets or Sets Verification */
  verification: VerificationDType;
  /** Additional properties of the object */
  additionalProperties: { [index: string]: any };
}
