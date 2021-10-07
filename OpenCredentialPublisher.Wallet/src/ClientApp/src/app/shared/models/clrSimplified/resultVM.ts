export class ResultVM {
  resultId: number;
  order: number;
  isDeleted: boolean;
  createdAt: Date;
  modifiedAt: Date;
  id: string;
  type: string;
  achievedLevel: string;
  resultDescription: string;
  status: string;
  value: string;
  additionalProperties: { [index: string]: any };
  assertionId: number;
}
