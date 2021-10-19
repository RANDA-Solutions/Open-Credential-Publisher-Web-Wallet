import { ClrProfileVM } from "../clrProfileVM";
import { ProfileVM } from "./profileVM";
import { CriteriaVM } from "../criteriaVM";
import { ResultDescriptionVM } from "./resultDescriptionVM";

export class AchievementVM {
  achievementId: number;
  /** IBaseEntity properties */
  isDeleted: boolean;
  createdAt: Date;
  modifiedAt: Date;
  id: string;
  type: string;
  achievementType: string;
  creditsAvailable?: number;
  description: string;
  humanCode: string;
  identifiers: any[];
  name: string;
  fieldOfStudy: string;
  image: string;
  level: string;
  specialization: string;
  tags: string[];
  additionalProperties: { [index: string]: any };
  resultDescriptions:ResultDescriptionVM[];
  issuer: ProfileVM;
  requirement: CriteriaVM;
}
