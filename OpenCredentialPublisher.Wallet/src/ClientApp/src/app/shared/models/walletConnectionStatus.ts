export class WalletConnectionStatus {
  relationshipId: number;
  status: string;
  done: boolean;
  constructor (relationshipId: number, status: string, done: boolean) {
    this.relationshipId = relationshipId;
    this.status = status;
    this.done = done;
  }
}
