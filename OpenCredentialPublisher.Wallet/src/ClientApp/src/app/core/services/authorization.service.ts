import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
// import { UserRegistration } from '../interfaces/user.registration.interface';
import { Router } from '@angular/router';
import { ApiResponse } from '@shared/models/apiResponse';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { BaseService } from './base.service';
import { ToastService } from './toast.service';


@Injectable({
  providedIn: 'root'
})

export class AuthorizationService extends BaseService {

	baseUrl = '';

	private _loggedIn: boolean;
	private _loggedInSource = new BehaviorSubject<boolean>(false);
	public loggedIn$ = this._loggedInSource.asObservable();

	private _authNavStatusSource = new BehaviorSubject<boolean>(false);
	authNavStatus$ = this._authNavStatusSource.asObservable();

	constructor(private http: HttpClient, private router: Router, private toastService: ToastService) {
		super();
		this._authNavStatusSource.next(this._loggedIn);
		this.baseUrl = environment.publicEndPoint;
	}

	login(userName, password, returnUrl) {
		const headers = new HttpHeaders();
		const apiUrl = `${this.baseUrl}account/login`;
		const formData = { email: userName, password, returnUrl };
		headers.append('Content-Type', 'application/json');
		return this.http.post<ApiResponse>(encodeURI(apiUrl), formData, { headers });
	}

	getUserName() {
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
}
