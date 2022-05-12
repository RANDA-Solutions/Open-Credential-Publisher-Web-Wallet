import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { UtilsService } from "@core/services/utils.service";
import { environment } from '@environment/environment';
import { ResetPasswordModel } from "@shared/interfaces/reset-password.interface";
import { ApiResponse } from "@shared/models/apiResponse";
import { catchError } from "rxjs/operators";

@Injectable({
	providedIn: 'root'
})
export class AccessService {

	private baseUrl = environment.publicEndPoint;
	constructor(private http: HttpClient, private utilsService: UtilsService) {}

	forgotPassword(email) {
		const headers = new HttpHeaders();
    const apiUrl = `${environment.apiEndPoint}Public/Account/Password/Forgot/${email}`;
		headers.append('Content-Type', 'application/json');

		return this.http
			.post<any>(encodeURI(apiUrl), null, { headers })
			.pipe();
	}

  resendConfirmation(email) {
		const headers = new HttpHeaders();
    const apiUrl = `${environment.apiEndPoint}Public/Account/Confirmation/Resend/${email}`;
		headers.append('Content-Type', 'application/json');

		return this.http
			.post<any>(encodeURI(apiUrl), null, { headers })
			.pipe();
	}

	resetPassword(input: ResetPasswordModel) {
		const headers = new HttpHeaders();
    const apiUrl = `${environment.apiEndPoint}Public/Account/Password/Reset`;

    return this.http.post<ApiResponse>(apiUrl, input)
    .pipe(
      catchError(err => this.utilsService.handleError(err))
    );
	}

	setPassword(password) {
		const headers = new HttpHeaders();
		const apiUrl = `${environment.apiEndPoint}setpassword/set`;
		const formData = { password };
		headers.append('Content-Type', 'application/json');

		return this.http
			.post<any>(encodeURI(apiUrl), formData, { headers })
			.pipe();
	}

	externalProviders() {
		const apiUrl = `${this.baseUrl}account/login/providers`;
		return this.http.get(apiUrl);
	}

	externalSignIn(name) {
		const apiUrl = `${this.baseUrl}account/login/external/${name}`;
		return this.http
			.get<any>(encodeURI(apiUrl));
	}
}
