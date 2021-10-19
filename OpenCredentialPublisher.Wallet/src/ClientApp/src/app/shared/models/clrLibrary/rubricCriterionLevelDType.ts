import { AlignmentDType } from "./alignmentDType";

/** Describes a rubric criterion level. */
export class RubricCriterionLevelDType {
  /** Unique IRI for the ResultCriterionLevel. Model Primitive Datatype = NormalizedString. */
  id: string;
  /** The JSON-LD type of this object. Normally 'RubricCriterionLevel'. Model Primitive Datatype = String. */
  type: string;
  /** The alignments between this rubric criterion level and nodes in external frameworks. */
  alignments: AlignmentDType[];
  /** A description of the rubric criterion level. Model Primitive Datatype = String. */
  description: string;
  /** The rubric performance level in terms of success. Model Primitive Datatype = String. */
  level: string;
  /** The name of the RubricCriterionLevel. Model Primitive Datatype = String. */
  name: string;
  /** The number of grade points corresponding to a specific rubric level. Model Primitive Datatype = String. */
  points: string;
  /** Additional properties of the object */
  additionalProperties: { [index: string]: any };
}
