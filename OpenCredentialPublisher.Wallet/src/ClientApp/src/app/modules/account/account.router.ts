import { RouterModule, Routes } from '@angular/router';
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
import { ShowRecoveryCodesComponent } from './pages/show-recovery-codes/show-recovery-codes.component';
import { TwoFactorAuthComponent } from './pages/two-factor-auth/two-factor-auth.component';

export const accountRoutes: Routes = [
  // {
  //     path: '',
  //     component: ProfileComponent,
  //     canActivate: [AuthGuard]
  // },
  {
    path: 'manage',
    component: ManageComponent,
    children: [
      {path: '', redirectTo: 'profile'},
      {path: 'profile', component: AccountProfileComponent},
      {path: 'profile-image', component: ProfileImageComponent},
      {path: 'email', component: EmailComponent},
      {path: 'password', component: ChangePasswordComponent},
      {path: 'two-factor-auth', component: TwoFactorAuthComponent},
      {path: 'disable-twofa', component: DisableTwoFAComponent},
      {path: 'enable-authenticator', component: EnableAuthenticatorComponent},
      {path: 'generate-recovery-codes', component: GenerateRecoveryCodesComponent},
      {path: 'show-recovery-codes', component: ShowRecoveryCodesComponent},
      {path: 'personal-data', component: PersonalDataComponent},
      {path: 'reset-authenticator', component: ResetAuthenticatorComponent},
    ]
  }
];

export const accountRouter = RouterModule.forChild(accountRoutes);
