declare module server {
	interface openBadgeVM {
		id: number;
		badgrAssertionId: string;
		badgeName: string;
		issuerName: string;
		badgeDescription: string;
		badgeImage: string;
		idIsUrl: boolean;
	}
}
