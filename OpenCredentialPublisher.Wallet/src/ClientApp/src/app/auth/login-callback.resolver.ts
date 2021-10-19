import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { LoginService } from '@root/app/auth/auth.service';
import { NGXLogger } from 'ngx-logger';
import { Observable } from 'rxjs';
import { mergeMap, take } from 'rxjs/operators';

@Injectable({
	providedIn: 'root'
})
export class LoginCallbackResolver implements Resolve<any> {
	error: boolean;

	constructor(
		private loginService: LoginService,
		private router: Router,
		private logger: NGXLogger
	) {
	}
	
	resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) : Observable<any> {
		var returnUrl = this.loginService.returnUrl;

		return this.loginService.authStateChanged.pipe(take(1), mergeMap(authState => {
			this.logger.info('AuthStateChanged: ', authState);
			
			if (!!authState && authState.isAuthenticated && !authState.isRenewProcess) {
				if (returnUrl) {
					this.loginService.clearReturnUrl();
					this.logger.info("LoginCallbackComponent Return Url: ", returnUrl);
					//returnUrl = decodeURI(returnUrl);
					return this.router.navigateByUrl(returnUrl, { replaceUrl: true });
				}
				else {
					this.logger.info("No return url.");
					return this.router.navigate(["/credentials"], { replaceUrl: true });
				}
			}
			return this.router.navigate(["/unauthorized"]);
		}));
	}
}
