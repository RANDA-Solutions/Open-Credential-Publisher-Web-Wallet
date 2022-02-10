import { VerificationDType } from "./clrLibrary/verificationDType";

export class VerifyVM {
  clrId: number;
  assertionId: string;
  clrIdentifier: string;
  endorsementId: string;
  verificationDType: VerificationDType;
  ancestors: string;
  ancestorKeys: string;
}

