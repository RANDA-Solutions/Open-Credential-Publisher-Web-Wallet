import { TwoFactorAuthenticationResultEnum } from "./enums/twoFactorAuthenticationResultEnum";
import { TwoFactorAuthenticationModelInput } from "./TwoFactorAuthModelInput";

export class TwoFactorAuthenticationModel {
  result?: TwoFactorAuthenticationResultEnum;
  externalLogins: any[];
  errorMessage: string;
  returnUrl: string;
  inputModel: TwoFactorAuthenticationModelInput;
  hasError: boolean;
  errorMessages: string[];
  email: string;
}
