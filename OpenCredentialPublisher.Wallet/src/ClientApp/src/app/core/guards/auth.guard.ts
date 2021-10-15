import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { environment } from '@environment/environment';
import { LoginService } from '@root/app/auth/auth.service';
import { AuthenticatedResult } from 'angular-auth-oidc-client/lib/auth-state/auth-result';
import { Observable } from 'rxjs';
import { first, map } from 'rxjs/operators';
@Injectable()
export class AuthGuard implements CanActivate, CanLoad {
	isAuthenticated$: Observable<AuthenticatedResult>;
	private authResult: AuthenticatedResult;
  	private debug = environment.debug;
	constructor(private loginService: LoginService, private router: Router) {

	}
	
	canLoad(route: Route, segments: UrlSegment[]): Observable<boolean | UrlTree> {
		console.log('canLoad segments: ', segments.join('/'));
		console.log('canLoad url: ', this.router.url);
		return this.isAuthenticated(this.router.url);
	}

	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> {
		return this.isAuthenticated(state.url);
	}

	isAuthenticated(url: string) : Observable<boolean | UrlTree> {
		return this.loginService.isAuthenticated$.pipe(first(), map((data: AuthenticatedResult) => {
			if (this.debug) console.log('AuthGuard - canActivate ', data);
			this.authResult = data;
			
			if (this.authResult?.isAuthenticated == true) {
				if (this.debug) console.log('Token: ', this.loginService.token);
				return true;
			} else {
				if (this.debug) console.log('AuthGuard Not authenticated, redirecting and adding redirect url...');
				return this.router.createUrlTree(['/access/login'], { queryParams: { returnUrl: url }});
			}
		}));
	}
}
