
const baseUrl = 'https://ocp-wallet-uat.azurewebsites.net';
export const environment = {
	name: '[default]',
	production: true,
	debug: false,
	errorMessageLife: 3000,
	httpDelay: 0,
	apiEndPoint: `${baseUrl}/api/`,
	publicEndPoint: `${baseUrl}/public/`,
	baseUrl: baseUrl,
	secureRoutes: [`${baseUrl}/api`],
  hubConnectionStatusEndpoint: `${baseUrl}/hubs/connection`,
  hubCredentialsStatusEndpoint: `${baseUrl}/hubs/credentials`,
  hubProofStatusEndpoint: `${baseUrl}/hubs/proofrequests`,
  configId: '0-ocp-wallet-client'

};