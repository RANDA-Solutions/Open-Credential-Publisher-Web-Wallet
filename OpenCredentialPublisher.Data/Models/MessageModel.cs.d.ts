declare module server {
	interface messageModel {
		id: number;
		recipient: string;
		body: string;
		subject: string;
		sendAttempts: number;
		statusId: any;
		createdOn: Date;
		shareId?: number;
		share: server.shareModel;
	}
}
