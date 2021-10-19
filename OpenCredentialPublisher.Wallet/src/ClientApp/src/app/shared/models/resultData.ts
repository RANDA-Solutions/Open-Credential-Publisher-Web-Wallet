import { ResultDescriptionVM } from "./clrSimplified/resultDescriptionVM";
import { RubricCriterionLevelVM } from "./clrSimplified/rubricCriterionLevelVM";

export class ResultData {
  resultDescription: ResultDescriptionVM;
  requiredLevel: RubricCriterionLevelVM;
  achievedLevel: RubricCriterionLevelVM;
  constructor(resultDescription: ResultDescriptionVM,
    requiredLevel: RubricCriterionLevelVM,
    achievedLevel: RubricCriterionLevelVM,){
      this.achievedLevel = achievedLevel;
      this.requiredLevel = requiredLevel;
      this.resultDescription = resultDescription;
    }
}
