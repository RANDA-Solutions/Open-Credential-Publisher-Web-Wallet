import { baseEnvironment } from "./ienvironment";

const baseUrl = 'https://ocp-wallet-dev.azurewebsites.net';
export const environment: baseEnvironment = {
  production: true,
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
  logoutTimer: 5 // in minutes
  ,
  name: "[default]",
  showMicrosoftLogin: true,
  showWallets: false
};
