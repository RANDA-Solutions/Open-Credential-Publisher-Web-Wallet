declare module server {
	interface sendWalletVM {
		invitation: server.invitationVM;
		credentials: server.walletCredentialVM[];
	}
}
