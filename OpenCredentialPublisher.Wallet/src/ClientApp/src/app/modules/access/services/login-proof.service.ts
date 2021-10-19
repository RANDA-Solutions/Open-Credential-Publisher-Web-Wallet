import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environment/environment";
import { LoginProofModel } from "@shared/models/LoginProofModel";
import { LoginProofStatusResponse } from "@shared/models/LoginProofStatusResponse";
import { Observable } from "rxjs";

@Injectable({
	providedIn: 'root'
})
export class LoginProofService {

	constructor(private http: HttpClient) {}

	get() : Observable<LoginProofModel> {
		const apiUrl = `${environment.publicEndPoint}Account/Proof`;
		const headers = new HttpHeaders();
		headers.append('Content-Type', 'application/json');
		return this.http
			.get<LoginProofModel>(encodeURI(apiUrl), { headers });
	}

	status(id: string) : Observable<LoginProofStatusResponse> {
		const apiUrl = `${environment.publicEndPoint}Account/Proof/${id}`;
		const headers = new HttpHeaders();
		headers.append('Content-Type', 'application/json');
		return this.http
			.get<LoginProofStatusResponse>(encodeURI(apiUrl), { headers });
	}
}