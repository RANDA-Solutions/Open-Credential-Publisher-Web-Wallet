import { RouterModule, Routes } from '@angular/router';
import { AccessComponent } from './access.component';
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
import { LoginWith2faComponent } from './pages/login-with2fa/login-with2fa.component';
import { LogoutComponent } from './pages/logout/logout.component';
import { RegisterAccountComponent } from './pages/register-account/register.component';
import { RegisterConfirmationComponent } from './pages/register-confirmation/register-confirmation.component';
import { ResendConfirmationComponent } from './pages/resend-confirmation/resend-confirmation.component';
import { ResetPasswordConfirmationComponent } from './pages/reset-password/reset-password-confirmation.component';
import { ResetPasswordComponent } from './pages/reset-password/reset-password.component';
import { CodeCredentialResolver } from './services/code-credential-resolver.service';
import { CodeResolver } from './services/code-resolver.service';

export const accessRoutes: Routes = [
  {
    path: '',
    component: AccessComponent,
    data: { hideNavBar: true },
    children: [
      {
        path: 'resend-confirmation', component: ResendConfirmationComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'forgot-password', component: ForgotPasswordComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'forgot-password-confirmation', component: ForgotPasswordConfirmationComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'reset-password', component: ResetPasswordComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'reset-password-confirmation', component: ResetPasswordConfirmationComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'email-confirmation', component: EmailConfirmationComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'email-credential/:key', component: EmailCredentialComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'az-email-credential/:key', component: AzEmailCredentialComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'email-verification/:type', component: EmailVerificationComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'register-confirmation', component: RegisterConfirmationComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'login', component: LoginFormComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'login-with-proof', component: LoginWithProofComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'az-login-with-proof', component: AzLoginWithProofComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'login-with2fa', component: LoginWith2faComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'login-with-recovery', component: LoginWithRecoveryComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'logout', component: LogoutComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'register', component: RegisterAccountComponent,
        data: { hideNavBar: true }
      },
      {path: 'confirm-email-change', component: ConfirmEmailChangeComponent},
      {
        path: '',
        component: LoginFormComponent,
        data: { hideNavBar: true }
      }
    ]
  },
  {
    path: 'code',
    component: AccessComponent,
    data: { hideNavBar: true },
    children: [
      {
        path: 'claim/:code',
        component: CodeClaimComponent,
        resolve: { response: CodeResolver },
        data: { hideNavBar: true }
      },
      {
        path: 'waiting',
        component: CodeWaitingComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'invalid',
        component: CodeInvalidComponent,
        data: { hideNavBar: true }
      },
      {
        path: 'credential/:code',
        component: CodeCredentialComponent,
        resolve: { response: CodeCredentialResolver },
        data: { hideNavBar: true }
      },
      {
        path: 'expired',
        component: CodeExpiredComponent,
        data: { hideNavBar: true }
      }
    ]
  }

];

export const accessRouter = RouterModule.forChild(accessRoutes);
