import { AlignmentModel } from "./alignmentModel";

export class ResultModel {
  /** Primary key. */
  resultId: number;
  order: number;
  /** IBaseEntity properties */
  isDeleted: boolean;
  createdAt: Date;
  modifiedAt: Date;
  /** Unique IRI for the object. Model Primitive Datatype = NormalizedString. */
  id: string;
  /** The JSON-LD type of this object. Normally 'Result'. Model Primitive Datatype = NormalizedString. */
  type: string;
  /** The id of the RubricCriterionLevel achieved. Model Primitive Datatype = NormalizedString. */
  achievedLevel: string;
  /** The id of the ResultDescription describing this result. Model Primitive Datatype = NormalizedString. */
  resultDescription: string;
  status: string;
  /** A grade or value representing the result of the performance, or demonstration, of the achievement.  For example, 'A' if the recipient received a grade of A in the class.  Model Primitive Datatype = String. */
  value: string;
  /** Additional properties of the object */
  additionalProperties: { [index: string]: any };
  /** End From ResultDTypeForeignKeys */
  assertionId: number;
  /** Relationships */
  resultAlignments: AlignmentModel[];
}
