import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { environment } from '@environment/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { LoginService } from './login.service';

@Injectable({
	providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  private debug = environment.debug;
	  constructor(private authService: AuthService, private loginService: LoginService, private router: Router) {
		
	}
	
	// canLoad(route: Route, segments: UrlSegment[]): Observable<boolean | UrlTree> {
	// 	return of(this.authService.isLoggedIn());
	// }

	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> {
    	const claimType: string = route.data['claimType'];
		if (this.debug) {
			console.log("Hit Auth Guard");
		}
		
		return this.authService.isLoggedIn().pipe(map((isLoggedIn) => {
			if (isLoggedIn) {
				if (this.debug) {
					console.log("Claim Type: ", claimType);
				}
				if(claimType) {
					let claims = this.authService.getClaims();
					console.log(claims);
					this.loginService.storeReturnUrl(state.url);
					return false;
				}
				return true;
			}
			
			this.loginService.storeReturnUrl(state.url);
			return this.router.createUrlTree(["/access/login"]);
		}));
	}
}
