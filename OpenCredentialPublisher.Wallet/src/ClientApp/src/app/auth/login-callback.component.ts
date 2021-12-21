import { Component, NgZone, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '@environment/environment';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { LoginService } from '@root/app/auth/auth.service';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { NGXLogger } from 'ngx-logger';

@UntilDestroy()
@Component({
	selector: 'app-login-callback',
	templateUrl: './login-callback.component.html'
})

export class LoginCallbackComponent implements OnInit, OnDestroy {
	error: boolean;

	constructor(
		private loginService: LoginService,
		private oidcService: OidcSecurityService,
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private logger: NGXLogger,
		private ngZone: NgZone
	) {
	}
	ngOnDestroy(): void {
		
	}

	async ngOnInit() {
		var returnUrl = this.loginService.returnUrl;
		this.loginService.authStateChanged.pipe(untilDestroyed(this)).subscribe((authState) => {
			this.logger.info('LoginCallbackComponent: ', authState);
			
			if (!!authState && authState.isAuthenticated && !authState.isRenewProcess) {
				this.ngZone.run((returnUrl) => { 
					if (returnUrl) {
						this.loginService.clearReturnUrl();
						if (environment.debug) console.log(returnUrl);
						if (returnUrl.includes(environment.baseUrl)) {
							returnUrl = returnUrl.replace(environment.baseUrl, '');
						}
						//returnUrl = decodeURI(returnUrl);
						this.router.navigateByUrl(returnUrl);
					}
					else {
						this.logger.info("No return url.");
						this.router.navigate(["/credentials"]);
					}

				}, this, [returnUrl]);
			}
		});
		// this.oidcService.isAuthenticated$.pipe(untilDestroyed(this)).subscribe((authenticationResult) => {
			
		// 	if (authenticationResult.isAuthenticated) {
		// 		if (returnUrl) {
		// 			this.loginService.clearReturnUrl();
		// 			this.logger.info("LoginCallbackComponent Return Url: ", returnUrl);
		// 			//returnUrl = decodeURI(returnUrl);
		// 			this.router.navigateByUrl(returnUrl);
		// 		}
		// 		else {
		// 			this.logger.info("No return url.");

		// 			this.router.navigate(["/credentials"]);
		// 		}
		// 	}
		// });
	}
}
