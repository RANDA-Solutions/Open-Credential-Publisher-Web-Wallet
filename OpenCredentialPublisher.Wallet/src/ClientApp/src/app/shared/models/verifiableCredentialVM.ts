import { Proof } from "./clrLibrary/proof";

export class VerifiableCredentialVM {
  id: number;
  identifier: string;
  issuer: string;
  issuedOn: Date;
  credentialsCount: number;
  proof: Proof;
  clrIds: number[];
}
