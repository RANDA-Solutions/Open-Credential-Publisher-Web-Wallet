
export class VerificationModel {
  /** Primary key. */
  verificationId: number;
  /** IBaseEntity properties */
  isDeleted: boolean;
  createdAt: Date;
  modifiedAt: Date;
  /** Unique IRI for the Verification. Model Primitive Datatype = NormalizedString. */
  id: string;
  /** The JSON-LD type of this object. The strongly typed value indicates the verification method. */
  type: any;
  /** The host registered name subcomponent of an allowed origin. Any given id URI will be considered valid. Model Primitive Datatype = String. */
  allowedOrigins: string[];
  /** The (HTTP) id of the key used to sign the Assertion, CLR, or Endorsement. If not present, verifiers will check the public key declared in the referenced issuer Profile. If a key is declared here, it must be authorized in the issuer Profile as well. creator is expected to be the dereferencable URI of a document that describes a CryptographicKey. Model Primitive Datatype = AnyURI. */
  creator: string;
  /** The URI fragment that the verification property must start with. Valid Assertions, Clrs, and Endorsements must have an id within this scope. Multiple values allowed, and Assertions, Clrs, and Endorsements will be considered valid if their id starts with one of these values. Model Primitive Datatype = String. */
  startsWith: string[];
  /** The property to be used for verification. Only 'id' is supported. Verifiers will consider 'id' the default value if verificationProperty is omitted or if an issuer Profile has no explicit verification instructions, so it may be safely omitted. Model Primitive Datatype = String. */
  verificationProperty: string;
  /** Additional properties of the object */
  additionalProperties: { [index: string]: any };
  /** End From VerificationDType */
}
