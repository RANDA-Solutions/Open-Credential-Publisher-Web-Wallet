import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
// import { UserRegistration } from '../interfaces/user.registration.interface';
import { Router } from '@angular/router';
import { ApiResponse } from '@shared/models/apiResponse';
import { BehaviorSubject } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { BaseService } from './base.service';
import { ToastService } from './toast.service';
import { UtilsService } from './utils.service';


@Injectable({
  providedIn: 'root'
})

export class AuthorizationService extends BaseService {

	baseUrl = '';

	private _loggedIn: boolean;
	private _loggedInSource = new BehaviorSubject<boolean>(false);
	public loggedIn$ = this._loggedInSource.asObservable();


	// Observable navItem source
	private _authNavStatusSource = new BehaviorSubject<boolean>(false);
	// Observable navItem stream
	authNavStatus$ = this._authNavStatusSource.asObservable();

	constructor(private http: HttpClient, private router: Router, private toastService: ToastService) {
		super();
		// ?? not sure if this the best way to broadcast the status but seems to resolve issue on page refresh where auth status is lost in
		// header component resulting in authed user nav links disappearing despite the fact user is still logged in
		this._authNavStatusSource.next(this._loggedIn);
		this.baseUrl = environment.publicEndPoint;
	}

	// register(email: string, password: string, firstName: string, lastName: string,location: string): Observable<UserRegistration> {
	// let body = JSON.stringify({ email, password, firstName, lastName,location });
	// let headers = new Headers({ 'Content-Type': 'application/json' });
	// let options = new RequestOptions({ headers: headers });

	//  return this.http.post<UserRegistration>(this.baseUrl + "/accounts", body, options).pipe(
	//    catchError(this.utilsService.handleError('getCourse',
	//      new UserRegistration()))
	//  );
	// }

	login(userName, password, returnUrl) {
		const headers = new HttpHeaders();
		const apiUrl = `${this.baseUrl}account/login`;
		const formData = { email: userName, password, returnUrl };
		headers.append('Content-Type', 'application/json');
		//  return this.http.post<any>(encodeURI(apiUrl), formData, { headers })
		//    .pipe(
		//      catchError(this.handleError)
		//    );
		// }

		return this.http.post<ApiResponse>(encodeURI(apiUrl), formData, { headers })
    .pipe(
      // catchError(err => this.utilService.handleError(err) would cause circular error with UtilsService
      // )
    );

	}
	getUserName() {
		console.log('getname');
		let name = '';
		const js = localStorage.getItem('oidc.user:teacherwallet');
		if (js !== null) {
			const profile = JSON.parse(js);
			if (profile !== null) {
				name = profile.preferredName ? profile.preferredName : profile.name;
			} else {
				name = 'null profile';
			}
		} else {
			name = 'null oidcuser';
		}

		return name;
	}
	getAccessToken() {
		const token = sessionStorage.getItem('ocp-wallet-client_authnResult');
		return (token === null) ? '' : token;
	}
	logout() {
		const apiUrl = `${this.baseUrl}account/logout`;

		sessionStorage.clear();
		return this.http.post(apiUrl, null);
	}

	isLoggedIn() {
		this._loggedIn = !!sessionStorage.getItem('ocp-wallet-client_authnResult');
		this._loggedInSource.next(this._loggedIn);
		return this._loggedIn;
	}
	refresh() {
	}
	// facebookLogin(accessToken:string) {
	//  let headers = new Headers();
	//  headers.append('Content-Type', 'application/json');
	//  let body = JSON.stringify({ accessToken });
	//  return this.http
	//    .post<any>(
	//    this.baseUrl + '/externalauth/facebook', body, { headers })
	//    .map(res => res.json())
	//    .map(res => {
	//      localStorage.setItem('auth_token', res.auth_token);
	//      this.loggedIn = true;
	//      this._authNavStatusSource.next(true);
	//      return true;
	//    })
	//    .catch(this.handleError);
	// }
}
