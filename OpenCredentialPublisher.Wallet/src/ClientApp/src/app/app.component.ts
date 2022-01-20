import { Component, OnDestroy, OnInit } from '@angular/core';
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

	 private debug = environment.debug;
	constructor(
		public appService: AppService
		, private idle: Idle
		, private loginService: LoginService
		, private timeoutService: TimeoutService
		, private authService: AuthService
		) {
    	
	}

	ngOnInit() {
		if (this.debug)
			console.log("Environment: ", environment);
		this.timeoutService.initialize();

		this.idle.onTimeout.pipe(untilDestroyed(this)).subscribe(e => {
			this.loginService.signOut("Your session timed out, please login again.");
		  });

		  this.authService.checkLogin();
		  this.authService.clearStaleStorage();

		
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

	}

  getData():any {
    if (environment.debug) console.log('AppComponent getData');
    
  }
}
