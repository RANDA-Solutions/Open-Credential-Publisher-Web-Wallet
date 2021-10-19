import { PdfRequestTypeEnum } from "./enums/pdfRequestTypeEnum";

export class PdfRequest {
  requestType: PdfRequestTypeEnum;
  linkId: string;
  clrId: number | null;
  assertionId: string;
  evidenceName: string;
  artifactId: number;
  artifactName: string;
  createLink: boolean;
}
