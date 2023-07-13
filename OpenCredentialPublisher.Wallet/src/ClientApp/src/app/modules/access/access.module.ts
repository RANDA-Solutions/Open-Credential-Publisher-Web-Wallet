import { ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LogService } from '@core/error-handling/logerror.service';
import { SharedModule } from '@shared/shared.module';
import { AccessComponent } from './access.component';
import { accessRouter } from './access.router';
import { AzEmailCredentialComponent } from './pages/az-email-credential/az-email-credential.component';
import { AzLoginWithProofComponent } from './pages/az-login-with-proof/az-login-with-proof.component';
import { CodeClaimComponent } from './pages/code-claim/code-claim.component';
import { CodeCredentialComponent } from './pages/code-credential/code-credential.component';
import { CodeExpiredComponent } from './pages/code-expired/code-expired.component';
import { CodeInvalidComponent } from './pages/code-invalid/code-invalid.component';
import { CodeWaitingComponent } from './pages/code-waiting/code-waiting.component';
import { ConfirmEmailChangeComponent } from './pages/confirm-email-change/confirm-email-change.component';
import { EmailConfirmationComponent } from './pages/email-confirmation/email-confirmation.component';
import { EmailCredentialComponent } from './pages/email-credential/email-credential.component';
import { EmailVerificationComponent } from './pages/email-verification/email-verification.component';
import { ForgotPasswordConfirmationComponent } from './pages/forgot-password/forgot-password-confirmation.component';
import { ForgotPasswordComponent } from './pages/forgot-password/forgot-password.component';
import { LoginFormComponent } from './pages/login-form/login-form.component';
import { LoginWithProofComponent } from './pages/login-with-proof/login-with-proof.component';
import { LoginWithRecoveryComponent } from './pages/login-with-recovery/login-with-recovery.component';
import { LogoutComponent } from './pages/logout/logout.component';
import { RegisterAccountComponent } from './pages/register-account/register.component';
import { RegisterConfirmationComponent } from './pages/register-confirmation/register-confirmation.component';
import { ResendConfirmationComponent } from './pages/resend-confirmation/resend-confirmation.component';
import { ResetPasswordConfirmationComponent } from './pages/reset-password/reset-password-confirmation.component';
import { ResetPasswordComponent } from './pages/reset-password/reset-password.component';
import { SetPasswordConfirmationComponent } from './pages/set-password/set-password-confirmation.component';
import { SetPasswordComponent } from './pages/set-password/set-password.component';
import { AccessService } from './services/access.service';
import { AzLoginProofService } from './services/az-login-proof.service';
import { CodeCredentialResolver } from './services/code-credential-resolver.service';
import { CodeResolver } from './services/code-resolver.service';
import { CodeService } from './services/code.service';
import { EmailVerificationService } from './services/email-verification.service';
import { LoginProofService } from './services/login-proof.service';
import { ResetPasswordResolver } from './services/reset-password-resolver.service';

@NgModule({
    imports: [
      accessRouter,
      SharedModule,
      FormsModule,
      ReactiveFormsModule
    ],
    declarations: [
      AccessComponent,
      LoginFormComponent,
      LogoutComponent,
      ConfirmEmailChangeComponent,
      ForgotPasswordComponent,
      ForgotPasswordConfirmationComponent,
      ResendConfirmationComponent,
      ResetPasswordComponent,
      ResetPasswordConfirmationComponent,
      RegisterAccountComponent,
      SetPasswordComponent,
      SetPasswordConfirmationComponent,
      EmailConfirmationComponent,
      RegisterConfirmationComponent,
      LoginWithRecoveryComponent,
      EmailVerificationComponent,
      EmailCredentialComponent,
      LoginWithProofComponent,
      AzLoginWithProofComponent,
      AzEmailCredentialComponent,
      CodeClaimComponent,
      CodeCredentialComponent,
      CodeExpiredComponent,
      CodeInvalidComponent,
      CodeWaitingComponent,
      ],
      providers: [ AccessService, CodeService, CodeResolver, CodeCredentialResolver, ResetPasswordResolver, LogService, LoginProofService, AzLoginProofService, EmailVerificationService ],
      entryComponents: [
      ],
  })
export class AccessModule {
  static forRoot(): ModuleWithProviders<AccessModule> {
      return {
        ngModule: AccessModule
      };
    }
  }
