import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environment/environment";
import { EmailCredentialStatusResponse } from "@shared/models/EmailCredentialStatusResponse";
import { EmailVerificationModelInput } from "@shared/models/EmailVerificationModelInput";
import { EmailVerificationResponse } from "@shared/models/EmailVerificationResponse";
import { Observable } from "rxjs";

@Injectable({
	providedIn: 'root'
})
export class EmailVerificationService {

	constructor(private http: HttpClient) {}

	beginVerification(model: EmailVerificationModelInput) {
		const apiUrl = `${environment.publicEndPoint}Account/Verification`;
		return this.http
			.post<any>(encodeURI(apiUrl), model);
	}

	verify(key: string) : Observable<EmailVerificationResponse> {
		const apiUrl = `${environment.publicEndPoint}Account/Verification/${key}`;
		const headers = new HttpHeaders();
		headers.append('Content-Type', 'application/json');
		return this.http
			.get<EmailVerificationResponse>(encodeURI(apiUrl), { headers });
	}

	status(key: string) : Observable<EmailCredentialStatusResponse> {
		const apiUrl = `${environment.publicEndPoint}Account/Verification/${key}/status`;
		const headers = new HttpHeaders();
		headers.append('Content-Type', 'application/json');
		return this.http
			.get<EmailCredentialStatusResponse>(encodeURI(apiUrl), { headers });
	}
}
