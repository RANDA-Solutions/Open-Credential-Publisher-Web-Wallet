declare module server {
	interface profileModel {
		hasProfileImage: boolean;
		profileImageUrl: string;
		displayName: string;
		missingDisplayName: boolean;
		credentials: number;
		achievements: number;
		scores: number;
		activeLinks: number;
		additionalData: { [index: string]: string };
	}
}
