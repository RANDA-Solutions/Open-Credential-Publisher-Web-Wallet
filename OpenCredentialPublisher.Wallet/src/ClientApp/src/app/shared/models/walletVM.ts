export class WalletVM {
  id: number;
  walletName: string;
  relationshipDid: string;
  relationshipVerKey: string;
  inviteUrl: string;
  isConnected: boolean;
  agentContextId: string;
  userId: string;
  createdOn: Date;
  modifiedOn?: Date;
  credentialsSent: number;
}
