import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '@environment/environment';
import { of, ReplaySubject } from 'rxjs';
import { AuthService } from './auth-client.service';

@Injectable({
	providedIn: 'root'
})
export class LoginService {
  private debug = true;

  private checkAuthCompleted$ = new ReplaySubject(1);

	constructor(private authService: AuthService
		, private httpClient: HttpClient
		, private router: Router) {}

	returnUrlKey = "originalReturnUrl";

	get isLoggedIn() {
		return this.authService.isLoggedIn();
	}

	get token() {
		return this.authService.getAccessToken();
	}

	get userData() {
		return of(this.authService.getClaims());
	}

	get config() {
		return {};
		//return this.oidcSecurityService.getConfiguration(environment.configId);
	}

	get stsCallback$() {
		return of("");
	}

	public refreshToken() {
		return of();
	}

	public refreshSession() {
		return of()
	}

	completeLogin() {
		
    	if (this.debug) console.log(`OAuthService doLogin()`);
		return this.authService.checkLogin();
	}

	storeReturnUrl(returnUrl) {
		if (this.debug) console.log(`Storing return url: ${returnUrl}`);
		localStorage.setItem(this.returnUrlKey, returnUrl);
	}

	clearReturnUrl() {
		if (this.debug) console.log(`Clearing return url: ${this.returnUrl}`);
		localStorage.removeItem(this.returnUrlKey);
	}

	get returnUrl() {
		return localStorage.getItem(this.returnUrlKey);
	}



	signOut(infoMessage?: string) {
		let url = `${environment.apiEndPoint}logout`;
		this.httpClient.post(url, null).subscribe((_) => this.goToLogout(infoMessage), (_) => this.goToLogout(infoMessage));
	}

	private goToLogout(infoMessage?: string) {
		this.router.navigate(["/access/logout"], { queryParams: { infoMessage: infoMessage }, replaceUrl: true })
	}
}
