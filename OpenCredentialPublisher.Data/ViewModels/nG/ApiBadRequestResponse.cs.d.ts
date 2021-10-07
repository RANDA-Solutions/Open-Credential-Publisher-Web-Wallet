declare module server {
	interface apiBadRequestResponse extends apiResponse {
		errors: string[];
	}
}
