import { RouterModule, Routes } from '@angular/router';
import { ContactusComponent } from './pages/contactus/contactus.component';
import { LinkDisplayComponent } from './pages/link-display/link-display.component';
import { LockoutComponent } from './pages/lockout/lockout.component';
import { PrivacyComponent } from './pages/privacy/privacy.component';
import { TermsComponent } from './pages/terms/terms.component';

export const publicRoutes: Routes = [
  {
    path: 'Links/Display/:id',
    component: LinkDisplayComponent,
    data: {hideNavBar: true}
  },

  {path: 'contactus', component: ContactusComponent},
  {path: 'privacy', component: PrivacyComponent},
  {path: 'terms', component: TermsComponent},
  {path: 'lockout', component: LockoutComponent},
  {
    path: ':id',
    component: LinkDisplayComponent,
    data: {hideNavBar: true}
  },
];

export const publicRouter = RouterModule.forChild(publicRoutes);
