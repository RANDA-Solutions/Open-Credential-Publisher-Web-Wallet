import { PdfShare } from "./pdfShare";

export class Dashboard {
  showShareableLinksSection: boolean;
  showLatestShareableLink: boolean;
  showNewestPdfTranscript: boolean;
  newestPdfTranscript: PdfShare;
  constructor() {
    this.showShareableLinksSection = false;
    this.showLatestShareableLink = false;
    this.showNewestPdfTranscript = false;
    this.newestPdfTranscript = new PdfShare();
  }
}
