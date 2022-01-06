import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { environment } from '@environment/environment';
import { LoginService } from '@root/app/auth/login.service';
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
			if (environment.debug) console.log("LoginCallbackResolver: ", authState);
			if (!!authState && authState.isAuthenticated && !authState.isRenewProcess) {
				if (returnUrl) {
					this.loginService.clearReturnUrl();
					if (environment.debug) console.log(returnUrl);
					if (returnUrl.includes(environment.baseUrl)) {
						returnUrl = returnUrl.replace(environment.baseUrl, '');
						if (environment.debug) console.log(returnUrl);
					}
					return this.router.navigateByUrl(returnUrl, { replaceUrl: true });
				}
				else {
					return this.router.navigate(["/credentials"], { replaceUrl: true });
				}
			}
			return this.router.navigate(["/unauthorized"]);
		}));
	}
}
