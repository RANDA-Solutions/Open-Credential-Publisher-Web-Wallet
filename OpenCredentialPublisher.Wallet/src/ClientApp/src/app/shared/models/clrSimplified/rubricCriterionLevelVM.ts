export class RubricCriterionLevelVM {
  rubricCriterionLevelId: number;
  order: number;
  /** IBaseEntity properties */
  isDeleted: boolean;
  createdAt: Date;
  modifiedAt: Date;
  id: string;
  type: string;
  description: string;
  level: string;
  name: string;
  points: string;
  additionalProperties: { [index: string]: any };
  resultDescriptionId: number;
};
