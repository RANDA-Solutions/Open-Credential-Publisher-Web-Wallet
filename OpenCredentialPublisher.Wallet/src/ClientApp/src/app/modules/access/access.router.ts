import { RouterModule, Routes } from '@angular/router';
import { AccessComponent } from './access.component';
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
import { ResetPasswordConfirmationComponent } from './pages/reset-password/reset-password-confirmation.component';
import { ResetPasswordComponent } from './pages/reset-password/reset-password.component';

export const accessRoutes: Routes = [
  {
    path: '',
    component: AccessComponent,
    data: { hideNavBar: true },
    children: [
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
        path: 'email-verification', component: EmailVerificationComponent,
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
    ]
  }

];

export const accessRouter = RouterModule.forChild(accessRoutes);
