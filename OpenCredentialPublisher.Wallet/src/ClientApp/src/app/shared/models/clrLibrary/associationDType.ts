import { AssociationTypeEnum } from "./enums/associationTypeEnum";

/** Association is based on the CASE AssociationLink object. An Association associates (relates) one Achievement with another Achievement. */
export class AssociationDType {
  /** The type of association. This uses an enumerated vocabulary. */
  associationType: AssociationTypeEnum;
  /** The '@id' of another achievement, or target of the association. Model Primitive Datatype = NormalizedString. */
  targetId: string;
  /** A human readable title for the associated achievement. Model Primitive Datatype = String. */
  title: string;
  /** Additional properties of the object */
  additionalProperties: { [index: string]: any };
}
