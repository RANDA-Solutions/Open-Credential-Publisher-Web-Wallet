import { WalletConnectionStatus } from "./walletConnectionStatus";

export class CredentialSendStatus extends WalletConnectionStatus{
  packageId: number;
  error: boolean;
  revoked: boolean;
  constructor (relationshipId: number, packageId: number, status: string, done: boolean, error: boolean, revoked: boolean) {
    super(relationshipId, status, done);
    this.packageId = packageId;
    this.error = error;
    this.revoked = revoked;
  }
}
