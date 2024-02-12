import { baseEnvironment } from "./ienvironment";

const baseUrl = window.location.origin;
export const environment: baseEnvironment  = {
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
  configId: '0-ocp-wallet-client',
  allowSelfEmailConfirmation: true,
  logoutTimer: 10 // in minutes
  ,
  showMicrosoftLogin: true,
  showWallets: false
};
