import { NgModule } from '@angular/core';
import { ExtraOptions, RouterModule, Routes } from '@angular/router';
import { AutoLoginPartialRoutesGuard } from 'angular-auth-oidc-client';
import { LoginCallbackComponent } from './auth/login-callback.component';
import { LoginCallbackResolver } from './auth/login-callback.resolver';
import { SourcesCallbackComponent } from './components/sources-callback/sources-callback.component';
import { SourcesErrorComponent } from './components/sources-error/sources-error.component';
import { CallbackResolverService } from './resolvers/callback-resolver.service';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';

const routerOptions: ExtraOptions = {
  useHash: false,
  anchorScrolling: 'enabled',
};

export const appRoutes: Routes = [
  {
   path: 'credentials',
   loadChildren: () => import('./modules/credentials/credentials.module').then(m =>m.CredentialsModule),
   // canLoad: [AuthGuard],
   canActivate: [AutoLoginPartialRoutesGuard]
  },
  {
   path: 'sources',
   loadChildren: () => import('./modules/sources/sources.module').then(m =>m.SourcesModule),
   // canLoad: [AutoLoginPartialRoutesGuard],
   canActivate: [AutoLoginPartialRoutesGuard]
  },
  {
    path: 'links',
    loadChildren: () => import('./modules/links/links.module').then(m =>m.LinksModule),
    // canLoad: [AutoLoginPartialRoutesGuard],
    canActivate: [AutoLoginPartialRoutesGuard]
  },
  {
    path: 'Links',
    loadChildren: () => import('./modules/links/links.module').then(m =>m.LinksModule),
    // canLoad: [AutoLoginPartialRoutesGuard],
    canActivate: [AutoLoginPartialRoutesGuard]
  },
  {
    path: 'wallets',
    loadChildren: () => import('./modules/wallets/wallets.module').then(m =>m.WalletsModule),
    // canLoad: [AutoLoginPartialRoutesGuard],
    canActivate: [AutoLoginPartialRoutesGuard]
  },
  {
    path: 'account',
    loadChildren: () => import('./modules/account/account.module').then(m =>m.AccountModule),
    // canLoad: [AutoLoginPartialRoutesGuard],
    canActivate: [AutoLoginPartialRoutesGuard]
  },

  /****************************************************
   * Unprotected routes                               *
   ****************************************************/
  {
    path: 'public',
    loadChildren: () => import('./modules/public/public.module').then(m =>m.PublicModule)
  },
  {
    path: 'Public',
    loadChildren: () => import('./modules/public/public.module').then(m =>m.PublicModule)
  },
  {
    path: 's',
    loadChildren: () => import('./modules/public/public.module').then(m =>m.PublicModule)
  },
  {
    path: 'access',
    loadChildren: () => import('./modules/access/access.module').then(m =>m.AccessModule)
  },
  {
    path: 'Access',
    loadChildren: () => import('./modules/access/access.module').then(m =>m.AccessModule)
  },
  {
    path: 'Verifier',
    loadChildren: () => import('./modules/verifier/verifier.module').then(m =>m.VerifierModule)
  },
  {
    path: 'verifier',
    loadChildren: () => import('./modules/verifier/verifier.module').then(m =>m.VerifierModule)
  },
  {
    path: 'search',
    loadChildren: () => import('./modules/search/search.module').then(m =>m.SearchModule)
  },
  {
    path: 'Search',
    loadChildren: () => import('./modules/search/search.module').then(m =>m.SearchModule)
  },
  {
      path: 'unauthorized',
      component: UnauthorizedComponent
  },
  {
      path: 'callback',
      resolve: { result: LoginCallbackResolver },
      component: LoginCallbackComponent
  },
  {
      path: 'sources-callback',
      component: SourcesCallbackComponent,
      resolve: { response: CallbackResolverService }
  },
  {
      path: 'sources-error',
      component: SourcesErrorComponent
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'credentials'
  },
];

@NgModule({
	imports: [RouterModule.forRoot(appRoutes, routerOptions)],
	exports: [RouterModule]
})
export class AppRoutingModule { }
