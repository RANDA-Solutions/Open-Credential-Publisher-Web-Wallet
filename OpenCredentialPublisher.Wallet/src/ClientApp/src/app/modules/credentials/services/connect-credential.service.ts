import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environment/environment";
import { ConnectCredentialModel } from "@shared/interfaces/connect-credential.interface";

@Injectable({
	providedIn: "root"
})
export class ConnectCredentialService {
	constructor(private http: HttpClient) {}

	connectPost(model:ConnectCredentialModel) {
		const endpoint = `${environment.apiEndPoint}connect`;
		return this.http.post(endpoint, model);
	}
}