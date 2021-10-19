declare module server {
	interface shareModel {
		id: number;
		linkId: string;
		shareTypeId: server.shareTypeEnum;
		recipientId?: number;
		accessKey: string;
		useCount: number;
		createdOn: Date;
		statusId: any;
		messages: server.messageModel[];
		recipient: {
			id: number;
			userId: string;
			user: server.applicationUser;
			name: string;
			email: string;
			createdOn: Date;
		};
		link: server.linkModel;
	}
	const enum shareTypeEnum {
		email = 1,
		pdf = 2,
		wallet = 3,
	}
}
