import { PdfShare } from "./pdfShare";
import { ShortLinkVM } from "./ShortLinkVM";


export class CredentialLinkVM {
  credentialPackageId: number;
  clrId: number;
  clrName: string;
  clrPublisherName: string;
  clrIssuedOn: Date;
  packageCreatedAt: Date;
  pdfs: PdfShare[];
  links: ShortLinkVM[];
}
