import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { AppService } from '@core/services/app.service';
import { environment } from '@environment/environment';
import { UntilDestroy } from '@ngneat/until-destroy';
import { LoginService } from '@root/app/auth/login.service';
import { UserDataResult } from 'angular-auth-oidc-client';
import { AuthenticatedResult } from 'angular-auth-oidc-client/lib/auth-state/auth-result';
import { Observable, of } from 'rxjs';

@UntilDestroy()
@Injectable()
export class AuthGuard implements CanActivate, CanLoad {
	isAuthenticated$: Observable<AuthenticatedResult>;
	private authResult: AuthenticatedResult;
  private userDataResult: UserDataResult;
  private debug = environment.debug;
	  constructor(private appService: AppService, private loginService: LoginService, private router: Router) {
		
	}
	
	canLoad(route: Route, segments: UrlSegment[]): Observable<boolean | UrlTree> {
		if (this.debug == true) console.log('AuthGuard canLoad segments: ', segments.join('/'));
		if (this.debug == true) console.log('AuthGuard canLoad url: ', this.router.url);
		if (this.appService.IsLoggedIn != true) {
      return of(this.router.createUrlTree(['/access/login'], { queryParams: { returnUrl: this.router.url }}));
    } else {
      return of(true);
    }
	}

	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> {
    const claimType: string = route.data['claimType'];
    if (this.debug) console.log(`AuthGuard - canActivate ${claimType}`);
    if (this.appService.IsLoggedIn != true) {
      if (this.debug == true) console.log('AuthGuard canActivate NOT isAuthenticated');
	  
	  if (this.router.url.includes("/connect/authorize/callback")) {
		return of(this.router.createUrlTree(['/access/login']));
	  }

      return of(this.router.createUrlTree(['/access/login'], { queryParams: { returnUrl: this.router.url }}));
    } else {
      if (this.debug == true) console.log('AuthGuard canActivate isAuthenticated');
      if (claimType) {
        return of(this.router.createUrlTree(['/access-denied']));
      } else {
        return of(true);
      }
    }
	}
	// isAuthenticated(url: string) : Observable<boolean | UrlTree> {
	// 	return this.loginService.isAuthenticated$.pipe(first(), map((data: AuthenticatedResult) => {
	// 		if (this.debug) console.log('AuthGuard - canActivate ', data);
	// 		this.authResult = data;
			
	// 		if (this.authResult?.isAuthenticated == true) {
	// 			if (this.debug) console.log('Token: ', this.loginService.token);
	// 			return true;
	// 		} else {
	// 			if (this.debug) console.log('AuthGuard Not authenticated, redirecting and adding redirect url...');
	// 			return this.router.createUrlTree(['/access/login'], { queryParams: { returnUrl: url }});
	// 		}
	// 	}));
	// }
}
