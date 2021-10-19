import { AssociationTypeEnum } from "../clrLibrary/enums/associationTypeEnum";

export class AssociationVM {
  associationId: number;
  /** IBaseEntity properties */
  isDeleted: boolean;
  createdAt: Date;
  modifiedAt: Date;
  associationType: AssociationTypeEnum;
  targetId: string;
  title: string;
  additionalProperties: { [index: string]: any };
  uri: string;
  targetAchievementName: string;
}
