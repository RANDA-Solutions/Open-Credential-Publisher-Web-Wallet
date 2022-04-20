import { baseEnvironment } from "./ienvironment";

const baseUrl = 'https://ocp.local.randasolutions.com';
export const environment: baseEnvironment  = {
  name: '[default]',
  production: true,
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
  logoutTimer: 1 // in minutes
  ,
  debug: false,
  showMicrosoftLogin: true
};
