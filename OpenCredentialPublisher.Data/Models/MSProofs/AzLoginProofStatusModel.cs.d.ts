declare module server {
	interface azLoginProofStatusModel {
		id: number;
		requestId: string;
		code: string;
		state: string;
		json: string;
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
	}
}
