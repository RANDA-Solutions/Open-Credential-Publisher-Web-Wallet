import { VerificationDType } from "./clrLibrary/verificationDType";
import { PdfShare } from "./pdfShare";
import { PdfShareViewModel } from "./pdfShareViewModel";

export class LinkDisplayVMNew {
  id: string;
  showData: boolean;
  requiresAccessKey: boolean;
  clrId: number;
  accessKey: string;
  transcriptPdf: PdfShare;
  showDownloadPdfButton: boolean;
  showDownloadVCJsonButton: boolean;

  clrIdentifier: string;
  clrName: string;
  clrIsRevoked: boolean;
  learnerName: string;
  publisherName: string;
  clrIssuedOn: Date;
  verification: VerificationDType;
  //verificationType: string;
  //verificationCreator: string;
  pdfs: PdfShareViewModel[];
}
