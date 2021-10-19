export class EndorsementClaimVM {
  endorsementClaimId: number;
  /** IBaseEntity properties */
  isDeleted: boolean;
  createdAt: Date;
  modifiedAt: Date;
  id: string;
  type: string;
  endorsementComment: string;
  additionalProperties: { [index: string]: any };
}
