declare module server {
	interface invitationVM {
		id: number;
		hideQRCode: boolean;
		qRCodeString: string;
		nickname: string;
		wallet: server.walletRelationshipModel;
	}
}
