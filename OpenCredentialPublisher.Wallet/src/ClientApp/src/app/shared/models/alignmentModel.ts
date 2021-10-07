export class AlignmentModel {
  /** Primary key. */
  alignmentId: number;
  /** IBaseEntity properties */
  isDeleted: boolean;
  createdAt: Date;
  modifiedAt: Date;
  /** Unique IRI for the object. Model Primitive Datatype = NormalizedString. */
  id: string;
  /** The JSON-LD type of this entity. Normally 'Alignment'. Model Primitive Datatype = NormalizedString. */
  type: string;
  /** The name of the framework to which the resource being described is aligned. Model Primitive Datatype = String. */
  educationalFramework: string;
  /** If applicable, a locally unique string identifier that identifies the alignment target within its framework. Model Primitive Datatype = String. */
  targetCode: string;
  /** The description of a node in an established educational framework. Model Primitive Datatype = String. */
  targetDescription: string;
  /** The name of a node in an established educational framework. Model Primitive Datatype = String. */
  targetName: string;
  /** The type of the alignment target node. This is an extensible enumerated vocabulary. Extending the vocabulary makes use of a naming convention. */
  targetType: string;
  /** The URL of a node in an established educational framework. When the alignment is to a CASE framework and the CASE Service support the CASE JSON-LD binding, the URL should be the 'uri' of the node document, otherwise the URL should be the CASE Service endpoint URL that returns the node JSON. Model Primitive Datatype = AnyURI. */
  targetUrl: string;
  /** Additional properties of the object */
  additionalProperties: { [index: string]: any };
}
