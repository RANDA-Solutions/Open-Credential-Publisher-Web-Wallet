import { baseEnvironment } from "./ienvironment";

const baseUrl = 'https://localhost:44392';
export const environment: baseEnvironment  = {
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
  logoutTimer: 5 // in minutes
  ,
  showMicrosoftLogin: true,
  showWallets: false
};
