import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { UtilsService } from "@core/services/utils.service";
import { environment } from '@environment/environment';

@Injectable({
	providedIn: 'root'
})
export class CodeService {

	private baseUrl = environment.publicEndPoint;
	constructor(private http: HttpClient, private utilsService: UtilsService) {}

	sendCode(email) {
		const headers = new HttpHeaders();
    const apiUrl = `${environment.publicEndPoint}Account/Code`;
		headers.append('Content-Type', 'application/json');

		return this.http
			.post<any>(encodeURI(apiUrl), { email }, { headers })
			.pipe();
	}

  verifyCode() {
    const headers = new HttpHeaders();
    const apiUrl = `${environment.publicEndPoint}Account/Code/Verify`;
    headers.append('Content-Type', 'application/json');
    return this.http.post<any>(apiUrl, null, { headers });
  }

  claimCode(code: string) {
    const headers = new HttpHeaders();
    const apiUrl = `${environment.publicEndPoint}Account/Code/${code}`;
    headers.append('Content-Type', 'application/json');
    return this.http.get<any>(apiUrl, { headers });
  }

  claimExternalCode(code: string) {
    const headers = new HttpHeaders();
    const apiUrl = `${environment.publicEndPoint}Account/Code/Claim/${code}`;
    headers.append('Content-Type', 'application/json');
    return this.http.get<any>(apiUrl, { headers });
  }
}
