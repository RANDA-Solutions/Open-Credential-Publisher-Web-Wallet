import { PdfShare } from "./pdfShare";

export class LinkVM {
  id: string;
  nickname: string;
  displayCount: number;
  clrId: number;
  clrPublisherName: string;
  clrIssuedOn: Date;
  url: string;
  packageCreatedAt: Date;
  pdfs: PdfShare[];
}
