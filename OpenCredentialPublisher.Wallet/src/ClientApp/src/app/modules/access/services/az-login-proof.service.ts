import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environment/environment";
import { ApiResponse } from "@shared/models/apiResponse";
import { AzLoginProofGetResponseModel } from "@shared/models/azLoginProofGetResponseModel";
import { EmailVerificationModelInput } from "@shared/models/EmailVerificationModelInput";
import { EmailVerificationResponse } from "@shared/models/EmailVerificationResponse";
import { LoginProofStatusResponse } from "@shared/models/LoginProofStatusResponse";
import { Observable } from "rxjs";

@Injectable({
	providedIn: 'root'
})
export class AzLoginProofService {

	constructor(private http: HttpClient) {}

	verifyEmail(key: string) {
		const apiUrl = `${environment.apiEndPoint}AzADProof/VerifyEmail/${key}`;
		return this.http
			.post<ApiResponse>(encodeURI(apiUrl), null);
	}

	get() : Observable<ApiResponse> {
		const apiUrl = `${environment.apiEndPoint}AzADProof/GetProof`;
		const headers = new HttpHeaders();
		headers.append('Content-Type', 'application/json');
		return this.http
			.get<ApiResponse>(encodeURI(apiUrl), { headers });
	}

	status(requestId: string) : Observable<ApiResponse> {
		const apiUrl = `${environment.apiEndPoint}AzADProof/ProofStatus/${requestId}`;
		const headers = new HttpHeaders();
		headers.append('Content-Type', 'application/json');
		return this.http
			.get<ApiResponse>(encodeURI(apiUrl), { headers });
	}
	issueStatus(requestId: string) : Observable<ApiResponse> {
		const apiUrl = `${environment.apiEndPoint}AzADProof/IssueStatus/${requestId}`;
		const headers = new HttpHeaders();
		headers.append('Content-Type', 'application/json');
		return this.http
			.get<ApiResponse>(encodeURI(apiUrl), { headers });
	}
}
