import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { environment } from '@environment/environment';
import { LoginService } from '@root/app/auth/auth.service';
import { AuthenticatedResult } from 'angular-auth-oidc-client/lib/auth-state/auth-result';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
@Injectable()
export class AuthGuard implements CanActivate {
	isAuthenticated$: Observable<AuthenticatedResult>;
	private authResult: AuthenticatedResult;
  private debug = false;
	constructor(private loginService: LoginService, private router: Router) {

	}

	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
		this.loginService.isAuthenticated$.pipe(tap((data: AuthenticatedResult) => {
			if (this.debug) console.log('AuthGuard - canActivate ', data);
			this.authResult = data;
		})).subscribe();

		if (this.authResult?.isAuthenticated == true) {
			if (this.debug) console.log('Token: ', this.loginService.token);
			return true;
		} else {
			if (this.debug) console.log('AuthGuard Not authenticated, redirecting and adding redirect url...');
			this.router.navigate(['/access/login'], { queryParams: { returnUrl: state.url }, replaceUrl: true });
			// this.authorizeService.doLogin();
			return false;
		}
	}
}
