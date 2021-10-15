import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { AppService } from '@core/services/app.service';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { AuthStateResult, EventTypes, OidcClientNotification, OidcSecurityService, PublicEventsService } from 'angular-auth-oidc-client';
import { filter } from 'rxjs/operators';
import { LoginService } from './auth/auth.service';

@UntilDestroy()
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
	title = 'Open Credential Publisher';
	envName = environment.name;
  	onLoginPage = false;
  	 private debug = true;

	constructor(
		public appService: AppService
		, private loginService: LoginService
		, private readonly eventService: PublicEventsService
		, private oidcSecurityService: OidcSecurityService
    	, private router: Router
		, private util: UtilsService
		) {
    	this.router.events
    		.pipe(
				filter(event => event instanceof NavigationEnd)
				, untilDestroyed(this))
			.subscribe((event: NavigationEnd) => {
				this.onLoginPage = event.url.includes('/access/') || event.url.includes('/access/register');
			});
	}

	ngOnInit() {
		if (this.debug) console.log('AppComponent ngOnInit');
		this.loginService.checkAuthIncludingServer().subscribe((result) => {
			if (this.debug) {
				console.log("Auth Result: ", result);
			}
		}, (error) => {
			if (this.debug) {
				console.log(error);
			}
		});

		this.eventService
			.registerForEvents()
			.pipe(filter((notification) => notification.type === EventTypes.ConfigLoaded))
			.subscribe((config) => {
				// console.log('ConfigLoaded', config);
			});

		this.eventService
			.registerForEvents()
			.pipe(filter((notification) => notification.type === EventTypes.NewAuthenticationResult))
			.subscribe((result: OidcClientNotification<AuthStateResult>) => {
				if (this.debug) {
					console.log(`Auth State (isAuthenticated: ${result.value?.isAuthenticated}) (isRenewProcess: ${result.value?.isRenewProcess})`);
				}
				
				this.loginService.reportAuthState(result.value);
			});
	}

	ngOnDestroy() {

	}
}
