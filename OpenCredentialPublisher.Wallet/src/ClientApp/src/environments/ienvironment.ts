export interface baseEnvironment {
	name: string;
	production: boolean;
	debug: boolean;
	errorMessageLife: number;
	httpDelay: number;
	apiEndPoint: string;
	publicEndPoint: string;
	baseUrl: string;
	secureRoutes: string[];
  hubConnectionStatusEndpoint: string;
  hubCredentialsStatusEndpoint: string;
  hubProofStatusEndpoint: string;
  configId: string;
  allowSelfEmailConfirmation: boolean;
  logoutTimer: number;
  showMicrosoftLogin: boolean;
}
