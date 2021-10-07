import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environment/environment";
import { CreateProofRequest } from "@shared/interfaces/create-proof-request.interface";
import { ApiResponse } from "@shared/models/apiResponse";
import { ProofInvitation, ProofRequest } from "@shared/models/proofInvitation";
import { ProofResponse } from "@shared/models/proofResponse";
import { Observable } from "rxjs";
import { catchError, tap } from "rxjs/operators";
import { UtilsService } from "./utils.service";

@Injectable({
	providedIn: 'root'
})
export class ProofService {
	constructor(private http: HttpClient, private utilsService: UtilsService) { }

	createProofRequest(credentialSchemaId: number, notificationAddress: string, name: string): Observable<ApiResponse> {
		let urlApi = `${environment.apiEndPoint}Proof/Request`;
		let request: CreateProofRequest = {
			credentialSchemaId,
			notificationAddress, 
			name
		};

		return this.http.post<ApiResponse>(urlApi, request).pipe(
			catchError(err => this.utilsService.handleError(err))
		  );
	}

	getPageModel() {
		let urlApi = `${environment.apiEndPoint}Proof`;
		return this.http.get<ApiResponse>(urlApi).pipe(
			catchError(err => this.utilsService.handleError(err))
		  );
	}

	getProof(id: string) {
		let urlApi = `${environment.apiEndPoint}Proof/${id}`;
		return this.http.get<ProofResponse>(urlApi).pipe((tap((data) => {
			console.log(data);
		})));
	}

	getProofStatus(id: string) {
		let urlApi = `${environment.apiEndPoint}Proof/status/${id}`;
		return this.http.get(urlApi, { responseType: 'text'});
	}

	getInvitation(id: string) {
		let urlApi = `${environment.apiEndPoint}Proof/invitation/${id}`;
		return this.http.get<ProofInvitation>(urlApi);
	}

	getProofInformation(id: string) {
		let urlApi = `${environment.apiEndPoint}Proof/request/${id}`;
		return this.http.get<ProofRequest>(urlApi);
	}


}