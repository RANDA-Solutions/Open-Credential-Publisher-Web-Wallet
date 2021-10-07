export class TwoFactorEnableAuthenticationVM
{
  sharedKey: string;
  authenticatorUri: string;
  recoveryCodes: Array<string>;
  statusMessage: string;
  code: string;
}
