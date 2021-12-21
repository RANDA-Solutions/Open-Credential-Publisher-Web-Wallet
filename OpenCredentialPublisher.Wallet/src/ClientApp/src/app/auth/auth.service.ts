import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '@environment/environment';
import { AuthenticatedResult, LoginResponse, OidcSecurityService } from 'angular-auth-oidc-client';
import { AuthStateResult } from 'angular-auth-oidc-client/lib/auth-state/auth-state';
import { Observable, of, ReplaySubject, throwError } from 'rxjs';
import { catchError, first, switchMap, tap } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class LoginService {
  private debug = true;
  authStateChanged: EventEmitter<AuthStateResult> = new EventEmitter<AuthStateResult>();
  private checkAuthCompleted$ = new ReplaySubject(1);

	constructor(private oidcSecurityService: OidcSecurityService
		, private httpClient: HttpClient
		, private router: Router) {}

	returnUrlKey = "originalReturnUrl";

	get isLoggedIn(): Observable<AuthenticatedResult> {
		return this.oidcSecurityService.isAuthenticated$;
	}

	get token() {
		return this.oidcSecurityService.getAccessToken(environment.configId);
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

	public get isAuthenticated$(): Observable<AuthenticatedResult> {
        return this.checkAuthCompleted$.pipe(
            first(),
            switchMap((_) => {
				console.log("inside switchMap");
				return this.oidcSecurityService.isAuthenticated$;
			})
        );
    }

    public checkAuth(): Observable<LoginResponse> {
        return this.oidcSecurityService.checkAuth(null, environment.configId).pipe(tap((_) => this.checkAuthCompleted$.next()));
    }

	public checkAuthIncludingServer(): Observable<LoginResponse> {
		return this.oidcSecurityService.checkAuthIncludingServer(environment.configId)
			.pipe(
				catchError((error) => {
					this.checkAuthCompleted$.next();
					return throwError(error);
				})
				, tap((_) => {
					this.checkAuthCompleted$.next();
		}));
	}

	public refreshToken() {
		return this.oidcSecurityService.getRefreshToken(environment.configId);
	}

	public refreshSession() {
		return this.oidcSecurityService.forceRefreshSession(null, environment.configId);
	}

	doLogin() {
		
    if (this.debug) console.log(`OAuthService doLogin()`);
		return of(this.oidcSecurityService.authorize(environment.configId));
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


	signOut(infoMessage?: string) {
		let url = `${environment.apiEndPoint}logout`;
		this.httpClient.post(url, null).subscribe((_) => this.goToLogout(infoMessage), (_) => this.goToLogout(infoMessage));
	}

	private goToLogout(infoMessage?: string) {
		this.router.navigate(["/access/logout"], { queryParams: { infoMessage: infoMessage }, replaceUrl: true })
	}
}

