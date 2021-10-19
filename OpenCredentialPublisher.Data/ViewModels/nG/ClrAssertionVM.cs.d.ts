declare module server {
	interface clrAssertionVM {
		id: number;
		achievementName: string;
		achievementType: string;
		achievementIssuerName: string;
		issuedOn?: Date;
		isCollapsed: boolean;
		achievementResults: server.achievementResult[];
	}
	interface achievementResult {
		value: string;
		name: string;
	}
}
