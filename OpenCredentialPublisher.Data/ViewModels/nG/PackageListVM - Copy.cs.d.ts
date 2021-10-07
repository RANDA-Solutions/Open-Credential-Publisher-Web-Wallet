declare module server {
	interface packageListVM {
		modelIsValid: boolean;
		enableSource: boolean;
		enableCollections: boolean;
		modelErrors: string[];
		userId: string;
		packages: server.packageVM[];
	}
}
