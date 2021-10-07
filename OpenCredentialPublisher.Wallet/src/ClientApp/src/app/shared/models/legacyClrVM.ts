import { ApplicationUser } from "./applicationUser";
import { ClrDType } from "./clrLibrary/ClrDType";
import { ClrModel } from "./clrModel";
import { PackageTypeEnum } from "./enums/packageTypeEnum";
import { PdfShare } from "./pdfShare";

export class clrViewModel {
  clr: ClrModel;
  rawClrDType: ClrDType;
  hasPdf: boolean;
  ancestorCredentialPackage: {
  /** Primary key */
    id: number;
    typeId: PackageTypeEnum;
    revoked: boolean;
    revocationReason: string;
  /** Foreign key back to the authorization. */
    authorizationForeignKey: string;
    authorization: any;
    clrSet: any;
    badgrBackpack: any;
    verifiableCredential: any;
    name: string;
  /** This Package is tied to a specific application user. */
    userId: string;
    user: ApplicationUser;
    isDeleted: boolean;
    createdAt: Date;
    modifiedAt: Date;
    containedClrs: any[];
  };
  allAssertions: any[];
  parentAssertions: any[];
  pdfs: PdfShare[];
}
