import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ChangePasswordComponent } from './components/change-password.component';
import { DeletePersonalDataComponent } from './components/delete-personal-data.component';
import { DisableTwoFactorComponent } from './components/disable-two-factor.component';
import { DownloadPersonalDataComponent } from './components/download-personal-data.component';
import { EmailComponent } from './components/email.component';
import { EnableAuthenticatorComponent } from './components/enable-authenticator.component';
import { ExternalLoginsComponent } from './components/external-logins.component';
import { GenerateRecoveryCodesComponent } from './components/generate-recovery-codes.component';
import { MainComponent } from './components/main.component';
import { PersonalDataComponent } from './components/personal-data.component';
import { ResetAuthenticatorComponent } from './components/reset-authenticator.component';
import { ShowRecoveryCodesComponent } from './components/show-recovery-codes.component';
import { TwoFactorAuthenticationComponent } from './components/two-factor-authentication.component';



@NgModule({
  declarations: [ChangePasswordComponent, DeletePersonalDataComponent, DisableTwoFactorComponent, DownloadPersonalDataComponent, EmailComponent, EnableAuthenticatorComponent, ExternalLoginsComponent, GenerateRecoveryCodesComponent, MainComponent, PersonalDataComponent, ResetAuthenticatorComponent, ShowRecoveryCodesComponent, TwoFactorAuthenticationComponent],
  imports: [
    CommonModule
  ]
})
export class AccountModule { }
