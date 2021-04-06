import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ApplicationPaths } from './api-authorization.constants';
import { ConfirmEmailChangeComponent } from './components/confirm-email/confirm-email-change.component';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';
import { AccessDeniedComponent } from './components/errors/access-denied.component';
import { LockoutComponent } from './components/errors/lockout.component';
import { ExternalLoginComponent } from './components/external-login/external-login.component';
import { ForgotPasswordConfirmationComponent } from './components/forgot-password/forgot-password-confirmation.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { LoginMenuComponent } from './components/login-menu/login-menu.component';
import { LoginComponent } from './components/login/login.component';
import { RecoveryCodesComponent } from './components/login/recovery-codes.component';
import { TwoFactorAuthenticationComponent } from './components/login/two-factor-authentication.component';
import { LogoutComponent } from './components/logout/logout.component';
import { RegisterConfirmationComponent } from './components/register/register-confirmation.component';
import { RegisterComponent } from './components/register/register.component';
import { ResetPasswordConfirmationComponent } from './components/reset-password/reset-password-confirmation.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    RouterModule.forChild(
      [
        { path: ApplicationPaths.Register, component: LoginComponent },
        { path: ApplicationPaths.Profile, component: LoginComponent },
        { path: ApplicationPaths.Login, component: LoginComponent },
        { path: ApplicationPaths.LoginFailed, component: LoginComponent },
        { path: ApplicationPaths.LoginCallback, component: LoginComponent },
        { path: ApplicationPaths.LogOut, component: LogoutComponent },
        { path: ApplicationPaths.LoggedOut, component: LogoutComponent },
        { path: ApplicationPaths.LogOutCallback, component: LogoutComponent }
      ]
    )
  ],
  declarations: [LoginMenuComponent, LoginComponent, LogoutComponent, AccessDeniedComponent, LockoutComponent, ConfirmEmailComponent, ConfirmEmailChangeComponent, ExternalLoginComponent, ForgotPasswordComponent, ForgotPasswordConfirmationComponent, TwoFactorAuthenticationComponent, RecoveryCodesComponent, RegisterComponent, RegisterPasswordComponent, RegisterConfirmationComponent, ResetPasswordComponent, ResetPasswordConfirmationComponent],
  exports: [LoginMenuComponent, LoginComponent, LogoutComponent]
})
export class ApiAuthorizationModule { }
