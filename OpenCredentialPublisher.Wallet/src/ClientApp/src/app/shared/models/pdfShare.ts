export class PdfShare{
  clrId: number;
  clrEvidenceName: string;
  clrName: string;
  clrIssuedOn: Date;
  assertionId: string;
  artifactId: number;
  evidenceName: string;
  artifactName: string;
  artifactUrl: string;
  isUrl: boolean;
  isPdf: boolean;
  mediaType: string;
  constructor() {
    this.clrId = 0;
    this.clrEvidenceName = '';
    this.clrName= '';
    this.clrIssuedOn = null;
    this.assertionId= '';
    this.artifactId = 0;
    this.evidenceName= '';
    this.artifactName= '';
    this.artifactUrl= '';
    this.isUrl = false;
    this.isPdf = false;
    this.mediaType= '';
  }
}
