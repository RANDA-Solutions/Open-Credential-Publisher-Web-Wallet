import { PackageTypeEnum } from "./enums/packageTypeEnum";
import { PdfShare } from "./pdfShare";

export class PackageVM {
  id: number;
  isOwner: boolean;
  typeId: PackageTypeEnum;
  assertionCount: number;
  createdAt: Date;
  modifiedAt: Date;
  showDownloadPdfButton: boolean;
  showDownloadVCJsonButton: boolean;
  clrIds: Array<number>
  newestPdfTranscript: PdfShare;
  constructor () {
    this.id = 0;
    this.typeId = 0;
    this.assertionCount = 0;
    this.createdAt = null;
    this.modifiedAt = null;
    this.isOwner = false;
    this.showDownloadPdfButton = true;
    this.showDownloadVCJsonButton = true;
    this.clrIds = new Array<number>();
    this.newestPdfTranscript = null;
  }
}
