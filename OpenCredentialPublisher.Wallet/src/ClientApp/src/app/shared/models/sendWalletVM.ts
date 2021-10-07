import { ConnectionViewModel } from "./connectionViewModel";
import { WalletCredentialVM } from "./walletCredentialVM";

export class SendWalletVM {
  connection: ConnectionViewModel;
  credentials: WalletCredentialVM[];
}
