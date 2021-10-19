declare module server {
	interface linkShareVM {
		linkId: string;
		linkNickname: string;
		recipientId?: number;
		recipients: any[];
	}
}
