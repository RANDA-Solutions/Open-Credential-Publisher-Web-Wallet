import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '@environment/environment';
import { AuthenticatedResult, LoginResponse, OidcSecurityService } from 'angular-auth-oidc-client';
import { AuthStateResult } from 'angular-auth-oidc-client/lib/auth-state/auth-state';
import { Observable, of, ReplaySubject, throwError } from 'rxjs';
import { catchError, first, switchMap, tap } from 'rxjs/operators';
import { SecureRoutesService } from '../services/secureRoutes.service';

@Injectable({ providedIn: 'root' })
export class LoginService {
  private debug = false;
  authStateChanged: EventEmitter<AuthStateResult> = new EventEmitter<AuthStateResult>();
  private checkAuthCompleted$ = new ReplaySubject(1);

	constructor(private oidcSecurityService: OidcSecurityService
		, private httpClient: HttpClient
		, private secureRoutesService: SecureRoutesService
		, private router: Router) {}

	returnUrlKey = "originalReturnUrl";

	get isLoggedIn() {
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

	public get isAuthenticated$(): Observable<boolean | AuthenticatedResult> {
        return this.checkAuthCompleted$.pipe(
            first(),
            switchMap((_) => { 
				console.log("inside switchMap");
				return this.oidcSecurityService.isAuthenticated$; 
			})
        );
    }

    // public checkAuth(): Observable<boolean | LoginResponse> {
    //     return this.oidcSecurityService.checkAuth().pipe(tap((_) => this.checkAuthCompleted$.next()));
    // }

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
		// if (!authState.isAuthenticated) {
		// 	if (environment.debug) {
		// 		console.log("No auth: ", this.router.url);
		// 	}
		// 	if (!this.secureRoutesService.isInsecure(this.router.url)) {
		// 		this.oidcSecurityService.authorize(environment.configId);
		// 	}
		// }
	}


	signOut() {
		let url = `${environment.apiEndPoint}logout`;
		this.httpClient.post(url, null).subscribe((_) => this.router.navigate(["/access/logout"], { replaceUrl: true }));
	}
}

