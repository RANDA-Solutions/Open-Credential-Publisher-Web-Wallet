import { ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LogService } from '@core/error-handling/logerror.service';
import { SharedModule } from '@shared/shared.module';
import { accountRouter } from './account.router';
import { AccountService } from './account.service';
import { ManageComponent } from './manage.component';
import { ChangePasswordComponent } from './pages/change-password/change-password.component';
import { DisableTwoFAComponent } from './pages/disable-twofa/disable-twofa.component';
import { EmailComponent } from './pages/email/email.component';
import { EnableAuthenticatorComponent } from './pages/enable-authenticator/enable-authenticator.component';
import { GenerateRecoveryCodesComponent } from './pages/generate-recovery-codes/generate-recovery-codes.component';
import { PersonalDataComponent } from './pages/personal-data/personal-data.component';
import { ProfileImageComponent } from './pages/profile-image/profile-image.component';
import { AccountProfileComponent } from './pages/profile/accountprofile.component';
import { ResetAuthenticatorComponent } from './pages/reset-authenticator/reset-authenticator.component';
import { TwoFactorAuthComponent } from './pages/two-factor-auth/two-factor-auth.component';
import { LoginWith2faComponent } from '../access/pages/login-with2fa/login-with2fa.component';
import { ShowRecoveryCodesComponent } from './pages/show-recovery-codes/show-recovery-codes.component';

@NgModule({
    imports: [
      SharedModule,
      FormsModule,
      ReactiveFormsModule,
      accountRouter,
    ],
    declarations: [
      ManageComponent,
      AccountProfileComponent,
      ProfileImageComponent,
      EmailComponent,
      ChangePasswordComponent,
      PersonalDataComponent,
      GenerateRecoveryCodesComponent,
      ShowRecoveryCodesComponent,
      DisableTwoFAComponent,
      EnableAuthenticatorComponent,
      ResetAuthenticatorComponent,
      TwoFactorAuthComponent,
      LoginWith2faComponent,
      ],
      providers: [ AccountService, LogService ],
      entryComponents: [
      ],
  })
export class AccountModule {
  static forRoot(): ModuleWithProviders<AccountModule> {
      return {
        ngModule: AccountModule
      };
    }
  }
