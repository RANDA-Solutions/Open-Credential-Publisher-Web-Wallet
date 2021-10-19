import { AddressDType } from "./addressDType";
import { CryptographicKeyDType } from "./cryptographicKeyDType";
import { VerificationDType } from "./verificationDType";

/** A Profile is a collection of information that describes the person or organization using Comprehensive Learner Record (CLR). Publishers, learners, and issuers must be represented as profiles. Recipients, endorsers, or other entities may also be represented using this vocabulary. An EndorsementProfile cannot have endorsements. */
export class EndorsementProfileDType {
  /** Unique IRI for the Profile. Model Primitive Datatype = NormalizedString. */
  id: string;
  /** The JSON-LD type of this entity. Normally 'EndorsementProfile'. Unlike Profile, EndorsementProfile cannot have endorsements. Model Primitive Datatype = NormalizedString. */
  type: string;
  additionalName: string;
  /** Gets or Sets Address */
  address: AddressDType;
  /** A short description of the individual or organization. Model Primitive Datatype = String. */
  description: string;
  /** A contact email address for the individual or organization. Model Primitive Datatype = String. */
  email: string;
  familyName: string;
  givenName: string;
  identifiers: any[];
  /** Image representing the individual or organization. Model Primitive Datatype = NormalizedString. */
  image: string;
  /** The name of the individual or organization. Model Primitive Datatype = String. */
  name: string;
  official: string;
  /** Gets or Sets PublicKey */
  publicKey: CryptographicKeyDType;
  /** The URL of the Revocation List document used for marking revocation of signed Assertions, CLRs, and Endorsements. Required for issuer profiles. Model Primitive Datatype = AnyURI. */
  revocationList: string;
  /** The individual's unique 'sourcedId' value, which is used for providing interoperability with IMS Learning Information Services (LIS). Model Primitive Datatype = String. */
  sourcedId: string;
  /** An institution's student identifier for the person. This is frequently issued through a Student Information System. Model Primitive Datatype = String. */
  studentId: string;
  /** Primary phone number for the individual or organization. Model Primitive Datatype = String. */
  telephone: string;
  /** Web resource that uniquely represents or belongs to the individual. This may be a resource about the individual, hosting provided by the institution to the individual, or an web resource independently controlled by the individual. Model Primitive Datatype = AnyURI. */
  url: string;
  /** Gets or Sets Verification */
  verification: VerificationDType;
  /** Additional properties of the object */
  additionalProperties: { [index: string]: any };
}
