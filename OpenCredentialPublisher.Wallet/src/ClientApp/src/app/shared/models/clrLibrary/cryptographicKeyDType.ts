/** Based on the Key class from the W3C Web Payments Community Group Security Vocabulary. A CryptographicKey document identifies and describes a public key used to verify signed Assertions. */
export class CryptographicKeyDType {
  /** URL to the JSON-LD context file. */
  context: string;
  /** The URI of the CryptographicKey document. Used during signed verification. Model Primitive Datatype = NormalizedString. */
  id: string;
  /** The JSON-LD type of this object. Normally 'CryptographicKey'. Model Primitive Datatype = NormalizedString. */
  type: string;
  /** The identifier for the Profile that owns this PUBLIC KEY and the PRIVATE KEY used to sign the assertion or endorsement. Model Primitive Datatype = NormalizedString. */
  owner: string;
  /** The PUBLIC KEY in PEM format corresponding to the PRIVATE KEY used by the owner to sign the assertion or endorsement. The PEM key encoding is a widely-used method to express public keys, compatible with almost every Secure Sockets Layer library implementation. Model Primitive Datatype = String. */
  publicKeyPem: string;
  additionalProperties: { [index: string]: any };
}
