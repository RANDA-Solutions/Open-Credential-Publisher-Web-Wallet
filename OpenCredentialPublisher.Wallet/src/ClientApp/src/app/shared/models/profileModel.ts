import { EndorsementModel } from "./endorsementModel";
import { VerificationModel } from "./verificationModel";

export class ProfileModel {
  /** Primary key. */
  profileId: number;
  isEndorsementProfile: boolean;
  /** IBaseEntity properties */
  isDeleted: boolean;
  createdAt: Date;
  modifiedAt: Date;
  /** Unique IRI for the Learner, Publisher, and Issuer Profile document. The Assertion Recipient is identified by reference to the Learner's Profile via the id, email, url, telephone, sourcedId, or studentId property. Model Primitive Datatype = NormalizedString. */
  id: string;
  /** The JSON-LD type of this object. Normally 'Profile'. Model Primitive Datatype = NormalizedString. */
  type: string;
  /** Gets or Sets Address */
  address: {
    id: string;
    type: string;
    addressCountry: string;
    addressLocality: string;
    addressRegion: string;
    postalCode: string;
    postOfficeBoxNumber: string;
    streetAddress: string;
    additionalProperties: { [index: string]: any };
  };
  additionalName: string;
  /** NOT on EndorsementProfileDType */
  birthDate: Date;
  /** A short description of the individual or organization. Model Primitive Datatype = String. */
  description: string;
  /** A contact email address for the individual or organization. Model Primitive Datatype = String. */
  email: string;
  familyName: string;
  givenName: string;
  identifiers: any[];
  /** IRI of an image representing the individual or organization. May be a DATA URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
  image: string;
  /** The name of the individual or organization. Model Primitive Datatype = String. */
  name: string;
  official: string;
  /** Gets or Sets PublicKey */
  publicKey: {
    context: string;
    id: string;
    type: string;
    owner: string;
    publicKeyPem: string;
    additionalProperties: { [index: string]: any };
  };
  /** The URL of the Revocation List document used for marking revocation of signed Assertions, CLRs, and Endorsements. Required for issuer profiles. Model Primitive Datatype = AnyURI. */
  revocationList: string;
  /** The individual's or organization's unique 'sourcedId' value, which is used for providing interoperability with other identity systems. Model Primitive Datatype = String. */
  sourcedId: string;
  /** An institution's student identifier for the person. This is frequently issued through a Student Information System. Model Primitive Datatype = String. */
  studentId: string;
  /** Primary phone number for the individual or organization. Model Primitive Datatype = String. */
  telephone: string;
  /** Web resource that uniquely represents or belongs to the individual. This may be a resource about the individual, hosting provided by the institution to the individual, or an web resource independently controlled by the individual. Model Primitive Datatype = AnyURI. */
  url: string;
  /** Additional properties of the object */
  additionalProperties: { [index: string]: any };
  /** End From ProfileDTypeForeignKeys */
  parentProfileId?: number;
  verificationId?: number;
  /** Relationships */
  parentOrg: ProfileModel;
  childrenOrgs: ProfileModel[];
  /** public virtual AchievementModel Achievement { get; set; }public virtual AssertionModel Assertion { get; set; }public virtual ClrModel Clr { get; set; } */
  profileEndorsements: EndorsementModel[];
  verification: VerificationModel;
}
