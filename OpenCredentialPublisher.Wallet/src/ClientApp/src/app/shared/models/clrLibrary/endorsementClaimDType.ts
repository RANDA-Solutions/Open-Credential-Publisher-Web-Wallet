/** An entity, identified by an id and additional properties that the endorser would like to claim about that entity. */
export class EndorsementClaimDType {
  /** The 'id' of the Profile, Achievement, or Assertion being endorsed. Model Primitive Datatype = NormalizedString. */
  id: string;
  /** The JSON-LD type of this entity. Normally 'EndorsementClaim'. Model Primitive Datatype = NormalizedString. */
  type: string;
  /** An endorser's comment about the quality or fitness of the endorsed entity. Markdown allowed. Model Primitive Datatype = String. */
  endorsementComment: string;
  /** Additional properties of the object */
  additionalProperties: { [index: string]: any };
}
