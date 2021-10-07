declare module server {
	interface clrVM {
		id: number;
		packageId: number;
		name: string;
		publisherName: string;
		isRevoked: boolean;
		issuedOn: Date;
	}
}
