import { AddressDType } from "./addressDType";
import { AlignmentDType } from "./alignmentDType";
import { CriteriaDType } from "./criteriaDType";
import { EndorsementDType } from "./endorsementDType";
import { ProfileDType } from "./profileDType";
import { ResultDescriptionDType } from "./resultDescriptionDType";

/** An accomplishment such as completing a degree, mastering a competency, or course completion that may be asserted about one or more learners. */
export class AchievementDType {
  /** Unique IRI for the Achievement. If the id is a URL it must be the location of an Achievement document. Model Primitive Datatype = NormalizedString. */
  id: string;
  /** The JSON-LD type of this object. Normally 'Achievement'. Model Primitive Datatype = NormalizedString. */
  type: string;
  /** A CLR Achievement can represent many different types of achievement from an assessment result to membership. Use 'Achievement' to indicate an achievement not in the list of allowed values. */
  achievementType: string;
  alignments: AlignmentDType[];
  associations: any[];
  /** Credit hours associated with this entity, or credit hours possible. For example '3.0'. Model Primitive Datatype = Float. */
  creditsAvailable?: number;
  /** A short description of the achievement. May be the same as name if no description is available. Model Primitive Datatype = String. */
  description: string;
  endorsements: EndorsementDType[];
  /** The code, generally human readable, associated with an achievement. Model Primitive Datatype = String. */
  humanCode: string;
  identifiers: any[];
  /** The name of the achievement. Model Primitive Datatype = String. */
  name: string;
  /** Category, subject, area of study,  discipline, or general branch of knowledge. Examples include Business, Education, Psychology, and Technology.  Model Primitive Datatype = String. */
  fieldOfStudy: string;
  /** IRI of an image representing the achievement. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
  image: string;
  issuer: ProfileDType;
  /** Text that describes the level of achievement apart from how the achievement was performed or demonstrated. Examples would include 'Level 1', 'Level 2', 'Level 3', or 'Bachelors', 'Masters', 'Doctoral'. Model Primitive Datatype = String. */
  level: string;
  requirement: CriteriaDType;
  resultDescriptions: ResultDescriptionDType[];
  signedEndorsements: string[];
  /** Name given to the focus, concentration, or specific area of study defined in the achievement. Examples include Entrepreneurship, Technical Communication, and Finance. Model Primitive Datatype = String. */
  specialization: string;
  tags: string[];
  additionalProperties: { [index: string]: any };
}
