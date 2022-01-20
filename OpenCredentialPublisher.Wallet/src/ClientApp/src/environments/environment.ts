// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.
const baseUrl = 'https://localhost:44392';
export const environment = {
	name: '[default]',
	production: false,
	debug: true,
	errorMessageLife: 3000,
	httpDelay: 0,
	apiEndPoint: `${baseUrl}/api/`,
	publicEndPoint: `${baseUrl}/public/`,
	baseUrl: baseUrl,
	secureRoutes: [`${baseUrl}/api`],
  hubConnectionStatusEndpoint: `${baseUrl}/hubs/connection`,
  hubCredentialsStatusEndpoint: `${baseUrl}/hubs/credentials`,
  hubProofStatusEndpoint: `${baseUrl}/hubs/proofrequests`,
  configId: '0-ocp-wallet-client',
  allowSelfEmailConfirmation: true,
  logoutTimer: 10// in minutes

};

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
