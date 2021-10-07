import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { AppService } from '@core/services/app.service';
import { UtilsService } from '@core/services/utils.service';
import { environment } from '@environment/environment';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { OidcSecurityService } from 'angular-auth-oidc-client';
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
  	 private debug = false;

	constructor(public appService: AppService, private loginService: LoginService, private oidcSecurityService: OidcSecurityService
    , private router: Router,  private util: UtilsService) {
    this.router.events
    .pipe(filter(event => event instanceof NavigationEnd), untilDestroyed(this))
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
			// if (!result.isAuthenticated && result.accessToken) {
			// 	this.loginService.authorize();
			// }
		}, (error) => {
			if (this.debug) {
				console.log(error);
			}
		});
		// this.oidcSecurityService.checkAuthIncludingServer().subscribe(c => {
		// 	if (this.debug) console.log('Auth checked and was: ', c);
		// }, (err) => {
		// 	this.util.handleError(err);
		// });
	}

	ngOnDestroy() {

	}
}
