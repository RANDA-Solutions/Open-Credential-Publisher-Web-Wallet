import { EventEmitter, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '@environment/environment';
import { AuthenticatedResult, LoginResponse, OidcSecurityService } from 'angular-auth-oidc-client';
import { AuthStateResult } from 'angular-auth-oidc-client/lib/auth-state/auth-state';
import { Observable, of, ReplaySubject } from 'rxjs';
import { first, switchMap, tap } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class LoginService {
  private debug = false;
  authStateChanged: EventEmitter<AuthStateResult> = new EventEmitter<AuthStateResult>();
  private checkAuthCompleted$ = new ReplaySubject(1);

	constructor(private oidcSecurityService: OidcSecurityService, private router: Router) {}

	returnUrlKey = "originalReturnUrl";

	get isLoggedIn() {
		return this.oidcSecurityService.isAuthenticated$;
	}

	get token() {
		return this.oidcSecurityService.getAccessToken();
	}

	get userData() {
		return this.oidcSecurityService.userData$;
	}

	get config() {
		return this.oidcSecurityService.getConfiguration();
	}

	get stsCallback$() {
		return this.oidcSecurityService.stsCallback$;
	}

	public get isAuthenticated$(): Observable<boolean | AuthenticatedResult> {
        return this.checkAuthCompleted$.pipe(
            first(),
            switchMap((_) => this.oidcSecurityService.isAuthenticated$)
        );
    }

    public checkAuth(): Observable<boolean | LoginResponse> {
        return this.oidcSecurityService.checkAuth().pipe(tap((_) => this.checkAuthCompleted$.next()));
    }

	public checkAuthIncludingServer(): Observable<LoginResponse> {
		return this.oidcSecurityService.checkAuthIncludingServer(environment.configId).pipe(tap((_) => this.checkAuthCompleted$.next()));
	}

	public authorize() {
		return this.oidcSecurityService.getRefreshToken();
	}

	doLogin() {
    if (this.debug) console.log(`OAuthService doLogin()`);
		return of(this.oidcSecurityService.authorize());
	}

	storeReturnUrl(returnUrl) {
		localStorage.setItem(this.returnUrlKey, returnUrl);
	}

	clearReturnUrl() {
		localStorage.removeItem(this.returnUrlKey);
	}



	get returnUrl() {
		return localStorage.getItem(this.returnUrlKey);
	}

	reportAuthState(authState: AuthStateResult)
	{
		this.authStateChanged.emit(authState);
	}


	signOut() {
		this.oidcSecurityService.logoffLocal();
    //TODO clean up ui jump around this causes...
		this.router.navigate(["/"]);
    // const redirectUrl = `${window.location.origin}/credentials`;
    // window.location.replace(redirectUrl);
	}
}
