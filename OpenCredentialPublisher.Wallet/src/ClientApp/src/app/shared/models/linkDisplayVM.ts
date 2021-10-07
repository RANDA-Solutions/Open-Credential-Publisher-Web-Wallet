import { clrViewModel } from "./legacyClrVM";
import { PdfShare } from "./pdfShare";

export class LinkDisplayVM {
  id: string;
  showData: boolean;
  requiresAccessKey: boolean;
  clrId: number;
  accessKey: string;
  transcriptPdf: PdfShare;
  clrVM: clrViewModel;
  showDownloadPdfButton: boolean;
  showDownloadVCJsonButton: boolean;
}
