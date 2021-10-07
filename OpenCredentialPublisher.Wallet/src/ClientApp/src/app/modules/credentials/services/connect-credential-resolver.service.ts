import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from "@angular/router";
import { ConnectCredentialModel } from "@shared/interfaces/connect-credential.interface";
import { Observable } from "rxjs";

@Injectable({
	providedIn: 'root'
  })
  export class ConnectCredentialResolver implements Resolve<ConnectCredentialModel> {
	  constructor(private router: Router) {}

	  resolve(
		route: ActivatedRouteSnapshot,
		state: RouterStateSnapshot
	  ): Observable<ConnectCredentialModel>|Promise<ConnectCredentialModel>|any {
		 let model: ConnectCredentialModel = {
			 endpoint: route.queryParams.endpoint ?? route.queryParams.Endpoint,
			 scope: route.queryParams.scope ?? route.queryParams.Scope,
			 method: route.queryParams.method ?? route.queryParams.Method,
			 issuer: route.queryParams.issuer ?? route.queryParams.Issuer,
			 payload: route.queryParams.payload ?? route.queryParams.Payload
		 };
		 
		 console.log(model);
		 return model;
	  }
  }