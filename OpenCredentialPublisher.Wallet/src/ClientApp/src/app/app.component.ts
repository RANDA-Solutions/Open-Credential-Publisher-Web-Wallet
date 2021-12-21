import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { AppService } from '@core/services/app.service';
import { environment } from '@environment/environment';
import { Idle } from '@ng-idle/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ApiOkResponse } from '@shared/models/apiOkResponse';
import { FooterSettingsVM } from '@shared/models/footerSettingsVM';
import { AuthStateResult, EventTypes, OidcClientNotification, PublicEventsService } from 'angular-auth-oidc-client';
import { AuthResult } from 'angular-auth-oidc-client/lib/flows/callback-context';
import { BehaviorSubject } from 'rxjs';
import { filter, take } from 'rxjs/operators';
import { LoginService } from './auth/auth.service';
import { TimeoutService } from './services/timeout.service';

@UntilDestroy()
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
	title = 'Open Credential Publisher';
  footerSettingsVM = new FooterSettingsVM();
	envName = environment.name;
	private _onLoginPage = false;
	private _onLoginPageBehavior = new BehaviorSubject(this._onLoginPage);
  	onLoginPage$ = this._onLoginPageBehavior.asObservable();
  	 private debug = true;
	private authStatus: AuthResult;
	constructor(
		public appService: AppService
		, private idle: Idle
		, private loginService: LoginService
		, private readonly eventService: PublicEventsService
    	, private router: Router
		, private timeoutService: TimeoutService
		) {
    	this.router.events
    		.pipe(
				filter(event => event instanceof NavigationEnd)
				, untilDestroyed(this))
			.subscribe((event: NavigationEnd) => {
				this._onLoginPageBehavior.next(event.url.includes('/access/'));
			});
	}

	ngOnInit() {
		this.timeoutService.initialize();

		this.idle.onTimeout.pipe(untilDestroyed(this)).subscribe(e => {
			this.loginService.signOut("Your session timed out, please login again.");
		  });

		if (environment.debug) console.log('AppComponent ngOnInit');
		this.loginService.checkAuthIncludingServer().subscribe((result) => {
				if (environment.debug) console.log("Auth Result: ", result);
		}, (error) => {
			if (environment.debug) {
				console.log(error);
			}
		});

		if (environment.debug) {
			this.eventService
				.registerForEvents()
				.pipe(filter((notification) => notification.type === EventTypes.ConfigLoaded))
				.subscribe((config) => {
					// console.log('ConfigLoaded', config);
				});
			
			this.eventService
				.registerForEvents()
				.subscribe(notification => console.log(notification));
		}
		this.eventService
			.registerForEvents()
			.pipe(filter((notification) => notification.type === EventTypes.NewAuthenticationResult))
			.subscribe((result: OidcClientNotification<AuthStateResult>) => {
				if (environment.debug) {
					console.log(`Auth State (isAuthenticated: ${result.value?.isAuthenticated}) (isRenewProcess: ${result.value?.isRenewProcess})`);
				}

				this.loginService.reportAuthState(result.value);
			});

	}

	ngOnDestroy() {

	}

  getData():any {
    if (environment.debug) console.log('AppComponent getData');
    this.appService.getFooterSettings()
      .pipe(take(1)).subscribe(data => {
        if (data.statusCode == 200) {
          this.footerSettingsVM = (<ApiOkResponse>data).result as FooterSettingsVM;
          if (environment.debug) console.log(`AppComponent gotData ${JSON.stringify(this.footerSettingsVM)}`);
        } else {
          this.footerSettingsVM = new FooterSettingsVM();
          if (environment.debug) console.log(`AppComponent gotData ${data.statusCode}`);
        }
      });
  }
}
