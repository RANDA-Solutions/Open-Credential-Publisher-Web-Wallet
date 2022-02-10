import { Component, OnDestroy, OnInit } from '@angular/core';
import { Event as NavigationEvent, NavigationStart, Router } from '@angular/router';
import { AppService } from '@core/services/app.service';
import { environment } from '@environment/environment';
import { Idle } from '@ng-idle/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { AuthService } from './auth/auth-client.service';
import { LoginService } from './auth/login.service';
import { TimeoutService } from './services/timeout.service';

@UntilDestroy()
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
	title = 'Open Credential Publisher';
  
	envName = environment.name;


	private currentUrl: string;
	private navigationEvents$;
	 private debug = environment.debug;
	constructor(
		public appService: AppService
		, private idle: Idle
		, private loginService: LoginService
		, private timeoutService: TimeoutService
		, private authService: AuthService
		, private router: Router
		) {
			this.navigationEvents$ = this.router.events
				.subscribe(
			  		(event: NavigationEvent) => {
						if(event instanceof NavigationStart) {
							this.currentUrl = event.url;
							if (this.debug) console.log(this.currentUrl);
						}
				});
	}

	ngOnInit() {
		if (this.debug)
			console.log("Environment: ", environment);
		this.timeoutService.initialize();

		this.idle.onTimeout.pipe(untilDestroyed(this)).subscribe(e => {
			this.loginService.signOut("Your session timed out, please login again.");
		  });

		this.authService.clearStaleStorage();
		
		this.authService.silentRenewError.pipe(untilDestroyed(this)).subscribe(ev => {
			if (this.debug) {
				console.log(`Current url is ${this.currentUrl} and silent renew error is ${ev}`);
			}
		})
		this.authService.checkLogin();
		this.authService.signIn();
		
		// if (environment.debug) console.log('AppComponent ngOnInit');
		// this.loginService.checkAuthIncludingServer().subscribe((result) => {
		// 		if (environment.debug) console.log("Auth Result: ", result);
		// }, (error) => {
		// 	if (environment.debug) {
		// 		console.log(error);
		// 	}
		// });

		// if (environment.debug) {
		// 	this.eventService
		// 		.registerForEvents()
		// 		.pipe(filter((notification) => notification.type === EventTypes.ConfigLoaded))
		// 		.subscribe((config) => {
		// 			// console.log('ConfigLoaded', config);
		// 		});
			
		// 	this.eventService
		// 		.registerForEvents()
		// 		.subscribe(notification => console.log(notification));
		// }
		// this.eventService
		// 	.registerForEvents()
		// 	.pipe(filter((notification) => notification.type === EventTypes.NewAuthenticationResult))
		// 	.subscribe((result: OidcClientNotification<AuthStateResult>) => {
		// 		if (environment.debug) {
		// 			console.log(`Auth State (isAuthenticated: ${result.value?.isAuthenticated}) (isRenewProcess: ${result.value?.isRenewProcess})`);
		// 		}

		// 		this.loginService.reportAuthState(result.value);
		// 	});

	}

	ngOnDestroy() {
		this.navigationEvents$.unsubscribe();
	}

  getData():any {
    if (environment.debug) console.log('AppComponent getData');
    
  }
}
