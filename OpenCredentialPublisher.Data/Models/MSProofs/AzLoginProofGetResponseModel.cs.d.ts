declare module server {
	interface azLoginProofGetResponseModel {
		id: number;
		requestId: string;
		url: string;
		expiry: string;
		state: string;
		image: string;
		pin: string;
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
	}
}
