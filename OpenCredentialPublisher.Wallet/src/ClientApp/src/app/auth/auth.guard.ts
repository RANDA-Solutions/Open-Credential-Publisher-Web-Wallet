import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { environment } from '@environment/environment';
import { Observable, of } from 'rxjs';
import { AuthService } from './auth.service';
import { LoginService } from './login.service';

@Injectable({
	providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanLoad {
  private debug = environment.debug;
	  constructor(private authService: AuthService, private loginService: LoginService, private router: Router) {
		
	}
	
	canLoad(route: Route, segments: UrlSegment[]): Observable<boolean | UrlTree> {
		return of(this.authService.isLoggedIn());
	}

	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> {
    	const claimType: string = route.data['claimType'];
		if (this.authService.isLoggedIn()) {
			if (this.debug) {
				console.log("Claim Type: ", claimType);
			}
			if(claimType) {
				let claims = this.authService.getClaims();
				console.log(claims);
				this.loginService.storeReturnUrl(state.url);
				return of(false);
			}
			return of(true);
		}

		this.loginService.storeReturnUrl(state.url);
		return of(this.router.createUrlTree(["/access/login"]));
	}
}
