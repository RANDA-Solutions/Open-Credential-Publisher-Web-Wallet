export class SearchResponse {
	searchTerm: string;
	records: number;
	credentials: CredentialSearch[];
}

export class CredentialSearch {
	id: number;
	credentialType: string;
	credentialName: string;
	credentialDescription: string;
	credentialNarrative: string;
}